using System;
using System.Linq;

namespace Lab_7
{
    public class Blue_2
    {
        public abstract class WaterJump
        {
            private string _name;
            private int _bank;
            private Participant[] _participants;
            private int _participantCount;

            public string Name => _name;
            public int Bank => _bank;
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null)
                        return new Participant[0];
                    return _participants.Take(_participantCount).ToArray();
                }
            }

            protected WaterJump(string name, int bank)
            {
                _name = name ?? throw new ArgumentNullException(nameof(name));
                _bank = bank;
                _participants = new Participant[10];
                _participantCount = 0;
            }

            public void Add(Participant participant)
            {
                if (_participantCount >= _participants.Length)
                {
                    Array.Resize(ref _participants, _participants.Length * 2);
                }
                _participants[_participantCount++] = participant;
            }

            public void Add(Participant[] participants)
            {
                if (participants == null)
                    throw new ArgumentNullException(nameof(participants));

                foreach (var participant in participants)
                {
                    Add(participant);
                }
            }

            public abstract double[] Prize { get; }
        }

        public class WaterJump3m : WaterJump
        {
            public WaterJump3m(string name, int bank) : base(name, bank) { }

            public override double[] Prize
            {
                get
                {
                    if (Participants == null || Participants.Length < 3) return new double[0];
                    return new double[]
                    {
                        Bank * 0.5, // 50% за первое место
                        Bank * 0.3, // 30% за второе место
                        Bank * 0.2  // 20% за третье место
                    };
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
                    if (Participants == null || Participants.Length < 3)
                        return new double[0]; 

                    int countAboveMid = Participants.Length / 2;
                    countAboveMid = Math.Max(3, Math.Min(10, countAboveMid)); 

                    double[] prizes = new double[countAboveMid + 3];

                    double prizePerAboveMid = (Bank * 0.2) / countAboveMid;
                    for (int i = 0; i < countAboveMid; i++)
                    {
                        prizes[i] = prizePerAboveMid;
                    }

                    prizes[countAboveMid] = Bank * 0.4;     // 40% за первое место
                    prizes[countAboveMid + 1] = Bank * 0.25; // 25% за второе место
                    prizes[countAboveMid + 2] = Bank * 0.15; // 15% за третье место

                    return prizes;
                }
            }
        }


        public struct Participant
        {
            private string _name;
            private string _surname;
            private int[,] _marks;
            private int _index;

            public string Name => _name;
            public string Surname => _surname;

            public int[,] Marks => (int[,])_marks.Clone();

            public int TotalScore
            {
                get
                {
                    int sum = 0;
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < _marks.GetLength(1); j++)
                        {
                            sum += _marks[i, j];
                        }
                    }
                    return sum;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name ?? throw new ArgumentNullException(nameof(name));
                _surname = surname ?? throw new ArgumentNullException(nameof(surname));
                _marks = new int[2, 5];
                _index = 0;
            }

            public void Jump(int[] result)
            {
                if (result == null || result.Length != 5 || _index > 1)
                    throw new ArgumentException("Invalid jump result");

                for (int i = 0; i < 5; i++)
                {
                    _marks[_index, i] = result[i];
                }
                _index++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));

                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j + 1].TotalScore > array[j].TotalScore)
                        {
                            (array[j + 1], array[j]) = (array[j], array[j + 1]);
                        }
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{_surname} {_name} - Total Score: {TotalScore}");
            }
        }
    }
}
