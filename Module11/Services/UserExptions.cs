using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Module11.Services
{
    /// <summary>
    /// Пользовательское исколючение - Пустая строка
    /// </summary>
    public class EmptyStringExption : Exception
    {
        public EmptyStringExption(long chatId) { SetChatID(chatId); }

        public EmptyStringExption(string message, long chatId) : base(message) { SetChatID(chatId); }

        public EmptyStringExption(string message, long chatId, Exception inner) : base(message, inner) { SetChatID(chatId); } //Конструктор для обработки внутренних исключений

        /// <summary>
        /// Добавляем ID чата как допинформацию для возможности сообщить об ошибке пользователю в чат
        /// </summary>
        /// <param name="_chatId">ID чата</param>
        private void SetChatID(long _chatId)
        {
            this.Data.Add("ChatID", _chatId);
        }
    }

    /// <summary>
    /// Пользовательское исколючение - некорректная строка
    /// </summary>
    public class InvalidStringExption : Exception
    {
        public InvalidStringExption(long chatId) { SetChatID(chatId); }

        public InvalidStringExption(string message, long chatId) : base(message) { SetChatID(chatId); }

        public InvalidStringExption(string message, long chatId, Exception inner) : base(message, inner) { SetChatID(chatId); } //Конструктор для обработки внутренних исключений

        /// <summary>
        /// Добавляем ID чата как допинформацию для возможности сообщить об ошибке пользователю в чат
        /// </summary>
        /// <param name="_chatId">ID чата</param>
        private void SetChatID(long _chatId)
        {
            this.Data.Add("ChatID", _chatId);
        }
    }

    /// <summary>
    /// Пользовательское исколючение - Мало слагаемых (аргументов)
    /// </summary>
    public class TooLessArguments : Exception
    {
        public TooLessArguments(long chatId) { SetChatID(chatId); }

        public TooLessArguments(string message, long chatId) : base(message) { SetChatID(chatId); }

        public TooLessArguments(string message, long chatId, Exception inner) : base(message, inner) { SetChatID(chatId); } //Конструктор для обработки внутренних исключений

        /// <summary>
        /// Добавляем ID чата как допинформацию для возможности сообщить об ошибке пользователю в чат
        /// </summary>
        /// <param name="_chatId">ID чата</param>
        private void SetChatID(long _chatId)
        {
            this.Data.Add("ChatID", _chatId);
        }
    }
}
