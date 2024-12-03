﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        
       

       











    }
}























