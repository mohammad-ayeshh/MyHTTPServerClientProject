using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main()
    {
        var router = new Router();

        // Add routes
        router.AddRoute("/files/", new[] { "GET" }, Handlers.HandleFileRequest);
        router.AddRoute("/", new[] { "GET" }, Handlers.HandleRoot);
        router.AddRoute("/user-agent", new[] { "GET" }, Handlers.HandleUserAgent);
        router.AddRoute("/echo", new[] { "GET" }, Handlers.HandleEcho);

        TcpListener server = new TcpListener(IPAddress.Any, 4221);
        server.Start();
        Console.WriteLine("Server is up and running");
        while (true)
        {
            Socket clientSocket = await server.AcceptSocketAsync(); // wait for client, the server wont do anything untill the clinet connects
            Console.WriteLine("client connected");

            _= Task.Run(() => HandleClientAsync(clientSocket,router));
        }
    }

    static async Task HandleClientAsync(Socket clientSocket,Router router)
    {
        using (clientSocket) // Ensure the socket is disposed when done
        {
            try
            {
                byte[] buffer = new byte[2048];
                int receivedBytes = await clientSocket.ReceiveAsync(buffer);
                
                string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                Request clientRequest = new Request(clientMessage);
                Response response = await router.HandleRequest(clientRequest);
                byte[] responseAsBytes = Encoding.UTF8.GetBytes(response.ToString());
                await clientSocket.SendAsync(responseAsBytes);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }
    }

}