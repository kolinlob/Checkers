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

        public void Start()
        {
            var board = new Board();

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
            Console.Write("\r\n # исходной клетки (например, B6): ");
            int[] adressOld = SelectCell();
            int selectedRowOld = adressOld[0];
            int selectedColOld = adressOld[1];

            Console.Write("\r\n # целевой клетки (например, С5): ");
            foreach (var checker in checkersSet)
            {
                if (selectedRowOld == checker.horizontalCoord && selectedColOld == checker.verticalCoord)
                {
                    int[] adressNew = SelectCell();
                    checker.horizontalCoord = adressNew[0];
                    checker.verticalCoord = adressNew[1];
                }
            }

            const int delayNpcMoveMiliseconds = 500;
            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);

            board.Draw(checkersSet);
        }

        public int GetInt()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public int[] SelectCell()
        {
            Encoding ascii = Encoding.ASCII;
            string input = Console.ReadLine().ToUpper();
            int row = 0;
            int col = 0;

            Byte[] encodedBytes = ascii.GetBytes(input);

            string firstChar = Convert.ToString(encodedBytes[0]);
            string secondChar = Convert.ToString(encodedBytes[1]);

            var canParse1 = Int32.TryParse(firstChar, out col);
            var canParse2 = Int32.TryParse(secondChar, out row);
            
            int selectedCheckerCol = col - 65;
            int selectedCheckerRow = 56 - row;

            int[] adress = { selectedCheckerRow, selectedCheckerCol };
            return adress;
        }
    }
}