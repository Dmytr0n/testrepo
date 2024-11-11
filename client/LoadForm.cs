using System;
using System.Data; // Додайте це
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace game_client
{
    public partial class LoadForm : Form
    {
        private Dictionary<string, string> gameKeys = new Dictionary<string, string>(); // Словник для зберігання ключів
        private Form1 mainForm;

        public LoadForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            LoadSavedGames();
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            listView1.Paint += new PaintEventHandler(DrawCustomBorder);
        }

        private void LoadSavedGames()
        {
            try
            {
                listView1.Items.Clear();
                gameKeys.Clear(); // Очищаємо словник перед завантаженням нових ігор

                IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));

                var keys = ini.ReadKeys("GameData");
                int gameCount = 0;

                foreach (var key in keys)
                {
                    if (key.StartsWith("Game_") && key.EndsWith("_TextBox1"))
                    {
                        // Отримуємо дані для гри
                        string value1 = ini.Read("GameData", key, "");
                        string value2 = ini.Read("GameData", key.Replace("_TextBox1", "_TextBox2"), "");
                        string value3 = ini.Read("GameData", key.Replace("_TextBox1", "_TextBox3"), "");

                        // Перевірка на наявність значень
                        if (!string.IsNullOrWhiteSpace(value1))
                        {
                            ListViewItem item = new ListViewItem(value1.Trim());
                            item.SubItems.Add(value2.Trim());
                            item.SubItems.Add(value3.Trim());

                            listView1.Items.Add(item);
                            gameCount++;

                            // Зберігаємо ключ для видалення
                            gameKeys[value1.Trim() + ";" + value2.Trim() + ";" + value3.Trim()] = key; // Формуємо ключ
                        }
                    }
                }
                MessageBox.Show($"Loaded {gameCount} games", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void label1_Click(object sender, EventArgs e)
        {
            // Ваш код для обробки кліку на label1, якщо потрібен
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

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string gameMode = selectedItem.SubItems[1].Text; // Режим гри
                string gameScore = selectedItem.SubItems[2].Text; // Рахунок гри

                // Викликаємо метод у MainForm для передачі даних
                mainForm.SetGameData(gameMode, gameScore);
                this.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // Отримання даних з вибраного елемента
                string selectedGameName = listView1.SelectedItems[0].SubItems[0].Text; // Назва гри
                string selectedGameMode = listView1.SelectedItems[0].SubItems[1].Text; // Режим
                string selectedGameScore = listView1.SelectedItems[0].SubItems[2].Text; // Рахунок

                // Формуємо ключ для пошуку
                string searchKey = $"{selectedGameName};{selectedGameMode};{selectedGameScore}";

                // Перевірка, чи є відповідний ключ у словнику
                if (gameKeys.TryGetValue(searchKey, out string selectedGameKey))
                {
                    // Шлях до вашого .ini файлу
                    string iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

                    // Читання всіх рядків з .ini файлу
                    var lines = File.ReadAllLines(iniFilePath); // Це повертає string[]

                    // Список для зберігання рядків, які залишаються
                    var filteredLines = new List<string>();

                    bool isGameFound = false; // Прапорець для перевірки, чи знайдено гру

                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Перевіряємо, чи цей рядок є ключем гри
                        if (lines[i].StartsWith(selectedGameKey))
                        {
                            isGameFound = true; // Гру знайдено
                                                // Пропускаємо два наступні рядки
                            if (i + 1 < lines.Length) i++; // Пропускаємо наступний рядок
                            if (i + 1 < lines.Length) i++; // Пропускаємо ще один наступний рядок
                        }
                        else
                        {
                            // Якщо гру не знайшли, додаємо рядок у список
                            filteredLines.Add(lines[i]);
                        }
                    }
                    // Якщо гра не знайдена, покажіть повідомлення
                    if (!isGameFound)
                    {
                        MessageBox.Show("Game session not found for deletion.");
                        return;
                    }

                    // Запис назад у .ini файл
                    File.WriteAllLines(iniFilePath, filteredLines);

                    MessageBox.Show("Game session deleted successfully.");
                    LoadSavedGames(); // Оновлюємо список ігор
                }
                else
                {
                    MessageBox.Show("Game session not found for deletion.");
                }
            }
            else
            {
                MessageBox.Show("Please select a game session to delete.");
            }


        }
    }
}

