using ServerForToDoList.Controllers;
using ServerForToDoList.Model;

namespace ServerForToDoList.Extensions
{
    public static class TaskAssignmentExtensions
    {

        #region Методы для конвертацмм(Мапинга)
        public static TaskAssignmentsDTO ToDto( TaskAssignment assignment)
        {
            if (assignment == null) return null;

            return new TaskAssignmentsDTO
            {
                AssignmentId = assignment.AssignmentId,
                TaskId = assignment.TaskId,
                UserId = assignment.UserId,
                AssignedAt = assignment.AssignedAt,
                AssignedBy = assignment.AssignedBy
            };
        }

        // Из TaskAssignmentsDTO в TaskAssignment
        public static TaskAssignment ToEntity( TaskAssignmentsDTO assignmentDto)
        {
            if (assignmentDto == null) return null;

            return new TaskAssignment
            {
                AssignmentId = assignmentDto.AssignmentId ?? 0,
                TaskId = assignmentDto.TaskId ?? 0,
                UserId = assignmentDto.UserId ?? 0,
                AssignedAt = assignmentDto.AssignedAt ?? DateTime.UtcNow,
                AssignedBy = assignmentDto.AssignedBy
            };
        }


        #endregion


    }
}
