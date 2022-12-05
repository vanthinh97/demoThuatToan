// See https://aka.ms/new-console-template for more information
using demoDictionary;

Console.WriteLine("Hello, World!");

var diction = new keyword();

var key = diction.MappingLanguageKey("LOG_JoiningExamTestReminderAction");

Console.WriteLine("1: " + key);

var key2 = diction.MappingLanguageKey("bổ sung moi");
var authorId = 0;
int.TryParse(key2, out authorId);

Console.WriteLine("2: " + authorId);

List<int> intList = new List<int> {};
var parseIntToStringList = string.Join(",", intList);
Console.WriteLine("3: " + parseIntToStringList);

if (!string.IsNullOrEmpty(parseIntToStringList))
{
    var numbers = parseIntToStringList?.Split(',')?.Select(int.Parse)?.ToList();
    if (numbers != null && numbers.Any())
    {
        foreach (var item in numbers)
        {
            Console.WriteLine(item);
        }
    }
}

var abc = intList.Where(x => x < 0);
Console.WriteLine("4. " + abc);