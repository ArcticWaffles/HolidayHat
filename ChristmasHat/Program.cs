using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristmasHat
{
    class Program
    {
        enum Person
        {
            Andrew,
            Christie,
            Ron,
            Dianne,
            John,
            Tisa
        }

        static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var results2019 = new Dictionary<Person, Person>()
            {
                {Person.Christie, Person.Ron },
                {Person.Andrew, Person.Christie },
                {Person.Ron, Person.John },
                {Person.Dianne, Person.Andrew },
                {Person.John, Person.Tisa },
                {Person.Tisa, Person.Dianne },
            };

            var people = Enum.GetValues(typeof(Person)).Cast<Person>().ToList();
            people.Remove(Person.Andrew);

            Dictionary<Person, Person> results = null;

            while (results == null)
                results = AssignRecipients(people, results2019);

            foreach (Person person in people)
                Debug.WriteLine($"{person} is gifting to {results[person]}");
        }

        static Dictionary<Person, Person> AssignRecipients(IEnumerable<Person> people,
                                                           Dictionary<Person, Person> lastYearsResults)
        {
            var gifters = new List<Person>(people);
            var recipients = new List<Person>(people);
            var results = new Dictionary<Person, Person>();

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
    }
}

//TODO: fix issue where there are 2 recipients left and it gets stuck because one is the gifter and the other is last year's recipient
