﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using game_client;
using System.Windows.Forms;

namespace testingMenu
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ModMenu_ShouldSetCorrectVisibility()
        {
            // Arrange
            using (var mainForm = new Form1())
            {
                // Емулюємо відкриття форми, щоб усі елементи були ініціалізовані
                mainForm.Show();
                mainForm.CreateControl(); // Створює всі елементи управління

                // Act
                mainForm.ModMenu(); // Викликаємо метод для зміни видимості елементів

                // Assert
                Assert.IsFalse(mainForm.button1.Visible, "button1 should be hidden.");
                Assert.IsFalse(mainForm.button22.Visible, "button22 should be hidden.");
                Assert.IsFalse(mainForm.button21.Visible, "button21 should be hidden.");
                Assert.IsFalse(mainForm.panel17.Visible, "panel17 should be hidden.");
                Assert.IsFalse(mainForm.pictureBox15.Visible, "pictureBox15 should be hidden.");

                Assert.IsTrue(mainForm.label2.Visible, "label2 should be visible.");
                Assert.IsTrue(mainForm.panel1.Visible, "panel1 should be visible.");
                Assert.IsTrue(mainForm.panel2.Visible, "panel2 should be visible.");
                Assert.IsTrue(mainForm.button2.Visible, "button2 should be visible.");
                Assert.IsTrue(mainForm.button3.Visible, "button3 should be visible.");
                Assert.IsTrue(mainForm.button4.Visible, "button4 should be visible.");

                // Закриваємо форму після перевірки
                mainForm.Close();
            }
        }
    }
}

