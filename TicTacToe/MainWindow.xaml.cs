using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace TicTacToe_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private string[] Results;
        public static int[] startingpos = { 0, 3, 6, 0, 1, 2, 0, 2 };
        public static int[] directions = { 1, 1, 1, 3, 3, 3, 4, 2 };
        public int numSymbols;
        public int choice;
        public bool vsplayer;
        public bool PlayerTurn = true;
        #endregion
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            newGame();
        }
        #endregion
        #region BaseGame
        private void newGame()
        {
            if (GameMode.SelectedIndex == 0)
            {
                vsplayer = true;
            }
            else vsplayer = false;
            PlayerTurn = true;
            Results = new string[9];
            for (int i = 0; i < Results.Length; i++)
            {
                Results[i] = i.ToString();
            }
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.IsEnabled = true;
            });
        }
        private void Button_NewGame(object sender, RoutedEventArgs e)
        {
            newGame();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Button = (Button)sender;
            if (vsplayer)
            {
                if (PlayerTurn)
                {
                    char lastCharacter = Button.Name[Button.Name.Length - 1];
                    Button.Content = "X";
                    Results[Convert.ToInt32(lastCharacter) - 49] = "X";
                }
                else
                {
                    char lastCharacter = Button.Name[Button.Name.Length - 1];
                    Button.Content = "O";
                    Results[Convert.ToInt32(lastCharacter) - 49] = "O";
                }

                PlayerTurn = !PlayerTurn;
                Button.IsEnabled = false;
                if (determineWinner()) return;
            }
            else
            {
                //Player Move
                Button.Content = "X";
                Container.Children.Cast<Button>().ToList().IndexOf(Button);
                Results[Container.Children.Cast<Button>().ToList().IndexOf(Button)] = "X";
                Button.IsEnabled = false;
                if (determineWinner()) return;
                //AI Move
                miniMax(Results, true, 1);
                Container.Children.Cast<Button>().ToList().ElementAt(choice).Content = "O";
                Container.Children.Cast<Button>().ToList().ElementAt(choice).IsEnabled = false;
                Results[choice] = "O";
                if (determineWinner()) return;
            }
        }

        private bool CheckWin(string[] board, string winner)
        {
            numSymbols = 0;
            for (int i = 0; i < 8; i++)
            {
                int startpos = startingpos[i];
                int direction = directions[i];
                string initalsymbol = board[startpos];
                bool isbreak = false;

                for (int j = 0; j < 3; j++)
                {
                    int pos = startpos + direction * j;
                    string currentsymbol = board[pos];

                    if ((currentsymbol != initalsymbol) || (currentsymbol != "X" && currentsymbol != "O"))
                    {
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                {
                    continue;
                }
                if (initalsymbol == "X" && winner == "Player")
                {
                    return true;
                }
                else if(winner == "AI")
                {
                    return true;
                }
            }
            if (GetEmptySpaces(board).Count == 0 && winner == "Draw")
            {
                return true;
            }
            return false;
        }
        private bool determineWinner()
        {
            if (CheckWin(Results, "Player"))
            {
                MessageBox.Show($"The winner is X.");
                newGame();
                return true;
            }
            if (CheckWin(Results, "AI"))
            {
                MessageBox.Show("The winner is O");
                newGame();
                return true;
            }
            if (CheckWin(Results, "Draw"))
            {
                MessageBox.Show("Draw.");
                newGame();
                return true;
            }
            return false;
        }
        private List<int> GetEmptySpaces(string[]board)
        {
            List<int> emptySpaces = new List<int>();
            for(int i = 0; i<board.Length; i++)
            {
                if (board[i] != "X" && board[i] != "O")
                {
                    emptySpaces.Add(i);
                }
            }
            return emptySpaces;
        }
        #endregion
        #region Minimax
        private string[] DoMove(string[] board, int move, bool maximizingPlayer)
        {
            string[] copyposition = copyGrid(board);
            if (maximizingPlayer)
            {
                copyposition[move] = "O";
            }
            else
            {
                copyposition[move] = "X";
            }
            return copyposition;
        }
        private int miniMax(string[] board, bool maximizingPlayer, int depth)
        {
            depth += 1;
            string[] copyboard = copyGrid(board);

            if (score(copyboard, depth) != 0)
                return score(copyboard, depth);
            else if (GetEmptySpaces(copyboard).Count == 0)
                return 0;

            List<int> scores = new List<int>();
            List<int> moves = new List<int>();

            for (int i = 0; i < copyboard.Length; i++)
            {
                if (board[i] != "X" && board[i] != "O")
                {
                    scores.Add(miniMax(DoMove(copyboard, i, maximizingPlayer), !maximizingPlayer, depth));
                    moves.Add(i);
                }
            }

            if (maximizingPlayer)
            {
                int maxindex = scores.IndexOf(scores.Max());
                choice = moves[maxindex];
                return scores.Max();
            }
            else
            {
                int minindex = scores.IndexOf(scores.Min());
                choice = moves[minindex];
                return scores.Min();
            }
        }
        private int score(string[] board, int depth)
        {
            if (CheckWin(board, "Player"))
            {
                return depth -10;
            }
            else if (CheckWin(board, "AI"))
            {
                return 10 - depth;
            }
            else
                return 0;
        }
        private string[] copyGrid(string[] board)
        {
            string[] clone = new string[9];
            for (int i = 0; i < 9; i++){
                clone[i] = board[i];
            }
            return clone;
        }
        #endregion

    }
}

