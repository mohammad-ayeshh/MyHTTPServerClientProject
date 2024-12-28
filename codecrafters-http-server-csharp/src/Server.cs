using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main()
    {
        // You can use print statements as follows for debugging, they'll be visible when running tests.
        Console.WriteLine("Logs from your program will appear here!");

        // Uncomment this block to pass the first stage
        TcpListener server = new TcpListener(IPAddress.Any, 4221);
        server.Start();
        Console.WriteLine("Server is up and running");
            while (true)
            {
                using Socket clientSocket = await server.AcceptSocketAsync(); // wait for client, the server wont move untill the clinet connects
                Console.WriteLine("client connected");                

                _= Task.Run(() => HandleClientAsync(clientSocket));
            }
    }

static async Task HandleClientAsync(Socket clientSocket)
    {
        using (clientSocket) // Ensure the socket is disposed when done
        {
            try
            {
                byte[] buffer = new byte[2048];
                int receivedBytes = await clientSocket.ReceiveAsync(buffer);
                
                string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                Request clientRequest = new Request(clientMessage);
                Console.WriteLine($"Received from client: {clientMessage}");

                Console.WriteLine($"the client is tring to go to this path/Target: {clientRequest.RequestTarget}");
                Console.WriteLine($"the client is tring to send this Body: {clientRequest.Body}");

                string responseMsg;
                if (clientRequest.IsMatchMethod("GET"))
                {
                    if (clientRequest.IsMatchPath("/user-agent")){
                        if (clientRequest.HasHeader("user-agent"))
                        {
                            string body = clientRequest.Headers["user-agent"];
                            Dictionary<string,string> headers= new Dictionary<string, string>();
                            headers.Add("Content-Type","text");
                            headers.Add("Content-Length",""+body.Length);
                            responseMsg = new Response("HTTP/1.1",200,"WHAT THE FACK",body).ToString();
                        }
                        else
                        responseMsg = new Response("HTTP/1.1",200,"Missing Header").ToString();
                    }
                    else
                    {
                        responseMsg = new Response("HTTP/1.1",404,"Path Not Found").ToString();
                        // responseMsg = "HTTP/1.1 404 Path Not Found\r\n\r\n";
                    }
                }
                else responseMsg = new Response("HTTP/1.1",405,"Method Not Allowed").ToString();
                // else responseMsg = "HTTP/1.1 405 Method Not Allowed\r\n\r\n";
                byte[] responseAsBytes = Encoding.UTF8.GetBytes(responseMsg);
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