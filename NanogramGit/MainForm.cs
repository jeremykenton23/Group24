using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonogramApp
{
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
        private Button settingsButton = null!;
        private Button profileButton = null!;
        private Button logoutButton = null!;
        private Label lblUser = null!;
        private System.Windows.Forms.Timer refreshTimer = null!;

        private User currentUser;
        private UserManager userManager = new UserManager();

        public MainForm(User user)
        {
            currentUser = user;
            this.Text = "Nonogram App";
            this.ClientSize = new Size(750, 600);
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            ApplyTheme();

            controller = new GameController(user.Settings.GridSize, user.Settings.GridSize, 5);
            controller.GenerateAllPuzzles();

            InitializeButtons();
            SetupTimer();
            LoadNextPuzzle();
        }

        private void ApplyTheme()
        {
            this.BackColor = currentUser.Settings.Theme == "dark"
                ? Color.FromArgb(30, 30, 30)
                : SystemColors.Control;
        }

        private void InitializeButtons()
        {
            nextPuzzleButton = CreateButton("Next Puzzle", 10, LoadNextPuzzleClick);
            solvePuzzleButton = CreateButton("Show Solution", 120, ShowSolutionClick);
            solveAsyncButton = CreateButton("Solve (Async)", 230, SolvePuzzleAsyncClick);
            checkSolutionButton = CreateButton("Check", 340, CheckSolutionClick);
            settingsButton = CreateButton("Settings", 450, OpenSettingsClick);
            profileButton = CreateButton("Profile", 560, OpenProfileClick);
            logoutButton = CreateButton("Logout", 670, LogoutClick);

            lblUser = new Label
            {
                Text = $"Logged in: {currentUser.Username}",
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(lblUser);
        }

        private Button CreateButton(string text, int left, EventHandler handler)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(100, 30),
                Location = new Point(left, 520)
            };
            btn.Click += handler;
            this.Controls.Add(btn);
            return btn;
        }

        private void LoadNextPuzzleClick(object sender, EventArgs e)
        {
            LoadNextPuzzle();
        }

        private void ShowSolutionClick(object sender, EventArgs e)
        {
            ShowSolution();
        }

        private async void SolvePuzzleAsyncClick(object sender, EventArgs e)
        {
            await SolvePuzzleAsync();
        }

        private void CheckSolutionClick(object sender, EventArgs e)
        {
            CheckSolution();
        }

        private void OpenSettingsClick(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void OpenProfileClick(object sender, EventArgs e)
        {
            OpenProfile();
        }

        private void LogoutClick(object sender, EventArgs e)
        {
            Logout();
        }

        private void OpenSettings()
        {
            var settingsForm = new SettingsForm(currentUser, user =>
            {
                userManager.UpdateUserSettings(user);
                ApplyTheme();
                controller = new GameController(user.Settings.GridSize, user.Settings.GridSize, 5);
                controller.GenerateAllPuzzles();
                LoadNextPuzzle();
            });
            settingsForm.ShowDialog();
        }

        private void OpenProfile()
        {
            var profileForm = new ProfileForm(currentUser, updatedUser =>
            {
                userManager.UpdateUser(updatedUser);
                lblUser.Text = $"Logged in: {updatedUser.Username}";
            },
            () =>
            {
                userManager.DeleteUser(currentUser);
                MessageBox.Show("Account verwijderd.");
                Logout();
            });

            profileForm.ShowDialog();
        }

        private void Logout()
        {
            this.Hide();
            var login = new LoginForm();
            login.ShowDialog();
            this.Close();
        }

        private void SetupTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 100
            };
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
    }
}
