using System.Net.Sockets;

string host = "127.0.0.1";
int port = 1414;
using TcpClient client = new TcpClient();
Console.Write("Кто ты, воин: ");
string? userName = Console.ReadLine();
Console.WriteLine($"Куку епта, {userName}");
StreamReader? Reader = null;
StreamWriter? Writer = null;
try {
    client.Connect(host, port); 
    Reader = new StreamReader(client.GetStream());
    Writer = new StreamWriter(client.GetStream());
    if (Writer is null || Reader is null) return;
    Task.Run(() => ReceiveMessageAsync(Reader));
    await SendMessageAsync(Writer);
}
catch (Exception exception) {
    Console.WriteLine(exception.Message);
}
Writer?.Close();
Reader?.Close();
async Task SendMessageAsync(StreamWriter writer) {
    await writer.WriteLineAsync(userName);
    await writer.FlushAsync();
    Console.WriteLine("Давай шустрей вводи словечки и жмакай Enter");
    while (true) {
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}
async Task ReceiveMessageAsync(StreamReader reader) {
    while (true) {
        try {
            string? message = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) continue;
            Print(message);
        } catch {
            break;
        }
    }
}
void Print(string message) {
    if (OperatingSystem.IsWindows()) {
        var cursorPosition = Console.GetCursorPosition();
        int left = cursorPosition.Left; 
        int top = cursorPosition.Top;
        Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
        Console.SetCursorPosition(0, top);
        Console.WriteLine(message);
        Console.SetCursorPosition(left, top + 1);
    }
    else Console.WriteLine(message);
}