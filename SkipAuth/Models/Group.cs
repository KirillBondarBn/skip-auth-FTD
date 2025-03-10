using System;
using System.Collections.Generic;

namespace SkipAuth.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public int Number { get; set; }

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
}
