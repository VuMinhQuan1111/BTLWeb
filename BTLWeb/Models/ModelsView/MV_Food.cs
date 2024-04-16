using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.ModelsView
{
    public class MV_Food
    {
        
        public int FoodId { get; set; }


        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Hãy điền tên của đồ ăn")]
        public int FoodName { get; set; }

        [Required(ErrorMessage = "Hãy điền nội dung của đồ ăn")]
        public string? FoodDescription { get; set; }

        [Required(ErrorMessage = "Hãy thêm ảnh của đồ ăn")]
        public string? FoodImg { get; set; }
    }
}
