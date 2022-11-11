# EchoServer

## Requirements

dotnet version 6

## Installation

To install dotnet dependencies run the following command:
```bash
dotnet restore
```

## Usage

To start the EchoServer run the following command:
```bash
dotnet run
```

To interact with the EchoServer open a terminal and run the following command to receive a response:
```bash
curl 127.0.0.1:8080 -d "Your Echo Message Here"
```

To shut down the EchoServer from the terminal run the following command:
```bash
curl 127.0.0.1:8080 -d "exit"
```

To run the tests, run the following command:
```bash
dotnet test
```