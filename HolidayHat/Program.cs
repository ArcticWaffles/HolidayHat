using System.Diagnostics;

var random = new Random();
var results2022 = new Results()
{
    {Person.Christie, Person.Tisa },
    {Person.Ron, Person.Dianne },
    {Person.Dianne, Person.Ron },
    {Person.John, Person.Christie },
    {Person.Tisa, Person.John },
};

var people = Enum.GetValues(typeof(Person)).Cast<Person>().ToList();

Results? results = null;

while (results == null)
    results = AssignRecipients(people, results2022);

foreach (Person person in people)
    Debug.WriteLine($"{person} is gifting to {results[person]}");

Results? AssignRecipients(IEnumerable<Person> people, Results lastYearsResults)
{
    var gifters = new List<Person>(people);
    var recipients = new List<Person>(people);
    var results = new Results();

    foreach (var gifter in gifters)
    {
        Person recipient;
        do
        {
            var invalidRecipients = new List<Person>() { gifter, lastYearsResults[gifter] };
            var validRecipients = recipients.Except(invalidRecipients);

            if (!validRecipients.Any()) return null;

            recipient = recipients[random.Next(recipients.Count)];
        }
        while (recipient == gifter || lastYearsResults[gifter] == recipient);
        recipients.Remove(recipient);
        results.Add(gifter, recipient);
    }
    return results;
}


enum Person { Christie, Ron, Dianne, John, Tisa }
class Results : Dictionary<Person, Person> { }