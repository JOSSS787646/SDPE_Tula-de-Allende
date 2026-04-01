using FluentValidation;


/// <summary>
/// Comportamiento de pipeline en MediatR que ejecuta validaciones
/// automáticas usando FluentValidation antes de que una solicitud
/// sea procesada por su handler.
///
/// Recorre todos los validadores asociados al request, ejecuta sus reglas
/// y, si encuentra errores, lanza una excepción de validación deteniendo
/// la ejecución.
///
/// Su propósito es centralizar la validación y evitar duplicarla en los handlers,
/// asegurando que solo se procesen solicitudes válidas.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }


            return await next();
        }
    }
}
