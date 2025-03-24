using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonogramApp
{
    public abstract class PuzzleGame
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Grid { get; protected set; } = null!;
        public List<List<int>> RowHints { get; protected set; } = null!;
        public List<List<int>> ColumnHints { get; protected set; } = null!;

        public abstract void GeneratePuzzle();
        public abstract bool SolvePuzzle();
        public abstract void ShowSolution();
        public abstract bool CheckUserSolution();
    }

    public class NonogramGame : PuzzleGame
    {
        private bool[,] solutionGrid = null!;

        public NonogramGame(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new bool[width, height];
            solutionGrid = new bool[width, height];
            RowHints = new List<List<int>>();
            ColumnHints = new List<List<int>>();
        }

        public override void GeneratePuzzle()
        {
            try
            {
                var rand = new Random();
                int fillRate = rand.Next(30, 60);

                for (int i = 0; i < Width; i++)
                    for (int j = 0; j < Height; j++)
                        solutionGrid[i, j] = rand.Next(100) < fillRate;

                GenerateHintsFromSolution();
                HideSolution();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating puzzle: " + ex.Message);
            }
        }

        public override bool SolvePuzzle()
        {
            try
            {
                ShowSolution();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error solving puzzle: " + ex.Message);
                return false;
            }
        }

        public override void ShowSolution()
        {
            Array.Copy(solutionGrid, Grid, solutionGrid.Length);
        }

        public override bool CheckUserSolution()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    if (Grid[i, j] != solutionGrid[i, j])
                        return false;
            return true;
        }

        private void GenerateHintsFromSolution()
        {
            RowHints.Clear();
            ColumnHints.Clear();

            for (int y = 0; y < Height; y++)
                RowHints.Add(GetHintsForLine(solutionGrid, y, true));

            for (int x = 0; x < Width; x++)
                ColumnHints.Add(GetHintsForLine(solutionGrid, x, false));
        }

        private List<int> GetHintsForLine(bool[,] grid, int index, bool isRow)
        {
            var hints = new List<int>();
            int count = 0;
            int length = isRow ? Width : Height;

            for (int i = 0; i < length; i++)
            {
                bool value = isRow ? grid[i, index] : grid[index, i];
                if (value) count++;
                else if (count > 0)
                {
                    hints.Add(count);
                    count = 0;
                }
            }

            if (count > 0) hints.Add(count);
            return hints.Count == 0 ? new List<int> { 0 } : hints;
        }

        private void HideSolution()
        {
            Grid = new bool[Width, Height];
        }
    }

    public class GameController
    {
        private readonly int width;
        private readonly int height;
        private readonly int puzzleCount;
        public List<PuzzleGame> Puzzles { get; private set; }

        public GameController(int width, int height, int puzzleCount)
        {
            this.width = width;
            this.height = height;
            this.puzzleCount = puzzleCount;
            Puzzles = new List<PuzzleGame>();
        }

        public void GenerateAllPuzzles()
        {
            try
            {
                Puzzles.Clear();
                for (int i = 0; i < puzzleCount; i++)
                {
                    var game = new NonogramGame(width, height);
                    game.GeneratePuzzle();
                    Puzzles.Add(game);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating puzzles: " + ex.Message);
            }
        }
    }

    public class MainForm : Form
    {
        private PuzzleGame game = null!;
        private int currentPuzzleIndex = -1;
        private GameController controller = null!;
        private const int cellSize = 30;
        private Button nextPuzzleButton = null!;
        private Button solvePuzzleButton = null!;
        private Button solveAsyncButton = null!;
        private Button checkSolutionButton = null!;
        private System.Windows.Forms.Timer refreshTimer = null!;

        public MainForm()
        {
            this.Text = "Nonogram App";
            this.ClientSize = new Size(700, 600);
            this.DoubleBuffered = true;

            controller = new GameController(10, 10, 5);
            controller.GenerateAllPuzzles();

            InitializeButtons();
            SetupTimer();
            LoadNextPuzzle();
        }

        private void InitializeButtons()
        {
            nextPuzzleButton = new Button { Text = "Next Puzzle", Location = new Point(10, 520), Size = new Size(120, 30) };
            nextPuzzleButton.Click += (sender, e) => LoadNextPuzzle();
            this.Controls.Add(nextPuzzleButton);

            solvePuzzleButton = new Button { Text = "Show Solution", Location = new Point(150, 520), Size = new Size(120, 30) };
            solvePuzzleButton.Click += (sender, e) => ShowSolution();
            this.Controls.Add(solvePuzzleButton);

            solveAsyncButton = new Button { Text = "Solve (Async)", Location = new Point(290, 520), Size = new Size(120, 30) };
            solveAsyncButton.Click += async (sender, e) => await SolvePuzzleAsync();
            this.Controls.Add(solveAsyncButton);

            checkSolutionButton = new Button { Text = "Check Solution", Location = new Point(430, 520), Size = new Size(120, 30) };
            checkSolutionButton.Click += (sender, e) => CheckSolution();
            this.Controls.Add(checkSolutionButton);
        }

        private void SetupTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 100;
            refreshTimer.Tick += (s, e) => Invalidate();
            refreshTimer.Start();
        }

        private void LoadNextPuzzle()
        {
            currentPuzzleIndex = (currentPuzzleIndex + 1) % controller.Puzzles.Count;
            game = controller.Puzzles[currentPuzzleIndex];
            Invalidate();
        }

        private async Task SolvePuzzleAsync()
        {
            solveAsyncButton.Enabled = false;
            try
            {
                bool solved = await Task.Run(() => game.SolvePuzzle());
                MessageBox.Show(solved ? "Puzzle solved!" : "Could not solve puzzle.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during async solve: " + ex.Message);
            }
            finally
            {
                solveAsyncButton.Enabled = true;
                Invalidate();
            }
        }

        private void ShowSolution()
        {
            try
            {
                game.ShowSolution();
                Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing solution: " + ex.Message);
            }
        }

        private void CheckSolution()
        {
            try
            {
                if (game.CheckUserSolution())
                    MessageBox.Show("Well done! Correct solution.");
                else
                    MessageBox.Show("Oops! Incorrect solution.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking solution: " + ex.Message);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (game == null) return;

            int offset = 100;

            for (int i = 0; i < game.Width; i++)
            {
                for (int j = 0; j < game.Height; j++)
                {
                    Rectangle rect = new Rectangle(offset + i * cellSize, offset + j * cellSize, cellSize, cellSize);
                    e.Graphics.FillRectangle(game.Grid[i, j] ? Brushes.Black : Brushes.White, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
            }

            for (int i = 0; i < game.Width; i++)
            {
                string hint = string.Join(" ", game.ColumnHints[i]);
                e.Graphics.DrawString(hint, DefaultFont, Brushes.Black, offset + i * cellSize, offset - 20);
            }

            for (int j = 0; j < game.Height; j++)
            {
                string hint = string.Join(" ", game.RowHints[j]);
                e.Graphics.DrawString(hint, DefaultFont, Brushes.Black, offset - 40, offset + j * cellSize);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            try
            {
                int offset = 100;
                int x = (e.X - offset) / cellSize;
                int y = (e.Y - offset) / cellSize;

                if (x >= 0 && x < game.Width && y >= 0 && y < game.Height)
                {
                    game.Grid[x, y] = !game.Grid[x, y];
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mouse click error: " + ex.Message);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
