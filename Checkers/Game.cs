using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        public Board Board { get; set; } // private set
        public List<Checker> CheckersSet = new List<Checker>(); // readonly
        public List<Coordinate> EnemiesCoordinates { get; set; }
        public Move Move { get; set; }
        public IUserInput Player1;
        public IUserInput Player2;
        public IUserInput CurrentPlayer { get; set; }
        public readonly Dictionary<Checker, List<Coordinate>> PossibleTakes = new Dictionary<Checker, List<Coordinate>>();
        public List<Checker> CheckersWithTakes; //readonly
        public List<Checker> BlockedCheckers;

        private const string selectCheckerToMoveMessage = "Choose a checker: ";
        private const string selectDestinationMessage = "Target cell: ";
        private const string cantMoveHereMessage = "Error! Wrong move!";
        private const string incorrectInputMessage = "Error! Incorrect input.";
        private const string cantSelectMessage = "Error! Cannot select!";

        public void Start()
        {
            Player1 = new HumanPlayer(true);
            Player2 = new HumanPlayer(false);
            CurrentPlayer = Player1;

            CreateCheckers(false);
            CreateCheckers(true);

            // TEST SITUATION #1
            //CheckersSet.Add(new Checker(true, true, 2, 3)); // CHECKER WE TEST
            //CheckersSet.Add(new Checker(false, true, 0, 1));
            //CheckersSet.Add(new Checker(false, true, 1, 2));
            //CheckersSet.Add(new Checker(false, true, 1, 4));
            //CheckersSet.Add(new Checker(false, true, 0, 5));
            //CheckersSet.Add(new Checker(true, true, 7, 0));

            // TEST SITUATION #2
            //CheckersSet.Add(new Checker(true, false, 2, 3)); // CHECKER WE TEST
            //CheckersSet.Add(new Checker(true, false, 7, 0));
            //CheckersSet.Add(new Checker(false, false, 6, 1));
            //CheckersSet.Add(new Checker(false, false, 5, 2));

            // TEST SITUATION #3 - white cheker is surrounded with reds
            //CheckersSet.Add(new Checker(true, false, 3, 4)); // CHECKER WE TEST
            //CheckersSet.Add(new Checker(false, false, 2, 3));
            //CheckersSet.Add(new Checker(false, false, 2, 5));
            //CheckersSet.Add(new Checker(false, false, 4, 3));
            //CheckersSet.Add(new Checker(false, false, 4, 5));

            // TEST SITUATION #4 - compound move
            //CheckersSet.Add(new Checker(true, false,  new Coordinate(4, 3))); // CHECKER WE TEST
            //CheckersSet.Add(new Checker(false, false, new Coordinate(3, 4)));
            //CheckersSet.Add(new Checker(false, false, new Coordinate(1, 4)));
            //CheckersSet.Add(new Checker(false, false, new Coordinate(1, 2)));
            //CheckersSet.Add(new Checker(false, false, new Coordinate(3, 2)));


            // TEST SITUATION #5 - the only white checker is blocked
            //CheckersSet.Add(new Checker(true, false, 4, 3)); // CHECKER WE TEST
            //CheckersSet.Add(new Checker(false, false, 3, 4));
            //CheckersSet.Add(new Checker(false, false, 2, 5));
            //CheckersSet.Add(new Checker(false, false, 3, 2));
            //CheckersSet.Add(new Checker(false, false, 2, 1));

            // TEST SITUATION #6 - for Take tests
            //CheckersSet.Add(new Checker(true, false, new Coordinate(2, 1)));
            //CheckersSet.Add(new Checker(false, false, new Coordinate(1, 2)));

            Board = new Board();
            Board.Draw(CheckersSet);
        }

        public void CreateCheckers(bool isWhite)
        {
            var startRow = 0;
            var endRow = 3;

            if (isWhite)
            {
                startRow = 5;
                endRow = 8;
            }

            for (var i = startRow; i < endRow; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 != 0) ||
                        (i % 2 != 0 && j % 2 == 0))
                    {
                        CheckersSet.Add(new Checker(isWhite, false, new Coordinate(i, j)));
                    }
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

        public IUserInput SwitchPlayer()
        {
            return CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }

        public Coordinate GetCoordinate(string message)
        {
            Coordinate coordinate;

            var isCoordinateWithinBoard = true;
            do
            {
                if (!isCoordinateWithinBoard)
                {
                    ClearMessageBar();
                    Console.Write(incorrectInputMessage);
                    Thread.Sleep(1000);
                }

                ClearMessageBar();
                Console.Write(message);
                var validInput = ValidateInput();

                coordinate = ConvertIntoCoordinates(validInput);
                isCoordinateWithinBoard = Board.DoesCellExist(coordinate);

            } while (!isCoordinateWithinBoard);

            return coordinate;
        }

        public Checker GetChecker(Coordinate coordinate)
        {
            return CheckersSet.Find(checker => checker.Coordinate.X == coordinate.X && checker.Coordinate.Y == coordinate.Y);
        }

        public string ValidateInput()
        {
            var rawInput = CurrentPlayer.InputCoordinates();

            while (rawInput == null || rawInput.Length != 2)
            {
                ClearMessageBar();

                Console.Write(incorrectInputMessage);
                Thread.Sleep(1000);
                ClearMessageBar();

                Console.Write(selectCheckerToMoveMessage);
                rawInput = CurrentPlayer.InputCoordinates();
            }
            var validInput = rawInput;

            return validInput;
        }

        public Coordinate ConvertIntoCoordinates(string validInput)
        {
            var ascii = Encoding.ASCII;
            var bytes = ascii.GetBytes(validInput.ToUpper());

            var row = 56 - Convert.ToInt32(bytes[1]);
            var column = Convert.ToInt32(bytes[0]) - 65;

            return new Coordinate(row, column);
        }
        
        public void FindCheckersWithTakes()
        {
            CheckersWithTakes = new List<Checker>();// CheckersWithTakes.Clear ?
            foreach (var checker in CheckersSet.Where(checker => CurrentPlayer.PlaysWhites == checker.IsWhite))
            {
                FindPossibleTakes(checker);
                if (PossibleTakes.ContainsKey(checker))
                {
                    CheckersWithTakes.Add(checker);
                }
            }
        }
        
        public void FindPossibleTakes(Checker currentChecker)
        {
            int[][] directions =
            {
                new [] {-1, -1}, //up left
                new [] {-1,  1}, //up right
                new [] {1,  -1}, //down left
                new [] {1,   1}  //down right
            };

            var currentCoordinate = new Coordinate(currentChecker.Coordinate.X, currentChecker.Coordinate.Y);

            EnemiesCoordinates = new List<Coordinate>();
            var emptyCellsBehindEnemy = new List<Coordinate>();

            var searchEnd = 1;
            if (currentChecker.IsQueen)
            {
                searchEnd = 7;
            }

            foreach (var direction in directions)
            {
                for (var depth = 1; depth <= searchEnd; depth++)
                {
                    var coordinateToCheck = new Coordinate(currentCoordinate.X + depth * direction[0],
                                                           currentCoordinate.Y + depth * direction[1]);

                    if (!Board.DoesCellExist(coordinateToCheck) || IsCellEmpty(coordinateToCheck)) continue;

                    var checkerToCheck = GetChecker(coordinateToCheck);

                    if (currentChecker.IsWhite == checkerToCheck.IsWhite) break;

                    EnemiesCoordinates.Add(coordinateToCheck);

                    for (var landingDepth = 1; landingDepth <= searchEnd; landingDepth++)
                    {
                        var nextCoordinate = new Coordinate(
                            coordinateToCheck.X + landingDepth * direction[0],
                            coordinateToCheck.Y + landingDepth * direction[1]);

                        var isNextCellEmpty = IsCellEmpty(nextCoordinate);

                        if (!isNextCellEmpty)
                        {
                            depth = searchEnd;
                            break;
                        }

                        if (Board.DoesCellExist(nextCoordinate))
                        {
                            emptyCellsBehindEnemy.Add(nextCoordinate);
                        }
                    }
                }
            }
            if (EnemiesCoordinates.Count > 0 && emptyCellsBehindEnemy.Count > 0 && !PossibleTakes.ContainsKey(currentChecker))
            {
                PossibleTakes.Add(currentChecker, emptyCellsBehindEnemy);
            }
        }

        public void SelectChecker()
        {
            Move = new Move();

            ClearMessageBar();
            DisplayCurrentPlayerMessage();

            Thread.Sleep(500);

            var moveStartCoordinate = GetCoordinate(selectCheckerToMoveMessage);
            var checkerToMove = GetChecker(moveStartCoordinate);

            while (!CanSelectChecker(checkerToMove))
            {
                ClearMessageBar();
                Console.Write(cantSelectMessage);
                Thread.Sleep(1000);

                moveStartCoordinate = GetCoordinate(selectCheckerToMoveMessage);
                checkerToMove = GetChecker(moveStartCoordinate);
            }
            Move.Coordinates.Add(moveStartCoordinate);
        }
        
        public void SetDestination()
        {
            var moveStartCoordinate = Move.Coordinates[0];
            var moveEndCoordinate = GetCoordinate(selectDestinationMessage);

            while (!CanMoveThere(moveStartCoordinate, moveEndCoordinate))
            {
                ClearMessageBar();
                Console.Write(cantMoveHereMessage);
                Thread.Sleep(1000);

                moveEndCoordinate = GetCoordinate(selectDestinationMessage);
            }
            Move.Coordinates.Add(moveEndCoordinate);
        }

        public void SetMove()
        {
            SelectChecker();

            if (PossibleTakes.Values.Count == 0)
            {
                SetDestination();
                MoveChecker();
            }
            else
            {
                while (PossibleTakes.Values.Count > 0)
                {
                    var currentChecker = GetChecker(Move.Coordinates[0]);

                    SetDestination();
                    PossibleTakes.Clear();
                    MoveChecker();
                    FindPossibleTakes(currentChecker);
                }
            }
        }

        public void MoveChecker()
        {
            var checker = GetChecker(Move.Coordinates[0]);
            var moves = Move.Coordinates.Count;

            for (var i = 1; i < moves; i++)
            {
                var coordinateNew = Move.Coordinates[1];

                checker.Coordinate.X = coordinateNew.X;
                checker.Coordinate.Y = coordinateNew.Y;

                CheckerBecomesQueen(checker);
                RemoveTakenChecker();

                Move.Coordinates.RemoveAt(0);

                Console.SetCursorPosition(0, 0);
                Board.Draw(CheckersSet);
            }
        }
        
        public void RemoveTakenChecker()
        {
            var finishX = Move.Coordinates[1].X;
            var startX = Move.Coordinates[0].X;

            var finishY = Move.Coordinates[1].Y;
            var startY = Move.Coordinates[0].Y;

            var deltaX = finishX - startX;
            var deltaY = finishY - startY;

            var signX = deltaX / Math.Abs(deltaX);
            var signY = deltaY / Math.Abs(deltaY);

            for (var x = startX + signX; deltaX < 0 ? (x > finishX) : (x < finishX); x = x + signX)
            {
                for (var y = startY + signY; deltaY < 0 ? (y > finishY) : (y < finishY); y = y + signY)
                {
                    var coordinate = new Coordinate(x, y);

                    var moveByX = Math.Abs(x - startX);
                    var moveByY = Math.Abs(y - startY);
                    var isDiagonalMove = moveByX == moveByY;

                    if (isDiagonalMove && !IsCellEmpty(coordinate))
                    {
                        CheckersSet.Remove(GetChecker(coordinate));
                    }
                }
            }
        }
        
        public void CheckerBecomesQueen(Checker checker)
        {
            const int topRow = 0;
            const int bottomRow = 7;

            if ((checker.IsWhite || checker.Coordinate.X != bottomRow) && (!checker.IsWhite || checker.Coordinate.X != topRow)) return;

            checker.ChangeToQueen();
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

            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool CanMoveThere(Coordinate moveStartCoordinate, Coordinate moveEndCoordinate)
        {
            var checkerToMove = GetChecker(moveStartCoordinate);
            var isCellEmpty = IsCellEmpty(moveEndCoordinate);

            if (PossibleTakes.ContainsKey(checkerToMove))
            {
                try
                {
                    var targetCoordinateToCheck =
                        PossibleTakes[checkerToMove].
                                Find(coord => coord.X == moveEndCoordinate.X
                                           && coord.Y == moveEndCoordinate.Y);

                    return targetCoordinateToCheck.X == moveEndCoordinate.X
                        && targetCoordinateToCheck.Y == moveEndCoordinate.Y;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }

            if (checkerToMove.IsQueen)
            {
                return isCellEmpty && IsDiagonalMove(moveStartCoordinate, moveEndCoordinate);
            }
            return isCellEmpty
                   && IsOneCellMove(moveStartCoordinate, moveEndCoordinate)
                   && IsMoveForward(moveStartCoordinate, moveEndCoordinate);
        }

        public bool IsMoveForward(Coordinate moveStartCoordinate, Coordinate moveEndCoordinate)
        {
            var currentChecker = GetChecker(moveStartCoordinate);

            if (currentChecker.IsWhite)
            {
                return ((moveEndCoordinate.X - moveStartCoordinate.X) == -1 && Math.Abs(moveEndCoordinate.Y - moveStartCoordinate.Y) == 1);
            }
            return ((moveEndCoordinate.X - moveStartCoordinate.X) == 1 && Math.Abs(moveEndCoordinate.Y - moveStartCoordinate.Y) == 1);
        }

        public bool IsOneCellMove(Coordinate moveStartCoordinate, Coordinate moveEndCoordinate)
        {
            return (Math.Abs(moveEndCoordinate.X - moveStartCoordinate.X) == 1 && Math.Abs(moveEndCoordinate.Y - moveStartCoordinate.Y) == 1);
        }

        public bool IsDiagonalMove(Coordinate moveStartCoordinate, Coordinate moveEndCoordinate)
        {
            return (Math.Abs(moveEndCoordinate.X - moveStartCoordinate.X) == Math.Abs(moveEndCoordinate.Y - moveStartCoordinate.Y));
        }
        
        public bool IsCheckerBlocked(Checker checker)
        {
            int[][] directions =
            {
                new [] {-1, -1}, //up left
                new [] {-1,  1}, //up right
                new [] {1,  -1}, //down left
                new [] {1,   1}  // down right
            };

            var isDirectionBlocked = new bool[4];

            var x = checker.Coordinate.X;
            var y = checker.Coordinate.Y;

            for (var i = 0; i < directions.Length; i++)
            {
                var twoEnemiesInARow = false;
                var friendlyChecker = false;
                var reverseMove = false;
                var outOfBoard = false;

                var firstX = x + directions[i][0];
                var firstY = y + directions[i][1];

                var secondX = x + 2 * directions[i][0];
                var secondY = y + 2 * directions[i][1];

                var currentCoordinate = new Coordinate(x, y);

                var firstCoordinate = new Coordinate(firstX, firstY);
                var firstChecker = GetChecker(firstCoordinate);

                var secondCoordinate = new Coordinate(secondX, secondY);
                var secondChecker = GetChecker(secondCoordinate);

                if (Board.DoesCellExist(firstCoordinate))
                {
                    var isFirstCellEmpty = IsCellEmpty(firstCoordinate);
                    var isSecondCellEmpty = IsCellEmpty(secondCoordinate);

                    if (!isFirstCellEmpty)
                    {
                        if (firstChecker.IsWhite == CurrentPlayer.PlaysWhites)
                        {
                            friendlyChecker = true;
                        }
                        else if (Board.DoesCellExist(secondCoordinate) && !isSecondCellEmpty && (secondChecker.IsWhite != CurrentPlayer.PlaysWhites))
                        {
                            twoEnemiesInARow = true;
                        }
                    }
                }
                else
                {
                    outOfBoard = true;
                }

                if (!checker.IsQueen && !IsMoveForward(currentCoordinate, firstCoordinate))
                {
                    reverseMove = true;
                }

                isDirectionBlocked[i] = twoEnemiesInARow || friendlyChecker || reverseMove || outOfBoard;
            }
            return isDirectionBlocked.All(value => value.Equals(true));
        }

        public bool IsCellEmpty(Coordinate coordinate)
        {
            return GetChecker(coordinate) == null;
        }
        
        public bool IsGameOver()
        {
            var noWhites = CheckersSet.All(checker => !checker.IsWhite);
            var noBlacks = CheckersSet.All(checker => checker.IsWhite);

            if (noWhites || noBlacks)
                return true;

            return CheckersSet.Where(checker => checker.IsWhite == CurrentPlayer.PlaysWhites).All(IsCheckerBlocked);
        }

        public void Over()
        {
            ClearMessageBar();
            Console.SetCursorPosition(50, 10);
            Console.Write("Game Over");
            Console.ReadLine();
        }
    }
}