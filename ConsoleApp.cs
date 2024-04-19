using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using WindowsFormsApp;

namespace WinFormsApp
{
    public partial class MainForm : Form
    {
        public object file3 { get; private set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ProcessFiles();
        }

        private void ProcessFiles()
        {
            string inputFileName = "input.txt";
            string outputFileName1 = "output1.txt";
            string outputFileName2 = "output2.txt";

            // Генеруємо випадкові числа для вхідного файлу
            Random rand = new Random();
            using (StreamWriter writer = new StreamWriter(inputFileName))
            {
                for (int i = 0; i < 20; i++)
                {
                    writer.WriteLine(rand.Next(1, 100));
                }
            }

            // Запускаємо перший потік для обробки першого файлу
            Thread thread1 = new Thread(() => ProcessFile(inputFileName, outputFileName1));
            thread1.Start();

            // Запускаємо другий потік для обробки другого файлу
            thread1.Join(); // Чекаємо закінчення першого потоку перед запуском другого
            Thread thread2 = new Thread(() => ProcessFile(outputFileName1, outputFileName2));
            thread2.Start();

            // Запускаємо третій потік для підготовки та виведення звіту
            thread2.Join(); // Чекаємо закінчення другого потоку перед запуском третього
            Thread thread3 = new Thread(() => GenerateReport(inputFileName, outputFileName1, outputFileName2));
            thread3.Start();
            thread3.Join(); // Чекаємо закінчення третього потоку перед завершенням програми
            Application.Exit();
        }

        private void ProcessFile(string inputFileName, string outputFileName)
        {
            // Зчитуємо числа з файлу
            string[] lines = System.IO.File.ReadAllLines(inputFileName);

            // Фільтруємо прості числа
            var primeNumbers = lines.Select(int.Parse).Where(IsPrime).ToList();

            // Записуємо прості числа в вихідний файл
            using (StreamWriter writer = new StreamWriter(outputFileName))
            {
                foreach (var number in primeNumbers)
                {
                    writer.WriteLine(number);
                }
            }
        }

        private void GenerateReport(string inputFileName, string outputFileName1, string outputFileName2)
        {
            using (StreamWriter writer = new StreamWriter("report.txt"))
            {
                writer.WriteLine($"Звіт про обробку файлів {DateTime.Now}");
                writer.WriteLine();

                // Зчитуємо дані про файли
                int file1Count = System.IO.File.ReadAllLines(inputFileName).Length;
                long file1Size = new FileInfo(inputFileName).Length;
                int file2Count = System.IO.File.ReadAllLines(outputFileName1).Length;
                long file2Size = new FileInfo(outputFileName1).Length;
                int file3Count = System.IO.File.ReadAllLines(outputFileName2).Length;
                long file3Size = new FileInfo(outputFileName2).Length;

                // Записуємо дані про файли у звіт
                writer.WriteLine($"Файл: {inputFileName}");
                writer.WriteLine($"Кількість чисел: {file1Count}");
                writer.WriteLine($"Розмір: {file1Size} байт");
                writer.WriteLine($"Вміст:");
                writer.WriteLine(System.IO.File.ReadAllText(inputFileName));
                writer.WriteLine();

                writer.WriteLine($"Файл: {outputFileName1}");
                writer.WriteLine($"Кількість чисел: {file2Count}");
                writer.WriteLine($"Розмір: {file2Size} байт");
                writer.WriteLine($"Вміст:");
                writer.WriteLine(System.IO.File.ReadAllText(outputFileName1));
                writer.WriteLine();

                writer.WriteLine($"Файл: {outputFileName2}");
                writer.WriteLine($"Кількість чисел: {file3Count}");
                writer.WriteLine($"Розмір: {file3Size} байт");
                writer.WriteLine($"Вміст:");
                writer.WriteLine(System.IO.File.ReadAllText(outputFileName2));
                writer.WriteLine();
            }

            MessageBox.Show("Звіт збережено у файлі report.txt", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsPrime(int number)
        {
            if (number <= 1)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
    }
}
