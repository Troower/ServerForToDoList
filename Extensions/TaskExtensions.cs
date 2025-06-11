using ServerForToDoList.Controllers;

namespace ServerForToDoList.Extensions
{
    public  class TaskExtensions
    {
        public static TaskDTO ToDto( Model.Task task)
        {
            if (task == null) return null;

            return new TaskDTO
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                DueTime = task.DueTime,
                StartDate = task.StartDate,
                IsImportant = task.IsImportant,
                TypeId = task.TypeId ?? 0,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                CreatedBy = task.CreatedBy,
                CompletedAt = task.CompletedAt,
                IsConfirmed = task.IsConfirmed,
                Assignments = task.Assignments?.Select(a => TaskAssignmentExtensions.ToDto(a)).ToList()
            };
        }

        // Из TaskDTO в Task
        public static Model.Task ToEntity(TaskDTO taskDto)
        {
            if (taskDto == null) return null;

            return new Model.Task
            {
                TaskId = taskDto.TaskId,
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                DueTime = taskDto.DueTime,
                StartDate = taskDto.StartDate,
                IsImportant = taskDto.IsImportant,
                TypeId = taskDto.TypeId,
                Status = taskDto.Status,
                CreatedAt = taskDto.CreatedAt,
                CreatedBy = taskDto.CreatedBy,
                CompletedAt = taskDto.CompletedAt,
                IsConfirmed = taskDto.IsConfirmed,
                Assignments = taskDto.Assignments?.Where(a => !a.ToDelete).Select(a => TaskAssignmentExtensions.ToEntity(a)).ToList() ?? []
            };
        }
    }
}
