using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace DiskSizeCheck
{
    public class DirectoryMemory : IComparable<DirectoryMemory>
    {
        string m_path;
        public string path { get { return m_path; } }

        public string directory { get { return m_directory; } }
        string m_directory;

        long m_bytes;
        public long bytes { get { return m_bytes; } }

        string m_output;
        public string output { get { return m_output; } }

        string m_printName;
        string m_printOutput;
        public string printOutput { get { return m_printName + m_printOutput; } }

        bool m_writeResults;


        public ConsoleColor color { get { return m_color; } }
        ConsoleColor m_color;


        public DirectoryMemory(string path, bool writeResults = true)
        {
            m_path = path;
            m_path = m_path.Replace("{", "");
            m_path = m_path.Replace("}", "");
            m_writeResults = writeResults;

            if (!Directory.Exists(path))
            {
                m_output = "Invalid path";
                return;
            }

            // Get the directory name from the path
            for (int i = m_path.Length - 1; i >= 0; i--)
            {
                if (m_path[i] == '\\')
                {
                    m_directory = m_path.Substring(i+1);
                    break;
                }
            }

            if (string.IsNullOrEmpty(directory))
                m_directory = m_path;

            // Save the print friendly name of the directory. It will have an exact length of 55 characters
            m_printName = "   " + directory;
            if (m_printName.Length >= 52)
            {
                m_printName = m_printName.Substring(0, 52) + "...   ";
            }
            else
            {
                for (int i = directory.Length; i < 55; i++)
                    m_printName += " ";
            }
            

            if(m_writeResults)
                Console.WriteLine("...");

            RecalculateMemory();
        }



        void RecalculateMemory()
        {
            m_bytes = 0;
            RecursiveMemoryCheck(m_path);

            if (string.IsNullOrEmpty(m_output))
                m_printOutput = "<System error|darkgray>";
            else 
                m_printOutput = "<" + m_output + "|" + m_color.ToString() + ">";

            AddBlankSpacesToPrintOutput();

            if (m_writeResults)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ConsoleUtility.SystemText(printOutput);
            }
        }


        void RecursiveMemoryCheck(string path)
        {
            try
            {
                // First add any files in this directory
                string[] names = Directory.GetFiles(path, "*.*");
                foreach (string fileName in names)
                    m_bytes += new FileInfo(fileName).Length;

                UpdateOutput();

                // Then get all directories, and run recursively on each one
                names = Directory.GetDirectories(path);
                foreach (string directory in names)
                    RecursiveMemoryCheck(directory);
            }
            catch
            {
            }
        }


        void UpdateOutput()
        {
            m_output = new ByteConverter(m_bytes).output;

            if (m_bytes < Program.byteLimit)            m_color = ConsoleColor.DarkGreen;
            else if (m_bytes < Program.byteLimit * 2)   m_color = ConsoleColor.Yellow;
            else if (m_bytes < Program.byteLimit * 10)  m_color = ConsoleColor.DarkRed;
            else                                        m_color = ConsoleColor.Red;

            m_printOutput = "<" + new ByteConverter(m_bytes).output + "|" + m_color.ToString() + ">";
            AddBlankSpacesToPrintOutput();

            
            if (m_writeResults)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ConsoleUtility.SystemText(printOutput);
            }
        }


        void AddBlankSpacesToPrintOutput()
        {
            for (int i = m_printName.Length + m_printOutput.Length; i < 95; i++)
                m_printOutput += " ";
        }

        public int CompareTo([AllowNull] DirectoryMemory other)
        {
            return m_bytes.CompareTo(other.m_bytes);
        }


        public void Print()
        {
            ConsoleUtility.SystemText(printOutput);
        }
    }
}
