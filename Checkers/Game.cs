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

        public void MakeMove()
        {
            const int delayNpcMoveMiliseconds = 500;
            const string selectCheckerToMoveMessage =   "Выберите шашку (например, B6): ";
            const string selectDestination =            "Целевая клетка (например, С5): ";
            const string cantSelectError =              "Нельзя ходить чужой шашкой!";
            const string cantMoveHereMessage =          "Нельзя ходить в выбранную клетку!";

            DrawWhiteLine();
            Console.Write("Ходят {0}!", CurrentPlayer.IsWhite ? "белые" : "черные");

            Thread.Sleep(4 * delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 30);
            
            int[] adressOld = SelectCell(selectCheckerToMoveMessage);
            
            int currentCheckerId = SelectedCheckerId(adressOld);

            while (!CanSelectChecker(currentCheckerId))
            {
                DrawWhiteLine();
                Console.Write(cantSelectError);
                Thread.Sleep(2 * delayNpcMoveMiliseconds);
                DrawWhiteLine();

                adressOld = SelectCell(selectCheckerToMoveMessage);
                currentCheckerId = SelectedCheckerId(adressOld);
            }

            int[] adressNew = SelectCell(selectDestination);

            while (!CanMoveThere(adressNew))
            {
                DrawWhiteLine();
                Console.Write(cantMoveHereMessage);
                adressNew = SelectCell(selectDestination); 
            }

                CheckersSet[currentCheckerId].HorizontalCoord = adressNew[0];
                CheckersSet[currentCheckerId].VerticalCoord = adressNew[1];
                CheckerBecomesQueen(CheckersSet[currentCheckerId]);
            
            Thread.Sleep(delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 0);
            board.Draw(CheckersSet);
            CurrentPlayer = SelectCurrentPlayer();
        }

        private void DrawWhiteLine()
        {
            const string whiteLine = "                                           ";
            Console.SetCursorPosition(0, 30);
            Console.Write(whiteLine);
            Console.SetCursorPosition(0, 30);
        }

        public int[] SelectCell(string message)
        {
            int[] adress;
            do
            {
                DrawWhiteLine();
                Console.Write(message);
                adress = CurrentPlayer.InputCheckerAdress();

            } while (adress[0] < 0 || adress[1] < 0 || adress[0] > 7 || adress[1] > 7);

            return adress;
        }

        public int SelectedCheckerId(int[] adress)
        {
            return (from checker in CheckersSet
                where adress[0] == checker.HorizontalCoord && adress[1] == checker.VerticalCoord
                select CheckersSet.IndexOf(checker)).FirstOrDefault();
        }

        public bool CanSelectChecker(int currentCheckerId)
        {
            return ((CurrentPlayer == player1 && CheckersSet[currentCheckerId].IsWhite) || (CurrentPlayer == player2 && !CheckersSet[currentCheckerId].IsWhite));
        }

        public bool CanMoveThere(int[] adressNew)
        {
            return (board.IsEmpty(adressNew[0], adressNew[1]) && board.IsUsable(adressNew[0], adressNew[1]));
        }

        public Player SelectCurrentPlayer()
        {
            return CurrentPlayer == player1 ? player2 : player1;
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((checker.IsWhite && checker.HorizontalCoord == 7) || (!checker.IsWhite && checker.HorizontalCoord == 0))
            {
                checker.IsQueen = true;
                checker.ChageSymbol();
            }
        }

        public bool GameIsOver()
        {
            var noWhites = CheckersSet.Count(checker => checker.IsWhite) == 0;
            var noBlacks = CheckersSet.Count(checker => checker.IsWhite == false) == 0;

            return (noWhites || noBlacks);
        }
    }
}