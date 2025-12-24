using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Behaviours;

public sealed class MessageValidatorBehaviour<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : notnull, IMessage
    where TResponse : Result
{
    private readonly IServiceProvider _serviceProvider;

    public MessageValidatorBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TMessage>>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(message);

            if (!validationResult.IsValid)
            {
                return (TResponse)Result.Invalid(validationResult.AsErrors());
            }
        }

        return await next(message, cancellationToken);
    }
}