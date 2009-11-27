// <copyright file="SimpleIndexingTest.cs" company="Microsoft">Copyright © Microsoft 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Void.Linq;
using System.Collections.Generic;

namespace Void.Linq
{
    [PexClass(typeof(SimpleIndexing))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SimpleIndexingTest
    {
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Eighth<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Eighth<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Eighth(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Fifth<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Fifth<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Fifth(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Fourth<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Fourth<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Fourth(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Ninth<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Ninth<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Ninth(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Second<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Second<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Second(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Seventh<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Seventh<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Seventh(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Sixth<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Sixth<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Sixth(IEnumerable`1<!!0>)
        }
        [PexGenericArguments(typeof(int))]
        [PexMethod, PexAllowedException(typeof(ArgumentOutOfRangeException))]
        public T Third<T>(IEnumerable<T> me)
        {
            T result = SimpleIndexing.Third<T>(me);
            return result;
            // TODO: add assertions to method SimpleIndexingTest.Third(IEnumerable`1<!!0>)
        }
    }
}
