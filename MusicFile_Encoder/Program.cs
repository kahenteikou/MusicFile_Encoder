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
            Console.WriteLine(enc.getList_Count);





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
