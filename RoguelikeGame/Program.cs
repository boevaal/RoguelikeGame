using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}

class Game
{
    private Player player;
    private Random random;
    private List<Enemy> enemies;
    private List<Weapon> weapons;
    private List<Aid> aids;

    public Game()
    {
        player = new Player("Герой", 100, 100, 1, 0);
        random = new Random();
        enemies = new List<Enemy>();
        weapons = new List<Weapon>();
        aids = new List<Aid>();
        InitGame();
    }

    public void InitGame()
    {
        weapons.Add(new Weapon("Меч", 15, 10));
        weapons.Add(new Weapon("Лук", 10, 5));
        weapons.Add(new Weapon("Топор", 20, 8));

        aids.Add(new Aid("Малая аптечка", 20));
        aids.Add(new Aid("Средняя аптечка", 40));
        aids.Add(new Aid("Большая аптечка", 60));

        for (int i = 0; i < 5; i++)
        {
            enemies.Add(new Enemy("Враг" + (i + 1), random.Next(30, 100), random.Next(30, 100), weapons[random.Next(weapons.Count)]));
        }
    }

    public void Start()
    {
        while (player.Health > 0)
        {
            Console.Clear();
            Console.WriteLine("Игрок: " + player.Name + " | Здоровье: " + player.Health + "/" + player.MaxHealth + " | Очки: " + player.Points);
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Сразиться с случайным врагом");
            Console.WriteLine("2. Использовать аптечку");
            Console.WriteLine("3. Выйти из игры");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Fight();
                    break;
                case "2":
                    UseHealingPotion();
                    break;
                case "3":
                    Console.WriteLine("Игра окончена! Финальный счет: " + player.Points);
                    return;
                default:
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                    break;
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("Вы проиграли! Финальный счет: " + player.Points);
                break;
            }
        }
    }

    private void Fight()
    {
        Enemy enemy = enemies[random.Next(enemies.Count)];
        Console.WriteLine("Появился враг: " + enemy.Name);
        Console.WriteLine("Здоровье врага: " + enemy.Health);

        while (enemy.Health > 0 && player.Health > 0)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Атаковать с оружием");
            Console.WriteLine("2. Сбежать");
            string action = Console.ReadLine();

            if (action == "1")
            {
                Weapon selectedWeapon = weapons[random.Next(weapons.Count)];
                Console.WriteLine("Вы атаковали с помощью " + selectedWeapon.Name + "!");
                int damage = selectedWeapon.Damage;
                enemy.Health -= damage;
                Console.WriteLine("Вы нанесли " + damage + " урона!");
                Console.WriteLine("Здоровье врага: " + enemy.Health);

                if (enemy.Health <= 0)
                {
                    Console.WriteLine("Вы победили врага " + enemy.Name + "!");
                    player.Points += 10;
                    break;
                }

                enemy.Attack(player);
                Console.WriteLine("Враг нанёс вам " + enemy.Damage + " урона. Ваше здоровье: " + player.Health);
            }
            else if (action == "2")
            {
                Console.WriteLine("Вы сбежали от врага!");
                break;
            }
            else
            {
                Console.WriteLine("Неверный выбор действия. Попробуйте снова.");
            }
        }
    }

    private void UseHealingPotion()
    {
        if (player.HealPotions > 0)
        {
            Aid potion = aids[random.Next(aids.Count)];
            player.Health += potion.HealAmount;
            if (player.Health > player.MaxHealth) player.Health = player.MaxHealth;
            player.HealPotions--;
            Console.WriteLine("Вы использовали " + potion.Name + ". Восстановлено " + potion.HealAmount + " здоровья.");
        }
        else
        {
            Console.WriteLine("Нет аптечек!");
        }
    }
}

class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int HealPotions { get; set; }
    public int Points { get; set; }

    public Player(string name, int health, int maxHealth, int healPotions, int points)
    {
        Name = name;
        Health = health;
        MaxHealth = maxHealth;
        HealPotions = healPotions;
        Points = points;
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth) Health = MaxHealth;
    }
}

class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }

    public int Damage => Weapon.Damage;

    public Enemy(string name, int health, int maxHealth, Weapon weapon)
    {
        Name = name;
        Health = health;
        MaxHealth = maxHealth;
        Weapon = weapon;
    }

    public void Attack(Player player)
    {
        player.Health -= Damage;
    }
}

class Aid
{
    public string Name { get; set; }
    public int HealAmount { get; set; }

    public Aid(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }
}

class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Durability { get; set; }

    public Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
    }
}
