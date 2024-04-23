using E_Commerce_Bot.Recources;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace E_Commerce_Bot.Services;

public class LocalizationHandler : ILocalizationHandler
{
    private readonly IServiceScopeFactory serviceScopeFactory;

    public LocalizationHandler(
        IServiceScopeFactory serviceScopeFactory)
            => this.serviceScopeFactory = serviceScopeFactory;

    public string GetValue(string key, params string[] arguments)
    {
        Type buttonType = typeof(Button);
        string assemblyQualifiedName = buttonType.AssemblyQualifiedName;

        Console.WriteLine(assemblyQualifiedName);
        using var scope = serviceScopeFactory.CreateScope();
        var t = GetResourceClasses(typeof(Button).Namespace);
        var types = t.Select(c => typeof(IStringLocalizer<>).MakeGenericType(c));
        var localizers = types.Select(t => scope.ServiceProvider.GetService(t) as IStringLocalizer);

        foreach (var localizer in localizers)
        {
            var value = localizer.GetString(key, arguments);
            if (value != key)
                return value;
        }

        return key;
    }

    private static Type[] GetResourceClasses(string @namespace)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        return asm.GetTypes()
            .Where(type => type.Namespace == @namespace)
            .ToArray();
    }
}