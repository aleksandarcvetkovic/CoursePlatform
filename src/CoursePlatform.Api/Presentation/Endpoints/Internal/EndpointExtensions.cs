using System.Reflection;

namespace PaymentProcessing.Api.Endpoints.Internal;

public static class EndpointsExtensions
{
    public static void MapEndpoints(this IApplicationBuilder app)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining();

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoint.DefineEndpoints))!
                .Invoke(app, [app]);
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining()
    {
        var endpointTypes = Assembly.GetExecutingAssembly().DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(IEndpoint).IsAssignableFrom(x));
        return endpointTypes;
    }

    public static RouteHandlerBuilder DefineDefaultResponseCodes(this RouteHandlerBuilder builder)
    {
        builder.Produces(StatusCodes.Status500InternalServerError);
        builder.Produces(StatusCodes.Status503ServiceUnavailable);
        builder.Produces(StatusCodes.Status200OK);
        return builder;
    }
}