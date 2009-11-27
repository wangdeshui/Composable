// <copyright file="DirectoryExtensionsTest.cs" company="Microsoft">Copyright © Microsoft 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Void.IO;
using System.IO;

namespace Void.IO
{
    [PexClass(typeof(DirectoryExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class DirectoryExtensionsTest
    {
        [PexMethod, PexAllowedException(typeof(ArgumentException))]
        public DirectoryInfo AsDirectory(string path)
        {
            DirectoryInfo result = DirectoryExtensions.AsDirectory(path);
            return result;
            // TODO: add assertions to method DirectoryExtensionsTest.AsDirectory(String)
        }
    }
}
