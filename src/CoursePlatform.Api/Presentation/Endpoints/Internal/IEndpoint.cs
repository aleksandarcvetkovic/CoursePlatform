namespace CoursePlatform.Api.Presentation.Endpoints.Internal;

public interface IEndpoint
{
    static abstract string BaseRoute { get; }
    public static abstract void DefineEndpoints(IEndpointRouteBuilder app);

}