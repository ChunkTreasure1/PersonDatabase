﻿using System;
using System.IO;
using System.Collections.Generic;

namespace PersonDatabase.Core
{
    class Database
    {
        private List<Person> myPersons = new List<Person>();
        private int myLastID = 0;

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
                Print.PrintColorText("0. Exit Program\n", ConsoleColor.DarkCyan);

                int tempChoice = 0;
                if (Int32.TryParse(Console.ReadLine(), out tempChoice))
                {
                    if (tempChoice == 1)
                    {
                        AddPerson();
                    }
                    else if (tempChoice == 2)
                    {
                        RemovePerson();
                    }
                    else if (tempChoice == 3)
                    {
                        AccessPerson();
                    }
                    else if (tempChoice == 0)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Clear();
                        Print.PrintColorText(tempChoice + " is not a choice!\n", ConsoleColor.Red);
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.Clear();
                    Print.PrintColorText("That is not a number!\n\n", ConsoleColor.Red);
                    Print.PrintColorText("Press ENTER to return", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
        }

        void AddPerson()
        {
            Console.Clear();

            Print.PrintColorText("Enter firstname: ", ConsoleColor.Green);
            string tempName = Console.ReadLine();

            Print.PrintColorText("Enter lastname: ", ConsoleColor.Green);
            string tempLastname = Console.ReadLine();

            Print.PrintColorText("Enter birthdate(YYMMDD): ", ConsoleColor.Green);
            string tempBirthdate = Console.ReadLine();

            if (!CheckForExisting(new Tuple<string, string>(tempName, tempLastname)))
            {
                myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, (myLastID + 1)));

                string tempPath = tempPath = Path.GetFullPath("Database/" + (myLastID + 1) + ".txt");

                if (!File.Exists(tempPath))
                {
                    using (StreamWriter tempSW = File.CreateText(tempPath))
                    {
                        tempSW.WriteLine(tempName + ";" + tempLastname + ";" + tempBirthdate + ";" + (myLastID + 1));
                    }
                }

                myLastID++;
            }
            else
            {
                bool tempDone = false;

                do
                {
                    Console.Clear();

                    Print.PrintColorText("Person already exists!\n\n", ConsoleColor.Red);
                    Print.PrintColorText("Do you want to override?(Y/N)", ConsoleColor.Red);

                    string tempString = Console.ReadLine();
                    if (tempString == "Y" || tempString == "y")
                    {
                        string tempPath = tempPath = Path.GetFullPath("Database/" + GetIDFromPerson(new Tuple<string, string>(tempName, tempLastname)) + ".txt");
                        File.Delete(tempPath);

                        myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, (myLastID + 1)));
                        if (!File.Exists(tempPath))
                        {
                            using (StreamWriter tempSW = File.CreateText(tempPath))
                            {
                                tempSW.WriteLine(tempName + ";" + tempLastname + ";" + tempBirthdate + ";" + (myLastID + 1));
                            }
                        }

                        Print.PrintColorText("Person overridden!\n", ConsoleColor.Red);
                        tempDone = true;
                    }
                    else
                    {
                        tempDone = true;
                    }

                } while (!tempDone);
            }
            Print.PrintColorText("Press ENTER to exit", ConsoleColor.Green);
            Console.ReadKey();
        }

        void RemovePerson()
        {
            bool tempDone = false;

            do
            {
                Console.Clear();
                Print.PrintColorText("Enter ID of person to remove.\n", ConsoleColor.Yellow);
                Print.PrintColorText("Enter 0 to exit.\n", ConsoleColor.Yellow);

                for (int i = 0; i < myPersons.Count; i++)
                {
                    Print.PrintColorText(myPersons[i].GetID() + ". " + myPersons[i].GetName().Item1 + " " + myPersons[i].GetName().Item2 + "\n", ConsoleColor.Green);
                }

                int tempSelection = 0;
                if (Int32.TryParse(Console.ReadLine(), out tempSelection))
                {
                    for (int i = 0; i < myPersons.Count; i++)
                    {
                        if (tempSelection == myPersons[i].GetID())
                        {
                            File.Delete(Path.GetFullPath("Database/" + myPersons[i].GetID() + ".txt"));
                            myPersons.RemoveAt(i);
                            i--;
                        }
                    }

                    if (tempSelection == 0)
                    {
                        tempDone = true;
                    }
                }
                else
                {
                    Console.Clear();
                    Print.PrintColorText("That is not a number!\n\n", ConsoleColor.Red);
                    Print.PrintColorText("Press ENTER to return", ConsoleColor.Red);
                    Console.ReadKey();
                }

            } while (!tempDone);
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

                tempSecondSemiColPos = tempLine.IndexOf(";", tempFirstSemiColPos + 1);


                length = tempSecondSemiColPos - tempFirstSemiColPos - 1;
                string tempLastname = tempLine.Substring(tempFirstSemiColPos + 1, length);

                tempFirstSemiColPos = tempLine.IndexOf(";", tempSecondSemiColPos + 1);

                length = tempFirstSemiColPos - tempSecondSemiColPos;
                string tempBirthdate = tempLine.Substring(tempSecondSemiColPos + 1, length - 1);

                tempFirstSemiColPos = tempLine.IndexOf(";", tempSecondSemiColPos + 1);

                length = tempLine.Length - 1 - tempFirstSemiColPos;
                string tempID = tempLine.Substring(tempFirstSemiColPos + 1, length);

                myPersons.Add(new Person(tempFirstname, tempLastname, tempBirthdate, Int32.Parse(tempID)));
                myLastID++;
            }
        }

        void AccessPerson()
        {
            bool tempDone = false;

            do
            {
                Console.Clear();
                Print.PrintColorText("Enter an ID to access person \n", ConsoleColor.Yellow);
                Print.PrintColorText("Press 0 to exit\n", ConsoleColor.Yellow);
                Console.WriteLine("");

                for (int i = 0; i < myPersons.Count; i++)
                {
                    Print.PrintColorText(myPersons[i].GetID() + ". " + myPersons[i].GetName().Item1 + " " + myPersons[i].GetName().Item2 + "\n", ConsoleColor.Green);
                }

                int tempSelection = 0;
                if (Int32.TryParse(Console.ReadLine(), out tempSelection))
                {
                    for (int i = 0; i < myPersons.Count; i++)
                    {
                        if (tempSelection == myPersons[i].GetID())
                        {
                            Console.Clear();

                            Print.PrintColorText("Name: " + myPersons[i].GetName().Item1 + "\n", ConsoleColor.Yellow);
                            Print.PrintColorText("Lastname: " + myPersons[i].GetName().Item2 + "\n", ConsoleColor.Yellow);

                            //IMPROVE DATE VIEW
                            Print.PrintColorText("Birthdate: " + ConvertDateString(myPersons[i].GetBirthdate()) + "\n", ConsoleColor.Yellow);

                            Console.WriteLine("");
                            Print.PrintColorText("Press ENTER to exit \n", ConsoleColor.Green);
                            Console.ReadKey();
                        }
                    }

                    if (tempSelection == 0)
                    {
                        tempDone = true;
                    }
                }
                else
                {
                    Console.Clear();
                    Print.PrintColorText("That is not a number!\n\n", ConsoleColor.Red);
                    Print.PrintColorText("Press ENTER to return", ConsoleColor.Red);
                    Console.ReadKey();
                }

            } while (!tempDone);
        }

        bool CheckForExisting(Tuple<string, string> aName)
        {
            for (int i = 0; i < myPersons.Count; i++)
            {
                if (aName.Item1 == myPersons[i].GetName().Item1 && 
                    aName.Item2 == myPersons[i].GetName().Item2)
                {
                    return true;
                }
            }
            return false;
        }

        string ConvertDateString(string aString)
        {
            string tempString = aString;

            tempString = tempString.Insert(2, "/");
            tempString = tempString.Insert(5, "/");

            return tempString;
        }

        string GetIDFromPerson(Tuple<string, string> aName)
        {
            for (int i = 0; i < myPersons.Count; i++)
            {
                if (myPersons[i].GetName().Item1 == aName.Item1 && 
                    myPersons[i].GetName().Item2 == aName.Item2)
                {
                    return myPersons[i].GetID().ToString();
                }
            }

            return "";
        }
    }
}
