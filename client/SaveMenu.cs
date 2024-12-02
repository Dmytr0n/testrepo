using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
namespace game_client
{
    /// \class SaveMenu
    /// \brief This class handles the saving of game data, displaying game mode and score to the user.
    /// 
    /// \image html media/doc_img/save_menu.png
    /// 
    /// This diagram illustrates the functionality of the SaveMenu class, which includes:
    /// 1. A form with read-only textboxes displaying the game mode and score.
    /// 2. A custom border drawing function for panel elements.
    /// 3. A save button that writes the game data to a configuration file.
    public partial class SaveMenu : Form
    {
        private string gameMode;
        private string gameScore;
        /// <summary>
        /// Initializes a new instance of the <c>SaveMenu</c> class.
        /// </summary>
        /// <param name="mode">The game mode to be displayed in the form.</param>
        /// <param name="score">The game score to be displayed in the form.</param>
        /// <remarks>
        /// This constructor performs the following actions:
        /// 1. Initializes form components using <c>InitializeComponent</c>.
        /// 2. Sets <c>textBox2</c> and <c>textBox3</c> to read-only to prevent user modification.
        /// 3. Disables the maximize button and sets the border style to <c>FixedSingle</c>.
        /// 4. Assigns game mode and score to the appropriate text boxes.
        /// 5. Adds custom border painting functionality for all panels.
        /// </remarks>
        /// @code
        /// public SaveMenu(string mode, string score)
        /// {
        ///     InitializeComponent();
        ///     textBox2.ReadOnly = true;
        ///     textBox3.ReadOnly = true;
        ///     this.FormBorderStyle = FormBorderStyle.FixedSingle;
        ///     this.MaximizeBox = false;
        ///     this.gameMode = mode;
        ///     this.gameScore = score;
        ///     textBox2.Text = gameMode;
        ///     textBox3.Text = gameScore;
        ///     panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel2.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel3.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel4.Paint += new PaintEventHandler(DrawCustomBorder);
        /// }
        /// @endcode
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
        /// <summary>
        /// Draws a custom border around the specified panel.
        /// </summary>
        /// <param name="sender">The panel triggering the paint event.</param>
        /// <param name="e">The paint event arguments containing graphics data.</param>
        /// <remarks>
        /// This method is used to create a custom visual appearance for the panels
        /// by drawing a rectangular border with a specified color and thickness.
        /// </remarks>
        /// @code
        /// private void DrawCustomBorder(object sender, PaintEventArgs e)
        /// {
        ///     Panel panel = sender as Panel;
        ///     Graphics g = e.Graphics;
        ///
        ///     // Set pen color and thickness for the border
        ///     Pen pen = new Pen(Color.Aqua, 3);
        ///
        ///     // Draw a rectangular border around the panel
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
        /// Saves game data to a configuration file when the save button is clicked.
        /// </summary>
        /// <param name="sender">The save button triggering the click event.</param>
        /// <param name="e">The event arguments for the click event.</param>
        /// <remarks>
        /// This method performs the following actions:
        /// 1. Creates or opens a configuration file named <c>config.ini</c>.
        /// 2. Generates a unique game key based on the current date and time.
        /// 3. Saves the content of <c>textBox1</c>, <c>textBox2</c>, and <c>textBox3</c>
        ///    under the generated game key in the configuration file.
        /// 4. Displays a success message if the operation is successful, or an error
        ///    message if an exception occurs.
        /// </remarks>
        /// @code
        /// private void button1_Click_1(object sender, EventArgs e)
        /// {
        ///     try
        ///     {
        ///         IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///
        ///         // Generate a unique key for each game
        ///         string gameKey = $"Game_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
        ///
        ///         // Write data to config.ini with the new unique key
        ///         ini.Write("GameData", $"{gameKey}_TextBox1", textBox1.Text);
        ///         ini.Write("GameData", $"{gameKey}_TextBox2", textBox2.Text);
        ///         ini.Write("GameData", $"{gameKey}_TextBox3", textBox3.Text);
        ///
        ///         MessageBox.Show("Data successfully saved to config.ini!");
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         MessageBox.Show($"Error saving data: {ex.Message}\nError type: {ex.GetType().Name}\n{ex.StackTrace}");
        ///     }
        /// }
        /// @endcode
        public void button1_Click_1(object sender, EventArgs e)
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
    }
}

