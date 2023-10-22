using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Module11.Configuration;
using Module11.Logger;
using Module11.Services;

namespace Module11.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            LogWriter.ConsoleLogger($"Контроллер {GetType().Name} обнаружил нажатие на кнопку {callbackQuery.Data}",false);

            if (callbackQuery?.Data == null)
                return;

            //Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).ModeWork = callbackQuery.Data;

            // Генерим информационное сообщение
            string modeText = callbackQuery.Data switch
            {
                "sc" => "Подсчёт количества символов в тексте",
                "sum" => $"Вычисление суммы чисел{Environment.NewLine}Введите слагаемые через пробел",
                _ => "Нереализованный режим. Сообщите разработчику",
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Установлен режим  - {modeText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
