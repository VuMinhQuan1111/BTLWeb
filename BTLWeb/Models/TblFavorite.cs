using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblFavorite
{
    public int FavoriteId { get; set; }

    public int? PostId { get; set; }

    public int UsersId { get; set; }

    public virtual TblPost? Post { get; set; }

    public virtual TblUser Users { get; set; } = null!;
}
