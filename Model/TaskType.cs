using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerForToDoList.Model
{
    public class TaskType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        // Навигационное свойство
        public ICollection<Task> Tasks { get; set; }
    }
}
