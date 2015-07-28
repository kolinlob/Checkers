using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkers
{
    public class Game
    {
        private Board Board { get; set; } 
        private readonly Checkers checkers = new Checkers(); 
        private List<Coordinate> EnemiesCoordinates { get; set; }
        private Move Move { get; set; }
        private IUserInput Player1;
        private IUserInput Player2;
        private IUserInput CurrentPlayer { get; set; }
        private readonly Dictionary<Checker, List<Coordinate>> PossibleTakes = new Dictionary<Checker, List<Coordinate>>();
        private List<Checker> CheckersWithTakes; 


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

            checkers.Create(false);
            checkers.Create(true);

            // TEST SITUATION #1
            //checkers.Add(new Checker(true, true, 2, 3)); // CHECKER WE TEST
            //checkers.Add(new Checker(false, true, 0, 1));
            //checkers.Add(new Checker(false, true, 1, 2));
            //checkers.Add(new Checker(false, true, 1, 4));
            //checkers.Add(new Checker(false, true, 0, 5));
            //checkers.Add(new Checker(true, true, 7, 0));

            // TEST SITUATION #2
            //checkers.Add(new Checker(true, false, 2, 3)); // CHECKER WE TEST
            //checkers.Add(new Checker(true, false, 7, 0));
            //checkers.Add(new Checker(false, false, 6, 1));
            //checkers.Add(new Checker(false, false, 5, 2));

            // TEST SITUATION #3 - white cheker is surrounded with reds
            //checkers.Set.Add(new Checker(true, false,  new Coordinate(3, 4))); // CHECKER WE TEST
            //checkers.Set.Add(new Checker(false, false, new Coordinate(2, 3)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(2, 5)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(4, 3)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(4, 5)));

            // TEST SITUATION #4 - compound move
            //checkers.Add(new Checker(true, false,  new Coordinate(4, 3))); // CHECKER WE TEST
            //checkers.Add(new Checker(false, false, new Coordinate(3, 4)));
            //checkers.Add(new Checker(false, false, new Coordinate(1, 4)));
            //checkers.Add(new Checker(false, false, new Coordinate(1, 2)));
            //checkers.Add(new Checker(false, false, new Coordinate(3, 2)));


            // TEST SITUATION #5 - the only white checker is blocked
            //checkers.Set.Add(new Checker(true, false,  new Coordinate(4, 3))); // CHECKER WE TEST
            //checkers.Set.Add(new Checker(false, false, new Coordinate(3, 4)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(2, 5)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(3, 2)));
            //checkers.Set.Add(new Checker(false, false, new Coordinate(2, 1)));

            // TEST SITUATION #6 - for Take tests
            //checkers.Add(new Checker(true, false, new Coordinate(2, 1)));
            //checkers.Add(new Checker(false, false, new Coordinate(1, 2)));

            Board = new Board();

            Screen.SetGraphicParameters();

            Board.Draw(checkers);

            while (!IsGameOver())
            {
                FindCheckersWithTakes();
                SetMove();
                SwitchPlayer();
            }
            Screen.GameOverMessage();
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
                    Screen.DisplayIncorrectInputMessage();
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

        public string GetValidInput()
        {
            var rawInput = CurrentPlayer.EnterCoordinates();

            while (!IsInputValid(rawInput))
            {
                Screen.ClearMessageBar();

                Screen.DisplayIncorrectInputMessage();
                Thread.Sleep(1000);
                Screen.ClearMessageBar();

                Screen.DisplaySelectCheckerMessage();
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

            foreach (var checker in checkers.GetOwnCheckers(CurrentPlayer))
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

                    var checkerToCheck = checkers.GetChecker(coordinateToCheck);

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

            var moveStartCoordinate = GetCoordinate(Screen.GetSelectCheckerMessage());
            var checkerToMove = checkers.GetChecker(moveStartCoordinate);

            while (!CanSelectChecker(checkerToMove))
            {
                Screen.ClearMessageBar();
                Screen.DisplayCantSelectMessage();
                Thread.Sleep(1000);

                moveStartCoordinate = GetCoordinate(Screen.GetSelectCheckerMessage());
                checkerToMove = checkers.GetChecker(moveStartCoordinate);
            }
            Move.AddCoordinate(moveStartCoordinate);
        }

        public void SetDestination()
        {
            var moveStartCoordinate = Move.GetStartCoordinate();
            var moveEndCoordinate = GetCoordinate(Screen.GetSelectDestinationMessage());

            while (!CanMoveThere(moveStartCoordinate, moveEndCoordinate))
            {
                Screen.ClearMessageBar();
                Screen.DisplayCantMoveHereMessage();
                Thread.Sleep(1000);

                moveEndCoordinate = GetCoordinate(Screen.GetSelectDestinationMessage());
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
                    var currentChecker = checkers.GetChecker(Move.GetStartCoordinate());

                    SetDestination();
                    PossibleTakes.Clear();
                    MoveChecker();
                    FindPossibleTakes(currentChecker);
                }
            }
        }

        public void MoveChecker()
        {
            var checker = checkers.GetChecker(Move.GetStartCoordinate());
            var coordinateNew = Move.GetEndCoordinate();

            checker.Coordinate.Change(coordinateNew);

            CheckerBecomesQueen(checker);
            RemoveTakenChecker();

            Move.RemoveFirstCoordinate();

            Console.SetCursorPosition(0, 0);
            Board.Draw(checkers);
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
                        var currentChecker = checkers.GetChecker(coordinate);
                        checkers.Set.Remove(currentChecker);
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
            var checkerToMove = checkers.GetChecker(moveStartCoordinate);
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
            var currentChecker = checkers.GetChecker(moveStartCoordinate);

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
                var firstChecker = checkers.GetChecker(firstCoordinate);

                if (Board.DoesCellExist(firstCoordinate))
                {
                    var secondX = x + 2 * directions[i][0];
                    var secondY = y + 2 * directions[i][1];

                    var secondCoordinate = new Coordinate(secondX, secondY);
                    var secondChecker = checkers.GetChecker(secondCoordinate);

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
            return checkers.GetChecker(coordinate) == null;
        }

        public bool IsGameOver()
        {
            var noWhites = checkers.Set.All(checker => !checker.IsWhite);
            var noBlacks = checkers.Set.All(checker => checker.IsWhite);

            if (noWhites || noBlacks)
                return true;

            return checkers.GetOwnCheckers(CurrentPlayer).All(IsCheckerBlocked);
        }
    }
}