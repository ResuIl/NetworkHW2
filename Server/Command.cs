namespace Server;

public class Command
{
    public const string PROCLIST = "proclist";
    public const string KILL = "kill";
    public const string RUN = "run";
    public string Text { get; set; }
    public string Param { get; set; }
}
