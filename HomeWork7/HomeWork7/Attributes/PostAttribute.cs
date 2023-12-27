namespace Server.Attributes;
public class PostAttribute : HttpMethodAttribute {
    public PostAttribute(string action) : base(action) { }
}