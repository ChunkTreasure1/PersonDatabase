using System;
using System.IO;
using System.Collections.Generic;

namespace PersonDatabase.Core
{
    class Database
    {
        private List<Person> myPersons = new List<Person>();
        private int myLastID = 0;

        /// <summary>
        /// Runs the main program
        /// </summary>
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
                        tempInMenu = false;
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

        /// <summary>
        /// Adds a person to the database
        /// </summary>
        void AddPerson()
        {
            Console.Clear();

            Print.PrintColorText("Enter firstname: ", ConsoleColor.Green);
            string tempName = Console.ReadLine();

            Print.PrintColorText("Enter lastname: ", ConsoleColor.Green);
            string tempLastname = Console.ReadLine();

            Print.PrintColorText("Enter birthdate(YYMMDD): ", ConsoleColor.Green);
            string tempBirthdate = Console.ReadLine();

            //Checks if the person exists in the database
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

                //Ask if the person existing should be overwritten
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

        /// <summary>
        /// Removes a person from the database
        /// </summary>
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
                    if (tempSelection == 0)
                    {
                        tempDone = true;
                        return;
                    }

                    if (!DeletePerson(tempSelection))
                    {
                        Console.Clear();

                        Print.PrintColorText("That ID does not exist!\n\n", ConsoleColor.Red);
                        Print.PrintColorText("Press ENTER to return\n", ConsoleColor.Red);
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

            } while (!tempDone);
        }

        /// <summary>
        /// Deletes a person from disk
        /// </summary>
        /// <param name="anID">The ID of the person to remove from disk</param>
        /// <returns>Returns wether or not the person actually exists</returns>
        bool DeletePerson(int anID)
        {
            for (int i = 0; i < myPersons.Count; i++)
            {
                if (anID == myPersons[i].GetID())
                {
                    File.Delete(Path.GetFullPath("Database/" + myPersons[i].GetID() + ".txt"));
                    myPersons.RemoveAt(i);
                    i--;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Loads all persons into the program
        /// </summary>
        void LoadPersons()
        {
            string tempPath = Path.GetFullPath("Database");
            string[] tempFiles = Directory.GetFiles(tempPath, "*.txt");

            int tempInt = 0;

            //Looks through all the files in the directory
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
                
                //Finds the correct semi-colon and gets the text after it
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

                //Sets the highest number of an ID that exist
                //this is done to not cause problems when creating a new person
                if (Int32.Parse(tempID) > tempInt)
                {
                    tempInt = Int32.Parse(tempID);
                }

                myLastID = tempInt;
            }
        }

        /// <summary>
        /// Let's the user to access the persons in the database
        /// </summary>
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
                    if (tempSelection == 0)
                    {
                        tempDone = true;
                        return;
                    }

                    if (!ShowPerson(tempSelection))
                    {
                        Console.Clear();

                        Print.PrintColorText("That ID does not exist!\n\n", ConsoleColor.Red);
                        Print.PrintColorText("Press ENTER to return\n", ConsoleColor.Red);
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

            } while (!tempDone);
        }

        /// <summary>
        /// Shows a person with a certain ID
        /// </summary>
        /// <param name="anID">The ID of the person to show</param>
        /// <returns>Returns wether or not the person actually exists</returns>
        bool ShowPerson(int anID)
        {
            for (int i = 0; i < myPersons.Count; i++)
            {
                if (anID == myPersons[i].GetID())
                {
                    Console.Clear();

                    Print.PrintColorText("Name: " + myPersons[i].GetName().Item1 + "\n", ConsoleColor.Yellow);
                    Print.PrintColorText("Lastname: " + myPersons[i].GetName().Item2 + "\n", ConsoleColor.Yellow);

                    Print.PrintColorText("Birthdate: " + ConvertDateString(myPersons[i].GetBirthdate()) + "\n", ConsoleColor.Yellow);

                    Console.WriteLine("");
                    Print.PrintColorText("Press ENTER to exit \n", ConsoleColor.Green);
                    Console.ReadKey();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a person exists in the database
        /// </summary>
        /// <param name="aName">The last and first name of the person to check</param>
        /// <returns>Returns wether or not the user exists</returns>
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

        /// <summary>
        /// Converts the format YYMMDD to YY/MM/DD
        /// </summary>
        /// <param name="aString">The YYMMDD format</param>
        /// <returns>Returns the input in the YY/MM/DD format</returns>
        string ConvertDateString(string aString)
        {
            string tempString = aString;

            tempString = tempString.Insert(2, "/");
            tempString = tempString.Insert(5, "/");

            return tempString;
        }

        /// <summary>
        /// Gets the ID number of a certain person
        /// </summary>
        /// <param name="aName">The first and lastname of the person</param>
        /// <returns>Returns the ID as a string</returns>
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