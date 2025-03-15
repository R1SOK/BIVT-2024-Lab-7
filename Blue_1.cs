using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_7
{
    public class Blue_1
    {
        public class Response
        {
            private string _name;
            protected int _votes;

            public string Name => _name;
            public int Votes => _votes;

            public Response(string name)
            {
                _name = name;
                _votes = 0;
            }

            public virtual int CountVotes(Response[] responses)
            {
                _votes = responses.Count(r => r.Name == _name);
                return _votes;
            }

            public virtual void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Votes: {_votes}");
            }
        }

        public class HumanResponse : Response
        {
            private string _surname;

            public string Surname => _surname;

            public HumanResponse(string name, string surname) : base(name)
            {
                _surname = surname;
            }

            public override int CountVotes(Response[] responses)
            {
                _votes = responses
                    .OfType<HumanResponse>()
                    .Count(r => r.Name == Name && r.Surname == _surname);

                return _votes;
            }

            public override void Print()
            {
                Console.WriteLine($"Name: {Name}");
                Console.WriteLine($"Surname: {_surname}");
                Console.WriteLine($"Votes: {_votes}");
            }
        }
    }
}
