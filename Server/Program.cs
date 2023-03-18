using System.Net.Sockets;
using System.Net;
using Server;
using System.Text.Json;
using System.Diagnostics;

var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 45678);
listener.Start();

while (true)
{
    var client = await listener.AcceptTcpClientAsync();

    new Task(() =>
    {
        var stream = client.GetStream();
        var bw = new BinaryWriter(stream);
        var br = new BinaryReader(stream);
        while (true)
        {
            var jsonStr = br.ReadString();

            var command = JsonSerializer.Deserialize<Command>(jsonStr);

            if (command is null)
                return;

            switch (command.Text.ToLower())
            {
                case Command.PROCLIST:
                    bw.Write(GetProcesses());
                    stream.Flush();
                    break;
                case Command.KILL:
                    bw.Write(KillProcess(command.Param).ToString());
                    stream.Flush();
                    break;
                case Command.RUN:
                    bw.Write(RunProcess(command.Param).ToString());
                    stream.Flush();
                    break;
                default:
                    bw.Write("Command Not Found!");
                    stream.Flush();
                    break;
            }

        }
    }).Start();

    bool KillProcess(string? processName)
    {
        if (processName is not null)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                try
                {
                    foreach (var p in processes)
                        p.Kill();

                    return true;
                }
                catch (Exception) { }
            }
        }
        return false;
    }

    bool RunProcess(string? processName)
    {
        if (processName is not null)
        {
            try
            {
                Process.Start(processName);
                return true;
            }
            catch (Exception) { }

        }
        return false;
    }

    string GetProcesses()
    {
        var list = Process.GetProcesses();
        var names = list.Select(p => p.ProcessName);
        var jsonList = JsonSerializer.Serialize(names);
        return jsonList;
    }
}