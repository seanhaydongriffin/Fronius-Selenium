using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Toolkit
{
    public static class File
    {

        public static bool exists(String filename)
        {
            return System.IO.File.Exists(filename);
        }



        public static String read(String filename)
        {
            //      string result_str = System.IO.File.ReadAllText(filename);
            //return result_str;

            // Code above will fail if the file is used by another process.  Solution below.

            string text = "";

            using (FileStream logFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader logFileReader = new StreamReader(logFileStream))
                {
                    text += logFileReader.ReadToEnd();
                }
            }

            return text;
        }



        public static bool overwrite(String filename, String data)
        {
            try
            {
                System.IO.File.WriteAllText(@filename, data);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        public static bool append(String filename, String text)
        {
            System.IO.File.AppendAllText(@filename, text);
            //            System.IO.File.AppendAllText(@filename, text + Environment.NewLine);

            return true;
        }

        public static bool remove_bytes_from_end(String filename, long num_of_bytes)
        {

            FileInfo fi = new FileInfo(filename);
            FileStream fs = fi.Open(FileMode.Open);
            fs.SetLength(Math.Max(0, fi.Length - num_of_bytes));
            fs.Close();

            return true;
        }

        public static bool delete(String directory_path, String filename)
        {
            bool result = true;

            var dir = new DirectoryInfo(directory_path);

            try
            {
                foreach (var file in dir.EnumerateFiles(filename))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        result = false;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                result = false;
            }

            return result;
        }

        public static bool delete(String file_path)
        {
            if (exists(file_path))

                System.IO.File.Delete(file_path);

            return true;
        }

        public static bool Copy(String SourceFilePath, String TargetFilePath)
        {
            bool result = true;

            try
            {
                System.IO.File.Copy(SourceFilePath, TargetFilePath, true);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }


        public static bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())) + "]");
            if (containsABadCharacter.IsMatch(testName)) { return false; };

            // other checks for UNC, drive-path format, etc

            return true;
        }

        public static String GetLatestFilepath(String SourceDirectory)
        {
            String result = "";

            try
            {
                var directory = new DirectoryInfo(SourceDirectory);
                var myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                result = myFile.FullName;
            }
            catch (Exception e)
            {
                result = "";
            }

            return result;
        }

        public static bool CopyLatest(String SourceDirectory, String TargetDirectory)
        {
            bool result = true;

            try
            {
                var directory = new DirectoryInfo("C:\\MyDirectory");
                var myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                Copy(myFile.FullName, "R:\\");
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        public static bool CopyLatestFromDownloads(String SourceDirectory, String TargetDirectory)
        {
            bool result = true;

            try
            {
                var directory = new DirectoryInfo("C:\\MyDirectory");
                var myFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                Copy(myFile.FullName, "R:\\");
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        public static bool Move(String directory_path, String oldName, String newNameFullPath)
        {


            bool result = true;
            var dir = new DirectoryInfo(directory_path);

            try
            {
                foreach (var file in dir.EnumerateFiles(oldName))
                {
                    file.MoveTo(newNameFullPath);
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        public static bool RobustCopy(String SourceFilePath, String TargetFilePath, String Filename = "")
        {
            bool result = true;

            if (Filename.Length > 0)

                Filename = " \"" + Filename + "\"";

            try
            {
                Command.execute("robocopy.exe", "\"" + SourceFilePath + "\" \"" + TargetFilePath + "\"" + Filename + " /sec /purge /is /it /r:15 /w:1 /np /ns /nc /nfl /ndl /njh /njs", Environment.SystemDirectory, Encoding.UTF8);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

    }
}
