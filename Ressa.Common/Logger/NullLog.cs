namespace Ressa.Common.Logger
{
    public sealed class NullLog : BaseLog
    {
        public override void Write(string value)
        {
        }
    }
}
