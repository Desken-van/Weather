using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Interface;
using TG.Models;
using TG.Models.Commands;
using TG.Models.Settings;

namespace TG.Controllers
{
    [Route("api/message/update")]
    public class SendNotice : Controller
    {
        List<Command> commands = AppSettings.GetCommand();
        TelegramBotClient botClient = Bot.GetBotClient();
        private IControllerService controllerService { get; set; } 
        IUserRepository repo;
        public SendNotice(IUserRepository r , IControllerService service)
        {
            repo = r;
            controllerService = service;
        }
        [HttpPost]
        public async Task<IActionResult> Post(int id, string text)
        {
            Message message = await controllerService.GetMessage(id, text, commands, botClient);
                foreach (var command in commands)
                {
                    if (command.Contains(message.Text))
                    {
                        await command.Execute(message, botClient);
                        break;
                    }
                }
                return Ok();            
        }
    }
}
