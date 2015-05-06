using System;
using System.Text;

namespace Checkers
{
    public class Player
    {
        public Player(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }

        public bool PlaysWhites { get; set; }


        public int[] ConvertInputToCoordinates(string validInput)
        {
            var ascii = Encoding.ASCII;
            var bytes = ascii.GetBytes(validInput.ToUpper());

            var row = 56 - Convert.ToInt32(bytes[1]);
            var col = Convert.ToInt32(bytes[0]) - 65;
            
            return new[] { row, col };
        }

        public string ValidateUserInput()
        {
            var rawInput = ReadUserInput();

            while (rawInput == null || rawInput.Length != 2)
            {
                const string incorrectInputError = "Неверный ввод. Повторите: ";
                const string whiteLine = "                                           ";
                
                Console.SetCursorPosition(0, 30);
                Console.Write(whiteLine);
                
                Console.SetCursorPosition(0, 30);
                Console.Write(incorrectInputError);

                rawInput = ReadUserInput();
            }

            var validInput = rawInput;

            return validInput;
        }

        private static string ReadUserInput()
        {
            return Console.ReadLine();
        }
    }
}