using System;
using System.Collections.Generic;
using System.IO;

namespace DiskSizeCheck
{
    class Program
    {
        public static long byteLimit { get { return m_byteLimit; } }
        static long m_byteLimit = 1;
        static string m_byteValueOutput;
        static string m_rootDirectory = "";


        static void Main(string[] args)
        {
            List<DirectoryMemory>  directories = new List<DirectoryMemory>();
            SetByteLimit(100, "mb");

            PrintMainHeader();

            while (true)
            {
                Console.CursorVisible = true;

                {   // Read input
                    while (true)
                    {
                        string input = ConsoleUtility.ReadLine().ToLower();
                        m_rootDirectory = "";

                        if (input.Trim() == "help")
                        {
                            PrintMainHeader();
                            ConsoleUtility.SystemValue("Change limit", "<limit = value|white>. For example: <limit = 100|white> for 100 bytes, or <limit = 1 gb|white> for 1000000000 bytes.");
                            ConsoleUtility.SystemValue("Run program", "To run the search, type the root directy you wish to search in, like <C:\\ProgramData|white>.");
                            Console.WriteLine();
                        }

                        else if (input.StartsWith("limit = "))
                        {
                            PrintMainHeader();

                            input = input.Substring(8);
                            string[] splitInput = input.Split(new char[] { ' ' });
                            long outValue;
                            if (!long.TryParse(splitInput[0], out outValue))
                            {
                                ConsoleUtility.Error("Could not parse " + splitInput[0]);
                                continue;
                            }

                            if (splitInput.Length == 1)     SetByteLimit(outValue);
                            else                            SetByteLimit(outValue, splitInput[1]);
                            ConsoleUtility.SystemValue("Limit changed", "Set to <" + m_byteValueOutput + "|white>");
                        }

                        else
                        {
                            if (!Directory.Exists(input))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("That path does not exist. Please write write an existing path.\n");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Gray;
                                continue;
                            }

                            m_rootDirectory = input.Replace('/', '\\');
                            break;
                        }
                    }
                }

                {   // Get the individual directories memory size
                    PrintMainHeader();
                    ConsoleUtility.SystemText("Searching...");
                    Console.WriteLine();
                    Console.CursorVisible = false;

                    string[] directoryNames = Directory.GetDirectories(m_rootDirectory);

                    // If there are no subdirectories, output only the root directories information and reset the loop
                    if(directoryNames.Length == 0)
                    {
                        PrintMainHeader();
                        ConsoleUtility.SystemText("There are no subdirectories for " + m_rootDirectory);
                        DirectoryMemory rootDirectory = new DirectoryMemory(m_rootDirectory, false);
                        Console.WriteLine();
                        ConsoleUtility.SystemValue("Total", new ByteConverter(rootDirectory.bytes).output);
                        Console.WriteLine();
                        continue;
                    }

                    // Else, read all subdirectories
                    foreach (string directoryName in directoryNames)
                        directories.Add(new DirectoryMemory(directoryName));

                    directories.Sort();
                }

                {   // Print results
                    PrintMainHeader();
                    ConsoleUtility.SystemText("Results:");

                    for (int i = 0; i < directories.Count; i++)
                    {
                        if (directories[i].bytes >= m_byteLimit)
                            directories[i].Print();
                    }

                    long totalMemory = 0;
                    foreach (DirectoryMemory directoryMemory in directories)
                        totalMemory += directoryMemory.bytes;

                    Console.WriteLine();
                    ConsoleUtility.SystemValue("Total", new ByteConverter(totalMemory).output);
                }

                Console.WriteLine();
                directories.Clear();
            }
            
        }

        static void SetByteLimit(long byteLimit, string type = "b")
        {
            m_byteLimit = System.Math.Max(1, byteLimit);
            if (type == "kb")       m_byteLimit *= 1000;
            else if (type == "mb")  m_byteLimit *= 1000000;
            else if (type == "gb")  m_byteLimit *= 1000000000;
            else if (type == "tb")  m_byteLimit *= 1000000000000;
            else if (type == "pb")  m_byteLimit *= 1000000000000000;

            m_byteValueOutput = m_byteLimit + " byte";
            if (m_byteLimit != 1)
                m_byteValueOutput += "s";

            if (m_byteLimit >= 1000)
                m_byteValueOutput += " (" + new ByteConverter(m_byteLimit) + ")";
        }


        static void PrintMainHeader()
        {
            Console.Clear();
            ConsoleUtility.Header("Directory Memory Search");
            ConsoleUtility.SystemValue("Byte limit", m_byteValueOutput);
            if(!string.IsNullOrEmpty(m_rootDirectory))
                ConsoleUtility.SystemValue("Directory", m_rootDirectory);
            ConsoleUtility.SystemText("Type <help|white> for help with commands.");
            Console.WriteLine();
        }
    }
}
