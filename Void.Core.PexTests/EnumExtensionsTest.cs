// <copyright file="EnumExtensionsTest.cs" company="Microsoft">Copyright � Microsoft 2009</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using Void;

namespace Void
{
    /// <summary>This class contains parameterized unit tests for EnumExtensions</summary>
    [PexClass(typeof(EnumExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class EnumExtensionsTest
    {
        /// <summary>Test stub for HasFlag(Enum, Enum)</summary>
        [PexMethod]
        public bool HasFlag(Enum value, Enum flag)
        {
            bool result = EnumExtensions.HasFlag(value, flag);
            return result;
            // TODO: add assertions to method EnumExtensionsTest.HasFlag(Enum, Enum)
        }
    }
}