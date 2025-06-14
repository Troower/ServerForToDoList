using Microsoft.EntityFrameworkCore;
using ServerForToDoList.Model;

namespace ServerForToDoList.DBContext
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Model.Task> Tasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<UserDeviceToken> UserDeviceTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.DeletedAt)
                .HasDatabaseName("idx_users_deleted_at");

            // Конфигурация для Task
            modelBuilder.Entity<Model.Task>()
                .HasIndex(t => t.DueDate)
                .HasDatabaseName("idx_tasks_due_date");

            modelBuilder.Entity<Model.Task>()
                .HasIndex(t => t.Status)
                .HasDatabaseName("idx_tasks_status");

            modelBuilder.Entity<Model.Task>()
                .HasIndex(t => t.CreatedBy)
                .HasDatabaseName("idx_tasks_created_by");

            modelBuilder.Entity<Model.Task>()
                .HasIndex(t => t.IsConfirmed)
                .HasDatabaseName("idx_tasks_is_confirmed");

            // Конфигурация для TaskAssignment
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Assignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.Assignments) // Связь только с одним навигационным свойством
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Assigner)
                .WithMany() // Без навигационного свойства в User
                .HasForeignKey(ta => ta.AssignedBy)
                .OnDelete(DeleteBehavior.SetNull);



            // Конфигурация для TaskType
            modelBuilder.Entity<TaskType>()
                .HasIndex(tt => tt.TypeName)
                .IsUnique();

            // Конфигурация для UserDeviceToken
            modelBuilder.Entity<UserDeviceToken>()
                .HasIndex(udt => udt.DeviceToken)
                .IsUnique();

            // Внешние ключи
            modelBuilder.Entity<Model.Task>()
                .HasOne(t => t.TaskType)
                .WithMany(tt => tt.Tasks)
                .HasForeignKey(t => t.TypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Model.Task>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Assignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Assigner)
                .WithMany()
                .HasForeignKey(ta => ta.AssignedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserDeviceToken>()
                .HasOne(udt => udt.User)
                .WithMany(u => u.DeviceTokens)
                .HasForeignKey(udt => udt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
