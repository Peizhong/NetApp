using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NetApp.Common.Extensions
{
    public static class LinqExtensions
    {
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var property = objType.GetProperty(name);
            if (property == null)
            {
                throw new ArgumentException(name);
            }
            return property;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        public static IEnumerable<T> OrderByBatch<T>(this IEnumerable<T> query, string expression)
        {
            var typeInfo = typeof(T);
            var firstParam = true;
            var items = expression.Split(',');
            foreach(var item in items)
            {
                var name = item.Trim();
                var m = firstParam ? "OrderBy" : "ThenBy";
                firstParam = false;
                if (item.StartsWith("-"))
                {
                    m += "Descending";
                    name = name.Substring(1);
                }
                var propertyInfo = GetPropertyInfo(typeInfo, name);
                var expr = GetOrderExpression(typeInfo, propertyInfo);
                var method = typeof(Enumerable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeInfo, propertyInfo.PropertyType);
                query = (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
            }
            return query;
        }

        public static IQueryable<T> OrderByBatch<T>(this IQueryable<T> query, string expression)
        {
            var typeInfo = typeof(T);
            var firstParam = true;
            var items = expression.Split(',');
            foreach (var item in items)
            {
                var name = item.Trim();
                var m = firstParam ? "OrderBy" : "ThenBy";
                firstParam = false;
                if (item.StartsWith("-"))
                {
                    m += "Descending";
                    name = name.Substring(1);
                }
                var propertyInfo = GetPropertyInfo(typeInfo, name);
                var expr = GetOrderExpression(typeInfo, propertyInfo);
                var method = typeof(Queryable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeInfo, propertyInfo.PropertyType);
                query = (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
            }
            return query;
        }
    }
}
