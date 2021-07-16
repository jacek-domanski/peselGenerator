using System;

namespace peselGenerator
{
  enum Sex
  {
    Undefined,
    Male,
    Female
  }
  class PeselGenerator
  {
    private static int yearFrom = 1992;
    private static int yearTo = 1993;
    private static int monthFrom = 1;
    private static int monthTo = 12;
    private static int dayFrom = 1;
    private static int dayTo = 31;
    private static Sex sex = Sex.Undefined;
    private static string file = "pesel.txt";
    static void Main(string[] args)
    {
      parseArgs(args);
      printWorkingParams();


    }

    private static void printWorkingParams()
    {
      Console.WriteLine("yearFrom " + yearFrom);
      Console.WriteLine("yearTo " + yearTo);
      Console.WriteLine("monthFrom " + monthFrom);
      Console.WriteLine("monthTo " + monthTo);
      Console.WriteLine("dayFrom " + dayFrom);
      Console.WriteLine("dayTo " + dayTo);
      Console.WriteLine("sex " + sex);
      Console.WriteLine("file " + file);
    }

    private static void parseArgs(string[] args)
    {

      // TODO limit values
      int i = 0;
      while (i < args.Length)
      {
        Console.WriteLine("Parsing " + args[i]);
        if (args[i] == "-yf" || args[i] == "--year-from")
        {
          int.TryParse(args[i + 1], out yearFrom);
          i += 2;
        }
        else if (args[i] == "-yt" || args[i] == "--year-to")
        {
          int.TryParse(args[i + 1], out yearTo);
          i += 2;
        }
        else if (args[i] == "-mf" || args[i] == "--month-from")
        {
          int.TryParse(args[i + 1], out monthFrom);
          i += 2;
        }
        else if (args[i] == "-mt" || args[i] == "--month-to")
        {
          int.TryParse(args[i + 1], out monthTo);
          i += 2;
        }
        else if (args[i] == "-df" || args[i] == "--day-from")
        {
          int.TryParse(args[i + 1], out dayFrom);
          i += 2;
        }
        else if (args[i] == "-dt" || args[i] == "--day-to")
        {
          int.TryParse(args[i + 1], out dayTo);
          i += 2;
        }
        else if (args[i] == "-s" || args[i] == "--sex")
        {
          string arg = args[i + 1];
          if (arg.ToLower().StartsWith('m'))
          {
            sex = Sex.Male;
          }
          else if (arg.ToLower().StartsWith('f') || arg.ToLower().StartsWith('w'))
          {
            sex = Sex.Female;
          }
          i += 2;
        }
        else if (args[i] == "-f" || args[i] == "--file")
        {
          file = args[i + 1];
          i += 2;
        }
        else
        {
          i++;
        }
      }
    }
  }
}
