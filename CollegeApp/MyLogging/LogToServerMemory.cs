namespace CollegeApp.MyLogging
{
    public class LogToServerMemory : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine("Log to Server Memory: " + message);
        }
    }
}
