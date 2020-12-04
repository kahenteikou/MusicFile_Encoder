using System;
using System.IO;
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
            //System.Console.WriteLine("コンストラクタ\n");
        }



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
            //Console.Write("File Path > ");
            string name = "C:\\Users\\yw325\\Desktop\\Music";   
            //string name = Console.ReadLine();
            List<string> str = new List<string>();

            Tree_Recursive(name,ref str);   //ファイルパス読み込み

            List<List<string>> index = new List<List<string>>();
            
            bool b = false;
            string t = "";

            List<string> tmp = new List<string>();

            foreach(string a in str)
            {
        //        Console.WriteLine(a);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            foreach(string st in str)
            {
                string s = System.IO.Path.GetDirectoryName(st);
            //    Console.WriteLine(s);
                if(t != s)
                {
                    if( b == true) // ループの最初は行わない
                    {
                        index.Add(tmp);
                        
                        //確認コード
                        foreach(var r in tmp)
                        {
        //                    Console.WriteLine(r);
                         }

          //              Console.WriteLine("\n\n\n");
                        tmp.Clear();
                    }
                    t = s;
                    tmp.Add(s);

                }else{
                    tmp.Add(s);
                }


                b = true;
            
            }
            index.Add(tmp);

            foreach(string a in tmp)
            {
            //    Console.WriteLine(a);
            }

            Console.WriteLine("index: " + index[0].Count());    //いくつ入っているか？





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
