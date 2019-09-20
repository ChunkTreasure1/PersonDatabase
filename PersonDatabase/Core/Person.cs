using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDatabase.Core
{
    class Person
    {
        private string myFirstname;
        private string myLastname;

        private string myBirthdate;
        private string mySex;
        private string myNationality;

        private int myId;

        //Getting
        public Tuple<string, string> GetName() { return Tuple.Create(myFirstname, myLastname); }
        public string GetBirthdate() { return myBirthdate; }
        public int GetID() { return myId; }
        
        public string GetSex() { return mySex; }
        public string GetNationality() { return myNationality; }

        //Setting
        public void SetName(string aFirstname, string aLastname) { myFirstname = aFirstname; myLastname = aLastname; }
        public void SetBirthdate(string aDate) { myBirthdate = aDate; }
        public void SetGender(string aSex) { mySex = aSex; }

        public void SetNationality(string aNationality) { myNationality = aNationality; }

        public Person(string aFirst, string aLast, string aBirth, string aSex, string aNat, int anId)
        {
            myFirstname = aFirst;
            myLastname = aLast;
            myBirthdate = aBirth;

            mySex = aSex;
            myNationality = aNat;
            myId = anId;
        }
    }
}
