﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public List<Checker> CheckersSet = new List<Checker>();
        public Board Board { get; set; }
        public Move Move { get; set; }
        public Move Enemies { get; set; }
        public IUserInput Player1;
        public IUserInput Player2;
        public IUserInput CurrentPlayer { get; set; }
        

        public void Start()
        {
            Player1 = new HumanPlayer(true);
            Player2 = new HumanPlayer(false);
            CurrentPlayer = Player1;

            //CreateCheckers(false);
            //CreateCheckers(true);
            
            CheckersSet.Add(new Checker(true, false, 3, 4));
            CheckersSet.Add(new Checker(false, false, 4, 5));
            
            //CheckersSet.Add(new Checker(true, false, 2, 1));
            //CheckersSet.Add(new Checker(true, false, 4, 3));
            //CheckersSet.Add(new Checker(false, false, 2, 3));
            //CheckersSet.Add(new Checker(false, false, 2, 5));
            
            Board = new Board();

            Board.Draw(CheckersSet);
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

        public int GetCheckerId(Coordinate coordinate)
        {
            return (from checker in CheckersSet
                    where coordinate.CellAddress[0] == checker.CoordHorizontal && coordinate.CellAddress[1] == checker.CoordVertical
                    select CheckersSet.IndexOf(checker)).FirstOrDefault();
        }

        public bool CanSelectChecker(int currentCheckerId)
        {
            return ((CurrentPlayer.PlaysWhites && CheckersSet[currentCheckerId].IsWhite) || (!CurrentPlayer.PlaysWhites && !CheckersSet[currentCheckerId].IsWhite));
        }

        public bool CanMoveThere(int[] adressOld, int[] adressNew, bool isQueen)
        {
            if (isQueen)
            {
                return (Board.IsCellEmpty(adressNew[0], adressNew[1]) && Board.IsCellUsable(adressNew[0], adressNew[1]));
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


        public IUserInput SwitchPlayer()
        {
            return CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((!checker.IsWhite && checker.CoordHorizontal == 7) || (checker.IsWhite && checker.CoordHorizontal == 0))
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
            Move.Coordinates.Add(new Coordinate(adressOld));

            //вставить проверку на возможность движения в рамках текущего хода
            int[] adressNew = GetCellAddress(selectDestination);
            Move.Coordinates.Add(new Coordinate(adressNew));
        }

        public void MoveChecker()
        {
            var currentCheckerId = GetCheckerId(Move.Coordinates[0]);

            var moves = Move.Coordinates.Count;

            for (var i = 1; i < moves; i++)
            {
                var coordinateOld = Move.Coordinates[0];
                var coordinateNew = Move.Coordinates[1];

                CheckersSet[currentCheckerId].CoordHorizontal = coordinateNew.CellAddress[0];
                CheckersSet[currentCheckerId].CoordVertical = coordinateNew.CellAddress[1];

                var cell = Board.GetCell(coordinateOld.CellAddress[0], coordinateOld.CellAddress[1]);
                cell.IsEmpty = true;

                CheckerBecomesQueen(CheckersSet[currentCheckerId]);

                Move.Coordinates.RemoveAt(0);

                //Thread.Sleep(500);
                Console.SetCursorPosition(0, 0);

                Board.Draw(CheckersSet);
            }
            CurrentPlayer = SwitchPlayer();
        }

        public Move GetEnemyCoordinates(Coordinate currentCoordinate)
        {
            var end = 1;
            int[][] direction =
            {
                new [] {-1, -1},
                new [] {-1,  1},
                new [] {1,  -1},
                new [] {1,   1}
            };

            var moveDirection = new int[4][];

            int id = GetCheckerId(currentCoordinate);

            Enemies = new Move();

            if (CheckersSet[id].IsQueen)
            {
                end = 7;
            }

            for (var i = 0; i < 4; i++)
            {
                for (var j = 1; j <= end; j++)
                {
                    moveDirection[i] = new[] { currentCoordinate.CellAddress[0] + j * direction[i][0], currentCoordinate.CellAddress[1] + j * direction[i][1] };

                    foreach (var checker in CheckersSet)
                    {
                        if (Board.CellExists(moveDirection[i]) &&
                            moveDirection[i][0] == checker.CoordHorizontal &&
                            moveDirection[i][1] == checker.CoordVertical)
                        {
                            if (CurrentPlayer.PlaysWhites == checker.IsWhite)
                            {
                                j = end + 1;
                                break;
                            }

                            Enemies.Coordinates.Add(new Coordinate(moveDirection[i][0], moveDirection[i][1]));
                        }
                    }
                }
            }
            return Enemies;
        }

        public bool CanTake()
        {
            return true; // checker calls this method. We iterate through its Enemies to find, if there is an empty cell behind the enemy.
        }
    }
}