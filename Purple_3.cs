namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;
            private int _scoresRecorded;
            internal bool _scoresFilled;
            private bool _placesFilled;

            public string Name => _name;
            public string Surname => _surname;
            public double[]? Marks
            {
                get
                {
                    if (this._marks == null)
                        return null;
                    double[] copy = new double[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }

            public int[]? Places
            {
                get
                {
                    if (this._marks == null)
                        return null;
                    int[] copy = new int[_places.Length];
                    Array.Copy(_places, copy, _places.Length);
                    return copy;
                }
            }

            public int Score => this.Places != null ? this.Places.Sum() : 0;
            private double TotalMarks => this.Marks != null ? this.Marks.Sum() : 0;
            private int BestPlace => this.Places != null ? this.Places.Min() : 0;

            public void Print()
            {
                System.Console.WriteLine(
                    $"{this.Name}\t{this.Surname}\t\t{this.Score}\t{String.Join(", ", this.Places)}\t{this.TotalMarks}"
                );
            }

            public Participant(string name, string surname)
            {
                this._name = name;
                this._surname = surname;
                this._marks = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                this._places = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                this._scoresRecorded = 0;
                this._scoresFilled = false;
                this._placesFilled = false;
            }

            public void Evaluate(double result)
            {
                if (this._scoresRecorded >= 7)
                    return;
                if (result < 0.0 || result > 6.0)
                    return;
                if (this._marks == null)
                    return;
                this._marks[this._scoresRecorded++] = result;
                if (this._scoresRecorded >= 7)
                    this._scoresFilled = true;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int judge = 0; judge < 7; judge++)
                {
                    Participant[] markDescending = participants.OrderByDescending(p => p.Marks != null ? p.Marks[judge] : 0).ToArray();
                    Array.Copy(markDescending, participants, markDescending.Length);
                    for (int part = 0; part < participants.Length; part++)
                    {
                        if (participants[part].Places == null) continue;
                        if (judge >= participants[part].Places.Length) continue;
                        participants[part]._places[judge] = part + 1;
                    }
                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null)
                    return;
                if (array.Length < 2)
                    return;

                Participant[] sortedArray = array
                    .OrderBy(p => p.Score)
                    .ThenBy(p => p.BestPlace)
                    .ThenByDescending(p => p.TotalMarks)
                    .ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }
        }

        public abstract class Skating
        {
            private Participant[] _participants;
            protected double[] _judgeMood;

            public Participant[] Participants
            {
                get
                {
                    if (_participants == null)
                        return default;

                    return _participants;
                }
            }

            public double[] Moods
            {
                get
                {
                    if (_judgeMood == null)
                        return default;
                    double[] copy = new double[_judgeMood.Length];
                    Array.Copy(_judgeMood, copy, _judgeMood.Length);
                    return copy;
                }
            }

            public Skating(double[] judgeMood)
            {
                _participants = new Participant[0];
                _judgeMood = new double[7];
                if (judgeMood == null)
                    return;
                Array.Copy(judgeMood, _judgeMood, int.Min(judgeMood.Length, 7));

                ModificateMood();
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null)
                    return;

                if (_participants == null || _judgeMood == null)
                    return;

                if (_participants.Count(p => !p._scoresFilled) == 0)
                    return;

                double[] finalMarks = marks
                    .Zip(_judgeMood)
                    .Select(t => t.First * t.Second)
                    .ToArray();

                for (int i = 0; i < 7; i++)
                {
                    _participants.First(p => !p._scoresFilled).Evaluate(finalMarks[i]);
                }
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
        }

        public class FigureSkating : Skating
        {
            public FigureSkating(double[] judgeMood)
                : base(judgeMood) { }

            protected override void ModificateMood()
            {
                if (_judgeMood == null)
                    return;

                for (int i = 0; i < _judgeMood.Length; i++)
                {
                    _judgeMood[i] += ((double)i + 1.0) / 10.0;
                }
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] judgeMood)
                : base(judgeMood) { }

            protected override void ModificateMood()
            {
                if (_judgeMood == null)
                    return;

                for (int i = 0; i < _judgeMood.Length; i++)
                {
                    _judgeMood[i] *= 1.0 + (double)i / 100.0 + 0.01;
                }
            }
        }
    }
}
