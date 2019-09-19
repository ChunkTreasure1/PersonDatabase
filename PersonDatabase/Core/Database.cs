using System;
using System.IO;
using System.Collections.Generic;

namespace PersonDatabase.Core
{
    class Database
    {
        private List<Person> myPersons = new List<Person>();
        private List<string> myNationalities = new List<string>();
        private int myLastID = 0;

        /// <summary>
        /// Runs the main program
        /// </summary>
        public void Run()
        {
            bool tempInMenu = true;
            LoadPersons();
            LoadNationalities();

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

            bool tempNameB = false;
            string tempName = "";
            do
            {
                Print.PrintColorText("Enter firstname: ", ConsoleColor.Green);
                tempName = Console.ReadLine();
                if (tempName.Contains(";"))
                {
                    Print.PrintColorText("Name cannot contain semi-colon!", ConsoleColor.Red);
                }
                else
                {
                    tempNameB = true;
                }
            } while (!tempNameB);

            bool tempLastnameB = false;
            string tempLastname = "";
            do
            {
                Print.PrintColorText("Enter lastname: ", ConsoleColor.Green);
                tempLastname = Console.ReadLine();
                if (tempLastname.Contains(";"))
                {
                    Print.PrintColorText("Name cannot contain semi-colon!", ConsoleColor.Red);
                }
                else
                {
                    tempLastnameB = true;
                }
            } while (!tempLastnameB);


            bool tempBirth = false;
            string tempBirthdate = "";
            do
            {
                Print.PrintColorText("Enter birthdate(YYMMDD): ", ConsoleColor.Green);
                tempBirthdate = Console.ReadLine();
                if (tempBirthdate.Length == 6)
                {
                    if (!tempBirthdate.Contains(";"))
                    {
                        tempBirth = true;
                    }
                    else
                    {
                        Print.PrintColorText("Birthdate cannot contain semi-colon!", ConsoleColor.Red);
                    }
                }
                else
                {
                    Print.PrintColorText("Wrong format!\n", ConsoleColor.Red);
                }
            } while (!tempBirth);

            bool tempGenderB = false;
            string tempGender;
            do
            {
                Print.PrintColorText("Enter gender: ", ConsoleColor.Green);
                tempGender = Console.ReadLine();
                if (!tempGender.Contains(";"))
                {
                    tempGenderB = true;
                }
                else
                {
                    Print.PrintColorText("Gender cannot contain semi-colon!", ConsoleColor.Red);
                }
            } while (!tempGenderB);

            Print.PrintColorText("Press ENTER to choose nationality", ConsoleColor.Green);
            Console.ReadKey();

            //bool tempNatGet = false;
            string tempNat = GetNationality();

            Console.Clear();
            Print.PrintColorText("Firstname: " + tempName, ConsoleColor.Green);
            Print.PrintColorText("Lastname: " + tempLastname, ConsoleColor.Green);
            Print.PrintColorText("Gender: " + tempGender, ConsoleColor.Green);
            Print.PrintColorText("Nationality: " + tempNat, ConsoleColor.Green);
            Print.PrintColorText("Birthdate: " + tempBirthdate, ConsoleColor.Green);

            //Checks if the person exists in the database
            if (!CheckForExisting(new Tuple<string, string>(tempName, tempLastname)))
            {
                myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, tempGender, tempNat, (myLastID + 1)));

                string tempPath = tempPath = Path.GetFullPath("Database/" + (myLastID + 1) + ".txt");

                if (!File.Exists(tempPath))
                {
                    using (StreamWriter tempSW = File.CreateText(tempPath))
                    {
                        tempSW.WriteLine(tempName + ";" + tempLastname + ";" + tempBirthdate + ";" + tempGender + ";" + tempNat + ";" + (myLastID + 1));
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

                        myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, tempGender, tempNat, (myLastID + 1)));
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

        string GetNationality()
        {
            bool tempUsing = true;

            int tempPage = 1;
            int tempOffset = 0;
            do
            {
                Console.Clear();

                Print.PrintColorText("Current page: " + tempPage + "\n", ConsoleColor.Green);
                Print.PrintColorText("Next page: N, Last page: L\n\n", ConsoleColor.Green);

                if (tempOffset == 190)
                {
                    tempOffset--;
                }
                for (int i = tempOffset; i < tempOffset + 10; i++)
                {
                    Print.PrintColorText("[" + (i + 1).ToString() + "]. ", ConsoleColor.Green);
                    Print.PrintColorText(myNationalities[i] + "\n", ConsoleColor.Yellow);
                }

                string tempIn = Console.ReadLine();

                for (int i = 0; i < myNationalities.Count; i++)
                {
                    int tempNum = 0;
                    if (Int32.TryParse(tempIn, out tempNum))
                    {
                        if (tempNum == (i + 1))
                        {
                            return myNationalities[i];
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (tempIn == "N" || tempIn == "n")
                {
                    if (tempPage < 20)
                    {
                        tempOffset += 10;
                        tempPage++;
                    }
                    else
                    {
                        tempOffset = 0;
                        tempPage = 1;
                    }
                }
                if (tempIn == "L" || tempIn == "l")
                {
                    if (tempPage > 1)
                    {
                        tempOffset -= 10;
                        tempPage--;
                    }
                    else
                    {
                        tempOffset = 190;
                        tempPage = 20;
                    }
                }

            } while (tempUsing);

            return "";
        }

        /// <summary>
        /// Checks if the nationality entered by user exists
        /// </summary>
        /// <param name="aNat">The nationality to check</param>
        /// <returns></returns>
        bool CheckNationality(string aNat)
        {
            for (int i = 0; i < myNationalities.Count; i++)
            {
                if (myNationalities[i] == aNat)
                {
                    return true;
                }
            }

            return false;
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

                string[] tempData = tempLine.Split(';');

                myPersons.Add(new Person(tempData[0], tempData[1], tempData[2], tempData[3], tempData[4], Int32.Parse(tempData[5])));

                //Sets the highest number of an ID that exist
                //this is done to not cause problems when creating a new person
                if (Int32.Parse(tempData[5]) > tempInt)
                {
                    tempInt = Int32.Parse(tempData[5]);
                }

                myLastID = tempInt;
            }
        }

        /// <summary>
        /// Loads all the nationalities into the program
        /// </summary>
        void LoadNationalities()
        {
            string tempPath = Path.GetFullPath("Core");
            string[] tempFiles = Directory.GetFiles(tempPath, "*.txt");

            string tempLine = "";

            using (StreamReader tempSR = new StreamReader(tempFiles[0]))
            {
                tempLine = tempSR.ReadLine();
            }

            string[] tempArr = tempLine.Split(';');
            for (int i = 0; i < tempArr.Length; i++)
            {
                myNationalities.Add(tempArr[i]);
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
                    Print.PrintColorText("Gender: " + myPersons[i].GetGender() + "\n", ConsoleColor.Yellow);
                    Print.PrintColorText("Nationality: " + myPersons[i].GetNationality() + "\n", ConsoleColor.Yellow);

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