namespace GistBlog.DAL.Entities.Models.UserEntities;

public static class StringConstants
{
    public const string PhoneNumberRegex = @"^\+[0-9]?()[0-9](\S)(\d[0-9]{9})$";

    public const string EmailRegex =
        @"\A(?:[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\Z";

    public const string Png = ".png";
    public const string Jpeg = ".jpeg";
    public const string Jpg = ".jpg";
    public const string Gif = ".gif";
    public const string Pdf = ".pdf";
    public const string Csv = ".csv";

    public static readonly string ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                                                     "Host=localhost;Database=TestHinfluenzerDB;Username=postgres;Password=root;Include Error Detail=true";
}
