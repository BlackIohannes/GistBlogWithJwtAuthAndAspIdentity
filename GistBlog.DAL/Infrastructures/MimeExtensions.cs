using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using GistBlog.DAL.Entities.Models.UserEntities;

namespace GistBlog.DAL.Infrastructures;

public static class MimeExtensions
{
    public static string GetContentType(string extension)
    {
        switch (extension)
        {
            case StringConstants.Jpeg:
            case StringConstants.Jpg:
                return "image/jpeg";
            case StringConstants.Gif:
                return "image/gif";
            case StringConstants.Png:
                return "image/png";
            case StringConstants.Pdf:
                return "application/pdf";
            case StringConstants.Csv:
                return "text/csv";
            default:
                return "application/octet-stream";
        }
    }
}

public static class EnumExtensions
{
    public static int All<T>() where T : unmanaged, Enum
    {
        if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
            throw new InvalidEnumArgumentException("Method Is Only Applicable For Enum Type Of Int");

        var values = Enum.GetValues<T>().Select(x => Unsafe.As<T, int>(ref x));

        return values.Sum(x => x);
    }

    public static string ToPascalString<T>(this T @enum) where T : struct, Enum
    {
        return @enum.ToString().PascalStringToSentence();
    }
}

public static class StringExtensions
{
    public static string PascalStringToSentence(this string word)
    {
        return Regex.Replace(word, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1]}");
    }
}
