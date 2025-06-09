using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerForToDoList.Model
{
    public class TaskAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("assignment_id")]
        public int AssignmentId { get; set; }

        [Required]
        [Column("task_id")]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("assigned_at")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [Column("assigned_by")]
        public int? AssignedBy { get; set; }

        [ForeignKey("AssignedBy")]
        public User? Assigner { get; set; }
    }
}
