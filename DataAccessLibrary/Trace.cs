using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataAccessLibrary
{
    public class Trace
    {

        public int ErrorNumbers { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public string ClassName { get; set; }
        public string MemberName { get; set; }

        public enum ErrorTypes { Critical, Logical};
        public ErrorTypes ErrorType { get; set; }
        public int ErrorNumber { get; internal set; }

        public void addErrorMessage(string errrorMessage)
        {
            ErrorMessages.Insert(0, errrorMessage);
        }
    }
}
