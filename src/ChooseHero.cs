using System;
using System.Collections.Generic;

namespace Low_magic_Fighter
{
    public class ChooseHero
    {
        private List<Hero> allHeroes;
        private int heroesPerPlayer;

        public ChooseHero(List<Hero> allHeroes, int heroesPerPlayer)
        {
            if (allHeroes == null || allHeroes.Count == 0)
                throw new ArgumentException("英雄列表不能为空");
            if (heroesPerPlayer <= 0)
                throw new ArgumentException("每个玩家英雄数量必须大于0");
            if (heroesPerPlayer * 2 > allHeroes.Count)
                throw new ArgumentException("英雄总数不足以满足两个玩家的选择");

            this.allHeroes = allHeroes;
            this.heroesPerPlayer = heroesPerPlayer;
        }

        /// <summary>
        /// 开始选择英雄，返回两个玩家的英雄列表
        /// </summary>
        public (List<Hero> player1Heroes, List<Hero> player2Heroes) StartSelection()
        {
            Console.WriteLine("欢迎来到英雄选择环节！");
            Console.WriteLine("可选英雄列表：");
            for (int i = 0; i < allHeroes.Count; i++)
            {
                Console.WriteLine($"{i}: {allHeroes[i].Name}");
            }

            List<Hero> selectedHeroes = new List<Hero>();
            List<Hero> player1Heroes = new List<Hero>();
            List<Hero> player2Heroes = new List<Hero>();

            int totalSelections = heroesPerPlayer * 2;

            for (int selectionTurn = 0; selectionTurn < totalSelections; selectionTurn++)
            {
                int currentPlayer = (selectionTurn % 2) + 1;
                int heroNumber = (selectionTurn / 2) + 1;

                Console.WriteLine($"\n玩家{currentPlayer}请选择你的第{heroNumber}名英雄：");

                while (true)
                {
                    Console.Write("请输入英雄编号（剩余可选英雄编号）：");

                    // 显示剩余可选英雄编号
                    for (int i = 0; i < allHeroes.Count; i++)
                    {
                        if (!selectedHeroes.Contains(allHeroes[i]))
                            Console.Write(i + " ");
                    }
                    Console.WriteLine();

                    string? input = Console.ReadLine();
                    if (int.TryParse(input, out int index))
                    {
                        if (index >= 0 && index < allHeroes.Count)
                        {
                            Hero chosenHero = allHeroes[index];
                            if (selectedHeroes.Contains(chosenHero))
                            {
                                Console.WriteLine("该英雄已被选择，请选择其他英雄。");
                            }
                            else
                            {
                                selectedHeroes.Add(chosenHero);
                                if (currentPlayer == 1)
                                    player1Heroes.Add(chosenHero);
                                else
                                    player2Heroes.Add(chosenHero);

                                Console.WriteLine($"玩家{currentPlayer}选择了 {chosenHero.Name}");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("输入的编号超出范围，请重新输入。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("输入无效，请输入数字编号。");
                    }
                }
            }

            Console.WriteLine("\n所有英雄选择完毕，开始游戏！");
            return (player1Heroes, player2Heroes);
        }
    }
}

