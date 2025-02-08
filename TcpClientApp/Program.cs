using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    private static List<Task>? connectionTasks;

    static async Task Main()
    {
        try
        {
            connectionTasks = new List<Task>();
            await SendParallelRequests(1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
    public static async Task SendParallelRequests(int numOfRequests)
    {
        for (int i = 0; i < numOfRequests; i++)
        {
            connectionTasks?.Add(MakeOneConnectionAsync());
        }
        try
        {
            if (connectionTasks != null && connectionTasks.Count > 0)
            {   
                // Wait for all tasks to complete
                await Task.WhenAll(connectionTasks);
                Console.WriteLine("All connections completed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"One or more connections failed: {ex.Message}");
        }
    }
    private static async Task MakeOneConnectionAsync()
    {
        // Each connection gets its own resources using 'using' for automatic cleanup
        using (TcpClient client = new TcpClient())
        {
            try
            {
                // Connect to server usnig TCP protocol
                await client.ConnectAsync("127.0.0.1", 4221);
                using (NetworkStream stream = client.GetStream())
                { 
                    // Create and send request (your existing request code)
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("user-agent", "foobar/1.2.3");
                    Request request = new Request("GET", "/files/help.txt", "HTTP/1.1", body: "", headers);
                    byte[] dataToSend = request.ToByte();
                    await stream.WriteAsync(dataToSend);

                    // Read response
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Response oResponse = new Response(response);

                    Console.WriteLine($"Response body received: {oResponse.Body}");
                    Console.WriteLine($"Response received: {response}");
                }
            }
            catch (Exception ex)
            {
                // Each connection handles its own errors
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }
    }

}
