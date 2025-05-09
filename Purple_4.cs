namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;
            private bool _timeRecorded;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;
            internal bool TimeRecorded => _timeRecorded;

            public Sportsman(string name, string surname)
            {
                this._name = name;
                this._surname = surname;
                this._time = 0;
                this._timeRecorded = false;
            }

            public void Run(double time)
            {
                if (this._timeRecorded)
                    return;
                if (time < 0)
                    return;
                this._time = time;
                this._timeRecorded = true;
            }

            public void Print()
            {
                System.Console.WriteLine(
                    $"{this.Name} {this.Surname}, {(this.TimeRecorded ? this.Time.ToString() : "не бегал")}"
                );
            }

            public static void Sort(Sportsman[] sportsmen)
            {
                if (sportsmen == null)
                    return;
                if (sportsmen.Any(s => !s.TimeRecorded))
                    return;
                Sportsman[] sorted = sportsmen.OrderBy(s => s.Time).ToArray();
                Array.Copy(sorted, sportsmen, sorted.Length);
            }
        }

        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname)
                : base(name, surname) { }

            public SkiMan(string name, string surname, double time)
                : base(name, surname)
            {
                Run(time);
            }
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname)
                : base(name, surname) { }

            public SkiWoman(string name, string surname, double time)
                : base(name, surname)
            {
                Run(time);
            }
        }

        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            public Group(string name)
            {
                this._name = name;
                this._sportsmen = new Sportsman[] { };
            }

            public Group(Group otherGroup)
            {
                if (otherGroup._sportsmen == null)
                    return;
                this._name = otherGroup.Name;
                this._sportsmen = new Sportsman[] { };
                Array.Copy(otherGroup.Sportsmen, this._sportsmen, otherGroup.Sportsmen.Length);
            }

            public void Add(Sportsman sportsman)
            {
                if (this.Sportsmen == null)
                    return;
                Sportsman[] oldArray = this.Sportsmen;
                this._sportsmen = new Sportsman[oldArray.Length + 1];
                Array.Copy(oldArray, this._sportsmen, oldArray.Length);
                this._sportsmen[oldArray.Length] = sportsman;
            }

            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null)
                    return;

                if (this.Sportsmen == null)
                    return;
                Sportsman[] oldArray = this.Sportsmen;
                this._sportsmen = new Sportsman[oldArray.Length + sportsmen.Length];
                oldArray.CopyTo(this._sportsmen, 0);
                sportsmen.CopyTo(this._sportsmen, oldArray.Length);
            }

            public void Add(Group otherGroup)
            {
                if (this.Sportsmen == null || otherGroup.Sportsmen == null)
                    return;
                Sportsman[] oldArray = this.Sportsmen;
                Sportsman[] otherArrray = otherGroup.Sportsmen;

                this._sportsmen = new Sportsman[oldArray.Length + otherArrray.Length];

                oldArray.CopyTo(this._sportsmen, 0);
                otherArrray.CopyTo(this._sportsmen, oldArray.Length);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;
                Sportsman[] sorted = _sportsmen.OrderBy(s => s.TimeRecorded ? s.Time : double.MaxValue).ToArray();
                Array.Copy(sorted, _sportsmen, sorted.Length);
            }

            public static Group Merge(Group group1, Group group2)
            {
                if (group1.Sportsmen == null || group2.Sportsmen == null)
                    return default;
                Sportsman[] array1 = group1.Sportsmen;
                Sportsman[] array2 = group2.Sportsmen;

                Sportsman[] newArray = new Sportsman[array1.Length + array2.Length];

                int i1 = 0;
                int i2 = 0;
                int iTotal = 0;

                while (iTotal < newArray.Length && i1 < array1.Length && i2 < array2.Length)
                {
                    if (array1[i1].Time <= array2[i2].Time)
                    {
                        newArray[iTotal++] = array1[i1++];
                    }
                    else
                    {
                        newArray[iTotal++] = array2[i2++];
                    }
                }

                while (i1 < array1.Length)
                    newArray[iTotal++] = array1[i1++];
                while (i2 < array2.Length)
                    newArray[iTotal++] = array2[i2++];

                Group newGroup = new Group("Финалисты");
                newGroup.Add(newArray);

                return newGroup;
            }

            public void Print()
            {
                System.Console.WriteLine($"Группа '{this.Name}'");
                if (this.Sportsmen == null)
                    return;
                if (this.Sportsmen.Length == 0)
                    return;

                foreach (Sportsman s in this.Sportsmen)
                {
                    Console.Write("\t");
                    s.Print();
                }
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = new Sportsman[0];
                women = new Sportsman[0];
                if (_sportsmen == null)
                    return;
                men = _sportsmen.Where(s => s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null)
                    return;
                Sportsman.Sort(_sportsmen);
                Split(out Sportsman[] men, out Sportsman[] women);
                Sportsman[] newArray = new Sportsman[_sportsmen.Length];

                int iMen = 0;
                int iWomen = 0;
                int iTotal = 0;

                while (iMen < men.Length || iWomen < women.Length)
                {
                    if ((iTotal % 2 == 0 || iWomen >= women.Length) && iMen < men.Length)
                    {
                        newArray[iTotal++] = men[iMen++];
                    }
                    else if (iWomen < women.Length)
                    {
                        newArray[iTotal++] = women[iWomen++];
                    }
                }


                Array.Copy(newArray, this.Sportsmen, newArray.Length);
            }
        }
    }
}
