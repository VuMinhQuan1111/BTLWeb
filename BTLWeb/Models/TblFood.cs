using System;
using System.Collections.Generic;

namespace BTLWeb.Models;

public partial class TblFood
{
    public int FoodId { get; set; }

    public int? CategoryId { get; set; }

    public string? FoodName { get; set; }

    public string? FoodDescription { get; set; }

    public string? FoodImg { get; set; }

    public virtual TblCategory? Category { get; set; }
}
