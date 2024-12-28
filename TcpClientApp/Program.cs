using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            // 1. Connect to the server on localhost at port 4221
            using TcpClient client = new TcpClient("127.0.0.1", 4221);

            // 2. Get the network stream for communication
            using NetworkStream stream = client.GetStream();
            
            Dictionary<string,string> headers= new Dictionary<string, string>();
            headers.Add("user-agent","foobar/1.2.3");

            //create new http request(Request line,Zero or more headers,Optional request body)
            Request request = new Request("GET","/user-agent","HTTP/1.1", body: "",headers);

                
            // 3. Send a request message to the server
            // string message = "GET /465 HTTP/1.1\r\nHost: localhost:4221\r\nUser-Agent: curl/7.64.1\r\nAccept: */*\r\n\r\n";
            byte[] dataToSend = request.ToByte();

            stream.Write(dataToSend, 0, dataToSend.Length);
            Console.WriteLine($"Sent to server: {request}");

            // 4. Read the server's response
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received from server: {serverResponse}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
}
