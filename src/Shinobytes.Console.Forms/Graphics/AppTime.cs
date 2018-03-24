namespace Shinobytes.Console.Forms.Graphics
{
    public class AppTime
    {
        public AppTime(double elapsed, double totalElapsed)
        {
            Elapsed = elapsed;
            TotalElapsed = totalElapsed;
        }

        public double TotalElapsed { get; }
        public double Elapsed { get; }
    }
}