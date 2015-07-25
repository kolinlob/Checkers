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

        private readonly int[][] directions =
            {
                new [] {-1, -1}, //up left
                new [] {-1,  1}, //up right
                new [] {1,  -1}, //down left
                new [] {1,   1}  //down right
            };

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

            Screen.SetGraphicParameters();

            Board.Draw(CheckersSet);

            while (!IsGameOver())
            {
                FindCheckersWithTakes();
                SetMove();
                SwitchPlayer();
            }
            Screen.GameOverMessage();
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
                    Screen.ClearMessageBar();
                    Console.Write(incorrectInputMessage);
                    Thread.Sleep(1000);
                }

                Screen.ClearMessageBar();
                Console.Write(message);
                var validInput = GetValidInput();

                coordinate = ConvertIntoCoordinates(validInput);
                isCoordinateWithinBoard = Board.DoesCellExist(coordinate);

            } while (!isCoordinateWithinBoard);

            return coordinate;
        }

        public Checker GetChecker(Coordinate coordinate)
        {
            return CheckersSet.Find(checker => checker.Coordinate.X == coordinate.X && checker.Coordinate.Y == coordinate.Y);
        }

        public List<Checker> GetOwnCheckers()
        {
            return new List<Checker>(CheckersSet.Where(checker => CurrentPlayer.PlaysWhites == checker.IsWhite));
        }

        public string GetValidInput()
        {
            var rawInput = CurrentPlayer.EnterCoordinates();

            while (!IsInputValid(rawInput))
            {
                Screen.ClearMessageBar();

                Console.Write(incorrectInputMessage);
                Thread.Sleep(1000);
                Screen.ClearMessageBar();

                Console.Write(selectCheckerToMoveMessage);
                rawInput = CurrentPlayer.EnterCoordinates();
            }
            var validInput = rawInput;

            return validInput;
        }

        private bool IsInputValid(string rawInput)
        {
            return (!IsNull(rawInput) && IsOfCorrectLength(rawInput));
        }

        private bool IsOfCorrectLength(string rawInput)
        {
            return rawInput.Length == 2;
        }

        private bool IsNull(string rawInput)
        {
            return rawInput == null;
        }

        public Coordinate ConvertIntoCoordinates(string validInput)
        {
            var ascii = Encoding.ASCII;
            var chars = ascii.GetBytes(validInput.ToUpper());

            int humanCoordinateX = chars[1];
            int humanCoordinateY = chars[0];

            var row = 56 - humanCoordinateX;
            var column = humanCoordinateY - 65;

            return new Coordinate(row, column);
        }

        public void FindCheckersWithTakes()
        {
            CheckersWithTakes = new List<Checker>();

            foreach (var checker in GetOwnCheckers())
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

                    var isOwnChecker = currentChecker.IsWhite == checkerToCheck.IsWhite;

                    if (isOwnChecker) break;

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
            if (IsTakePossible(currentChecker, emptyCellsBehindEnemy))
            {
                PossibleTakes.Add(currentChecker, emptyCellsBehindEnemy);
            }
        }

        private bool IsTakePossible(Checker currentChecker, List<Coordinate> emptyCellsBehindEnemy)
        {
            return EnemiesCoordinates.Count > 0 && emptyCellsBehindEnemy.Count > 0 && !PossibleTakes.ContainsKey(currentChecker);
        }

        public void SelectChecker()
        {
            Move = new Move();

            Screen.ClearMessageBar();
            Screen.DisplayCurrentPlayerMessage(CurrentPlayer);

            Thread.Sleep(500);

            var moveStartCoordinate = GetCoordinate(selectCheckerToMoveMessage);
            var checkerToMove = GetChecker(moveStartCoordinate);

            while (!CanSelectChecker(checkerToMove))
            {
                Screen.ClearMessageBar();
                Console.Write(cantSelectMessage);
                Thread.Sleep(1000);

                moveStartCoordinate = GetCoordinate(selectCheckerToMoveMessage);
                checkerToMove = GetChecker(moveStartCoordinate);
            }
            Move.AddCoordinate(moveStartCoordinate);
        }

        public void SetDestination()
        {
            var moveStartCoordinate = Move.GetStartCoordinate();
            var moveEndCoordinate = GetCoordinate(selectDestinationMessage);

            while (!CanMoveThere(moveStartCoordinate, moveEndCoordinate))
            {
                Screen.ClearMessageBar();
                Console.Write(cantMoveHereMessage);
                Thread.Sleep(1000);

                moveEndCoordinate = GetCoordinate(selectDestinationMessage);
            }
            Move.AddCoordinate(moveEndCoordinate);
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
                    var currentChecker = GetChecker(Move.GetStartCoordinate());

                    SetDestination();
                    PossibleTakes.Clear();
                    MoveChecker();
                    FindPossibleTakes(currentChecker);
                }
            }
        }

        public void MoveChecker()
        {
            var checker = GetChecker(Move.GetStartCoordinate());
            var coordinateNew = Move.GetEndCoordinate();

            checker.Coordinate.Change(coordinateNew);

            CheckerBecomesQueen(checker);
            RemoveTakenChecker();

            Move.RemoveFirstCoordinate();

            Console.SetCursorPosition(0, 0);
            Board.Draw(CheckersSet);
        }

        public void RemoveTakenChecker()
        {
            var finishX = Move.GetEndCoordinate().X;
            var startX = Move.GetStartCoordinate().X;

            var finishY = Move.GetEndCoordinate().Y;
            var startY = Move.GetStartCoordinate().Y;

            var deltaX = finishX - startX;
            var deltaY = finishY - startY;

            var signX = deltaX / Math.Abs(deltaX);
            var signY = deltaY / Math.Abs(deltaY);

            var initialX = startX + signX;
            var moveUp = deltaX < 0;

            var initialY = startY + signY;
            var moveRight = deltaY < 0;

            for (var x = initialX; moveUp ? (x > finishX) : (x < finishX); x = x + signX)
            {
                for (var y = initialY; moveRight ? (y > finishY) : (y < finishY); y = y + signY)
                {
                    var coordinate = new Coordinate(x, y);

                    var moveByX = Math.Abs(x - startX);
                    var moveByY = Math.Abs(y - startY);
                    var isDiagonalMove = moveByX == moveByY;

                    if (isDiagonalMove && !IsCellEmpty(coordinate))
                    {
                        var currentChecker = GetChecker(coordinate);
                        CheckersSet.Remove(currentChecker);
                        Console.Beep();
                    }
                }
            }
        }

        public void CheckerBecomesQueen(Checker checker)
        {
            const int topRow = 0;
            const int bottomRow = 7;

            var isAtBottomRow = checker.Coordinate.X == bottomRow;
            var isAtTopRow = checker.Coordinate.X == topRow;

            if ((checker.IsWhite || !isAtBottomRow) && (!checker.IsWhite || !isAtTopRow)) return;

            checker.ChangeToQueen();
        }

        public bool CanSelectChecker(Checker checker)
        {
            if (checker != null)
            {
                var isOwnChecker = CurrentPlayer.PlaysWhites == checker.IsWhite;

                if (!isOwnChecker)
                {
                    return false;
                }
                if (CheckersWithTakes.Count > 0)
                {
                    return CheckersWithTakes.Contains(checker);
                }
                return !IsCheckerBlocked(checker);
            }
            return false;
        }

        public bool CanMoveThere(Coordinate moveStartCoordinate, Coordinate moveEndCoordinate)
        {
            var checkerToMove = GetChecker(moveStartCoordinate);
            var isCellEmpty = IsCellEmpty(moveEndCoordinate);

            if (checkerToMove != null && PossibleTakes.ContainsKey(checkerToMove))
            {
                var targetCoordinateToCheck =
                    PossibleTakes[checkerToMove].
                            Find(coord => coord.X == moveEndCoordinate.X
                                       && coord.Y == moveEndCoordinate.Y);

                if (targetCoordinateToCheck != null)
                {
                    return targetCoordinateToCheck.X == moveEndCoordinate.X
                        && targetCoordinateToCheck.Y == moveEndCoordinate.Y;
                }
                return false;
            }

            if (checkerToMove != null && checkerToMove.IsQueen)
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

                var currentCoordinate = new Coordinate(x, y);

                var firstCoordinate = new Coordinate(firstX, firstY);
                var firstChecker = GetChecker(firstCoordinate);

                if (Board.DoesCellExist(firstCoordinate))
                {
                    var secondX = x + 2 * directions[i][0];
                    var secondY = y + 2 * directions[i][1];

                    var secondCoordinate = new Coordinate(secondX, secondY);
                    var secondChecker = GetChecker(secondCoordinate);

                    var isFirstCellEmpty = IsCellEmpty(firstCoordinate);
                    var isSecondCellEmpty = IsCellEmpty(secondCoordinate);

                    if (!isFirstCellEmpty)
                    {
                        if (IsOwnChecker(firstChecker))
                        {
                            friendlyChecker = true;
                        }
                        else if (Board.DoesCellExist(secondCoordinate) && !isSecondCellEmpty && !IsOwnChecker(secondChecker))
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

        private bool IsOwnChecker(Checker checker)
        {
            return checker.IsWhite == CurrentPlayer.PlaysWhites;
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

            return GetOwnCheckers().All(IsCheckerBlocked);
        }
    }
}