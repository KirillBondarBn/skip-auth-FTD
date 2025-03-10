using System;
using System.Collections.Generic;

namespace SkipAuth.Models;

public partial class GroupStudent
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
