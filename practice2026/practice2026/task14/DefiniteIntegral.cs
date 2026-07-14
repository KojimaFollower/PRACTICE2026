using System;
using System.Threading;
namespace task14
{
    public class DefiniteIntegral
    {
        public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
        {
            int totalsteps = (int)Math.Ceiling((b - a) / step);
            double exactstep = (b - a) / totalsteps;
            double totalarea = 0.0;

            for (int s = 0; s < totalsteps; s++)
            {
                double x1 = a + s * exactstep;
                double x2 = x1 + exactstep;

                double y1 = function(x1);
                double y2 = function(x2);

                totalarea += (y1 + y2) / 2.0 * exactstep;
            }

            return totalarea;
        }

        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
        {
            double totalarea = 0.0;
            int totalsteps = (int)Math.Ceiling((b - a) / step);
            int stepsperthread = totalsteps / threadsnumber;
            Thread[] threads = new Thread[threadsnumber];
            double exactstep = (b - a) / totalsteps;
            for (int i = 0; i < threadsnumber; i++)
            {
                int start = i * stepsperthread;
                int end = (i == threadsnumber - 1) ? totalsteps : (i + 1) * stepsperthread;

                threads[i] = new Thread(() =>
                {
                    double localarea = 0.0;
                    for (int s = start; s < end; s++)
                    {
                        double x1 = a + s * exactstep;
                        double x2 = x1 + exactstep;

                        double y1 = function(x1);
                        double y2 = function(x2);
                        localarea += (y1 + y2) / 2.0 * exactstep;
                    }
                    double initialvalue;
                    double value;
                    do
                    {
                        initialvalue = totalarea;
                        value = initialvalue + localarea;
                    }
                    while (Interlocked.CompareExchange(ref totalarea, value, initialvalue) != initialvalue);
                });
                threads[i].Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            return totalarea;
        }
    }
}