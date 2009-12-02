// <copyright file="ObjectExtensionsTest.cs" company="Microsoft">Copyright � Microsoft 2009</copyright>
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using Void.Linq;

namespace Void.Linq
{
    /// <summary>This class contains parameterized unit tests for ObjectExtensions</summary>
    [PexClass(typeof(ObjectExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ObjectExtensionsTest
    {
        /// <summary>Test stub for Repeat(!!0, Int32)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public IEnumerable<T> Repeat<T>(T me, int times)
        {
            IEnumerable<T> result = ObjectExtensions.Repeat<T>(me, times);
            return result;
            // TODO: add assertions to method ObjectExtensionsTest.Repeat(!!0, Int32)
        }
    }
}