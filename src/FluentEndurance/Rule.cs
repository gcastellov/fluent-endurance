using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance
{
    internal class Rule<T> : IRule
    {
        private MethodCallExpression _memberExpression;

        public Rule<T> BindRuleTo(Expression<Func<T, CancellationToken, Task>> expression)
        {
            var lambda = (LambdaExpression)expression;
            _memberExpression = (MethodCallExpression)lambda.Body;
            return this;
        }

        public string Name => _memberExpression.ToString();
        
        public MethodInfo GetMethodInfo() => _memberExpression.Method;
    }
}