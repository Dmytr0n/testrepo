using System;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using System.IO;

namespace game_client
{
    /// \class SettingsForm
    /// \brief This class is responsible for managing game settings, including checkbox states, sound options, and the number of rounds.
    /// 
    /// \image html media/doc_img/settings_form.png
    /// 
    /// This diagram illustrates the main functionality of the SettingsForm class, which includes:
    /// 1. A form that manages checkbox states and sound settings, storing them in a configuration file.
    /// 2. A method to save checkbox states and other values to an .ini file.
    /// 3. A method to load saved settings from the configuration file.
    /// 4. Sound management, which plays background music when a specific checkbox is checked and stops when unchecked.
    /// 5. Custom drawing for borders around panels.
    public partial class SettingsForm : Form
    {
        private SoundPlayer _player;

        public int Rounds { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <c>SettingsForm</c> class.
        /// </summary>
        /// <remarks>
        /// This constructor performs the following actions:
        /// 1. Initializes form components using <c>InitializeComponent</c>.
        /// 2. Loads checkbox states and text box values using <c>LoadCheckboxStates</c>.
        /// 3. Sets the form's border style to <c>FixedSingle</c> to prevent resizing.
        /// 4. Disables the maximize button to prevent resizing the form.
        /// 5. Registers custom paint events for all panels to draw custom borders around them.
        /// </remarks>
        /// @code
        /// public SettingsForm()
        /// {
        ///     InitializeComponent();
        ///     LoadCheckboxStates();  // Load previous checkbox states from the config file
        ///     this.FormBorderStyle = FormBorderStyle.FixedSingle;  // Prevent form resizing
        ///     this.MaximizeBox = false;  // Disable maximize button
        ///     panel1.Paint += new PaintEventHandler(DrawCustomBorder);  // Custom border for panel1
        ///     panel2.Paint += new PaintEventHandler(DrawCustomBorder);  // Custom border for panel2
        ///     panel3.Paint += new PaintEventHandler(DrawCustomBorder);  // Custom border for panel3
        ///     panel4.Paint += new PaintEventHandler(DrawCustomBorder);  // Custom border for panel4
        ///     panel5.Paint += new PaintEventHandler(DrawCustomBorder);  // Custom border for panel5
        /// }
        /// @endcode
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
        /// <summary>
        /// Saves the states of the checkboxes and the value in <c>textBox1</c> to the <c>config.ini</c> file.
        /// </summary>
        /// <remarks>
        /// This method writes the current state of the checkboxes and the text in <c>textBox1</c> to the config file.
        /// The checkbox states are written under the section <c>CheckboxStates</c>, and the text box value is written under <c>TextBoxValues</c>.
        /// If an error occurs during the saving process, a message box is shown with the error details.
        /// </remarks>
        /// @code
        /// private void SaveCheckboxStates()
        /// {
        ///     try
        ///     {
        ///         IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///         ini.Write("CheckboxStates", "CheckBox1", checkBox1.Checked.ToString());
        ///         ini.Write("CheckboxStates", "CheckBox2", checkBox2.Checked.ToString());
        ///         ini.Write("CheckboxStates", "CheckBox3", checkBox3.Checked.ToString());
        ///         ini.Write("TextBoxValues", "TextBox1", textBox1.Text);
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         MessageBox.Show($"Error saving checkbox states: {ex.Message}"); // Show error message if saving fails
        ///     }
        /// }
        /// @endcode
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
        /// <summary>
        /// Handles the form's closing event.
        /// Saves the current checkbox states before closing the form.
        /// </summary>
        /// <param name="e">The event arguments for the form closing event.</param>
        /// <remarks>
        /// This method is called when the form is about to be closed. It ensures that the current checkbox states are saved
        /// before the form is closed, by calling <see cref="SaveCheckboxStates"/>.
        /// </remarks>
        /// @code
        /// protected override void OnFormClosing(FormClosingEventArgs e)
        /// {
        ///     base.OnFormClosing(e);
        ///     SaveCheckboxStates(); // Save checkbox states before the form is closed
        /// }
        /// @endcode
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
        /// <summary>
        /// Handles the form's closing event.
        /// Saves the current checkbox states before closing the form.
        /// </summary>
        /// <param name="e">The event arguments for the form closing event.</param>
        /// <remarks>
        /// This method is called when the form is about to be closed. It ensures that the current checkbox states are saved
        /// before the form is closed, by calling <see cref="SaveCheckboxStates"/>.
        /// </remarks>
        /// @code
        /// protected override void OnFormClosing(FormClosingEventArgs e)
        /// {
        ///     base.OnFormClosing(e);
        ///     SaveCheckboxStates(); // Save checkbox states before the form is closed
        /// }
        /// @endcode
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveCheckboxStates(); // Зберегти стан чекбоксів перед закриттям
        }
        /// <summary>
        /// Customizes the appearance of the panel by drawing a border around it.
        /// </summary>
        /// <param name="sender">The object that raised the paint event (the panel).</param>
        /// <param name="e">The event arguments containing the graphics object for drawing.</param>
        /// <remarks>
        /// This method handles the <c>Paint</c> event of the panels. It draws a custom aqua-colored border around the panel
        /// using the specified graphics object and a <c>Pen</c> with a thickness of 3.
        /// </remarks>
        /// @code
        /// private void DrawCustomBorder(object sender, PaintEventArgs e)
        /// {
        ///     Panel panel = sender as Panel;
        ///     Graphics g = e.Graphics;
        ///     Pen pen = new Pen(Color.Aqua, 3); // Set the pen color to aqua and thickness to 3
        ///     g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1); // Draw border around the panel
        /// }
        /// @endcode
        private void DrawCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Graphics g = e.Graphics;

            // Задання кольору і товщини пензля для рамки
            Pen pen = new Pen(Color.Aqua, 3);

            // Малюємо прямокутну рамку навколо панелі
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }
        /// <summary>
        /// Handles the change event for <c>checkBox2</c>. If <c>checkBox3</c> is checked, <c>checkBox2</c> is unchecked.
        /// </summary>
        /// <param name="sender">The object that raised the event (the checkbox).</param>
        /// <param name="e">The event arguments for the checkbox state change.</param>
        /// <remarks>
        /// This method ensures that <c>checkBox2</c> is unchecked if <c>checkBox3</c> is checked, maintaining a mutual exclusivity between these checkboxes.
        /// </remarks>
        /// @code
        /// private void checkBox2_CheckedChanged(object sender, EventArgs e)
        /// {
        ///     if (checkBox3.Checked)
        ///     {
        ///         checkBox2.Checked = false; // Uncheck checkBox2 if checkBox3 is checked
        ///     }
        /// }
        /// @endcode
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false; // Деактивуємо checkBox2, якщо checkBox1 активовано
            }
        }
        /// <summary>
        /// Handles the change event for <c>checkBox3</c>. If <c>checkBox2</c> is checked, <c>checkBox3</c> is unchecked.
        /// </summary>
        /// <param name="sender">The object that raised the event (the checkbox).</param>
        /// <param name="e">The event arguments for the checkbox state change.</param>
        /// <remarks>
        /// This method ensures that <c>checkBox3</c> is unchecked if <c>checkBox2</c> is checked, maintaining a mutual exclusivity between these checkboxes.
        /// </remarks>
        /// @code
        /// private void checkBox3_CheckedChanged(object sender, EventArgs e)
        /// {
        ///     if (checkBox2.Checked)
        ///     {
        ///         checkBox3.Checked = false; // Uncheck checkBox3 if checkBox2 is checked
        ///     }
        /// }
        /// @endcode
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox3.Checked = false; // Деактивуємо checkBox1, якщо checkBox2 активовано
            }
        }
        /// <summary>
        /// Handles the change event for <c>checkBox1</c>. If the checkbox is checked, it starts playing a sound.
        /// </summary>
        /// <param name="sender">The object that raised the event (the checkbox).</param>
        /// <param name="e">The event arguments for the checkbox state change.</param>
        /// <remarks>
        /// This method starts playing a sound from a specified location if <c>checkBox1</c> is checked. If unchecked, it stops the sound.
        /// </remarks>
        /// @code
        /// private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        /// {
        ///     if (checkBox1.Checked)
        ///     {
        ///         _player = new SoundPlayer();
        ///         _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav"; // Set sound file path
        ///         _player.LoadAsync(); // Asynchronously load the sound
        ///         _player.PlayLooping(); // Play sound in a loop
        ///     }
        ///     else
        ///     {
        ///         _player?.Stop(); // Stop sound if the checkbox is unchecked
        ///     }
        /// }
        /// @endcode
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
