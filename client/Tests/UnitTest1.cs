using Microsoft.VisualStudio.TestTools.UnitTesting;
using game_client;
using System;
using System.IO;
using System.Windows.Forms;

namespace testingMenu
{
    [TestClass]
    public class UnitTest1
    {
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
    }
}


