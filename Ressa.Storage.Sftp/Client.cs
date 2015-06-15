using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Rebex.Net;
using Ressa.Common.Extensions;
using Ressa.Common.Logger;
using Ressa.Storage.Sftp.Dtos;
using Ressa.Storage.Sftp.Exceptions;
using Ressa.Storage.Sftp.Interfaces;
using SftpException = Ressa.Storage.Sftp.Exceptions.SftpException;

namespace Ressa.Storage.Sftp
{
    public class Client : IClient
    {
        private const string UploadCompletedCheckNumber = "UploadCompletedCheckNumber";
        private const string SftpOperationTimeoutSeconds = "SftpOperationTimeoutSeconds";
        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
        private readonly IConfigurationManagerWrapper configuration;
        private readonly ISftpUrlParser parser;
        private Rebex.Net.Sftp sftpClient;
        private DateTime lastReport = DateTime.MinValue;

        public Client(SftpProvider sftpDetails, ISftpUrlParser parser, IConfigurationManagerWrapper configuration)
        {
            this.parser = parser;
            this.configuration = configuration;
            CreateSftpClient(sftpDetails);
        }

        public void Connect()
        {
            Log.Write(string.Format("Creating SFTP client. Host: '{0}', Port: '{1}', Username: '{2}'.",
                                    sftpClient.ServerName, sftpClient.ServerPort, sftpClient.UserName));

            try
            {
                ConnectSftpClient(sftpClient.ServerPort, sftpClient.ServerName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Operation: SFTPConnection, ExceptionMessage: Unable to connect to remote server", ex);
            }
        }

        public bool Exists(string path)
        {
            try
            {
                return sftpClient.FileExists(path) || sftpClient.DirectoryExists(path);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Operation: SFTPDirectoryExists, Directory: {0}, ExceptionMessage: Unable to verify directory existence", path), ex);
            }
        }

        public void CreateDirectory(string path)
        {
            Log.Write("Creating directory: {0}", path);

            try
            {
                sftpClient.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw new SftpException(string.Format("Operation: SFTPCreateDirectory, Directory: {0}, ExceptionMessage: Unable to create directory", path), ex);
            }
        }

        public SftpFileDetails CopyFile(string srcFileName, string dstFileName)
        {
            Log.Write("Prepare copying of file '{0}' to SFTP '{1}'.", srcFileName, dstFileName);

            try
            {
                using (var stream = new FileStream(srcFileName, FileMode.Open))
                {
                    var maxCompletionChecks = GetNumberOfCompletionChecks();

                    Log.Write(string.Format("MaxCompletionChecks: '{0}'; OperationTimeout: '{1}'.",
                                             maxCompletionChecks, sftpClient.Timeout));

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var asyncTransfer = sftpClient.PutFileAsync(stream, dstFileName);

                    for (var i = 0; i < maxCompletionChecks; i++)
                    {
                        Log.Write("Copying file", srcFileName, dstFileName, stopwatch);
                        
                        if (asyncTransfer.IsCompleted)
                        {
                            Log.Write("Copy Complete", srcFileName, dstFileName, stopwatch);
                            break;
                        }

                        if (asyncTransfer.IsFaulted)
                        {
                            Log.Write("Transfer is in faulted state as an unhandled exception found when putting file", srcFileName, dstFileName, stopwatch);
                            break;
                        }

                        Thread.Sleep(1000);
                    }

                    stopwatch.Stop();

                    Log.Write("Finished copying file", srcFileName, dstFileName, stopwatch);

                    VerifyUploadFileSize(dstFileName, stream.Length, stopwatch);

                    Log.Write("Successfully completed copying file", srcFileName, dstFileName, stopwatch);

                    return new SftpFileDetails((ulong)sftpClient.GetFileLength(dstFileName), stopwatch.ElapsedMilliseconds);
                }
            }
            catch (SftpTimeoutException exception)
            {
                Log.Write(exception,"");
                throw;
            }
            catch (SftpUploadException exception)
            {
                Log.Write(exception, "");
                throw;
            }
            catch (Exception ex)
            {
                throw new SftpException(string.Format("Operation: SFTPTransfer. Source File Name: {0}.  Destination File Name: {1}.",
                                                      srcFileName, dstFileName), ex);
            }
        }

        public void Dispose()
        {
            Log.Write("Disposing.");
            if (null == sftpClient)
            {
                Log.Write("Dispose: sftpClient is null.");
                return;
            }

            sftpClient.Disconnect();
            Log.Write("Dispose: Disconnected.");
            sftpClient.Dispose();
            Log.Write("Disposed.");
        }

        private bool GetSftpIncludeFileSizeCheck()
        {
            bool value;
            return !bool.TryParse(configuration.AppSettings["SftpIncludeFileSizeCheck"], out value) || value;
        }

        private void VerifyUploadFileSize(string dstFileName, long expectedSize, Stopwatch stopwatch)
        {
            if (!GetSftpIncludeFileSizeCheck())
            {
                Log.Write("Skip VerifyUploadFileSize.");
                return;
            }

            long actualSize;
            try
            {
                actualSize = sftpClient.GetFileLength(dstFileName);
            }
            catch (Exception ex)
            {
                Log.Write(ex, string.Format("Unable to retrieve the actual size of the file '{0}' from the SFTP destination.", dstFileName), ex);
                throw;
            }

            Log.Write(string.Format("Expected size: '{0}'; Actual size: '{1}'.", expectedSize, actualSize));

            if (actualSize != expectedSize)
            {
                throw new SftpUploadException(dstFileName, expectedSize, actualSize, (ulong)actualSize, stopwatch.ElapsedMilliseconds);
            }
        }

        private void CreateSftpClient(SftpProvider sftpDetails)
        {
            Log.Write(string.Format("Creating SFTP client. SftpUrl: '{0}', SftpUser: '{1}'", sftpDetails.SftpUrl, sftpDetails.SftpUserName));

            sftpClient = new Rebex.Net.Sftp
            {
                Timeout = (int)GetSftpOperationTimeoutSeconds().TotalMilliseconds
            };

            sftpClient.TransferProgress += TransferProgress;

            var port = parser.GetPort(sftpDetails.SftpUrl);
            var host = parser.GetHost(sftpDetails.SftpUrl);

            ConnectSftpClient(port, host);

            if (!sftpDetails.SftpPrivateKey.IsNullEmptyOrWhiteSpace())
            {
                using (var stringReader = new MemoryStream(Encoding.ASCII.GetBytes(sftpDetails.SftpPrivateKey)))
                {
                    sftpClient.Login(sftpDetails.SftpUserName, new SshPrivateKey(stringReader, sftpDetails.SftpPassPhrase));
                }
            }
            else
            {
                sftpClient.Login(sftpDetails.SftpUserName, sftpDetails.SftpPassPhrase);
            }
        }

        private void ConnectSftpClient(int port, string host)
        {
            if (!sftpClient.GetConnectionState().Connected)
            {
                sftpClient.Connect(host, port);
            }
        }

        private void TransferProgress(object sender, SftpTransferProgressEventArgs e)
        {
            if (e.Finished)
            {
                Log.Write("Transfer Complete. {0} bytes transferred.", e.BytesTransferred);
                return;
            }

            if ((DateTime.Now - lastReport).TotalSeconds <= 1)
            {
                return;
            }

            Log.Write("{0} bytes transferred.", e.BytesTransferred);

            lastReport = DateTime.Now;
        }

        //private int GetNumberOfCompletionChecks()
        //{
        //    return GetIntegerAppSettingValue(UploadCompletedCheckNumber, 60);
        //}

        //private TimeSpan GetSftpOperationTimeoutSeconds()
        //{
        //    return TimeSpan.FromSeconds(GetIntegerAppSettingValue(SftpOperationTimeoutSeconds, 30));
        //}

        //private int GetIntegerAppSettingValue(string appSetting, int defaultValue)
        //{
        //    int value;
        //    return int.TryParse(configuration.AppSettings[appSetting], out value) ? value : defaultValue;
        //}
    }
}
