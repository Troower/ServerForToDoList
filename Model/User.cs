using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerForToDoList.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column("surname")]
        public string? Surname { get; set; }

        [Required]
        [StringLength(50)]
        [Column("login")]
        public string Login { get; set; }

        [Required]
        [StringLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [Column("role")]
        public string Role { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Навигационные свойства
        public ICollection<Task> CreatedTasks { get; set; }
        public ICollection<TaskAssignment> Assignments { get; set; }
        public ICollection<UserDeviceToken> DeviceTokens { get; set; }
    }
}
