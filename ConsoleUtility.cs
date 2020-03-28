using System;
using System.Collections.Generic;
using System.Text;

namespace DiskSizeCheck
{
    public static class ConsoleUtility
    {
        public static void Header(string text)
        {
            text = text.ToUpper();
            string output = "   ";

            for(int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ') output += "   ";
                else output += text[i] + " ";
            }

            Console.WriteLine();
            Write(output, ConsoleColor.DarkMagenta);
        }


        public static void SystemValue(string valueName, string value)
        {
            for (int i = valueName.Length; i < 15; i++)
                valueName += " ";

            SystemText("<" + valueName + "|darkmagenta>  " + value);
        }

        public static void SystemText(string text) 
        {    
            Write(" " + text, ConsoleColor.Gray); 
        }


        public static void Error(string text) 
        {         
            Write(" " + text, ConsoleColor.DarkRed);  
        }



        public static string ReadLine()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ");
            return Console.ReadLine();
        }


        static void Write(string text, ConsoleColor color)
        {
            List<OutputData> data = new List<OutputData>();
            int startId = 0;
            for(int i = 0; i < text.Length; i++)
            {
                if(text[i] == '<')
                {
                    data.Add(new OutputData(text.Substring(startId, i-startId), color));

                    startId = i + 1;
                    string tmpText = "";
                    string tmpColorString = "";
                    ConsoleColor tmpColor = color;

                    for (int subI = startId+1; subI < text.Length; subI++)
                    {
                        if(text[subI] == '|')
                        {
                            tmpText = text.Substring(startId, subI - startId);
                            startId = subI + 1;
                            continue;
                        }
                        if(text[subI] == '>')
                        {
                            if (tmpText.Length > 0)
                            {
                                tmpColorString = text.Substring(startId, subI - startId).ToLower();
                                if (     tmpColorString == "red")           tmpColor = ConsoleColor.Red;
                                else if (tmpColorString == "darkred")       tmpColor = ConsoleColor.DarkRed;

                                else if (tmpColorString == "green")         tmpColor = ConsoleColor.Green;
                                else if (tmpColorString == "darkgreen")     tmpColor = ConsoleColor.DarkGreen;

                                else if (tmpColorString == "blue")          tmpColor = ConsoleColor.Blue;
                                else if (tmpColorString == "darkblue")      tmpColor = ConsoleColor.DarkBlue;

                                else if (tmpColorString == "yellow")        tmpColor = ConsoleColor.Yellow;

                                else if (tmpColorString == "cyan")          tmpColor = ConsoleColor.Cyan;
                                else if (tmpColorString == "darkcyan")      tmpColor = ConsoleColor.DarkCyan;

                                else if (tmpColorString == "magenta")       tmpColor = ConsoleColor.Magenta;
                                else if (tmpColorString == "darkmagenta")   tmpColor = ConsoleColor.DarkMagenta;

                                else if (tmpColorString == "white")         tmpColor = ConsoleColor.White;
                                else if (tmpColorString == "gray")          tmpColor = ConsoleColor.Gray;
                                else if (tmpColorString == "darkgray")      tmpColor = ConsoleColor.DarkGray;
                                else if (tmpColorString == "black")         tmpColor = ConsoleColor.Black;
                            }
                            else
                            {
                                tmpText = text.Substring(startId, subI - startId);
                            }

                            data.Add(new OutputData(tmpText, tmpColor));
                            startId = i = subI + 1;
                            break;
                        }
                    }
                }
            }

            if (startId < text.Length)
                data.Add(new OutputData(text.Substring(startId), color));

            for (int i = 0; i < data.Count; i++)
                data[i].Print();

            Console.ResetColor();
            Console.WriteLine();
        }



        class OutputData
        {
            string m_text;
            ConsoleColor m_color;

            public OutputData(string text, ConsoleColor color)
            {
                m_text = text;
                m_color = color;
            }


            public void Print()
            {
                Console.ForegroundColor = m_color;
                Console.Write(m_text);
            }
        }
    }
}
