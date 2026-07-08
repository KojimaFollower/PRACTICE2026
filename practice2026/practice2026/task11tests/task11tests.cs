using task11;


namespace task11tests
{
    public class task11tests
    {
        [Fact]
        public void CreateCalculator_ShouldPerformAllArithmeticOperationsCorrectly()
        {
            ICalculator calculator = CalculatorCreator.CreateCalculator();
            Assert.Equal(8, calculator.Add(5, 3));
            Assert.Equal(2, calculator.Minus(5, 3));
            Assert.Equal(15, calculator.Mul(5, 3));
            Assert.Equal(2, calculator.Div(6, 3));
        }
        [Fact]
        public void CreateCalculator_DivisionByZero_ShouldThrowDivideByZeroException()
        {
            ICalculator calculator = CalculatorCreator.CreateCalculator();
            Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
        }
        [Fact]
        public void Div_IntMinValueByMinusOne_ShouldThrowOverflowException()
        {
            ICalculator calculator = CalculatorCreator.CreateCalculator();
            Assert.Throws<OverflowException>(() => calculator.Div(int.MinValue, -1));
        }

        [Fact]
        public void CreateCalculator_Instance_ShouldNotBeNull()
        {
            ICalculator calculator = CalculatorCreator.CreateCalculator();
            Assert.NotNull(calculator);
            Assert.IsAssignableFrom<ICalculator>(calculator);
        }


    }
}
