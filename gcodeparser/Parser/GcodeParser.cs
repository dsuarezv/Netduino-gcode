using System;
using System.Text;


namespace gcodeparser
{
    public enum State
    { 
        
    }


    public class GCodeParser
    {
#if DESKTOP
        private static System.Globalization.CultureInfo USCulture = new System.Globalization.CultureInfo("en-US");
#endif

        private static char[] NumberBuffer = new char[20];
        private static int NumberBufferIndex = 0;

        private static bool DeletedLine = false;
        private static int LineNumber = 0;

        private static int CurrentIndex = 0;
        public static string Line;
        public static int ParserLineNumber = 0;

        public static void ParseLine(string line)
        {
            ParserLineNumber++;
            Line = line.ToUpper();
            CurrentIndex = 0;

            //Logger.Log("=== {0} ===", line);

            ParseCommand();
        }

        private static bool SkipSpaces()
        {
            while (CurrentIndex < Line.Length)
            {
                char c = Line[CurrentIndex];

                switch (c)
                {
                    // Skip spaces and tabs
                    case ' ':
                    case '\t':
                        CurrentIndex++;
                        break;
                    default:
                        return true;
                }
            }

            return false;
        }

        internal static char PeekChar()
        {
            if (!SkipSpaces()) return (char)0;
            if (CurrentIndex >= Line.Length) return (char)0;                

            return Line[CurrentIndex];
        }

        internal static char ReadChar()
        {
            if (!SkipSpaces()) return (char)0;
            if (CurrentIndex >= Line.Length) return (char)0;

            return Line[CurrentIndex++];
        }

        internal static string ReadNumber()
        { 
            NumberBufferIndex = 0;
            bool isFirst = true;

            while (true)
            {
                char c = PeekChar();

                switch (c)
                { 
                    case '(':
                        SkipToEndComment();
                        break;
                    case ' ': 
                    case '\t':
                        if (NumberBufferIndex > 0) return GetNumberString();
                        break;

                    case ';':
                        return GetNumberString();

                    case '-':
                        if (isFirst)
                            AddDigitToNumber(c);
                        else 
                            return GetNumberString();

                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        AddDigitToNumber(c);
                        break;
                    
                    case '\0':
                    default:
                        return GetNumberString();
                }

                isFirst = false;
            }
        }

        private static void SkipToEndComment()
        {
            while (true)
            {
                char c = ReadChar();
             
                if (c == ')') break;
            }
        }

        private static void AddDigitToNumber(char d)
        {
            ReadChar();
            NumberBuffer[NumberBufferIndex] = d;
            NumberBufferIndex++;
        }

        private static string GetNumberString()
        {
            if (NumberBufferIndex > 0)
            {
                return new string(NumberBuffer, 0, NumberBufferIndex);
            }
            else
            {
                return null;
            }
        }

        internal static int ParseInt()
        {
            string val = ReadNumber();

            if (val == null)
            {
                Logger.Error("ParseInt: no numbers at {0} in '{1}'. Skipped.", CurrentIndex, Line);
                return -1;
            }

            try
            {
                return int.Parse(val);
            }
            catch (Exception ex)
            {
                Logger.Error("ParseInt ERROR parsing '{0}': {1}", val, ex);
                return -1;
            }
        }

        internal static double ParseDouble()
        {
            string val = ReadNumber();

            if (val == null) 
            {
                Logger.Error("ParseDouble: no numbers at {0} in '{1}'. Skipped.", CurrentIndex, Line);
                return double.MinValue;
            }

            try
            {
                return Math2.StringToDouble(val);
            }
            catch (Exception ex)
            {
                Logger.Error("ParseDouble ERROR parsing '{0}': {1}", val, ex);
                return double.MinValue;
            }
        }

        private static bool ParseComment()
        {
            while (true)
            {
                char c = ReadChar();

                switch (c)
                {
                    case '\0': return false;
                    case ')': return true;
                }
            }
        }

        private static bool ParseDashComment()
        {
            int numDashes = 1;

            while (true)
            {
                char c = ReadChar();

                switch (c)
                {
                    case '\0': return false;
                    case '-': numDashes++; break;
                    default:
                        if (numDashes < 3)
                        {
                            Logger.Error("ERROR: Parse --- comment: not enough '-' to interpret as comment. Returning control to parser, but it's an unsuported combination.");
                            return true;
                        }
                        
                        return false;      // otherwise, the rest of the line is a comment, no need to continue.
                }
            }
        }

        private static void ParseCommand()
        {
            while (true)
            {
                char c = ReadChar();

                switch (c)
                {
                    case '\0': return;  // EOL

                    case ';': return;  // comment line

                    case '(': // Begin comment
                        if (!ParseComment()) return;
                        break;

                    case '-':   // comment --- (not parsed by readnumber)
                        if (!ParseDashComment()) return;
                        break;

                    case '/': 
                        DeletedLine = true;
                        break;
                        
                    case 'N':
                        LineNumber = ParseInt();
                        break;
                    
                    case 'G':
                        ProcessCommand(new CommandG());    
                        break;

                    case 'T':
                        ProcessCommand(new CommandT());
                        break;

                    case 'F':
                        ProcessCommand(new CommandF());
                        break;

                    case 'M':
                        ProcessCommand(new CommandM());
                        break;

                    case 'S':
                        ProcessCommand(new CommandS());
                        break;

                    case 'D':
                        ProcessCommand(new CommandD());
                        break;

                    case 'X':  // Primary axes
                    case 'Y': 
                    case 'Z':
                    case 'I':  
                    case 'J':
                    case 'K':
                    case 'R':
                        ProcessCommand(new CommandAxis(c));
                        break;

                    default:
                        Logger.Log("ParseCommand: Unknown state at {0}: '{1}'", CurrentIndex, c);
                        ReadChar();
                        break;
                }
            }
        }

        private static void ProcessCommand(BaseCommand cmd)
        {
            cmd.Deleted = DeletedLine;
            cmd.LineNumber = LineNumber;
            cmd.Parse();
        }
    }
}
