using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Ressa.Testing.Extensions
{
    public static class AssertionExtensions
    {
        [DebuggerHidden]
        public static void ShouldBeTrue(this bool actual, string message = null)
        {
            Assert.IsTrue(actual, message);
        }

        [DebuggerHidden]
        public static void ShouldBeFalse(this bool actual, string message = null)
        {
            Assert.IsFalse(actual, message);
        }

        [DebuggerHidden]
        public static void ShouldEqual(this string actual, string expected, StringComparison comparison = StringComparison.InvariantCulture, string message = null)
        {
            Assert.IsTrue(expected.Equals(actual, comparison), message ?? string.Format("Expected \"{0}\" Actual \"{1}\"", expected, actual));
        }

        [DebuggerHidden]
        public static void ShouldEqual(this object actual, object expected, string message = null)
        {
            Assert.AreEqual(expected, actual, message);
        }
    }
}
