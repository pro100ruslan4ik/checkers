namespace Checkers
{
    public partial class MainWindowForm : System.Windows.Forms.Form
    {
        private const int BOARD_SIZE = 8;
        private const int CELL_SIZE = 50;

        private int currentPlayer;

        int countEatSteps = 0;
        bool isMoving;
        bool isContinue = false;

        Button prevButton;
        Button pressedButton;

        List<Button> simpleSteps = new List<Button>();

        private int[,] board = new int[BOARD_SIZE, BOARD_SIZE];
        private Button[,] buttons = new Button[BOARD_SIZE, BOARD_SIZE];

        private Image whiteChecker;
        private Image whiteQueen;

        private Image blackChecker;
        private Image blackQueen;


        public MainWindowForm()
        {
            InitializeComponent();

            this.Text = "Checkers Delta";

            Bitmap[] sprites = GetSprites();
            whiteChecker = sprites[0];
            whiteQueen = sprites[1];
            blackChecker = sprites[2];
            blackQueen = sprites[3];



            Init();
        }

        private void Init()
        {
            currentPlayer = 1;
            isMoving = false;
            prevButton = null;

            board = new int[BOARD_SIZE, BOARD_SIZE]
            {
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {2,0,2,0,2,0,2,0},
                {0,2,0,2,0,2,0,2},
                {2,0,2,0,2,0,2,0}
            };
            CreateBoard();
        }

        private void CreateBoard()
        {
            this.Width = (BOARD_SIZE + 1) * CELL_SIZE;
            this.Height = (BOARD_SIZE + 1) * CELL_SIZE;

            for (int col = 0; col < BOARD_SIZE; col++)
            {
                for (int row = 0; row < BOARD_SIZE; row++)
                {
                    Button button = new Button();
                    button.Location = new Point(row * CELL_SIZE, col * CELL_SIZE);
                    button.Size = new Size(CELL_SIZE, CELL_SIZE);
                    button.Click += new EventHandler(OnCellClick);

                    if (board[col, row] == 1)
                        button.Image = whiteChecker;

                    if (board[col, row] == 2)
                        button.Image = blackChecker;

                    button.BackColor = GetPrevButtonColor(button);
                    button.ForeColor = Color.Orange;

                    buttons[col, row] = button;

                    this.Controls.Add(button);
                }
            }
        }
        private void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == 1)
                        player1 = true;
                    if (board[i, j] == 2)
                        player2 = true;
                }
            if (!player1 || !player2)
            {
                this.Controls.Clear();
                Init();
            }
        }
        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ResetGame();
        }
        private Color GetPrevButtonColor(Button prevButton)
        {
            if ((prevButton.Location.Y / CELL_SIZE % 2) != 0 && (prevButton.Location.X / CELL_SIZE % 2) == 0)
            {
                return Color.Gray;
            }
            if ((prevButton.Location.Y / CELL_SIZE % 2) == 0 && (prevButton.Location.X / CELL_SIZE % 2) != 0)
            {
                return Color.Gray;
            }
            return Color.White;
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor(prevButton);

            pressedButton = sender as Button;
            int pressedButtonOnBoard = board[pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE];

            if (pressedButtonOnBoard != 0 && pressedButtonOnBoard == currentPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Orange;
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                countEatSteps = 0;

                if (pressedButton.Text == "D")
                    ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE, false);
                else
                    ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowPossibleSteps();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false;
                    if (Math.Abs(pressedButton.Location.X / CELL_SIZE - prevButton.Location.X / CELL_SIZE) > 1)
                    {
                        isContinue = true;
                        DeleteEaten(pressedButton, prevButton);
                    }

                    int temp = board[pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE];
                    board[pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE] =
                        board[prevButton.Location.Y / CELL_SIZE, prevButton.Location.X / CELL_SIZE];
                    board[prevButton.Location.Y / CELL_SIZE, prevButton.Location.X / CELL_SIZE] = temp;

                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;

                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";

                    SwitchButtonToCheat(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;

                    CloseSteps();
                    DeactivateAllButtons();

                    if (pressedButton.Text == "D")
                        ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE, false);
                    else
                        ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE);

                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        SwitchPlayer();
                        ShowPossibleSteps();
                        isContinue = false;
                    }
                    else if (isContinue)
                    {
                        pressedButton.BackColor = Color.Orange;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                }
            }
            prevButton = pressedButton;
        }

        private void ShowPossibleSteps()
        {
            bool isOneStep = true;
            bool isEatStep = false;
            DeactivateAllButtons();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Text == "D")
                            isOneStep = false;
                        else
                            isOneStep = true;
                        if (IsButtonHasEatStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isEatStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep)
                ActivateAllButtons();
        }

        private void SwitchButtonToCheat(Button button)
        {
            if (board[button.Location.Y / CELL_SIZE, button.Location.X / CELL_SIZE] == 1 && button.Location.Y / CELL_SIZE == BOARD_SIZE - 1)
            {
                button.Text = "D";
            }
            if (board[button.Location.Y / CELL_SIZE, button.Location.X / CELL_SIZE] == 2 && button.Location.Y / CELL_SIZE == 0)
            {
                button.Text = "D";
            }

        }

        private void DeleteEaten(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.Location.Y / CELL_SIZE - startButton.Location.Y / CELL_SIZE);

            int startIndexX = endButton.Location.Y / CELL_SIZE - startButton.Location.Y / CELL_SIZE;
            int startIndexY = endButton.Location.X / CELL_SIZE - startButton.Location.X / CELL_SIZE;

            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;

            int currCount = 0;
            int i = startButton.Location.Y / CELL_SIZE + startIndexX;
            int j = startButton.Location.X / CELL_SIZE + startIndexY;

            while (currCount < count - 1)
            {
                board[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";

                i += startIndexX;
                j += startIndexY;

                currCount++;
            }
        }

        private void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOneStep = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isOneStep);
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        private void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j;

            j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        private bool DeterminePath(int ti, int tj)
        {
            if (board[ti, tj] == 0 && !isContinue)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {
                if (board[ti, tj] != currentPlayer)
                {
                    if (pressedButton.Text == "D")
                        ShowProceduralEat(ti, tj, false);
                    else
                        ShowProceduralEat(ti, tj);
                }

                return false;
            }
            return true;
        }

        private void ShowProceduralEat(int i, int j, bool isOneStep = true)
        {
            int dirX = i - pressedButton.Location.Y / CELL_SIZE;
            int dirY = j - pressedButton.Location.X / CELL_SIZE;

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int il = i;
            int jl = j;

            bool isEmpty = true;

            while (IsInsideBorders(il, jl))
            {
                if (board[il, jl] != 0 && board[il, jl] != currentPlayer)
                {
                    isEmpty = false;
                    break;
                }
                il += dirX;
                jl += dirY;

                if (isOneStep)
                    break;

            }

            if (isEmpty)
                return;

            List<Button> toClose = new List<Button>();

            bool closeSimple = false;

            int ik = il + dirX;
            int jk = jl + dirY;

            while (IsInsideBorders(ik, jk))
            {
                if (board[ik, jk] == 0)
                {
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        closeSimple = true;
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]);
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countEatSteps++;
                }
                else
                    break;

                if (isOneStep)
                    break;

                jk += dirY;
                ik += dirX;
            }

            if (closeSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }

        }

        private void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        private bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep, int[] dir)
        {
            bool eatStep = false;
            int j;

            j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            eatStep = false;
                        else if (board[i - 1, j + 1] != 0)
                            eatStep = false;
                        else
                            return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else if (board[i - 1, j - 1] != 0)
                            eatStep = false;
                        else
                            return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            eatStep = false;
                        else if (board[i + 1, j - 1] != 0)
                            eatStep = false;
                        else
                            return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                            eatStep = false;
                        else if (board[i + 1, j + 1] != 0)
                            eatStep = false;
                        else
                            return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            return eatStep;
        }
        private void CloseSteps()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].BackColor = GetPrevButtonColor(buttons[col, row]);
        }
        private bool IsInsideBorders(int tX, int tY)
        {
            if (tX >= BOARD_SIZE || tX < 0 || tY >= BOARD_SIZE || tY < 0)
            {
                return false;
            }
            return true;
        }

        private void DeactivateAllButtons()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].Enabled = false;
        }
        private void ActivateAllButtons()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].Enabled = true;
        }

        private Bitmap[] GetSprites()
        {
            Bitmap spriteSheet = new Bitmap(@"C:\Users\Ruslan\source\repos\Checkers\pixelCheckers _v1.0\checkers_topDown.png");

            int spriteWidth = spriteSheet.Width / 4;
            int spriteHeight = spriteSheet.Height;

            Bitmap[] sprites = new Bitmap[4];

            for (int i = 0; i < 4; i++)
            {
                Bitmap sprite = new Bitmap(spriteWidth, spriteHeight);
                using (Graphics g = Graphics.FromImage(sprite))
                {
                    g.DrawImage(spriteSheet, new Rectangle(0, 0, spriteWidth, spriteHeight),
                        new Rectangle(i * spriteWidth, 0, spriteWidth, spriteHeight), GraphicsUnit.Pixel);
                }
                sprites[i] = new Bitmap(sprite, new Size(sprite.Width * 3 + 3, sprite.Height * 3 + 3));
            }

            return sprites;
        }

    }
}