using System;

namespace Lab4
{
  [Serializable]
  public class FullNameClass : IOriginator
  {
    public string Name;
    public string Surname;
    public string MiddleName;

    public FullNameClass()
    {
    }

    public FullNameClass(string name, string surname, string middleName)
    {
      Name = name;
      Surname = surname;
      MiddleName = middleName;
    }

    public void Print()
    {
      Console.WriteLine($"Name={Name} Surname={Surname} Middle={MiddleName}");
    }

    public object GetMemento()
    {
      Memento m = new Memento();
      m.Name = Name;
      m.Surname = Surname;
      m.MiddleName = MiddleName;
      return m;
    }

    public void SetMemento(object memento)
    {
      if (memento is Memento)
      {
        Memento m = (Memento)memento;
        Name = m.Name;
        Surname = m.Surname;
        MiddleName = m.MiddleName;
      }
    }
  }
}