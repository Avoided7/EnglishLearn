namespace EnglishLearn.Models;

public class Container
{
  public string Word { get; set; } = string.Empty;
  public string Translate { get; set; } = string.Empty;
  public bool IsKnow { get; set; }

  public override string ToString()
  {
    return $"{Word}: {Translate}";
  }
}