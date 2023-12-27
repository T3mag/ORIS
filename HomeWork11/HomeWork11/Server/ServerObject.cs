using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Game.utils.Paths;
namespace Server;
internal class ServerObject {
    private readonly TcpListener tcpListener = new(IPAddress.Any, 8888);
    private readonly List<ClientObject> clients = new();
    private readonly List<SendPoint> pointsField = new();
    protected internal void RemoveConnection(string id) {
        var client = clients.FirstOrDefault(c => c.Id.Equals(id));
        if (client != null) clients.Remove(client);
        client?.Close();
    }
    protected internal void Listen() {
        try {
            tcpListener.Start();
            while (true) {
                var tcpClient = tcpListener.AcceptTcpClient();
                var clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
        finally {
            Disconnect();
        }
    }
    private string GenerateRandomColor() {
        var random = new Random();
        var color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        var hexColor = ColorTranslator.ToHtml(color);
        while (clients.Select(i => i.Color).Contains(hexColor))
            color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        return ColorTranslator.ToHtml(color);
    }
    protected internal async Task SendListAsync() {
        var sb = new StringBuilder();
        sb.Append("SendList ");
        sb.Append(JsonSerializer.Serialize(clients.Select(x => new AddUser(x.UserName!, x.Color!)).ToList()));
        foreach (var client in clients) {
            await client.Writer.WriteLineAsync(sb);
            await client.Writer.FlushAsync();
        }
    }
    protected internal async Task BroadcastColoredMessageAsync(AddUser addUser) {
        var color = GenerateRandomColor();
        addUser.Color = color;
        clients.Last().Color = color;
        var sb = new StringBuilder();
        sb.Append("AddUser ");
        var message = JsonSerializer.Serialize(addUser);
        sb.Append(message); {
            try {
                await clients.Last().Writer.WriteLineAsync(sb);
                await clients.Last().Writer.FlushAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
    protected internal async Task BroadcastPointsFieldMessageAsync() {
        foreach (var point in pointsField) {
            var sb = new StringBuilder();
            sb.Append("SendPoint "); {
                try {
                    var message = JsonSerializer.Serialize(point);
                    sb.Append(message);
                    await clients.Last().Writer.WriteLineAsync(sb);
                    await clients.Last().Writer.FlushAsync();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
    protected internal async Task BroadcastPointMessageAsync(SendPoint point) {
        var sb = new StringBuilder();
        sb.Append("SendPoint "); {
            try {
                sb.Append(JsonSerializer.Serialize(point));
                foreach (var client in clients) {
                    await client.Writer.WriteLineAsync(sb);
                    await client.Writer.FlushAsync();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }

    protected internal void Disconnect() {
        foreach (var client in clients)
            client.Close();
        tcpListener.Stop();
    }
    protected internal async Task BroadcastMessageAsync(string message, string id) {
        var usersToJson = JsonSerializer.Serialize(clients.Select(x => x.UserName).ToList());
        foreach (var client in clients) {
            await client.Writer.WriteLineAsync(usersToJson);
            await client.Writer.FlushAsync();
        }
    }
    protected internal async Task AddPoint(SendPoint point) {
        pointsField.Add(point);
        await BroadcastPointMessageAsync(point);
    }
}