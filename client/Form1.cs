using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO.Ports;
using System.IO;
/// <summary>
/// Namespace for the game client.
/// This namespace contains the core logic of the application, handling user interface, game mode selection, 
/// and interactions with external hardware (if necessary).
/// </summary>
namespace game_client
{
    /// <summary>
    /// Main form for the game client.
    /// Responsible for initializing the game, updating the interface, and handling user interactions with the application.
    /// </summary>
    public partial class Form1 : Form
    {
        // <summary>
        /// Used to play background music.
        /// </summary>
        public SoundPlayer _player = new SoundPlayer();

        /// <summary>
        /// Stores the first user's selection for the game.
        /// The values can be:
        /// 1 - Rock,
        /// 2 - Paper,
        /// 3 - Scissors.
        /// </summary>
        public int select1User;

        /// <summary>
        /// Stores the second user's selection for the game.
        /// The values can be:
        /// 1 - Rock,
        /// 2 - Paper,
        /// 3 - Scissors.
        /// </summary>
        public int select2User;

        /// <summary>
        /// Strategy for determining the winner. Controls the game behavior.
        /// If true, AI uses a strategic approach to determine the winner; otherwise, it uses random selection.
        /// </summary>
        public bool winStrategy;

        /// <summary>
        /// Determines if the random selection mode is enabled in the game.
        /// If true, the game will randomly select moves for players or AI.
        /// </summary>
        public bool randomMode;

        /// <summary>
        /// Flag indicating whether background music is enabled.
        /// If true, music will play in the background during the game.
        /// </summary>
        public bool musicOn;

        /// <summary>
        /// The current game mode, such as "Man VS Man", "AI VS AI".
        /// </summary>
        public string mode;

        /// <summary>
        /// Flag indicating whether the game is loaded.
        /// Used to track whether the game has been initialized and ready for play.
        /// </summary>
        public bool onLoad;

        /// <summary>
        /// Player 1's score.
        /// This value is incremented based on the outcome of each round.
        /// </summary>
        public int score1;

        /// <summary>
        /// Player 2's score.
        /// This value is incremented based on the outcome of each round.
        /// </summary>
        public int score2;

        /// <summary>
        /// Serial port object for COM port interaction.
        /// Used to communicate with external hardware (e.g., Arduino or other devices).
        /// </summary>
        public SerialPort serialPort = new SerialPort();

        /// <summary>
        /// List to store Player 1's moves.
        /// Each element in the list corresponds to a move made by Player 1 (1 - Rock, 2 - Paper, 3 - Scissors).
        /// </summary>
        public List<int> player1Moves = new List<int>();

        /// <summary>
        /// Timer for delaying the AI's move.
        /// Used to simulate AI thinking time or delay between moves.
        /// </summary>
        public Timer clickTimer;

        /// <summary>
        /// Random number generator for generating random moves or delays.
        /// </summary>
        public Random random = new Random();
        /// <summary>
        /// Constructor for the <see cref="Form1"/> class.
        /// Initializes the main game form, sets up window properties, attaches event handlers for custom painting,
        /// and calls the method to load the start menu.
        /// </summary>
        /// <remarks>
        /// This constructor performs the following key tasks:
        /// 1. Initializes the form components through the <see cref="InitializeComponent"/> method.
        /// 2. Loads initial settings and displays the start menu using the <see cref="StartMenu"/> method.
        /// 3. Ensures the form is static, preventing resizing by the user.
        /// 4. Attaches custom paint event handlers for drawing borders on panels.
        /// </remarks>
        /// <code>
        /// public Form1()
        /// {
        ///     // *** 1. Initialize form components ***
        ///     // The InitializeComponent method creates all the visual elements and adds them to the form.
        ///     InitializeComponent();
        ///
        ///     // *** 2. Load the start menu ***
        ///     // The StartMenu method sets up initial interface settings and displays the start menu.
        ///     StartMenu();
        ///
        ///     // *** 3. Configure window properties ***
        ///     // Sets the form style to be non-resizable.
        ///     this.FormBorderStyle = FormBorderStyle.FixedSingle; // Alternative style: FormBorderStyle.Fixed3D.
        ///     
        ///     // Disables the maximize button.
        ///     this.MaximizeBox = false;
        ///
        ///     // *** 4. Attach event handlers for custom panel border painting ***
        ///     // The DrawCustomBorder method handles painting custom borders on panels.
        ///     // Each call adds this method to the Paint event of the corresponding panel.
        ///     panel13.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel14.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel16.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel8.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel12.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel9.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel2.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel3.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel5.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel7.Paint += new PaintEventHandler(DrawCustomBorder);
        ///     panel17.Paint += new PaintEventHandler(DrawCustomBorder);
        /// }
        /// </code>
        public Form1()
        {
            InitializeComponent();
            StartMenu();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Або FormBorderStyle.Fixed3D
            this.MaximizeBox = false; // Вимкнути кнопку максимізації
            panel13.Paint += new PaintEventHandler(DrawCustomBorder);
            panel14.Paint += new PaintEventHandler(DrawCustomBorder);
            panel16.Paint += new PaintEventHandler(DrawCustomBorder);
            panel8.Paint += new PaintEventHandler(DrawCustomBorder);
            panel12.Paint += new PaintEventHandler(DrawCustomBorder);
            panel9.Paint += new PaintEventHandler(DrawCustomBorder);
            panel2.Paint += new PaintEventHandler(DrawCustomBorder);
            panel1.Paint += new PaintEventHandler(DrawCustomBorder);
            panel3.Paint += new PaintEventHandler(DrawCustomBorder);
            panel5.Paint += new PaintEventHandler(DrawCustomBorder);
            panel7.Paint += new PaintEventHandler(DrawCustomBorder);
            panel17.Paint += new PaintEventHandler(DrawCustomBorder);
        }
        /// <summary>
        /// Handles the click event for the "New Game" button.
        /// Opens the mode selection menu and resets the game loading state.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "New Game" button.</param>
        /// <param name="e">Event arguments associated with the button click.</param>
        /// <remarks>
        /// This method performs the following actions:
        /// 1. Calls the <see cref="ModMenu"/> method to open the mode selection menu.
        /// 2. Resets the <see cref="onLoad"/> flag to ensure the game is not in a loading state.
        /// </remarks>
        /// <code>
        /// public void button1_Click(object sender, EventArgs e)
        /// {
        ///     // Opens the mode selection menu.
        ///     ModMenu();
        ///     
        ///     // Resets the loading state.
        ///     onLoad = false;
        /// }
        /// </code>
        /// <summary>
        /// This method handles the click event for the "New Game" button. It transitions the application to the main game menu and updates the state.
        /// 
        /// The method performs the following actions:
        /// 1. Calls the `ModMenu` method to configure the visibility of UI elements for the main menu.
        /// 2. Updates the `onLoad` flag to indicate the game is no longer in the initial load state.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button1_Click` method:
        /// 
        /// \image html media/doc_img/button1.png
        /// </summary>
        /// <summary>
        /// This method enables navigation from the initial application state to the game menu.
        /// 
        /// Below is a screenshot showing the interface after triggering the "New Game" button:
        /// 
        /// \image html media/doc_img/button1_screen.png
        /// </summary>
        /// <remarks>
        /// Button Click Process:
        /// - Invokes the `ModMenu` method to adjust the visibility of UI elements for the main menu.
        /// - Updates the `onLoad` flag to reflect the transition from the loading phase.
        /// 
        /// Dependencies:
        /// - Requires the `ModMenu` method to correctly configure the UI for the main menu.
        /// - Relies on the `onLoad` flag to track the application's loading state.
        /// </remarks>
        public void button1_Click(object sender, EventArgs e) // кнопка New Game
        {
            ModMenu();
            onLoad = false;
        }
        /// <summary>
        /// Loads the menu for selecting game modes.
        /// Adjusts the visibility of buttons, panels, and labels to display the mode selection interface.
        /// </summary>
        /// <remarks>
        /// This method modifies the visibility of several UI elements to prepare the game mode selection menu.
        /// - Hides elements that are not relevant during mode selection.
        /// - Displays elements required for the user to choose a mode.
        /// 
        /// **Key Actions:**
        /// 1. Hides buttons such as `button1`, `button22`, and `button21`.
        /// 2. Hides unnecessary UI components like `panel17` and `pictureBox15`.
        /// 3. Makes essential components like `label2`, `panel1`, and `panel2` visible.
        /// 4. Activates mode selection buttons (`button2`, `button3`, `button4`).
        /// </remarks>
        /// <code>
        /// public void ModMenu()
        /// {
        ///     // Hide irrelevant buttons and UI components.
        ///     button1.Visible = false;
        ///     button22.Visible = false;
        ///     button21.Visible = false;
        ///     panel17.Visible = false;
        ///     pictureBox15.Visible = false;
        ///
        ///     // Display the mode selection interface.
        ///     label2.Visible = true;
        ///     panel1.Visible = true;
        ///     panel2.Visible = true;
        ///     button2.Visible = true;
        ///     button3.Visible = true;
        ///     button4.Visible = true;
        /// }
        /// </code>
        /// <summary>
        /// This method manages the visibility of various UI elements to configure and display the main menu of the application.
        /// 
        /// The method performs the following actions:
        /// 1. Hides specific buttons, panels, and images that are not needed for the main menu view.
        /// 2. Shows the main menu components, including specific buttons, panels, and labels.
        /// 3. Prepares the user interface for interaction with the main menu options.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `ModMenu` method:
        /// 
        /// \image html media/doc_img/mod_menu.png
        /// </summary>
        /// <summary>
        /// This method ensures the main menu UI elements are correctly displayed and non-relevant elements are hidden.
        /// 
        /// Below is a screenshot showing the main menu interface after applying the `ModMenu` function:
        /// 
        /// \image html media/doc_img/mod_menu_screen.png
        /// </summary>
        /// <remarks>
        /// UI Configuration Process:
        /// - Updates the visibility of specific UI elements such as buttons, panels, and labels to focus on the main menu.
        /// - Ensures irrelevant elements are hidden to avoid user confusion.
        /// 
        /// Dependencies:
        /// - Requires defined visibility properties for buttons, panels, labels, and images.
        /// - Relies on the `ModMenu` method being invoked at the appropriate point in the application lifecycle.
        /// </remarks>
        public void ModMenu()
        {
            button1.Visible = false;
            button22.Visible = false;
            button21.Visible = false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            label2.Visible = true;
            panel1.Visible = true;
            panel2.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;

        }
        /// <summary>
        /// Loads the main menu and initializes settings from the configuration file.
        /// Configures the initial state of the game based on saved settings and manages background music.
        /// </summary>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Reads settings from an INI configuration file (`config.ini`) to determine:
        ///    - Background music state.
        ///    - Game-winning strategy mode.
        ///    - Random mode state.
        /// 2. Initializes the background music if enabled.
        /// 3. Hides unnecessary UI elements to display the main menu.
        /// 4. Handles errors during INI file reading gracefully by showing a message box.
        /// 
        /// **Key Elements:**
        /// - Uses the `IniFile` class to manage INI file reading.
        /// - Handles boolean parsing using `bool.TryParse`.
        /// - Utilizes the `SoundPlayer` class for background music playback.
        /// - Updates UI visibility based on the current state.
        /// </remarks>
        /// <code>
        /// public void StartMenu()
        /// {
        ///     // Specify the path to the INI file.
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     try
        ///     {
        ///         // Read values from the INI file as strings.
        ///         string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
        ///         string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
        ///         string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");
        ///
        ///         // Convert string values to booleans using TryParse.
        ///         bool.TryParse(checkBox1Value, out musicOn);
        ///         bool.TryParse(checkBox2Value, out winStrategy);
        ///         bool.TryParse(checkBox3Value, out randomMode);
        ///
        ///         if (musicOn == true)
        ///         {
        ///             _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
        ///             _player.LoadAsync();
        ///             _player.PlayLooping();
        ///         }
        ///         else
        ///         {
        ///             _player?.Stop();
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        ///
        ///     // Hide unnecessary UI elements to display the main menu.
        ///     label2.Visible = false;
        ///     panel1.Visible = false;
        ///     panel2.Visible = false;
        ///     button2.Visible = false;
        ///     button3.Visible = false;
        ///     button4.Visible = false;
        ///     panel3.Visible = false;
        ///     panel8.Visible = false;
        ///     panel13.Visible = false;
        /// }
        /// </code>
        /// <summary>
        /// This method initializes the application's start menu by reading configuration settings from an INI file and configuring the user interface accordingly.
        /// 
        /// The method performs the following actions:
        /// 1. Reads the configuration values for music, AI strategies, and random modes from the `config.ini` file.
        /// 2. Converts the retrieved string values into boolean flags using `bool.TryParse`.
        /// 3. Configures the background music based on the `musicOn` setting, playing or stopping it as necessary.
        /// 4. Updates the visibility of UI components to prepare the start menu.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `StartMenu` method:
        /// 
        /// \image html media/doc_img/start_menu.png
        /// </summary>
        /// <summary>
        /// This method ensures the application initializes with the correct settings and user interface configuration.
        /// 
        /// Below is a screenshot showing the user interface after the start menu is initialized, highlighting the visibility of UI elements and the status of background music.
        /// 
        /// \image html media/doc_img/start_menu_screen.png
        /// </summary>
        /// <remarks>
        /// Application Initialization Process:
        /// - Reads configuration data from an INI file (`config.ini`) to determine settings like music playback and AI strategies.
        /// - Handles potential errors during file reading gracefully by displaying an error message to the user.
        /// - Configures the user interface by hiding or showing relevant panels and buttons based on the initial state.
        /// 
        /// Dependencies:
        /// - Requires an INI file (`config.ini`) located in the application's base directory.
        /// - Relies on the `IniFile` class for reading configuration values.
        /// - Uses a `SoundPlayer` object (`_player`) to manage background music.
        /// </remarks>
        public void StartMenu()
        {
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {
                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out winStrategy);
                bool.TryParse(checkBox3Value, out randomMode);
                if (musicOn == true)
                {

                    _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                    _player.LoadAsync();
                    _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            label2.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            panel3.Visible = false;
            panel8.Visible = false;
            panel13.Visible = false;
        }
        /// <summary>
        /// Draws a custom border around a panel.
        /// </summary>
        /// <remarks>
        /// This method customizes the appearance of a `Panel` control by drawing a border around it.
        /// The border is styled using a specific color and thickness defined in the method.
        /// 
        /// **Steps Performed:**
        /// 1. Casts the `sender` object to a `Panel`.
        /// 2. Uses the `Graphics` object from the `PaintEventArgs` to draw the border.
        /// 3. Configures a `Pen` with a predefined color (`Color.Aqua`) and width (`3`).
        /// 4. Draws a rectangle that fits within the panel's dimensions, leaving a one-pixel margin.
        /// 
        /// **Example Usage:**
        /// Attach this method to the `Paint` event of any `Panel`:
        /// ```csharp
        /// panel1.Paint += new PaintEventHandler(DrawCustomBorder);
        /// ```
        /// </remarks>
        /// <param name="sender">The source of the event, expected to be a `Panel` object.</param>
        /// <param name="e">Provides data for the `Paint` event, including the `Graphics` object.</param>
        /// <code>
        /// private void DrawCustomBorder(object sender, PaintEventArgs e)
        /// {
        ///     Panel panel = sender as Panel;
        ///     Graphics g = e.Graphics;
        ///
        ///     // Specify the color and thickness of the border pen.
        ///     Pen pen = new Pen(Color.Aqua, 3);
        ///
        ///     // Draw a rectangular border around the panel.
        ///     g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        /// }
        /// </code>
        /// <summary>
        /// This method customizes the appearance of a panel by drawing a border around it during a paint event.
        /// 
        /// The method performs the following actions:
        /// 1. Casts the `sender` object to a `Panel`.
        /// 2. Retrieves the `Graphics` object from the `PaintEventArgs` to enable drawing on the panel.
        /// 3. Creates a `Pen` with a specified color (`Color.Aqua`) and thickness (`3`).
        /// 4. Uses the `Graphics.DrawRectangle` method to draw a border around the panel.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `DrawCustomBorder` method:
        /// 
        /// \image html media/doc_img/draw_custom_border.png
        /// </summary>
        /// <summary>
        /// This method visually enhances panels by adding a custom border, improving the user interface appearance.
        /// 
        /// Below is a screenshot showing the customized panel border rendered during the paint event, 
        /// demonstrating the application of the border style.
        /// 
        /// \image html media/doc_img/draw_custom_border_screen.png
        /// </summary>
        /// <remarks>
        /// Panel Border Customization Process:
        /// - The `sender` object is dynamically cast to a `Panel` to identify the target for border rendering.
        /// - The `Graphics` object is utilized for drawing operations during the paint event.
        /// - A `Pen` is instantiated with specific properties (color and thickness) for consistent border styling.
        /// 
        /// Dependencies:
        /// - Requires a `Panel` object to be the sender of the paint event.
        /// - Uses the `Graphics` class for rendering shapes.
        /// - Depends on a `Pen` object for specifying the appearance of the border.
        /// </remarks>
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
        /// Handles the "Man VS Man" button click event.
        /// </summary>
        /// <remarks>
        /// This method is triggered when the user clicks the "Man VS Man" button.
        /// It sets up the game mode for a two-player experience ("Man VS Man") and initiates Player 1's turn.
        /// 
        /// **Steps Performed:**
        /// 1. Calls the `Player1` method to start the game sequence for Player 1.
        /// 2. Updates the `mode` variable to indicate the current game mode is "Man VS Man".
        /// 
        /// **Example Usage:**
        /// Attach this method to the `Click` event of the "Man VS Man" button:
        /// ```csharp
        /// button3.Click += new EventHandler(button3_Click);
        /// ```
        /// </remarks>
        /// <param name="sender">The source of the event, typically the "Man VS Man" button.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <code>
        /// public void button3_Click(object sender, EventArgs e)
        /// {
        ///     Player1();
        ///     mode = "Man VS Man";
        /// }
        /// </code>
        /// <summary>
        /// This method handles the `Man VS Man` game mode setup when the corresponding button is clicked.
        /// 
        /// The method performs the following actions:
        /// 1. Calls the `Player1` method to initialize the game for Player 1.
        /// 2. Sets the game mode to `"Man VS Man"`.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button3_Click` method:
        /// 
        /// \image html media/doc_img/button3.png
        /// </summary>
        /// <summary>
        /// This method prepares the game for Player 1 in the `Man VS Man` mode. It ensures the UI is updated correctly and initiates Player 1's turn.
        /// 
        /// Below is a screenshot showing the interface during the `Man VS Man` mode setup, highlighting the active player panel and relevant buttons.
        /// 
        /// \image html media/doc_img/button3_screen.png
        /// </summary>
        /// <remarks>
        /// Game Setup Process:
        /// - The game mode is set to `"Man VS Man"` to establish a turn-based interaction between two human players.
        /// - The `Player1` method is invoked to configure the game panel for Player 1's move.
        /// 
        /// Dependencies:
        /// - Relies on the `Player1` method for initializing Player 1's game turn.
        /// - Updates the `mode` field to `"Man VS Man"` for tracking the game type.
        /// </remarks>
        public void button3_Click(object sender, EventArgs e) // кнопка Man Vs Man
        {
            Player1();
            mode = "Man VS Man";
        }
        /// <summary>
        /// Handles the transition and setup for Player 1's turn in the game.
        /// </summary>
        /// <remarks>
        /// This method manages the visibility of the UI panels and sets up specific behavior depending on the game mode.
        /// If the game mode is "AI VS AI", it initializes a timer to simulate a delayed random AI move.
        /// 
        /// **Steps Performed:**
        /// 1. Hides `panel1` and `panel2`, which are likely for Player 1's initial view.
        /// 2. Displays `panel3`, which is likely for Player 1's actual game view.
        /// 3. If the mode is "AI VS AI", it initializes a timer that triggers a random AI move after 1000 ms:
        ///    - A random number (1 to 3) is generated and a corresponding button (button5, button6, or button7) is clicked programmatically.
        /// 
        /// **Example Usage:**
        /// This method can be called when Player 1's turn begins, especially in a scenario like "AI VS AI".
        /// ```csharp
        /// Player1();
        /// ```
        /// </remarks>
        /// <code>
        /// public virtual void Player1()
        /// {
        ///     panel1.Visible = false;
        ///     panel2.Visible = false;
        ///     panel3.Visible = true;
        ///     if (mode == "AI VS AI")
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             // Generate a random number between 1 and 3
        ///             int buttonNumber = random.Next(1, 4); // Generates 1, 2, or 3
        /// 
        ///             // Simulate a button press based on the random number
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button5.PerformClick(); // Simulate click on button5
        ///                     break;
        ///                 case 2:
        ///                     button6.PerformClick(); // Simulate click on button6
        ///                     break;
        ///                 case 3:
        ///                     button7.PerformClick(); // Simulate click on button7
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// }
        /// </code>
        /// <summary>
        /// This method manages the visibility of game panels and initializes the AI's decision-making process in the `AI VS AI` mode.
        /// 
        /// The method performs the following actions:
        /// 1. Hides panels (`panel1` and `panel2`) and displays the game panel (`panel3`).
        /// 2. Checks the current game mode to determine if AI-controlled gameplay is required.
        /// 3. Initializes a timer that simulates AI decisions by randomly selecting and "pressing" a button after a set interval.
        /// 4. Executes the corresponding game actions for the AI's decision.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `Player1` method:
        /// 
        /// \image html media/doc_img/Player1.png
        /// </summary>
        /// <summary>
        /// This method enables AI-driven gameplay when the mode is set to `AI VS AI`. It dynamically updates the UI and automates AI button presses.
        /// 
        /// Below is a screenshot showing the interface during `AI VS AI` mode, including the active game panel and the visual representation of the AI's moves.
        /// 
        /// \image html media/doc_img/Player1_screen.png
        /// </summary>
        /// <remarks>
        /// AI Control Process:
        /// - The visibility of UI components is adjusted based on the game state.
        /// - In `AI VS AI` mode, a timer triggers automated button presses to simulate the AI's decision-making process.
        /// - Randomness is incorporated by selecting a button to "press" from a range of predefined options.
        /// 
        /// Dependencies:
        /// - Requires game mode to be set to `AI VS AI`.
        /// - Utilizes a timer (`clickTimer`) for simulating delays in AI decision-making.
        /// - Relies on `random` for generating unpredictable AI actions.
        /// </remarks>
        public virtual void Player1()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            if (mode == "AI VS AI")
            {
                // Ініціалізуємо таймер
                clickTimer = new Timer();
                clickTimer.Interval = 1000; // Затримка 500 мс
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop(); // Зупиняємо таймер

                    // Генеруємо випадкове число 1, 2 або 3
                    int buttonNumber = random.Next(1, 4); // 1, 2 або 3

                    // Вибираємо кнопку для натискання
                    switch (buttonNumber)
                    {
                        case 1:
                            button5.PerformClick();
                            break;
                        case 2:
                            button6.PerformClick();
                            break;
                        case 3:
                            button7.PerformClick();
                            break;
                    }
                };
                clickTimer.Start(); // Запускаємо таймер
            }

        }
        /// <summary>
        /// Handles the transition and setup for Player 2's turn in the game.
        /// </summary>
        /// <remarks>
        /// This method manages the behavior of Player 2's turn depending on the game mode:
        /// - If the game mode is "Man VS AI" or "AI VS AI" with random mode enabled, it simulates a random AI move after a 1-second delay.
        /// - If the game mode is "Man VS AI" or "AI VS AI" with a strategy mode enabled, it either calculates a strategic counter-move (if sufficient data is available) or defaults to a random move.
        ///
        /// **Steps Performed:**
        /// 1. Displays `panel8`, which is likely representing Player 2's game view.
        /// 2. Initializes a timer that triggers after 1 second to simulate Player 2's choice:
        ///    - If the mode is `"AI VS AI"` or `"Man VS AI"` with random mode, it generates a random choice for Player 2 (Rock, Paper, or Scissors).
        ///    - If the mode is `"AI VS AI"` or `"Man VS AI"` with strategy mode enabled, it calculates a counter move based on Player 1's previous moves (if sufficient data is available) or defaults to a random move.
        /// 
        /// **Example Usage:**
        /// This method can be called when Player 2's turn begins, especially in a scenario like "Man VS AI" or "AI VS AI".
        /// ```csharp
        /// Player2();
        /// ```
        /// </remarks>
        /// <code>
        /// public void Player2()
        /// {
        ///     panel8.Visible = true; // Display Player 2's game panel
        /// 
        ///     // If the mode is "Man VS AI" or "AI VS AI" with random mode, simulate a random choice
        ///     if (mode == "Man VS AI" || mode == "AI VS AI" && randomMode == true)
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             // Generate a random number between 1 and 3
        ///             int buttonNumber = random.Next(1, 4); // Generates 1 (Rock), 2 (Paper), or 3 (Scissors)
        /// 
        ///             // Simulate a button press based on the random number
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button11.PerformClick(); // Simulate click on button for Rock
        ///                     break;
        ///                 case 2:
        ///                     button12.PerformClick(); // Simulate click on button for Paper
        ///                     break;
        ///                 case 3:
        ///                     button13.PerformClick(); // Simulate click on button for Scissors
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// 
        ///     // If the mode is "Man VS AI" or "AI VS AI" with strategy mode, calculate counter move or random choice
        ///     else if ((mode == "Man VS AI" || mode == "AI VS AI") && winStrategy == true)
        ///     {
        ///         clickTimer = new Timer();
        ///         clickTimer.Interval = 1000; // Set timer interval to 1 second (1000 ms)
        ///         clickTimer.Tick += (s, e) =>
        ///         {
        ///             clickTimer.Stop(); // Stop the timer after it ticks
        /// 
        ///             int buttonNumber;
        /// 
        ///             // If there is enough data, calculate a counter move
        ///             if (winStrategy == true && player1Moves.Count >= 5)
        ///             {
        ///                 buttonNumber = GetCounterMove(); // Get counter move based on Player 1's previous moves
        ///             }
        ///             else
        ///             {
        ///                 // Default to a random choice if not enough data is available
        ///                 buttonNumber = random.Next(1, 4);
        ///             }
        /// 
        ///             // Simulate a button press based on the calculated or random move
        ///             switch (buttonNumber)
        ///             {
        ///                 case 1:
        ///                     button11.PerformClick(); // Simulate click on button for Rock
        ///                     break;
        ///                 case 2:
        ///                     button12.PerformClick(); // Simulate click on button for Paper
        ///                     break;
        ///                 case 3:
        ///                     button13.PerformClick(); // Simulate click on button for Scissors
        ///                     break;
        ///             }
        ///         };
        ///         clickTimer.Start(); // Start the timer
        ///     }
        /// }
        /// </code>
        /// <summary>
        /// This method manages the actions and decision-making process for Player 2 in different game modes.
        /// 
        /// The method performs the following actions:
        /// 1. Displays the game panel for Player 2 (`panel8`).
        /// 2. Checks the game mode and AI strategy to determine Player 2's move.
        /// 3. If the mode includes AI (`Man VS AI` or `AI VS AI`) with random mode enabled, initializes a timer to simulate a random AI move.
        /// 4. If the AI strategy is enabled (`winStrategy`), calculates Player 2's counter move based on Player 1's move history and executes it.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `Player2` method:
        /// 
        /// \image html media/doc_img/player2.png
        /// </summary>
        /// <summary>
        /// This method handles Player 2's actions, including AI-driven decision-making and UI updates, in various game modes.
        /// 
        /// Below is a screenshot showing the interface during Player 2's turn, demonstrating the AI's decision-making process in action.
        /// 
        /// \image html media/doc_img/player2_screen.png
        /// </summary>
        /// <remarks>
        /// Player 2 Control Process:
        /// - Adjusts the visibility of `panel8` to indicate Player 2's turn.
        /// - Simulates Player 2's move based on the game mode and the AI strategy:
        ///   - Random mode generates moves without considering Player 1's history.
        ///   - Strategic mode calculates a counter move using Player 1's recent moves (`player1Moves`) when sufficient data is available.
        /// - Timer (`clickTimer`) ensures a delay for simulating real-time decision-making.
        /// 
        /// Dependencies:
        /// - Relies on the `mode` variable to determine the current game configuration.
        /// - Utilizes `random` for random move generation and `GetCounterMove` for strategic move calculation.
        /// - Requires Player 1's move history (`player1Moves`) for the strategic AI mode.
        /// </remarks>
        public void Player2()
        {
            panel8.Visible = true;
            if (mode == "Man VS AI" || mode == "AI VS AI" && randomMode == true)
            {
                // Initialize a timer for AI's random choice
                clickTimer = new Timer();
                clickTimer.Interval = 1000; // 1-second delay
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop(); 

                    // Generate a random choice (1: Rock, 2: Paper, 3: Scissors)
                    int buttonNumber = random.Next(1, 4);

                    // Simulate button clicks
                    switch (buttonNumber)
                    {
                        case 1:
                            button11.PerformClick();
                            break;
                        case 2:
                            button12.PerformClick();
                            break;
                        case 3:
                            button13.PerformClick();
                            break;
                    }
                };
                clickTimer.Start(); 
            }
            // Initialize a timer for AI's strategic choice
            else if ((mode == "Man VS AI" || mode == "AI VS AI") && winStrategy == true)
            {
                clickTimer = new Timer();
                clickTimer.Interval = 1000;
                clickTimer.Tick += (s, e) =>
                {
                    clickTimer.Stop();

                    int buttonNumber;

                    // If enough data is available, calculate a counter move
                    if (winStrategy == true && player1Moves.Count >= 5)
                    {
                        buttonNumber = GetCounterMove();
                    }
                    else
                    {
                        // Default to a random choice if data is insufficient
                        buttonNumber = random.Next(1, 4);
                    }

                    switch (buttonNumber)
                    {
                        case 1:
                            button11.PerformClick(); // Paper
                            break;
                        case 2:
                            button12.PerformClick(); // Scissors
                            break;
                        case 3:
                            button13.PerformClick(); // Rock
                            break;
                    }
                };
                clickTimer.Start();
            }
        }
        /// <summary>
        /// Calculates the counter move based on Player 1's previous moves.
        /// Analyzes the last 5 moves and predicts the most likely move to counter it.
        /// </summary>
        /// <returns>
        /// The counter move as an integer:
        /// 1 - Rock,
        /// 2 - Paper,
        /// 3 - Scissors.
        /// </returns>
        /// 
        /// <remarks>
        /// This method calculates the counter move for Player 2 by analyzing the last 5 moves of Player 1.
        /// Each move is assigned a weighted score, with more recent moves receiving higher weights.
        /// The function also introduces a small chance of randomness to make the AI less predictable.
        /// Based on the most likely move of Player 1, the AI will choose a counter move:
        /// - Rock is countered by Paper (2),
        /// - Paper is countered by Scissors (3),
        /// - Scissors is countered by Rock (1).
        /// If the randomness condition is met, the AI will choose a move randomly.
        /// </remarks>
        /// 
        /// @code
        /// public int GetCounterMove()
        /// {
        ///     // Frequency weights for each move
        ///     Dictionary<int, double> moveWeights = new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 } };
        /// 
        ///     // Assign weights to Player 1's moves
        ///     double weight = 1.0;
        ///     double weightIncrement = 0.15; // Increment for weighting moves
        ///     foreach (int move in player1Moves)
        ///     {
        ///         moveWeights[move] += weight;
        ///         weight += weightIncrement;
        ///     }
        /// 
        ///     // Determine the most likely move
        ///     int mostLikelyMove = moveWeights.OrderByDescending(m => m.Value).First().Key;
        /// 
        ///     // Introduce randomness in AI's choice
        ///     double randomness = 0.15; // 15% chance of random move
        ///     if (random.NextDouble() < randomness)
        ///     {
        ///         return random.Next(1, 4); 
        ///     }
        /// 
        ///     // Return the counter move
        ///     switch (mostLikelyMove)
        ///     {
        ///         case 1: // Rock -> Paper
        ///             return 1; 
        ///         case 2: // Paper -> Scissors
        ///             return 2; 
        ///         case 3: // Scissors -> Rock
        ///             return 3; 
        ///         default:
        ///             return random.Next(1, 4); // Default random move
        ///     }
        /// }
        /// @endcode
        /// <summary>
        /// This method calculates the AI's counter move based on Player 1's move history.
        /// 
        /// The method performs the following actions:
        /// 1. Initializes and assigns frequency weights for Player 1's previous moves.
        /// 2. Determines the most likely move Player 1 will make next based on these weights.
        /// 3. Introduces a small chance of randomness (15%) to make the AI less predictable.
        /// 4. Returns the counter move based on the most likely move or a random choice if randomness is triggered.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `GetCounterMove` method:
        /// 
        /// \image html media/doc_img/get_counter_move.png
        /// </summary>
        /// <summary>
        /// This method ensures the AI calculates an effective counter move based on weighted predictions and randomness.
        /// 
        /// Below is a screenshot showing the AI's decision-making process visualized for debugging purposes, including the move weights, 
        /// the most likely move, and the final counter move.
        /// 
        /// \image html media/doc_img/get_counter_move_screen.png
        /// </summary>
        /// <remarks>
        /// AI Decision Process:
        /// - Frequency weights are calculated for Player 1's moves (`player1Moves`) based on recency and an incremental weight factor.
        /// - A 15% chance of randomness is included to make the AI's decisions less predictable.
        /// - The counter move is determined based on the most likely move Player 1 will make next.
        ///
        /// Dependencies:
        /// - Requires a `player1Moves` list to track Player 1's previous moves.
        /// - Utilizes a random number generator for introducing randomness in the AI's decision.
        /// </remarks>
        public int GetCounterMove()
        {
            // Frequency weights for each move
            Dictionary<int, double> moveWeights = new Dictionary<int, double> { { 1, 0 }, { 2, 0 }, { 3, 0 } };

            // Assign weights to Player 1's moves
            double weight = 1.0;
            double weightIncrement = 0.15; // Increment for weighting moves
            foreach (int move in player1Moves)
            {
                moveWeights[move] += weight;
                weight += weightIncrement;
            }

            // Determine the most likely move
            int mostLikelyMove = moveWeights.OrderByDescending(m => m.Value).First().Key;

            // Introduce randomness in AI's choice
            double randomness = 0.15; // 15% chance of random move
            if (random.NextDouble() < randomness)
            {
                return random.Next(1, 4); 
            }

            // Return the counter move
            switch (mostLikelyMove)
            {
                case 1: // Rock -> Paper
                    return 1; 
                case 2: // Paper -> Scissors
                    return 2; 
                case 3: // Scissors -> Rock
                    return 3; 
                default:
                    return random.Next(1, 4); // Default random move
            }
        }
        /// <summary>
        /// Handles the "Rock" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Rock" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Rock" (represented by the value 1).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button5_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 1; // Set Player 1's move to "Rock"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks the "Rock" button for Player 1.
        /// 
        /// The method performs the following actions:
        /// 1. Calls the `Player2` method to handle Player 2's turn.
        /// 2. Sets `select1User` to 1, representing "Rock" as the choice for Player 1.
        /// 3. Tracks Player 1's choice by calling the `TrackPlayer1Move` method, 
        ///    ensuring the player's move history is updated and maintained.
        ///
        /// Below is a sequence diagram illustrating the flow of the `button5_Click` method:
        /// 
        /// \image html media/doc_img/button5.png
        /// </summary>
        /// <summary>
        /// This method allows Player 1 to select "Rock" and updates their move history using the `TrackPlayer1Move` method.
        /// 
        /// Below is a screenshot showing the UI after the button is clicked, highlighting Player 1's choice 
        /// and any relevant updates to the game state displayed in the interface.
        /// The image also demonstrates how the move history is maintained for Player 1.
        /// 
        /// \image html media/doc_img/button5_screen.png
        /// </summary>
        /// <remarks>
        /// Button Click Action:
        /// - Calls the `Player2` method to handle Player 2's turn.
        /// - Player 1's choice is directly set to "Rock" (`select1User = 1`).
        /// - Updates Player 1's move history using the `TrackPlayer1Move` method.
        /// 
        /// Dependencies:
        /// - This method relies on the `Player2` function for processing Player 2's actions.
        /// - Utilizes the `TrackPlayer1Move` method to ensure Player 1's move history is tracked and maintained.
        /// </remarks>
        public void button5_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 1; // Rock
            TrackPlayer1Move(select1User);
        }
        /// <summary>
        /// Handles the "Paper" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Paper" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Paper" (represented by the value 2).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button7_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 2; // Set Player 1's move to "Paper"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks the "Paper" button for Player 1.
        /// 
        /// The method performs the following actions:
        /// 1. Calls the `Player2` method to handle Player 2's turn.
        /// 2. Sets `select1User` to 2, representing "Paper" as the choice for Player 1.
        /// 3. Tracks Player 1's choice by calling the `TrackPlayer1Move` method, 
        ///    ensuring the player's move history is updated and maintained.
        ///
        /// Below is a sequence diagram illustrating the flow of the `button7_Click` method:
        /// 
        /// \image html media/doc_img/button7.png
        /// </summary>
        /// <summary>
        /// This method allows Player 1 to select "Paper" and updates their move history using the `TrackPlayer1Move` method.
        /// 
        /// Below is a screenshot showing the UI after the button is clicked, highlighting Player 1's choice 
        /// and any relevant updates to the game state displayed in the interface.
        /// The image also demonstrates how the move history is maintained for Player 1.
        /// 
        /// \image html media/doc_img/button7_screen.png
        /// </summary>
        /// <remarks>
        /// Button Click Action:
        /// - Calls the `Player2` method to handle Player 2's turn.
        /// - Player 1's choice is directly set to "Paper" (`select1User = 2`).
        /// - Updates Player 1's move history using the `TrackPlayer1Move` method.
        /// 
        /// Dependencies:
        /// - This method relies on the `Player2` function for processing Player 2's actions.
        /// - Utilizes the `TrackPlayer1Move` method to ensure Player 1's move history is tracked and maintained.
        /// </remarks>
        public void button7_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 2; // Paper
            TrackPlayer1Move(select1User);
        }
        /// <summary>
        /// Handles the "Scissors" button click event for Player 1.
        /// Tracks the move and initiates Player 2's turn.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Scissors" button is clicked, the following actions are performed:
        /// 1. Player 1's move is set to "Scissors" (represented by the value 3).
        /// 2. The <c>TrackPlayer1Move()</c> method is called to record the move in the list of Player 1's recent moves.
        /// 3. The <c>Player2()</c> method is invoked to initiate Player 2's turn, where Player 2 will choose their move.
        /// </remarks>
        /// 
        /// @code
        /// public void button6_Click(object sender, EventArgs e)
        /// {
        ///     Player2(); // Initiate Player 2's turn
        ///     select1User = 3; // Set Player 1's move to "Scissors"
        ///     TrackPlayer1Move(select1User); // Track Player 1's move
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks the "Scissors" button for Player 1.
        /// 
        /// The method performs the following actions:
        /// 1. Calls the `Player2` method to handle Player 2's turn.
        /// 2. Sets `select1User` to 3, representing "Scissors" as the choice for Player 1.
        /// 3. Tracks Player 1's choice by calling the `TrackPlayer1Move` method, 
        ///    ensuring the player's move history is updated and maintained.
        ///
        /// Below is a sequence diagram illustrating the flow of the `button6_Click` method:
        /// 
        /// \image html media/doc_img/button6.png
        /// </summary>
        /// <summary>
        /// This method allows Player 1 to select "Scissors" and updates their move history using the `TrackPlayer1Move` method.
        /// 
        /// Below is a screenshot showing the UI after the button is clicked, highlighting Player 1's choice 
        /// and any relevant updates to the game state displayed in the interface.
        /// The image also demonstrates how the move history is maintained for Player 1.
        /// 
        /// \image html media/doc_img/button6_screen.png
        /// </summary>
        /// <remarks>
        /// Button Click Action:
        /// - Calls the `Player2` method to handle Player 2's turn.
        /// - Player 1's choice is directly set to "Scissors" (`select1User = 3`).
        /// - Updates Player 1's move history using the `TrackPlayer1Move` method.
        /// 
        /// Dependencies:
        /// - This method relies on the `Player2` function for processing Player 2's actions.
        /// - Utilizes the `TrackPlayer1Move` method to ensure Player 1's move history is tracked and maintained.
        /// </remarks>
        public void button6_Click(object sender, EventArgs e)
        {
            Player2();
            select1User = 3; // Scissors
            TrackPlayer1Move(select1User);
        }
        /// <summary>
        /// Tracks the last 5 moves made by Player 1.
        /// Keeps a rolling list of the most recent moves.
        /// </summary>
        /// <param name="move">The move made by Player 1 (1: Rock, 2: Paper, 3: Scissors).</param>
        /// 
        /// <remarks>
        /// This function performs the following tasks:
        /// 1. Adds the new move to the <c>player1Moves</c> list.
        /// 2. If the list exceeds 5 moves, the oldest move is removed to maintain a maximum of 5 recent moves.
        /// This allows the game to track Player 1's most recent moves and potentially use that information for AI decision-making.
        /// </remarks>
        /// 
        /// @code
        /// public void TrackPlayer1Move(int move)
        /// {
        ///     // Add the new move to the list
        ///     player1Moves.Add(move);
        ///     // Remove the oldest move if the list exceeds 5 moves
        ///     if (player1Moves.Count > 5)
        ///     {
        ///         player1Moves.RemoveAt(0); // Remove the first (oldest) move
        ///     }
        /// }
        /// @endcode
        /// <summary>
        /// This method is responsible for tracking the moves of Player 1 in the game.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `TrackPlayer1Move` method.
        /// It shows how a new move is added to the `player1Moves` list. If the list exceeds the 
        /// maximum size of 5, the oldest move is removed to maintain a fixed history.
        /// 
        /// \image html media/doc_img/track_player1_move.png
        /// </summary>
        public void TrackPlayer1Move(int move)
        {
            // Maintain a maximum size of 5 moves
            player1Moves.Add(move); 
            if (player1Moves.Count > 5)
            {
                player1Moves.RemoveAt(0); 
            }
        }
        /// <summary>
        /// Handles the "Rock" button click event for Player 2.
        /// Sets the move for Player 2 as "Rock" and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event (button).</param>
        /// <param name="e">Event data for the click event.</param>
        /// 
        /// <remarks>
        /// When the "Rock" button is clicked, the following actions are performed:
        /// 1. The move for Player 2 is set to "Rock" (represented by the value 4).
        /// 2. The <c>serverControl()</c> method is called to send Player 2's move along with Player 1's move to the server for further processing.
        /// </remarks>
        /// 
        /// @code
        /// public void button13_Click(object sender, EventArgs e)
        /// {
        ///     // Player 2 chooses "Rock"
        ///     select2User = 4; // Set Player 2's choice to "Rock"
        ///     serverControl(); // Send the player choices to the server and handle the response
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks the "Rock" button for Player 2.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button13_Click` method. 
        /// It highlights the direct assignment of Player 2's choice (`Rock`), followed by a call to the `serverControl` function. 
        /// The diagram demonstrates how the `serverControl` method handles the communication with the server to process and retrieve the game results.
        /// 
        /// \image html media/doc_img/button13.png
        /// </summary>
        /// <summary>
        /// This method allows Player 2 to select "Rock" and initiates the game result processing via the `serverControl` method.
        /// 
        /// Below is a screenshot showing the UI after the button is clicked, including the indication of Player 2's choice 
        /// and the initiation of the server communication process.
        /// The image also shows the expected interface updates once the results are processed.
        /// 
        /// \image html media/doc_img/button13_screen.png
        /// </summary>
        /// <remarks>
        /// Button Click Action:
        /// - Player 2's choice is directly set to "Rock" (`select2User = 4`).
        /// - Calls the `serverControl` method to process and handle game data.
        /// - The UI dynamically updates based on the server's response or error handling mechanisms.
        ///
        /// Dependencies:
        /// - This method relies on the `serverControl` function for server communication and game result processing.
        /// </remarks>
        public void button13_Click(object sender, EventArgs e)
        {
            select2User = 4; // Player 2 chooses "Rock"
            serverControl(); // Send player choices to the server and handle the response
        }
        /// <summary>
        /// Manages the communication with the server via the serial port.
        /// This method handles the process of sending user choices to the server, receiving responses, and processing those responses.
        /// It retrieves the port name from a configuration file (`config.ini`), opens the serial port, and exchanges data with the server.
        /// If there is an error (such as a timeout or serial port issue), it displays an error message and redirects the user to the main menu.
        /// </summary>
        /// <remarks>
        /// **Steps Performed:**
        /// 1. Reads the serial port name from the configuration file (`config.ini`).
        /// 2. Tries to open the serial port with the specified port name and sets the read timeout to 3 seconds.
        /// 3. Sends a message containing the selected user choices to the server.
        /// 4. Attempts to read the server's responses. If successful, it processes those responses.
        /// 5. If an error occurs (e.g., a timeout), it catches the exception and displays an error message. The user is redirected to the main menu.
        /// 
        /// **Error Handling:**
        /// - A `TimeoutException` is caught if no response is received from the server within the timeout period.
        /// - A general exception is caught if there is an issue opening the serial port or sending/receiving data.
        /// </remarks>
        /// <code>
        /// public void serverControl()
        /// {
        ///     // Initialize the configuration file for reading the port name
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Read port name from config file
        /// 
        ///     try
        ///     {
        ///         // Open the serial port with the specified port name and 9600 baud rate
        ///         using (SerialPort serialPort = new SerialPort(portName, 9600))
        ///         {
        ///             serialPort.ReadTimeout = 3000; // Set 3-second timeout for reading data
        ///             serialPort.Open(); // Open the serial port
        /// 
        ///             // Send user choices to the server
        ///             string message = select1User.ToString() + " " + select2User.ToString();
        ///             serialPort.WriteLine(message); // Send the message
        /// 
        ///             try
        ///             {
        ///                 // Receive responses from the server
        ///                 string response = serialPort.ReadLine(); // First response
        ///                 string counter1 = serialPort.ReadLine(); // Second response
        ///                 string counter2 = serialPort.ReadLine(); // Third response
        /// 
        ///                 // Process the responses
        ///                 FinalAction(response, counter1, counter2);
        ///             }
        ///             catch (TimeoutException)
        ///             {
        ///                 // Handle timeout if no response from the server
        ///                 MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///                 StartMenu(); // Redirect to main menu
        ///                 button1.Visible = true;
        ///                 button21.Visible = true;
        ///                 button22.Visible = true;
        ///                 panel17.Visible = true;
        ///                 pictureBox15.Visible = true;
        ///                 return;
        ///             }
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle general errors (e.g., issues opening the serial port)
        ///         MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///         StartMenu(); // Redirect to main menu
        ///         button1.Visible = true;
        ///         button21.Visible = true;
        ///         button22.Visible = true;
        ///         panel17.Visible = true;
        ///         pictureBox15.Visible = true;
        ///         return;
        ///     }
        /// }
        /// </code>
        /// <summary>
        /// This method manages the communication between the client and the server over a serial port connection.
        /// 
        /// Below is a sequence diagram that illustrates the flow of the `serverControl` method. 
        /// It demonstrates how the method interacts with the configuration file (`IniFile`), the `SerialPort` for data transmission, 
        /// and the UI components like the `MessageBox` and various buttons and panels in case of errors. 
        /// The diagram also highlights the process of sending player choices to the server and receiving the game's results for further processing.
        /// 
        /// \image html media/doc_img/server_control.png
        /// </summary>
        /// <summary>
        /// This method establishes communication with the server and processes game data using the `serverControl` function.
        /// 
        /// Below is a screenshot showing the user interface during the server control operation. 
        /// The image captures the interaction between the application and the server, illustrating the steps for sending the player's choices 
        /// and receiving the game's results. Additionally, it includes the error handling interface displayed when the server fails to respond.
        /// 
        /// \image html media/doc_img/server_control_screen.png
        /// </summary>
        public void serverControl()
        {
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", "");
            try
            {
                using (SerialPort serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // 3-second timeout
                    serialPort.Open();

                    // Send choices to the server
                    string message = select1User.ToString() + " " + select2User.ToString();
                    serialPort.WriteLine(message);

                    try
                    {
                        // Receive responses from the server
                        string response = serialPort.ReadLine();
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        FinalAction(response, counter1, counter2);
                    }
                    catch (TimeoutException)
                    {
                        MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StartMenu(); // Перенаправлення на головне меню
                        button1.Visible = true;
                        button21.Visible = true;
                        button22.Visible = true;
                        panel17.Visible = true;
                        pictureBox15.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartMenu(); // Перенаправлення на головне меню
                button1.Visible = true;
                button21.Visible = true;
                button22.Visible = true;
                panel17.Visible = true;
                pictureBox15.Visible = true;
                return;
            }
        }
        /// <summary>
        /// This method processes the results of the game between Player 1 and Player 2 based on their selections. 
        /// It compares the response from the server with predefined results for ties and wins and displays the corresponding outcome.
        /// The method also updates the game score in the UI if needed.
        /// </summary>
        /// <param name="response">The response received from the server that contains the outcome of the game.</param>
        /// <param name="counter1">The server's response related to Player 1's score.</param>
        /// <param name="counter2">The server's response related to Player 2's score.</param>
        /// <remarks>
        /// **Steps Performed:**
        /// 1. The method compares the `response` with predefined strings to determine if it's a tie or if one player wins.
        /// 2. Based on the outcome, it displays images corresponding to the players' choices in two `PictureBox` controls.
        /// 3. It also displays the result message ("It's a tie!" or "Player X win!") in a `Label` control.
        /// 4. Updates the scores for Player 1 and Player 2 in `textBox1` and `textBox2` respectively.
        /// </remarks>
        /// <code>
        /// public void FinalAction(string response, string counter1, string counter2)
        /// {
        ///     // Define the tie and win conditions
        ///     string tieRock1 = "It's a tie. Player1 and Player2 select rock";
        ///     string tieRock2 = "It's a tie. Player1 and Player2 select paper";
        ///     string tieRock3 = "It's a tie. Player1 and Player2 select scissors";
        /// 
        ///     string WinPlayer1f = "Player1 Win!. Player1 select rock and Player2 select scissors";
        ///     string WinPlayer1s = "Player1 Win!. Player1 select scissors and Player2 select paper";
        ///     string WinPlayer1t = "Player1 Win!. Player1 select paper and Player2 select rock";
        /// 
        ///     string WinPlayer2f = "Player2 Win!. Player1 select scissors and Player2 select rock";
        ///     string WinPlayer2s = "Player2 Win!. Player1 select paper and Player2 select scissors";
        ///     string WinPlayer2t = "Player2 Win!. Player1 select rock and Player2 select paper";
        /// 
        ///     // Define image paths
        ///     string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
        ///     string imagePath = Path.Combine(projectRoot, "img", "rock.png");
        ///     string imagePath1 = Path.Combine(projectRoot, "img", "paper.png");
        ///     string imagePath2 = Path.Combine(projectRoot, "img", "scissors.png");
        /// 
        ///     // Check for tie scenarios
        ///     if (tieRock1 == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     else if (tieRock2 == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     else if (tieRock3 == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label16.Visible = false;
        ///         label9.Visible = true;
        ///         label9.Text = "It's a tie!";
        ///         panel13.Visible = true;
        ///     }
        ///     // Check for Player 1 win scenarios
        ///     else if (WinPlayer1f == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer1s == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer1t == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 1 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     // Check for Player 2 win scenarios
        ///     else if (WinPlayer2f == response.Trim())
        ///     {
        ///         LoadImage(imagePath2, pictureBox13);
        ///         LoadImage(imagePath, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer2s == response.Trim())
        ///     {
        ///         LoadImage(imagePath1, pictureBox13);
        ///         LoadImage(imagePath2, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        ///     else if (WinPlayer2t == response.Trim())
        ///     {
        ///         LoadImage(imagePath, pictureBox13);
        ///         LoadImage(imagePath1, pictureBox14);
        ///         label9.Visible = false;
        ///         label16.Visible = true;
        ///         label16.Text = "Player 2 win!";
        ///         label9.TextAlign = ContentAlignment.MiddleCenter;
        ///         panel13.Visible = true;
        ///     }
        /// 
        ///     // Update scores
        ///     if (onLoad == false)
        ///     {
        ///         textBox1.Text = counter1.Substring(14).ToString();
        ///         textBox2.Text = counter2.Substring(14).ToString();
        ///     }
        ///     else
        ///     {
        ///         int line1 = int.Parse(counter1.Substring(14));
        ///         int line2 = int.Parse(counter2.Substring(14));
        ///         textBox1.Text = (line1 + score1).ToString();
        ///         textBox2.Text = (line2 + score2).ToString();
        ///     }
        /// }
        /// </code>
        /// <summary>
        /// This method performs a crucial operation. 
        /// 
        /// Below is a sequence diagram that illustrates the flow of the `FinalAction` method, which handles the results of a game round based on players' choices. 
        /// It shows the interaction between the `Form`, `PictureBox`, `Label`, and `TextBox` components, 
        /// along with how the method updates the game status and the player scores.
        /// 
        /// \image html media/doc_img/final_action.png
        /// </summary>
        /// <summary>
        /// This method displays the result of the game after the `FinalAction` function is executed.
        /// 
        /// Below is a screenshot showing the user interface during the final action, including the updated images of the players' choices, the result label (tie or win), and the updated score fields.
        /// This provides a visual representation of the outcome after the `FinalAction` method is called.
        /// 
        /// \image html media/doc_img/final_action_screen.png
        /// </summary>
        public void FinalAction(string response, string counter1, string counter2)
        {
            // нічийні варіанті

            string tieRock1 = "It's a tie. Player1 and Player2 select rock";
            string tieRock2 = "It's a tie. Player1 and Player2 select paper";
            string tieRock3 = "It's a tie. Player1 and Player2 select scissors";

            // перемога першого гравця програш другого 
            string WinPlayer1f = "Player1 Win!. Player1 select rock and Player2 select scissors";
            string WinPlayer1s = "Player1 Win!. Player1 select scissors and Player2 select paper";
            string WinPlayer1t = "Player1 Win!. Player1 select paper and Player2 select rock";

            // перемога другого гравця програш другого 
            string WinPlayer2f = "Player2 Win!. Player1 select scissors and Player2 select rock";
            string WinPlayer2s = "Player2 Win!. Player1 select paper and Player2 select scissors";
            string WinPlayer2t = "Player2 Win!. Player1 select rock and Player2 select paper";

            // Отримати кореневу директорію проекту
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            // Побудувати шляхи до зображень відносно кореня проекту
            string imagePath = Path.Combine(projectRoot, "img", "rock.png");
            string imagePath1 = Path.Combine(projectRoot, "img", "paper.png");
            string imagePath2 = Path.Combine(projectRoot, "img", "scissors.png");


            // нічия якщо два гравці вибрали камінь
            if (tieRock1 == response.Trim())
            {
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали папір
            else if (tieRock2 == response.Trim())
            {
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // нічия якщо два гравці вибрали ножниці
            else if (tieRock3 == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label16.Visible = false;
                label9.Visible = true;
                label9.Text = "It's a tie!";
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим каменю, вибір другим ножниць)
            else if (WinPlayer1f == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим ножниці, вибір другим папір)
            else if (WinPlayer1s == response.Trim())
            {
                // Вказати шлях до файлу зображення
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога першого гравця програш другого (Вибір першим папір, вибір другим камінь)
            else if (WinPlayer1t == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 1 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим ножниць, вибір другим каменю)
            else if (WinPlayer2f == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath2, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим папір , вибір другим ножниці)
            else if (WinPlayer2s == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath1, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath2, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            // перемога другого гравця програш першого (Вибір першим камінь, вибір другим папір)
            else if (WinPlayer2t == response.Trim())
            {
                // Завантажити зображення у PictureBox
                LoadImage(imagePath, pictureBox13);  // Завантаження для першого PictureBox
                LoadImage(imagePath1, pictureBox14);  // Завантаження для другого PictureBox
                label9.Visible = false;
                label16.Visible = true;
                label16.Text = "Player 2 win!";
                label9.TextAlign = ContentAlignment.MiddleCenter;
                panel13.Visible = true;
            }
            if (onLoad == false)
            {
                textBox1.Text = counter1.Substring(14).ToString();
                textBox2.Text = counter2.Substring(14).ToString();
            }
            else
            {
                int line1 = int.Parse(counter1.Substring(14));
                int line2 = int.Parse(counter2.Substring(14));
                textBox1.Text = (line1 + score1).ToString();
                textBox2.Text = (line2 + score2).ToString();
            }
        }
        /// <summary>
        /// Loads an image into a specified PictureBox from a given file path.
        /// </summary>
        /// <remarks>
        /// This function checks if the PictureBox already contains an image. 
        /// If it does, the previous image's resources are released to avoid memory leaks.
        /// Then, the new image is loaded from the specified file path and displayed in the PictureBox.
        /// </remarks>
        /// <param name="imagePath">The file path to the image that needs to be loaded.</param>
        /// <param name="pictureBox">The PictureBox where the image will be displayed.</param>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Thrown if the specified file does not exist.
        /// </exception>
        /// @code
        /// private void LoadImage(string imagePath, PictureBox pictureBox)
        /// {
        ///     // Check if the PictureBox already has an image
        ///     if (pictureBox.Image != null)
        ///     {
        ///         // Release resources of the existing image
        ///         pictureBox.Image.Dispose();
        ///     }
        /// 
        ///     // Load and display the new image
        ///     pictureBox.Image = Image.FromFile(imagePath);
        /// }
        /// @endcode
        /// <summary>
        /// This method loads an image into a `PictureBox` control.
        /// 
        /// The method performs the following actions:
        /// 1. Checks if the `PictureBox` already has an image. If it does, the existing image is disposed of to free up resources.
        /// 2. Loads a new image from the specified file path and assigns it to the `PictureBox`.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `LoadImage` method:
        /// 
        /// \image html media/doc_img/load_image.png
        /// </summary>
        /// <summary>
        /// This method updates the image displayed in a `PictureBox` by first disposing of any existing image and then loading a new one from the given path.
        /// 
        /// Below is a screenshot showing the interface with the image loaded into the `PictureBox` after calling the `LoadImage` method.
        /// 
        /// \image html media/doc_img/load_image_screen.png
        /// </summary>
        /// <remarks>
        /// Image Loading Process:
        /// - The method first checks if the `PictureBox` already contains an image. If it does, the previous image is disposed of to prevent memory leaks.
        /// - The image is then loaded from the file path provided and displayed in the `PictureBox`.
        /// 
        /// Dependencies:
        /// - Requires the `imagePath` to be a valid path to an image file.
        /// - Relies on the `PictureBox` to display the image.
        /// </remarks>
        public void LoadImage(string imagePath, PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = Image.FromFile(imagePath);
        }
        /// <summary>
        /// Handles the `FormClosing` event for the main form.
        /// Ensures proper cleanup of resources by closing the serial port if it is open.
        /// </summary>
        /// <param name="sender">The source of the event, typically the form being closed.</param>
        /// <param name="e">Provides data for the `FormClosing` event, including the ability to cancel the closure.</param>
        /// <remarks>
        /// This method prevents potential resource leaks by checking if the serial port is open and closing it 
        /// before the application exits.
        /// </remarks>
        /// @code
        /// public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        /// {
        ///     // Close the serial port if it is open
        ///     if (serialPort.IsOpen)
        ///     {
        ///         serialPort.Close();
        ///     }
        /// }
        /// @endcode
        /// <summary>
        /// This method handles the form closing event and ensures that the serial port is properly closed before the application exits.
        /// 
        /// The method performs the following actions:
        /// 1. Checks if the serial port is open.
        /// 2. If the serial port is open, it closes the serial port to release resources and avoid any potential communication issues when the application is restarted.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `MainForm_FormClosing` method:
        /// 
        /// \image html media/doc_img/form_closing.png
        /// </summary>
        /// <summary>
        /// This method ensures that the serial port is closed when the form is closing, preventing any communication issues upon reopening.
        /// 
        /// Below is a screenshot showing the application with the serial port closed upon form closing.
        /// 
        /// \image html media/doc_img/form_closing_screen.png
        /// </summary>
        /// <remarks>
        /// Port Closing Process:
        /// - The method checks if the serial port is open by calling `serialPort.IsOpen`.
        /// - If the port is open, the `serialPort.Close()` method is called to close the connection before the application exits.
        /// 
        /// Dependencies:
        /// - Requires a `serialPort` object to be initialized and accessible.
        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            base.OnFormClosing(e);
        }
        /// <summary>
        /// Handles the click event for the button corresponding to the "Paper" choice for User 2.
        /// Sets the `select2User` variable to indicate the "Paper" option and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button being clicked.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <remarks>
        /// This method is triggered when the user selects the "Paper" option. 
        /// It assigns a value of `5` to the `select2User` variable, which represents "Paper," 
        /// and calls the `serverControl` method to process the choice and communicate with the server.
        /// </remarks>
        /// @code
        /// private void button11_Click(object sender, EventArgs e)
        /// {
        ///     select2User = 5; // Set the choice for User 2 to "Paper"
        ///     serverControl(); // Call serverControl to handle communication
        /// }
        /// @endcode
        /// <summary>
        /// This method handles the click event for button11 and triggers the server control function when clicked.
        /// 
        /// The method performs the following actions:
        /// 1. Sets the `select2User` variable to 5, representing a specific user selection.
        /// 2. Calls the `serverControl()` method to perform any associated actions related to the selected user.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button11_Click` method:
        /// 
        /// \image html media/doc_img/button11.png
        /// </summary>
        /// <summary>
        /// This method updates the user selection and calls the server control function, enabling the game or system to proceed with the selected option.
        /// 
        /// Below is a screenshot showing the interface after the button is clicked, including any updated selection states.
        /// 
        /// \image html media/doc_img/button11_screen.png
        /// </summary>
        /// <remarks>
        /// Action Flow:
        /// - The method sets the `select2User` variable to 5, which may correspond to a particular user or action.
        /// - The `serverControl()` method is invoked to handle the action triggered by this selection.
        /// 
        /// Dependencies:
        /// - Requires the `select2User` variable to be defined and accessible.
        /// - Requires the `serverControl()` method to be implemented for handling further actions.
        public void button11_Click(object sender, EventArgs e)
        {
            select2User = 5;
            serverControl();
        }
        /// <summary>
        /// Handles the click event for the button corresponding to the "Scissors" choice for User 2.
        /// Sets the `select2User` variable to indicate the "Scissors" option and initiates server communication.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button being clicked.</param>
        /// <param name="e">Provides data for the `Click` event.</param>
        /// <remarks>
        /// This method is triggered when the user selects the "Scissors" option. 
        /// It assigns a value of `6` to the `select2User` variable, which represents "Scissors," 
        /// and calls the `serverControl` method to process the choice and communicate with the server.
        /// </remarks>
        /// @code
        /// private void button12_Click(object sender, EventArgs e)
        /// {
        ///     select2User = 6; // Set the choice for User 2 to "Scissors"
        ///     serverControl(); // Call serverControl to handle communication
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on button12, which sets Player 2's move to "Scissors" (represented by the value 6).
        /// It then calls the `serverControl` method to handle the communication with the server and process the game logic.
        /// 
        /// The following actions occur:
        /// 1. Sets the `select2User` variable to 6, representing the "Scissors" move for Player 2.
        /// 2. Calls the `serverControl` method to send the player's move to the server and process the result.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button12_Click` method:
        /// 
        /// \image html media/doc_img/button12.png
        /// </summary>
        /// <summary>
        /// This method triggers the communication flow when Player 2 chooses "Scissors".
        /// It updates the game state and sends the data to the server for further processing.
        /// 
        /// Below is a screenshot showing the interface during Player 2's move selection.
        /// 
        /// \image html media/doc_img/button12_screen.png
        /// </summary>
        /// <remarks>
        /// - Sets `select2User` to 6, indicating Player 2's choice of "Scissors".
        /// - Triggers the `serverControl` method to process the player's move.
        /// 
        /// Dependencies:
        /// - Requires the `serverControl` method to process the game logic after the move is made.
        /// - Requires an established connection with the server to handle the communication.
        /// </remarks>
        public void button12_Click(object sender, EventArgs e)
        {
            select2User = 6;
            serverControl();
        }
        /// <summary>
        /// Event handler for the "Play Again" button. This function hides the game result panels and starts a new game for Player 1 and Player 2.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game result panels, <c>panel13</c> and <c>panel8</c>, are hidden.
        /// 2. The <c>Player1()</c> function is called to start a new game for Player 1 and Player 2.
        /// </remarks>
        /// 
        /// @code
        /// private void button17_Click_1(object sender, EventArgs e)
        /// {
        ///     // Hide the game result panels
        ///     panel13.Visible = false;
        ///     panel8.Visible = false;
        ///
        ///     // Start a new game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on button17. It hides specific panels (`panel13` and `panel8`) 
        /// and calls the `Player1()` method to continue the game logic for Player 1's turn.
        /// 
        /// The following actions occur:
        /// 1. Hides `panel13` and `panel8` to adjust the game interface for the next phase.
        /// 2. Calls the `Player1()` method to proceed with Player 1's actions, likely including AI or manual interactions.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button17_Click_1` method:
        /// 
        /// \image html media/doc_img/button17.png
        /// </summary>
        /// <summary>
        /// This method manages the visibility of UI elements and triggers Player 1's turn to continue the gameplay.
        /// It hides unnecessary panels and initiates the logic for Player 1's actions.
        /// 
        /// Below is a screenshot showing the interface after button17 is clicked and Player 1's turn begins.
        /// 
        /// \image html media/doc_img/button17_screen.png
        /// </summary>
        /// <remarks>
        /// - Hides `panel13` and `panel8` to adjust the UI for the next phase of the game.
        /// - Calls `Player1()` to proceed with Player 1's turn, whether controlled by AI or the user.
        /// 
        /// Dependencies:
        /// - Relies on `Player1()` method to manage the game logic for Player 1's actions.
        /// </remarks>
        public void button17_Click_1(object sender, EventArgs e)
        {
            panel13.Visible = false;
            panel8.Visible = false;
            Player1();
        }
        /// <summary>
        /// Event handler for the button that redirects to the main menu. This function makes several UI elements visible, including buttons and panels, to display the main menu.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The <c>StartMenu()</c> function is called to navigate to the main menu.
        /// 2. Several UI elements are made visible to display the main menu, including:
        ///    - <c>button1</c>
        ///    - <c>button21</c>
        ///    - <c>button22</c>
        ///    - <c>panel17</c>
        ///    - <c>pictureBox15</c>
        /// </remarks>
        /// 
        /// @code
        /// private void button19_Click(object sender, EventArgs e)
        /// {
        ///     // Navigate to the main menu
        ///     StartMenu();
        ///
        ///     // Make UI elements visible for the main menu
        ///     button1.Visible = true;
        ///     button21.Visible = true;
        ///     button22.Visible = true;
        ///     panel17.Visible = true;
        ///     pictureBox15.Visible = true;
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on button19. It calls the `StartMenu()` method to initialize 
        /// the start menu and sets the visibility of various UI elements to prepare the interface for user interaction.
        /// 
        /// The following actions occur:
        /// 1. Calls the `StartMenu()` method to reset or initialize the start menu.
        /// 2. Makes `button1`, `button21`, `button22`, `panel17`, and `pictureBox15` visible to prepare for the next steps in the game.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button19_Click` method:
        /// 
        /// \image html media/doc_img/button19.png
        /// </summary>
        /// <summary>
        /// This method manages the visibility of UI elements and initializes the game interface. 
        /// It calls the `StartMenu()` method and sets specific buttons and panels to be visible.
        /// 
        /// Below is a screenshot showing the interface after button19 is clicked and the game menu is prepared.
        /// 
        /// \image html media/doc_img/button19_screen.png
        /// </summary>
        /// <remarks>
        /// - Calls `StartMenu()` to initialize or reset the start menu.
        /// - Makes `button1`, `button21`, `button22`, `panel17`, and `pictureBox15` visible to prepare for further interactions.
        /// 
        /// Dependencies:
        /// - Requires the `StartMenu()` method to be implemented for resetting or initializing the start menu.
        /// </remarks>
        public void button19_Click(object sender, EventArgs e)
        {
            StartMenu();
            button1.Visible = true;
            button21.Visible = true;
            button22.Visible = true;
            panel17.Visible = true;
            pictureBox15.Visible = true;
        }
        /// <summary>
        /// Resets the score by communicating with the serial port. This function reads the score from the connected device and updates the UI with the received values.
        /// </summary>
        /// <remarks>
        /// The function performs the following steps:
        /// 1. Reads the serial port name from a configuration file (config.ini).
        /// 2. Opens a serial port with the specified port name and sends a reset command ("0") to the connected device.
        /// 3. Reads the response from the serial port, extracting the score values.
        /// 4. If successful, the score values are displayed in <c>textBox1</c> and <c>textBox2</c>.
        /// 5. If an error occurs (e.g., timeout or other serial port issues), an appropriate error message is shown.
        /// </remarks>
        /// 
        /// @code
        /// private void ResetScore()
        /// {
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Default value: empty string
        ///     try
        ///     {
        ///         using (serialPort = new SerialPort(portName, 9600))
        ///         {
        ///             serialPort.ReadTimeout = 3000; // Set read timeout to 3 seconds
        ///             serialPort.Open(); // Open the serial port
        ///
        ///             serialPort.WriteLine("0"); // Send reset command
        ///
        ///             try
        ///             {
        ///                 // Read responses from the port
        ///                 string counter1 = serialPort.ReadLine();
        ///                 string counter2 = serialPort.ReadLine();
        ///
        ///                 // Process the received data
        ///                 textBox1.Text = counter1.Substring(14);
        ///                 textBox2.Text = counter2.Substring(14);
        ///             }
        ///             catch (TimeoutException)
        ///             {
        ///                 // Handle timeout error if the port doesn't respond
        ///                 MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///             }
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle other exceptions related to the serial port
        ///         MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        /// }
        /// @endcode
        /// <summary>
        /// This method resets the game scores by interacting with a serial port. It retrieves the scores from the 
        /// connected device through the serial port and updates the UI accordingly. The method performs the following actions:
        /// 1. Reads the serial port configuration from an INI file (specifically the port name).
        /// 2. Opens the serial port connection and sends a reset command ("0") to the connected device.
        /// 3. Reads the updated scores from the serial port and updates the corresponding UI elements (`textBox1` and `textBox2`).
        /// 4. Handles any errors that may occur during communication with the serial port, such as timeouts or other exceptions.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `ResetScore` method:
        /// 
        /// \image html media/doc_img/reset_score.png
        /// </summary>
        /// <summary>
        /// This method is responsible for resetting the scores by sending a reset command to the connected serial port 
        /// and reading the updated values back from the device. The UI is then updated with the new scores.
        /// 
        /// Below is a screenshot showing the interface after the scores have been reset.
        /// 
        /// \image html media/doc_img/reset_score_screen.png
        /// </summary>
        /// <remarks>
        /// - Communicates with a serial port to reset scores and retrieve updated values.
        /// - Handles timeouts when the port does not respond within the specified time.
        /// - Updates the UI elements `textBox1` and `textBox2` with the new score data received from the device.
        /// 
        /// Dependencies:
        /// - Requires the serial port to be properly configured in the INI file (`TextBox1` value).
        /// - Relies on the device connected to the serial port to process the reset command and return the scores.
        /// </remarks>
        public void ResetScore()
        {
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string portName = ini.Read("TextBoxValues", "TextBox1", ""); // Значення за замовчуванням: порожнє
            try
            {
                using (serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.ReadTimeout = 3000; // Встановлюємо таймаут читання у 3 секунди
                    serialPort.Open(); // Відкриваємо порт

                    serialPort.WriteLine("0"); // Відправляємо команду для скидання

                    try
                    {
                        // Читаємо відповіді від порту
                        string counter1 = serialPort.ReadLine();
                        string counter2 = serialPort.ReadLine();

                        // Обробка отриманих даних
                        textBox1.Text = counter1.Substring(14);
                        textBox2.Text = counter2.Substring(14);
                    }
                    catch (TimeoutException)
                    {
                        // Якщо порт не відповідає протягом часу таймауту, показуємо повідомлення і повертаємося до головного меню
                        MessageBox.Show("The serial port is inactive or not responding.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка інших помилок під час роботи з портом
                MessageBox.Show("Error: " + ex.Message, "Serial Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        /// <summary>
        /// Event handler for the button that triggers the score reset. This function calls the <c>ResetScore()</c> method to reset the score.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the <c>ResetScore()</c> method is called, which communicates with the serial port to reset the score and update the UI accordingly.
        /// </remarks>
        /// 
        /// @code
        /// private void button20_Click_1(object sender, EventArgs e)
        /// {
        ///     // Call ResetScore method to reset the score
        ///     ResetScore();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button20`. It initiates the process of resetting the game scores by 
        /// calling the `ResetScore` method, which interacts with a serial port to send a reset command and retrieve updated scores.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button20_Click` method:
        /// 
        /// \image html media/doc_img/button20.png
        /// </summary>
        /// <summary>
        /// The `button20_Click` method simplifies the user interaction by delegating the task of resetting the scores to the 
        /// `ResetScore` method. It ensures the game UI reflects the latest scores after the reset operation.
        /// 
        /// Below is a screenshot showing the interface after the reset operation has been triggered.
        /// 
        /// \image html media/doc_img/button20_screen.png
        /// </summary>
        /// <remarks>
        /// - Calls `ResetScore` to handle the actual resetting and updating of game scores.
        /// - The UI elements `textBox1` and `textBox2` will display the new scores after the reset.
        /// - Handles any exceptions raised in the `ResetScore` method to ensure application stability.
        /// </remarks>
        public void button20_Click_1(object sender, EventArgs e)
        {
            ResetScore();
        }
        /// <summary>
        /// Event handler for the button that starts a "Man vs AI" game mode. This function sets the game mode to "Man VS AI" and initiates the game by calling the <c>Player1()</c> method.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game mode is set to "Man VS AI" by assigning the string "Man VS AI" to the <c>mode</c> variable.
        /// 2. The <c>Player1()</c> method is called to start the game for Player 1 in the selected mode.
        /// </remarks>
        /// 
        /// @code
        /// private void button2_Click(object sender, EventArgs e)
        /// {
        ///     // Set the game mode to "Man VS AI"
        ///     mode = "Man VS AI";
        ///
        ///     // Start the game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button2`. It sets the game mode to "Man VS AI" and initiates 
        /// Player 1's turn by calling the `Player1` method.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button2_Click` method:
        /// 
        /// \image html media/doc_img/button2.png
        /// </summary>
        /// <summary>
        /// The `button2_Click` method simplifies the user interaction for starting a "Man VS AI" game by configuring the 
        /// game mode and delegating the turn logic to the `Player1` method. This ensures that the game proceeds smoothly 
        /// for Player 1's turn.
        /// 
        /// Below is a screenshot showing the interface after the "Man VS AI" game mode has been selected.
        /// 
        /// \image html media/doc_img/button2_screen.png
        /// </summary>
        /// <remarks>
        /// - Sets the `mode` variable to "Man VS AI" to define the game type.
        /// - Calls the `Player1` method to manage the gameplay for Player 1's turn.
        /// - Ensures a seamless transition between user selection and game logic execution.
        /// 
        /// Dependencies:
        /// - Relies on the `Player1` method to handle the actions and decisions for Player 1.
        /// - Assumes the UI is properly updated based on the game mode and turn logic.
        /// </remarks>
        public void button2_Click(object sender, EventArgs e)
        {
            mode = "Man VS AI";
            Player1();
        }
        /// <summary>
        /// Event handler for the button that starts an "AI vs AI" game mode. This function sets the game mode to "AI VS AI" and initiates the game by calling the <c>Player1()</c> method.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The game mode is set to "AI VS AI" by assigning the string "AI VS AI" to the <c>mode</c> variable.
        /// 2. The <c>Player1()</c> method is called to start the game for Player 1 in the selected mode, which involves AI-controlled gameplay.
        /// </remarks>
        /// 
        /// @code
        /// private void button4_Click(object sender, EventArgs e)
        /// {
        ///     // Set the game mode to "AI VS AI"
        ///     mode = "AI VS AI";
        ///
        ///     // Start the game for Player 1
        ///     Player1();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button4`. It sets the game mode to "AI VS AI" and initiates 
        /// Player 1's turn by calling the `Player1` method, which automates the gameplay for both players.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button4_Click` method:
        /// 
        /// \image html media/doc_img/button4.png
        /// </summary>
        /// <summary>
        /// The `button4_Click` method simplifies the user interaction for starting an "AI VS AI" game by setting the game 
        /// mode and initiating the automated gameplay process. This method ensures the game operates autonomously for both players.
        /// 
        /// Below is a screenshot showing the interface after the "AI VS AI" game mode has been selected.
        /// 
        /// \image html media/doc_img/button4_screen.png
        /// </summary>
        /// <remarks>
        /// - Sets the `mode` variable to "AI VS AI" to define the game type.
        /// - Calls the `Player1` method to automate Player 1's actions and begin the gameplay sequence.
        /// - Automates the gameplay process for both players, making it entirely computer-controlled.
        /// 
        /// Dependencies:
        /// - Relies on the `Player1` method to handle AI-driven gameplay logic.
        /// - Assumes the UI is correctly updated to reflect the "AI VS AI" game mode.
        /// </remarks>
        public void button4_Click(object sender, EventArgs e)
        {
            mode = "AI VS AI";
            Player1();
        }
        /// <summary>
        /// Event handler for the "Exit" button. This function closes the application when the button is clicked.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the <c>Application.Exit()</c> method is called, which terminates the application and closes all windows.
        /// </remarks>
        /// 
        /// @code
        /// private void button22_Click(object sender, EventArgs e)
        /// {
        ///     // Exit the application
        ///     Application.Exit();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button22`. It terminates the application by calling the `Application.Exit()` method.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button22_Click` method:
        /// 
        /// \image html media/doc_img/button22.png
        /// </summary>
        /// <summary>
        /// The `button22_Click` method ensures a clean exit from the application when the exit button is clicked. 
        /// It performs no additional cleanup or confirmation prompts before termination.
        /// 
        /// Below is a screenshot showing the interface before the application exits:
        /// 
        /// \image html media/doc_img/button22_screen.png
        /// </summary>
        /// <remarks>
        /// - Calls `Application.Exit()` to terminate the application.
        /// - Does not include any additional cleanup logic or user confirmation steps.
        /// - It is advisable to confirm unsaved data or prompt the user before exiting in future implementations.
        /// </remarks>
        public void button22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Event handler for the button that opens the game settings form. This function opens the <c>SettingsForm</c> as a modal window and updates game settings based on the values from a configuration file.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The <c>SettingsForm</c> is opened as a modal window, allowing the user to adjust the game settings.
        /// 2. After the settings form is closed, the function reads values from the <c>config.ini</c> file to configure various game settings, such as:
        ///    - <c>CheckBox1</c>: Whether music is enabled.
        ///    - <c>CheckBox2</c>: The player's win strategy.
        ///    - <c>CheckBox3</c>: Whether the game is in random mode.
        /// 3. If music is enabled, the sound file is played; otherwise, the music stops.
        /// </remarks>
        /// 
        /// @code
        /// private void button23_Click(object sender, EventArgs e)
        /// {
        ///     // Open the SettingsForm as a modal window
        ///     SettingsForm settingsForm = new SettingsForm();
        ///     settingsForm.ShowDialog(); // Show the settings form as a dialog (modal)
        ///
        ///     // Specify the path to the ini file
        ///     IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
        ///     try
        ///     {
        ///         // Read values from the ini file as strings
        ///         string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
        ///         string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
        ///         string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");
        ///
        ///         // Convert the string values to boolean using TryParse
        ///         bool.TryParse(checkBox1Value, out musicOn);
        ///         bool.TryParse(checkBox2Value, out winStrategy);
        ///         bool.TryParse(checkBox3Value, out randomMode);
        ///
        ///         // If music is enabled, play the sound; otherwise, stop the music
        ///         if (musicOn == true)
        ///         {
        ///             _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
        ///             _player.LoadAsync();
        ///             _player.PlayLooping();
        ///         }
        ///         else
        ///         {
        ///             _player?.Stop();
        ///         }
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///         // Handle any errors while reading from the ini file
        ///         MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ///     }
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button23`. It opens the settings form as a modal dialog, reads configuration values from an INI file, 
        /// and adjusts the application settings (like music and game strategy) based on those values.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button23_Click` method:
        /// 
        /// \image html media/doc_img/button23.png
        /// </summary>
        /// <summary>
        /// The `button23_Click` method handles the user interaction for opening the settings form. It reads configuration values from the INI file and adjusts settings 
        /// like music preferences and game strategy based on the values retrieved. The settings form is shown as a modal dialog to allow the user to make changes.
        /// 
        /// Below is a screenshot showing the interface before the settings form is opened:
        /// 
        /// \image html media/doc_img/button23_screen.png
        /// </summary>
        /// <remarks>
        /// - Opens the `SettingsForm` as a modal dialog.
        /// - Reads values for `CheckBox1`, `CheckBox2`, and `CheckBox3` from the INI file to set up application settings.
        /// - Adjusts the music playback based on the value of `CheckBox1`.
        /// - Provides an error message if there is an issue with reading the INI file.
        /// </remarks>
        public void button23_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog(); // Відкриваємо як модальне вікно
            // Вказуємо шлях до ini файлу
            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            try
            {
                // Читаємо значення з ini файлу як рядки
                string checkBox1Value = ini.Read("CheckboxStates", "CheckBox1", "");
                string checkBox2Value = ini.Read("CheckboxStates", "CheckBox2", "");
                string checkBox3Value = ini.Read("CheckboxStates", "CheckBox3", "");

                // Перетворюємо рядкові значення у bool за допомогою TryParse
                bool.TryParse(checkBox1Value, out musicOn);
                bool.TryParse(checkBox2Value, out winStrategy);
                bool.TryParse(checkBox3Value, out randomMode);
                if (musicOn == true)
                {
                    _player.SoundLocation = @"C:\Users\Дмитро\Downloads\music.wav";
                    _player.LoadAsync();
                    _player.PlayLooping();
                }
                else
                {
                    _player?.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading INI file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Event handler for the "Save Game" button. This function formats the current game score and opens a <c>SaveMenu</c> to allow the user to save the game.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. The current score from <c>textBox1</c> and <c>textBox2</c> is formatted into a string in the format "Player1Score : Player2Score".
        /// 2. A new <c>SaveMenu</c> form is instantiated with the current game mode and score.
        /// 3. The <c>SaveMenu</c> form is displayed to the user, allowing them to save the game.
        /// </remarks>
        /// 
        /// @code
        /// public void button18_Click(object sender, EventArgs e)
        /// {
        ///     // Format the current game score
        ///     string mainScore = textBox1.Text.Replace("\n", "").Replace("\r", "") + " : " +
        ///            textBox2.Text.Replace("\n", "").Replace("\r", "");
        ///
        ///     // Create and show the SaveMenu form with the game mode and score
        ///     SaveMenu saveMenu = new SaveMenu(mode, mainScore);
        ///     saveMenu.Show();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button18`. It retrieves the current score from `textBox1` and `textBox2`, 
        /// formats the score into a string, and then opens the `SaveMenu` form, passing the game mode and the formatted score as parameters.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button18_Click` method:
        /// 
        /// \image html media/doc_img/button18.png
        /// </summary>
        /// <summary>
        /// The `button18_Click` method formats the current game score by concatenating the values from `textBox1` and `textBox2` (removing newline characters), 
        /// and then passes this formatted score along with the game mode to the `SaveMenu` form to allow the user to save the current game progress.
        /// 
        /// Below is a screenshot showing the interface before the user clicks `button18`:
        /// 
        /// \image html media/doc_img/button18_screen.png
        /// </summary>
        /// <remarks>
        /// - Retrieves the current score from `textBox1` and `textBox2`, removing any newline characters.
        /// - Creates a new `SaveMenu` form, passing the game mode (`mode`) and the formatted score string as parameters.
        /// - Opens the `SaveMenu` form for the user to save the current game progress.
        /// </remarks>
        public void button18_Click(object sender, EventArgs e)
        {
            string mainScore = textBox1.Text.Replace("\n", "").Replace("\r", "") + " : " +
                   textBox2.Text.Replace("\n", "").Replace("\r", "");

            SaveMenu saveMenu = new SaveMenu(mode, mainScore);
            saveMenu.Show();
        }
        /// <summary>
        /// Event handler for the "Load Game" button. This function opens the <c>LoadForm</c> to allow the user to load a previously saved game.
        /// </summary>
        /// <param name="sender">The object that triggered the event (the button clicked by the user).</param>
        /// <param name="e">An event argument containing the data related to the event that occurred when the button was clicked.</param>
        /// 
        /// <remarks>
        /// When the button is clicked, the following actions are performed:
        /// 1. A new <c>LoadForm</c> is instantiated, passing the current instance of the form to it.
        /// 2. The <c>LoadForm</c> is shown to the user, allowing them to select and load a saved game.
        /// </remarks>
        /// 
        /// @code
        /// private void button21_Click(object sender, EventArgs e)
        /// {
        ///     // Create and show the LoadForm with the current form instance passed to it
        ///     LoadForm loadForm = new LoadForm(this);
        ///     loadForm.Show();
        /// }
        /// @endcode
        /// <summary>
        /// This method is triggered when the user clicks on `button21`. It opens the `LoadForm`, passing the current form instance (`this`) as a parameter.
        /// 
        /// Below is a sequence diagram illustrating the flow of the `button21_Click` method:
        /// 
        /// \image html media/doc_img/button21.png
        /// </summary>
        /// <summary>
        /// The `button21_Click` method opens the `LoadForm` dialog, allowing the user to load a previously saved game or configuration.
        /// It passes the current form (`this`) as a parameter to the `LoadForm` to potentially access data or methods from the current form.
        /// 
        /// Below is a screenshot showing the interface before the user clicks `button21`:
        /// 
        /// \image html media/doc_img/button21_screen.png
        /// </summary>
        /// <remarks>
        /// - Opens the `LoadForm` when the user clicks `button21`.
        /// - Passes the current form instance (`this`) to the `LoadForm` constructor, allowing it to interact with the existing form.
        /// - The user can use this form to load a saved game or configuration based on the passed parameters.
        /// </remarks>
        public void button21_Click(object sender, EventArgs e)
        {
            LoadForm loadForm = new LoadForm(this);
            loadForm.Show();
        }
        /// <summary>
        /// Sets the game data by processing the game mode and score, and prepares the game for loading the saved state.
        /// </summary>
        /// <param name="gameMode">The mode of the game (e.g., "Man VS AI", "AI VS AI").</param>
        /// <param name="gameScore">A string representing the game score in the format "Player1Score : Player2Score".</param>
        /// 
        /// <remarks>
        /// This function performs the following tasks:
        /// 1. It splits the provided game score string into two parts, representing the scores of Player 1 and Player 2.
        /// 2. The game mode is stored in the <c>mode</c> variable, and various UI elements are hidden.
        /// 3. If the score format is valid (i.e., contains exactly two parts), the scores are parsed into integers and used for updating the game state.
        /// 4. The <c>ResetScore()</c> function is called to reset the score display, and the <c>Player1()</c> function is called to start the game.
        /// 5. If the score format is invalid, an error message is shown to the user.
        /// </remarks>
        /// 
        /// @code
        /// public void SetGameData(string gameMode, string gameScore)
        /// {
        ///     // Split the score string into two parts
        ///     string[] scores = gameScore.Split(':');
        ///     mode = gameMode;
        ///     button21.Visible = false;
        ///     button22.Visible = false;
        ///     panel17.Visible = false;
        ///     pictureBox15.Visible = false;
        ///     if (scores.Length == 2) // Ensure there are two values
        ///     {
        ///         score1 = int.Parse(scores[0].Trim()); // First score as integer
        ///         score2 = int.Parse(scores[1].Trim()); // Second score as integer
        ///         onLoad = true;
        ///     }
        ///     else
        ///     {
        ///         MessageBox.Show("Invalid score format", "Error");
        ///     }
        ///     ResetScore();
        ///     Player1();
        /// }
        /// @endcode
        public void SetGameData(string gameMode, string gameScore)
        {
            // Розділяємо рядок рахунку на дві частини
            string[] scores = gameScore.Split(':');
            mode = gameMode;
            button21.Visible = false;
            button22.Visible = false;
            panel17.Visible = false;
            pictureBox15.Visible = false;
            if (scores.Length == 2) // Переконуємось, що є два значення
            {
                score1 = int.Parse(scores[0].Trim()); // Перше значення рахунку як int
                score2 = int.Parse(scores[1].Trim()); // Друге значення рахунку як int
                onLoad = true;
            }
            else
            {
                MessageBox.Show("Invalid score format", "Error");
            }
            ResetScore();
            Player1();
        }
    }
}

