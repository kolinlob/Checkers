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
        public Board Board;
        public Move Move { get; set; }
        public Move Enemies { get; set; }
        public IUserInput Player1;
        public IUserInput Player2;
        public IUserInput CurrentPlayer { get; set; }

        public Dictionary<Checker, Move> PossibleTakes = new Dictionary<Checker, Move>();

        public List<Checker> CheckersWithTakes;
        public List<Checker> BlockedCheckers;

        public void Start()
        {
            Player1 = new HumanPlayer(true);
            Player2 = new HumanPlayer(false);
            CurrentPlayer = Player1;

            //CreateCheckers(false);
            //CreateCheckers(true);

            //CheckersSet.Add(new Checker(true, false, 3, 4)); // CHECKER WE TEST
            CheckersSet.Add(new Checker(true, false, 6, 7));
            CheckersSet.Add(new Checker(false, false, 4, 5));
            CheckersSet.Add(new Checker(false, false, 5, 6));
            CheckersSet.Add(new Checker(true, false, 7, 0));
            CheckersSet.Add(new Checker(false, false, 6, 1));

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

        private void ClearMessageBar()
        {
            Console.SetCursorPosition(50, 3); Console.Write("                              ");
            Console.SetCursorPosition(50, 3);
        }

        private void DisplayCurrentPlayerMessage()
        {
            Console.SetCursorPosition(50, 1); Console.Write("                     ");
            Console.SetCursorPosition(50, 1); Console.Write("{0} move!", CurrentPlayer.PlaysWhites ? "White" : "Black");
            Console.SetCursorPosition(50, 3);
        }

        public int[] GetCellAddress(string message)
        {
            int[] cellAdress;
            var counter = 0;
            do
            {
                ClearMessageBar();
                if (counter != 0)
                {
                    ClearMessageBar();
                    Console.Write("Error. Incorrect input.");
                    Thread.Sleep(1000);
                    ClearMessageBar();
                }

                Console.Write(message);
                var validInput = ValidateInput();
                cellAdress = ConvertIntoCoordinates(validInput);
                counter++;

            } while (cellAdress[0] < 0 || cellAdress[1] < 0 || cellAdress[0] > 7 || cellAdress[1] > 7);

            return cellAdress;
        }

        public int GetCheckerId(Coordinate coordinate)
        {
            try
            {
                return (from checker in CheckersSet
                        where
                            coordinate.CellAddress[0] == checker.CoordHorizontal &&
                            coordinate.CellAddress[1] == checker.CoordVertical
                        select CheckersSet.IndexOf(checker)).Single();
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        public bool CanSelectChecker(Checker checker) //надо добавить проверку, что у шашки есть куда ходить, иначе её нельзя выбирать
        {
            try
            {
                var isOwnChecker = (CurrentPlayer.PlaysWhites == checker.IsWhite);

                if (!isOwnChecker)
                    return false;
                if (CheckersWithTakes.Count > 0)
                    return CheckersWithTakes.Contains(checker);

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public void FindCheckersWithTakes()
        {
            CheckersWithTakes = new List<Checker>();
            foreach (var checker in CheckersSet)
            {
                if (CurrentPlayer.PlaysWhites == checker.IsWhite)
                {
                    FindPossibleTakes(checker);
                    if (PossibleTakes.ContainsKey(checker))
                    {
                        CheckersWithTakes.Add(checker);
                    }
                }
            }
        }

        public bool CanMoveThere(int[] adressOld, int[] adressNew)
        {
            var id = GetCheckerId(new Coordinate(adressOld));
            var checker = CheckersSet[id];
            var newId = GetCheckerId(new Coordinate(adressNew));
            var cellIsEmpty = (newId == -1);

            if (PossibleTakes.ContainsKey(checker))
            {
                try
                {
                    var coordinate =
                        PossibleTakes[checker].Coordinates.Find(
                            c => c.CellAddress[0] == adressNew[0]
                              && c.CellAddress[1] == adressNew[1]);

                    var x = coordinate.CellAddress[0];
                    var y = coordinate.CellAddress[1];

                    return x == adressNew[0]
                        && y == adressNew[1];
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }

            if (checker.IsQueen)
            {
                return cellIsEmpty
                    && IsDiagonalMove(adressOld, adressNew);
            }
            return cellIsEmpty
                   && OneCellMove(adressOld, adressNew)
                   && MoveForward(adressOld, adressNew);
        }

        public bool MoveForward(int[] adressOld, int[] adressNew)
        {
            var id = GetCheckerId(new Coordinate(adressOld));
            var currentChecker = CheckersSet[id];

            if (currentChecker.IsWhite)
            {
                return ((adressNew[0] - adressOld[0]) == -1 && Math.Abs(adressNew[1] - adressOld[1]) == 1);
            }
            return ((adressNew[0] - adressOld[0]) == 1 && Math.Abs(adressNew[1] - adressOld[1]) == 1);
        }

        public bool OneCellMove(int[] adressOld, int[] adressNew)
        {
            return (Math.Abs(adressNew[0] - adressOld[0]) == 1 && Math.Abs(adressNew[1] - adressOld[1]) == 1);
        }

        public bool IsDiagonalMove(int[] adressOld, int[] adressNew)
        {
            return (Math.Abs(adressNew[0] - adressOld[0]) == Math.Abs(adressNew[1] - adressOld[1]));
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((checker.IsWhite || checker.CoordHorizontal != 7)
            && (!checker.IsWhite || checker.CoordHorizontal != 0))
                return;
            checker.IsQueen = true;
            checker.ChangeSymbol();
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
                ClearMessageBar();

                Console.Write("Error! Incorrect input.");
                Thread.Sleep(1000);
                ClearMessageBar();
                
                Console.Write("Choose a checker: ");
                rawInput = CurrentPlayer.InputCoordinates();
            }
            var validInput = rawInput;

            return validInput;
        }

        public void SetCoordinatesForMove()
        {
            const string selectCheckerToMoveMessage = "Choose a checker: ";
            const string selectDestination = "Target cell: ";
            const string cantMoveHereMessage = "Error! Wrong move!";

            Move = new Move();

            ClearMessageBar();
            DisplayCurrentPlayerMessage();

            Thread.Sleep(500);

            var adressOld = GetCellAddress(selectCheckerToMoveMessage);
            var id = GetCheckerId(new Coordinate(adressOld));

            while (id < 0 || !CanSelectChecker(CheckersSet[id]))
            {
                ClearMessageBar();
                Console.Write("Error! Cannot select!");
                Thread.Sleep(1000);

                adressOld = GetCellAddress(selectCheckerToMoveMessage);
                id = GetCheckerId(new Coordinate(adressOld));
            }
            Move.Coordinates.Add(new Coordinate(adressOld));

            var adressNew = GetCellAddress(selectDestination);

            while (!CanMoveThere(adressOld, adressNew))
            {
                ClearMessageBar();
                Console.Write(cantMoveHereMessage);
                Thread.Sleep(1000);

                adressNew = GetCellAddress(selectDestination);
            }
            Move.Coordinates.Add(new Coordinate(adressNew));
        }

        public void MoveChecker()
        {
            var id = GetCheckerId(Move.Coordinates[0]);

            var moves = Move.Coordinates.Count;

            for (var i = 1; i < moves; i++)
            {
                var coordinateNew = Move.Coordinates[1];

                CheckersSet[id].CoordHorizontal = coordinateNew.CellAddress[0];
                CheckersSet[id].CoordVertical = coordinateNew.CellAddress[1];

                CheckerBecomesQueen(CheckersSet[id]);

                Move.Coordinates.RemoveAt(0);

                Console.SetCursorPosition(0, 0);
                Board.Draw(CheckersSet);
            }
            CurrentPlayer = SwitchPlayer();
        }

        public void FindPossibleTakes(Checker currentChecker)
        {

            int[][] direction =
            {
                new [] {-1, -1},
                new [] {-1,  1},
                new [] {1,  -1},
                new [] {1,   1}
            };

            var coordinateToCheck = new Coordinate[4];
            var currentCoordinate = new Coordinate(currentChecker.CoordHorizontal, currentChecker.CoordVertical);
            Enemies = new Move();

            var end = 1;
            if (currentChecker.IsQueen)
            {
                end = 7;
            }

            for (var i = 0; i < 4; i++)
            {
                for (var depth = 1; depth <= end; depth++)
                {
                    coordinateToCheck[i] = new Coordinate(
                        currentCoordinate.CellAddress[0] + depth * direction[i][0], 
                        currentCoordinate.CellAddress[1] + depth * direction[i][1]);

                    foreach (var checker in CheckersSet)
                    {
                        if (Board.CellExists(coordinateToCheck[i])
                            && coordinateToCheck[i].CellAddress[0] == checker.CoordHorizontal
                            && coordinateToCheck[i].CellAddress[1] == checker.CoordVertical)
                        {
                            if (currentChecker.IsWhite == checker.IsWhite)
                            {
                                depth = end + 1;
                                break;
                            }

                            for (var landingDepth = 1; landingDepth <= end; landingDepth++)
                            {
                                var nextCell = new Coordinate(
                                    coordinateToCheck[i].CellAddress[0] + landingDepth * direction[i][0],
                                    coordinateToCheck[i].CellAddress[1] + landingDepth * direction[i][1]);

                                var nextCellId = GetCheckerId(nextCell);

                                var isNextCellEmpty = (nextCellId == -1);
                                
                                if (!isNextCellEmpty)
                                {
                                    landingDepth = end + 1;
                                    break;
                                }

                                var nextCoordinate = new Coordinate(nextCell.CellAddress[0], nextCell.CellAddress[1]);
                                if (Board.CellExists(nextCoordinate))
                                {
                                    Enemies.Coordinates.Add(new Coordinate(nextCell.CellAddress));
                                }
                            }
                        }
                    }
                }
            }
            if (Enemies.Coordinates.Count > 0)
            {
                PossibleTakes.Add(currentChecker, Enemies);
            }
        }
        
        public IUserInput SwitchPlayer()
        {
            return CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public bool IsGameOver()
        {
            var noWhites = CheckersSet.Count(checker => checker.IsWhite) == 0;
            var noBlacks = CheckersSet.Count(checker => checker.IsWhite == false) == 0;

            return (noWhites || noBlacks);
        }

        //public bool IsCheckerBlocked(Checker currentChecker)
        //{
        //    int[][] direction =
        //    {
        //        new [] {-1, -1},
        //        new [] {-1,  1},
        //        new [] {1,  -1},
        //        new [] {1,   1}
        //    };
        //
        //    var coordinateToCheck = new Coordinate[4];
        //    var currentCoordinate = new Coordinate(currentChecker.CoordHorizontal, currentChecker.CoordVertical);
        //
        //    int directionStartNumber;
        //    int directionEndNumber;
        //
        //    if (CurrentPlayer.PlaysWhites && !currentChecker.IsQueen)
        //    {
        //        directionStartNumber = 0;
        //        directionEndNumber = 2;
        //    }
        //    else
        //    {
        //        directionStartNumber = 2;
        //        directionEndNumber = 4;
        //    }
        //
        //    for (int i = directionStartNumber; i < directionEndNumber; i++)
        //    {
        //        coordinateToCheck[i] = new Coordinate(
        //                currentCoordinate.CellAddress[0] + direction[i][0],
        //                currentCoordinate.CellAddress[1] + direction[i][1]);
        //        if (Board.CellExists(coordinateToCheck[i]))
        //        {
        //            
        //        }
        //    }
        //    return true;
        //}
      }
   
}