using System;
using System.Security.Cryptography;
using static Lab_7.Blue_5;

namespace Lab_7
{
    public class Blue_5
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private int _place;

            public string Name { get { return _name; } }
            public string Surname { get { return _surname; } }
            public int Place { get { return _place; } }

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _place = 0;
            }

            public void SetPlace(int place)
            {
                if (_place != 0) return;
                _place = place;
            }

            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} - Место: {Place}");
            }
        }

        public abstract class Team
        {
            protected string _name;
            protected Sportsman[] _sportsmen;
            protected int _count;

            public string Name { get { return _name; } }
            public Sportsman[] Sportsmen { get { return _sportsmen; } }


            public int SummaryScore
            {
                get
                {
                    if (_sportsmen == null || _count == 0)
                        return 0;

                    int total = 0;
                    for (int i = 0; i < _count; i++)
                    {
                        switch (_sportsmen[i].Place)
                        {
                            case 1: total += 5; break;
                            case 2: total += 4; break;
                            case 3: total += 3; break;
                            case 4: total += 2; break;
                            case 5: total += 1; break;
                            default: break;
                        }
                    }
                    return total;
                }
            }

            public int TopPlace
            {
                get
                {
                    if (_sportsmen == null || _sportsmen.Length == 0) return 18;

                    int topPlace = 18;
                    //return topPlace == 18 ? 0 : topPlace;
                    for (int i = 0; i < _sportsmen.Length; i++)
                    {
                        if (_sportsmen[i] != null && _sportsmen[i].Place > 0 && _sportsmen[i].Place < topPlace) 
                            topPlace = _sportsmen[i].Place;
                    }
                    return topPlace;
                }
            }

            public Team(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[6];
                _count = 0;
            }

            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null || sportsman == null || _sportsmen.Length == 0 || _count >= _sportsmen.Length) 
                    return;
                _sportsmen[_count] = sportsman;
                _count++;
            }

            public void Add(Sportsman[] sportsman)
            {
                if (_sportsmen == null || sportsman == null || sportsman.Length == 0 || _sportsmen.Length == 0 || _count >= _sportsmen.Length) 
                    return;
                int i = 0;
                while (_count < _sportsmen.Length && i < sportsman.Length)
                {
                    if (sportsman[i] != null)
                    {
                        _sportsmen[_count] = sportsman[i];
                        _count++;
                        i++;
                    }
                }
            }

            public static void Sort(Team[] teams)
            {
                if (teams == null || teams.Length == 0) return;

                for (int i = 1, j = 2; i < teams.Length;)
                {
                    if (i == 0 || teams[i - 1].SummaryScore > teams[i].SummaryScore)
                    {
                        i = j;
                        j++;
                    }
                    else if (teams[i - 1].SummaryScore == teams[i].SummaryScore && teams[i - 1].TopPlace <= teams[i].TopPlace)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        Team temp = teams[i];
                        teams[i] = teams[i - 1];
                        teams[i - 1] = temp;
                        i--;
                    }
                }
            }

            protected abstract double GetTeamStrength();
            public static Team GetChampion(Team[] teams)
            {
                if (teams == null || teams.Length == 0) return null;

                double max = 0;
                if (teams[0] != null) max = teams[0].GetTeamStrength();

                int Imax = 0;
                for (int i = 0; i < teams.Length; i++)
                {
                    if (teams[i] != null && teams[i].GetTeamStrength() > max)
                    {
                        max = teams[i].GetTeamStrength();
                        Imax = i;
                    }
                }
                return teams[Imax];
            }


            public abstract void Print();
        }

        public class ManTeam : Team
        {
            public ManTeam(string name) : base(name) { }

            protected override double GetTeamStrength()
            {
                if (_count == 0) return 0;

                double plases = 0;
                int count = 0;
                for (int i = 0; i < Sportsmen.Length; i++)
                {
                    if (Sportsmen[i] != null)
                    {
                        plases += Sportsmen[i].Place;
                        count++;
                    }
                }
                if (plases == 0) { throw new Exception("invalid count"); }
                return 100 * count  / plases ;

            }

            public override void Print()
            {
                Console.WriteLine($"Мужская команда: {Name}");
                for (int i = 0; i < _count; i++)
                    _sportsmen[i].Print();
            }
        }

        public class WomanTeam : Team
        {
            public WomanTeam(string name) : base(name) { }

            protected override double GetTeamStrength()
            {
                if (_count == 0) return 0;

                double plases = 0;
                int count = 0;
                double firsts = 1;
                for (int i = 0; i < Sportsmen.Length; i++)
                {
                    if (Sportsmen[i] != null)
                    {
                        plases += Sportsmen[i].Place;
                        firsts *= Sportsmen[i].Place;
                        count++;
                    }
                }
                return 100 * plases * count / firsts;
            }

            public override void Print()
            {
                Console.WriteLine($"Женская команда: {Name}");
                for (int i = 0; i < _count; i++)
                    _sportsmen[i].Print();
            }
        }
    }
}
