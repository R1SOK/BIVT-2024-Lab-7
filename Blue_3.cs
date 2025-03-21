using System;
using System.Linq;

namespace Lab_7
{
    public class Blue_3
    {
        public class Participant
        {
            private readonly string name;
            private readonly string surname;
            protected int[] penaltyTimes;

            public string Name => name;
            public string Surname => surname;

            public int[] Penalties
            {
                get => penaltyTimes?.ToArray() ?? new int[0];
            }

            public int Total => penaltyTimes?.Sum() ?? 0;

            public virtual bool IsExpelled => penaltyTimes?.Contains(10) ?? false;

            public Participant(string name, string surname)
            {
                this.name = name;
                this.surname = surname;
                penaltyTimes = new int[0];
            }

            public virtual void PlayMatch(int time)
            {
                if (time < 0 || time > 10) return;

                Array.Resize(ref penaltyTimes, penaltyTimes.Length + 1);
                penaltyTimes[penaltyTimes.Length - 1] = time;
            }

            public static void Sort(Participant[] array)
            {
                Array.Sort(array, (x, y) => x.Total.CompareTo(y.Total));
            }

            public void Print()
            {
                Console.WriteLine($"Name: {Name}, Surname: {Surname}, Total: {Total}, IsExpelled: {IsExpelled}");
            }
        }

        public class BasketballPlayer : Participant
        {
            private int[] fouls = new int[0];

            public BasketballPlayer(string name, string surname) : base(name, surname) { }

            public override bool IsExpelled
            {
                get
                {
                    if (fouls.Length == 0) return false;

                    int matchesWithFiveFouls = fouls.Count(f => f == 5);
                    double percentWithFiveFouls = (double)matchesWithFiveFouls / fouls.Length;
                    if (percentWithFiveFouls > 0.1) return true;

                    return fouls.Sum() > 2 * fouls.Length;
                }
            }

            public override void PlayMatch(int foulsInMatch)
            {
                if (foulsInMatch < 0 || foulsInMatch > 5) return;

                Array.Resize(ref fouls, fouls.Length + 1);
                fouls[^1] = foulsInMatch;
            }
        }

        public class HockeyPlayer : Participant
        {
            private static List<HockeyPlayer> allHockeyPlayers = new List<HockeyPlayer>();

            public HockeyPlayer(string name, string surname) : base(name, surname)
            {
                allHockeyPlayers.Add(this);
            }

            public override bool IsExpelled
            {
                get
                {
                    if (penaltyTimes?.Contains(10) ?? false) return true;

                    double totalPenaltyMinutes = penaltyTimes?.Sum() ?? 0;
                    double totalAllPenaltyMinutes = allHockeyPlayers.Sum(p => p.penaltyTimes?.Sum() ?? 0);
                    double averagePenalty = totalAllPenaltyMinutes / allHockeyPlayers.Count;

                    return totalPenaltyMinutes > 0.1 * averagePenalty;
                }
            }

            public override void PlayMatch(int penaltyTime)
            {
                if (penaltyTime < 0 || penaltyTime > 10) return;

                Array.Resize(ref penaltyTimes, penaltyTimes.Length + 1);
                penaltyTimes[^1] = penaltyTime;
            }

            public void RemoveFromList()
            {
                allHockeyPlayers.Remove(this);
            }
        }
    }
}
