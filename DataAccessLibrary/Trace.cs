using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataAccessLibrary
{
    public class Trace
    {
        public enum ErrorTypes { Critical, Logical, None };
        public ErrorTypes ErrorType { get; set; } = ErrorTypes.None;
        public String ClassName { get; set; } = "";
        public String MemberName { get; set; } = "";
        public int ErrorNumber { get; set; }
        public List<String> ErrorMessages { get; set; } = new List<String>();

        public void addErrorMessage(string errrorMessage)
        {
            ErrorMessages.Insert(0, errrorMessage);
        }
    }
}
