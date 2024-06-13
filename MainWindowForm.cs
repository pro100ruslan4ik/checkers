namespace Checkers
{
    public partial class MainWindowForm : System.Windows.Forms.Form
    {
        private const int BOARD_SIZE = 8;
        private const int CELL_SIZE = 50;

        private int currentPlayer;

        int countEatSteps = 0;
        bool isMoving;
        bool isContinueEat = false;

        Button prevButton;
        Button pressedButton;

        List<Button> simpleSteps = new List<Button>();

        private int[,] board = new int[BOARD_SIZE, BOARD_SIZE];
        private Button[,] buttons = new Button[BOARD_SIZE, BOARD_SIZE];

        private Image whiteCheckerImage;
        private Image whiteQueenImage;

        private Image blackCheckerImage;
        private Image blackQueenImage;

        /// <summary>
        /// Конструктор формы основного окна
        /// </summary>
        public MainWindowForm()
        {
            InitializeComponent();

            this.Text = "Checkers Delta";

            Bitmap[] sprites = GetSprites();
            whiteCheckerImage = sprites[0];
            //whiteQueenImage = sprites[1];
            blackCheckerImage = sprites[2];
            //blackQueenImage = sprites[3];


            Init();
        }

        /// <summary>
        /// Инициализирует шашечную доску
        /// </summary>
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

        /// <summary>
        /// Задает графические элементы доски
        /// </summary>
        private void CreateBoard()
        {
            this.Width = (BOARD_SIZE + 1) * CELL_SIZE - 32;
            this.Height = (BOARD_SIZE + 1) * CELL_SIZE - 10;

            string cursorPath = @"C:\Users\Ruslan\source\repos\Checkers\Resources\cursor-_1_.cur";
            Cursor customCursorForButtons = new Cursor(cursorPath);

            for (int col = 0; col < BOARD_SIZE; col++)
            {
                for (int row = 0; row < BOARD_SIZE; row++)
                {
                    Button button = new Button();
                    button.Location = new Point(row * CELL_SIZE, col * CELL_SIZE);
                    button.Size = new Size(CELL_SIZE, CELL_SIZE);
                    button.Click += new EventHandler(OnCellClick);
                    button.Cursor = customCursorForButtons;

                    if (board[col, row] == 1)
                        button.Image = whiteCheckerImage;

                    if (board[col, row] == 2)
                        button.Image = blackCheckerImage;

                    button.BackColor = GetButtonColorByCoordOnBoard(button);
                    button.ForeColor = Color.Orange;

                    buttons[col, row] = button;

                    this.Controls.Add(button);
                }
            }
        }

        /// <summary>
        /// Окончание и сброс игры, если у одного из игроков не осталось фигур
        /// </summary>
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

            if (!player1)
            {
                MessageBox.Show("Черные победили!");
                this.Controls.Clear();
                Init();
            }
            else if (!player2)
            {
                MessageBox.Show("Белые победили!");
                this.Controls.Clear();
                Init();
            }
        }

        /// <summary>
        /// Сменить игрока
        /// </summary>
        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ResetGame();
        }

        /// <summary>
        /// Возвращает цвет кнопки, который она должна иметь на шашечной доске
        /// </summary>
        /// <param name="button"></param>
        /// <returns>Color.Gray или Color.White</returns>
        private Color GetButtonColorByCoordOnBoard(Button button)
        {
            if ((button.Location.Y + button.Location.X) / CELL_SIZE % 2 != 0)
            {
                return Color.Gray;
            }
            return Color.White;
        }

        /// <summary>
        /// Обработчик нажатия на клетку-кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellClick(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetButtonColorByCoordOnBoard(prevButton);

            pressedButton = sender as Button;
            int pressedButtonOnBoard = board[pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE];

            if (pressedButtonOnBoard != 0 && pressedButtonOnBoard == currentPlayer)
            {
                ResetColorForAllButtons();
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
                    ResetColorForAllButtons();
                    pressedButton.BackColor = GetButtonColorByCoordOnBoard(pressedButton);
                    HighlightAttackingFigures();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    isContinueEat = false;
                    if (Math.Abs(pressedButton.Location.X / CELL_SIZE - prevButton.Location.X / CELL_SIZE) > 1)
                    {
                        isContinueEat = true;
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

                    PromoteToQueen(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;

                    ResetColorForAllButtons();
                    DeactivateAllButtons();

                    if (pressedButton.Text == "D")
                        ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE, false);
                    else
                        ShowSteps(pressedButton.Location.Y / CELL_SIZE, pressedButton.Location.X / CELL_SIZE);

                    if (countEatSteps == 0 || !isContinueEat)
                    {
                        ResetColorForAllButtons();
                        SwitchPlayer();
                        HighlightAttackingFigures();
                        isContinueEat = false;
                    }
                    else if (isContinueEat)
                    {
                        pressedButton.BackColor = Color.Orange;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                }
            }
            prevButton = pressedButton;
        }

        /// <summary>
        /// Выделяет те фигуры, у которых есть съедобный ход (В самом начале хода), при наличии таковых
        /// Делает ход таковой фигурой обязательным
        /// </summary>
        private void HighlightAttackingFigures()
        {
            bool isChecker_NotQueen = true;
            bool isEatStep = false;
            DeactivateAllButtons();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Text == "D")
                            isChecker_NotQueen = false;
                        else
                            isChecker_NotQueen = true;
                        if (IsButtonHasEatStep(i, j, isChecker_NotQueen, new int[2] { 0, 0 }))
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

        /// <summary>
        /// Превращает шашку в дамку, изменяя текст кнопки на "D", если шашка оказалась на краю доски
        /// </summary>
        /// <param name="button">Кнопка, на которую ступила шашка</param>
        private void PromoteToQueen(Button button)
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

        /// <summary>
        /// Удаляет шашку между конечной и начальной кнопками, а также шашку в начальной кнопке
        /// </summary>
        /// <param name="endButton"></param>
        /// <param name="startButton"></param>
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

        /// <summary>
        /// Показать возможные шаги, для текущей кнопки 
        /// </summary>
        /// <param name="iCurrFigure">Вертикальный индекс текущей кнопки</param>
        /// <param name="jCurrFigure">Горизонтальный индекс текущей кнопки</param>
        /// <param name="isChecker_NotQueen">Является шашкой - true, дамкой - false</param>
        private void ShowSteps(int iCurrFigure, int jCurrFigure, bool isChecker_NotQueen = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isChecker_NotQueen);

            //Оставляем только съедобные ходы
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        /// <summary>
        /// Вызывает метод DeterminePath во всех 4 направлениях
        /// </summary>
        /// <param name="IcurrFigure">Вертикальный индекс текущей кнопки</param>
        /// <param name="JcurrFigure">Горизонтальный индекс текущей кнопки</param>
        /// <param name="isChecker_NotQueen">Является шашкой - true, дамкой - false</param>
        private void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isChecker_NotQueen = false)
        {
            int j;

            //Вправо вверх
            j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isChecker_NotQueen && !isContinueEat) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            //Влево вверх
            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isChecker_NotQueen && !isContinueEat) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            //Влево вниз
            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isChecker_NotQueen && !isContinueEat) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            //Вправо вниз
            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isChecker_NotQueen && !isContinueEat) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isChecker_NotQueen)
                    break;
            }
        }

        /// <summary>
        /// Определяет, можно ли переместить шашку на заданную кнопку. Если она пуста, то делает кнопку активной для перемещения.
        /// Если не пуста, то вызывает показ возможных съедобных ходов
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="tj"></param>
        /// <returns></returns>
        private bool DeterminePath(int ti, int tj)
        {
            //Если в этом направлении соседняя клетка свободна и она еще никого не ела,
            //то красим ее в желтый, включаем ее, а также добавляем в простые ходы
            if (board[ti, tj] == 0 && !isContinueEat)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {
                //Если сосед - враг, то вызываем метод показывающий возможный съедобный ход в этом направлении
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

        /// <summary>
        /// Показывает возможные съедобные ходы с нажатой кнопки в сторону кнопки, заданной координатами i и j. 
        /// Приоритет отдается ходам, которые имеют продолжение
        /// </summary>
        /// <param name="i">Вертикальный индекс соседней кнопки, на которую можно сходить</param>
        /// <param name="j">Горизонтальный индекс соседней кнопки, на которую можно сходить</param>
        /// <param name="isChecker_NotQueen">Является шашкой - true, дамкой - false</param>
        private void ShowProceduralEat(int i, int j, bool isChecker_NotQueen = true)
        {
            //Вычисление направления, в котором будет совершен ход 
            int dirX = i - pressedButton.Location.Y / CELL_SIZE;
            int dirY = j - pressedButton.Location.X / CELL_SIZE;

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int il = i;
            int jl = j;

            bool NoEnemyInThisDir = true;

            while (IsInsideBorders(il, jl))
            {
                if (board[il, jl] != 0 && board[il, jl] != currentPlayer)
                {
                    NoEnemyInThisDir = false;
                    break;
                }
                il += dirX;
                jl += dirY;

                if (isChecker_NotQueen)
                    break;
            }

            if (NoEnemyInThisDir)
                return;

            List<Button> buttonsForNextEatStep = new List<Button>();

            //Нужно ли закрыть простые ходы
            bool needCloseSimple = false;

            int ik = il + dirX;
            int jk = jl + dirY;

            while (IsInsideBorders(ik, jk))
            {
                if (board[ik, jk] == 0)
                {
                    //Если нашелся ход с серией убийств, то нужно закрыть простые ходы
                    if (IsButtonHasEatStep(ik, jk, isChecker_NotQueen, new int[2] { dirX, dirY }))
                    {
                        needCloseSimple = true;
                    }
                    else
                    {
                        buttonsForNextEatStep.Add(buttons[ik, jk]);
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countEatSteps++;
                }
                else
                    break;

                if (isChecker_NotQueen)
                    break;

                jk += dirY;
                ik += dirX;
            }

            //Если нашелся сложный ход, а мы знаем, что buttonsForNextEatStep содержит только простые ходы, то мы их скрываем
            if (needCloseSimple && buttonsForNextEatStep.Count > 0)
            {
                CloseSimpleSteps(buttonsForNextEatStep);
            }

        }

        /// <summary>
        /// Закрывает ходы с низким приоритетом, то есть не дает сходить на кнопки, после которых не будет 
        /// возможности съесть еще одну шашку, если таковые ходы имеются. Задает значение свойства Enabled как false 
        /// для полученных кнопок и возвращает их исходный цвет
        /// </summary>
        /// <param name="simpleSteps">Список кнопок простых ходов, которые не имеют продолжения</param>
        private void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetButtonColorByCoordOnBoard(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Проверяет, есть ли у фигуры в нажатой кнопке ходы, в которых можно съесть шашку противника
        /// </summary>
        /// <param name="IcurrFigure">Горизонтальный индекс фигуры</param>
        /// <param name="JcurrFigure">Вертикальный индекс фигуры</param>
        /// <param name="isChecker_NotQueen">Является шашкой - true, дамкой - false</param>
        /// <param name="dirPrev">Массив из 2 элементов - направление по i и j сделанного хода (предыдущего съедания в этом ходу)</param>
        /// <returns></returns>
        private bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isChecker_NotQueen, int[] dirPrev)
        {
            bool hasEatStep = false;
            int j;

            //i направлено сверху вниз      при уменьшении идем вверх
            //j направлено слева направо    при увеличении идем вправо

            //Проверка хода вправо вверх
            j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                //Если это первый игрок, то его шашка не может сходить вправо вверх, если она не дамка или не совершает серию убийств
                if (currentPlayer == 1 && isChecker_NotQueen && !isContinueEat) break;

                //Если мы в этом ходу уже съели шашку в направлении вниз влево, то проверять в этом случае не нужно для обычной шашки
                if (dirPrev[0] == 1 && dirPrev[1] == -1 && !isChecker_NotQueen) break;


                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        hasEatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            hasEatStep = false;
                        else if (board[i - 1, j + 1] != 0)
                            hasEatStep = false;
                        else
                            return hasEatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                //Если проверяется шашка, то она проверяет только ближайшую клетку
                if (isChecker_NotQueen)
                    break;
            }

            //Проверка хода влево вверх
            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isChecker_NotQueen && !isContinueEat) break;
                if (dirPrev[0] == 1 && dirPrev[1] == 1 && !isChecker_NotQueen) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        hasEatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            hasEatStep = false;
                        else if (board[i - 1, j - 1] != 0)
                            hasEatStep = false;
                        else
                            return hasEatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            //Проверка хода влево вниз
            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isChecker_NotQueen && !isContinueEat) break;
                if (dirPrev[0] == -1 && dirPrev[1] == 1 && !isChecker_NotQueen) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        hasEatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            hasEatStep = false;
                        else if (board[i + 1, j - 1] != 0)
                            hasEatStep = false;
                        else
                            return hasEatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            //Проверка хода вправо вниз
            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isChecker_NotQueen && !isContinueEat) break;
                if (dirPrev[0] == -1 && dirPrev[1] == -1 && !isChecker_NotQueen) break;
                if (IsInsideBorders(i, j))
                {
                    if (board[i, j] != 0 && board[i, j] != currentPlayer)
                    {
                        hasEatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                            hasEatStep = false;
                        else if (board[i + 1, j + 1] != 0)
                            hasEatStep = false;
                        else
                            return hasEatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isChecker_NotQueen)
                    break;
            }

            return hasEatStep;
        }

        /// <summary>
        /// Сбрасывает цвет всех кнопок в изначальные для шашечного поля цвета
        /// </summary>
        private void ResetColorForAllButtons()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].BackColor = GetButtonColorByCoordOnBoard(buttons[col, row]);
        }

        /// <summary>
        /// Проверка на вхождение индексов в границы доски
        /// </summary>
        /// <param name="tX">Горизонтальный индекс</param>
        /// <param name="tY">Вертикальный индекс</param>
        private bool IsInsideBorders(int tX, int tY)
        {
            if (tX >= BOARD_SIZE || tX < 0 || tY >= BOARD_SIZE || tY < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Изменяет свойство Enabled всех кнопок на false
        /// </summary>
        private void DeactivateAllButtons()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].Enabled = false;
        }

        /// <summary>
        /// Изменяет свойство Enabled всех кнопок на true
        /// </summary>
        private void ActivateAllButtons()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
                for (int row = 0; row < BOARD_SIZE; row++)
                    buttons[col, row].Enabled = true;
        }

        /// <summary>
        /// Возвращает массив картинок фигур
        /// </summary>
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