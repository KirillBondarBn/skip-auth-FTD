namespace SkipAuth.Models;

public partial class FileEntity
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public byte[] FileData { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
