using System;
using System.Text;

namespace Checkers
{
    public class Player
    {
        public Player(bool isWhite)
        {
            IsWhite = isWhite;
        }

        public bool IsWhite { get; set; }

        public int[] SelectCell()
        {
            var ascii = Encoding.ASCII;
            var input = InputCoordinate();

            Byte[] encodedBytes = ascii.GetBytes(input.ToUpper());

            int selectedCellCol = Convert.ToInt32(encodedBytes[0]) - 65;
            int selectedCellRow = 56 - Convert.ToInt32(encodedBytes[1]);

            int[] adress = { selectedCellRow, selectedCellCol };
            return adress;
        }

        private static string InputCoordinate()
        {
            var input = Console.ReadLine();
            while (input == null || input.Length != 2)
            {
                const string incorrectInputError = "Неверный ввод. Повторите: ";
                const string whiteLine = "                                           ";

                Console.SetCursorPosition(0, 30);
                Console.Write(whiteLine);
                
                Console.SetCursorPosition(0, 30);
                Console.Write(incorrectInputError);

                input = Console.ReadLine();
            }
            return input;
        }


        public int[] SelectCell(string input)
        {
            Encoding ascii = Encoding.ASCII;
            Byte[] encodedBytes = ascii.GetBytes(input.ToUpper());

            int selectedCellCol = Convert.ToInt32(encodedBytes[0]) - 65;
            int selectedCellRow = 56 - Convert.ToInt32(encodedBytes[1]);

            int[] adress = { selectedCellRow, selectedCellCol };
            return adress;
        }
    }
}