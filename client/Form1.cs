using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading.Tasks;

namespace client
{
    public partial class Form1 : Form
    {
        private bool isMonitoring;
        SerialPort serialPort;
        public Form1()
        {
            InitializeComponent();
            LoadAvailablePorts();
            Exchangerate();
            textBox1.Text = "Hello Arduino";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Метод для завантаження доступних COM-портів у ComboBox
        private void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames(); // Отримуємо список портів
            comboBox1.Items.Clear(); // Очищаємо ComboBox перед додаванням нових елементів

            if (ports.Length > 0)
            {
                // Додаємо кожен COM-порт у ComboBox
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }

                // Автоматично вибираємо перший елемент
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                // Якщо немає доступних портів, показуємо повідомлення
                comboBox1.Items.Add("no ports");
                comboBox1.SelectedIndex = 0;
            }
        }
        private void Exchangerate()
        {
            comboBox2.Items.Add("9600");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedСom = comboBox1.SelectedItem?.ToString();
            string selectedSpeed = comboBox2.SelectedItem?.ToString();
            int speed = Convert.ToInt32(selectedSpeed);
            serialPort = new SerialPort(selectedСom, speed); // Змініть "COMx" на ваш віртуальний COM-порт
            serialPort.Open();
            richTextBox1.AppendText(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " - Connected to Arduino.");
            // Відправка даних
            string line = textBox1.Text;
            serialPort.WriteLine(line);

            // Читання даних
            isMonitoring = true; // Починаємо моніторинг
            StartMonitoring(); // Запускаємо моніторинг в окремому потоці
        }
        private async void StartMonitoring()
        {
            await Task.Run(() =>
            {
                while (isMonitoring)
                {
                    try
                    {
                        // Читання даних з порту
                        string data = serialPort.ReadLine();

                        // Оновлення ListBox з основного потоку UI
                        this.Invoke((MethodInvoker)delegate
                        {
                            richTextBox1.AppendText("\n"+ $"{DateTime.Now:dd.MM.yyyy HH:mm:ss -} {data}");
                        });
                    }
                    catch (TimeoutException) { /* Ігноруємо тайм-аут, якщо дані не прийшли */ }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Зупиняємо моніторинг і закриваємо порт
            isMonitoring = false;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            isMonitoring = false;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
