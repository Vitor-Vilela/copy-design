using Adocao.Core;

namespace Adocao.Web.Configuration;
public class CommandDispatcher
{
    private readonly Dictionary<Type, object> _handlers = new();

    public void Register<TCommand, TResult>(ICommandHandler<TCommand, TResult> handler)
        where TCommand : ICommand<TResult>
    {
        _handlers[typeof(TCommand)] = handler;
    }

    public TResult Dispatch<TCommand, TResult>(TCommand command)
        where TCommand : ICommand<TResult>
    {
        if (_handlers.TryGetValue(typeof(TCommand), out var handlerObj))
        {
            var handler = (ICommandHandler<TCommand, TResult>)handlerObj;
            return handler.Handle(command);
        }

        throw new InvalidOperationException($"Nenhum handler registrado para {typeof(TCommand).Name}");
    }
}