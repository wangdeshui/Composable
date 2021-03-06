﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Composable.System.Reflection
{
    ///<summary>Provides high performance access to object fields and properties.</summary>
    public static class MemberAccessorHelper
    {
        private static readonly IDictionary<Type, Func<Object, Object>[]> TypeFields = new ConcurrentDictionary<Type, Func<Object, Object>[]>();

        private static Func<object, object> BuildFieldGetter(FieldInfo field)
        {
            Contract.Requires(field != null && field.DeclaringType != null);

            var obj = Expression.Parameter(typeof(object), "obj");

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.Field(
                        Expression.Convert(obj, field.DeclaringType),
                        field),
                    typeof(object)),
                obj).Compile();
        }

        ///<summary>Returns the values of all the fields and properties that the supplied object contains.</summary>
        public static IEnumerable<object> GetFieldAndPropertyValues(object o)
        {
            Contract.Requires(o != null);

            return GetFieldsAndPropertyGetters(o.GetType()).Select(getter => getter(o));
        }

        ///<summary>Returns functions that when invoked will return the values of the fields an properties in an instance of the supplied type.</summary>
        public static Func<Object, Object>[] GetFieldsAndPropertyGetters(Type type)
        {
            Contract.Requires(type != null);

            return InnerGetField(type);
        }

        private static Func<object, object>[] InnerGetField(Type type)
        {
            Func<Object, Object>[] fields;
            Contract.Ensures(Contract.Result<Func<Object, Object>[]>() != null);

            if (!TypeFields.TryGetValue(type, out fields))
            {
                var newFields = new List<Func<Object, object>>();
                if (!type.IsPrimitive)
                {
                    newFields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Select(BuildFieldGetter));

                    var baseType = type.BaseType;
                    if (baseType != null && baseType != typeof (object))
                    {
                        newFields.AddRange(GetFieldsAndPropertyGetters(baseType));
                    }
                }
                TypeFields[type] = fields = newFields.ToArray();
            }            
            return fields;
        }
    }

    ///<summary>Provides high performance access to object fields and properties.</summary>
    public static class MemberAccessorHelper<T>
    {
        // ReSharper disable StaticFieldInGenericType
        private static readonly Func<Object, Object>[] Fields;
        // ReSharper restore StaticFieldInGenericType

        static MemberAccessorHelper()
        {
            Fields = MemberAccessorHelper.GetFieldsAndPropertyGetters(typeof(T));
        }

        ///<summary>Returns functions that when invoked will return the values of the fields an properties in an instance of the supplied type.</summary>
        public static Func<object, object>[] GetFieldsAndProperties(Type type)
        {
            Contract.Requires(type != null);
            if(type == typeof(T))
            {
                return Fields;
            }
            return MemberAccessorHelper.GetFieldsAndPropertyGetters(type);
        }
    }
}