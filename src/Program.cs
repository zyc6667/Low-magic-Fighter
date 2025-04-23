using System;
using System.Reflection.Metadata.Ecma335;

namespace Low_magic_Fighter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("hello!");
            // 创建英雄
        Hero warrior = new Warrior();
  

        // 创建游戏
        Game game = new Game(new List<Hero> { warrior});

        // 启动游戏
        game.StartGame();
        }
    }
}
