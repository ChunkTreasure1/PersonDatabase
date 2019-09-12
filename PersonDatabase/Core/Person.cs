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
        private int myId;

        //Getting
        public Tuple<string, string> GetName() { return Tuple.Create(myFirstname, myLastname); }
        public string GetBirthdate() { return myBirthdate; }

        //Setting
        public void SetName(string aFirstname, string aLastname) { myFirstname = aFirstname; myLastname = aLastname; }
        public void SetBirthdate(string aDate) { myBirthdate = aDate; }

        public Person(string aFirst, string aLast, string aBirth, int anId)
        {
            myFirstname = aFirst;
            myLastname = aLast;
            myBirthdate = aBirth;

            myId = anId;
        }
    }
}
