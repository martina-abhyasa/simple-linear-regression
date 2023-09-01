using System;
using System.IO;   //to work with files in C#, we use the System.IO


namespace SimpleLinearRegression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string textFile = @"D:\C#\SimpleLinearRegression\Data.txt";

            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                int l = lines.Length;

                double[] x = new double[l]; // array x -> explanatory independent variable (predictor)
                double[] y = new double[l]; // array y -> dependent variable (response)

                StreamReader data = new StreamReader(textFile);
                StreamWriter result = new StreamWriter(@"D:\C#\RegressionResults\Result.txt");

                int i = 0;

                while (!data.EndOfStream)
                {
                    string row = data.ReadLine();
                    string[] rowArray = row.Split(',');

                    x[i] = double.Parse(rowArray[0]);
                    y[i] = double.Parse(rowArray[1]);
                    i++;
                }

                double r = CorrelationCoefficient(x, y);


                string[] strength = { "perfect", "very strong", "strong", "moderate", "weak" };
                string[] sign = { "positive", "negative", "no" };

                // describing the strength of linear correlation using r 
                if (r == 1.0) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[0], sign[0]); }
                else if (r >= 0.95 && r < 1) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[1], sign[0]); }
                else if (r >= 0.87 && r < 0.95) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[2], sign[0]); }
                else if (r >= 0.5 && r < 0.87) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[3], sign[0]); }
                else if (r >= 0.1 && r < 0.5) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[4], sign[0]); }
                else if (r >= 0.0 && r < 0.1) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} correlation", r, sign[2]); }
                else if (r == -1.0) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[0], sign[1]); }
                else if (r > -1.0 && r <= -0.95) { result.WriteLine("Correlation Coefficient: {0}  " + "-> {1} {2} correlation", r, strength[1], sign[1]); }
                else if (r > -0.95 && r <= -0.87) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[2], sign[1]); }
                else if (r > -0.87 && r <= -0.5) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[3], sign[1]); }
                else if (r > -0.5 && r <= -0.1) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} {2} correlation", r, strength[4], sign[1]); }
                else if (r > -0.1 && r <= -0.0) { result.WriteLine("Correlation Coefficient: {0} " + "-> {1} correlation", r, sign[2]); }

                result.WriteLine("");
                // the sign of r indicates the direction of the correlation
                result.Write("The direction of the correlation -> ");

                if (r >= 0.1)
                {
                    result.WriteLine("Because sign of r is +, the variables are {0}ly correlated, so " +
                                       "an increase in one of the variables will result in an increase in the other.\n", sign[0]);
                }
                else if (r <= -0.1)
                {
                    result.WriteLine("Because sign of r is -, the variables are {0}ly correlated, so " +
                                       "an increase in one of the variables will result in a decrease in the other.\n", sign[1]);
                }
                else if (r < 0.1 || r > -0.1)
                {
                    result.WriteLine("The points are randomly scattered, with no upward or downward linear trend.\n"); ;
                }


                double a = Intercept_a(x, y);
                double b = Slope_b(x, y);

                if (b >= 0)
                {
                    result.WriteLine("Regression equation: ŷ  = {0} + {1} x", a, b);

                }
                else
                {
                    result.WriteLine("Regression equation: ŷ  = {0} - {1} x", a, (-1) * b);
                }

                data.Close();
                result.Close();

            }
            else { Console.WriteLine("Data file error!"); }



            Console.ReadLine();

        }


        /* Correlation Coefficient (r):

         r = (n * sum_XY - sum_X*Sum_Y) / (sqrt( (n * squareSum_X - sum_X * sum_X) * (n * squareSum_Y - sum_Y * sum_Y) )) ... (*)

        */

        static double CorrelationCoefficient(double[] x, double[] y)
        {

            double sum_X = 0, sum_Y = 0, sum_XY = 0;
            double squareSum_X = 0, squareSum_Y = 0;

            int n = x.Length;

            for (int i = 0; i < n; i++)
            {
                sum_X = sum_X + x[i];  // sum of elements of array x
                sum_Y = sum_Y + y[i];  // sum of elements of array y

                sum_XY = sum_XY + x[i] * y[i]; // sum of x[i] * y[i]

                // sum of square of array elements
                squareSum_X = squareSum_X + x[i] * x[i];
                squareSum_Y = squareSum_Y + y[i] * y[i];
            }

            // use formula (*) for calculating correlation coefficient 

            double r = (n * sum_XY - sum_X * sum_Y) /
                      (Math.Sqrt((n * squareSum_X - sum_X * sum_X) * (n * squareSum_Y - sum_Y * sum_Y)));

            return r;
        }


        /* simple linear regresion model: ŷ  = a + bx ... (**)
         
         slope: b = ( n * sum_XY - sum_X * sum_Y ) / ( n * squareSum_X - sum_X * sum_X ) ... (**1) 
         intercept: a = ( sum_Y - b * sum_X ) / n ... (**2) 
         
         */

        static double Slope_b(double[] x, double[] y)
        {
            double sum_X = 0, sum_Y = 0, sum_XY = 0;
            double squareSum_X = 0, squareSum_Y = 0;

            int n = x.Length;

            for (int i = 0; i < n; i++)
            {
                sum_X = sum_X + x[i];
                sum_Y = sum_Y + y[i];
                sum_XY = sum_XY + x[i] * y[i];
                squareSum_X = squareSum_X + x[i] * x[i];
                squareSum_Y = squareSum_Y + y[i] * y[i];
            }

            // use formula (**1) for calculating slope b

            double b = (n * sum_XY - sum_X * sum_Y) / (n * squareSum_X - sum_X * sum_X);

            return b;
        }



        static double Intercept_a(double[] x, double[] y)
        {
            double sum_X = 0, sum_Y = 0;
            int n = x.Length;

            for (int i = 0; i < n; i++)
            {
                sum_X = sum_X + x[i];
                sum_Y = sum_Y + y[i];
            }

            // use formula (**2) for calculating regression coefficient b

            double a = (sum_Y - Slope_b(x, y) * sum_X) / n;

            return a;
        }
    }
}
