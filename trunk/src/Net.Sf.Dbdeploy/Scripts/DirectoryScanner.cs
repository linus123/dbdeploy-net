using System;
using System.Collections.Generic;
using System.IO;
using Net.Sf.Dbdeploy.Exceptions;

namespace Net.Sf.Dbdeploy.Scripts
{
    public class DirectoryScanner
    {
        private readonly FilenameParser filenameParser;

        public DirectoryScanner()
        {
            filenameParser = new FilenameParser();
        }

        public List<ChangeScript> GetChangeScriptsForDirectory(DirectoryInfo directory)
        {
            try
            {
                Console.Out.WriteLine("Reading change scripts from directory " + directory.FullName + "...");
            }
            catch (IOException)
            {
                // ignore
            }

            List<ChangeScript> scripts = new List<ChangeScript>();

            foreach (FileInfo file in directory.GetFiles())
            {
				if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
					continue;

                string filename = file.Name;
                try
                {
                    Int64 id = filenameParser.ExtractIdFromFilename(filename);
                    scripts.Add(new ChangeScript(id, file));
                }
                catch (UnrecognisedFilenameException)
                {
                    // ignore
                }
            }

            return scripts;
        }
    }
}