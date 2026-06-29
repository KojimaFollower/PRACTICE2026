namespace task04
{
    public interface ISpaceship
    {
        void Rotate(int angle);
        void Fire();
        int Speed { get; }
        int FirePower { get; }
        
    }
    public class Cruiser : ISpaceship
    {
        public int Speed => 50;         
        public int FirePower => 100;    

        public void MoveForward()
        {
            Console.WriteLine($"Крейсер продвигается вперед со скоростью {Speed} единиц");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Крейсер разворачивается на {angle} градусов");
        }

        public void Fire()
        {
            Console.WriteLine($"Крейсер проивзодит выстрел. Мощность выстрела: {FirePower} единиц");
        }
    }
    public class Fighter : ISpaceship
    {
        public int Speed => 100;      
        public int FirePower => 50;    

        public void MoveForward()
        {
            Console.WriteLine($"Истрибитель продвигается вперед со скоростью {Speed} единиц");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Крейсер разворачивается на {angle} градусов");
        }

        public void Fire()
        {
            Console.WriteLine($"Крейсер проивзодит выстрел. Мощность выстрела: {FirePower} единиц");
        }
    }
}
