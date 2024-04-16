using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDescription { get; set; }

    public virtual ICollection<TblFood> TblFoods { get; set; } = new List<TblFood>();

    public virtual ICollection<TblPost> TblPosts { get; set; } = new List<TblPost>();
}
