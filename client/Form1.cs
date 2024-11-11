using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO.Ports;
using System.IO;


namespace game_client
{
    public partial class Form1 : Form
    {
        private SoundPlayer _player = new SoundPlayer();
        private int select1User;
        private int select2User;
        private bool winStrategy;
        private bool randomMode;
        private bool musicOn;
        private string mode;
        private bool onLoad;
        private int score1;
        private int score2;
        SerialPort serialPort;
        private List<int> player1Moves = new List<int>();
        private Timer clickTimer;
        private Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            StartMenu();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            panel13.Paint += new PaintEventHandler(DrawCustomBorder);
            panel14.Paint += new PaintEventHandler(DrawCustomBorder);
            panel16.Paint += new PaintEventHandler(DrawCustomBorder);
            panel8.Paint += new PaintEventHandler(DrawCustomBorder);
            panel12.Paint += new PaintEventHandler(DrawCustomBorder);
            panel9.Paint += new PaintEventHandler(DrawCustomBorder);
            panel2.Paint += new PaintEventHandler(DrawCustomBorder);
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            panel3.Paint += new PaintEventHandler(DrawCustomBorder);
            panel5.Paint += new PaintEventHandler(DrawCustomBorder);
            panel7.Paint += new PaintEventHandler(DrawCustomBorder);
            panel17.Paint += new PaintEventHandler(DrawCustomBorder);
        }
        private void button1_Click(object sender, EventArgs e) // кнопка New Game
        {
            ModMenu();
            onLoad = false;
        }
        //Метод для завантаження меню для вибору режимів
        private void ModMenu()
        {
            button1.Visible = false;
            button22.Visible = false;
            button21.Visible = false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            label2.Visible = true;
            panel1.Visible = true;
            panel2.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            
        }
        //Метод для завантаження головного меню
        private void StartMenu()
        {
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {
                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1","");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2","");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3","");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out  winStrategy);
                bool.TryParse(checkBox3Value, out  randomMode);
                if(musicOn==true)
                {
                    
                        _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                        _player.LoadAsync();
                        _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            label2.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            panel3.Visible = false;
            panel8.Visible = false;
            panel13.Visible = false;
        }
        // Метод для малювання кастомної рамки
        private void DrawCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;

            // Задання кольору і товщини пензля для рамки
            Pen pen = new Pen(Color.Aqua, 3);

            // Малюємо прямокутну рамку навколо панелі
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }

        private void button3_Click(object sender, EventArgs e) // кнопка Man Vs Man
        {
            Player1();
            mode = "Man VS Man";
        }
        //функція яка відкриває вибір камінь або ножниці або бумагу користувачу 1
        private void Player1()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            if (mode == "AI VS AI")
            {
                // Ініціалізуємо таймер
                clickTimer = new Timer();
                clickTimer.Interval = 1000; // Затримка 500 мс
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop(); // Зупиняємо таймер

                    // Генеруємо випадкове число 1, 2 або 3
                    int buttonNumber = random.Next(1, 4); // 1, 2 або 3

                    // Вибираємо кнопку для натискання
                    switch (buttonNumber)
                    {
                        case 1:
                            button5.PerformClick();
                            break;
                        case 2:
                            button6.PerformClick();
                            break;
                        case 3:
                            button7.PerformClick();
                            break;
                    }
                };
                clickTimer.Start(); // Запускаємо таймер
            }

        }
        //функція яка відкриває вибір камінь або ножниці або бумагу користувачу 2
        private void Player2()
        {
            panel8.Visible = true;
            if (mode == "Man VS AI" || mode == "AI VS AI" && randomMode == true)
            {
                // Ініціалізуємо таймер
                clickTimer = new Timer();
                clickTimer.Interval = 1000; // Затримка 500 мс
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop(); // Зупиняємо таймер

                    // Генеруємо випадкове число 1, 2 або 3
                    int buttonNumber = random.Next(1, 4); // 1, 2 або 3

                    // Вибираємо кнопку для натискання
                    switch (buttonNumber)
                    {
                        case 1:
                            button11.PerformClick();
                            break;
                        case 2:
                            button12.PerformClick();
                            break;
                        case 3:
                            button13.PerformClick();
                            break;
                    }
                };
                clickTimer.Start(); // Запускаємо таймер
            }
            else if ((mode == "Man VS AI" || mode == "AI VS AI") && winStrategy == true)
            {
                clickTimer = new Timer();
                clickTimer.Interval = 1000;
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop();

                    int buttonNumber;

                    if (winStrategy == true && player1Moves.Count >= 5)
                    {
                        // Викликаємо контрхід, якщо є достатньо даних про ходи гравця
                        buttonNumber = GetCounterMove();
                    }
                    else
                    {
                        // Якщо даних недостатньо, обираємо випадковий хід
                        buttonNumber = random.Next(1, 4);
                    }

                    switch (buttonNumber)
                    {
                        case 1:
                            button11.PerformClick(); //папір
                            break;
                        case 2:
                            button12.PerformClick(); //ножниці
                            break;
                        case 3:
                            button13.PerformClick(); // камінь
                            break;
                    }
                };
                clickTimer.Start();
            }
        }
        // Метод для визначення найбільш частого ходу гравця 1 і вибору контрходу
        private int GetCounterMove()
        {
            // Підрахунок частот ходів із вагами
            Dictionary<int, double> moveWeights = new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 } };

            // Ваговий коефіцієнт - останні ходи отримують більшу вагу
            double weight = 1.0;
            double weightIncrement = 0.15; // Зменшуємо крок, щоб вага зростала більш плавно на 8 ходах
            foreach (int move in player1Moves)
            {
                moveWeights[move] += weight;
                weight += weightIncrement;
            }

            // Знаходимо найбільш ймовірний хід гравця
            int mostLikelyMove = moveWeights.OrderByDescending(m => m.Value).First().Key;

            // Імовірність випадкового ходу
            double randomness = 0.15; // 15% шанс вибрати випадковий хід
            if (random.NextDouble() < randomness)
            {
                return random.Next(1, 4); // Випадковий хід
            }

            // Контрхід: камінь (1) -> папір (2), папір (2) -> ножиці (3), ножиці (3) -> камінь (1)
            switch (mostLikelyMove)
            {
                case 1: // Гравець найчастіше вибирає камінь
                    return 1; // AI вибирає папір
                case 2: // Гравець найчастіше вибирає папір
                    return 2; // AI вибирає ножиці
                case 3: // Гравець найчастіше вибирає ножиці
                    return 3; // AI вибирає камінь
                default:
                    return random.Next(1, 4); // Випадковий вибір на випадок непередбаченої ситуації
            }
        }
        //клавіша для вибору користувачем 1 - варіанту камінь
        private void button5_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 1;
            TrackPlayer1Move(select1User);
        }
        //клавіша для вибору користувачем 1 - варіанту папір
        private void button7_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 2;
            TrackPlayer1Move(select1User);
        }

        //клавіша для вибору користувачем 1 - варіанту ножниці
        private void button6_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 3;
            TrackPlayer1Move(select1User);
        }
        // Додати метод для відстеження останніх 5 ходів
        private void TrackPlayer1Move(int move)
        {
            player1Moves.Add(move); // Додаємо новий хід гравця 1
            if (player1Moves.Count > 5)
            {
                player1Moves.RemoveAt(0); // Видаляємо найстаріший хід, щоб зберігати тільки останні 5
            }
        }

        //клавіша для вибору користувачем 2 - варіанту камінь
        private void button13_Click(object sender, EventArgs e)
        {
            select2User = 4;
            serverControl();
        }
        // робота з сервером у вигляді ардуіно
        private void serverControl()
        {
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", "");
            try
            {
                using (SerialPort serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // Встановлюємо таймаут читання на 3 секунди
                    serialPort.Open();

                    string message = select1User.ToString() + " " + select2User.ToString();
                    serialPort.WriteLine(message);

                    try
                    {
                        string response = serialPort.ReadLine();
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        FinalAction(response, counter1, counter2);
                    }
                    catch (TimeoutException)
                    {
                        MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StartMenu(); // Перенаправлення на головне меню
                        button1.Visible = true;
                        button21.Visible = true;
                        button22.Visible = true;
                        panel17.Visible = true;
                        pictureBox15.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartMenu(); // Перенаправлення на головне меню
                button1.Visible = true;
                button21.Visible = true;
                button22.Visible = true;
                panel17.Visible = true;
                pictureBox15.Visible = true;
            }
        }
        // вибір фінально екран (нічия, виграв 1 гравець, виграв 2 гравеець)
        private void FinalAction(string response, string counter1, string counter2)
        {
            // нічийні варіанті
           
            string tieRock1 = "It's a tie. Player1 and Player2 select rock";
            string tieRock2 = "It's a tie. Player1 and Player2 select paper";
            string tieRock3 = "It's a tie. Player1 and Player2 select scissors";

            // перемога першого гравця програш другого 
            string WinPlayer1f = "Player1 Win!. Player1 select rock and Player2 select scissors";
            string WinPlayer1s = "Player1 Win!. Player1 select scissors and Player2 select paper";
            string WinPlayer1t = "Player1 Win!. Player1 select paper and Player2 select rock";

            // перемога другого гравця програш другого 
            string WinPlayer2f = "Player2 Win!. Player1 select scissors and Player2 select rock";
            string WinPlayer2s = "Player2 Win!. Player1 select paper and Player2 select scissors";
            string WinPlayer2t = "Player2 Win!. Player1 select rock and Player2 select paper";
            //шлях до зображення камінь
            string imagePath = Path.Combine(@"D:\документи\", @"Четвертий курс\АПКС\Лабораторні\Лабораторна №3\game client\img\rock.png");
            //шлях до зображення папір
            string imagePath1 = Path.Combine(@"D:\документи\", @"Четвертий курс\АПКС\Лабораторні\Лабораторна №3\game client\img\paper.png");
            //шлях до зображення ножниці
            string imagePath2 = Path.Combine(@"D:\документи\", @"Четвертий курс\АПКС\Лабораторні\Лабораторна №3\game client\img\scissors.png");
            // нічия якщо два гравці вибрали камінь
            if (tieRock1 == response.Trim())
            {
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали папір
            else if (tieRock2 == response.Trim())
            {
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали ножниці
            else if (tieRock3 == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим каменю, вибір другим ножниць)
            else if (WinPlayer1f == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим ножниці, вибір другим папір)
            else if (WinPlayer1s == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим папір, вибір другим камінь)
            else if (WinPlayer1t == response.Trim())
            { 
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим ножниць, вибір другим каменю)
            else if (WinPlayer2f == response.Trim())
            { 
                // Завантажити зображення у PictureBox
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим папір , вибір другим ножниці)
            else if (WinPlayer2s == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим камінь, вибір другим папір)
            else if (WinPlayer2t == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            if(onLoad == false)
            {
                textBox1.Text = counter1.Substring(14).ToString();
                textBox2.Text = counter2.Substring(14).ToString();
            }
            else
            {
                int line1 = int.Parse(counter1.Substring(14));
                int line2 = int.Parse(counter2.Substring(14));
                textBox1.Text = (line1 + score1).ToString();
                textBox2.Text = (line2 + score2).ToString();
            }
        }
        // Функція для завантаження зображення
        private void LoadImage(string imagePath, PictureBox pictureBox)
        {
            // Перевірка на існування зображення
            if (pictureBox.Image != null)
            {
                // Звільняємо ресурси старого зображення
                pictureBox.Image.Dispose();
            }

            // Завантажуємо нове зображення
            pictureBox.Image = Image.FromFile(imagePath);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закриваємо серійний порт, якщо він відкритий
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Тут можна закрити серійний порт, якщо він відкритий
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            base.OnFormClosing(e);
        }
        //клавіша для вибору користувачем 2 - варіанту папір
        private void button11_Click(object sender, EventArgs e)
        {
            select2User = 5;
            serverControl();
        }
        //клавіша для вибору користувачем 2 - варіанту ножниці
        private void button12_Click(object sender, EventArgs e)
        {
            select2User = 6;
            serverControl();
        }
        //клавіша грати знову
        private void button17_Click_1(object sender, EventArgs e)
        {
            panel13.Visible = false;
            panel8.Visible = false;
            Player1();
        }
        //клавіша яка перекидає на головне меню
        private void button19_Click(object sender, EventArgs e)
        {
            StartMenu();
            button1.Visible = true;
            button21.Visible = true;
            button22.Visible = true;
            panel17.Visible = true;
            pictureBox15.Visible = true;
        }
        //клавіша скиду рахунку
        private void ResetScore()
        {
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Значення за замовчуванням: порожнє
            try
            {
                using (SerialPort serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // Встановлюємо таймаут читання у 3 секунди
                    serialPort.Open(); // Відкриваємо порт

                    serialPort.WriteLine("0"); // Відправляємо команду для скидання

                    try
                    {
                        // Читаємо відповіді від порту
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        // Обробка отриманих даних
                        textBox1.Text = counter1.Substring(14);
                        textBox2.Text = counter2.Substring(14);
                    }
                    catch (TimeoutException)
                    {
                        // Якщо порт не відповідає протягом часу таймауту, показуємо повідомлення і повертаємося до головного меню
                        MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка інших помилок під час роботи з портом
                MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //Man vs AI
        private void button20_Click_1(object sender, EventArgs e)
        {
            ResetScore();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mode = "Man VS AI";
            Player1();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            mode = "AI VS AI";
            Player1();
        }
        private void button22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button23_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog(); // Відкриваємо як модальне вікно
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {
                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out winStrategy);
                bool.TryParse(checkBox3Value, out randomMode);
                if (musicOn == true)
                {
                    _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                    _player.LoadAsync();
                    _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Save game
        private void button18_Click(object sender, EventArgs e)
        {
            string mainScore = textBox1.Text.Replace("\n", "").Replace("\r", "") + " : " +
                   textBox2.Text.Replace("\n", "").Replace("\r", "");

            SaveMenu saveMenu = new SaveMenu(mode, mainScore);
            saveMenu.Show();
        }
        //Load game
        private void button21_Click(object sender, EventArgs e)
        {
            LoadForm loadForm = new LoadForm(this);
            loadForm.Show();
        }
        public void SetGameData(string gameMode, string gameScore)
        {
            // Розділяємо рядок рахунку на дві частини
            string[] scores = gameScore.Split(':');
            mode = gameMode;
            button21.Visible = false;
            button22.Visible =false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            if (scores.Length == 2) // Переконуємось, що є два значення
            {
                score1 = int.Parse(scores[0].Trim()); // Перше значення рахунку як int
                score2 = int.Parse(scores[1].Trim()); // Друге значення рахунку як int
                onLoad = true;
            }
            else
            {
                MessageBox.Show("Invalid score format", "Error");
            }
            ResetScore();
            Player1();
        }
    }
}
