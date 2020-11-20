using System;
using System.Collections.Generic;
using System.IO;

namespace AddUseDatabaseNameInAllfiles
{
  class Program
  {
    static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("Adding a USE Database name for every files in a directory v1.0");
      string directoryName = @"C:\Users\username\Documents\_US - bug\Centralisation des bases\prod\";
      string pattern = "*.SQL";
      var files = Directory.GetFiles(directoryName, pattern);
      var oneFile = new List<string>();
      var allFiles = new Dictionary<string, List<string>>();
      foreach (string file in files)
      {
        try
        {
          oneFile = new List<string>();
          using (StreamReader streamReader = new StreamReader(file))
          {
            string line = string.Empty;
            while (streamReader.Peek() >= 0)
            {
              line = streamReader.ReadLine();
              oneFile.Add(line);
            }
          }
        }
        catch (Exception exception)
        {
          display($"Exception : {exception.Message}");
        }

        allFiles.Add(file, oneFile);
      }

      string outputDirectory = Path.Combine(directoryName, "output");
      if (!Directory.Exists(outputDirectory))
      {
        Directory.CreateDirectory(outputDirectory);
      }

      // writing modified scripts
      foreach (var kvpFile in allFiles)
      {
        //sourceServerName_SQL_instance.ReferenceDatabaseName-20201120-1027serverTarget_SqlInstance.TargetDatabaseName_C_12
        string databaseName = kvpFile.Key.Split('.')[2];
        string addUseDatabaseName = $"USE {databaseName}{Environment.NewLine}GO{Environment.NewLine}";

        try
        {
          string outputFileName = Path.Combine(outputDirectory, Path.GetFileName(kvpFile.Key));
          using (StreamWriter streamWriter = new StreamWriter(outputFileName))
          {
            streamWriter.WriteLine(addUseDatabaseName);
            foreach (string line in kvpFile.Value)
            {
              streamWriter.WriteLine(line);
            }
          }
        }
        catch (Exception exception)
        {
          display($"Exception while writing files: {exception.Message}");
        }
      }
      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}
