using System.ComponentModel.DataAnnotations;

namespace BTLWeb.Models.RequestModels
{
    public class AddComments
    {
        [Required]
        public int id { get; set; }

        [Required]
        public required string comments { get; set; }
    }
}
