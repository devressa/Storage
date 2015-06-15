using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using NUnit.Framework;
using Ressa.Storage.Tests.AmazonS3;

namespace Ressa.Storage.Tests.Integration.AmazonS3
{
    class When_upload_file_from_stream : AmazonS3FacadeFixture
    {
        [Test]
        public void Should_upload_file()
        {
            try
            {
                using (var s = new MemoryStream(new byte[] { 1 }))
                {
                    Facade.Upload(Bucket, FileName, s);
                }

                Assert.IsTrue(Facade.Exists(Bucket, FileName));
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_upload_file_if_file_exist()
        {
            try
            {
                using (var s = new MemoryStream(new byte[] { 1, 2, 3 }))
                {
                    Facade.Upload(Bucket, FileName, s);
                }

                using (var s = new MemoryStream(new byte[] { 1 }))
                {
                    Facade.Upload(Bucket, FileName, s);
                }

                Assert.IsTrue(Facade.Exists(Bucket, FileName));
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_upload_file_greater_then_70mb()
        {
            var bytes = BigData.Mb100;

            try
            {
                using (var s = new MemoryStream(bytes))
                {
                    Facade.Upload(Bucket, FileName, s);
                }

                Assert.IsTrue(Facade.Exists(Bucket, FileName));
                using (var w = new WebClient())
                {
                    Assert.AreEqual(bytes, w.DownloadData(Facade.GetPreviewUrl(Bucket, FileName)));
                }
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_upload_different_files_in_parallel()
        {
            var fn1 = FileName + "1";
            var fn2 = FileName + "2";
            var bytes1 = BigData.Mb40;
            var bytes2 = BigData.Mb10;

            try
            {
                Parallel.Invoke(() => Upload(bytes1, fn1), () => Upload(bytes2, fn2));

                Assert.IsTrue(Facade.Exists(Bucket, fn1));
                using (var w = new WebClient())
                {
                    Assert.AreEqual(bytes1, w.DownloadData(Facade.GetPreviewUrl(Bucket, fn1)));
                }
                Assert.IsTrue(Facade.Exists(Bucket, fn2));
                using (var w = new WebClient())
                {
                    Assert.AreEqual(bytes2, w.DownloadData(Facade.GetPreviewUrl(Bucket, fn2)));
                }
            }
            finally
            {
                Facade.Delete(Bucket, fn1);
                Facade.Delete(Bucket, fn2);
            }
        }

        [Test]
        public void Should_upload_file_greater_then_70mb_and_remove_it_if_error()
        {
            var bytes = BigData.Mb100;

            try
            {
                var now = DateTime.Now;
                var token = new CancellationTokenSource();
                var thread = new Thread(() => DeleteTempFiles(now, token.Token));
                thread.Start();
                using (var s = new MemoryStream(bytes))
                {
                    Assert.Throws<AmazonS3Exception>(() => Facade.Upload(Bucket, FileName, s));
                }
                token.Cancel();
                thread.Join();

                Assert.IsFalse(Facade.Exists(Bucket, FileName));
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_upload_file_and_save_data()
        {
            try
            {
                var now = DateTime.UtcNow;
                using (var s = new MemoryStream(new byte[] { 1 }))
                {
                    Facade.Upload(Bucket, FileName, s);
                }

                var details = Facade.GetFileDetails(Bucket, FileName);
                Assert.AreEqual(Bucket.Name, details.BucketName);
                Assert.AreEqual(FileName, details.Filename);
                Assert.AreEqual(1, details.SizeInBytes);
                Assert.LessOrEqual(Math.Abs((details.LastModified - now).Minutes), AmazonS3BucketBuilder.MaximumTimeDifference);
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_throw_exception_if_bucket_not_exist()
        {
            Assert.Throws<AmazonS3Exception>(() =>
            {
                using (var s = new MemoryStream(new byte[] { 1 }))
                {
                    Facade.Upload(AmazonS3BucketBuilder.NewNotExitBucket(), FileName, s);
                }
            });
        }

        private void Upload(byte[] bytes, string fileName)
        {
            using (var s = new MemoryStream(bytes))
            {
                Facade.Upload(Bucket, fileName, s);
            }
        }

        private static void DeleteTempFiles(DateTime now, CancellationToken token)
        {
            Thread.Sleep(5000);
            while (!token.IsCancellationRequested)
            {
                foreach (var f in new DirectoryInfo(Path.GetTempPath()).GetFiles("tmp*.tmp").Where(f => f.CreationTime >= now))
                {
                    try
                    {
                        f.Delete();
                    }
                    catch { }
                }
                Thread.Sleep(100);
            }
        }
    }
}
