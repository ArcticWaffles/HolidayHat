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
            var people = Enum.GetValues(typeof(Person)).Cast<Person>();

            Dictionary<Person, Person> results = null;

            while (results == null)
            {
                results = AssignRecipients(people);
            }

            foreach (Person person in people)
            {
                Debug.WriteLine($"{person} is gifting to {results[person]}");
            }
        }

        static Dictionary<Person, Person> AssignRecipients(IEnumerable<Person> people)
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
                    if (recipients.Count == 1 && recipients[0] == gifter)
                    {
                        return null;
                    }
                    recipient = recipients[random.Next(recipients.Count)];
                }
                while (recipient == gifter);
                recipients.Remove(recipient);
                results.Add(gifter, recipient);
            }
            return results;
        }
    }
}
