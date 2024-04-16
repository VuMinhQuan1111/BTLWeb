using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.ModelsView
{
    public class MV_Categories
    {
        [Required(ErrorMessage = "Hãy điền tên danh mục")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Hãy điền nội dung danh mục")]
        public string? CategoryDescription { get; set; }
    }
}
