using System.Net.Sockets;
using System.Text.Json;
using Game.utils.;

namespace Server;

internal class ClientObject {
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }
    public string? UserName { get; set; }
    public string? Color { get; set; }
    private readonly TcpClient client;
    private readonly ServerObject server;

    public ClientObject(TcpClient tcpClient, ServerObject serverObject) {
        client = tcpClient;
        server = serverObject;
        var stream = client.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }
    public async Task ProcessAsync() {
        try {
            UserName = await Reader.ReadLineAsync();
            var addUserMessage = new AddUser { UserName = UserName, Color = ""};
            await server.BroadcastColoredMessageAsync(addUserMessage);
            var message = $"{UserName}";
            Console.WriteLine(message);
            await server.SendListAsync();
            await server.BroadcastPointsFieldMessageAsync();
            while (true) {
                await Task.Delay(10);
                try {
                    message = await Reader.ReadLineAsync();
                    var point = JsonSerializer.Deserialize<SendPoint>(message!);
                    await server.AddPoint(point!);
                } catch {
                    message = $"{UserName} ������� ����";
                    Console.WriteLine(message);
                    server.RemoveConnection(Id);
                    await server.SendListAsync();
                    await server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        } catch (Exception exception) {
            Console.WriteLine(exception.Message);
        }
        finally {
            server.RemoveConnection(Id);
        }
    }
    protected internal void Close() {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}