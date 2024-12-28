using System;
using System.Text;

public class Response
{
    // Properties for the status line
    public string HTTPVersion { get; set; }
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; }

    // Body of the response
    public string Body { get; set; }

    // Optional headers
    public Dictionary<string, string> Headers { get; set; }

    // Constructor to initialize a response manually (useful for the server-side)
    public Response(string version, int statusCode, string reasonPhrase, string body = "", Dictionary<string, string>? headers = null)
    {
        HTTPVersion = version;
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        Body = body;
        Headers = headers ?? new Dictionary<string, string>();
    }

    // Constructor to parse a raw HTTP response string (useful for client-side)
    public Response(string rawResponse)
    {
        // Split the response into sections: headers and body
        string[] responseSections = rawResponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
        string headerSection = responseSections[0];
        Body = responseSections.Length > 1 ? responseSections[1] : string.Empty;

        // Split header section into lines
        string[] lines = headerSection.Split("\r\n");

        // Parse the status line (first line)
        string[] statusLineParts = lines[0].Split(" ", 3);
        if (statusLineParts.Length < 2)
        {
            throw new ArgumentException("Invalid status line. Expected format: HTTP_VERSION STATUS_CODE REASON_PHRASE");
        }

        HTTPVersion = statusLineParts[0];
        StatusCode = int.Parse(statusLineParts[1]);
        ReasonPhrase = statusLineParts.Length > 2 ? statusLineParts[2] : string.Empty;

        // Parse headers (remaining lines in the header section)
        Headers = new Dictionary<string, string>();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] headerParts = lines[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
            if (headerParts.Length == 2)
            {
                Headers[headerParts[0]] = headerParts[1];
            }
        }
    }

    // Method to check if the response matches a specific status code
    public bool IsStatusCode(int code)
    {
        return StatusCode == code;
    }

    // ToString method to convert the response back into a raw HTTP response string (useful for debugging or server-side)
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        // Add the status line
        sb.AppendLine($"{HTTPVersion} {StatusCode} {ReasonPhrase}");

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

    // ToByte method to convert the response to a byte array (useful for server-side TCP communication)
    public byte[] ToByte()
    {
        return Encoding.UTF8.GetBytes(ToString());
    }
}
        