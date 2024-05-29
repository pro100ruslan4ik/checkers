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
            MainWindowForm mainWindow = new MainWindowForm();
            mainWindow.Show();
            this.Hide();

        }
    }
}