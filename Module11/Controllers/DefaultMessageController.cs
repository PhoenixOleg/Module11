using Module11.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Module11.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public DefaultMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            LogWriter.ConsoleLogger($"Контроллер {GetType().Name} получил сообщение", false);
                        
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: ct);
        }
    }
}
