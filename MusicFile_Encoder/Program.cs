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
            foreach (String fname in Directory.EnumerateFiles(folderPath))//ファイルを列挙する
            {
                list.Add(fname);
            }

            IEnumerable<String> folders = Directory.EnumerateDirectories(folderPath);//ディレクトリを列挙
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


        //ディレクトリ名　だけを返す
        private string getdirname_Path(string str)
        {
            string s = Path.GetDirectoryName(str);
            return s;
        }

        private string getdirname(string str)
        {
            string s = Path.GetFileNameWithoutExtension(str);
            return s;
        }



        private string RootInputDir_Path;
        // メイン
        public void Update()
        {
            
            //　ファイルパス読み込み
            RootInputDir_Path = "C:\\Users\\yw325\\Desktop\\Music";


            // ファイルパス読み込み
            List<string> FileList = new List<string>();         //ファイルフルパスのリスト     
            Tree_Recursive(RootInputDir_Path, ref FileList);    //ディレクトリを再帰的に走査


            //ファイルパス確認用
            foreach(var v in FileList)
            {
            //    Console.WriteLine(v);
            }
            
                
            List<List<string>> AlbumList = new List<List<string>>();   //ディレクトリ名ごとまとめる

            // --------------------------------------------------------------------------------  ファイルのフルパスを分解 
            string tt = "";
            List<string> tmp = new List<string>();
            bool eee = false;
            foreach(string str in FileList)
            {
               // Console.WriteLine(getdirname(str));
                string ss = Path.GetDirectoryName(str);
            //    Console.WriteLine(str);

                if (tt != ss)
                {
                    tt = ss;
                    if ( eee == true)
                    {
                        AlbumList.Add(new List<string>(tmp));
                        tmp = new List<string>();
                    }
                    tmp.Add(str);

                }
                else
                {
                    tmp.Add(str);

                }
                
                eee = true;
            }
            AlbumList.Add(new List<string>(tmp));
            // --------------------------------------------------------------------------------


            // エンコードをかける
            foreach(List<string> l in AlbumList)
            {
                Encode(l);
            }

 
            // ディレクトリに移動
            string www = getdirname_Path(AlbumList[0][0]);
            string r = getdirname_Path(www);
            string file = getdirname(getdirname_Path(www)); 

            Console.WriteLine(r);

           // Directory.CreateDirectory(file);
            List<string> vs = new List<string>();
            foreach (List<string> t in AlbumList)
            {
               // Console.WriteLine(getdirname_Path(getdirname_Path(t[0])));
                
                if (getdirname_Path(getdirname_Path(t[0])) == r)
                {
                    vs.Add(getdirname_Path(t[0]));    
                }
            }


            //確認ログ
            
            foreach(string y in vs)
            {
            
            
            }



            


            Console.WriteLine("\n\n-------------- 処理終了 --------------");          
            Console.ReadKey();
        }

        // --エンコードをかける関数
        void Encode(List<string> FileList)  //引数はフルパス
        {
            
            // ------------------------------------ 
            
            string DirName = Path.GetFileName(Path.GetDirectoryName(FileList[0]));  //ディレクトリ名 
            string DirName_FullPath = Path.GetDirectoryName(FileList[0]);           //ディレクトリ名のフルパス
            Directory.CreateDirectory(DirName);                                     //ディレクトリを生成
            string Select_Path = DirName_FullPath;  //出力ディレクトリのフルパスを指定して出力ディレクトリを設定
            int num = 0;
            int thread_count = FileList.Count;

            Console.WriteLine("-------------- "+ DirName +" --------------\n");
            using (CountdownEvent ce = new CountdownEvent(thread_count))
            {
                foreach (string fpath in FileList)
                {
                    string[] pool = { fpath, fpath.Replace(RootInputDir_Path, Select_Path) }; //ディレクトリ名をdest_path変数に変換

                    //スレットプール
                    ThreadPool.QueueUserWorkItem(state => {
                        string[] state_array = (string[])state;
                        Run_Encode(state_array[0], state_array[1]);

                        ce.Signal();//シグナル送信
                    }, pool);  //スレッドプールにする
                    num++;
                }

                //Wait
                ce.Wait();
            }
            // ------------------------------- 
            Console.Write("\n");
        }


        // エンコード
        private static void Run_Encode(string fname, string out_name)   //out_name ファイル名のフルパス
        {
            
            Process proc = new Process();           //外部プログラムを起動するためのクラス
            proc.StartInfo.FileName = "ffmpeg.exe"; //.exeを指定

         
            string option = " -vn -ac 2 -ar 44100 -ab 320.2k -f wav ";          //オプション
            const string dc = "\"";                                             //ダブルクォート
            string OutPut_Title = Path.GetFileNameWithoutExtension(out_name);   //ファイル名(拡張子    なし)
            string TitleName = OutPut_Title +".wav";                            //ファイル名(拡張子　あり)
            string DirName = Path.GetFileName(Path.GetDirectoryName(out_name)); //ディレクトリ名                                
            Directory.CreateDirectory(DirName);                                 //ディレクトリを生成
            

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
                ConsoleColor Fore_Color = ConsoleColor.Red; //前景色
                Console.ForegroundColor = Fore_Color;   //前景色を設定

                Console.Write("[ ERROR ]");
                Console.ResetColor();   //配色を元に戻す
                Console.WriteLine("  " + file);

            }
            else
            {
                //成功　
                ConsoleColor Back_Color = ConsoleColor.DarkGreen;   //背景色
                ConsoleColor Fore_Color = ConsoleColor.Gray;        //前景色

                Console.Write("       ");

                Console.BackgroundColor = Back_Color;   //背景色を設定
                Console.ForegroundColor = Fore_Color;   //前景色を設定

                Console.Write("[ OK! ]" );
                Console.ResetColor();   //配色を元に戻す
                Console.WriteLine("      "+file);

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
