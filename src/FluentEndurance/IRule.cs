using System.Reflection;

namespace FluentEndurance
{
    public interface IRule
    {
        string Name { get; }

        MethodInfo GetMethodInfo();
    }
}