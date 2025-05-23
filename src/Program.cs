﻿using System;
using System.Collections.Generic;

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

            // 创建选择英雄实例
            ChooseHero chooser = new ChooseHero(allHeroes, heroesPerPlayer);

            // 开始选择，返回两个玩家的英雄列表
            var (player1Heroes, player2Heroes) = chooser.StartSelection(); //这是个元组tuple

            // 创建游戏并启动
            Game game = new Game(player1Heroes, player2Heroes);
            game.StartGame();
        }
    }
}

