using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblPost
{
    public int PostId { get; set; }

    public int UsersId { get; set; }

    public int CategoryId { get; set; }

    public string? PostTitle { get; set; }

    public string? PostContent { get; set; }

    public string? PostImg { get; set; }

    public DateTime PostCreateAt { get; set; }

    public virtual TblCategory Category { get; set; } = null!;

    public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();

    public virtual ICollection<TblFavorite> TblFavorites { get; set; } = new List<TblFavorite>();

    public virtual TblUser Users { get; set; } = null!;
}
