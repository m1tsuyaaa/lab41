using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Lab4
{
  class Program
  {
    static void Main()
    {
      Console.WriteLine("=== Laboratory Work No. 4 ===\n");

      Console.WriteLine("=== Part 1. Standard Input-Output ===");

      TextReader originalIn = Console.In;
      TextWriter originalOut = Console.Out;
      TextWriter originalError = Console.Error;

      try
      {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        Console.Write("Enter your age: ");
        int age = int.Parse(Console.ReadLine());

        Console.WriteLine($"\nHello, {name}! You are {age} years old.");

        string outputFile = "output.txt";
        using (StreamWriter sw = new StreamWriter(outputFile))
        {
          Console.SetOut(sw);
          Console.WriteLine("--- Report ---");
          Console.WriteLine($"Name: {name}");
          Console.WriteLine($"Age: {age}");
          Console.WriteLine($"Date: {DateTime.Now}");
        }
        Console.SetOut(originalOut);
        Console.WriteLine($"Output saved to {outputFile}. Content:\n{File.ReadAllText(outputFile)}");

        string inputFile = "input.txt";
        File.WriteAllText(inputFile, $"{name}\n{age}");
        using (StreamReader sr = new StreamReader(inputFile))
        {
          Console.SetIn(sr);
          string fileInputName = Console.ReadLine();
          string fileInputAge = Console.ReadLine();
          Console.WriteLine($"Reading from file: name = {fileInputName}, age = {fileInputAge}");
        }
        Console.SetIn(originalIn);

        Console.Error.WriteLine("This is a message to the standard error stream.");

        string errorFile = "error.log";
        using (StreamWriter err = new StreamWriter(errorFile))
        {
          Console.SetError(err);
          Console.Error.WriteLine("Error: test record.");
        }
        Console.SetError(originalError);
        Console.WriteLine($"Error stream written to {errorFile}");
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine($"Error: {ex.Message}");
      }
      finally
      {
        Console.SetIn(originalIn);
        Console.SetOut(originalOut);
        Console.SetError(originalError);
      }

      Console.WriteLine("\n=== Part 2. File Input-Output ===");

      string testFile = "test.dat";

      using (FileStream fs = new FileStream(testFile, FileMode.Create))
      using (BinaryWriter bw = new BinaryWriter(fs))
      {
        bw.Write(123.45);
        bw.Write("Hello, File!");
        bw.Write(42);
      }

      using (FileStream fs = new FileStream(testFile, FileMode.Open))
      using (BinaryReader br = new BinaryReader(fs))
      {
        double d = br.ReadDouble();
        string s = br.ReadString();
        int i = br.ReadInt32();
        Console.WriteLine($"Read from file: double={d}, string={s}, int={i}");
      }

      FileInfo fi = new FileInfo(testFile);
      Console.WriteLine($"File attributes: {fi.Attributes}");
      fi.Attributes |= FileAttributes.ReadOnly;
      Console.WriteLine($"ReadOnly set. New attributes: {fi.Attributes}");
      fi.Attributes &= ~FileAttributes.ReadOnly;

      Console.WriteLine("\n=== Part 3. Binary Serialization ===");

      FullNameClass person = new FullNameClass("Ivan", "Ivanov", "Ivanovich");
      person.Print();

      string binFile = "person.bin";
      BinaryFormatter bf = new BinaryFormatter();

      using (FileStream fs = new FileStream(binFile, FileMode.Create))
      {
        bf.Serialize(fs, person);
      }
      Console.WriteLine($"Object serialized to {binFile}");

      using (FileStream fs = new FileStream(binFile, FileMode.Open))
      {
        FullNameClass restored = (FullNameClass)bf.Deserialize(fs);
        Console.Write("Deserialized object: ");
        restored.Print();
      }

      Console.WriteLine("\n=== Part 4. XML Serialization ===");

      string xmlFile = "person.xml";
      XmlSerializer xs = new XmlSerializer(typeof(FullNameClass));

      using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
      {
        xs.Serialize(fs, person);
      }
      Console.WriteLine($"Object serialized to {xmlFile}");
      Console.WriteLine($"XML content:\n{File.ReadAllText(xmlFile)}");

      using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
      {
        FullNameClass restored = (FullNameClass)xs.Deserialize(fs);
        Console.Write("Deserialized from XML: ");
        restored.Print();
      }

      Console.WriteLine("\n=== Part 5. Memento Pattern ===");

      FullNameClass originator = new FullNameClass("Petr", "Petrov", "Petrovich");
      Caretaker caretaker = new Caretaker();

      Console.Write("Initial state: ");
      originator.Print();

      caretaker.SaveState(originator);

      originator.Name = "Sidor";
      originator.Surname = "Sidorov";
      Console.Write("Modified state: ");
      originator.Print();

      caretaker.RestoreState(originator);
      Console.Write("Restored state: ");
      originator.Print();

      Console.WriteLine("\nLaboratory work completed. Press any key...");
      Console.ReadKey();
    }
  }
}