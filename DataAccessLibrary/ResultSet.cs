using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public class ResultSet<T>
    {

        public ResultSet()
        {

        }

        public ResultSet(T model) 
        {
            Result = model;
            // Result = (T)model.GetType().GetProperty("Result").GetValue(model);

        }
        public T Result { get; set; } = (T)Activator.CreateInstance(typeof(T));

        public List<Trace> Traces { get; set; } = new List<Trace>();

        //Logcial erros are falglged by idb
        public bool CriticalError { get; set; } = false;
        public bool LogicalError { get; set; } = false;
    //uses interface to pass generic instance, get its tytpe

        public void Merge(object fromResultSet)
        {
            Type fromObjectType = fromResultSet.GetType();

            CriticalError = CriticalError || (bool)fromObjectType.GetProperty("CriticalError").GetValue(fromResultSet);
            LogicalError = LogicalError || (bool)fromObjectType.GetProperty("LogicalError").GetValue(fromResultSet);

            //First in last out
            Traces.InsertRange(0, (List<Trace>)fromObjectType.GetProperty("Traces").GetValue(fromResultSet));
        }

        public void AddTrace(Trace trace)
        {
            //first in last out
            Traces.Insert(0, trace);
        }
    }
}