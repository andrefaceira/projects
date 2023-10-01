using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Faceira.Shared.Application.Service;

public static class ServiceApiRouteBuilder
{
    public static RouteGroupBuilder BuildServiceApiRoute<TMessage>(this RouteGroupBuilder routeBuilder, string? route = null)
        where TMessage : IMessage
    {
        route ??= typeof(TMessage).Name;
        
        routeBuilder.MapPost(
            route,
            async (TMessage message, IDispatcher dispatcher) => 
            {
                await dispatcher.Dispatch(message);
    
                return Results.Ok();
            });
        
        return routeBuilder;
    }
}