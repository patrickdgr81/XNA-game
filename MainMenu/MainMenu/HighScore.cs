using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Main
{
    class HighScore
    {
        // data variables needed in the high score menu.
        public string Name;
        public int caveNumber;
        public int Score;
        public HighScore() { }
        public HighScore(string n, int c, int s)
        {
            Name = n;
            caveNumber = c;
            Score = s;
        }
        List<HighScore> scores = new List<HighScore>();

        public void addHighScore(string name, int cave, int score)
        {
            // (name, cave #, score)
            HighScore h1 = new HighScore(name, cave, score);
            scores.Add(h1);

            // Sort the list in decreasing order
            for (int i = 0; i < scores.Count; i++)
            {
                HighScore hScore = scores[i];
                for (int j = i + 1; j < scores.Count; j++)
                {
                    HighScore hScore2 = scores[j];
                    if (hScore.Score < hScore2.Score)
                    {
                        scores[i] = scores[j];
                        scores[j] = hScore;
                        hScore = scores[i];


                    }
                }
            }
            // This is to make sure only the top ten highest scores are 
            // displayed.
            if (scores.Count > 10)
                scores.RemoveAt(10);

            // put write file method here.
        }

        // return highscore
        public List<HighScore> HighScores()
        {
            return scores;
        }

        // How the data should be displayed.
        public string ToString()
        {
            return Name + "," + caveNumber + "," + Score + ",";
        }

        public void readFile()
        {

            StreamReader sr = new StreamReader("HighScoreData.txt");

            //read in a line of data from the text file

            string input = sr.ReadLine();

            while (input != null)
            {
                string[] data = input.Split(',');

                //fill out the info for the song object
                string nameString;
                int caveNumberInt;
                int scoreInt;

                nameString = data[0];
                caveNumberInt = int.Parse(data[1]);
                scoreInt = int.Parse(data[2]);

                HighScore s = new HighScore(nameString, caveNumberInt, scoreInt);

                //recreate the list array
                scores.Add(s);

                //get the next line of data
                input = sr.ReadLine();
            }

            sr.Close();
        }

        /// <summary>
        /// this will accept a string value and two integers and write them off to a file and
        /// </summary>
        /// <param name="index"></param>
        public void writeFile(String name, int cave, int highscore, int turns, int gold, int arrows)
        {
            using (StreamWriter outStream = new StreamWriter("HighScoreData.txt", true))
            {
                outStream.Write("");
                outStream.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", name, cave, highscore, turns, gold, arrows));

                //string separator = ", ";
                //outStream.Write(name);
                //outStream.Write(separator);
                //outStream.Write(cave);
                //outStream.Write(separator);
                //outStream.Write(highscore);
                //outStream.WriteLine();
            }
            //.flush()
            //.close()
            //.dispose()

        }

    }
}

