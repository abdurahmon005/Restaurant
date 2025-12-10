using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using static System.Formats.Asn1.AsnWriter;

namespace WebApp.Aplication.Services.Impl
{
    public class RestaurantTelegramBot : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITelegramBotClient _telegramBotClient;

        public RestaurantTelegramBot(IServiceScopeFactory serviceScopeFactory, ITelegramBotClient telegramBotClient)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _telegramBotClient = telegramBotClient;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{

        //    var me = await _telegramBotClient.GetMe();
        //    Console.WriteLine($"Bot : {me} is running ...");

        //    var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions
        //    {
        //        AllowedUpdates = Array.Empty<UpdateType>()
        //    };

        //    _telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, stoppingToken);
        //    await Task.Delay(-1, stoppingToken);

        //    throw new NotImplementedException();
        //}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _telegramBotClient.GetMe();
            Console.WriteLine($"Bot : {me} is running ...");
            var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            _telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, stoppingToken);
            await Task.Delay(-1, stoppingToken);
        }
        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            var myscope = _serviceScopeFactory.CreateScope();
            var dbcontext = myscope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (update.Type != UpdateType.Message)
            {
                return;
            }
            var message = update.Message!;
            var chatId = message.Chat.Id;
            if (message.Text == "/start")
            {
                var button = new KeyboardButton("📱 Share phone number") { RequestContact = true };
                var keyboard = new ReplyKeyboardMarkup(button)
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true,
                };
                await bot.SendMessage(chatId, "Welcome! Please share your phone number for verification.",
                replyMarkup: keyboard);

            }

            else if (message.Contact != null)
            {
                Console.WriteLine($"User send his/her number {message.Contact.PhoneNumber}");

                var numberExists = await dbcontext.Users.AnyAsync(x => x.PhoneNumber == message.Contact.PhoneNumber || x.PhoneNumber.Contains(message.Contact.PhoneNumber));
                if (numberExists)
                {
                    return;
                    /*
                    var existingUser = await dbcontext.Users.FirstAsync(x => x.PhoneNumber == message.Contact.PhoneNumber || x.PhoneNumber.Contains(message.Contact.PhoneNumber));
                    Random random = new Random();
                    string otpcode = random.Next(10000, 100000).ToString();
                    OtpCode newOtpCode = new OtpCode
                    {
                        Code = otpcode,
                        Status = OtpCodeStatus.Unverified,
                    };
                    await bot.SendMessage(chatId, $"Sizning bir martalik kodingiz: <b>{otpcode}</b>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    existingUser.OtpCodes.Add(newOtpCode);
                    dbcontext.Users.Update(existingUser);
                    await dbcontext.SaveChangesAsync();

                    */
                }
                else
                {
                    Random random = new Random();
                    string otpCode = random.Next(10000, 100000).ToString();

                    TempUser tempUser = new TempUser
                    {
                        PhoneNumber = message.Contact.PhoneNumber,
                    };
                    //OtpCodes newOtpCode = new OtpCode
                    //{
                    //    Code = otpCode,
                    //    Status = OtpCodeStatus.Unverified,
                    //};
                    UserOtps userOtps = new UserOtps
                    {
                        Code = otpCode
                    };


                }
            }
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}


/*
 

namespace ProfChemistry.Application.Services.Impl
{
    public class TelegramBotService : BackgroundService
    {
        private readonly IServiceScopeFactory _scope;
        private readonly ITelegramBotClient _botClient;
        public TelegramBotService(ITelegramBotClient botClient, IServiceScopeFactory scope)
        {
            _botClient = botClient;
            _scope = scope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _botClient.GetMe();
            Console.WriteLine($"Bot : {me} is running ...");
            var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, stoppingToken);
            await Task.Delay(-1, stoppingToken);
        }
        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            var myscope = _scope.CreateScope();
            var dbcontext = myscope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (update.Type != UpdateType.Message)
            {
                return;
            }
            var message = update.Message!;
            var chatId = message.Chat.Id;
            if (message.Text == "/start")
            {
                var button = new KeyboardButton("📱 Share phone number") { RequestContact = true };
                var keyboard = new ReplyKeyboardMarkup(button)
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true,
                };
                await bot.SendMessage(chatId, "Welcome! Please share your phone number for verification.",
                replyMarkup: keyboard);

            }

            else if (message.Contact != null)
            {
                Console.WriteLine($"User send his/her number {message.Contact.PhoneNumber}");

                var numberExists = await dbcontext.Users.AnyAsync(x => x.PhoneNumber == message.Contact.PhoneNumber || x.PhoneNumber.Contains(message.Contact.PhoneNumber));
                if (numberExists)
                {
                    return;
                    //var existingUser = await dbcontext.Users.FirstAsync(x => x.PhoneNumber == message.Contact.PhoneNumber || x.PhoneNumber.Contains(message.Contact.PhoneNumber));
                    //Random random = new Random();
                    //string otpcode = random.Next(10000, 100000).ToString();
                    //OtpCode newOtpCode = new OtpCode
                    //{
                    //    Code = otpcode,
                    //    Status = OtpCodeStatus.Unverified,
                    //};
                    //await bot.SendMessage(chatId, $"Sizning bir martalik kodingiz: <b>{otpcode}</b>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    //existingUser.OtpCodes.Add(newOtpCode);
                    //dbcontext.Users.Update(existingUser);
                    //await dbcontext.SaveChangesAsync();
                }
                else
                {
                    Random random = new Random();
                    string otpCode = random.Next(10000, 100000).ToString();

                    TempUser tempUser = new TempUser
                    {
                        PhoneNumber = message.Contact.PhoneNumber,
                    };
                    OtpCode newOtpCode = new OtpCode
                    {
                        Code = otpCode,
                        Status = OtpCodeStatus.Unverified,

                    }

                }
            }
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }

    }
}
*/