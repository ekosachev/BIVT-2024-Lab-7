namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumpsRecorded;

            public string Name => _name;
            public string Surname => _surname;

            public void Print() { }

            public double[]? Coefs
            {
                get
                {
                    if (this._coefs == null)
                        return null;
                    double[] copy = new double[4];
                    this._coefs.CopyTo(copy, 0);
                    return copy;
                }
            }

            public int[,]? Marks
            {
                get
                {
                    if (this._marks == null)
                        return null;
                    int[,] copy = new int[4, 7];
                    Array.Copy(this._marks, copy, this._marks.Length);
                    return copy;
                }
            }

            public double TotalScore
            {
                get
                {
                    if (this.Marks == null || this.Coefs == null)
                        return 0.0;
                    double score = 0.0;
                    for (int jump = 0; jump < 4; jump++)
                    {
                        int worstMark = int.MaxValue;
                        int bestMark = int.MinValue;
                        int jumpMark = 0;
                        for (int judge = 0; judge < 7; judge++)
                        {
                            int currentMark = this.Marks[jump, judge];
                            if (currentMark > bestMark)
                                bestMark = currentMark;
                            if (currentMark < worstMark)
                                worstMark = currentMark;
                            jumpMark += currentMark;
                        }
                        jumpMark -= worstMark;
                        jumpMark -= bestMark;
                        score += jumpMark * this.Coefs[jump];
                    }
                    return score;
                }
            }

            public Participant(string name, string surname)
            {
                this._name = name;
                this._surname = surname;
                this._coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                this._marks = new int[,]
                {
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                };

                this._jumpsRecorded = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null)
                    return;
                if (coefs.Length != 4)
                    return;

                foreach (double coeff in coefs)
                {
                    if (coeff > 3.5 || coeff < 2.5)
                        return;
                }

                if (this._coefs == null)
                    this._coefs = new double[4];
                Array.Copy(coefs, this._coefs, 4);
            }

            public void Jump(int[] marks)
            {
                if (marks == null)
                    return;
                if (marks.Length != 7)
                    return;
                if (this._jumpsRecorded >= 4)
                    return;

                foreach (int mark in marks)
                {
                    if (mark > 6 || mark < 1)
                        return;
                }

                if (this._marks == null)
                    this._marks = new int[4, 7];
                for (int judge = 0; judge < 7; judge++)
                {
                    this._marks[this._jumpsRecorded, judge] = marks[judge];
                }

                this._jumpsRecorded++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null)
                    return;

                Participant[] sorted = array.OrderByDescending(p => p.TotalScore).ToArray();
                Array.Copy(sorted, array, sorted.Length);
            }
        }

        public class Judge
        {
            public string Name { get; private set; }
            private int[] _favMarks;
            private int _markIndexToReturn = 0;

            public Judge(string name, int[] favMarks)
            {
                Name = name;
                Array.Resize(ref _favMarks, favMarks.Length);
                Array.Copy(favMarks, _favMarks, favMarks.Length);
            }

            public int CreateMark()
            {
                if (_favMarks == null)
                    return default;

                return _favMarks[_markIndexToReturn++ % _favMarks.Length];
            }

            public void Print() { }
        }

        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges)
            {
                _judges = new Judge[judges.Length];
                Array.Copy(judges, _judges, judges.Length);
                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper)
            {
                if (_judges == null)
                    return;

                if (_judges.Length != 7)
                    return;

                int[] marks = _judges.Select(j => j.CreateMark()).ToArray();

                jumper.Jump(marks);
            }

            public void Add(Participant jumper)
            {
                if (_participants == null)
                    return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = jumper;
                Evaluate(jumper);
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

            public void Sort()
            {
                Participant.Sort(_participants);
            }
        }
    }
}
