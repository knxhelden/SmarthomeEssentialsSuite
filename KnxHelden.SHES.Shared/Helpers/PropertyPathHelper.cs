using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KnxHelden.SHES.Shared.Helpers
{
    public static class PropertyPathHelper
    {
        /// <summary>
        /// Extracts the full property path from a lambda expression, such as "Device.Sensor.Id"
        /// from an expression like x => x.Device.Sensor.Id.
        /// </summary>
        /// <typeparam name="T">The type returned by the expression.</typeparam>
        /// <param name="expression">The lambda expression representing the property access.</param>
        /// <returns>A string representing the full property access path, with dot-separated member names.</returns>
        public static string GetPropertyPath<T>(Expression<Func<T>> expression)
        {
            var stack = new Stack<string>();
            Expression expr = expression.Body;

            // Traverse the expression tree as long as it's a MemberExpression
            while (expr is MemberExpression memberExpr)
            {
                stack.Push(memberExpr.Member.Name); // Add member name to the stack
                expr = memberExpr.Expression;       // Move to the next inner expression
            }

            // Handle conversions (e.g. object boxing), which are represented as UnaryExpressions
            if (expr is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            {
                expr = unaryMember.Expression;
                stack.Push(unaryMember.Member.Name);

                // Continue traversing if there are nested member expressions
                while (expr is MemberExpression nested)
                {
                    stack.Push(nested.Member.Name);
                    expr = nested.Expression;
                }
            }

            // Combine member names into a dot-separated path
            return string.Join(".", stack);
        }
    }
}
