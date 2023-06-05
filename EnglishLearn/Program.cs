using System.Text;
using EnglishLearn;
using EnglishLearn.Extenstions;
using EnglishLearn.Models;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

Handler handler = new Handler("words.json");

Console.WriteLine("> Choose action: ");
Console.WriteLine("\t[1] Continue");
Console.WriteLine("\t[2] Start from begin");

int choose;

while ((choose = GetIntegerFromInput()) < 1 || choose > 2) { }

if (choose == 2)
{
  handler.MarkAllAsUnknown();
  handler.Save();
}

while (true)
{
  Console.Clear();

  List<Container> toLearn = handler.GetWords((container) => !container.IsKnow);

  if (toLearn.Count == 0)
  {
    Console.WriteLine("You know everything!");
    Console.WriteLine();
    Console.WriteLine("Press enter to start from begin...");
    Console.ReadLine();
    
    handler.MarkAllAsUnknown();
    handler.Save();
    
    continue;
  }
  
  ShowWords(toLearn);
  RepeatWords(toLearn);

  Console.WriteLine("> Choose action: ");
  Console.WriteLine("\t[1] Repeat this words");
  Console.WriteLine("\t[2] Continue");

  int userInput;

  while ((userInput = GetIntegerFromInput()) < 1 || userInput > 2) { }

  if (userInput == 1)
  {
    continue;
  }
  
  handler.MarkAsKnow(toLearn);
  handler.Save();
}

void ShowWords(List<Container> wordsToLearn)
{
  Console.WriteLine($"> Your {wordsToLearn.Count} words.");
  Console.WriteLine(string.Join("\n", wordsToLearn));

  Console.WriteLine();
  Console.WriteLine("Press enter to continue...");

  Console.ReadLine();
}

void RepeatWords(List<Container> wordsToLearn)
{
  List<Container> shuffledWords = wordsToLearn.Shuffle();
  
  while (shuffledWords.Count > 0)
  {
    Console.Clear();
    Console.WriteLine("---------------------------------");
  
    List<Container> incorrect = new List<Container>();
  
    foreach (Container container in shuffledWords)
    {
      string word = container.Word;
      string translate = container.Translate;
      bool isCorrect = true;
  
      switch (Random.Shared.Next(0, 2))
      {
        case 0:
          int randomIndex = Random.Shared.Next(0, container.Word.Length);
  
          char replacedLetter = word[randomIndex];
          string replaced = word[..randomIndex] + "*" + word[(randomIndex + 1)..];
  
          Console.WriteLine($"{replaced} - {translate}");
          Console.Write("Replaced letter is: ");
          string letterInput = Console.ReadLine()!;
  
          isCorrect = letterInput.Length == 1 && char.ToLower(letterInput[0]) == char.ToLower(replacedLetter);
  
          break;
        case 1:
          Console.WriteLine(word);
          Console.Write("Translate is: ");
          string translateInput = Console.ReadLine()!;
  
          isCorrect = string.Equals(translateInput.Trim(), translate, StringComparison.CurrentCultureIgnoreCase);
  
          break;
      }
  
      Console.WriteLine();
      if (isCorrect)
      {
        Console.WriteLine("Yeah, you are right!");
      }
      else
      {
        Console.WriteLine("No ;(");
        incorrect.Add(container);
      }
  
      Console.WriteLine();
    }
  
    shuffledWords = incorrect;
  }
}

int GetIntegerFromInput()
{
  int result;
  
  while (!int.TryParse(Console.ReadLine(), out result)) { }

  return result;
}