using System;
using System.Collections.Generic;

namespace SkipAuth.Models;

public partial class Token
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token1 { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public virtual User User { get; set; } = null!;
}
