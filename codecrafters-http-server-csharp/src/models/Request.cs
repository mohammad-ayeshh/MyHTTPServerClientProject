using System;
using System.Text;

public class Request
{
    // request line
    public string HTTPMethod { get; set; }
    public string RequestTarget { get; set; }
    public string HTTPVersion { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Headers { get; set; }

    // Constructor to initialize a request manually (useful for the client-side)
    public Request(string method, string target, string version, string body = "", Dictionary<string, string>? headers = null)
    {
        HTTPMethod = method;
        RequestTarget = target;
        HTTPVersion = version;
        Body = body;
        Headers = headers ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    // Constructor to parse raw HTTP request strings (useful for server-side)
    public Request(string rawRequest)
    {
        // Split the request into sections: headers and body, what splits them is the "\r\n\r\n" in the end of the headers
        string[] requestSections = rawRequest.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
        string headerSection = requestSections[0];
        Body = requestSections.Length > 1 ? requestSections[1] : string.Empty;

        // Split header section into lines
        string[] lines = headerSection.Split("\r\n");

        // Parse the request line (first line), first line has the http method, and the path(target), and the http version
        string[] requestLineParts = lines[0].Split(" ");
        if (requestLineParts.Length != 3)
        {
            throw new ArgumentException("Invalid request line. Expected format: METHOD TARGET HTTP_VERSION");
        }

        HTTPMethod = requestLineParts[0];
        RequestTarget = requestLineParts[1];
        HTTPVersion = requestLineParts[2];

        // Parse headers (remaining lines in the header section)
        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] headerParts = lines[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
            if (headerParts.Length == 2)
            {
                Headers[headerParts[0].ToLower()] = headerParts[1];
            }
        }
    }

    // Method to check if the request matches specific conditions (useful for routing on the server)
    public bool IsMatchMethod(string method)
    {
        return string.Equals(HTTPMethod, method, StringComparison.OrdinalIgnoreCase);
    }
    public bool IsMatchPath(string path)
    {
        return string.Equals(RequestTarget, path, StringComparison.OrdinalIgnoreCase);
    }
    public bool HasHeader(string header){
        Console.WriteLine($"Checking if header exists: {header}");
        bool exists = Headers.ContainsKey(header);
        Console.WriteLine($"Header exists: {exists}");
        return exists;
    }

    // ToString method to convert the request back into a raw HTTP request string (useful for debugging or client-side)
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        // Add the request line
        sb.AppendLine($"{HTTPMethod} {RequestTarget} {HTTPVersion}");

        // Add headers
        foreach (var header in Headers)
        {
            sb.AppendLine($"{header.Key}: {header.Value}");
        }

        // Add a blank line to separate headers and body
        sb.AppendLine();

        // Add the body
        if (!string.IsNullOrEmpty(Body))
        {
            sb.Append(Body);
        }

        return sb.ToString();
    }

    // ToByte method to convert the request to a byte array (useful for client-side TCP communication)
    public byte[] ToByte()
    {
        return Encoding.UTF8.GetBytes(ToString());
    }
}
