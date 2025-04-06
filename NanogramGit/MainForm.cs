using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public partial class MainForm : Form
    {
        private Label lblWelcome = new Label();
        private Button btnLogout = new Button();
        private User _currentUser;

        private void InitializeComponent()
        {

        }

        public MainForm(User user)
        {
            _currentUser = user;
            InitializeComponent();  // Zorg ervoor dat je ook de Windows Forms designer aanroept

            this.Text = "Welkom bij NonogramApp";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Welkomstlabel
            lblWelcome.Text = $"Welkom, {_currentUser.FirstName} {_currentUser.LastName}!";
            lblWelcome.Top = 50;
            lblWelcome.Left = 50;
            lblWelcome.Width = 300;
            lblWelcome.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);

            // Uitlogknop
            btnLogout.Text = "Uitloggen";
            btnLogout.Top = 150;
            btnLogout.Left = 150;
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                new LoginForm().ShowDialog();
                this.Close();
            };

            // Voeg componenten toe aan het formulier
            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnLogout);
        }

        // Hier kun je andere methodes toevoegen die de functionaliteit van MainForm uitbreiden.
    }
}
