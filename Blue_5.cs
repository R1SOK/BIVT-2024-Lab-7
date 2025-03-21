using System;

namespace Lab_7
{
    public class Blue_5
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private int _place;

            public string Name => _name;
            public string Surname => _surname;
            public int Place => _place;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _place = 0;
            }

            public void SetPlace(int place)
            {
                if (_place == 0)
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

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

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
                    if (_count == 0) return 0;

                    int topPlace = 18;
                    for (int i = 0; i < _count; i++)
                    {
                        int currentPlace = _sportsmen[i].Place;
                        if (currentPlace != 0 && currentPlace < topPlace)
                            topPlace = currentPlace;
                    }
                    return topPlace == 18 ? 0 : topPlace;
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
                if (_count < 6)
                {
                    _sportsmen[_count] = sportsman;
                    _count++;
                }
            }

            public void Add(params Sportsman[] newSportsmen)
            {
                foreach (var sportsman in newSportsmen)
                    Add(sportsman);
            }

            public static void Sort(Team[] teams)
            {
                if (teams == null) return;

                Array.Sort(teams, (x, y) =>
                {
                    int scoreCompare = y.SummaryScore.CompareTo(x.SummaryScore);
                    return scoreCompare != 0 ? scoreCompare : x.TopPlace.CompareTo(y.TopPlace);
                });
            }

            protected abstract double GetTeamStrength();
            public static Team GetChampion(Team[] teams)
            {
                if (teams == null || teams.Length == 0) return null;

                Team champion = teams[0];
                foreach (var team in teams)
                {
                    if (team.GetTeamStrength() > champion.GetTeamStrength())
                        champion = team;
                }
                return champion;
            }

            public abstract void Print();
        }

        public class ManTeam : Team
        {
            public ManTeam(string name) : base(name) { }

            protected override double GetTeamStrength()
            {
                if (_count == 0) return 0;

                double sum = 0;
                for (int i = 0; i < _count; i++)
                    sum += _sportsmen[i].Place;

                return sum == 0 ? 0 : 100 / (sum / _count);
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

                double sum = 0, product = 1;
                for (int i = 0; i < _count; i++)
                {
                    int place = _sportsmen[i].Place;
                    sum += place;
                    product *= place;
                }

                return product == 0 ? 0 : 100 * (sum * _count) / product;
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
