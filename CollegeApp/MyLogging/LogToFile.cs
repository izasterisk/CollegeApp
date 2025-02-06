namespace CollegeApp.MyLogging
{
    public class LogToFile : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine("Log to File: " + message);
        }
    }
}
