using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Toolkit
{
    public static class Directory
    {

        public static bool exists(String filename)
        {
            return System.IO.Directory.Exists(filename);
        }

        public static bool create(String path)
        {
            System.IO.Directory.CreateDirectory(path);
            return true;
        }

        public static bool delete_all_files(String path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            return true;
        }

        public static bool delete_files(String path, String wildcard)
        {
            var dir = new DirectoryInfo(path);

            foreach (var file in dir.EnumerateFiles(wildcard))
            {
                file.Delete();
            }

            return true;
        }

        public static bool delete(String path, bool recursive)
        {
            try
            {
                System.IO.Directory.Delete(@path, recursive);
            } catch (Exception e)
            {
            //    Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static void ReplaceText(String rootfolder, string filename_pattern, string text_to_find, string replacement_text)
        {
            //Log.WriteLine("   Getting files from \"" + rootfolder + "\" using pattern \"" + filename_pattern + "\".");
            string[] files = System.IO.Directory.GetFiles(rootfolder, filename_pattern, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                try
                {
                    //Log.WriteLine("   Reading \"" + file + "\".");
                    string contents = System.IO.File.ReadAllText(file);

                    if (contents.Contains(@text_to_find))
                    {
                        //Log.WriteLine("   Found text \"" + @text_to_find + "\".");
                        contents = contents.Replace(@text_to_find, @replacement_text);

                        // Make files writable
                        //Log.WriteLine("   Making file writable.");
                        System.IO.File.SetAttributes(file, FileAttributes.Normal);

                        //Log.WriteLine("   Writing \"" + contents + "\" to \"" + file + "\".");
                        System.IO.File.WriteAllText(file, contents);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.Message);
                }
            }
        }

    }
}
