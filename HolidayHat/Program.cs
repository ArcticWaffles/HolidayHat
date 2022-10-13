using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

var oldResults = ReadResultsFile();
var newResults = AssignNewRecipients(oldResults);
WriteResultsFile(newResults);
ShowResultsMessage();


static Results AssignNewRecipients(Results oldResults)
{
    Results? results = null;
    while (results == null)
        results = TryAssignNewRecipients(oldResults);
    return results;
}

static Results? TryAssignNewRecipients(Results oldResults)
{
    var gifters = new List<Person>(people);
    var recipients = new List<Person>(people);
    var random = new Random();
    var results = new Results();

    foreach (var gifter in gifters)
    {
        var invalidRecipients =
            new List<Person>() { gifter, oldResults[gifter] };
        var validRecipients = recipients.Except(invalidRecipients);

        if (!validRecipients.Any())
            return null;

        Person recipient;
        do
        {
            var randomIndex = random.Next(recipients.Count);
            recipient = recipients[randomIndex];
        }
        while (invalidRecipients.Contains(recipient));

        recipients.Remove(recipient);
        results.Add(gifter, recipient);
    }

    return results;
}

static Results ReadResultsFile()
{
    var lines = File.ReadAllLines(resultsFile);
    var json = string.Join(newline, lines);
    return JsonSerializer.Deserialize<Results>(json, jsonOptions)
           ?? throw new InvalidOperationException();
}

static void WriteResultsFile(Results results)
{
    var json = JsonSerializer.Serialize(results, jsonOptions);
    var resultsForFileWrite = json.Split(new string[] { newline },
                                         StringSplitOptions.RemoveEmptyEntries);
    File.WriteAllLines(resultsFile, resultsForFileWrite);
}

void ShowResultsMessage()
{
    foreach (Person person in people)
        WriteLine($"{person} is gifting to {newResults[person]}");

    var reminder = """

        ╔══════════════════════════════════════════════╗
        ║ ► Remember to push changes to results.json   ║
        ╚══════════════════════════════════════════════╝

        """;
    WriteLine(reminder);

    static void WriteLine(string message)
    {
        Console.WriteLine(message);
        Debug.WriteLine(message);
    }
}


enum Person { Christie, Ron, Dianne, John, Tisa }

class Results : Dictionary<Person, Person> { }

partial class Program
{
    static readonly string newline = Environment.NewLine;
    static readonly string resultsFile = @"..\..\..\results.json";
    static readonly JsonSerializerOptions jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true
    };
    static readonly List<Person> people =
        Enum.GetValues(typeof(Person)).Cast<Person>().ToList();
}