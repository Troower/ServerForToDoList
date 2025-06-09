using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerForToDoList.Model
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public TimeSpan? DueTime { get; set; }

        public DateTime? StartDate { get; set; }

        public bool IsImportant { get; set; } = false;

        public int? TypeId { get; set; }

        [ForeignKey("TypeId")]
        public TaskType TaskType { get; set; }

        public bool Status { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User Creator { get; set; }

        public DateTime? CompletedAt { get; set; }

        public bool IsConfirmed { get; set; } = false;

        // Навигационные свойства
        public ICollection<TaskAssignment> Assignments { get; set; }
    }
}
