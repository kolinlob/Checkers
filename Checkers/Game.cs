using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> checkersSet = new List<Checker>();
        private Board board;

        public void Start()
        {
            board = new Board();

            CreateCheckers("whites");
            CreateCheckers("blacks");
            board.Draw(checkersSet);

            while (true)
                MakeMove(board);
        }

        private void CreateCheckers(string color = "whites")
        {
            int start = 0;
            int end = 3;
            bool isWhite = true;

            if (color == "blacks")
            {
                start = 5;
                end = 9;
                isWhite = false;
            }

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                        checkersSet.Add(new Checker(isWhite, false, i, j));
                }
            }
        }

        public void MakeMove(Board board)
        {
            Console.SetCursorPosition(0, 28);
            Console.Write("\r\nВыберите шашку (например, B6): ");
            int[] adressOld = SelectCell();
            int selectedRowOld = adressOld[0];
            int selectedColOld = adressOld[1];

            Console.Write("Целевая клетка (например, С5): ");
            foreach (var checker in checkersSet)
            {
                if (selectedRowOld == checker.horizontalCoord && selectedColOld == checker.verticalCoord)
                {
                    int[] adressNew = SelectCell();
                    if (CanMove(adressNew))
                    {
                        checker.horizontalCoord = adressNew[0];
                        checker.verticalCoord = adressNew[1];
                    }
                    else
                    {
                        Console.WriteLine("Нельзя ходить в выбранную клетку!");
                        //return;
                    }
                }
            }

            const int delayNpcMoveMiliseconds = 500;
            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);
            board.Draw(checkersSet);

            Console.SetCursorPosition(0, 28);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 45; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private bool CanMove(int[] adressNew)
        {
            return (board.IsEmpty(adressNew[0], adressNew[1])); // || board.IsUsable(adressNew[0], adressNew[1]));
        }

        public int[] SelectCell()
        {
            Encoding ascii = Encoding.ASCII;
            string input = Console.ReadLine();
            while (input == null || !IsOfCorrectLength(input))
            {
                const string incorrectInputError = "Некорректный ввод. Повторите попытку: ";
                Console.Write(incorrectInputError);

                input = Console.ReadLine();
            }

            Byte[] encodedBytes = ascii.GetBytes(input.ToUpper());

            string firstChar = Convert.ToString(encodedBytes[0]);
            string secondChar = Convert.ToString(encodedBytes[1]);

            int columnCode = Convert.ToInt32(firstChar);
            int rowCode = Convert.ToInt32(secondChar);

            int selectedCheckerCol = columnCode - 65;
            int selectedCheckerRow = 56 - rowCode;

            if (selectedCheckerCol < 0 || selectedCheckerCol > 7 || selectedCheckerRow < 0 || selectedCheckerRow > 7)
            {
                SelectCell();
            }
            int[] adress = { selectedCheckerRow, selectedCheckerCol };
            return adress;
        }

        private bool IsOfCorrectLength(string input)
        {
            return (input.Length == 2);
        }
    }
}