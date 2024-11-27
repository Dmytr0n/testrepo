using System;
using System.Data; 
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace game_client
{
    /// \class LoadForm
    /// \brief This class is responsible for loading saved games in the game client.
    /// 
    /// \image html media/doc_img/load_form.png
    /// 
    /// This diagram illustrates the inheritance structure for the LoadForm class.
    public partial class LoadForm : Form
    {
        private Dictionary<string, string> gameKeys = new Dictionary<string, string>(); 
        private Form1 mainForm;
        /// <summary>
        /// Initializes a new instance of the <c>LoadForm</c> class.
        /// </summary>
        /// <param name="mainForm">The main form instance (<c>Form1</c>) used for interaction with the main application.</param>
        /// <remarks>
        /// This constructor performs the following actions:
        /// 1. Sets the form's border style to <c>FixedSingle</c> to prevent resizing.
        /// 2. Disables the maximize button.
        /// 3. Calls <see cref="LoadSavedGames"/> to populate the list of saved games.
        /// 4. Adds custom border painting for <c>panel1</c> and <c>listView1</c>.
        /// </remarks>
        /// @code
        /// public LoadForm(Form1 mainForm)
        /// {
        ///     InitializeComponent();
        ///     this.mainForm = mainForm;
        ///     this.FormBorderStyle = FormBorderStyle.FixedSingle;
        ///     this.MaximizeBox = false;
        ///     LoadSavedGames();
        ///     panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     listView1.Paint += new PaintEventHandler(DrawCustomBorder);
        /// }
        /// @endcode
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
        /// <summary>
        /// Loads the saved games from a configuration file and populates the <c>ListView</c>.
        /// </summary>
        /// <remarks>
        /// This method performs the following tasks:
        /// 1. Clears the existing items in the list view and the dictionary of game keys.
        /// 2. Reads the saved game data from a configuration file named <c>config.ini</c>.
        /// 3. Filters the data to extract game-related entries based on predefined key patterns.
        /// 4. Populates the <c>ListView</c> with the game names, modes, and scores.
        /// 5. Stores keys in a dictionary for easy lookup when deleting a game session.
        /// </remarks>
        /// <exception cref="FileNotFoundException">Thrown if the configuration file is not found.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during file processing.</exception>
        /// @code
        /// private void LoadSavedGames()
        /// {
        ///     try
        ///     {
        ///         listView1.Items.Clear();
        ///         gameKeys.Clear();
        ///         IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///         var keys = ini.ReadKeys("GameData");
        ///         int gameCount = 0;
        ///
        ///         foreach (var key in keys)
        ///         {
        ///             if (key.StartsWith("Game_") && key.EndsWith("_TextBox1"))
        ///             {
        ///                 string value1 = ini.Read("GameData", key, "");
        ///                 string value2 = ini.Read("GameData", key.Replace("_TextBox1", "_TextBox2"), "");
        ///                 string value3 = ini.Read("GameData", key.Replace("_TextBox1", "_TextBox3"), "");
        ///
        ///                 if (!string.IsNullOrWhiteSpace(value1))
        ///                 {
        ///                     ListViewItem item = new ListViewItem(value1.Trim());
        ///                     item.SubItems.Add(value2.Trim());
        ///                     item.SubItems.Add(value3.Trim());
        ///                     listView1.Items.Add(item);
        ///                     gameCount++;
        ///                     gameKeys[value1.Trim() + ";" + value2.Trim() + ";" + value3.Trim()] = key;
        ///                 }
        ///             }
        ///         }
        ///         MessageBox.Show($"Loaded {gameCount} games", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ///     }
        ///     catch (FileNotFoundException ex)
        ///     {
        ///         MessageBox.Show($"File not found: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        /// }
        /// @endcode
        public void LoadSavedGames()
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
        // <summary>
        /// Draws a custom border around a given <c>Panel</c>.
        /// </summary>
        /// <param name="sender">The object that triggered the event (in this case, a <c>Panel</c>).</param>
        /// <param name="e">Contains event data needed for custom painting.</param>
        /// <remarks>
        /// This method customizes the appearance of a <c>Panel</c> or <c>ListView</c> by drawing a colored border
        /// around its edges using the <c>Graphics</c> object.
        /// </remarks>
        /// @code
        /// private void DrawCustomBorder(object sender, PaintEventArgs e)
        /// {
        ///     Panel panel = sender as Panel;
        ///     Graphics g = e.Graphics;
        ///     Pen pen = new Pen(Color.Aqua, 3);
        ///     g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        /// }
        /// @endcode
        public void DrawCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;

            // Задання кольору і товщини пензля для рамки
            Pen pen = new Pen(Color.Aqua, 3);

            // Малюємо прямокутну рамку навколо панелі
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }
        /// <summary>
        /// Handles the double-click event on the <c>ListView</c> to load the selected game.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the <c>ListView</c>).</param>
        /// <param name="e">Contains information about the mouse event, including the location of the double-click.</param>
        /// <remarks>
        /// When the user double-clicks on a game in the <c>ListView</c>, this method retrieves the selected game's
        /// mode and score and sends the data to the main form for further processing.
        /// After transferring the data, the <c>LoadForm</c> is closed.
        /// </remarks>
        /// @code
        /// private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        /// {
        ///     if (listView1.SelectedItems.Count > 0)
        ///     {
        ///         ListViewItem selectedItem = listView1.SelectedItems[0];
        ///         string gameMode = selectedItem.SubItems[1].Text;
        ///         string gameScore = selectedItem.SubItems[2].Text;
        ///         mainForm.SetGameData(gameMode, gameScore);
        ///         this.Close();
        ///     }
        /// }
        /// @endcode
        public void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
        /// <summary>
        /// Handles the click event on the "Delete Game" button to remove a selected game session.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button).</param>
        /// <param name="e">Contains event data related to the button click.</param>
        /// <remarks>
        /// This method performs the following actions:
        /// 1. Checks if a game is selected in the <c>ListView</c>.
        /// 2. Locates the selected game in the configuration file using a constructed key.
        /// 3. Removes the game data from the file if it exists.
        /// 4. Updates the <c>ListView</c> to reflect the deletion.
        /// </remarks>
        /// <exception cref="IOException">Thrown if an error occurs during file access or modification.</exception>
        /// @code
        /// private void button1_Click_1(object sender, EventArgs e)
        /// {
        ///     if (listView1.SelectedItems.Count > 0)
        ///     {
        ///         string selectedGameName = listView1.SelectedItems[0].SubItems[0].Text;
        ///         string selectedGameMode = listView1.SelectedItems[0].SubItems[1].Text;
        ///         string selectedGameScore = listView1.SelectedItems[0].SubItems[2].Text;
        ///
        ///         string searchKey = $"{selectedGameName};{selectedGameMode};{selectedGameScore}";
        ///
        ///         if (gameKeys.TryGetValue(searchKey, out string selectedGameKey))
        ///         {
        ///             string iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
        ///             var lines = File.ReadAllLines(iniFilePath);
        ///             var filteredLines = new List<string>();
        ///             bool isGameFound = false;
        ///
        ///             for (int i = 0; i < lines.Length; i++)
        ///             {
        ///                 if (lines[i].StartsWith(selectedGameKey))
        ///                 {
        ///                     isGameFound = true;
        ///                     if (i + 1 < lines.Length) i++;
        ///                     if (i + 1 < lines.Length) i++;
        ///                 }
        ///                 else
        ///                 {
        ///                     filteredLines.Add(lines[i]);
        ///                 }
        ///             }
        ///
        ///             if (!isGameFound)
        ///             {
        ///                 MessageBox.Show("Game session not found for deletion.");
        ///                 return;
        ///             }
        ///
        ///             File.WriteAllLines(iniFilePath, filteredLines);
        ///             MessageBox.Show("Game session deleted successfully.");
        ///             LoadSavedGames();
        ///         }
        ///         else
        ///         {
        ///             MessageBox.Show("Game session not found for deletion.");
        ///         }
        ///     }
        ///     else
        ///     {
        ///         MessageBox.Show("Please select a game session to delete.");
        ///     }
        /// }
        /// @endcode
        public void button1_Click_1(object sender, EventArgs e)
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

