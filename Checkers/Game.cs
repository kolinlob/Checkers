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
        public Move Move    { get; set; }
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

            //CheckersSet.Add(new Checker(true, false, 3, 4)); 
            CheckersSet.Add(new Checker(true, false, 2, 3)); // CHECKER WE TEST
            CheckersSet.Add(new Checker(false, false, 0, 1));
            CheckersSet.Add(new Checker(false, false, 1, 2));
            CheckersSet.Add(new Checker(false, false, 1, 4));
            CheckersSet.Add(new Checker(false, false, 0, 5));

            CheckersSet.Add(new Checker(true, false, 7, 0));
            //CheckersSet.Add(new Checker(false, false, 6, 1));

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

        public Coordinate GetCellAddress(string message)
        {
            Coordinate coordinate;
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
                coordinate = ConvertIntoCoordinates(validInput);
                counter++;

            } while (coordinate.X < 0 || coordinate.Y < 0 || coordinate.X > 7 || coordinate.Y > 7);

            return coordinate;
        }

        public int GetCheckerId(Coordinate coordinate)
        {
            try
            {
                return (from checker in CheckersSet
                        where
                            coordinate.X == checker.CoordHorizontal &&
                            coordinate.Y == checker.CoordVertical
                        select CheckersSet.IndexOf(checker)).Single();
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        public bool CanSelectChecker(Checker checker) 
        {
            try
            {
                var isOwnChecker = (CurrentPlayer.PlaysWhites == checker.IsWhite);

                if (!isOwnChecker)
                    return false;

                if (CheckersWithTakes.Count > 0)
                    return CheckersWithTakes.Contains(checker);
                
                return !IsCheckerBlocked(checker);
            }

            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public void FindCheckersWithTakes()
        {
            CheckersWithTakes = new List<Checker>();
            foreach (var checker in CheckersSet.Where(checker => CurrentPlayer.PlaysWhites == checker.IsWhite))
            {
                FindPossibleTakes(checker);
                if (PossibleTakes.ContainsKey(checker))
                {
                    CheckersWithTakes.Add(checker);
                }
            }
        }

        public bool CanMoveThere(Coordinate start, Coordinate target)
        {
            var id = GetCheckerId(start);
            var newId = GetCheckerId(target);
            var checker = CheckersSet[id];
            var isCellEmpty = (newId == -1);

            if (PossibleTakes.ContainsKey(checker))
            {
                try
                {
                    var coordinate = PossibleTakes[checker].Coordinates.Find(coord => coord.X == target.X 
                                                                                   && coord.Y == target.Y);

                    return coordinate.X == target.X
                        && coordinate.Y == target.Y;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }

            if (checker.IsQueen)
            {
                return isCellEmpty
                    && IsDiagonalMove(start, target);
            }
            return isCellEmpty
                   && OneCellMove(start, target)
                   && MoveForward(start, target);
        }

        public bool MoveForward(Coordinate start, Coordinate target)
        {
            var id = GetCheckerId(start);
            var currentChecker = CheckersSet[id];

            if (currentChecker.IsWhite)
            {
                return ((target.X - start.X) == -1 && Math.Abs(target.Y - start.Y) == 1);
            }
            return ((target.X - start.X) == 1 && Math.Abs(target.Y - start.Y) == 1);
        }

        public bool OneCellMove(Coordinate start, Coordinate target)
        {
            return (Math.Abs(target.X - start.X) == 1 && Math.Abs(target.Y - start.Y) == 1);
        }

        public bool IsDiagonalMove(Coordinate start, Coordinate target)
        {
            return (Math.Abs(target.X - start.X) == Math.Abs(target.Y - start.Y));
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            if ((checker.IsWhite || checker.CoordHorizontal != 7)
            && (!checker.IsWhite || checker.CoordHorizontal != 0))
                return;
            checker.IsQueen = true;
            checker.ChangeSymbol();
        }

        public Coordinate ConvertIntoCoordinates(string validInput)
        {
            var ascii = Encoding.ASCII;
            var bytes = ascii.GetBytes(validInput.ToUpper());

            var row = 56 - Convert.ToInt32(bytes[1]);
            var col = Convert.ToInt32(bytes[0]) - 65;

            return new Coordinate(row, col);
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

            var start = GetCellAddress(selectCheckerToMoveMessage);
            var id = GetCheckerId(start);

            while (id < 0 || !CanSelectChecker(CheckersSet[id]))
            {
                ClearMessageBar();
                Console.Write("Error! Cannot select!");
                Thread.Sleep(1000);

                start = GetCellAddress(selectCheckerToMoveMessage);
                id = GetCheckerId(start);
            }
            Move.Coordinates.Add(start);

            var target = GetCellAddress(selectDestination);

            while (!CanMoveThere(start, target))
            {
                ClearMessageBar();
                Console.Write(cantMoveHereMessage);
                Thread.Sleep(1000);

                target = GetCellAddress(selectDestination);
            }
            Move.Coordinates.Add(target);
        }

        public void MoveChecker()
        {
            var id = GetCheckerId(Move.Coordinates[0]);

            var moves = Move.Coordinates.Count;

            for (var i = 1; i < moves; i++)
            {
                var coordinateNew = Move.Coordinates[1];

                CheckersSet[id].CoordHorizontal = coordinateNew.X;
                CheckersSet[id].CoordVertical = coordinateNew.Y;

                CheckerBecomesQueen(CheckersSet[id]);

                Move.Coordinates.RemoveAt(0);

                Console.SetCursorPosition(0, 0);
                Board.Draw(CheckersSet);
            }
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
                        currentCoordinate.X + depth * direction[i][0], 
                        currentCoordinate.Y + depth * direction[i][1]);

                    foreach (var checker in CheckersSet)
                    {
                        if (Board.CellExists(coordinateToCheck[i])
                            && coordinateToCheck[i].X == checker.CoordHorizontal
                            && coordinateToCheck[i].Y == checker.CoordVertical)
                        {
                            if (currentChecker.IsWhite == checker.IsWhite)
                            {
                                depth = end + 1;
                                break;
                            }

                            for (var landingDepth = 1; landingDepth <= end; landingDepth++)
                            {
                                var nextCoordinate = new Coordinate(
                                    coordinateToCheck[i].X + landingDepth * direction[i][0],
                                    coordinateToCheck[i].Y + landingDepth * direction[i][1]);

                                var nextCellId = GetCheckerId(nextCoordinate);

                                var isNextCellEmpty = (nextCellId == -1);
                                
                                if (!isNextCellEmpty)
                                {
                                    break;
                                }

                                if (Board.CellExists(nextCoordinate))
                                {
                                    Enemies.Coordinates.Add(nextCoordinate);
                                }
                            }
                        }
                    }
                }
            }
            if (Enemies.Coordinates.Count > 0 && !PossibleTakes.ContainsKey(currentChecker))
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

            foreach (var checker in CheckersSet.Where(checker => checker.IsWhite == CurrentPlayer.PlaysWhites))
            {
                if (checker.IsWhite && !IsCheckerBlocked(checker))
                {
                    return false;
                }

                if (!checker.IsWhite && !IsCheckerBlocked(checker))
                {
                    return false;
                }

                return true;
            }
            return (noWhites || noBlacks);
        }

        public bool IsCheckerBlocked(Checker checker)
        {
            int[][] directions =
            {
                new [] {-1, -1},
                new [] {-1,  1},
                new [] {1,  -1},
                new [] {1,   1}
            };

            var isDirectionBlocked = new bool[4];

            var x = checker.CoordHorizontal;
            var y = checker.CoordVertical;
            
            for(var i = 0; i < directions.Length; i++)
            {
                var twoEnemiesInARow = false;
                var friendlyChecker = false;
                var reverseMove = false;

                var firstX = x + directions[i][0];
                var firstY = y + directions[i][1];

                var secondX = x + 2 * directions[i][0];
                var secondY = y + 2 * directions[i][1];

                var currentCoordinate = new Coordinate(x, y);
                var firstCoordinate = new Coordinate(firstX, firstY);
                var secondCoordinate = new Coordinate(secondX, secondY);

                if (Board.CellExists(firstCoordinate))
                {
                    var firstId = GetCheckerId(firstCoordinate);
                    var secondId = GetCheckerId(secondCoordinate);

                    if (firstId > 0)
                    {
                        if (CheckersSet[firstId].IsWhite == CurrentPlayer.PlaysWhites)
                        {
                            friendlyChecker = true;
                        }
                        else if (Board.CellExists(secondCoordinate) && CheckersSet[secondId].IsWhite != CurrentPlayer.PlaysWhites)
                        {
                            twoEnemiesInARow = true;
                        }
                    }
                }

                if (!checker.IsQueen && !MoveForward(currentCoordinate, firstCoordinate))
                {
                    reverseMove = true;
                }

                isDirectionBlocked[i] = twoEnemiesInARow || friendlyChecker || reverseMove;
            }
            return isDirectionBlocked.All(value => value.Equals(true));
        }
    }
}