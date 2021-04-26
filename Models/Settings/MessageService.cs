using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TG.Controllers;
using TG.Data;
using TG.Entity;
using TG.Interface;
using TG.Models.CallBacks;
using TG.Models.Commands;

namespace TG.Models.Settings
{
    public class MessageService :BackgroundService
    {
        static TelegramBotClient client = Bot.GetBotClient();
        static List<Command> list = AppSettings.GetCommand();
        IUserRepository repo;
        private IControllerService controllerService { get; set; }
        public MessageService(IUserRepository r,IControllerService service)
        {
            repo = r;
            controllerService = service;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {                   
                    client.StartReceiving();
                    client.OnMessage += OnMessageHandler;
                    client.OnCallbackQuery += async (object sc, CallbackQueryEventArgs ev) =>
                    {
                        var message = ev.CallbackQuery.Message;
                        if (ev.CallbackQuery.Data == "callback1")
                        {
                            await CallBack1.Execute(message, client);
                        }
                        else if (ev.CallbackQuery.Data == "callback2")
                        {
                            await CallBack2.Execute(message, client);
                        }
                        else if (ev.CallbackQuery.Data == "callback3")
                        {
                            await CallBack3.Execute(message, client);
                        }
                    };
                    Console.WriteLine("[Log]: Bot started");
                    Console.ReadLine();
                    client.StopReceiving();
                }
                catch (Exception)
                {
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
        public async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var commands = list;
            var message = e.Message;
            Logic logic = new Logic(repo,controllerService);          
            UserResponse user = await logic.Get(message.Chat.Id);
            if (message.Text == "/start" && (user == null))
            {
                await logic.StartAsync(message, client);
            }
            if (message.Text == "/register" && (user == null))
            {
                await logic.RegisterAsync(message, client);
            }
            if (user != null && user.Status == "Active")
            {
                foreach (var comm in commands)
                {
                    if (comm.Contains(message.Text))
                    {
                        await comm.Execute(message, client);
                    }
                }
            }
        }
    }
}

