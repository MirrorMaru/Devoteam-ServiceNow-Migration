using System;
using System.Collections.Generic;
using System.IO;

namespace Devoteam_ServiceNow_Migration
{
    public class FilesInitialization
    {
        public List<ImportedFile> files;

        public FilesInitialization()
        {
            this.files = new List<ImportedFile>();
        }

        public void Initialization()
        {
            string path = Environment.CurrentDirectory + "/files/config.txt";
            List<string> folderList = new List<string>();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                string currentFolder = null;

                foreach (var VARIABLE in lines)
                {
                    string trimmedLine = VARIABLE.Trim();

                    if (trimmedLine.EndsWith(":"))
                    {
                        //It's a folder !
                        currentFolder = trimmedLine.TrimEnd(':');
                        folderList.Add(currentFolder);
                    }
                    else
                    {
                        //It's a file !
                        string[] parts = trimmedLine.Split(',');
                        string fileName = parts[0].Trim();

                        if (parts.Length > 1)
                        {
                            //There is parameters
                            string param = parts[1].Trim();
                            files.Add(new ImportedFile(currentFolder, fileName, param));
                        }
                        else
                        {
                            files.Add(new ImportedFile(currentFolder, fileName, ""));
                        }
                    }
                }
            }
            //Creating folders if they don't already exists
            path = Environment.CurrentDirectory + "/out";
            foreach (var VARIABLE in folderList)
            {
                if (!Directory.Exists(path + "/" + VARIABLE))
                {
                    try
                    {
                        Directory.CreateDirectory(path + "/" + VARIABLE);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}