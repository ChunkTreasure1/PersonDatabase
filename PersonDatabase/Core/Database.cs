using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
                    Print.PrintColorText("Name cannot contain semi-colon!\n", ConsoleColor.Red);
                }
                else
                {
                    bool tempIsNum = !tempName.All(Char.IsLetter);
                    if (tempIsNum)
                    {
                        Print.PrintColorText("Name cannot contain non-letters!\n", ConsoleColor.Red);
                        continue;
                    }

                    if (tempName.Length == 0)
                    {
                        Print.PrintColorText("Name cannot be empty!\n", ConsoleColor.Red);
                        continue;
                    }
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
                    Print.PrintColorText("Lastname cannot contain semi-colon!\n", ConsoleColor.Red);
                }
                else
                {
                    bool tempIsNum = !tempLastname.All(Char.IsLetter);
                    if (tempIsNum)
                    {
                        Print.PrintColorText("Lastname cannot contain non-letters!\n", ConsoleColor.Red);
                        continue;
                    }
                    if (tempLastname.Length == 0)
                    {
                        Print.PrintColorText("Lastname cannot be empty!\n", ConsoleColor.Red);
                        continue;
                    }
                    tempLastnameB = true;
                }
            } while (!tempLastnameB);

            bool tempBirth = false;
            string tempBirthdate = "";
            do
            {
                //Using YYYYMMDD and not YYMMDD to support people from the 20th century
                Print.PrintColorText("Enter birthdate(YYYYMMDD): ", ConsoleColor.Green);
                tempBirthdate = Console.ReadLine();

                bool tempIsNum = !tempBirthdate.All(Char.IsNumber);
                if (tempIsNum)
                {
                    Print.PrintColorText("Birthdate cannot contain non-numbers!\n", ConsoleColor.Red);
                    continue;
                }

                if (tempBirthdate.Length == 0)
                {
                    Print.PrintColorText("Birthdate cannot be empty!\n", ConsoleColor.Red);
                    continue;
                }

                DateTime tempDT = DateTime.UtcNow.Date;
                if (Int32.Parse(tempDT.ToString("yyyyMMdd")) < Int32.Parse(tempBirthdate))
                {
                    Print.PrintColorText("You can not be this age!\n", ConsoleColor.Red);
                    continue;
                }

                if (tempBirthdate.Length == 8)
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

            bool tempSexB = false;
            string tempSex;
            do
            {
                Print.PrintColorText("Enter sex(Male/Female): ", ConsoleColor.Green);
                tempSex = Console.ReadLine();
                if (!tempSex.Contains(";"))
                {
                    if (tempSex == "Male" || tempSex == "Female")
                    {
                        tempSexB = true;
                    }
                }
                else
                {
                    Print.PrintColorText("Sex cannot contain semi-colon!", ConsoleColor.Red);
                }

                if (tempSex != "Male" && tempSex != "Female")
                {
                    Print.PrintColorText("Incorrect input!\n", ConsoleColor.Red);
                }

            } while (!tempSexB);

            Print.PrintColorText("Press ENTER to choose nationality", ConsoleColor.Green);
            Console.ReadKey();

            //bool tempNatGet = false;
            string tempNat = GetNationality();

            Console.Clear();
            Print.PrintColorText("Firstname: ", ConsoleColor.Green);
            Print.PrintColorText(tempName + "\n", ConsoleColor.Yellow);

            Print.PrintColorText("Lastname: ", ConsoleColor.Green);
            Print.PrintColorText(tempLastname + "\n", ConsoleColor.Yellow);

            Print.PrintColorText("Sex: ", ConsoleColor.Green);
            Print.PrintColorText(tempSex + "\n", ConsoleColor.Yellow);

            Print.PrintColorText("Nationality: ", ConsoleColor.Green);
            Print.PrintColorText(tempNat + "\n", ConsoleColor.Yellow);

            Print.PrintColorText("Birthdate: ", ConsoleColor.Green);
            Print.PrintColorText(ConvertDateString(tempBirthdate) + "\n", ConsoleColor.Yellow);

            //Checks if the person exists in the database
            if (!CheckForExisting(new Tuple<string, string>(tempName, tempLastname)))
            {
                myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, tempSex, tempNat, (myLastID + 1)));

                string tempPath = tempPath = Path.GetFullPath("Database/" + (myLastID + 1) + ".txt");

                if (!File.Exists(tempPath))
                {
                    using (StreamWriter tempSW = File.CreateText(tempPath))
                    {
                        tempSW.WriteLine(tempName + ";" + tempLastname + ";" + tempBirthdate + ";" + tempSex + ";" + tempNat + ";" + (myLastID + 1));
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

                        myPersons.Add(new Person(tempName, tempLastname, tempBirthdate, tempSex, tempNat, (myLastID + 1)));
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
        /// Gets the users nationality via a new menu
        /// </summary>
        /// <returns>Returns the nationality the user selects</returns>
        string GetNationality()
        {
            bool tempUsing = true;

            int tempPage = 1;
            int tempOffset = 0;
            do
            {
                Console.Clear();


                Print.PrintColorText("Current page: " + tempPage + "\n", ConsoleColor.Green);
                Print.PrintColorText("Next page: N, Last page: L, Search: S\n\n\n", ConsoleColor.Green);

                if (tempOffset == 190)
                {
                    tempOffset--;
                }
                for (int i = tempOffset; i < tempOffset + 10; i++)
                {
                    Print.PrintColorText("[" + (i + 1).ToString() + "] ", ConsoleColor.Green);
                    Print.PrintColorText(myNationalities[i] + "\n", ConsoleColor.Yellow);
                }

                Console.SetCursorPosition(0, 2);

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

                bool tempKeyPressed = false;
                do
                {
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
                        break;
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
                        break;
                    }
                    if (tempIn == "S" || tempIn == "s")
                    {
                        ConsoleKeyInfo tempCKI;
                        string tempSearchLine = "";
                        Console.Clear();

                        bool tempSearching = true;
                        do
                        {
                            Console.Clear();
                            Print.PrintColorText("Search for nationality(One letter only): ", ConsoleColor.Green);

                            tempCKI = Console.ReadKey();
                            tempSearchLine += tempCKI.KeyChar.ToString().ToUpper();
                            Console.Clear();

                            Print.PrintColorText("Search for nationality(One letter only): ", ConsoleColor.Green);
                            Print.PrintColorText(tempSearchLine + "\n", ConsoleColor.Yellow);

                            //Checks for all elements with first letter equal to the search lines.
                            //Using for-loops in case of expansion for multiletter search
                            for (int i = 0; i < myNationalities.Count; i++)
                            {
                                for (int j = 0; j < tempSearchLine.Length; j++)
                                {
                                    if (myNationalities[i][j] == tempSearchLine[j])
                                    {
                                        Print.PrintColorText("[" + (i + 1).ToString() + "] ", ConsoleColor.Green);
                                        Print.PrintColorText(myNationalities[i] + "\n", ConsoleColor.Yellow);
                                    }
                                }
                            }

                            Print.PrintColorText("\nPress ENTER to select or BACKSPACE to re-search\n", ConsoleColor.Green);

                            tempCKI = Console.ReadKey();
                            if (tempCKI.Key == ConsoleKey.Backspace)
                            {
                                tempSearchLine = "";
                            }
                            else
                            {
                                Console.WriteLine("");

                                tempIn = Console.ReadLine();
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
                            }

                        } while (tempSearching);
                        
                    }
                    else
                    {
                        tempKeyPressed = true;
                    }
                } while (!tempKeyPressed);

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
                    Print.PrintColorText("Sex: " + myPersons[i].GetSex() + "\n", ConsoleColor.Yellow);
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

            tempString = tempString.Insert(4, "/");
            tempString = tempString.Insert(7, "/");

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