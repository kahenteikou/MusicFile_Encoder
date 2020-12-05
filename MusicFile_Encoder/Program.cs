using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace MusicFile_Encoder
{
    class Encorder
    {

        void test(int start,int length)
        {

          //  Console.WriteLine(s);
        }



        //アルバム管理クラス
        class Album
        {
            //コンストラクタ
            public Album(List<string> st)
            {
                //フォルダ名を取得  
                string ss = System.IO.Path.GetDirectoryName(st[0]);
                string tt = Path.GetFileNameWithoutExtension(ss);

                Title = tt;
      //          Console.WriteLine(Title);
                Music_List =st;
            }

            private string Title;             //アルバム名
            private List<string> Music_List;  // 曲名リスト

            //タイトルを参照
            public string getTitle
            {
                get{
                    return Title;
                }
            }

            //曲リストを参照
            public List<string> getList
            {
                get{
                    return Music_List;
                }
            }


            //曲リストの数を参照
            public int getList_Count
            {
                get
                {
                    return Music_List.Count();
                }
            }

        }
        public Encorder()
        {
            //System.Console.WriteLine("コンストラクタ\n");
        }

        // ディレクトリを再帰的に走査
        public static void Tree_Recursive(string folderPath, ref List<string> list)
        {
            foreach (String fname in Directory.EnumerateFiles(folderPath))
            {
                list.Add(fname);
            }
            IEnumerable<String> folders = Directory.EnumerateDirectories(folderPath);
            if (folders.Count() == 0)
            {
                return;
            }
            else
            {
                foreach (String dname in folders)
                {
                    Tree_Recursive(dname, ref list);
                }
            }
        }
        // メイン
        public void Update()
        {
    
            //　ファイルパス読み込み
            string name = "C:\\Users\\yw325\\Desktop\\Music";


            // ファイルパス読み込み
            List<string> str = new List<string>();
            Tree_Recursive(name,ref str);           //ディレクトリを再帰的に走査

            List<Album> Enc_List = new List<Album>();               //エンコード リスト
            List<List<string>> index = new List<List<string>>();    //アルバムことに分解    

            // ------------------------------------ アルバムごとに分解
            bool b = false;
            string t = "";
            List<string> tmp = new List<string>();
            foreach (string st in str)
            {
               string s = System.IO.Path.GetFileName(Path.GetDirectoryName(st));
                //Console.WriteLine(s);
                if (t != s)
                {
                    if( b == true) // ループの最初は行わない
                    {
                        index.Add(tmp);
                        tmp = new List<string>();
                    }
                    t = s;
                    tmp.Add(st);

                }else{
                    tmp.Add(st);
                }
                b = true;
            }
            index.Add(tmp);
            // ------------------------------------ 

            // アルバムクラスに設定
            Album enc = new Album(new List<string>(index[0]));






            // ------------------------------- エンコードする 
            string DirName = Path.GetFileName(Path.GetDirectoryName(index[0][0]));  //ディレクトリ名 
            string DirName_FullPath = Path.GetDirectoryName(index[0][0]);           //ディレクトリ名のフルパス
            Directory.CreateDirectory(DirName);                                     //ディレクトリを生成
            string Select_Path = DirName_FullPath;  //出力ディレクトリのフルパスを指定して出力ディレクトリを設定
            int num = 0;
            int thread_count = str.Count;
            using(CountdownEvent ce　=　new CountdownEvent(thread_count))
            {
                foreach (string fpath in str)
                {
                    string[] pool = { fpath, fpath.Replace(name, Select_Path)}; //ディレクトリ名をdest_path変数に変換

                    //スレットプール
                    ThreadPool.QueueUserWorkItem(state => {
                        string[] state_array = (string[])state;
                        Run_Encode(state_array[0], state_array[1]);
                        
                        ce.Signal();//シグナル送信
                    },pool);  //スレッドプールにする
                    num++;
                }

                //Wait
                ce.Wait();
            }
            // ------------------------------- 

            Console.WriteLine("終了");

            




            Console.ReadKey();
        }

        // エンコード
        private static void Run_Encode(string fname, string out_name)   //out_name ファイル名のフルパス
        {
            
            Process proc = new Process();           //外部プログラムを起動するためのクラス
            proc.StartInfo.FileName = "ffmpeg.exe"; //.exeを指定

          //  Console.WriteLine(out_name);                                                  
            string option = " -vn -ac 2 -ar 44100 -ab 320.2k -acodec libmp3lame -f wav ";   //オプション
            const string dc = "\"";                                                         //ダブルクォート
            string OutPut_Title = Path.GetFileNameWithoutExtension(out_name);               //ファイル名(拡張子    なし)
            string TitleName = OutPut_Title +".wav";                                       //ファイル名(拡張子　あり)
            //Console.WriteLine(OutPut_Title);
            string DirName = Path.GetDirectoryName(out_name);                               //ディレクトリ名 フルパス
            Console.WriteLine(DirName);
            Directory.CreateDirectory(DirName);                                             //ディレクトリを生成
            

            Console.WriteLine("このディレクトリに出力: "+DirName);
            Console.WriteLine("ああ　" + OutPut_Title);
            Console.WriteLine("いい　" + TitleName);
            Console.WriteLine("うう" + fname);

            //エンコードファイル(フルパス), 出力ファイル名 
            proc.StartInfo.Arguments = " -y -i " + dc + fname + dc + option + dc + DirName + "\\" + TitleName + dc;

            
            




            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();







            string file = System.IO.Path.GetFileName(fname);
            if (proc.ExitCode != 0)
            {
                // ERROR
                //ConsoleColor Back_Color = ConsoleColor.DarkGreen;   //背景色
                ConsoleColor Fore_Color = ConsoleColor.Red;        //前景色

                //Console.BackgroundColor = Back_Color;   //背景色を設定
                Console.ForegroundColor = Fore_Color;   //前景色を設定

                Console.Error.Write("[ ERROR ]");
                Console.ResetColor();   //配色を元に戻す
                Console.WriteLine("  " + file);

            }
            else
            {
                //成功　
                ConsoleColor Back_Color = ConsoleColor.DarkGreen;   //背景色
                ConsoleColor Fore_Color = ConsoleColor.Gray;        //前景色

                Console.BackgroundColor = Back_Color;   //背景色を設定
                Console.ForegroundColor = Fore_Color;   //前景色を設定

                Console.Write("[ OK! ]" );
                Console.ResetColor();   //配色を元に戻す
                Console.WriteLine(" "+file);

            }
            

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
