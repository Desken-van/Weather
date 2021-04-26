using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Interface;
using TG.Models.Commands;

namespace TG.Models.Settings
{
    public class ControllerService :IControllerService
    {

        IUserRepository repo;
        public ControllerService(IUserRepository r)
        {
            repo = r;
        }
        public async Task<Message> GetMessage(int id,string text,List<Command> commands ,TelegramBotClient botClient)
        {
            Data.User argument = await repo.GetUser(id);
            if (argument.Status == "Blocked")
            {
                return null;
            }
            else
            {
                User user = GetUser(argument);
                Chat chat = new Chat()
                {
                    Id = user.Id
                };
                Message message = new Message()
                {
                    Text = text,
                    From = user,
                    Chat = chat
                };
                return message;
            }
        }       
        public User GetUser(Data.User argument)
        {
                User user = new User()
                {
                    FirstName = argument.FirstName,
                    LastName = argument.LastName,
                    Username = argument.Username,
                    Id = (int)argument.TGId,
                };
                if (user == null)
                {
                    return null;
                }
                return user;        
        }
        public async void StatusNotif(int id,TelegramBotClient botClient)
        {
            Data.User argument = await repo.GetUser(id);
            try
            {
                if (argument.Status == "Active")
                {
                    await botClient.SendTextMessageAsync(argument.TGId, "Вам открыт доступ");
                }
                else
                {
                    await botClient.SendTextMessageAsync(argument.TGId, "Вам закрыт доступ");
                }
            }
            catch (Exception)
            {
            }
        }
        public async void DeleteNotif(int id, TelegramBotClient botClient)
        {
            Data.User argument = await repo.GetUser(id);
            try
            {
              await botClient.SendTextMessageAsync(argument.TGId, "Вы были удалены из базы данных");
            }
            catch (Exception)
            {
            }
        }
        public async void UpdateNotif(int id, TelegramBotClient botClient)
        {
            Data.User argument = await repo.GetUser(id);
            try
            {
                await botClient.SendTextMessageAsync(argument.TGId, "Ваши данные были обновлены в базе данных");
            }
            catch (Exception)
            {
            }
            
        }
        public IEnumerable<UserResponse> SortList(IEnumerable<UserResponse> listusers,string sort,string range)
        {
            IEnumerable<UserResponse> sorted = new List<UserResponse>();
            bool confirm = false;
            if (sort == "f")
            {
                if (range == "+")
                {
                    sorted = listusers.OrderBy(x => x.FirstName);
                    confirm = true;
                }
                else if (range == "-")
                {
                    sorted = listusers.OrderByDescending(x => x.FirstName);
                    confirm = true;
                }
            }
            else if (sort == "l")
            {
                if (range == "+")
                {
                    sorted = listusers.OrderBy(x => x.LastName);
                    confirm = true;
                }
                else if (range == "-")
                {
                    sorted = listusers.OrderByDescending(x => x.LastName);
                    confirm = true;
                }
            }
            else if (sort == "u")
            {
                if (range == "+")
                {
                    sorted = listusers.OrderBy(x => x.Username);
                    confirm = true;
                }
                else if (range == "-")
                {
                    sorted = listusers.OrderByDescending(x => x.Username);
                    confirm = true;
                }
            }
            else if (sort == "i")
            {
                if (range == "+")
                {
                    sorted = listusers.OrderBy(x => x.Id);
                    confirm = true;
                }
                else if (range == "-")
                {
                    sorted = listusers.OrderByDescending(x => x.Id);
                    confirm = true;
                }
            }
            if(confirm == false)
            {
                sorted = listusers;
            }
            return sorted;
        }
    }
}
