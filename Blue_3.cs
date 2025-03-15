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
            protected int[] penaltyTimes; // Уровень доступа повышен до protected

            public string Name => name;
            public string Surname => surname;

            public int[] Penalties // Переименовано в Penalties
            {
                get
                {
                    if (penaltyTimes == null) return null;
                    int[] copy = new int[penaltyTimes.Length];
                    Array.Copy(penaltyTimes, copy, copy.Length);
                    return copy;
                }
            }

            public int Total // Переименовано в Total
            {
                get
                {
                    if (penaltyTimes == null) return 0;

                    int total = 0;
                    foreach (int time in penaltyTimes)
                    {
                        total += time;
                    }
                    return total;
                }
            }

            public virtual bool IsExpelled // Сделано виртуальным
            {
                get
                {
                    if (penaltyTimes == null)
                        return false;

                    foreach (int time in penaltyTimes)
                    {
                        if (time == 10) return true;
                    }
                    return false;
                }
            }

            public Participant(string name, string surname)
            {
                this.name = name;
                this.surname = surname;
                penaltyTimes = new int[0];
            }

            public virtual void PlayMatch(int time) // Сделано виртуальным
            {
                if (penaltyTimes == null) return;

                int[] newArray = new int[penaltyTimes.Length + 1];
                Array.Copy(penaltyTimes, newArray, penaltyTimes.Length);
                penaltyTimes = newArray;
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
            private int[] fouls; // Массив для хранения фолов
            private int foulsCount; // Количество добавленных фолов

            public BasketballPlayer(string name, string surname) : base(name, surname)
            {
                fouls = new int[0];
                foulsCount = 0;
            }

            public override bool IsExpelled
            {
                get
                {
                    if (foulsCount == 0) return false;

                    // Условие 1: более 10% матчей с 5 фолами

                    int matchesWithFiveFouls = 0;
                    for (int i = 0; i < foulsCount; i++)
                    {
                        if (fouls[i] == 5) matchesWithFiveFouls++;
                    }
                    double percentWithFiveFouls = (double)matchesWithFiveFouls / foulsCount;
                    if (percentWithFiveFouls > 0.1) return true;


                    int totalFouls = 0;
                    for (int i = 0; i < foulsCount; i++)
                    {
                        totalFouls += fouls[i];
                    }
                    if (totalFouls > 2 * foulsCount) return true; // Условие 2: сумма фолов больше, чем удвоенное количество матчей

                    return false;
                }
            }

            public override void PlayMatch(int foulsInMatch)
            {
                if (foulsInMatch < 0 || foulsInMatch > 5) return; 

                int[] newArray = new int[foulsCount + 1];
                Array.Copy(fouls, newArray, foulsCount);
                fouls = newArray;
                fouls[foulsCount] = foulsInMatch;
                foulsCount++;
            }
        }

        public class HockeyPlayer : Participant
        {
            private static HockeyPlayer[] allHockeyPlayers = new HockeyPlayer[0];
            private static int playersCount = 0; 

            public HockeyPlayer(string name, string surname) : base(name, surname)
            {
                Array.Resize(ref allHockeyPlayers, playersCount + 1);
                allHockeyPlayers[playersCount] = this;
                playersCount++;
            }

            public override bool IsExpelled
            {
                get
                {
                    if (penaltyTimes != null)
                    {
                        for (int i = 0; i < penaltyTimes.Length; i++)
                        {
                            if (penaltyTimes[i] == 10) return true; // Условие 1: хотя бы в одном матче было 10 минут штрафа
                        }
                    }
                    
                    double totalPenaltyMinutes = 0;
                    if (penaltyTimes != null)
                    {
                        for (int i = 0; i < penaltyTimes.Length; i++)
                        {
                            totalPenaltyMinutes += penaltyTimes[i];
                        }
                    }

                    double totalAllPenaltyMinutes = 0;
                    for (int i = 0; i < playersCount; i++)
                    {
                        if (allHockeyPlayers[i].penaltyTimes != null)
                        {
                            for (int j = 0; j < allHockeyPlayers[i].penaltyTimes.Length; j++)
                            {
                                totalAllPenaltyMinutes += allHockeyPlayers[i].penaltyTimes[j];
                            }
                        }
                    }

                    double averagePenalty = totalAllPenaltyMinutes / playersCount;
                    if (totalPenaltyMinutes > 0.1 * averagePenalty) return true;

                    return false;
                }
            }

            public override void PlayMatch(int penaltyTime)
            {
                if (penaltyTime < 0 || penaltyTime > 10) return; 

                int[] newArray = new int[penaltyTimes.Length + 1];
                Array.Copy(penaltyTimes, newArray, penaltyTimes.Length);
                penaltyTimes = newArray;
                penaltyTimes[penaltyTimes.Length - 1] = penaltyTime;
            }

            public void RemoveFromList()
            {
                int index = -1;
                for (int i = 0; i < playersCount; i++)
                {
                    if (allHockeyPlayers[i] == this)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1) return;

                for (int i = index; i < playersCount - 1; i++)
                {
                    allHockeyPlayers[i] = allHockeyPlayers[i + 1];
                }
                Array.Resize(ref allHockeyPlayers, playersCount - 1);
                playersCount--;
            }
        }
    }
}