using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.ModelsView
{
    public class MV_SignIn
    {
        [Required(ErrorMessage = "Hãy điền tên người dùng")]
        public required string UsersName { get; set; }

        [Required(ErrorMessage = "Hãy điền mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string UsersPass { get; set; }

        public bool RememberMe { get; set; }
    }
}
