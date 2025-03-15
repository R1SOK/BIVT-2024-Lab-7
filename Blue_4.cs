using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_7
{
    public class Blue_4
    {
        public abstract class Team
        {
            protected string name;
            protected int[] scores;

            public string Name => name;
            public int[] Scores
            {
                get
                {
                    if (scores == null) return null;
                    int[] copy = new int[scores.Length];
                    Array.Copy(scores, copy, copy.Length);

                    return copy;
                }
            }

            public int TotalScore => scores?.Sum() ?? 0;

            protected Team(string name)
            {
                this.name = name;
                this.scores = new int[0];
            }

            public abstract void Print();

            public void PlayMatch(int result)
            {
                int[] newScores = new int[scores.Length + 1];
                Array.Copy(scores, newScores, scores.Length);
                newScores[newScores.Length - 1] = result;
                scores = newScores;
            }
        }

        public class ManTeam : Team
        {
            public ManTeam(string name) : base(name) { }

            public override void Print()
            {
                Console.WriteLine($"Мужская команда: {Name}, Очки: {TotalScore}");
            }
        }

        public class WomanTeam : Team
        {
            public WomanTeam(string name) : base(name) { }

            public override void Print()
            {
                Console.WriteLine($"Женская команда: {Name}, Очки: {TotalScore}");
            }
        }

        public class Group
        {
            private string name;
            private ManTeam[] manTeams;
            private WomanTeam[] womanTeams;

            public string Name => name;
            public ManTeam[] ManTeams => manTeams;
            public WomanTeam[] WomanTeams => womanTeams;

            public Group(string name)
            {
                this.name = name;
                this.manTeams = new ManTeam[12];
                this.womanTeams = new WomanTeam[12];
            }

            public void Add(Team team)
            {
                if (team is ManTeam manTeam)
                {
                    AddToArray(ref manTeams, manTeam);
                }
                else if (team is WomanTeam womanTeam)
                {
                    AddToArray(ref womanTeams, womanTeam);
                }
            }

            private void AddToArray<T>(ref T[] array, T team) where T : Team
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null)
                    {
                        array[i] = team;
                        return;
                    }
                }
            }

            public void Add(params Team[] teams)
            {
                foreach (var team in teams)
                {
                    Add(team);
                }
            }

            public void Sort()
            {
                SortArray(ref manTeams);
                SortArray(ref womanTeams);
            }

            private void SortArray<T>(ref T[] array) where T : Team
            {
                Array.Sort(array, (x, y) => (y?.TotalScore ?? 0).CompareTo(x?.TotalScore ?? 0));
            }

            public static Group Merge(Group group1, Group group2, int size)
            {
                Group finalists = new Group("Финалисты");

                MergeTeams(ref finalists.manTeams, group1.ManTeams, group2.ManTeams, size);
                MergeTeams(ref finalists.womanTeams, group1.WomanTeams, group2.WomanTeams, size);

                return finalists;
            }

            private static void MergeTeams<T>(ref T[] resultArray, T[] array1, T[] array2, int size) where T : Team
            {
                var merged = array1.Concat(array2)
                                   .Where(t => t != null)
                                   .OrderByDescending(t => t.TotalScore)
                                   .Take(size)
                                   .ToArray();

                for (int i = 0; i < merged.Length; i++)
                {
                    resultArray[i] = merged[i];
                }
            }

            public void Print() { }
        }
    }
}