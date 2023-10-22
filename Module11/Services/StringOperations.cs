using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Services
{
    /// <summary>
    /// Класс операций над строкой
    /// </summary>
    public static class StringOperations
    {
        /// <summary>
        /// Статический метод вычисления длины строки
        /// </summary>
        /// <param name="message">Обрабатываемая строка</param>
        /// <returns>Возвращаемое значение - длина строки в message</returns>
        public static int GetLength(string message)
        {
            return message.Length;
        }
    }
}
