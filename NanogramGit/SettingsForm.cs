using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public class SettingsForm : Form
    {
        private ComboBox cbTheme = new ComboBox();
        private NumericUpDown nudGrid = new NumericUpDown();
        private Button btnSave = new Button();
        private User user;
        private Action<User> onSave;

        public SettingsForm(User user, Action<User> onSaveCallback)
        {
            this.user = user;
            this.onSave = onSaveCallback;

            this.Text = "Settings";
            this.Size = new System.Drawing.Size(250, 200);

            cbTheme.Items.AddRange(new[] { "light", "dark" });
            cbTheme.SelectedItem = user.Settings.Theme;
            cbTheme.Top = 20;
            cbTheme.Left = 20;

            nudGrid.Minimum = 5;
            nudGrid.Maximum = 20;
            nudGrid.Value = user.Settings.GridSize;
            nudGrid.Top = 60;
            nudGrid.Left = 20;

            btnSave.Text = "Save";
            btnSave.Top = 100;
            btnSave.Left = 20;
            btnSave.Click += (s, e) =>
            {
                user.Settings.Theme = cbTheme.SelectedItem?.ToString() ?? "light";
                user.Settings.GridSize = (int)nudGrid.Value;
                onSave(user);
                this.Close();
            };

            this.Controls.Add(cbTheme);
            this.Controls.Add(nudGrid);
            this.Controls.Add(btnSave);
        }
    }
}
