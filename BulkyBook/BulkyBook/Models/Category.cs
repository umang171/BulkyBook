using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Display Order")]
        [Range(1,100,ErrorMessage ="Please enter value between 1 to 100")]
        public int DisplayOrder { get; set; }
        public DateTime CreataedDateTime { get; set; }= DateTime.Now;

    }
}
