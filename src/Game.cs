namespace Low_magic_Fighter
{
    public class Game
    {
        public List<Hero> Heroes { get; set; }
        public int CurrentTurn { get; set; }

        public Game(List<Hero> heroes)
        {
            Heroes = heroes;
            CurrentTurn = 0;
        }

        public void StartGame()
        {
            while (Heroes.Count > 1)
            {
                Hero currentHero = Heroes[CurrentTurn % Heroes.Count];

                Console.WriteLine($"{currentHero.Name} 的回合");
                currentHero.Passive?.ApplyEffect(currentHero); // 应用被动效果
                Console.WriteLine("请选择技能：");     
                string? input = Console.ReadLine();
                // 尝试将输入的字符串转换为整数 
                bool isNumber = int.TryParse(input, out int skillIndex);
                if (!isNumber)
                {
                    Console.WriteLine("输入无效，请确保您输入的是一个有效的整数。");
                    break;
                }
                Console.WriteLine("请选择技能对象：");     
                string? input2 = Console.ReadLine();
                // 尝试将输入的字符串转换为整数
                bool isNumber2 = int.TryParse(input2, out int heroIndex);
                if (!isNumber2)
                {
                    Console.WriteLine("输入无效，请确保您输入的是一个有效的整数。");
                    break;
                }
                Hero targetHero = Heroes[heroIndex % Heroes.Count];
                currentHero.UseSkill(skillIndex,targetHero);


                if (targetHero.Health <= 0)
                {
                    Console.WriteLine($"{targetHero.Name} 被击败！");
                    Heroes.Remove(targetHero);
                }

                CurrentTurn++;
                Console.WriteLine();
            }

            Console.WriteLine($"游戏结束，胜利者是 {Heroes[0].Name}!");
        }
    }
}