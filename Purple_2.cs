namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int[] _marks;
            private int _distance;
            internal bool _hasJumped;
            private int _target = 0;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;

            public int[]? Marks
            {
                get
                {
                    if (this._marks == null)
                        return null;
                    int[] copy = new int[5];
                    Array.Copy(_marks, copy, 5);
                    return copy;
                }
            }

            public int Result
            {
                get
                {
                    if (this.Marks == null || !_hasJumped)
                        return 0;
                    int stylePoints = 0;
                    int bestMark = int.MinValue;
                    int worstMark = int.MaxValue;

                    for (int judge = 0; judge < 5; judge++)
                    {
                        int currentMark = Marks[judge];
                        if (currentMark > bestMark)
                            bestMark = currentMark;
                        if (currentMark < worstMark)
                            worstMark = currentMark;

                        stylePoints += currentMark;
                    }

                    stylePoints -= worstMark;
                    stylePoints -= bestMark;

                    int distanceUpToTarget = _distance - _target;
                    int distancePoints = 60 - distanceUpToTarget * 2;

                    return int.Max(0, stylePoints + distancePoints);
                }
            }

            public void Print() { }

            public Participant(string name, string surname)
            {
                this._name = name;
                this._surname = surname;
                this._marks = new int[5] { 0, 0, 0, 0, 0 };
                this._distance = 0;
                this._hasJumped = false;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (this._hasJumped)
                    return;
                if (marks == null)
                    return;
                if (this.Marks == null)
                    return;
                if (marks.Length < 5)
                    return;

                if (distance < 0)
                    return;
                if (target < 0)
                    return;

                this._distance = distance;
                this._hasJumped = true;
                _target = target;
                Array.Copy(marks, this._marks, 5);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null)
                    return;

                if (array.Length <= 1)
                    return;

                Participant[] sorted = array.OrderByDescending(p => p.Result).ToArray();
                Array.Copy(sorted, array, sorted.Length);
            }
        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _distance;
            private Participant[] _participants;

            public string Name => _name;
            public int Standard => _distance;
            public Participant[] Participants => _participants;

            public SkiJumping(string name, int distance)
            {
                _name = name;
                _distance = distance;
                _participants = new Participant[0];
            }

            public void Add(Participant jumper)
            {
                if (_participants == null)
                    return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = jumper;
            }

            public void Add(Participant[] jumpers)
            {
                if (jumpers == null)
                    return;
                foreach (var jumper in jumpers)
                {
                    Add(jumper);
                }
            }

            public void Jump(int distance, int[] marks)
            {
                if (_participants == null)
                    return;

                if (_participants.Count(p => !p._hasJumped) == 0)
                    return;
                _participants.First(p => !p._hasJumped).Jump(distance, marks, _distance);
            }

            public void Print() { }
        }

        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping()
                : base("100m", 100) { }
        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping()
                : base("150m", 150) { }
        }
    }
}
