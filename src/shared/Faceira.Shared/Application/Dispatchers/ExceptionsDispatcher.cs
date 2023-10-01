using Faceira.Shared.Application.Messages;

namespace Faceira.Shared.Application.Application.Dispatchers;

public class ExceptionsDispatcher : IDispatcher
{
    private readonly IDispatcher _dispatcher;

    public ExceptionsDispatcher(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public Task Dispatch<TMessage>(TMessage message) where TMessage : IMessage
    {
        try
        {
            return _dispatcher.Dispatch(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}