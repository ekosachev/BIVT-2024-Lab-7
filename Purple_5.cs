namespace Lab_7
{
    public class Purple_5
    {
        public struct Response
        {
            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _characterTrait;
            public string Concept => _concept;

            public Response(string animal, string characterTrait, string concept)
            {
                this._animal = animal;
                this._characterTrait = characterTrait;
                this._concept = concept;
            }

            internal string GetAnswer(int question)
            {
                if (question > 3 || question < 1)
                    return "";
                switch (question)
                {
                    case 1:
                        return this.Animal;
                    case 2:
                        return this.CharacterTrait;
                    case 3:
                        return this.Concept;
                    default:
                        return "";
                }
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (responses == null)
                    return 0;
                if (questionNumber < 1 || questionNumber > 3)
                    return 0;

                string thisAnswer = this.GetAnswer(questionNumber);

                return responses
                    .Where(r => r.GetAnswer(questionNumber) != null)
                    .Where(r => r.GetAnswer(questionNumber) == thisAnswer)
                    .ToArray()
                    .Length;
            }

            static void SortByLength(Response[] responses, int questionNumber)
            {
                if (responses == null)
                    return;
                if (responses.Length <= 1)
                    return;
                if (questionNumber < 1 || questionNumber > 3)
                    return;

                Response[] sorted = responses
                    .OrderBy(r => r.GetAnswer(questionNumber).Length)
                    .ToArray();
                responses = sorted;
            }

            public void Print()
            {
                System.Console.WriteLine(
                    $"Ответ: {this.Animal}, {this.CharacterTrait}, {this.Concept}"
                );
            }
        }

        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[]? Responses
            {
                get
                {
                    if (this._responses == null)
                        return null;
                    Response[] copy = new Response[_responses.Length];
                    _responses.CopyTo(copy, 0);
                    return copy;
                }
            }

            public Research(string name)
            {
                this._name = name;
                this._responses = new Response[] { };
            }

            public void Add(string[] answers)
            {
                if (answers == null)
                    return;
                if (answers.Length != 3)
                    return;

                if (this.Responses == null)
                    return;
                Response newResponse = new Response(answers[0], answers[1], answers[2]);

                Response[] newArray = new Response[this.Responses.Length + 1];
                this.Responses.CopyTo(newArray, 0);
                newArray[this.Responses.Length] = newResponse;

                this._responses = newArray;
            }

            public string[] GetTopResponses(int question)
            {
                if (this.Responses == null)
                    return default;
                if (question < 1 || question > 3)
                    return new string[0];
                if (this.Responses.Length == 0)
                    return new string[0];

                var sorted = this
                    .Responses.GroupBy(
                        r => r.GetAnswer(question),
                        r => r.GetAnswer(question),
                        (answer, answers) => new { Key = answer, Count = answers.Count() }
                    )
                    .Where(r => r.Key != "" && r.Key != null)
                    .OrderByDescending(r => r.Count)
                    .ToArray();

                return sorted
                    .Take(int.Min(5, sorted.Length))
                    .Where(r => r.Key != "" && r.Key != null)
                    .Select(r => r.Key)
                    .ToArray();
            }

            public void Print()
            {
                System.Console.WriteLine($"Исследование {this.Name}");
                if (this.Responses == null)
                    return;
                if (this.Responses.Length == 0)
                    return;
                foreach (Response r in this.Responses)
                {
                    Console.Write("\t");
                    r.Print();
                }
            }
        }

        public class Report
        {
            Research[] _researches;
            static int _newResearchNumber;

            public Research[] Researches
            {
                get
                {
                    if (_researches == null)
                        return default;
                    Research[] copy = new Research[_researches.Length];
                    Array.Copy(_researches, copy, _researches.Length);
                    return copy;
                }
            }

            static Report()
            {
                _newResearchNumber = 1;
            }

            public Report()
            {
                _researches = new Research[0];
            }

            public Research MakeResearch()
            {
                if (_researches == null)
                    return default;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = new Research(
                    $"No_{_newResearchNumber++}_{DateTime.Today:MM}/{DateTime.Today:yy}"
                );

                return _researches[_researches.Length - 1];
            }

            public (string, double)[] GetGeneralReport(int question)
            {
                if (_researches == null)
                    return default;

                if (question > 3 || question < 1)
                    return default;

                string[] answers = _researches
                    .Select(r => r.Responses)
                    .Where(r => r != null)
                    .Aggregate(
                        (r1, r2) =>
                        {
                            Response[] newArray = new Response[r1.Length + r2.Length];
                            Array.Copy(r1, newArray, r1.Length);
                            Array.Copy(r2, 0, newArray, r1.Length, r2.Length);
                            return newArray;
                        }
                    )
                    .Select(r => r.GetAnswer(question))
                    .Where(a => !String.IsNullOrEmpty(a))
                    .ToArray();

                string[] uniqeAnswers = answers.GroupBy(x => x).Select(y => y.First()).ToArray();

                double[] percentages = uniqeAnswers
                    .Select(uA => (double)answers.Count(a => a == uA) / (double)answers.Length)
                    .ToArray();

                return uniqeAnswers.Zip(percentages).ToArray();
            }
        }
    }
}
