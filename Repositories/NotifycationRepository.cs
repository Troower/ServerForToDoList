using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerForToDoList.DBContext;
using ServerForToDoList.Model;
using System.Threading.Tasks;

namespace ServerForToDoList.Repositories
{
    public class NotifycationRepository
    {

        public static async System.Threading.Tasks.Task NotifyForUserCreateTask(Model.Task task, ToDoContext _context, FcmNotificationService notificationService)
        {
            try
            {
                if (task == null) return;
                var assignmentsIds = _context.TaskAssignments.Where(i => i.TaskId == task.TaskId).Select(u => u.UserId).ToList();
                if (!assignmentsIds.Any()) return;
                var deviceTokens = _context.UserDeviceTokens.Where(u => assignmentsIds.Contains(u.UserId)).Select(t => t.DeviceToken);
                foreach (var token in deviceTokens)
                {
                    if (token == null) continue;
                    else
                    {
                        await notificationService.SendNotificationAsync(token, "Новая задача", $"Назначена новая задача: \"{task.Title}\"");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public static async System.Threading.Tasks.Task NotifyForUserUpdateTask(Model.Task Task, ToDoContext _context, FcmNotificationService notificationService)
        {
            try
            {
                var userId = _context.TaskAssignments.Where(t => t.TaskId == Task.TaskId).Select(t => t.UserId).ToList();
                var deviceTokens = await _context.UserDeviceTokens
                     .Where(u => userId.Contains(u.UserId))
                     .Select(u => u.DeviceToken)
                     .ToListAsync();
                foreach (var token in deviceTokens)
                {
                    await notificationService.SendNotificationAsync(token, "Задача обновленна", $"Задача : \"{Task.Title}\" обновленна.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async System.Threading.Tasks.Task NotifyForUserConfirmTask(Model.Task task, ToDoContext _context, FcmNotificationService notificationService)
        {
            try
            {
                var Userids = _context.TaskAssignments.Where(t => t.TaskId == task.TaskId).Select(t => t.UserId).ToList();

                var deviceTokens = new List<string>();
                deviceTokens = await _context.UserDeviceTokens
                .Where(u => Userids.Contains(u.UserId))
                .Select(u => u.DeviceToken)
                .ToListAsync();

                foreach (var token in deviceTokens)
                {
                    await notificationService.SendNotificationAsync(token, "Задача прошла проверку", $"Ваша задача \"{task.Title}\" прошла проверку и была сдана");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static async System.Threading.Tasks.Task NotifyConfirmTask(Model.Task task, bool Flag, ToDoContext _context, FcmNotificationService notificationService)
        {
            try { 
            if (Flag)
            {
                var userIds = _context.TaskAssignments.Where(t => t.TaskId == task.TaskId).Select(u => u.UserId).ToList();
                var deviceTokens = _context.UserDeviceTokens.Where(t => userIds.Contains(t.UserId)).Select(u => u.DeviceToken).ToList();
                var CreatorToken = _context.UserDeviceTokens.FirstOrDefault(a => a.UserId == task.CreatedBy);
                if (CreatorToken != null)
                    deviceTokens.Add(CreatorToken.DeviceToken);
                foreach (var token in deviceTokens)
                {
                    notificationService.SendNotificationAsync(token, "Задача на проверке", $"Задача \"{task.Title}\" переданна на проверку");
                }
            }
            else
            {
                var userIds = _context.TaskAssignments.Where(t => t.TaskId == task.TaskId).Select(u => u.UserId).ToList();
                var deviceTokens = _context.UserDeviceTokens.Where(t => userIds.Contains(t.UserId)).Select(u => u.DeviceToken).ToList();
                foreach (var token in deviceTokens)
                {
                    notificationService.SendNotificationAsync(token, "Задача не прошла проверку", $"Задача \"{task.Title}\" возвращена в работу");
                }
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async System.Threading.Tasks.Task NotifyDeleteTask(Model.Task task, ToDoContext _context, FcmNotificationService notificationService)
        {
            try { 
            var userIds = _context.TaskAssignments.Where(t => t.TaskId == task.TaskId).Select(u => u.UserId).ToList();
            var deviceTokens = _context.UserDeviceTokens.Where(t => userIds.Contains(t.UserId)).Select(u => u.DeviceToken).ToList();
            foreach (var token in deviceTokens)
            {
                notificationService.SendNotificationAsync(token, "Задача удаленна", $"Задача \"{task.Title}\" удаленна");
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }



    }
}
