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
            Console.Write("Ходят {0}!", CurrentPlayer.PlaysWhites ? "белые" : "черные");

            Thread.Sleep(4 * delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 30);
            
            var adressOld = GetCellAddress(selectCheckerToMoveMessage);
            var currentCheckerId = GetCheckerId(adressOld);

            while (!CanSelectChecker(currentCheckerId))
            {
                DrawWhiteLine();
                Console.Write(cantSelectError);
                Thread.Sleep(2 * delayNpcMoveMiliseconds);
                DrawWhiteLine();

                adressOld = GetCellAddress(selectCheckerToMoveMessage);
                currentCheckerId = GetCheckerId(adressOld);
            }

            int[] adressNew = GetCellAddress(selectDestination);

            while (!CanMoveThere(adressNew))
            {
                DrawWhiteLine();
                Console.Write(cantMoveHereMessage);
                adressNew = GetCellAddress(selectDestination); 
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

        public int[] GetCellAddress(string message)
        {
            int[] cellAdress;
            do
            {
                DrawWhiteLine(); Console.Write(message);

                var validInput = CurrentPlayer.ValidateUserInput();
                    cellAdress = CurrentPlayer.ConvertInputToCoordinates(validInput);

            } while (cellAdress[0] < 0 || cellAdress[1] < 0 || cellAdress[0] > 7 || cellAdress[1] > 7);

            return cellAdress;
        }

        public int GetCheckerId(int[] adress)
        {
            return (from checker in CheckersSet
                        where adress[0] == checker.HorizontalCoord && adress[1] == checker.VerticalCoord
                            select CheckersSet.IndexOf(checker)).FirstOrDefault();
        }

        public bool CanSelectChecker(int currentCheckerId)
        {
            return ((CurrentPlayer == player1 && CheckersSet[currentCheckerId].IsWhite) || (CurrentPlayer == player2 && !CheckersSet[currentCheckerId].IsWhite));
        }

        public bool CanMoveThere(int[] adress)
        {
            return (board.CellIsEmpty(adress[0], adress[1]) && board.CellIsUsable(adress[0], adress[1]));
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
                checker.ChangeSymbol();
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