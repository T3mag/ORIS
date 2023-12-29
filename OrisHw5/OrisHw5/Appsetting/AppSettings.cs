namespace Server.AppSetting;
public class AppSettings {
    public AppSettings(uint port, string? address, string? staticFilesPath)
    {
        Port = port;
        Address = address;
        StaticFilesPath = staticFilesPath;
    }
    public string? Address {  get; set; }
    public uint Port { get; set; }
    public string? StaticFilesPath { get; set; }
}