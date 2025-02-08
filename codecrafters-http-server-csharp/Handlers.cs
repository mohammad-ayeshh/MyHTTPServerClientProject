public class Handlers
{
    public static async Task<Response> HandleUserAgent(Request request)
    {
        if (!request.HasHeader("user-agent"))
        {
            return new Response("HTTP/1.1", 400, "Bad Request", "User-Agent header is required");
        }

        var userAgent = request.Headers["user-agent"];
        var headers = new Dictionary<string, string>
        {
            {"Content-Type", "text/plain"},
            {"Content-Length", userAgent.Length.ToString()}
        };

        return new Response("HTTP/1.1", 200, "OK", userAgent, headers);
    }
    public static async Task<Response> HandleRoot(Request request)
    {
        return new Response("HTTP/1.1", 200, "OK", "Welcome to the server!");
    }
    public static async Task<Response> HandleEcho(Request request)
    {
        string content = request.RequestTarget.Substring("/echo/".Length);
        var headers = new Dictionary<string, string>
        {
            {"Content-Type", "text/plain"},
            {"Content-Length", content.Length.ToString()}
        };

        return new Response("HTTP/1.1", 200, "OK", content, headers);
    }
    public static async Task<Response> HandleFileRequest(Request request)
    {
        try
        {
            // Extract filename from path
            string filePath = request.RequestTarget.Substring("/files/".Length);

            // Security: Prevent directory traversal
            filePath = Path.GetFileName(filePath);  // Only take filename

            // Construct full path (in a specific directory)
            string fullPath = Path.Combine("files", filePath);
            Console.Write($"the full path is {fullPath}");

            if (!File.Exists(fullPath))
            {
                return new Response("HTTP/1.1", 404, "File Not Found");
            }

            // Read file content
            string content = await File.ReadAllTextAsync(fullPath);

            var headers = new Dictionary<string, string>
        {
            {"Content-Type", "text/plain"},
            {"Content-Length", content.Length.ToString()}
        };

            return new Response("HTTP/1.1", 200, "OK", content, headers);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File error: {ex.Message}");
            return new Response("HTTP/1.1", 500, "Internal Server Error");
        }
    }
}