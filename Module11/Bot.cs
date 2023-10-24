using Microsoft.Extensions.Hosting;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Module11.Configuration;
using Module11.Controllers;
using Module11.Logger;
using System.Reflection.Metadata.Ecma335;

namespace Module11
{
    public class Bot : BackgroundService
    {
        public static bool IsError = false;

        // Клиент к Telegram Bot API
        private readonly ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        private readonly InlineKeyboardController _inlineKeyboardController;
        private readonly TextMessageController _textMessageController;
        private readonly DefaultMessageController _defaultMessageController;

        public Bot(
            ITelegramBotClient telegramClient,
            InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController,
            DefaultMessageController defaultMessageController
            )
        {
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: cancellationToken);

            LogWriter.ConsoleLogger("Бот запущен", false);
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                LogWriter.ConsoleLogger($"Получено сообщение {update!.Message!.Text}", false);
               

                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }

         async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            ///Что я здесь творил и вытворял...
            ///Тестировал сложение 12 и a
            ///Т. к., видимо, из-за того, что метод перехвата ошибок из текста модуля 11 возвращает CompletedTask, то после перехвата ошибке сервис уходит "в закат", 
            ///а не перезапускается.
            ///Чтобы после ошибки бот продолжал работать:
            ///1. Сделал статический public флаг IsError (догадываюсь, что плохое решение)
            ///2. Здесь его устанавливаю в истину
            ///3. Этот метод я сделал ансинхронным и сделал повторный вызов ExecuteAsync
            ///4. Т. к. после возвращения к жизни TextMessageController снова получает строку, на которой парсинг строки ушел в ошибку
            ///делаю принудительно вызов "старта" => if (Module11.Bot.IsError == true) { message.Text = "/start"; }
            ///5. при отработке команды /start сбрасываю флаг на false
            ///Как правильно перезапустить асинхронный метод из документации пока не понял
            ///Можно было бы исключения парсинга строки ловить там же на месте, но если есть метод HandleErrorAsync, то по логике это и д. б. единственная точка отлова

            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            
            // Выводим в консоль информацию об ошибке
            LogWriter.ConsoleLogger(errorMessage, true);

            if (exception.Message.Contains("Строка содержит недопустимый символ") || 
                exception.Message.Contains("является некорректным слагаемым") || 
                exception.Message == "Получена пустая строка" ||
                exception.Message == "Количество слагаемых должно быть не менее двух"
                )
            {
                await _telegramClient.SendTextMessageAsync(exception?.Data?["ChatID"]?.ToString(), $"{exception!.Message}{Environment.NewLine}Подождите 10 сек - я приду в себя и попробуем еще раз", cancellationToken: cancellationToken);

                // Задержка перед повторным подключением
                LogWriter.ConsoleLogger("Ожидаем 10 секунд перед повторным подключением.", false);

                IsError = true;

                //Thread.Sleep(10000);
                await Task.Delay(10000, cancellationToken).ConfigureAwait(false);

                await ExecuteAsync(cancellationToken);

                return;
            }
            else
            {
                await HandleErrorAsyncOriginal();
            }
        }

        Task HandleErrorAsyncOriginal()
        {
            // Задержка перед повторным подключением
            LogWriter.ConsoleLogger("Ожидаем 10 секунд перед повторным подключением.", false);
            Thread.Sleep(10000);
            //Task.Delay(10000).ConfigureAwait(false);
            return Task.CompletedTask;
        }
    }
}
