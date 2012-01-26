﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CrunchApiExplorer.Framework.Extensions
{
    public static class SymbolExtensions
    {
        /// <summary>
        /// Gets the PropertyInfo for the last property access in an expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T>> expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("expression must consist of a property access");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new InvalidOperationException("expression must consist of a property access");
            }

            return propertyInfo;
        }

        public static string GetPropertyName<T>(this Expression<Func<T>> expression)
        {
            return GetPropertyInfo(expression).Name;
        }
    }
}
