using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Main
{
    public class Trivia
    {
        private System.IO.StreamReader sr;
        List<TriviaReadandWrite> questions = new List<TriviaReadandWrite>();
        private int triviaQuestionsAsked;
        private int[] randomIndexes;
        private int currentIndex = 0;

        public Trivia()
        {
            readfilefortrivia();//Reads from TriviaReadandWrite
            randomIndexes = new int[questions.Count];//Counts through the number of questions

            for(int n = 0; n < questions.Count; n++)//Loops through questions
            {
                randomIndexes[n] = n;
            }


            DateTime currentDateTime = DateTime.Now;
            Random generateRandom = new Random(currentDateTime.Millisecond);


            for (int q = 0; q < questions.Count; q++)//Randomizes the question taken
            {
                int number = generateRandom.Next(0, questions.Count - 1);//Generate random number
                int temp = randomIndexes[q];//Initializes 
                randomIndexes[q] = randomIndexes[number];
                randomIndexes[number] = temp;
            }
        }

        private void readfilefortrivia()
        {
            string buffer;
            sr = new StreamReader("notepad.txt");

            buffer = sr.ReadLine();
            while (buffer != null)
            {
                string[] data = buffer.Split(',');

                string q = data[0];
                string c1 = data[1];
                string c2 = data[2];
                string c3 = data[3];
                string c4 = data[4];
                int ans = int.Parse(data[5]);
                String ANS = data[6];

                TriviaReadandWrite t = new TriviaReadandWrite(q, c1, c2, c3, c4, ans, ANS);

                questions.Add(t);
                buffer = sr.ReadLine();
                //buffer = sr.ReadLine();
            }
            sr.Close();

        }

        public TriviaReadandWrite getQuestion()
        {
            //triviaQuestionsAsked[number] = 1;
            
            if (currentIndex > questions.Count-1)
            {
                // Call up the question.
                currentIndex = 0;
            }

            return questions[randomIndexes[currentIndex++]];

        }
    }
}
