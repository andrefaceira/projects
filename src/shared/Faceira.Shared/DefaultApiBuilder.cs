using System.Reflection;
using Faceira.Shared.Application.Service.Installers;
using Microsoft.AspNetCore.Builder;

namespace Faceira.Shared.Application;

public static class DefaultApiBuilder
{
    public static WebApplicationBuilder CreateApiBuilder(string[] args,
        IEnumerable<Assembly>? assemblies = null)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        
        builder.Services.AddApi();
        builder.Services.AddApplication();
        builder.Services.AddHandlers(assemblies);

        return builder;
    }
}