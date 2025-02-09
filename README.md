# Build Your Own HTTP Server

A from-scratch implementation of an HTTP server in C#, built as a learning project to understand web protocols and network programming fundamentals.

## Project Overview

This project is an educational implementation of an HTTP server that handles basic HTTP requests and responses. It was built following the principles of [Build Your Own X](https://codecraft.tv/courses/build-your-own/http-server/introduction/), with the goal of understanding how HTTP servers work at a fundamental level.

### Features

- Basic HTTP server implementation
- Support for GET requests
- Multiple endpoint handlers (/files/, /user-agent, /echo)
- Concurrent request handling
- Simple routing system
- File serving capabilities

## Getting Started

### Prerequisites

- .NET 6.0 or higher
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/mohammad-ayeshh/MyHTTPServerClientProject.git
```

2. Navigate to the project directory
```bash
cd MyHTTPServerClientProject
```

3. Build the project
```bash
dotnet build
```

### Running the Server

1. Start the server
```bash
dotnet run --project Server
```

The server will start listening on port 4221.

### Testing the Server

You can test the server using:
- A web browser (navigate to `http://localhost:4221`)
- cURL commands
- The included client application

Example cURL commands:
```bash
# Test root endpoint
curl http://localhost:4221/

# Test echo endpoint
curl http://localhost:4221/echo/hello

# Test user-agent endpoint
curl http://localhost:4221/user-agent
```

## Project Structure

- `Server.cs`: Main server implementation
- `Request.cs`: HTTP request parsing and handling
- `Response.cs`: HTTP response construction
- `Router.cs`: Request routing system
- `Handlers.cs`: Endpoint handlers
- `Program.cs`: Entry point and client implementation

## Learning Outcomes

This project demonstrates:
- HTTP protocol fundamentals
- TCP/IP networking
- Request/Response patterns
- Concurrent programming
- C# async/await patterns
- Basic security considerations

## Contributing

This is a learning project, but suggestions and improvements are welcome! Feel free to:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

This project is open source and available under the [MIT License](LICENSE).

## Acknowledgments

- Inspired by [Build Your Own X](https://codecraft.tv/courses/build-your-own/http-server/introduction/)
- Built as a learning exercise in web protocols and network programming
- Special thanks to the developer community for their resources and inspiration
