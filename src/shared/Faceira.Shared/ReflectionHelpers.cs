using System.Reflection;

namespace Faceira.Shared.Application;

public static class ReflectionHelpers
{
    public static IEnumerable<Type> GetGenericImplementations(this Assembly assembly, Type type)
    {
        return assembly
            .GetTypes()
            .Where(p => p.GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == type));
    }
    
    public static Type GetGenericImplementationType(this Type type)
    {
        return type
            .GetInterfaces()
            .First()
            .GetGenericArguments()
            .First();
    }

    public static Type GetImplementedInterface(this Type type)
    {
        return type.GetInterfaces().First();
    }

    public static void InvokeGenericMethod(this Type type, 
        string methodName, Type genericType, object[] arguments)
    {
        var method = type
            .GetMethod(methodName)?
            .MakeGenericMethod(genericType);
        method?.Invoke(null, arguments);
    }
}