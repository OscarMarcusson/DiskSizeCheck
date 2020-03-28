using System;
using System.Collections.Generic;
using System.Text;

namespace DiskSizeCheck
{
    public class ByteConverter
    {
        long m_bytes;
        public string output { get { return m_output; } }
        string m_output;

        static readonly char[] splitChars = new char[] { '.' };

        public ByteConverter(long bytes)
        {
            m_bytes = bytes;
            if (m_bytes >= 1e15)        SetByteOutput(1e15, "petabyte");
            else if (m_bytes >= 1e12)   SetByteOutput(1e12, "terabyte");
            else if (m_bytes >= 1e9)    SetByteOutput(1e9,  "gigabyte");
            else if (m_bytes >= 1e6)    SetByteOutput(1e6,  "megabyte");
            else if (m_bytes >= 1e3)    SetByteOutput(1e3,  "kilobyte");
            else                        SetByteOutput(1,    "byte");
        }


        void SetByteOutput(double divideBy, string name)
        {
            double output = m_bytes / divideBy;
            m_output = (m_bytes / divideBy).ToString("0.0");

            string[] split = m_output.Split(splitChars);
            if (split[1] == "0")
            {
                m_output = split[0];
                if (m_output != "1")
                    name += "s";
            }
            else
                name += "s";
                
            m_output += " " + name;
        }


        public override string ToString()
        {
            return m_output;
        }
    }
}
