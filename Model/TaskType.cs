using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerForToDoList.Model
{
    public class TaskType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("type_name")]
        public string TypeName { get; set; }

        // Навигационное свойство
        public ICollection<Task> Tasks { get; set; }
    }
}
