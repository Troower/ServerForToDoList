using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerForToDoList.Model
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("task_id")]
      
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("title")]
    
        public string Title { get; set; }

        [Required]
        [Column("description")]
     
        public string Description { get; set; }

        [Required]
        [Column("due_date")]
   
        public DateTime DueDate { get; set; }

        [Column("due_time")]
        
        public TimeSpan? DueTime { get; set; }

        [Column("start_date")]
       
        public DateTime? StartDate { get; set; }

        [Column("is_important")]
       
        public bool IsImportant { get; set; } = false;

        [Column("type_id")]
       
        public int? TypeId { get; set; }

        [ForeignKey("TypeId")]
       
        public TaskType? TaskType { get; set; }

        [Column("status")]
       
        public bool Status { get; set; } = false;

        [Column("created_at")]
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("created_by")]
       
        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        
        public User Creator { get; set; }

        [Column("completed_at")]
        
        public DateTime? CompletedAt { get; set; }

        [Column("is_confirmed")]
        
        public bool IsConfirmed { get; set; } = false;

        // Навигационные свойства
       
        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();

    }
}
