using System;
using System.Globalization;

namespace CritterWorld
{
    public class LogEntry
    {
        public LogEntry(int critterNumber, string critterName, string author, string eventMessage, Exception exception = null)
        {
            Timestamp = DateTime.Now;
            CritterNumber = critterNumber;
            CritterName = critterName;
            Author = author;
            EventMessage = eventMessage;
            Exception = exception;
        }

        public LogEntry(string eventMessage, Exception exception = null) : this(0, "<system>", "<system>", eventMessage, exception) { }

        public DateTime Timestamp { get; }
        public int CritterNumber { get; }
        public string CritterName { get; }
        public string Author { get; }
        public string EventMessage { get; }
        public Exception Exception { get; }

        private static string ToQuoted(string input)
        {
            return "\"" + input.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }

        public override string ToString()
        {
            return Timestamp.ToString("o", CultureInfo.CurrentCulture) + ": #" + CritterNumber + " " + CritterName + " by " + Author + " " + EventMessage + ((Exception != null) ? " due to exception: " + Exception.StackTrace : "");
        }

        public string ToCSV()
        {
            return Timestamp.ToString("o", CultureInfo.CurrentCulture) + ", " + CritterNumber + ", " + ToQuoted(CritterName) + ", " + ToQuoted(Author) + ", " + ToQuoted(EventMessage) + ", " + ((Exception == null) ? ToQuoted("") : ToQuoted(Exception.StackTrace));
        }
    }

}
