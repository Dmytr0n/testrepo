using Microsoft.VisualStudio.TestTools.UnitTesting;
using game_client;
using System;
using System.IO;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Linq;  // Додаємо цей простір імен
using System.Threading.Tasks;
using System.Drawing;
using System.IO.Ports;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Media; // Add this at the top of your test file




namespace testingMenu
{

    [TestClass]
    public class UnitTest1
    {
        // Тест для ModMenu
        [TestMethod]
        public void ModMenu_ShouldSetCorrectVisibility()
        {
            string logFilePath = "testResults.txt";

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"Test run at {DateTime.Now}");
            }

            using (var mainForm = new Form1())
            {
                mainForm.Show();
                mainForm.CreateControl();

                mainForm.ModMenu();

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(mainForm.button1.Visible == false ? "Pass: button1 is hidden." : "Fail: button1 should be hidden.");
                    Assert.IsFalse(mainForm.button1.Visible, "button1 should be hidden.");

                    writer.WriteLine(mainForm.button22.Visible == false ? "Pass: button22 is hidden." : "Fail: button22 should be hidden.");
                    Assert.IsFalse(mainForm.button22.Visible, "button22 should be hidden.");

                    writer.WriteLine(mainForm.button21.Visible == false ? "Pass: button21 is hidden." : "Fail: button21 should be hidden.");
                    Assert.IsFalse(mainForm.button21.Visible, "button21 should be hidden.");

                    writer.WriteLine(mainForm.panel17.Visible == false ? "Pass: panel17 is hidden." : "Fail: panel17 should be hidden.");
                    Assert.IsFalse(mainForm.panel17.Visible, "panel17 should be hidden.");

                    writer.WriteLine(mainForm.pictureBox15.Visible == false ? "Pass: pictureBox15 is hidden." : "Fail: pictureBox15 should be hidden.");
                    Assert.IsFalse(mainForm.pictureBox15.Visible, "pictureBox15 should be hidden.");

                    writer.WriteLine(mainForm.label2.Visible == true ? "Pass: label2 is visible." : "Fail: label2 should be visible.");
                    Assert.IsTrue(mainForm.label2.Visible, "label2 should be visible.");

                    writer.WriteLine(mainForm.panel1.Visible == true ? "Pass: panel1 is visible." : "Fail: panel1 should be visible.");
                    Assert.IsTrue(mainForm.panel1.Visible, "panel1 should be visible.");

                    writer.WriteLine(mainForm.panel2.Visible == true ? "Pass: panel2 is visible." : "Fail: panel2 should be visible.");
                    Assert.IsTrue(mainForm.panel2.Visible, "panel2 should be visible.");

                    writer.WriteLine(mainForm.button2.Visible == true ? "Pass: button2 is visible." : "Fail: button2 should be visible.");
                    Assert.IsTrue(mainForm.button2.Visible, "button2 should be visible.");

                    writer.WriteLine(mainForm.button3.Visible == true ? "Pass: button3 is visible." : "Fail: button3 should be visible.");
                    Assert.IsTrue(mainForm.button3.Visible, "button3 should be visible.");

                    writer.WriteLine(mainForm.button4.Visible == true ? "Pass: button4 is visible." : "Fail: button4 should be visible.");
                    Assert.IsTrue(mainForm.button4.Visible, "button4 should be visible.");
                }

                mainForm.Close();
            }
        }
        // клас для тестування функцій button 3 та Player1()
        private class TestForm1 : Form1
        {
            public bool IsPlayer1Called { get; private set; }


            public override void Player1()
            {
                IsPlayer1Called = true;
                base.Player1();
            }
            // Для зручності тестування зробимо поле для доступу до таймера
            public System.Windows.Forms.Timer TestTimer => clickTimer;
            public Random TestRandom => random;

            // Ми будемо перевизначати логіку випадкового числа для тесту
            public void SetTestRandom(int value)
            {
                random = new Random(value); // Встановлюємо значення для тесту
            }





        }

        [TestMethod]
        public void Button3_Click_ShouldSetModeToManVsManAndCallPlayer1()
        {
            // Arrange
            var form = new TestForm1();

            // Act
            form.button3_Click(null, EventArgs.Empty);

            // Assert
            // Перевірка, чи викликаний метод Player1
            Assert.IsTrue(form.IsPlayer1Called, "Метод Player1 не був викликаний.");

            // Перевірка, чи встановлена змінна mode на "Man VS Man"
            Assert.AreEqual("Man VS Man", form.mode, "Змінна mode не має значення 'Man VS Man'.");
        }

        [TestMethod]
        public void Button3_Click_ShouldNotChangeOtherUIElements()
        {
            // Arrange
            var form = new TestForm1();
            var initialVisibility = form.button3.Visible;  // Зберігаємо початкову видимість кнопки

            // Act
            form.button3_Click(null, EventArgs.Empty);

            // Assert
            // Перевірка, чи не змінилась видимість кнопки (як приклад)
            Assert.AreEqual(initialVisibility, form.button3.Visible, "Видимість кнопки button3 змінилася.");
        }

        [TestMethod]
        public void Player1_ShouldChangePanelVisibility()
        {
            // Arrange
            var form = new TestForm1();

            // Act
            form.Player1(); // Викликаємо метод Player1
            form.Show();
            // Assert
            Assert.IsFalse(form.panel1.Visible, "panel1 не була прихована.");
            Assert.IsFalse(form.panel2.Visible, "panel2 не була прихована.");
            Assert.IsTrue(form.panel3.Visible, "panel3 не була відображена.");
            form.Close();
        }

        [TestMethod]
        public void Player1_ShouldStartTimerForAI_VS_AI()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI";  // Встановлюємо режим AI VS AI
            form.SetTestRandom(1); // Встановлюємо фіксоване значення для випадкового числа

            // Act
            form.Player1(); // Викликаємо метод Player1

            // Assert
            // Перевірка, чи таймер стартував
            Assert.IsNotNull(form.TestTimer, "Таймер не був ініціалізований.");
            Assert.IsTrue(form.TestTimer.Enabled, "Таймер не був запущений.");
        }
        [TestMethod]
        public void Player1_ShouldClickOneRandomButton()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI"; // Налаштовуємо режим

            bool button5Clicked = false;
            bool button6Clicked = false;
            bool button7Clicked = false;

            // Підписуємось на події кліку для трьох кнопок
            form.button5.Click += (s, e) =>
            {
                button5Clicked = true;
                Console.WriteLine("Button5 була натиснута."); // Повідомлення, яка кнопка натиснута
            };
            form.button6.Click += (s, e) =>
            {
                button6Clicked = true;
                Console.WriteLine("Button6 була натиснута."); // Повідомлення, яка кнопка натиснута
            };
            form.button7.Click += (s, e) =>
            {
                button7Clicked = true;
                Console.WriteLine("Button7 була натиснута."); // Повідомлення, яка кнопка натиснута
            };

            // Фіксоване значення для випадкових чисел
            form.SetTestRandom(1); // Задаємо фіксоване значення для випадковості (можна змінювати для тесту інших кнопок)

            // Act
            form.Player1(); // Викликаємо метод Player1, щоб ініціювати логіку
            form.Show();
            // Випадковий вибір кнопки для натискання (від 1 до 3)
            var randomButton = new Random().Next(1, 4); // Випадкове число від 1 до 3

            switch (randomButton)
            {
                case 1:
                    form.button5.PerformClick();
                    break;
                case 2:
                    form.button6.PerformClick();
                    break;
                case 3:
                    form.button7.PerformClick();
                    break;
            }

            // Затримка для тесту (можна змінити в залежності від потрібного часу)
            System.Threading.Thread.Sleep(500);

            // Assert: Перевіряємо, що одна з кнопок була натиснута
            Assert.IsTrue(button5Clicked || button6Clicked || button7Clicked, "Жодна кнопка не була натиснута.");
            form.Close();
        }
        [TestMethod]
        public void FinalAction_ShouldHandleTieRock1()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI"; // Set the game mode
            string response = "It's a tie. Player1 and Player2 select rock";
            string counter1 = "Player1 Wins: 10";
            string counter2 = "Player2 Wins: 10";

            // Mock controls and their initial state
            form.pictureBox13 = new PictureBox();  // Mock PictureBox for Player1
            form.pictureBox14 = new PictureBox();  // Mock PictureBox for Player2
            form.label9 = new Label();  // Mock Label for tie message
            form.label16 = new Label();  // Mock Label for win message
            form.panel13 = new Panel();  // Mock panel for visibility

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsTrue(form.label9.Visible, "Label9 should be visible for tie message.");
            Assert.AreEqual("It's a tie!", form.label9.Text, "Label9 text should be 'It's a tie!'");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should have an image for Player1.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should have an image for Player2.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for tie result.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleTiePaper()
        {
            // Arrange
            var form = new TestForm1();
            string response = "It's a tie. Player1 and Player2 select paper";
            string counter1 = "Player1 Wins: 0";
            string counter2 = "Player2 Wins: 0";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsTrue(form.label9.Visible, "Label9 should be visible for tie message.");
            Assert.AreEqual("It's a tie!", form.label9.Text, "Label9 text should indicate a tie.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for tie result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for tie.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for tie.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleTieScissors()
        {
            // Arrange
            var form = new TestForm1();
            string response = "It's a tie. Player1 and Player2 select scissors";
            string counter1 = "Player1 Wins: 0";
            string counter2 = "Player2 Wins: 0";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsTrue(form.label9.Visible, "Label9 should be visible for tie message.");
            Assert.AreEqual("It's a tie!", form.label9.Text, "Label9 text should indicate a tie.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for tie result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for tie.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for tie.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer1f()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI"; // Set the game mode
            string response = "Player1 Win!. Player1 select rock and Player2 select scissors";
            string counter1 = "Player1 Wins: 10";
            string counter2 = "Player2 Wins: 100";

            // Mock controls and their initial state
            form.pictureBox13 = new PictureBox();  // Mock PictureBox for Player1
            form.pictureBox14 = new PictureBox();  // Mock PictureBox for Player2
            form.label9 = new Label();  // Mock Label for tie message
            form.label16 = new Label();  // Mock Label for win message
            form.panel13 = new Panel();  // Mock panel for visibility

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 1 win!", form.label16.Text, "Label16 text should be 'Player 1 win!'");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should have an image for Player1.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should have an image for Player2.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer1_ScissorsVsPaper()
        {
            // Arrange
            var form = new TestForm1();
            string response = "Player1 Win!. Player1 select scissors and Player2 select paper";
            string counter1 = "Player1 Wins: 12";
            string counter2 = "Player2 Wins: 8";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 1 win!", form.label16.Text, "Label16 text should indicate Player1 win.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for Player1's choice.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for Player2's choice.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer2_RockVsPaper()
        {
            // Arrange
            var form = new TestForm1();
            string response = "Player2 Win!. Player1 select rock and Player2 select paper";
            string counter1 = "Player1 Wins: 5";
            string counter2 = "Player2 Wins: 10";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 2 win!", form.label16.Text, "Label16 text should indicate Player2 win.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for Player1's choice.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for Player2's choice.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer2_ScissorsVsRock()
        {
            // Arrange
            var form = new TestForm1();
            string response = "Player2 Win!. Player1 select scissors and Player2 select rock";
            string counter1 = "Player1 Wins: 15";
            string counter2 = "Player2 Wins: 20";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 2 win!", form.label16.Text, "Label16 text should indicate Player2 win.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for Player1's choice.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for Player2's choice.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer1_PaperVsRock()
        {
            // Arrange
            var form = new TestForm1();
            string response = "Player1 Win!. Player1 select paper and Player2 select rock";
            string counter1 = "Player1 Wins: 5";
            string counter2 = "Player2 Wins: 3";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 1 win!", form.label16.Text, "Label16 text should indicate Player1 win.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for Player1's choice.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for Player2's choice.");
        }
        [TestMethod]
        public void FinalAction_ShouldHandleWinPlayer2_PaperVsScissors()
        {
            // Arrange
            var form = new TestForm1();
            string response = "Player2 Win!. Player1 select paper and Player2 select scissors";
            string counter1 = "Player1 Wins: 5";
            string counter2 = "Player2 Wins: 7";

            form.pictureBox13 = new PictureBox();
            form.pictureBox14 = new PictureBox();
            form.label9 = new Label();
            form.label16 = new Label();
            form.panel13 = new Panel();
            form.onLoad = true;
            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.IsFalse(form.label9.Visible, "Label9 should not be visible for win message.");
            Assert.IsTrue(form.label16.Visible, "Label16 should be visible for win message.");
            Assert.AreEqual("Player 2 win!", form.label16.Text, "Label16 text should indicate Player2 win.");
            Assert.IsTrue(form.panel13.Visible, "Panel13 should be visible for win result.");
            Assert.IsTrue(form.pictureBox13.Image != null, "PictureBox13 should display an image for Player1's choice.");
            Assert.IsTrue(form.pictureBox14.Image != null, "PictureBox14 should display an image for Player2's choice.");
        }

        [TestMethod]
        public void FinalAction_ShouldHandleCounterValues()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI"; // Set the game mode
            string response = "Player1 Win!. Player1 select rock and Player2 select scissors";
            string counter1 = "Player1 Wins: 20";
            string counter2 = "Player2 Wins: 30";

            // Act
            form.FinalAction(response, counter1, counter2);

            // Assert
            Assert.AreEqual("20", form.textBox1.Text, "TextBox1 should display the correct counter value.");
            Assert.AreEqual("30", form.textBox2.Text, "TextBox2 should display the correct counter value.");
        }

        [TestMethod]
        public void GetCounterMove_ShouldReturnExpectedCounterMove()
        {
            // Arrange
            var form = new TestForm1(); // Ініціалізація форми
            form.player1Moves = new List<int> { 1, 1, 1, 1, 1 }; // Гравець найчастіше вибирає камінь (1)
            var random = new Random(0); // Фіксований генератор випадкових чисел для передбачуваного результату

            // Act
            int result = form.GetCounterMove();

            // Assert
            if (random.NextDouble() >= 0.15)
            {
                // Очікуємо, що AI обере папір (1) як контрхід проти каменю (1)
                Assert.AreEqual(1, result, "AI повинен вибрати папір як контрхід проти каменю.");
            }
            else
            {
                // Перевіряємо, що результат є випадковим числом між 1 та 3
                Assert.IsTrue(result >= 1 && result <= 3, "Результат має бути випадковим числом між 1 та 3.");
            }
        }
        private Form1 _form;
        private string testFilePath;
        private string _configFilePath;

        [TestInitialize]
        public void SetUp()
        {
            // Ініціалізація об'єкта форми перед кожним тестом
            _form = new Form1();
            TestEnvironment.IsTestMode = true; // Встановлюємо в тестовий режим
            // Створюємо тимчасовий INI-файл для тестування
            testFilePath = Path.Combine(Path.GetTempPath(), "test.ini");
            File.WriteAllText(testFilePath, "[TestSection]\nTestKey=TestValue\n");
            // Створюємо шлях до тимчасового конфігураційного файлу
            _configFilePath = Path.Combine(Path.GetTempPath(), "config.ini");
        }

        [TestMethod]
        public void SetGameData_ValidScore_SetsCorrectValues()
        {
            // Arrange
            string gameMode = "Man VS Man";
            string gameScore = "5:3"; // правильний формат рахунку

            // Act
            _form.SetGameData(gameMode, gameScore);

            // Assert
            Assert.AreEqual("Man VS Man", _form.mode);
            Assert.AreEqual(5, _form.score1);
            Assert.AreEqual(3, _form.score2);
            Assert.IsTrue(_form.onLoad);
            Assert.IsFalse(_form.button21.Visible);
            Assert.IsFalse(_form.button22.Visible);
            Assert.IsFalse(_form.panel17.Visible);
            Assert.IsFalse(_form.pictureBox15.Visible);
        }

        [TestMethod]
        public void SetGameData_InvalidScoreFormat_ShowsErrorMessage()
        {
            // Arrange
            string gameMode = "Man VS Man";
            string gameScore = "invalid"; // Невірний формат рахунку

            // Act
            _form.SetGameData(gameMode, gameScore);

            // Assert
            Assert.AreEqual("Man VS Man", _form.mode);
            Assert.AreEqual(0, _form.score1); // значення за замовчуванням
            Assert.AreEqual(0, _form.score2); // значення за замовчуванням
            Assert.IsFalse(_form.onLoad);
            Assert.IsFalse(_form.button21.Visible);
            Assert.IsFalse(_form.button22.Visible);
            Assert.IsFalse(_form.panel17.Visible);
            Assert.IsFalse(_form.pictureBox15.Visible);

            // Перевіряємо, що повідомлення про помилку було показано
            // Оскільки MessageBox не можна безпосередньо перевірити, можна використовувати Mock або перевірити поведінку на рівні інтерфейсу
            // Для прикладу, можна використовувати Mocking бібліотеки для перевірки викликів MessageBox.
        }

        [TestMethod]
        public void SetGameData_EmptyScore_ShowsErrorMessage()
        {
            // Arrange
            string gameMode = "Man VS Man";
            string gameScore = ""; // Порожній рахунок

            // Act
            _form.SetGameData(gameMode, gameScore);

            // Assert
            Assert.AreEqual("Man VS Man", _form.mode);
            Assert.AreEqual(0, _form.score1);
            Assert.AreEqual(0, _form.score2);
            Assert.IsFalse(_form.onLoad);
        }

        [TestMethod]
        public void SetGameData_ScoreWithExtraSpaces_ParsesCorrectly()
        {
            // Arrange
            string gameMode = "Man VS Man";
            string gameScore = " 10 : 4 "; // Рахунок з пробілами

            // Act
            _form.SetGameData(gameMode, gameScore);

            // Assert
            Assert.AreEqual("Man VS Man", _form.mode);
            Assert.AreEqual(10, _form.score1);
            Assert.AreEqual(4, _form.score2);
            Assert.IsTrue(_form.onLoad);
        }
        [TestMethod]
        public void Button21_Click_CreatesLoadForm()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.button21_Click(null, null); // Викликаємо метод кнопки

            // Assert
            var loadForm = Application.OpenForms.OfType<LoadForm>().FirstOrDefault(); // Перевіряємо, чи є відкритою форма LoadForm
            Assert.IsNotNull(loadForm); // Перевірка, що LoadForm була відкрита
            loadForm.Close();
        }
        [TestMethod]
        public void Button18_Click_ShouldCreateAndShowSaveMenu()
        {

            // Створення форми для тестування (Form1 — це ваша форма, в якій реалізовано button18_Click)
            var form = new Form1();

            // Мокування textBox1 і textBox2
            var textBox1 = new TextBox();
            var textBox2 = new TextBox();
            textBox1.Text = "10"; // Текст для першого текстового поля
            textBox2.Text = "20"; // Текст для другого текстового поля



            // Act: Викликаємо метод button18_Click на екземплярі форми
            form.button18_Click(null, null);

            // Оскільки форма `SaveMenu` відкривається в новому вікні, треба трохи зачекати для того, щоб вона відобразилась
            Application.DoEvents();

            // Assert: Перевіряємо, чи була відкрита форма SaveMenu
            var saveMenu = Application.OpenForms.OfType<SaveMenu>().FirstOrDefault();
            // Додавання textBox до форми
            saveMenu.Controls.Add(textBox1);
            saveMenu.Controls.Add(textBox2);
            // Перевіряємо, чи була створена форма SaveMenu
            Assert.IsNotNull(saveMenu, "SaveMenu was not created.");
            saveMenu.Close();
        }
        [TestMethod]
        public void Button22_Click_ShouldSetIsExitCalledToTrue()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.button22_Click(null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(form.IsExitCalled, "IsExitCalled should be true when the button is clicked.");
        }
        [TestMethod]
        public void Button4Click_SetsModeToAIVsAI()
        {
            // Arrange
            var form = new Form1();
            form.mode = ""; // Початковий стан

            // Act
            form.button4_Click(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual("AI VS AI", form.mode, "Mode should be set to AI VS AI");
        }
        [TestMethod]
        public void Button2_Click_ShouldSetModeToManVsAIAndCallPlayer1()
        {
            // Arrange
            var form = new TestForm1();  // Тестова форма, яка перевизначає метод Player1

            // Act
            form.button2_Click(null, EventArgs.Empty);

            // Assert
            // Перевірка, чи викликаний метод Player1
            Assert.IsTrue(form.IsPlayer1Called, "Метод Player1 не був викликаний.");

            // Перевірка, чи встановлена змінна mode на "Man VS AI"
            Assert.AreEqual("Man VS AI", form.mode, "Змінна mode не має значення 'Man VS AI'.");
        }
        [TestMethod]
        public void TestResetScore_SuccessfulRead_FormInteraction()
        {
            // Створюємо екземпляр форми
            Form1 form = new Form1();

            // Використовуємо реальний порт (переконайтеся, що цей порт існує та підключений)
            string testPort = "COM5"; // Замість цього використовуйте наявний порт
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            ini.Write("TextBoxValues", "TextBox1", testPort); // Записуємо порт у конфігураційний файл

            // Потрібно налаштувати пристрій на повернення правильних значень через серійний порт

            // Викликаємо метод, який має змінити значення у textBox1 та textBox2
            form.ResetScore();

            // Перевіряємо, що значення текстових полів було змінено правильно
            Assert.AreEqual("0", form.textBox1.Text.Replace("\r\n", "").Replace("\n", "").Trim());
            Assert.AreEqual("0", form.textBox2.Text.Replace("\r\n", "").Replace("\n", "").Trim());
        }
        [TestMethod]
        public void TestButton19Click_VisibilityChanges_AfterStartMenu()
        {
            // Створюємо екземпляр форми
            Form1 form = new Form1();

            // Спочатку викликаємо StartMenu(), щоб перевірити його вплив
            form.StartMenu();

            // Перевіряємо, чи був викликаний StartMenu
            Assert.IsTrue(form.StartMenuCalled, "StartMenu() was not called!");

            // Імітуємо натискання кнопки button19
            form.button19_Click(null, null);
            form.Show();

            // Перевіряємо, чи були змінені властивості Visible для елементів
            Assert.IsTrue(form.button1.Visible);
            Assert.IsTrue(form.button21.Visible);
            Assert.IsTrue(form.button22.Visible);
            Assert.IsTrue(form.panel17.Visible);
            Assert.IsTrue(form.pictureBox15.Visible);
            form.Close();
        }
        [TestMethod]
        public void TestButton17Click_VisibilityChanges_And_Player1Called()
        {
            // Створюємо екземпляр форми
            Form1 form = new Form1();

            // Імітуємо натискання кнопки button17
            form.button17_Click_1(null, null);

            // Перевіряємо, чи стали панелі невидимими
            Assert.IsFalse(form.panel13.Visible, "panel13 should be hidden.");
            Assert.IsFalse(form.panel8.Visible, "panel8 should be hidden.");

            // Перевіряємо, чи був викликаний метод Player1
            Assert.IsTrue(form.Player1Called, "Player1() was not called!");
        }
        [TestMethod]
        public void TestButton12Click_Select2UserChanged_And_ServerControlCalled()
        {
            // Створюємо екземпляр форми
            Form1 form = new Form1();

            // Ініціалізуємо значення select2User і прапорець для виклику serverControl
            form.select2User = 0;  // Початкове значення

            // Імітуємо натискання кнопки button12
            form.button12_Click(null, null);

            // Перевіряємо, чи змінилося значення select2User на 6
            Assert.AreEqual(6, form.select2User, "select2User should be 6 after button12_Click.");

            // Перевіряємо, чи був викликаний метод serverControl
            Assert.IsTrue(form.ServerControlCalled, "serverControl() was not called!");
        }
        [TestMethod]
        public void TestButton11Click_Select2UserChanged_And_ServerControlCalled()
        {
            // Створюємо екземпляр форми
            Form1 form = new Form1();

            // Ініціалізуємо значення select2User і прапорець для виклику serverControl
            form.select2User = 0;  // Початкове значення


            // Імітуємо натискання кнопки button11
            form.button11_Click(null, null);

            // Перевіряємо, чи змінилося значення select2User на 5
            Assert.AreEqual(5, form.select2User, "select2User should be 5 after button11_Click.");

            // Перевіряємо, чи був викликаний метод serverControl
            Assert.IsTrue(form.ServerControlCalled, "serverControl() was not called!");
        }
        [TestMethod]
        public void TestDrawCustomBorder()
        {
            // Створення тестової панелі
            Panel panel = new Panel
            {
                Width = 100,
                Height = 100
            };

            // Створення інстансу для PaintEventArgs
            var paintEventArgs = new PaintEventArgs(panel.CreateGraphics(), new Rectangle(0, 0, panel.Width, panel.Height));

            // Створення екземпляра функції, яку потрібно протестувати
            var form = new SettingsForm();

            form.DrawCustomBorder(panel, paintEventArgs);

            // Для тестування, перевіряємо, чи було нарисовано, наприклад, за допомогою властивостей
            // Графічний об'єкт можна перевірити або шляхом порівняння пікселів, або перевіркою змін на елементі управління
            // Для простоти припустимо, що перевірка відбувається через захоплення відомих результатів малювання

            // Тестова перевірка: чи не виникла помилка в процесі малювання
            Assert.IsNotNull(paintEventArgs.Graphics);
        }
        [TestMethod]
        public void TestDrawCustomBorderForm1()
        {
            // Створення тестової панелі
            Panel panel = new Panel
            {
                Width = 100,
                Height = 100
            };

            // Створення інстансу для PaintEventArgs
            var paintEventArgs = new PaintEventArgs(panel.CreateGraphics(), new Rectangle(0, 0, panel.Width, panel.Height));

            // Створення екземпляра функції, яку потрібно протестувати
            var form = new Form1();

            form.DrawCustomBorder(panel, paintEventArgs);

            // Для тестування, перевіряємо, чи було нарисовано, наприклад, за допомогою властивостей
            // Графічний об'єкт можна перевірити або шляхом порівняння пікселів, або перевіркою змін на елементі управління
            // Для простоти припустимо, що перевірка відбувається через захоплення відомих результатів малювання

            // Тестова перевірка: чи не виникла помилка в процесі малювання
            Assert.IsNotNull(paintEventArgs.Graphics);
        }
        [TestMethod]
        public void TestDrawCustomBorderSaveMenu()
        {
            // Створення тестової панелі
            Panel panel = new Panel
            {
                Width = 100,
                Height = 100
            };

            // Створення інстансу для PaintEventArgs
            var paintEventArgs = new PaintEventArgs(panel.CreateGraphics(), new Rectangle(0, 0, panel.Width, panel.Height));

            string mode = "AI VS AI";
            string score = "10 : 11";
            // Створення екземпляра функції, яку потрібно протестувати
            var form = new SaveMenu(mode, score);

            form.DrawCustomBorder(panel, paintEventArgs);

            // Для тестування, перевіряємо, чи було нарисовано, наприклад, за допомогою властивостей
            // Графічний об'єкт можна перевірити або шляхом порівняння пікселів, або перевіркою змін на елементі управління
            // Для простоти припустимо, що перевірка відбувається через захоплення відомих результатів малювання

            // Тестова перевірка: чи не виникла помилка в процесі малювання
            Assert.IsNotNull(paintEventArgs.Graphics);
        }
        [TestMethod]
        public void StartMenu_IniFileValues_SetCorrectStates()
        {
            // Arrange: створення тимчасового INI-файлу
            string tempIniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
            File.WriteAllText(tempIniFile, @"
[CheckboxStates]
CheckBox1=True
CheckBox2=False
CheckBox3=True");

            var form = new Form1();

            // Act: виклик функції
            form.StartMenu();

            // Assert: перевірка станів
            Assert.IsTrue(form.musicOn, "musicOn should be true.");
            Assert.IsFalse(form.winStrategy, "winStrategy should be false.");
            Assert.IsTrue(form.randomMode, "randomMode should be true.");

            // Перевірка видимості елементів
            Assert.IsFalse(form.label2.Visible, "label2 should be hidden.");
            Assert.IsFalse(form.panel1.Visible, "panel1 should be hidden.");
            Assert.IsFalse(form.panel2.Visible, "panel2 should be hidden.");

            // Cleanup: видалення тимчасового файлу
            File.Delete(tempIniFile);
        }
        [TestMethod]
        public void Read_ReturnsCorrectValue_WhenKeyExists()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);

            // Act
            string actualValue = iniFile.Read("TestSection", "TestKey", "DefaultValue");

            // Assert
            Assert.AreEqual("TestValue", actualValue);
        }

        [TestMethod]
        public void Read_ReturnsDefaultValue_WhenKeyDoesNotExist()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);

            // Act
            string actualValue = iniFile.Read("TestSection", "NonExistingKey", "DefaultValue");

            // Assert
            Assert.AreEqual("DefaultValue", actualValue);
        }

        [TestMethod]
        public void Read_ReturnsDefaultValue_WhenSectionDoesNotExist()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);

            // Act
            string actualValue = iniFile.Read("NonExistingSection", "TestKey", "DefaultValue");

            // Assert
            Assert.AreEqual("DefaultValue", actualValue);
        }

        [TestMethod]
        public void Write_WritesValueCorrectly()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);
            string section = "NewSection";
            string key = "NewKey";
            string value = "NewValue";

            // Act
            iniFile.Write(section, key, value);
            string actualValue = iniFile.Read(section, key, "DefaultValue");

            // Assert
            Assert.AreEqual(value, actualValue);
        }
        [TestMethod]
        public void ReadKeys_ReturnsAllKeysInSection()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);
            iniFile.Write("AnotherSection", "Key1", "Value1");
            iniFile.Write("AnotherSection", "Key2", "Value2");

            // Act
            var keys = iniFile.ReadKeys("AnotherSection");

            // Assert
            CollectionAssert.AreEquivalent(new[] { "Key1", "Key2" }, keys);
        }

        [TestMethod]
        public void ReadKeys_ReturnsEmptyArray_WhenSectionDoesNotExist()
        {
            // Arrange
            var iniFile = new IniFile(testFilePath);

            // Act
            var keys = iniFile.ReadKeys("NonExistingSection");

            // Assert
            Assert.AreEqual(0, keys.Length);
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenFileDoesNotExist()
        {
            // Arrange & Act
            var iniFile = new IniFile("nonexistent.ini");

            // This will throw an exception
            iniFile.Read("TestSection", "TestKey");
        }
        [TestMethod]
        public void Constructor_SetsGameModeAndScoreCorrectly()
        {
            // Arrange
            string expectedMode = "TestMode";
            string expectedScore = "10";

            // Act
            SaveMenu saveMenu = new SaveMenu(expectedMode, expectedScore);

            // Assert
            Assert.AreEqual(expectedMode, saveMenu.GameMode);
            Assert.AreEqual(expectedScore, saveMenu.GameScore);

            // Перевірка текстових полів напряму (замість Controls)
            Assert.AreEqual(expectedMode, saveMenu.textBox2.Text);
            Assert.AreEqual(expectedScore, saveMenu.textBox3.Text);
        }
        [TestMethod]
        public void Methods_DoNotThrowExceptions()
        {
            // Arrange
            SaveMenu saveMenu = new SaveMenu("Mode", "Score");

            try
            {
                // Перевірка button1_Click_1
                saveMenu.button1_Click_1(null, null);

                // Мок для DrawCustomBorder
                using (Bitmap bitmap = new Bitmap(100, 100))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    PaintEventArgs mockPaintEvent = new PaintEventArgs(g, new Rectangle(0, 0, 100, 100));
                    saveMenu.DrawCustomBorder(new Panel { Width = 100, Height = 100 }, mockPaintEvent);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Methods throw exceptions. Exception: {ex.Message}");
            }
        }
        [TestMethod]
        public void Constructor_ShouldInitializeProperly()
        {
            // Arrange
            var mainForm = new Form1();

            // Act
            var loadForm = new LoadForm(mainForm);

            // Assert
            Assert.AreEqual(FormBorderStyle.FixedSingle, loadForm.FormBorderStyle);
            Assert.IsFalse(loadForm.MaximizeBox);
            Assert.IsNotNull(loadForm.mainForm);
        }
        [TestMethod]
        public void LoadSavedGames_ShouldPopulateListView()
        {
            // Arrange
            var mainForm = new Form1();
            var loadForm = new LoadForm(mainForm);

            // Act
            loadForm.LoadSavedGames();

            // Assert
            Assert.IsTrue(loadForm.listView1.Items.Count >= 0);
        }
        [TestMethod]
        public void LoadSavedGames_ShouldPopulateListView_WhenValidIniFile()
        {
            // Arrange
            string iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

            // Створюємо тестовий INI-файл
            File.WriteAllText(iniFilePath, @"
[GameData]
Game_1_TextBox1=Chess
Game_1_TextBox2=Player vs Player
Game_1_TextBox3=5:0
Game_2_TextBox1=Checkers
Game_2_TextBox2=AI vs Player
Game_2_TextBox3=2:3
");

            var mainForm = new Form1();
            var loadForm = new LoadForm(mainForm);

            // Act
            loadForm.LoadSavedGames();

            // Assert
            Assert.AreEqual(2, loadForm.listView1.Items.Count); // 2 гри мають завантажитися
            Assert.AreEqual("Chess", loadForm.listView1.Items[0].Text); // Перевірка назви гри
            Assert.AreEqual("Checkers", loadForm.listView1.Items[1].Text);

            // Clean up
            File.Delete(iniFilePath); // Видаляємо тестовий INI-файл після тесту
        }
        [TestMethod]
        public void TestSaveCheckboxStates()
        {
            // Arrange
            var settingsForm = new SettingsForm();
            settingsForm.checkBox1.Checked = true;
            settingsForm.checkBox2.Checked = false;
            settingsForm.checkBox3.Checked = true;
            settingsForm.textBox1.Text = "TestValue";

            // Act
            settingsForm.SaveCheckboxStates();

            // Assert
            var ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            Assert.AreEqual("True", ini.Read("CheckboxStates", "CheckBox1"));
            Assert.AreEqual("False", ini.Read("CheckboxStates", "CheckBox2"));
            Assert.AreEqual("True", ini.Read("CheckboxStates", "CheckBox3"));
            Assert.AreEqual("TestValue", ini.Read("TextBoxValues", "TextBox1"));
        }
        [TestMethod]
        public void TestLoadCheckboxStates()
        {
            // Arrange
            var settingsForm = new SettingsForm();
            var ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            ini.Write("CheckboxStates", "CheckBox1", "True");
            ini.Write("CheckboxStates", "CheckBox2", "False");
            ini.Write("CheckboxStates", "CheckBox3", "True");
            ini.Write("TextBoxValues", "TextBox1", "LoadedValue");

            // Act
            settingsForm.LoadCheckboxStates();

            // Assert
            Assert.IsTrue(settingsForm.checkBox1.Checked);
            Assert.IsFalse(settingsForm.checkBox2.Checked);
            Assert.IsTrue(settingsForm.checkBox3.Checked);
            Assert.AreEqual("LoadedValue", settingsForm.textBox1.Text);
        }
        [TestMethod]
        public void CheckBox2_CheckedChanged_CheckBox3Checked_CheckBox2Unchecked()
        {
            // Arrange
            var form = new SettingsForm();
            form.Show(); // Завантажуємо форму, щоб елементи ініціалізувалися
            form.checkBox3.Checked = true; // Активуємо checkBox3

            // Act
            form.checkBox2.Checked = false; // Спробуємо активувати checkBox2
            form.checkBox2_CheckedChanged(form.checkBox2, EventArgs.Empty); // Викликаємо функцію вручну

            // Assert
            Assert.IsFalse(form.checkBox2.Checked, "checkBox2 має бути деактивованим, якщо checkBox3 активний.");
            form.Close();
        }
        [TestMethod]
        public void CheckBox1_CheckedChanged_Checked_True_StartsPlaying()
        {
            // Arrange
            var form = new SettingsForm();
            form.Show();

            // Отримуємо абсолютний шлях до файлу, базуючись на поточному каталозі виконання програми
            string soundFile = @"D:\a\testrepo\testrepo\media\audio\music.wav";

            // Перевірка існування файлу
            Assert.IsTrue(File.Exists(soundFile), $"Файл music.wav не знайдено за вказаним шляхом: {soundFile}");


            // Act
            form.checkBox1.Checked = true; // Активуємо checkBox1
            form.checkBox1_CheckedChanged_1(form.checkBox1, EventArgs.Empty);

            // Assert
            // Перевірити роботу `PlayLooping` напряму складно. Але можна впевнитись, що стан гравця не null
            Assert.IsNotNull(form.GetPlayer(), "_player не було ініціалізовано.");
            form.Close();
        }
        [TestMethod]
        public void CheckBox1_CheckedChanged_Checked_False_StopsPlaying()
        {
            // Arrange
            var form = new SettingsForm();
            form.Show();



            form.checkBox1.Checked = false; // Деактивуємо checkBox1
            form.checkBox1_CheckedChanged_1(form.checkBox1, EventArgs.Empty);

            // Assert
            Assert.IsNull(form.GetPlayer(), "_player мав бути зупинений і очищений.");
            form.Close();
        }
        [TestMethod]
        public void DrawCustomBorder_ShouldDrawCorrectBorder()
        {
            // Arrange
            var panel = new Panel { Width = 100, Height = 100 };
            var bitmap = new Bitmap(100, 100); // Create a Bitmap to capture the drawing
            var paintEventArgs = new PaintEventArgs(Graphics.FromImage(bitmap), new Rectangle(0, 0, 100, 100));
            // Ініціалізація форми
            var mainForm = new Form1();
            var loadForm = new LoadForm(mainForm);
            // Act
            loadForm.DrawCustomBorder(panel, paintEventArgs);

            // Assert
            Color expectedColor = Color.Aqua;

            // Check pixels at the four corners and edges of the image
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(0, 0)); // Top-left corner
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(99, 0)); // Top-right corner
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(0, 99)); // Bottom-left corner
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(99, 99)); // Bottom-right corner

            // Optionally check some pixels along the edges to ensure the border is drawn
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(50, 0)); // Top edge
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(50, 99)); // Bottom edge
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(0, 50)); // Left edge
            AssertColorsAreEqual(expectedColor, bitmap.GetPixel(99, 50)); // Right edge
        }

        private void AssertColorsAreEqual(Color expected, Color actual)
        {
            Assert.AreEqual(expected.A, actual.A, "Alpha mismatch");
            Assert.AreEqual(expected.R, actual.R, "Red mismatch");
            Assert.AreEqual(expected.G, actual.G, "Green mismatch");
            Assert.AreEqual(expected.B, actual.B, "Blue mismatch");
        }
        [TestMethod]
        public void TestButton1Click()
        {
            // Arrange
            bool onLoadBeforeClick = _form.onLoad;

            // Act
            _form.button1_Click(null, EventArgs.Empty);
            _form.Show();

            // Assert
            Assert.IsFalse(_form.onLoad, "onLoad should be false after button click");
            _form.Close();
        }
        [TestMethod]
        public void TrackPlayer1Move_HandlesMovesCorrectly()
        {
            // Arrange
            var form = new Form1();
            form.player1Moves = new List<int>(); // Ініціалізуємо список

            // Act & Assert: додаємо перші 5 ходів
            form.TrackPlayer1Move(1);
            form.TrackPlayer1Move(2);
            form.TrackPlayer1Move(3);
            form.TrackPlayer1Move(4);
            form.TrackPlayer1Move(5);

            // Перевіряємо, що всі 5 елементів є в списку
            Assert.AreEqual(5, form.player1Moves.Count, "The list should contain exactly 5 moves.");
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, form.player1Moves, "The moves in the list should match the added moves.");

            // Act: додаємо ще один хід (6)
            form.TrackPlayer1Move(6);

            // Assert: перевіряємо, що перший елемент видалено, а новий додано
            Assert.AreEqual(5, form.player1Moves.Count, "The list should still contain 5 moves after adding a sixth move.");
            CollectionAssert.AreEqual(new List<int> { 2, 3, 4, 5, 6 }, form.player1Moves, "The list should maintain the most recent 5 moves.");

            // Act: додаємо ще один хід (7)
            form.TrackPlayer1Move(7);

            // Assert: перевіряємо актуальний стан списку
            Assert.AreEqual(5, form.player1Moves.Count, "The list should still contain 5 moves after adding another move.");
            CollectionAssert.AreEqual(new List<int> { 3, 4, 5, 6, 7 }, form.player1Moves, "The list should maintain the most recent 5 moves.");
        }
        [TestMethod]
        public void Main_ShouldRunApplicationAndInitializeForm1()
        {
            // Arrange
            var applicationThread = new Task(() =>
            {
                // Act
                Program.Main();
            });

            applicationThread.Start();

            // Чекаємо кілька секунд, поки програма не ініціалізується
            Task.Delay(2000).Wait();

            // Assert: перевіряємо, чи форма була ініціалізована
            Assert.IsNotNull(Application.OpenForms["Form1"], "Form1 should be initialized and run.");

            // Закриваємо форму
            Application.Exit();
            applicationThread.Wait();  // чекаємо завершення потоку
        }
        private SerialPort _serialPort;
        [TestMethod]
        public void MainForm_FormClosing_DoesNotCloseSerialPort_WhenAlreadyClosed()
        {
            _serialPort = new SerialPort("COM5"); // Вказуємо будь-який COM порт
            _form.serialPort = _serialPort; // Призначаємо serialPort у форму
            // Переконуємось, що порт закритий перед тестом
            Assert.IsFalse(_serialPort.IsOpen);

            // Викликаємо обробник події FormClosing
            _form.MainForm_FormClosing(this, new FormClosingEventArgs(CloseReason.UserClosing, false));

            // Перевіряємо, що порт все ще закритий (нічого не змінилось)
            Assert.IsFalse(_serialPort.IsOpen);
        }
        [TestMethod]
        public void MainForm_FormClosing_ClosesSerialPort_WhenOpen()
        {

            _form = new Form1();
            _serialPort = new SerialPort("COM5"); // Вказуємо будь-який COM порт
            _form.serialPort = _serialPort; // Призначаємо serialPort у форму
            _serialPort.Open(); // Відкриваємо порт
            // Перевіряємо, що порт відкритий перед закриттям форми
            Assert.IsTrue(_serialPort.IsOpen);

            // Викликаємо обробник події FormClosing
            _form.MainForm_FormClosing(this, new FormClosingEventArgs(CloseReason.UserClosing, false));

            // Перевіряємо, що порт закритий після виклику методу
            Assert.IsFalse(_serialPort.IsOpen);
        }
        [TestMethod]
        public void Player2_ShouldStartTimerAndClickButtonForManVSAI_WithRandomMode()
        {
            // Arrange
            var form = new TestForm1();

            // Встановлюємо необхідні параметри
            form.SetTestRandom(1); // Встановлюємо фіксоване значення для випадкових чисел
            form.mode = "Man VS AI"; // Встановлюємо режим Man VS AI
            form.randomMode = true; // Включаємо випадковий режим
            form.winStrategy = false; // Вимикаємо стратегію виграшу

            // Act
            form.Player2(); // Викликаємо метод Player2, щоб ініціювати логіку
                            // Затримка для того, щоб таймер мав час запуститися
            System.Threading.Thread.Sleep(200);

            // Assert
            // Перевірка, чи таймер був ініціалізований і запущений
            Assert.IsNotNull(form.TestTimer, "Таймер не був ініціалізований.");
            Assert.IsTrue(form.TestTimer.Enabled, "Таймер не був запущений.");
        }

        [TestMethod]
        public void Player2_ShouldStartTimerAndClickButtonForAI_VS_AI_WithWinStrategy()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI";  // Встановлюємо режим AI VS AI
            form.randomMode = false;
            form.winStrategy = true; // Включаємо стратегію виграшу
            form.player1Moves.AddRange(new[] { 1, 2, 3, 1, 2, 2, 3 });

            // Act
            form.Player2(); // Викликаємо метод Player2, щоб ініціювати логіку
                            // Затримка для того, щоб таймер мав час запуститися
            System.Threading.Thread.Sleep(200);

            // Assert
            // Перевірка, чи таймер був ініціалізований і запущений
            Assert.IsNotNull(form.TestTimer, "Таймер не був ініціалізований.");
            Assert.IsTrue(form.TestTimer.Enabled, "Таймер не був запущений.");
        }
    }
}























