using System;
using System.Linq;

namespace Lab_7
{
    public class Blue_2
    {
        public abstract class WaterJump
        {
            private string _name; // Название турнира
            private int _bank; // Призовой фонд
            private Participant[] _participants; // Массив участников
            private int _count;

            public string Name => _name;
            public int Bank => _bank;
            public Participant[] Participants => _participants;

            public abstract double[] Prize { get; } // Абстрактное свойство призовых мест

            public WaterJump(string name, int bank)
            {
                _name = name;
                _bank = bank;
                _participants = new Participant[0];
                _count = 0;
            }

            // Метод для добавления одного участника
            public void Add(Participant participant)
            {
                if (participant == null) return;
                Array.Resize(ref _participants, _count + 1);
                _participants[_count] = participant;
                _count++;
            }

            // Метод для добавления нескольких участников
            public void Add(params Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;
                int newSize = _count + participants.Length;
                Array.Resize(ref _participants, newSize);
                Array.Copy(participants, 0, _participants, _count, participants.Length);
                _count = newSize;
            }
        }

        public class WaterJump3m : WaterJump
        {
            public WaterJump3m(string name, int bank) : base(name, bank) { }

            public override double[] Prize
            {
                get
                {
                    if (Participants.Length < 3) return new double[0];
                    Participant.Sort(Participants);
                    return new double[] { Bank * 0.5, Bank * 0.3, Bank * 0.2 };
                }
            }
        }

        public class WaterJump5m : WaterJump
        {
            public WaterJump5m(string name, int bank) : base(name, bank) { }

            public override double[] Prize
            {
                get
                {
                    if (Participants.Length < 3) return new double[0];
                    Participant.Sort(Participants);

                    // Распределяем 80% фонда для первых трех мест

                    double[] prizes = new double[Participants.Length];

                    prizes[0] = Bank * 0.4;
                    prizes[1] = Bank * 0.25;
                    prizes[2] = Bank * 0.15;

                    // Распределяем оставшиеся 20% среди верхней половины (кроме первых трех)
                    int topCount = Math.Max(3, Math.Min(10, Participants.Length / 2));
                    double sharedPrize = Bank * 0.2 / (topCount - 3); // Делим 20% на оставшихся участников
                    for (int i = 3; i < topCount; i++)
                    {
                        prizes[i] = sharedPrize;
                    }

                    return prizes.Take(Participants.Length).ToArray();
                }
            }
        }

        public class Participant
        {
            private string _name;
            private string _surname;
            private int[,] _marks;
            private int _index;

            public string Name => _name;
            public string Surname => _surname;

            public int[,] Marks => _marks.Clone() as int[,];

            public int TotalScore
            {
                get
                {
                    int sum = 0;

                    foreach (int mark in _marks)
                    {
                        sum += mark;
                    }

                    return sum;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[2, 5];
                _index = 0;
            }

            public void Jump(int[] result)
            {
                if (result == null || result.Length != 5 || _index >= 2) return;
                for (int i = 0; i < 5; i++)
                {
                    _marks[_index, i] = result[i];
                }
                _index++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length == 0) return;
                Array.Sort(array, (p1, p2) => p2.TotalScore.CompareTo(p1.TotalScore));
            }
        }
    }
}