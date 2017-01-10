using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Погода
{
    class Program
    {

        static Random rand = new Random();
        static int BufWidth = 0;
        static string Space = " ";
        static int Y = 12;
        static bool Выход = true;
        static int[][][] arr = new int[2017][][];
        
        static void Main()
        {
            NullAll(ref arr);

            int year = CheckedIntegerInput("Введите год: ", ">=0", 0, 2016);
            FillEmptyYear(ref arr, year);
            int month = HorizontalInterfaceSelection("Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Дякабрь");
            Console.Clear();

            while (Выход)
            {
                switch (PunktMenu(month, year) + 1)
                {
                    case 1:
                        {
                            Console.CursorTop = Y;
                            PrintMonth(arr[year][month]);
                            Y = Console.CursorTop + 1;
                        }
                        break;
                    case 2:
                        {
                            Console.CursorTop = Y;
                            double temp;
                            AverageTemp(arr[year][month], out temp);
                            Console.WriteLine("Самый длинный промежуток " + months[month] + " " + year + " года равен: " +temp);
                            Y = Console.CursorTop + 1;
                        }
                        break;
                    case 3:
                        {
                            Console.CursorTop = Y;
                            int heat, cold;
                            string daysheat, dayscold;
                            Heatest(arr[year][month], out heat, out daysheat);
                            Coldest(arr[year][month], out cold, out dayscold);
                            Console.WriteLine("Максимальная температура: "+ heat);
                            Console.WriteLine("Дни с максимальной температурой: "+ daysheat);
                            Console.WriteLine("Минимальная температура: " + cold);
                            Console.WriteLine("Дни с минимальной температурой: " + dayscold);
                            Y = Console.CursorTop + 2;
                        }
                        break;
                    case 4:
                        {
                            Console.CursorTop = Y;
                            Console.WriteLine("Самый длинный промежуток "+months[month]+" "+year+" года равен: "+LongPeriod(year, month));
                            Y = Console.CursorTop + 1;
                        }break;
                    case 5:
                        {
                            Console.Clear();
                            month = HorizontalInterfaceSelection("Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Дякабрь");
                            Console.Clear();
                        }break;
                    case 6:
                        {
                            Console.Clear();
                            Main();
                        }
                        break;
                    case 7:
                        {
                            Y = 12;
                            Console.Clear();
                        }
                        break;
                    case 8:
                        Выход = false;
                        break;
                        
                }
            }


        }


        static void DeleteRows(int StartPosition)
        {
            if (Console.BufferWidth != BufWidth)
            {// Проверка изменения ширины буфера консоли
                BufWidth = Console.BufferWidth;
                Space = "";
                for (int i = 0; i < BufWidth; i++)
                    Space += " ";
            }
            int StringQuantity = Console.CursorTop - StartPosition;
            if (StringQuantity <= 0) return;
            Console.CursorTop = StartPosition;
            // Удаление строк
            for (int j = 0; j < StringQuantity; j++)
                Console.Write(Space);
            Console.CursorTop = StartPosition;
        }

        static int HorizontalInterfaceSelection(params string[] items)
        {
            // Инициализация переменных
            int previousLenght = 0, currentLenght = 0;
            int currentIndex = 0, previousIndex = 0;
            int positionX = 5, positionY = Console.CursorTop + 1;
            bool itemSelected = false;

            if (items.Length == 0) return 0;

            // Начальная печать пунктов меню
            for (int i = 0; i < items.Length; i++)
            {
                Console.CursorLeft = positionX + previousLenght;
                Console.CursorTop = positionY;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[i]);
                previousLenght += items[i].Length + 1;
            }
            previousLenght = 0;
            do
            {
                // Печать предыдущего активного пункта основным                
                Console.CursorLeft = positionX + previousLenght;
                Console.CursorTop = positionY;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[previousIndex]);
                if (previousIndex < currentIndex)
                {
                    previousLenght += items[previousIndex].Length + 1;
                }
                else if (previousIndex > currentIndex)
                {
                    previousLenght -= items[currentIndex].Length + 1;
                }

                //Печать активного пункта
                if (previousIndex < currentIndex)
                {
                    currentLenght += items[previousIndex].Length + 1;
                }
                else if (previousIndex > currentIndex)
                {
                    currentLenght -= items[currentIndex].Length + 1;
                }
                Console.CursorLeft = positionX + currentLenght;
                Console.CursorTop = positionY;
                Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(items[currentIndex]);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                previousIndex = currentIndex;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        currentIndex++;
                        break;
                    case ConsoleKey.LeftArrow:
                        currentIndex--;
                        break;
                    case ConsoleKey.Enter:
                        itemSelected = true;
                        break;
                }
                // Избегание выхода за границы 
                if (currentIndex == items.Length)
                    currentIndex = items.Length - 1;
                else if (currentIndex < 0)
                    currentIndex = 0;
            }
            while (!itemSelected);

            Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorLeft = 0;
            Console.CursorTop += 2;

            return currentIndex;
        }

        static int CheckedIntegerInput(string OutputMessage, string type = "<=>0", int min = 0, int max = 1000)
        {
            bool input = false;
            int Number = 0, StartPosition = Console.CursorTop;
            if (type == "<=>0")
            {
                do
                {
                    Console.Write(OutputMessage);
                    input = Int32.TryParse(Console.ReadLine(), out Number);
                    if (!input)
                        Console.WriteLine("Введите целое число!");
                } while (!input);
            }
            else if (type == "=>0" || type == ">=0")
            {
                do
                {
                    Console.Write(OutputMessage);
                    input = Int32.TryParse(Console.ReadLine(), out Number);
                    if (!input || Number < min || Number > max)
                    {
                        Console.WriteLine("Введите целое положительное число в диапозоне от " + min + " до " + max + "!");
                        input = false;
                    }
                } while (!input);
            }
            DeleteRows(StartPosition);
            return Number;
        }

        static string[] months = { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };

        public static int PunktMenu(int num, int year)   // Функция выбора пункта меню 
        {
            // Описание переменных и массивов для программы вывода меню

            int currentIndex = 0, previousIndex = 0, i;
            int positionX = 5, positionY = 2;
            bool itemSelected = false;
           

            // Программа вывода меню 

            string[] items = { "1. Показать температуру каждого дня", "2. Показать среднюю температуру",
                "3. Показать самый холодный и теплый день",
                "4. Показать самый длительный промежуток отрицательной температуры",
                 "5. Выбрать новый месяц","6. Выбрать новый год и месяц" ,"7. Очистить", "8. Выход" };
            Console.WriteLine("Выбран "+months[num]+" "+year+ " года");

            //Начальный вывод пунктов меню.
            for (i = 0; i < items.Length; i++)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + i;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[i]);
            }

            do
            {
                // Вывод предыдущего активного пункта основным цветом.
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + previousIndex;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[previousIndex]);


                //Вывод активного пункта.
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + currentIndex;
                Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(items[currentIndex]);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                previousIndex = currentIndex;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        currentIndex++;
                        break;
                    case ConsoleKey.UpArrow:
                        currentIndex--;
                        break;
                    case ConsoleKey.Enter:
                        itemSelected = true;
                        break;
                }

                if (currentIndex == items.Length)
                    currentIndex = items.Length - 1;
                else if (currentIndex < 0)
                    currentIndex = 0;
            }
            while (!itemSelected);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
            return currentIndex;
        }

        public static void PrintMenu()   // Функция вывода меню 
        {
            Console.Clear();
            // Описание переменных и массивов для программы вывода меню

            int i;
            int positionX = 5, positionY = 2;


            // Программа вывода меню 

            string[] items = { "Вывести температуры всех дней", "Вывести среднюю температуру", "Вывести самый холодный и теплый день" };
            
            //Начальный вывод пунктов меню.
            for (i = 0; i < items.Length; i++)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + i;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[i]);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
        }

        static int LongPeriod(int year, int month)
        {
            int Length = 0, maxLength = 0;
            for (int i = 0; i < arr[year][month].Length; i++)
            {
                if (arr[year][month][i] < 0)
                    Length++;
                else Length = 0;
                if (Length > maxLength)
                    maxLength = Length;
            }
            return maxLength;
        }

        static void PrintMonth(int[] month)
        {
            for (int i = 0; i < 28; i++)
            {
                Console.Write("{0,3}", i + 1);
                if ((i + 1) % 7 == 0)
                {
                    Console.WriteLine();
                    for (int j = i - 6; j < i + 1; j++)
                        Console.Write("{0,3}", month[j]);
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            if (month.Length > 28)
            {
                for (int i = 28; i < month.Length; i++)
                    Console.Write("{0,3}", i + 1);
                Console.WriteLine();
                for (int i = 28; i < month.Length; i++)
                    Console.Write("{0,3}", month[i]);

            }


        }

        static void Heatest(int[] month, out int temp, out string days)
        {
            days = "1";
            temp = month[0];

            for (int i = 1; i < month.Length; i++)
            {
                if (temp < month[i])
                {
                    days = (i + 1).ToString();//создание новой строки с днями
                    temp = month[i];
                }
                else if (temp == month[i])
                    days += " " + (i + 1).ToString(); //добавление еще одного дня в список
            }
        }

        static void Coldest(int[] month, out int temp, out string days)
        {
            temp = month[0];
            days = "1";

            for (int i = 1; i < month.Length; i++)
            {
                if (temp > month[i])
                {
                    days = (i + 1).ToString();//создание новой строки с днями
                    temp = month[i];
                }
                else if (temp == month[i])
                {
                    days += ", " + (i + 1).ToString();//добавление еще одного дня в список
                }
            }
        }

        static void AverageTemp(int[] month, out double temp)
        {
            double sum = 0;
            for (int i = 0; i < month.Length; i++)
                sum += month[i];
            temp = (sum / month.Length);
        }
        
        private static void NullAll(ref int[][][] yearsData)
        {
            for (int i = 0; i < yearsData.Length; i++)
                yearsData[i] = null;
        }

        private static void FillEmptyYear(ref int[][][] yearsData, int year)
        {
            if (yearsData[year] == null)
                yearsData[year] = RandomiseYear();
        }

        private static int[][] RandomiseYear()
        {
            int[][] array = new int[12][];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = RandomiseMonth(i + 1);
            }
            return array;
        }

        private static readonly Random Rand = new Random();

        private static int[] RandomiseMonth(int month)
        {
            int[] array = null;

            #region declaration

            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    array = new int[31];
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    array = new int[30];
                    break;
                case 2:
                    array = new int[28];
                    break;
            }

            #endregion

            #region Temperatures

            int RMin = -50, RMax = 50;

            switch (month)
            {
                case 12:
                case 1:
                case 2:
                    RMin = -50;
                    RMax = 0;
                    break;
                case 3:
                case 4:
                case 5:
                    RMin = -5;
                    RMax = 25;
                    break;
                case 6:
                case 7:
                case 8:
                    RMin = 15;
                    RMax = 50;
                    break;
                case 9:
                case 10:
                case 11:
                    RMin = -20;
                    RMax = 15;
                    break;
            }

            #endregion


            for (int i = 0; i < array.Length; i++)
                array[i] = Rand.Next(RMin, RMax + 1);

            return array;
        }

    }
}
