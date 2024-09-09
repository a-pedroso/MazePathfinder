namespace MazePathfinder.Tests.Integration.Core;
using System.Text.Json;

internal static class HttpResponseMessageExtensions
{
    public static string ToJson(this HttpResponseMessage result)
    {
        return JsonSerializer.Serialize(result);
    }
}
