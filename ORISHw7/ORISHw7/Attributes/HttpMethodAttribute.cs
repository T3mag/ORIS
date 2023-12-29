namespace MyHttpServer.Attributes;

public class HttpMethodAttribute : Attribute {
    public string ActionName { get; }
    protected HttpMethodAttribute(string actionName) 
        => ActionName = actionName;
}