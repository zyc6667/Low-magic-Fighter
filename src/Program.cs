﻿using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Low_magic_Fighter
{
    class Program
    {
        static void Main(string[] args)
        {
            // 所有可选英雄
            List<Hero> allHeroes = new List<Hero>
            {
                new Warrior(),
                new Mage(),
                new Archer(),
                new Assassin(),
                new Paladin(),
                new Berserker()
            };

            int heroesPerPlayer = 3; // 每个玩家选择英雄数量，可根据需求修改
            int index = 0;

            Console.WriteLine("0：开始游戏\n1：查看英雄手册");
            while (true)
            {
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out index))
                {
                    Console.WriteLine("输入无效，请输入数字编号。");
                    continue;
                }

                if (index == 0)
                {
                    // 创建选择英雄实例
                    ChooseHero chooser = new ChooseHero(allHeroes, heroesPerPlayer);

                    // 开始选择，返回两个玩家的英雄列表
                    var (player1Heroes, player2Heroes) = chooser.StartSelection(); //这是个元组tuple

                    // 创建游戏并启动
                    Game game = new Game(player1Heroes, player2Heroes);
                    game.StartGame();
                }
                else if (index == 1)
                {
                    string fileName = Directory.GetCurrentDirectory() + "\\doc\\英雄手册.pdf";
                     Process.Start(new ProcessStartInfo
                    {
                        FileName = fileName,
                        UseShellExecute = true // 关键，使用系统默认程序打开
                    });
                }
                else
                {
                    Console.WriteLine("未知命令");
                }
            }
        }
    }
}

