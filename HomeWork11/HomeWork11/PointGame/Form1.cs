using System.Net.Sockets;
using System.Text.Json;
using Game.utils.Paths;
namespace PointGame;
public partial class Form1 : Form {
    private TcpClient client = null!;
    private StreamReader reader = null!;
    private StreamWriter writer = null!;
    private Button[,] buttons = null!;
    private const int GridSize = 15;
    public Form1() {
        InitializeComponent();
        listOfUsers.Visible = false;
        name.Visible = false;
        color.Visible = false;
    }
    private void btn_signIn_Click(object sender, EventArgs e) {
        const string host = "127.0.0.1";
        const int port = 8888;
        var userName = enterName.Text;
        try {
            client = new TcpClient();
            client.Connect(host, port);
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Task.Run(ReceiveMessageAsync);
            EnterUser(userName);
            name.Text = userName;
            label1.Visible = false;
            enterName.Visible = false;
            btn_signIn.Visible = false;
            listOfUsers.Visible = true;
            name.Visible = true;
        }
        catch (Exception ex) {
            MessageBox.Show($@"������ �����������: {ex.Message}");
        }
    }
    private async Task ReceiveMessageAsync() {
        while (true) {
            try {
                var message = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(message)) continue;
                Invoke((MethodInvoker)delegate { Print(message); });
            }catch (IOException) {
                MessageBox.Show(@"ошибка");
                break;
            }
            catch (Exception) { }
        }
    }
    private void Print(string message)
    {
        var messageSplit = message.Split();
        var messageType = messageSplit[0];
        var messageJson = messageSplit[1];
        switch (messageType) {
            case "SendList": {
                var users = JsonSerializer.Deserialize<List<AddUser>>(messageJson)
                            ?? throw new ArgumentNullException(nameof(messageJson));
                listOfUsers.Items.Clear();
                foreach (var user in users) {
                    listOfUsers.Items.Add(user.UserName);
                    listOfUsers.Items[^1].BackColor = ColorTranslator.FromHtml(user.Color!);
                }
                break;
            }
            case "AddUser": {
                var addUser = JsonSerializer.Deserialize<AddUser>(messageJson)
                              ?? throw new ArgumentNullException(nameof(messageJson));
                label1.Text = addUser.UserName;
                color.BackColor = ColorTranslator.FromHtml(addUser.Color!);
                color.Visible = true;
                InitializeGrid();
                break;
            }
            case "SendPoint": {
                var point = JsonSerializer.Deserialize<SendPoint>(messageJson)
                            ?? throw new ArgumentNullException(nameof(messageJson));
                buttons[point.Point.X, point.Point.Y].BackColor = ColorTranslator.FromHtml(point.Color!);
                break;
            }
        }
    }

    private void EnterUser(string userName) {
        writer.WriteLine(userName);
        writer.Flush();
    }

    private void InitializeGrid() {
        buttons = new Button[GridSize, GridSize];
        for (var i = 0; i < GridSize; i++) {
            for (var j = 0; j < GridSize; j++) {
                buttons[i, j] = new Button();
                buttons[i, j].SetBounds(20 * i, 20 * j, 20, 20);
                buttons[i, j].Click += Button_Click!;
                buttons[i, j].Tag = new Point(i, j);
                Controls.Add(buttons[i, j]);
            }
        }
    }
    private void Button_Click(object sender, EventArgs e) {
        var button = sender as Button;
        var position = (Point)button!.Tag;
        if (button.BackColor.Equals(color.BackColor)) return;
        button.BackColor = color.BackColor;
        var sendPoint = new SendPoint(position, ColorTranslator.ToHtml(color.BackColor));
        var json = JsonSerializer.Serialize(sendPoint);
        writer.WriteLine(json);
        writer.Flush();
    }
}