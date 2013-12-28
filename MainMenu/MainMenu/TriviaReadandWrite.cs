using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    public class TriviaReadandWrite
    {
        string questionString;
        string choice1String;
        string choice2String;
        string choice3String;
        string choice4String;
        int answerToQuestionInteger;
        string AnswerHint;
        public string answerHint
        {
            get { return AnswerHint; }
            set { AnswerHint = value; }
        }
        public string Question
        {
            get { return questionString; }
            set { questionString = value; }
        }

        public string Choice1
        {
            get { return choice1String; }
            set { choice1String = value; }
        }

        public string Choice2
        {
            get { return choice2String; }
            set { choice2String = value; }
        }

        public string Choice3
        {
            get { return choice3String; }
            set { choice3String = value; }
        }

        public string Choice4
        {
            get { return choice4String; }
            set { choice4String = value; }
        }

        public int Answer
        {
            get { return answerToQuestionInteger; }
            set {answerToQuestionInteger = value;}
        }

        public TriviaReadandWrite () {}
        public TriviaReadandWrite(string q, string c1, string c2, string c3, string c4, int ans, string ANS)
        {
            questionString = q;
            choice1String = c1;
            choice2String = c2;
            choice3String = c3;
            choice4String = c4;
            answerToQuestionInteger = ans;
            answerHint = ANS;
        }
    }
}
