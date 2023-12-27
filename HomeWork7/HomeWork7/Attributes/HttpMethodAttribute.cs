namespace Server.Attributes;
public class HttpMethodAttribute : Attribute {
    public HttpMethodAttribute(string action) => Action = action;
    public string Action { get; set; }
}