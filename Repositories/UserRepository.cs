using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;
using ServerForToDoList.Model;

namespace ServerForToDoList.Repositories
{
    public class UserRepository
    {
        // Хранилище методов для работы с user (CRUD create read update delete)


        #region create
        //Добавление пользователя
        public static async System.Threading.Tasks.Task AddUserAsync(ToDoContext context, User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        #endregion

        #region read
        //Получение пользователя по id
        public static async Task<User?> GetUserByIdAsync(ToDoContext context, int userId)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.UserId == userId); // Возвращает null если не найден
        }

        //Получение всех пользователей(Для админа у которого created_by=null)
        public static async Task<List<User>> GetUsersAsync(ToDoContext context)
        {
            return await context.Users.Where(u => u.CreatedBy != null).ToListAsync();
        }

        //Получение пользователе по id создателя. Как 6 числа я объяснял, все что ниже создателя попадает сюда(Для супер админа использовать метод выше 🙌)
        public static async Task<List<User>> GetAllUserByIdCreatedAsync(ToDoContext context, int creatorId)
        {
            var result = new List<User>();
            var queue = new Queue<int>();
            queue.Enqueue(creatorId);

            while (queue.Count > 0)
            {
                var currentCreatorId = queue.Dequeue();
                var directUsers = await context.Users
                    .Where(u => u.CreatedBy == currentCreatorId)
                    .ToListAsync();

                foreach (var user in directUsers)
                {
                    result.Add(user);
                    queue.Enqueue(user.UserId);
                }
            }

            return result;
        }
        #endregion

        #region update
        //Редактирование пользователя
        public static async System.Threading.Tasks.Task UpdateUserAsync(ToDoContext context, User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        #endregion

        #region delete
        //Удаление пользователя(полностью)
        public static async void DeleteUserAsync(ToDoContext context, int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        //Удаление пользователя(sofr-delte)
        public static async void SoftDeleteUserAsync(ToDoContext context, int userId)
        {
           var user = await context.Users.FindAsync(userId);
           user.DeletedAt= DateTime.Now;
           await context.SaveChangesAsync();
        }
        #endregion
        
    }

}
