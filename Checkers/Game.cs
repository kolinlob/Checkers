using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> checkersSet = new List<Checker>();
        private Board board;
        private Move move;
        private Player player;



        public void Start()
        {
            this.board = new Board();
            this.move = new Move();
            this.player = new Player();

            CreateCheckers("whites");
            CreateCheckers("blacks");
            board.Draw(checkersSet);
        }

        public void CreateCheckers(string color = "whites")
        {
            int start = 0;
            int end = 3;
            bool isWhite = true;

            if (color == "blacks")
            {
                start = 5;
                end = 8;
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


        public bool CanMove(int[] adressNew)
        {
            return (board.IsEmpty(adressNew[0], adressNew[1]) && board.IsUsable(adressNew[0], adressNew[1]));
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((checker.IsWhite && checker.HorizontalCoord == 7) || (!checker.IsWhite && checker.HorizontalCoord == 0))
            {
                checker.IsQueen = true;
                checker.ChageSymbol();
            }
        }

        public void MakeMove()
        {
            const int delayNpcMoveMiliseconds = 500;
            Console.SetCursorPosition(0, 29);
            Console.Write("\r\nВыберите шашку (например, B6): ");

            int[] adressOld = player.SelectCell();
            int selectedRowOld = adressOld[0];
            int selectedColOld = adressOld[1];

            Console.Write("Целевая клетка (например, С5): ");
            foreach (var checker in checkersSet)
            {
                if (selectedRowOld == checker.HorizontalCoord && selectedColOld == checker.VerticalCoord)
                {
                    int[] adressNew = player.SelectCell();
                    if (CanMove(adressNew))
                    {
                        checker.HorizontalCoord = adressNew[0];
                        checker.VerticalCoord = adressNew[1];
                        CheckerBecomesQueen(checker);
                    }
                    else
                    {
                        Console.WriteLine("Нельзя ходить в выбранную клетку!");
                        Thread.Sleep(delayNpcMoveMiliseconds);
                    }
                }
            }

            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);
            board.Draw(checkersSet);

            Console.SetCursorPosition(0, 29);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 45; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public bool GameIsOver()
        {
            bool noWhites = checkersSet.Count(checker => checker.IsWhite) == 0;
            bool noBlacks = checkersSet.Count(checker => checker.IsWhite == false) == 0;

            return (noWhites || noBlacks);
        }
    }
}