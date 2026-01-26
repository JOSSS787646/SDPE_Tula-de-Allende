namespace SistemaDigitalizacionPolizas.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(
                typeof(LoginCommandHandler).Assembly
            );

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehaviour<,>)
            );

            return services;
        }
    }
}
