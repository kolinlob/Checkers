using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> CheckersSet = new List<Checker>();
        private Board board;
        //private Move move;
        private Player player1;
        private Player player2;
        public Player CurrentPlayer { get; set; }

        public void Start()
        {
            board = new Board();
            //move = new Move();
            player1 = new Player(true);
            player2 = new Player(false);
            CurrentPlayer = player1;

            CreateCheckers(true);
            CreateCheckers(false);
            board.Draw(CheckersSet);
        }

        public void CreateCheckers(bool isWhite)
        {
            var start = 0;
            var end = 3;

            if (!isWhite)
            {
                start = 5;
                end = 8;
            }

            for (var i = start; i < end; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                        CheckersSet.Add(new Checker(isWhite, false, i, j));
                }
            }
        }

        public bool CanMoveThere(int[] adressNew)
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
            
            Console.SetCursorPosition(0, 30);
            Console.Write("Ходят {0}!", CurrentPlayer.IsWhite ? "белые" : "черные");

            Thread.Sleep(2 * delayNpcMoveMiliseconds);
            

            const string selectCheckerToMoveMessage = "Выберите шашку (например, B6): ";
            const string selectDestination = "Целевая клетка (например, С5): ";
            const string cantSelectError = "Нельзя ходить чужой шашкой!";
            const string cantMoveHereMessage = "Нельзя ходить в выбранную клетку!";
            const string whiteLine = "                                           ";

            Console.SetCursorPosition(0, 30);
            int currentCheckerId = SelectCheckerToMove(selectCheckerToMoveMessage, whiteLine);

            while (!CanSelectChecker(currentCheckerId))
            {
                Console.SetCursorPosition(0, 30);
                Console.Write(whiteLine);
                Console.SetCursorPosition(0, 30);
                Console.Write(cantSelectError);

                Thread.Sleep(2 * delayNpcMoveMiliseconds);

                Console.SetCursorPosition(0, 0);
                board.Draw(CheckersSet);

                currentCheckerId = SelectCheckerToMove(selectCheckerToMoveMessage, whiteLine);
            }

            Console.Write(selectDestination);

            int[] adressNew = CurrentPlayer.SelectCell();

            if (CanMoveThere(adressNew))
            {
                CheckersSet[currentCheckerId].HorizontalCoord = adressNew[0];
                CheckersSet[currentCheckerId].VerticalCoord = adressNew[1];
                CheckerBecomesQueen(CheckersSet[currentCheckerId]);
            }
            else
            {
                Console.WriteLine(cantMoveHereMessage);
                Thread.Sleep(2 * delayNpcMoveMiliseconds);
                Console.SetCursorPosition(0, 0);
                board.Draw(CheckersSet);
            }

            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);
            board.Draw(CheckersSet);

            Console.SetCursorPosition(0, 30);
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(whiteLine);
            }

            CurrentPlayer = SelectCurrentPlayer();
        }

        public Player SelectCurrentPlayer()
        {
            return CurrentPlayer == player1 ? player2 : player1;
        }

        public int SelectCheckerToMove(string message, string whiteLine)
        {
            int[] adressOld;
            do
            {
                Console.SetCursorPosition(0,30);
                Console.Write(whiteLine);
                Console.SetCursorPosition(0,30);
                Console.Write(message);
                
                adressOld = CurrentPlayer.SelectCell();
            } while (adressOld[0] < 0 || adressOld[1] < 0 || adressOld[0] > 7 || adressOld[1] > 7);

            return (from checker in CheckersSet
                        where adressOld[0] == checker.HorizontalCoord && adressOld[1] == checker.VerticalCoord
                             select CheckersSet.IndexOf(checker)).FirstOrDefault();

            //foreach (var checker in checkersSet)
            //{
            //    if (adressOld[0] == checker.HorizontalCoord && adressOld[1] == checker.VerticalCoord)
            //    {
            //        currentCheckerId = checkersSet.IndexOf(checker);
            //        break;
            //    }
            //}
            //return currentCheckerId;
        }

        public bool CanSelectChecker(int currentCheckerId)
        {
            return ((CurrentPlayer == player1 && CheckersSet[currentCheckerId].IsWhite) || (CurrentPlayer == player2 && !CheckersSet[currentCheckerId].IsWhite));
        }

        public bool GameIsOver()
        {
            bool noWhites = CheckersSet.Count(checker => checker.IsWhite) == 0;
            bool noBlacks = CheckersSet.Count(checker => checker.IsWhite == false) == 0;

            return (noWhites || noBlacks);
        }
    }
}