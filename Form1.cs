using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace Quiz
{

    public partial class Form1: Form
    {
        List<Question> questions;
        //the questions are now in a list so start at 0
        int questionNo = 0;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
            loadQuestion();
            askQuestion(questionNo);
        }

        //wow this is just like b4a
        private void checkAnswer(object sender, EventArgs e)
        {
            //identify what is setting off the event
            var senderObject = (Button)sender;

            //get the tag of the button that sent it, which is a string. gotta convert
            int buttonTag = Convert.ToInt32(senderObject.Tag);

            if (buttonTag == questions[questionNo].CorrectAnswer)
            {
                score++;
            }
            questionNo++;

            if (questionNo < questions.Count)
            {
                askQuestion(questionNo);
            } else
            {
                string end = string.Format("Your score is {0} out of {1}", score, questions.Count);
                MessageBox.Show(
                    "Thanks for taking the quiz." + Environment.NewLine +
                    end
                    ,
                    "Quiz done!");
            }
        }

        private void loadQuestion()
        {   
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //debug
            string filePath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "questions.json"));

            //release
            //string filePath = Path.Combine(baseDirectory, "questions.json");

            string content = File.ReadAllText(filePath);

            questions = JsonConvert.DeserializeObject<List<Question>>(content);
        }

        //questions now come from json
        private void askQuestion(int qNum)
        {
            if (qNum >= questions.Count)
            {
                return;
            }
            else
            {
                var currentQuestion = questions[qNum];

                pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentQuestion.Image);

                // Console.WriteLine($"ah swear to fucken gawd {currentQuestion.QuestionText}");
                lblQuestion.Text = currentQuestion.QuestionText;

                button1.Text = currentQuestion.Answers[0];
                button2.Text = currentQuestion.Answers[1];
                button3.Text = currentQuestion.Answers[2];
                button4.Text = currentQuestion.Answers[3];
            }
        }
    }
    public class Question
    {
        //was not using Newtonsoft properly, now am
        [JsonProperty("question")]
        public string QuestionText { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("answers")]
        public List<string> Answers { get; set; }

        [JsonProperty("correctAnswer")]
        public int CorrectAnswer { get; set; }
    }

}
