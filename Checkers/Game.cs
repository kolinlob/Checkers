using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> CheckersSet = new List<Checker>();
        private Board board;
        private Move move;
        private IUserInput player1;
        private IUserInput player2;
        public IUserInput CurrentPlayer { get; set; }

        public void Start()
        {
            board = new Board();
            player1 = new HumanPlayer(true);
            player2 = new HumanPlayer(false);
            CurrentPlayer = player1;
            
            CreateCheckers(false);
            CreateCheckers(true);
            board.Draw(CheckersSet);
        }

        public void CreateCheckers(bool isWhite)
        {
            var start = 0;
            var end = 3;

            if (isWhite)
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

                var validInput = ValidateInput();
                    cellAdress = ConvertIntoCoordinates(validInput);

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

        public bool CanMoveThere(int[] adressOld, int[] adressNew, bool isQueen)
        {
            if (isQueen)
            {
                return (board.IsCellEmpty(adressNew[0], adressNew[1]) && board.IsCellUsable(adressNew[0], adressNew[1]));
            }
            
            
                bool isOneCellMovePossible;
                if (Math.Abs(adressNew[0] - adressOld[0]) == 1 && Math.Abs(adressNew[1] - adressOld[1]) == 1)
                {
                    isOneCellMovePossible = true;
                }
                else
                {
                    isOneCellMovePossible = false;
                }
                
                
                return (board.IsCellEmpty(adressNew[0], adressNew[1]) && board.IsCellUsable(adressNew[0], adressNew[1]) && isOneCellMovePossible);
            
        }

        public IUserInput SwitchPlayer()
        {
            return CurrentPlayer == player1 ? player2 : player1;
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((!checker.IsWhite && checker.HorizontalCoord == 7) || (checker.IsWhite && checker.HorizontalCoord == 0))
            {
                checker.IsQueen = true;
                checker.ChangeSymbol();
            }
        }

        public bool IsGameOver()
        {
            var noWhites = CheckersSet.Count(checker => checker.IsWhite) == 0;
            var noBlacks = CheckersSet.Count(checker => checker.IsWhite == false) == 0;

            return (noWhites || noBlacks);
        }

        public int[] ConvertIntoCoordinates(string validInput)
        {
            var ascii = Encoding.ASCII;
            var bytes = ascii.GetBytes(validInput.ToUpper());

            var row = 56 - Convert.ToInt32(bytes[1]);
            var col = Convert.ToInt32(bytes[0]) - 65;

            return new[] { row, col };
        }

        public string ValidateInput()
        {
            var rawInput = CurrentPlayer.InputCoordinates();

            while (rawInput == null || rawInput.Length != 2)
            {
                const string incorrectInputError = "Неверный ввод. Повторите: ";
                const string whiteLine = "                                           ";

                Console.SetCursorPosition(0, 30);
                Console.Write(whiteLine);

                Console.SetCursorPosition(0, 30);
                Console.Write(incorrectInputError);

                rawInput = CurrentPlayer.InputCoordinates();
            }
            var validInput = rawInput;

            return validInput;
        }

        public void SetCoordinatesForMove()
        {
            const int delayNpcMoveMiliseconds = 500;
            const string selectCheckerToMoveMessage = "Выберите шашку (например, B6): ";
            const string selectDestination = "Целевая клетка (например, С5): ";
            //const string cantSelectError = "Нельзя ходить чужой шашкой!";
            //const string cantMoveHereMessage = "Нельзя ходить в выбранную клетку!";

            move = new Move();

            DrawWhiteLine();
            Console.Write("Ходят {0}!", CurrentPlayer.PlaysWhites ? "белые" : "черные");

            Thread.Sleep(4*delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 30);

            var adressOld = GetCellAddress(selectCheckerToMoveMessage);
            move.MoveCoordinates.Add(adressOld);

            //for (int i = 0; i < 3; i++)
            //{
                int[] adressNew = GetCellAddress(selectDestination);
                move.MoveCoordinates.Add(adressNew);
          //  }     
        }      

        public void MoveChecker()
        {
            int currentCheckerId = GetCheckerId(move.MoveCoordinates[0]);

            var iterations = move.MoveCoordinates.Count;

            for (var i = 1; i < iterations; i++)
            {
                var addressNew = move.MoveCoordinates[1];

                CheckersSet[currentCheckerId].HorizontalCoord = addressNew[0];
                CheckersSet[currentCheckerId].VerticalCoord   = addressNew[1];
                
                CheckerBecomesQueen(CheckersSet[currentCheckerId]);

                move.MoveCoordinates.RemoveAt(0);

                Thread.Sleep(500);
                Console.SetCursorPosition(0, 0);

                board.Draw(CheckersSet); 
            }
            CurrentPlayer = SwitchPlayer();
        }
    }
}