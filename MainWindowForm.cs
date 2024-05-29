using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class MainWindowForm : System.Windows.Forms.Form
    {
        private const int BoardSize = 8;
        private Button[,] boardButtons = new Button[BoardSize, BoardSize];
        private FigureColor playerFigureColor = FigureColor.White;

        public MainWindowForm()
        {
            InitializeComponent();
            this.Text = "Шашки";
            this.Size = new Size(480, 503);
            InitializeBoard();
        }

        private void MainWindowForm_Load(object sender, EventArgs e)
        {

        }
        private void InitializeBoard()
        {
            Panel boardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.DarkGray
            };
            this.Controls.Add(boardPanel);

            int buttonSize = boardPanel.Width / BoardSize;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * buttonSize, row * buttonSize),
                        FlatStyle = FlatStyle.Flat
                    };

                    // Определение цвета клетки
                    if ((row + col) % 2 == 0)
                    {
                        button.BackColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.Black;
                        button.Click += Button_Click;
                    }


                    // Добавление кнопки в панель
                    boardPanel.Controls.Add(button);
                    boardButtons[row, col] = button;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            // Здесь будет логика обработки кликов по клеткам
            MessageBox.Show("Клетка нажата!");
        }

    }
}