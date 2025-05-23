namespace Low_magic_Fighter
{
    public class Game
    {
        private List<Hero> player1Heroes;
        private List<Hero> player2Heroes;

        private int currentRoundIndex; // 当前轮到第几个英雄出招，范围0 ~ player1Heroes.Count-1
        private bool isPlayer1Turn;    // true表示玩家1回合，false玩家2回合

        public Game(List<Hero> player1Heroes, List<Hero> player2Heroes)
        {
            if (player1Heroes == null || player2Heroes == null)
                throw new ArgumentNullException("玩家英雄列表不能为空");

            if (player1Heroes.Count != player2Heroes.Count)
                throw new ArgumentException("两个玩家的英雄数量必须相等");

            if (player1Heroes.Count == 0)
                throw new ArgumentException("英雄列表不能为空");

            this.player1Heroes = new List<Hero>(player1Heroes);
            this.player2Heroes = new List<Hero>(player2Heroes);

            currentRoundIndex = 0;
            isPlayer1Turn = true;
        }

        public void StartGame()
        {
            while (player1Heroes.Count > 0 && player2Heroes.Count > 0)
            {
                // 当前行动英雄
                Hero currentHero = isPlayer1Turn ? player1Heroes[currentRoundIndex] : player2Heroes[currentRoundIndex];
                Console.WriteLine($"{currentHero.Name} 的回合");

                currentHero.Passive?.ApplyEffect(currentHero);

                int skillIndex;
                while (true)
                {
                    Console.WriteLine("请选择技能编号：");
                    string? input = Console.ReadLine();
                    if (int.TryParse(input, out skillIndex) && skillIndex >= 0)
                        break;
                    Console.WriteLine("输入无效，请输入非负整数。");
                }

                // 目标英雄只能是对方存活的英雄
                List<Hero> opponentHeroes = isPlayer1Turn ? player2Heroes : player1Heroes;

                int heroIndex;
                while (true)
                {
                    Console.WriteLine("请选择技能目标英雄编号：");
                    for (int i = 0; i < opponentHeroes.Count; i++)
                    {
                        Console.WriteLine($"{i}: {opponentHeroes[i].Name} (生命值: {opponentHeroes[i].Health})");
                    }
                    string? input2 = Console.ReadLine();
                    if (int.TryParse(input2, out heroIndex) && heroIndex >= 0 && heroIndex < opponentHeroes.Count)
                        break;
                    Console.WriteLine($"输入无效，请输入0到{opponentHeroes.Count - 1}之间的数字。");
                }

                Hero targetHero = opponentHeroes[heroIndex];
                currentHero.UseSkill(skillIndex, targetHero);

                if (targetHero.Health <= 0)
                {
                    Console.WriteLine($"{targetHero.Name} 被击败！");
                    opponentHeroes.Remove(targetHero);

                    // 如果对方英雄数量减少，且当前索引超出范围，调整索引
                    if (currentRoundIndex >= opponentHeroes.Count)
                    {
                        currentRoundIndex = 0;
                    }
                }

                // 切换回合
                if (!isPlayer1Turn)
                {
                    // 玩家2回合结束，轮到玩家1的下一个英雄
                    currentRoundIndex = (currentRoundIndex + 1) % player1Heroes.Count;
                }
                isPlayer1Turn = !isPlayer1Turn;

                Console.WriteLine();
            }

            // 判断胜负
            if (player1Heroes.Count == 0 && player2Heroes.Count == 0)
            {
                Console.WriteLine("双方英雄全部阵亡，平局！");
            }
            else if (player1Heroes.Count == 0)
            {
                Console.WriteLine("游戏结束，玩家2获胜！");
            }
            else
            {
                Console.WriteLine("游戏结束，玩家1获胜！");
            }
        }
    }

}