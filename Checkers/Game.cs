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

        public Dictionary<int, Move> PossibleTakes = new Dictionary<int, Move>();

        public List<Checker> checkersWithTakes;
        
        public void Start()
        {
            Player1 = new HumanPlayer(true);
            Player2 = new HumanPlayer(false);
            CurrentPlayer = Player1;

            //CreateCheckers(false);
            //CreateCheckers(true);

            CheckersSet.Add(new Checker(true, false, 4, 5)); // CHECKER WE TEST
            CheckersSet.Add(new Checker(false, false, 5, 6));
            CheckersSet.Add(new Checker(false, false, 3, 4));
           

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

        //надо добавить сюда проверку, что у шашки есть куда ходить - она не заблокирована другими шашками или границам поля
        public bool CanSelectChecker(Checker checker)
        {
            try
            {
                bool isOwnChecker = CurrentPlayer.PlaysWhites && checker.IsWhite;

                if (!isOwnChecker) 
                    return false;
                if (checkersWithTakes.Count > 0)
                    return checkersWithTakes.Contains(checker);
                
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public void FindCheckersWithTakes()
        {
            checkersWithTakes = new List<Checker>();
            foreach (Checker checker in CheckersSet)
            {
                FindPossibleTakes(checker);
                int id = CheckersSet.IndexOf(checker);
                if (PossibleTakes.ContainsKey(id))
                {
                    checkersWithTakes.Add(checker);
                }
            }
        }

        public bool CanMoveThere(int[] adressOld, int[] adressNew)
        {
            var id = GetCheckerId(new Coordinate(adressOld));
            var checker = CheckersSet[id];
            var newId = GetCheckerId(new Coordinate(adressNew));
            var cellIsEmpty = (newId == -1);

            if (PossibleTakes.ContainsKey(id))
            {
                return PossibleTakes[id].Coordinates[0].CellAddress[0] == adressNew[0] && PossibleTakes[id].Coordinates[0].CellAddress[1] == adressNew[1];
            }

            if (checker.IsQueen)
            {
                return cellIsEmpty
                       && Board.IsCellUsable(adressNew[0], adressNew[1])
                       && IsDiagonalMove(adressOld, adressNew);
            }
            return OneCellMove(adressOld, adressNew)
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
            if ((checker.IsWhite || checker.CoordHorizontal != 7) && (!checker.IsWhite || checker.CoordHorizontal != 0))
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
            var selectedChecker = CheckersSet[id];

            while (!CanSelectChecker(selectedChecker))
            {
                ClearMessageBar();
                Console.Write("Error! Cannot select!");
                Thread.Sleep(1000);
                adressOld = GetCellAddress(selectCheckerToMoveMessage);
                id = GetCheckerId(new Coordinate(adressOld));

                selectedChecker = CheckersSet[id];
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
            var end = 1;
            int[][] direction =
            {
                new [] {-1, -1},
                new [] {-1,  1},
                new [] {1,  -1},
                new [] {1,   1}
            };

            var moveDirection = new int[4][];
            var currentCoordinate = new Coordinate(currentChecker.CoordHorizontal, currentChecker.CoordVertical);
            var id = GetCheckerId(currentCoordinate);
            Enemies = new Move();

            if (CheckersSet[id].IsQueen)
                end = 7;
            
            for (var i = 0; i < 4; i++)
            {
                for (var j = 1; j <= end; j++)
                {
                    moveDirection[i] = new[] { currentCoordinate.CellAddress[0] + j * direction[i][0], currentCoordinate.CellAddress[1] + j * direction[i][1] };
                    
                    foreach (var checker in CheckersSet)
                    {
                        if (Board.CellExists(moveDirection[i])
                            && moveDirection[i][0] == checker.CoordHorizontal
                            && moveDirection[i][1] == checker.CoordVertical)
                        {
                            if (CurrentPlayer.PlaysWhites == checker.IsWhite)
                            {
                                j = end + 1;
                                break;
                            }
                            
                            for (int k = 1; k <= end; k++)
                            {
                                var nextCell = new Coordinate(moveDirection[i][0] + k*direction[i][0],
                                    moveDirection[i][1] + k*direction[i][1]);

                                var nextCellId = GetCheckerId(nextCell);

                                if (nextCellId != -1 && CurrentPlayer.PlaysWhites == CheckersSet[nextCellId].IsWhite)
                                {
                                    break;
                                }

                                if (Board.CellExists(new[] {nextCell.CellAddress[0], nextCell.CellAddress[1]}) &&
                                    nextCellId == -1)
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
                PossibleTakes.Add(id, Enemies);
            }
        }
        
        public bool CanTake(Checker checker)
        {
            //if ()
            return true; // currentChecker calls this method. We iterate through its Enemies to find, if there is an empty cell behind the enemy.
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
    }
}