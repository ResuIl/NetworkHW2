using Client;
using System.Net.Sockets;
using System.Text.Json;

var client = new TcpClient("127.0.0.1", 45678);

var stream = client.GetStream();

var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

while (true)
{
    Console.WriteLine("Commands: proclist, run, kill");
    Console.WriteLine("Enter Command: ");

    var command = new Command();
    command.Text = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(command.Text))
    {
        switch (command.Text.ToLower())
        {
            case Command.PROCLIST:
                bw.Write(JsonSerializer.Serialize(command));
                break;
            case Command.RUN:
                Console.WriteLine("Enter Parameter: ");
                command.Param = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    bw.Write(JsonSerializer.Serialize(command));
                    continue;
                }
                Console.WriteLine("Enter Parameter!");
                break;
            case Command.KILL:
                Console.WriteLine("Enter Parameter: ");
                command.Param = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    bw.Write(JsonSerializer.Serialize(command));
                    continue;
                }
                Console.WriteLine("Enter Parameter!");
                break;
            default:
                bw.Write(JsonSerializer.Serialize(command));
                break;
        }
        await Task.Delay(50);
        Console.WriteLine(br.ReadString());
        Console.ReadKey();
        Console.Clear();
    }
}