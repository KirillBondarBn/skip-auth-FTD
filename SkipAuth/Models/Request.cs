using System;
using System.Collections.Generic;

namespace SkipAuth.Models;

public partial class Request
{
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }

    public Guid? ModeratorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateOnly DateStart { get; set; }

    public DateOnly DateEnd { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public bool? FileInDean { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
}
