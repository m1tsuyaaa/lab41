using System;

namespace Lab4
{
  public class Caretaker
  {
    public object Memento;

    public void SaveState(IOriginator originator)
    {
      Memento = originator.GetMemento();
    }

    public void RestoreState(IOriginator originator)
    {
      originator.SetMemento(Memento);
    }
  }
}