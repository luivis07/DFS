using System.Text.Json;
using dfs.core.common.models;

namespace dfs.core.common.helpers;

public static class StringExtensions
{
    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;

        try
        {
            JsonSerializer.Deserialize<BaseMessage>(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
    public static string AtMost(this string source, int length)
    {
        return (source.Length < length) ? source : source.Substring(0, length);
    }
}