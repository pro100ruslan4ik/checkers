namespace Checkers
{
    public partial class MenuForm : System.Windows.Forms.Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {

        }
        private void MainWindowForm_Closing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }

        private void PvP_Button_Click(object sender, EventArgs e)
        {
            MainWindowForm mainWindow = new MainWindowForm();
            mainWindow.Show();
            mainWindow.FormClosing += MainWindowForm_Closing;
            this.Hide();
        }
    }
}