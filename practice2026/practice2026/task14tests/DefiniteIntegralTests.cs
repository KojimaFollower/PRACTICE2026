using task14;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void Test_Xfunction_CorrectAnswer()
        {
            var X = (double x) => x;
            Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
            Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
        }
        [Fact]
        public void Test_QuadraticFunction_CorrectAnswer()
        {
            // Функция f(x) = x^2
            var X2 = (double x) => x * x;
            Assert.Equal(9.0, DefiniteIntegral.Solve(0, 3, X2, 1e-5, 4), 1e-4);
            Assert.Equal(333.3333, DefiniteIntegral.Solve(0, 10, X2, 1e-6, 6), 1e-4);
        }
        [Fact]
        public void Test_ZeroInterval_ReturnsZero()
        {
            var X = (double x) => Math.Sin(x);

            Assert.Equal(0, DefiniteIntegral.Solve(5, 5, X, 1e-3, 2), 1e-4);
        }
        [Fact]
        public void Test_CosFunction_CorrectAnswer()
        {
            var cos = (double x) => Math.Cos(x);
            Assert.Equal(1.0, DefiniteIntegral.Solve(0, Math.PI / 2, cos, 1e-6, 4), 1e-4);
            Assert.Equal(0.0, DefiniteIntegral.Solve(0, 2 * Math.PI, cos, 1e-6, 8), 1e-4);
        }
        [Fact]
        public void Test_ExponentFunction_CorrectAnswer()
        {
            var exp = (double x) => Math.Exp(x);
            double expected1 = Math.E - 1.0;
            Assert.Equal(expected1, DefiniteIntegral.Solve(0, 1, exp, 1e-6, 4), 1e-4);
            double expected2 = Math.Exp(2) - 1.0;
            Assert.Equal(expected2, DefiniteIntegral.Solve(0, 2, exp, 1e-6, 8), 1e-4);
        }
        [Fact]
        public void Test_ManyThreads_And_TinyStep()
        {
            var X2 = (double x) => x * x;
            Assert.Equal(0.333333, DefiniteIntegral.Solve(0, 1, X2, 1e-7, 32), 1e-5);
            Assert.Equal(2.666666, DefiniteIntegral.Solve(0, 2, X2, 1e-6, 64), 1e-5);
        }
        [Fact]
        public void Test_OddNumberOfThreads_CorrectRemainder()
        {
            var f = (double x) => x * x + x;
            Assert.Equal(4.66666, DefiniteIntegral.Solve(0, 2, f, 1e-6, 3), 1e-5);
            Assert.Equal(12.66666, DefiniteIntegral.Solve(1, 3, f, 1e-6, 7), 1e-5);
        }
    }
}
