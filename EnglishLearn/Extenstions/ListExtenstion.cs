namespace EnglishLearn.Extenstions;

public static class ListExtenstion
{
  public static List<T> Shuffle<T>(this List<T> list)
  {
    List<T> copy = new List<T>(list);
    List<T> result = new List<T>();

    while (copy.Count > 0)
    {
      int count = copy.Count;
      int randomIndex = Random.Shared.Next(0, count);

      T element = copy[randomIndex];
      
      result.Add(element);
      copy.Remove(element);
    }

    return result;
  }
}