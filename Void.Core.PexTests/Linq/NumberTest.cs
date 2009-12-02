// <copyright file="NumberTest.cs" company="Microsoft">Copyright � Microsoft 2009</copyright>
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using Void.Linq;

namespace Void.Linq
{
    /// <summary>This class contains parameterized unit tests for Number</summary>
    [PexClass(typeof(Number))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class NumberTest
    {
        /// <summary>Test stub for By(Int32, Int32)</summary>
        [PexMethod]
        public Number.IterationSpecification By(int me, int stepsize)
        {
            Number.IterationSpecification result = Number.By(me, stepsize);
            return result;
            // TODO: add assertions to method NumberTest.By(Int32, Int32)
        }

        /// <summary>Test stub for Through(Int32, Int32)</summary>
        [PexMethod]
        public IEnumerable<int> Through(int me, int guard)
        {
            IEnumerable<int> result = Number.Through(me, guard);
            return result;
            // TODO: add assertions to method NumberTest.Through(Int32, Int32)
        }

        /// <summary>Test stub for Through(IterationSpecification, Int32)</summary>
        [PexMethod]
        public IEnumerable<int> Through01(Number.IterationSpecification me, int guard)
        {
            IEnumerable<int> result = Number.Through(me, guard);
            return result;
            // TODO: add assertions to method NumberTest.Through01(IterationSpecification, Int32)
        }

        /// <summary>Test stub for Until(Int32, Int32)</summary>
        [PexMethod]
        public IEnumerable<int> Until(int me, int guard)
        {
            IEnumerable<int> result = Number.Until(me, guard);
            return result;
            // TODO: add assertions to method NumberTest.Until(Int32, Int32)
        }

        /// <summary>Test stub for Until(IterationSpecification, Int32)</summary>
        [PexMethod]
        public IEnumerable<int> Until01(Number.IterationSpecification me, int guard)
        {
            IEnumerable<int> result = Number.Until(me, guard);
            return result;
            // TODO: add assertions to method NumberTest.Until01(IterationSpecification, Int32)
        }
    }
}