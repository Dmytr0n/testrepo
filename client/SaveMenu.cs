using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
namespace game_client
{
    public partial class SaveMenu : Form
    {
        private string gameMode;
        private string gameScore;

        public SaveMenu(string mode, string score)
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            this.gameMode = mode;
            this.gameScore = score;
            textBox2.Text = gameMode;
            textBox3.Text = gameScore;
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            panel2.Paint += new PaintEventHandler(DrawCustomBorder);
            panel3.Paint += new PaintEventHandler(DrawCustomBorder);
            panel4.Paint += new PaintEventHandler(DrawCustomBorder);
        }
        private void DrawCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;

            // Задання кольору і товщини пензля для рамки
            Pen pen = new Pen(Color.Aqua, 3);

            // Малюємо прямокутну рамку навколо панелі
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));

                // Генерація нового унікального ключа для кожної гри
                string gameKey = $"Game_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}"; // Формат: Game_YYYYMMDD_HHMMSS

                // Записуємо дані у config.ini з новим унікальним ключем
                ini.Write("GameData", $"{gameKey}_TextBox1", textBox1.Text);
                ini.Write("GameData", $"{gameKey}_TextBox2", textBox2.Text);
                ini.Write("GameData", $"{gameKey}_TextBox3", textBox3.Text);

                MessageBox.Show("Дані успішно збережені у config.ini!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}\nError type: {ex.GetType().Name}\n{ex.StackTrace}");
            }
        }

        private void SaveMenu_Load(object sender, EventArgs e)
        {

        }
    }
}

