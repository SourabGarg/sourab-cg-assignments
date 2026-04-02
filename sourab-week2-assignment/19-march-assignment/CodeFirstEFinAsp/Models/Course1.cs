using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstEFinAsp.Models
{
    public class Course1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [Column("Stitle", TypeName = "varchar")]
        public string Title { get; set; }

        [Required]
        [MaxLength(220)]
        public string Description { get; set; }
        public float fullprice { get; set; }
        public Author1 Author { get; set; }
        
        [ForeignKey("Author")]
        public int AuthorId { get; set; }



    }
}
