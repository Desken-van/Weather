using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Data;
using TG.Entity;
using TG.Interface;
using TG.Models.Commands;

namespace TG.Models.Settings
{
    public class Logic
    {
        IUserRepository repo;
        private IControllerService controllerService { get; set; }
        public Logic(IUserRepository r,IControllerService service)
        {
            repo = r;
            controllerService = service;
        }
        public async Task<UserResponse> Get(long id)
        {
            Data.User user = await repo.GetTG(id);
            if(user == null)
            {
                return null;
            }
            UserResponse response = new UserResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Status = user.Status
            };
            return response;
        }
        public async Task RegisterAsync(Message message , TelegramBotClient client )
        {
            RegisterCommand register = new RegisterCommand(repo,controllerService);
            await register.Execute(message, client);           
        }
        public async Task StartAsync(Message message, TelegramBotClient client)
        {
            StartCommand start = new StartCommand();
            await start.Execute(message, client);
        }
    }
}
