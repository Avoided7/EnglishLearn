using EnglishLearn.Models;
using System.Text.Json;

namespace EnglishLearn;

public class Handler
{
  private readonly List<Container> _containers;
  private readonly int _wordLimit;
  private readonly string _fileName;
  
  public Handler(string fileName, int wordLimit = 10)
  {
    _containers = LoadWords(fileName).OrderByDescending(container => container.IsKnow).ToList();
    _wordLimit = wordLimit;

    _fileName = fileName;
  }

  public List<Container> GetWords(Func<Container, bool> expression)
  {
    return _containers.Where(expression).Take(_wordLimit).ToList();
  }

  public void MarkAsKnow(List<Container> containers)
  {
    foreach (Container container in containers)
    {
      container.IsKnow = true;
    }
  }

  public void MarkAllAsUnknown()
  {
    foreach (Container container in _containers)
    {
      container.IsKnow = false;
    }
  }
  
  public void Save() => SaveWords();
  
  private List<Container> LoadWords(string fileName)
  {
    using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
    {
      using (StreamReader reader = new StreamReader(stream))
      {
        string json = reader.ReadToEnd();

        JsonSerializerOptions options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        List<Container> containers;
        try
        {
          containers = JsonSerializer.Deserialize<List<Container>>(json, options)!;

        }
        catch
        {
          containers = new List<Container>();
        }
        return containers;
      }
    }
  }

  private void SaveWords()
  {
    string serialized = JsonSerializer.Serialize(_containers);

    using (StreamWriter writer = new StreamWriter(_fileName))
    {
      writer.Write(serialized);
    }
  }
}