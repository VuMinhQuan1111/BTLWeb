using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.ModelsView
{
    public class MV_Changepass
    {
        [Required(ErrorMessage = "Hãy điền mật khẩu")]
        //[MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string UsersOldPass { get; set; }

        [Required(ErrorMessage = "Hãy điền mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string UsersNewPass { get; set; }

        [Required(ErrorMessage = "Hãy điền xác nhận mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [Compare("UsersNewPass", ErrorMessage = "Mật khẩu không khớp")]
        public required string ConfirmPass { get; set; }
    }
}
