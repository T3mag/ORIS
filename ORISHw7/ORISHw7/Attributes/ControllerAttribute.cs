namespace MyHttpServer.Attributes;

public class ControllerAttribute : Attribute {
    public string ControllerName { get; }
    public ControllerAttribute(string controllerName) 
        => ControllerName = controllerName;
}