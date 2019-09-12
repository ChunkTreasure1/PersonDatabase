using System;
using System.IO;
using System.Collections.Generic;

namespace PersonDatabase.Core
{
    class Database
    {
        private List<Person> myPersons = new List<Person>();

        public void Run()
        {
            bool tempInMenu = true;
            LoadPersons();

            while (tempInMenu)
            {
                Console.Clear();

                Print.PrintColorText("1. Add Person\n", ConsoleColor.DarkCyan);
                Print.PrintColorText("2. Remove Person\n", ConsoleColor.DarkCyan);
                Print.PrintColorText("3. Access Person\n", ConsoleColor.DarkCyan);

                //MAKE MORE SECURE
                int tempChoice = Int32.Parse(Console.ReadLine());
                if (tempChoice == 1)
                {
                    AddPerson();
                }
                else if (tempChoice == 2)
                {
                    RemovePerson();
                }
                else if(tempChoice == 3)
                {
                    AccessPerson();
                }
                else
                {
                    Console.Clear();
                    Print.PrintColorText(tempChoice + " is not a choice!\n", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
        }

        void AddPerson()
        {
        }

        void RemovePerson()
        {

        }

        void LoadPersons()
        {
            string tempPath = Path.GetFullPath("Database");
            string[] tempFiles = Directory.GetFiles(tempPath, "*.txt");

            for (int i = 0; i < tempFiles.Length; i++)
            {
                string tempLine = "";
                using (StreamReader tempSR = new StreamReader(tempFiles[i]))
                {
                    tempLine = tempSR.ReadLine();
                }

                int tempFirstSemiColPos = 0;
                int tempSecondSemiColPos = 0;
                int length = 0;

                tempFirstSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos);

                string tempFirstname = tempLine.Substring(0, tempFirstSemiColPos);

                tempFirstSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);
                tempSecondSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);


                length = tempSecondSemiColPos - tempFirstSemiColPos;
                string tempLastname = tempLine.Substring(tempFirstSemiColPos + 1, length);

                tempFirstSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);
                //tempSecondSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);

                length = tempFirstSemiColPos - tempSecondSemiColPos;
                string tempBirthdate = tempLine.Substring(tempFirstSemiColPos + 1, length);

                tempFirstSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);

                //length = tempSecondSemiColPos - tempFirstSemiColPos;
                //string tempID = tempLine.Substring(tempFirstSemiColPos + 1, length);


                Console.WriteLine(tempFirstname);
                Console.WriteLine(tempLastname);
                Console.WriteLine(tempBirthdate);
                //Console.WriteLine(tempID);

                Console.ReadKey();

                //myPersons.Add(new Person(tempFirstname, tempLastname, tempBirthdate, Int32.Parse(tempID)));
            }
        }

        void AccessPerson()
        {

        }
    }
}
