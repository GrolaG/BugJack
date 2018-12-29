using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlackjackAPP
{
    public static class BestScore
    {
        private static ArrayList list = new ArrayList();
        private static  Dictionary<string, int> scrores = new Dictionary<string, int>();
        private static string path = Directory.GetCurrentDirectory() + "\\best.txt";
        private static bool init = false;

        public static int GetBest(string player)
        {
            if (!init)
            {
                InitFile();
            }
            int best;
            scrores.TryGetValue(player, out best);
            return best;
        }
        public static void SetBest(string name, int value)
        {
            string[] arrLine = File.ReadAllLines(path);
            try
            {
                arrLine[FindLineWithPlayerName(name) - 1] = "" + name + ":" + value;
                File.WriteAllLines(path, arrLine);
            }
            catch (Exception)
            {
                File.WriteAllLines(path, arrLine);
                File.AppendAllText(path, "" + name + ":" + value);
            }
        }


        private static void InitFile()
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Close();
            StreamReader f = new StreamReader(path);
            string line;
            while ((line = f.ReadLine()) != null)
            {
                list.Add(line);
            }
            foreach (string item in list)
            {
                string[] temp = item.Split(':');
                try
                {
                    scrores.Add(temp[0], Int32.Parse(temp[1]));
                }
                catch (Exception)
                {
                    Console.WriteLine("Файл рекордов поврежден!");
                    System.Threading.Thread.Sleep(3000);
                }
            }
            init = true;
            f.Close();
        }
        private static int FindLineWithPlayerName(string name)
        {
            int counter = 1;
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Close();
            StreamReader f = new StreamReader(path);
            string line;

            while ((line = f.ReadLine()) != null)
            {
                if (line.Contains(name))
                {
                    break;
                }
                counter++;
            }
            f.Close();
            return counter++;
        }
    }

}
