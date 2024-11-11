using System;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using System.IO;

namespace game_client
{
    public partial class SettingsForm : Form
    {
        private SoundPlayer _player;

        public int Rounds { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();
            LoadCheckboxStates(); // Завантажити стан чекбоксів
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            panel2.Paint += new PaintEventHandler(DrawCustomBorder);
            panel3.Paint += new PaintEventHandler(DrawCustomBorder);
            panel4.Paint += new PaintEventHandler(DrawCustomBorder);
            panel5.Paint += new PaintEventHandler(DrawCustomBorder);
        }
        // Метод для збереження стану чекбоксів у .ini файл
        private void SaveCheckboxStates()
        {
            try
            {
                IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));

                ini.Write("CheckboxStates", "CheckBox1", checkBox1.Checked.ToString());
                ini.Write("CheckboxStates", "CheckBox2", checkBox2.Checked.ToString());
                ini.Write("CheckboxStates", "CheckBox3", checkBox3.Checked.ToString());
                ini.Write("TextBoxValues", "TextBox1", textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving checkbox states: {ex.Message}");
            }

        }

        // Метод для завантаження стану чекбоксів з .ini файлу
        private void LoadCheckboxStates()
        {
            try
            {
                IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));

                checkBox1.Checked = bool.Parse(ini.Read("CheckboxStates", "CheckBox1", ""));
                checkBox2.Checked = bool.Parse(ini.Read("CheckboxStates", "CheckBox2", ""));
                checkBox3.Checked = bool.Parse(ini.Read("CheckboxStates", "CheckBox3", ""));
                textBox1.Text = ini.Read("TextBoxValues", "TextBox1", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading checkbox states: {ex.Message}");
            }

        }

        // Обробник події для закриття форми
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveCheckboxStates(); // Зберегти стан чекбоксів перед закриттям
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
        

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false; // Деактивуємо checkBox2, якщо checkBox1 активовано
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox3.Checked = false; // Деактивуємо checkBox1, якщо checkBox2 активовано
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                _player = new SoundPlayer();
                _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                _player.LoadAsync();
                _player.PlayLooping();
            }
            else
            {
                _player?.Stop();
            }
        }
    }
}
