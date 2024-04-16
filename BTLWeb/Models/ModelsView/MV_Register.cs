using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.ModelsView
{
    public class MV_Register
    {
        [Required(ErrorMessage = "Hãy điền tên người dùng")]
        public required string UsersName {  get; set; }

        [Required(ErrorMessage = "Hãy điền Email")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public required string UsersEmail { get; set; }

        [Required(ErrorMessage = "Hãy điền mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string UsersPass { get; set; }

        [Required(ErrorMessage = "Hãy xác nhận mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [Compare("UsersPass", ErrorMessage ="Mật khẩu không khớp")]
        public required string ConfirmPass { get; set; }
    }
}
