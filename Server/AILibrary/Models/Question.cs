using AILibrary.DAL;

namespace AILibrary.Models
{
    public class Question
    {
        int id;
        string questionText;
        string answer1;
        string answer2;
        string answer3;
        string answer4;
        string correctAnswer;

        public int Id { get => id; set => id = value; }
        public string QuestionText { get => questionText; set => questionText = value; }
        public string Answer1 { get => answer1; set => answer1 = value; }
        public string Answer2 { get => answer2; set => answer2 = value; }
        public string Answer3 { get => answer3; set => answer3 = value; }
        public string Answer4 { get => answer4; set => answer4 = value; }
        public string CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }

        public Question()
        {

        }

        public Question(int id, string questionText, string answer1, string answer2, string answer3, string answer4, string correctAnswer)
        {
            Id = id;
            QuestionText = questionText;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
            Answer4 = answer4;
            CorrectAnswer = correctAnswer;
        }

        static public List<Question> GetRandomQuestions()
        {
            DBservices dbs = new DBservices();
            return dbs.GetRandomQuestions();
        }

    }
}
