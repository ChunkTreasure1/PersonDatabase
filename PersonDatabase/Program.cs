using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonDatabase.Core;

namespace PersonDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Database tempDatabase = new Database();
            tempDatabase.Run();
        }
    }

    class Print
    {
        public static void PrintColorText(string aText, ConsoleColor aColor)
        {
            Console.ForegroundColor = aColor;
            Console.Write(aText);

            Console.ForegroundColor = ConsoleColor.Yellow;

        }

        public static void PrintMiddle(string aText, bool aLine, int anOffsetY, int anOffsetX)
        {

            Console.SetCursorPosition((Console.WindowWidth - aText.Length - anOffsetX) / 2, Console.CursorTop + anOffsetY);

            if (aLine)
            {
                Console.WriteLine(aText);
            }
            else
            {
                Console.Write(aText);
            }
        }

        public static void PrintMiddle(string aText, bool aLine, int anOffsetY, int anOffsetX, ConsoleColor aColor)
        {
            Console.SetCursorPosition((Console.WindowWidth - aText.Length - anOffsetX) / 2, Console.CursorTop + anOffsetY);

            Console.ForegroundColor = aColor;
            if (aLine)
            {
                Console.WriteLine(aText);
            }
            else
            {
                Console.Write(aText);
            }

            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
