using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;

namespace ReneWitlox
{
    class Program
    {
        const string InputComportMessage = "Please input a comport: ";
        const int DELAY_TIMEOUT = 5000;

        static ArduinoSerial arduinoSerial = new ArduinoSerial();
        static Random random = new Random();

        static string[] mp3Files;
        static Task beepTask;

        static void Main(string[] args)
        {
            Console.WriteLine(InputComportMessage);

            int comPort = 0;
            while (!int.TryParse(Console.ReadLine(), out comPort))
                Console.WriteLine(InputComportMessage);

            Console.WriteLine("Connecting to COM port: " + comPort);
            arduinoSerial.DataReceived += ArduinoSerial_DataReceived;
            arduinoSerial.Connect(comPort);

            mp3Files = ScanMp3Files();
            Console.ReadLine();

            arduinoSerial.Disconnect();
        }

        private static void PlayFile()
        {
            var randomIndex = random.Next(mp3Files.Length);

            var soundPlayer = new SoundPlayer();
            soundPlayer.SoundLocation = mp3Files[randomIndex];
            soundPlayer.Play();
        }

        private static string[] ScanMp3Files()
        {
            Console.WriteLine("Scanning MP3 files in directory:");
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine(assemblyPath);

            var path = Path.GetDirectoryName(assemblyPath);
            var files = Directory.GetFiles(path, "*.wav");
            Console.WriteLine($"Found {files.Length} files:");

            foreach (var file in files)
                Console.WriteLine("- " + Path.GetFileName(file));

            return files;
        }

        private static void ArduinoSerial_DataReceived(object sender, ArduinoSerial.DataEventArgs e)
        {
            foreach (var b in e.Bytes)
            {
                // Check for very magic string
                if (b == 'x')
                    Beep();
            }
        }

        private static void Beep()
        {
            if (beepTask == null)
            {
                beepTask = Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("PlayFile");
                    PlayFile();

                    await Task.Delay(DELAY_TIMEOUT);
                    beepTask = null;
                });
            }
        }
    }
}
