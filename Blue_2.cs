using System;
using System.Linq;

namespace Lab_6
{
    public class Blue_2
    {
        public abstract class WaterJump
        {
            private string _tournamentName;
            private int _prizeFund;
            private List<Participant> _participants;

            public string TournamentName => _tournamentName;
            public int PrizeFund => _prizeFund;
            public IReadOnlyList<Participant> Participants => _participants;

            protected WaterJump(string tournamentName, int prizeFund)
            {
                _tournamentName = tournamentName;
                _prizeFund = prizeFund;
                _participants = new List<Participant>();
            }

            public void Add(Participant participant)
            {
                _participants.Add(participant);
            }

            public void Add(IEnumerable<Participant> participants)
            {
                _participants.AddRange(participants);
            }

            public abstract void StartCompetition();
            public abstract double[] Prize { get; }
        }

        public class WaterJump3m : WaterJump
        {
            public WaterJump3m(string tournamentName, int prizeFund) : base(tournamentName, prizeFund) { }

            public override void StartCompetition()
            {
                var sortedParticipants = Participants.OrderByDescending(p => p.TotalScore).ToArray();
                Participant.Sort(sortedParticipants);
            }

            public override double[] Prize
            {
                get
                {
                    if (Participants.Count < 3)
                        return new double[0];

                    double[] prizes = new double[3];
                    prizes[0] = PrizeFund * 0.5;
                    prizes[1] = PrizeFund * 0.3;
                    prizes[2] = PrizeFund * 0.2;

                    return prizes;
                }
            }
        }

        public class WaterJump5m : WaterJump
        {
            public WaterJump5m(string tournamentName, int prizeFund) : base(tournamentName, prizeFund) { }

            public override void StartCompetition()
            {
                var sortedParticipants = Participants.OrderByDescending(p => p.TotalScore).ToArray();
                Participant.Sort(sortedParticipants);
            }

            public override double[] Prize
            {
                get
                {
                    if (Participants.Count < 3)
                        return new double[0];

                    int countAboveMid = Participants.Count / 2; 
                    countAboveMid = Math.Max(3, Math.Min(10, countAboveMid)); 

                    double[] prizes = new double[countAboveMid + 3]; 
                    double prizePerAboveMid = (PrizeFund * 0.2) / countAboveMid; 

                    for (int i = 0; i < countAboveMid; i++)
                    {
                        prizes[i] = prizePerAboveMid;
                    }

                    prizes[countAboveMid] = PrizeFund * 0.4; 
                    prizes[countAboveMid + 1] = PrizeFund * 0.25;
                    prizes[countAboveMid + 2] = PrizeFund * 0.15;

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

            public int[,] Marks
            {
                get
                {
                    if (_marks == null || _marks.GetLength(0) == 0 || _marks.GetLength(1) == 0) return null;

                    int[,] copymatrix = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < _marks.GetLength(1); j++)
                        {
                            copymatrix[i, j] = _marks[i, j];
                        }
                    }
                    return copymatrix;
                }
            }

            public int TotalScore
            {
                get
                {
                    if (_marks == null || _marks.GetLength(0) == 0 || _marks.GetLength(1) == 0) return 0;

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
                _name = name;
                _surname = surname;
                _marks = new int[2, 5];
                _index = 0;
            }

            public void Jump(int[] result)
            {
                if (_marks == null || result == null || result.Length == 0 || _index > 1) return;

                for (int i = 0; i < 5; i++)
                {
                    _marks[_index, i] = result[i];
                }
                _index++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length == 0) return;

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
