using System;

namespace HlslDecompiler.Util
{
    public class CommandLineOptions
    {
        public string InputFilename { get; }
        public bool DoAstAnalysis { get; }
        public int Offset { get; }

        public static CommandLineOptions Parse(string[] args)
        {
            return new CommandLineOptions(args);
        }

        private CommandLineOptions(string[] args)
        {
            Offset = 0;

            foreach (string arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    string option = arg.Substring(2);
                    if (option == "ast")
                    {
                        DoAstAnalysis = true;
                    }
                    else if (option.StartsWith("offset="))
                    {
                        string le = "offset=";
                        Offset = Convert.ToInt32(option.Substring(le.Length));
                    }
                    else
                    {
                        Console.WriteLine("Unknown option: --" + option);
                    }
                }
                else
                {
                    InputFilename = arg;
                }
            }
        }
    }
}
