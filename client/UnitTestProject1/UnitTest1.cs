using Microsoft.VisualStudio.TestTools.UnitTesting;
using game_client;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

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
        public void Player1_ShouldClickAtLeastOneButton()
        {
            // Arrange
            var form = new TestForm1();
            form.mode = "AI VS AI"; // Налаштовуємо режим

            bool button5Clicked = false;
            bool button6Clicked = false;
            bool button7Clicked = false;

            // Підписуємось на події кліку для трьох кнопок
            form.button5.Click += (s, e) => button5Clicked = true;
            form.button6.Click += (s, e) => button6Clicked = true;
            form.button7.Click += (s, e) => button7Clicked = true;

            // Фіксоване значення для випадкових чисел
            form.SetTestRandom(1); // Задаємо фіксоване значення для випадковості (можна змінювати для тесту інших кнопок)

            // Act
            form.Player1(); // Викликаємо метод Player1

            // Затримка, щоб дочекатись завершення таймера
            System.Threading.Thread.Sleep(1500); // Затримка для тесту (можна зменшити або збільшити залежно від інтервалу таймера)

            // Assert: Перевіряємо, що хоча б одна кнопка була натиснута
            Assert.IsTrue(button5Clicked || button6Clicked || button7Clicked, "Жодна кнопка не була натиснута.");
        }
    }
}

