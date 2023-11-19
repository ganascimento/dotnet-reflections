namespace Dotnet.Reflaction.Api.Models;

public class CallerModel
{
    public required string Namespace { get; set; }
    public required string Target { get; set; }
    public Dictionary<string, dynamic>? Params { get; set; }
}