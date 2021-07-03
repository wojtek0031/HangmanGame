using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace HangmanApp
{
    class StartPoint
    {
        static void Main(string[] args)
        {
            Hangman hangman = new Hangman();
            hangman.startGame();
        }
    }

    class Hangman
    {
        private string capital;
        private string underLinedCapital;
        private int lifeCounter = 5;
        private ArrayList notInWordChars = new ArrayList();
        public void startGame()
        {

            lifeCounter = 5;
            notInWordChars = new ArrayList();
            Console.WriteLine("Hangman Game");
            ArrayList capitals = getEuropeanCapitals();
            capital = getRandomCapital(capitals).ToLower();
            underLinedCapital = getUnderLined(capital);
            while (true)
            {
                Console.WriteLine(capital);
                Console.WriteLine(underLinedCapital + "               Life: " + lifeCounter + "               Not in word characters: "+ getNotInWordFormattedChars());
                Console.WriteLine("Type '0' if you want to enter a word \nType '1' if you want to enter a letter");
                var userDecision = Console.ReadLine().Trim().ToLower();
                if(userDecision == "0")
                {
                    guessWord();
                } else if(userDecision == "1")
                {
                    guessLetter();
                }
                Console.Clear();

                if (lifeCounter == 0)
                {
                    gameOver();
                }
            }
        }

        private void gameOver()
        {
            Console.WriteLine("You lost! \n Press any key to restart...");
            Console.ReadKey();
            Console.Clear();
            startGame();
        }

        private string getNotInWordFormattedChars()
        {
            string formattedChars = "";
            foreach(string character in notInWordChars)
            {
                formattedChars += character + " ";
            }
            return "[ "+ formattedChars +"]";
        }

        private void guessWord()
        {
            Console.WriteLine("Enter a word");
            var choosenWord = Console.ReadLine().ToString().ToLower();
            if(capital == choosenWord)
            {
                winGame();
            } else
            {
                Console.WriteLine("Wrong word!");
                lifeCounter--;
                Thread.Sleep(1500);
            }
        }

        private void winGame()
        {
            Console.WriteLine("You win!");
            Console.ReadKey();
            Console.Clear();
            startGame();
        }

        private void guessLetter()
        {
            Console.WriteLine("Enter a letter: ");
            var choosenLetter = ((char)Console.Read()).ToString().ToLower();
            if (capital.Contains(choosenLetter))
            {
                onLetterCorrect(choosenLetter);
            } else
            {
                lifeCounter--;
                if(!notInWordChars.Contains(choosenLetter))
                    notInWordChars.Add(choosenLetter);
            }

        }

        private void onLetterCorrect(string choosenLetter)
        {
            var newUnderlinedCapital = new StringBuilder(underLinedCapital);
            var capitalCharArray = capital.ToCharArray();
            for(int i = 0; i < capitalCharArray.Length; i++)
            {
                if(capitalCharArray[i].ToString()==choosenLetter)
                {
                    newUnderlinedCapital.Remove(i, 1);
                    newUnderlinedCapital.Insert(i, choosenLetter);
                }
            }
            underLinedCapital = newUnderlinedCapital.ToString();
            if(underLinedCapital == capital)
            {
                winGame();
            }
        }

        private string getUnderLined(string text)
        {
            var textAsCharArray = text.ToCharArray();
            string underlinedText = "";
            foreach(char character in textAsCharArray)
            {
                underlinedText += "_";
            }
            return underlinedText;
        }

        private ArrayList getEuropeanCapitals()
        {
            string[] fileContent = File.ReadAllLines("europeanCapitals.txt");
            var capitals = new ArrayList();
            foreach (string entry in fileContent)
            {
                capitals.Add(entry);
            }
            return capitals;
        }

        private Dictionary<string, string> getMapFromCapitalsFile()
        {
            string[] fileContent = File.ReadAllLines("countries_and_capitals.txt");
            Dictionary<string, string> countriesAndCapitals = new Dictionary<string, string>();
            foreach (string entry in fileContent)
            {
                string[] countriesToCapital = Regex.Split(entry, "\\|");
                countriesAndCapitals.Add(countriesToCapital[0].Trim(), countriesToCapital[1].Trim());
            }
            return countriesAndCapitals;
        }

        private string getRandomCapital(ArrayList capitals)
        {
            var random = new Random();
            return (string) capitals[random.Next(capitals.Count)];
        }
    }
}
