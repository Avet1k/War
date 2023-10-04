namespace War;
class Program
{
    static void Main(string[] args)
    {
        Platoon villarriba = new Platoon("Виллариба");
        Platoon villabaxu = new Platoon("Виллабаджо");
        Battle battle = new Battle(villarriba, villabaxu);
        
        battle.Fight();
    }
}

class Soldier
{
    private int _damage;
    private int _accuracy;
    private int _maxAccuracy = 9;
    private Platoon _platoon;
    private Random _random;

    public Soldier(Platoon platoon)
    {
        int minHealth = 10;
        int maxHealth = 20;
        int minDamage = 5;
        int maxDamage = 10;
        int minAccuracy = 4;
        
        _random = new Random();

        Health = _random.Next(minHealth, maxHealth);
        _damage = _random.Next(minDamage, maxDamage);
        _accuracy = _random.Next(minAccuracy, _maxAccuracy);
        _platoon = platoon;
    }

    public int Health { get; private set;  }
    
    public void Attack(Soldier enemy)
    {
        if (_random.Next(_maxAccuracy) < _accuracy)
            enemy.TakeDamage(_damage);
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health > 0) 
            return;
        
        Health = 0;
        _platoon.RemoveDead(this);
    }
}

class Platoon
{
    private int _capacity = 20;
    private Soldier[] _soldiersForDraw;
    private List<Soldier> _aliveSoldiers;

    public Platoon(string name)
    {
        _soldiersForDraw = new Soldier[_capacity];
        _aliveSoldiers = new List<Soldier>(_capacity);
        
        for (int i = 0; i < _capacity; i++)
        {
            _aliveSoldiers.Add(new Soldier(this));
            _soldiersForDraw[i] = _aliveSoldiers[i];
        }

        Name = name;
    }
    
    public string Name { get; }

    public int GetAliveSoldiersCount()
    {
        return _aliveSoldiers.Count;
    }

    public Soldier GetSoldierByIndex(int index)
    {
        return _aliveSoldiers[index];
    }

    public void DrawSoldiers()
    {
        foreach (Soldier soldier in _soldiersForDraw)
            Console.Write("{0:d2} ", soldier.Health);

        Console.WriteLine();
    }

    public void RemoveDead(Soldier deadSoldier)
    {
        _aliveSoldiers.Remove(deadSoldier);
    }
}

class Battle
{
    private Platoon _firstPlatoon;
    private Platoon _secondPlatoon;

    public Battle(Platoon firstPlatoon, Platoon secondPlatoon)
    {
        _firstPlatoon = firstPlatoon;
        _secondPlatoon = secondPlatoon;
    }

    public void Fight()
    {
        string winner;
        
        while(_firstPlatoon.GetAliveSoldiersCount() > 0 &&
              _secondPlatoon.GetAliveSoldiersCount() > 0)
        {
            Random random = new Random();

            Attack(_firstPlatoon, _secondPlatoon);
            
            if (_secondPlatoon.GetAliveSoldiersCount() > 0)
                Attack(_secondPlatoon, _firstPlatoon);

            Console.Clear();
            
            _firstPlatoon.DrawSoldiers();
            Console.WriteLine("\n\n");
            _secondPlatoon.DrawSoldiers();
            
            Console.WriteLine("\n\nНажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }

        if (_firstPlatoon.GetAliveSoldiersCount() == 0)
            winner = _secondPlatoon.Name;
        else
            winner = _firstPlatoon.Name;
        
        Console.WriteLine($"\n\nПобедил взвод {winner}");
    }

    private void Attack(Platoon attacker, Platoon defender)
    {
        Random random = new Random();
        
        Soldier attackerSoldier = attacker.GetSoldierByIndex(random.Next(attacker.GetAliveSoldiersCount()));
        Soldier defenderSoldier = defender.GetSoldierByIndex(random.Next(defender.GetAliveSoldiersCount()));
        
        attackerSoldier.Attack(defenderSoldier);
    }
}