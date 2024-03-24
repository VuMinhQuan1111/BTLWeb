using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblComment
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public int UsersId { get; set; }

    public string? CommentText { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual TblPost Post { get; set; } = null!;

    public virtual TblUser Users { get; set; } = null!;
}
