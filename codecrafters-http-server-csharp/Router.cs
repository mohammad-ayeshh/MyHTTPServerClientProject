public delegate Task<Response> RequestHandler(Request request);

public class Route
{
    public string PathPattern { get; set; }
    public string[] Methods { get; set; }
    public RequestHandler Handler { get; set; }

}

public class Router
{
    private List<Route> _routes = new List<Route>();

    public void AddRoute(string path, string[] methods, RequestHandler handler)
    {
        _routes.Add(new Route
        {
            PathPattern = path,
            Methods = methods,
            Handler = handler
        });
    }

    public async Task<Response> HandleRequest(Request request)
    {
        var route = _routes.FirstOrDefault(r =>
            IsPathMatch(r.PathPattern, request.RequestTarget) &&
            r.Methods.Contains(request.HTTPMethod, StringComparer.OrdinalIgnoreCase));

        if (route == null)
        {
            return new Response("HTTP/1.1", 404, "Not Found");
        }

        try
        {
            Console.WriteLine($"the handler is: {request}");
            return await route.Handler(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling request: {ex.Message}");
            return new Response("HTTP/1.1", 500, "Internal Server Error");
        }
    }

    private bool IsPathMatch(string pattern, string path)
    {
        //todo: use Dynamic Routes instead
        // If pattern ends with '/', treat it as a prefix match
        if (pattern.EndsWith("/"))
        {
            return path.StartsWith(pattern, StringComparison.OrdinalIgnoreCase);
        }

        // Otherwise, require exact match
        return string.Equals(pattern, path, StringComparison.OrdinalIgnoreCase);
    }
}