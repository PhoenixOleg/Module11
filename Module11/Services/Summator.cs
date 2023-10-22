using Module11.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Services
{
    /// <summary>
    /// Статический класс реализующий суммирование чисел
    /// </summary>
    public static class Summator
    {
        /// <summary>
        /// Метод возвращает суммсу чисел, переданных через параметр message и разделенных пробелом
        /// </summary>
        /// <param name="message">Строка для обработки</param>
        /// <param name="chatId">ID чата с пользователем</param>
        /// <returns>Сумма чисел</returns>
        /// <exception cref="TooLessArguments">Порождаемое исключение - слишком мало слагаемых (менее 2)</exception>
        /// <exception cref="InvalidStringExption">Порождаемое исключение - неправильный формат строки или некорректный символ в строке</exception>
        public static double GetSum(string message, long chatId)
        {
            double sum = 0;
            ArrayList numList = new(StringParser(message, chatId));
            
            if (numList.Count < 2)
            {
                throw new TooLessArguments("Количество слагаемых должно быть не менее двух", chatId);
            }

            foreach (string sNum in numList)
            {
                if (double.TryParse(sNum, out double num))
                { sum += num; }
                else
                { throw new InvalidStringExption($"'{sNum}' является некорректным слагаемым", chatId); }
            }
            return sum;
        }

        /// <summary>
        /// Метод разбора строки на слагаемые
        /// </summary>
        /// <param name="str">Строка для обработки</param>
        /// <param name="chatId">ID чата с пользователем</param>
        /// <returns>ArrayList со списком слагаемых</returns>
        /// <exception cref="EmptyStringExption">Порождаемое исключение - пустая строка для обработки</exception>
        /// <exception cref="InvalidStringExption">Порождаемое исключение - неправильный формат строки или некорректный символ в строке</exception>
        private static ArrayList StringParser(string str, long chatId)
        {
            ArrayList list = new();
            int idx = -1;

            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    throw new EmptyStringExption("Получена пустая строка", chatId);
                }

                //Можно было бы использовать string[] subStr = str.Split(' ');
                StringBuilder stringBuilder = new();
                foreach (char c in str + " ") //Завершающий пробел добавляется, чтобы выделить крайнее слагаемое
                {
                    idx++;
                   
                    switch (c)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '.':
                            stringBuilder.Append(c);
                            break;
                        case ',':
                            stringBuilder.Append('.'); //не мучаем пользователя разделителями
                            break;
                        case '+': //игнорим
                            break;
                        case '-':
                            if (idx + 1 <= str.Length - 1)
                            {
                                switch (str[idx + 1])
                                {
                                    case '0':
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                    case '.':
                                    case ',':
                                        stringBuilder.Append(c); //признак отрицательного числа, если дальше идет цифра или разделить целой и дробной частей
                                        break;
                                    default:
                                        throw new InvalidStringExption($"Строка содержит недопустимый символ '{str[idx + 1]}' или неправильный формат строки", chatId);
                                }
                            }
                            break;
                        case ' ':
                            if (!string.IsNullOrEmpty(stringBuilder.ToString()))
                            {
                                list.Add(stringBuilder.ToString());
                            }
                            stringBuilder.Clear();                            
                            break;
                        default:
                            throw new InvalidStringExption($"Строка содержит недопустимый символ '{str[idx]}'", chatId);
                    }
                }
            }         
            return list;
        }
    }
}
