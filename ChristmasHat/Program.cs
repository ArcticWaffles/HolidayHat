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

        static void Main(string[] args)
        {
            var results2019 = new Dictionary<Person, Person>()
            {
                {Person.Christie, Person.Ron },
                {Person.Andrew, Person.Christie },
                {Person.Ron, Person.John },
                {Person.Tisa, Person.Dianne },
                {Person.Dianne, Person.Andrew },
                {Person.John, Person.Tisa },
            };

            var people = Enum.GetValues(typeof(Person)).Cast<Person>();

            Dictionary<Person, Person> results = null;

            while (results == null)
                results = AssignRecipients(people, results2019);

            foreach (Person person in people)
                Debug.WriteLine($"{person} is gifting to {results[person]}");
        }

        static Dictionary<Person, Person> AssignRecipients(IEnumerable<Person> people,
                                                           Dictionary<Person, Person> lastYearsResults)
        {
            var random = new Random();
            var gifters = new List<Person>(people);
            var recipients = new List<Person>(people);
            var results = new Dictionary<Person, Person>();

            foreach (var gifter in gifters)
            {
                Person recipient;
                do
                {
                    if (recipients.Count == 1 && (recipients[0] == gifter || lastYearsResults[gifter] == recipients[0]))
                        return null;

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
