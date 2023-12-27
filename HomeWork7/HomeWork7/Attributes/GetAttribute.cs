namespace Server.Attributes;
public class GetAttribute : HttpMethodAttribute
{
    public GetAttribute(string action) : base(action) { }
}