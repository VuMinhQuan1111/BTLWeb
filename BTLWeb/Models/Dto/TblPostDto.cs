using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.Dto
{
    public class TblPostDto
    {
        public int UsersId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string? PostTitle { get; set; }
       
        [Required]
        public string? PostContent { get; set; }

        [Required]
        public IFormFile? PostImg { get; set; }

        [Required]
        public string? PostAuthor { get; set; }

        public virtual TblCategory Category { get; set; } = null!;

        public virtual ICollection<TblComment> TblComments { get; set; } = new List<TblComment>();

        public virtual ICollection<TblFavorite> TblFavorites { get; set; } = new List<TblFavorite>();

        public virtual TblUser Users { get; set; } = null!;
    }
}
