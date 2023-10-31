using System.Text.Json.Serialization;
using GistBlog.DAL.Enums;

namespace GistBlog.DAL.Entities.Resources;

public class StatusResource
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public ResponseType ResponseType { get; set; }

    public bool Status { get; set; }

    public string Message { get; set; } = null!;
}
