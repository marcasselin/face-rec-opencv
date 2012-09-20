﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FaceDataBaseDealing
{
    class Database
    {
        private Dictionary<string, List<string>> databaseDictionary;

        public Database()
        {
            databaseDictionary = new Dictionary<string, List<string>>();
        }

        public Dictionary<string, List<string>> DatabaseDictionary
        {
            get { return databaseDictionary; }
            set { databaseDictionary = value; }
        }

        public bool ReadFileToDictionary(string dataPath)
        {
            FileStream fs;
            try
            {
                fs = new FileStream(dataPath, FileMode.Open);
            }
            catch (Exception)
            {

                return false;
            }

            StreamReader sr = new StreamReader(fs);

            if (databaseDictionary != null)
            {
                databaseDictionary.Clear();
            }
            databaseDictionary = new Dictionary<string, List<string>>();

            string line;
            string[] data;

            line = sr.ReadLine();
            while (line != null)
            {
                data = line.Split(';');

                if (!databaseDictionary.ContainsKey(data[1]))
                {
                    databaseDictionary.Add(data[1], new List<string>());
                }
                databaseDictionary[data[1]].Add(data[0]);

                line = sr.ReadLine();

            }

            sr.Close();
            fs.Close();

            return true;
        }

        public bool WriteDictionaryToFile(string dataPath)
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            StreamWriter sr = new StreamWriter(fs);

            foreach (var item in databaseDictionary)
            {
                foreach (string path in databaseDictionary[item.Key])
                {
                    sr.WriteLine(path + ";" + item.Key);
                }
            }

            sr.Close();
            fs.Close();

            return true;
        }

        public bool DeleteFromDictionary(string name)
        {
            if (databaseDictionary.ContainsKey(name))
            {
                databaseDictionary.Remove(name);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool EditDictionary(string name, string newName)
        {
            if (databaseDictionary.ContainsKey(name) && !databaseDictionary.ContainsKey(newName))
            {
                databaseDictionary.Add(newName, databaseDictionary[name]);
                databaseDictionary.Remove(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditDictionary(string name, List<string> images)
        {
            if (databaseDictionary.ContainsKey(name))
            {
                databaseDictionary[name].Clear();
                databaseDictionary[name] = images;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddToDictionary(string name, List<string> images)
        {
            if (databaseDictionary.ContainsKey(name))
            {
                databaseDictionary.Add(name, databaseDictionary[name]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}