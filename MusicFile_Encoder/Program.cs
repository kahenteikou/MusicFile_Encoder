using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace MusicFile_Encoder
{
    class Encorder
    {
        public Encorder()
        {
            System.Console.WriteLine("コンストラクタ\n");


        }

        // メイン
        public void Update()
        {
            var app = new ProcessStartInfo();
            //app.UseShellExecute = true;



            Console.Write("FilePath >  ");
            string name = Console.ReadLine();
            string output = "C:\\Users\\yw325\\Desktop\\MusicFile_Encoder\\MusicFile_Encoder\\bin\\Debug";
            string tmp =  " -i " + name + " -vn -ac 2 -ar 44100 -ab 320k - acodec libmp3lame -f wav " + output + "test.wav";
            Console.WriteLine(tmp);
            app.FileName = "ffmpeg.exe";
            app.Arguments = tmp;
            app.CreateNoWindow = true;

            Process.Start(app);
            Console.ReadKey();
        }
    }

    class Program
    {
      

        static void Main(string[] args)
        {
            Encorder enc = new Encorder();

            enc.Update();
             



          //  Console.ReadKey();
        }
    }
}
