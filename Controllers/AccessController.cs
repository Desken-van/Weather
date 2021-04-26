using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TG.Interface;
using TG.Data;
using TG.Models;
using TG.Models.Commands;
using Telegram.Bot;
using TG.Models.Settings;

namespace TG.Controllers
{
    public class AccessController : Controller
    {
        List<Command> commands = AppSettings.GetCommand();
        TelegramBotClient botClient = Bot.GetBotClient();
        private IControllerService controllerService { get; set; }
        IUserRepository repo;
        public AccessController(IUserRepository r,IControllerService service)
        {
            repo = r;
            controllerService = service;
        }
        [HttpPost("/Create")]
        public async Task<IActionResult> Create(string firstname, string lastname, string username, string tgid)
        {
            if (firstname == null || username == null || tgid.Length != 9)
            {
                return BadRequest(new { errorText = "Invalid data" });
            }
            else
            {
                IEnumerable<User> bigdata = await repo.GetUserList();
                foreach (User s in bigdata)
                {
                    if (username == s.Username)
                    {
                        return BadRequest("We already have this user");
                    }
                }
                await repo.Add(firstname,lastname,username,Convert.ToInt64(tgid));
                return Json(Ok());
            }
        }
        [HttpPost("/Status")]
        public async Task<IActionResult> Status(int id)
        {
            User user = await repo.GetUser(id);
            if (user == null)
            {
                return BadRequest("We havent this user");
            }
            if (user.Status == "Active")
            {
                user.Status = "Blocked";
                await repo.Update(user);
                controllerService.StatusNotif(id, botClient);
                return Ok("User Blocked");
            }
            else
            {
                user.Status = "Active";
                await repo.Update(user);
                controllerService.StatusNotif(id, botClient);
                return Ok("User Activate");
            }
        }
        [HttpPost("/Userlist")]
        public async Task<IActionResult> UserList(string sort,string range)
        {
            IEnumerable<User> list = await repo.GetUserList();
            var listusers = list.Select(x => new UserResponse
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.Username,
                Status = x.Status
            });
            IEnumerable<UserResponse> sorted = controllerService.SortList(listusers, sort, range);
            var response = new
            {
                sorted
            };
            return Json(response);
        }       
        [HttpPost("/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await repo.GetUser(id);
            if (user == null)
            {
                return BadRequest("We havent this user");
            }
            controllerService.DeleteNotif(id, botClient);
            await repo.Delete(id);            
            return Json(Ok());
        }
        [HttpPost("/Update")]
        public async Task<IActionResult> Update(int id, string newfirstname, string newlastname, string newusername)
        {
            User user = await repo.GetUser(id);
            if(newfirstname == null || newusername == null || user == null)
            {
                return BadRequest("You write invalid data");
            }
            user.FirstName = newfirstname;
            user.LastName = newlastname;
            user.Username = newusername;
            await repo.Update(user);
            controllerService.UpdateNotif(id, botClient);
            return Json(Ok());
        }
    }
}
