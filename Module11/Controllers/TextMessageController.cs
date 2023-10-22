using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Module11.Logger;
using Module11.Services;

namespace Module11.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            LogWriter.ConsoleLogger($"Контроллер {GetType().Name} получил сообщение", false);
            if (Module11.Bot.IsError == true) { message.Text = "/start"; }

            switch (message.Text)
            {
                case "/start": //Начинаем работу бота
                    Module11.Bot.IsError = false;

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"sc"),
                        InlineKeyboardButton.WithCallbackData($" Сложение числел" , $"sum"),
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)                    
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Этот тренировочный бот может:{Environment.NewLine}1. подсчитать количество символов в сообщении{Environment.NewLine}2. найти сумму числе.</b> " +
                        $"{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    //Если установлен режим работы бота, разбираем ввод
                    string modeWork = _memoryStorage.GetSession(message.Chat.Id).ModeWork;

                    switch (modeWork)
                    {
                        case "sc":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина введенного сообщения - {StringOperations.GetLength(message.Text)} знаков.", cancellationToken: ct);
                            break;
                        case "sum":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел - {Summator.GetSum(message.Text, message.Chat.Id)}", cancellationToken: ct);
                            break;
                        default:
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Выберите режим работы бота.", cancellationToken: ct);
                            break;
                    }
                    break;
            }
        }
    }
}
