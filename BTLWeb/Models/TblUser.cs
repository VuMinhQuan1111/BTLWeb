using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblUser
{
    public int UsersId { get; set; }

    public string? UsersName { get; set; }

    public string? UsersEmail { get; set; }

    public string? UsersPass { get; set; }

    public string? UsersRole { get; set; }

    public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();

    public virtual ICollection<TblFavorite> TblFavorites { get; set; } = new List<TblFavorite>();

    public virtual ICollection<TblPost> TblPosts { get; set; } = new List<TblPost>();
}
