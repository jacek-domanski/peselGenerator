using System;
using System.IO;

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
    private static int yearTo = 1994;
    private static int monthFrom = 5;
    private static int monthTo = 8;
    private static int dayFrom = 1;
    private static int dayTo = 31;
    private static Sex sex = Sex.Undefined;
    private static string fileName = "pesel.txt";
    static void Main(string[] args)
    {
      parseArgs(args);
      printWorkingParams();

      int serialStartsAt, serialIncrement;
      setSerialParameters(out serialStartsAt, out serialIncrement);

      using (StreamWriter writer = new StreamWriter(fileName))
      {
        loopOverDates(serialStartsAt, serialIncrement, writer);
      }
      Console.WriteLine("Done!");
    }

    private static void setSerialParameters(out int serialStartsAt, out int serialIncrement)
    {
      serialStartsAt = 0;
      serialIncrement = 1;
      if (sex == Sex.Male)
      {
        serialStartsAt = 1;
        serialIncrement = 2;
      }
      else if (sex == Sex.Female)
      {
        serialStartsAt = 0;
        serialIncrement = 2;
      }
    }

    private static void loopOverDates(int serialStartsAt, int serialIncrement, StreamWriter writer)
    {
      for (long y = yearFrom; y <= yearTo; y++)
      {
        for (long m = monthFrom; m <= monthTo; m++)
        {
          int daysInMonth = DateTime.DaysInMonth((int)y, (int)m);

          for (long d = dayFrom; d <= dayTo; d++)
          {
            if (d > daysInMonth)
            {
              break;
            }
            peselsForDateOfBirth(serialStartsAt, serialIncrement, writer, y, m, d);
          }
        }
      }
    }

    private static void peselsForDateOfBirth(int serialStartsAt, int serialIncrement, StreamWriter writer, long y, long m, long d)
    {
      long pesel;
      for (long s = serialStartsAt; s <= 9999; s += serialIncrement)
      {
        pesel =
          (y % 100) * 1_000_000_000 +
          m * 10_000_000 +
          d * 100_000 +
          s * 10;

        pesel += checksum(pesel);

        writer.WriteLine(pesel);
      }
    }

    private static void deleteFile()
    {
      if (System.IO.File.Exists(fileName))
      {
        try
        {
          System.IO.File.Delete(fileName);
        }
        catch (System.IO.IOException e)
        {
          Console.WriteLine(e.Message);
          return;
        }
      }
    }

    // Checksum calculation according to:
    // https://pl.wikipedia.org/wiki/PESEL
    private static int checksum(long pesel)
    {
      int[] weights = new int[10] { 3, 1, 9, 7, 3, 1, 9, 7, 3, 1 };
      int sum = 0;
      long divisor = 10;

      foreach (int weight in weights)
      {
        divisor *= 10;
        long digitWithZeros = pesel % divisor;
        pesel -= digitWithZeros;
        int digit = (int)(digitWithZeros / (divisor / 10));
        sum += digit * weight;
      }

      int remainder = sum % 10;
      return (10 - remainder) % 10;

    }

    private static void printWorkingParams()
    {
      Console.WriteLine("yearFrom: " + yearFrom);
      Console.WriteLine("yearTo: " + yearTo);
      Console.WriteLine("monthFrom: " + monthFrom);
      Console.WriteLine("monthTo: " + monthTo);
      Console.WriteLine("dayFrom: " + dayFrom);
      Console.WriteLine("dayTo: " + dayTo);
      Console.WriteLine("sex: " + sex);
      Console.WriteLine("file: " + fileName);
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
          fileName = args[i + 1];
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
