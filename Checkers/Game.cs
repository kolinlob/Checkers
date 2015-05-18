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
        public Move Move { get; set; }
        private IUserInput player1;
        private IUserInput player2;
        public IUserInput CurrentPlayer { get; set; }

        public void Start()
        {
            player1 = new HumanPlayer(true);
            player2 = new HumanPlayer(false);
            CurrentPlayer = player1;

            CreateCheckers(false);
            CreateCheckers(true);
            board = new Board();

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
                    select CheckersSet.IndexOf(checker)).First();
        }

        public bool CanSelectChecker(int currentCheckerId)
        {
            return ((CurrentPlayer.PlaysWhites && CheckersSet[currentCheckerId].IsWhite) || (!CurrentPlayer.PlaysWhites && !CheckersSet[currentCheckerId].IsWhite));
        }

        public bool CanMoveThere(int[] adressOld, int[] adressNew, bool isQueen)
        {
            if (isQueen)
            {
                return (board.IsCellEmpty(adressNew[0], adressNew[1]) && board.IsCellUsable(adressNew[0], adressNew[1]));
            }
            return OneCellMove(adressOld, adressNew) && MoveForward(adressOld, adressNew);
        }

        public bool MoveForward(int[] adressOld, int[] adressNew)
        {
            return ((adressNew[0] - adressOld[0]) == 1 && Math.Abs(adressNew[1] - adressOld[1]) == 1);
        }

        public bool OneCellMove(int[] adressOld, int[] adressNew)
        {
            return (Math.Abs(adressNew[0] - adressOld[0]) == 1 && Math.Abs(adressNew[1] - adressOld[1]) == 1);
        }

        public bool CellExist(int[] adress)
        {
            return adress[0] < 0 || adress[0] > 7 || adress[1] < 0 || adress[1] > 7;

        }

        public Move SetEnemyCoordinates(int[] playerCheckerAdress, bool playerColor, bool Queen)
        {
            //            int end = 1;
            //            if (Queen)
            //            {
            //                end = 7;
            //            }
            //
            //            int[][] moveOptions = new int[4][];
            //
            //            int[][] direction =
            //            {
            //                new [] {-1, -1},
            //                new [] {-1,  1},
            //                new [] {1,  -1},
            //                new [] {1,   1}
            //            };
            //
            //            Move Enemy = new Move();
            //
            //
            //
            //            for (int j = 0; j < 4; j++)
            //            {
            //               
            //
            //
            //            for (int i = 1; i <= end; i++)
            //            {
            //
            //                moveOptions[0] = new[] {playerCheckerAdress[0] - i, playerCheckerAdress[1] - i};
            //                moveOptions[1] = new[] {playerCheckerAdress[0] - i, playerCheckerAdress[1] + i};
            //                moveOptions[2] = new[] {playerCheckerAdress[0] + i, playerCheckerAdress[1] + i};
            //                moveOptions[3] = new[] {playerCheckerAdress[0] + i, playerCheckerAdress[1] - i};
            //                foreach (var checker in CheckersSet)
            //                {
            //                    if (moveOptions[0][0] == checker.HorizontalCoord &&
            //                        moveOptions[0][1] == checker.VerticalCoord && checker.IsWhite != playerColor)
            //                    {
            //                        Enemy.Coordinates.Add(moveOptions[0]);
            //                    }
            //                }
            //
            //            }
            //        }
            //
            //    return moveOptions.Any(option => CellExist(option) &&
            //                                             CheckersSet.Any(foe => foe.HorizontalCoord == option[0] &&
            //                                                             foe.VerticalCoord == option[1] &&
            //                                                             foe.IsWhite != playerColor));
            //
            Move Enemy = new Move();
            int end = 1;
            int[][] direction =
            {
                new [] {-1, -1},
                new [] {-1,  1},
                new [] {1,  -1},
                new [] {1,   1}
            };

            int[][] moveOptions = new int[4][];

            if (Queen)
            {
                end = 7;
            }
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 1; j <= end; j++)
                {

                    moveOptions[i] = new[] { playerCheckerAdress[0] + j * direction[i][0], playerCheckerAdress[1] + j * direction[i][0] };

                    foreach (var checker in CheckersSet)
                    {
                        if (CellExist(moveOptions[i]) && moveOptions[i][0] == checker.HorizontalCoord &&
                            moveOptions[i][1] == checker.VerticalCoord && checker.IsWhite != playerColor)
                        {
                            Enemy.Coordinates.Add(moveOptions[i]);
                        }
                    }

                }


            }
            return Enemy;
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

        public bool QueenMove(int[] adressOld, int[] adressNew)
        {
            return (Math.Abs(adressNew[0] - adressOld[0]) == Math.Abs(adressNew[1] - adressOld[1]));

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

            Move = new Move();

            DrawWhiteLine();
            Console.Write("Ходят {0}!", CurrentPlayer.PlaysWhites ? "белые" : "черные");

            Thread.Sleep(4 * delayNpcMoveMiliseconds);
            Console.SetCursorPosition(0, 30);

            var adressOld = GetCellAddress(selectCheckerToMoveMessage);
            Move.Coordinates.Add(adressOld);

            //вставить проверку на возможность движения в рамках текущего хода
            int[] adressNew = GetCellAddress(selectDestination);
            Move.Coordinates.Add(adressNew);
        }

        public void MoveChecker()
        {
            int currentCheckerId = GetCheckerId(Move.Coordinates[0]);

            var moves = Move.Coordinates.Count;

            for (var i = 1; i < moves; i++)
            {
                var addressOld = Move.Coordinates[0];
                var addressNew = Move.Coordinates[1];

                CheckersSet[currentCheckerId].HorizontalCoord = addressNew[0];
                CheckersSet[currentCheckerId].VerticalCoord = addressNew[1];

                var cell = board.GetCell(addressOld[0], addressOld[1]);
                cell.IsEmpty = true;

                CheckerBecomesQueen(CheckersSet[currentCheckerId]);

                Move.Coordinates.RemoveAt(0);

                //Thread.Sleep(500);
                Console.SetCursorPosition(0, 0);

                board.Draw(CheckersSet);
            }
            CurrentPlayer = SwitchPlayer();
        }
    }
}