using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTLWeb.Models;

[Table("TblUser")]
public partial class TblUser
{
    [Key]
    public int UsersId { get; set; }

    [Required(ErrorMessage = "Hãy điền đủ thông tin vào đây")]
    public string? UsersName { get; set; }

    [Required(ErrorMessage = "Hãy điền đủ thông tin vào đây")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [RegularExpression(@"^.*@hou\.edu\.vn$", ErrorMessage = "Email phải có domain @hou.edu.vn")]
    public string? UsersEmail { get; set; }

    [Required(ErrorMessage = "Hãy điền đủ thông tin vào đây")]
    public string? UsersPass { get; set; }

    public string? UsersRole { get; set; }

    public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();

    public virtual ICollection<TblFavorite> TblFavorites { get; set; } = new List<TblFavorite>();

    public virtual ICollection<TblPost> TblPosts { get; set; } = new List<TblPost>();
}
