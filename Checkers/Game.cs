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

            CreateCheckersSet("whites");
            CreateCheckersSet("blacks");

            board.Draw(checkersSet);

            while (true)
                MakeMove(board);
        }



        private void CreateCheckersSet(string color = "whites")
        {
            int start = 0;
            int end = 3;
            bool isWhite = true;

            switch (color)
            {
                case "blacks":
                    {
                        start = 5;
                        end = 9;
                        isWhite = false;
                        break;
                    }
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


        private void MakeMove(Board board)
        {
            Console.Write("\r\nID шашки: ");
            int selectedCheckerId = GetInt();


            Console.Write("строка, куда ходить: ");
            checkersSet[selectedCheckerId].horizontalCoord = GetInt();

            Console.Write("колонка, куда ходить: ");
            checkersSet[selectedCheckerId].verticalCoord = GetInt();

            const int delayNpcMoveMiliseconds = 1500;
            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);

            board.Draw(checkersSet);
        }

        private int GetInt()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        //private int SelectCheckerCol()
        //{
        //    Encoding ascii = Encoding.ASCII;
        //    string input = Console.ReadLine().ToUpper();
        //
        //    Byte[] encodedBytes = ascii.GetBytes(input);
        //    int row = Convert.ToInt32(encodedBytes);
        //    int selectedCheckerCol = row - row;
        //
        //    return selectedCheckerCol;
        //}
        //
        //private int SelectCheckerRow()
        //{
        //    return GetInt();
        //}
    }
}