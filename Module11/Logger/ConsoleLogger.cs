using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Logger
{
    /// <summary>
    /// Класс логирования событий
    /// </summary>
    public class LogWriter
    {
        /// <summary>
        /// Статический метод вывода сообщений в консолько сервиса
        /// </summary>
        /// <param name="message">Сообщение для вывода</param>
        /// <param name="isError">Признак ошибки. Если true - сообщение выводится красным</param>
        public static void ConsoleLogger(string message, bool isError)
        {
            if (isError) { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine(message);
            if (isError) { Console.ForegroundColor = ConsoleColor.White; }
        }
    }
}
