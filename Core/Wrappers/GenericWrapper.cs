﻿namespace Void.Wrappers
{
    /// <summary>
    /// The most simple imaginable implementation of <see cref="IWrapper{T}"/>
    /// </summary>
    public class GenericWrapper<T> : IWrapper<T>
    {
        /// <summary>Implements: <see cref="IWrapper{T}.Wrapped"/></summary>
        public T Wrapped { get; private set; }

        /// <summary>Constructs an instance where <see cref="Wrapped"/> is <paramref name="wrapped"/> </summary>
        public GenericWrapper(T wrapped)
        {
            Wrapped = wrapped;
        }
    }
}