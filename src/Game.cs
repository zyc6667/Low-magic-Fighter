namespace Low_magic_Fighter
{
    public class Game
    {
        private List<Hero> player1Heroes;
        private List<Hero> player2Heroes;

        private int currentRoundIndex; // 当前轮到第几个英雄出招，范围0 ~ player1Heroes.Count-1
        private bool isPlayer1Turn;    // true表示玩家1回合，false玩家2回合
        private int currentTurn;       // 新增：当前是第几回合
        private List<string> eventLog = new List<string>(); // 主界面事件日志
        private void Log(string msg)
        {
            eventLog.Add(msg);
            Console.WriteLine(msg);
        }

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
            currentTurn = 1; // 初始化回合数为1
        }

        public void StartGame()
        {
            for (int i = 0; i < player1Heroes.Count; i++) //开场前要先检查一遍所有英雄的被动激活情况
            {
                player1Heroes[i].Passive?.ApplyEffect(player1Heroes[i]);
                player1Heroes[i].InitCooldowns();
            }
            for (int i = 0; i < player2Heroes.Count; i++)
            {
                player2Heroes[i].InitCooldowns();
            }
            
            while (player1Heroes.Count > 0 && player2Heroes.Count > 0)
            {
                // 当前行动英雄
                Hero currentHero = isPlayer1Turn ? player1Heroes[currentRoundIndex] : player2Heroes[currentRoundIndex];
                Log($"{currentHero.Name} 的回合");

                currentHero.Passive?.ApplyEffect(currentHero);//每回合都要检查被动激活情况,被动模板参照狂战士

                int skillIndex;
                while (true)
                {
                    Console.WriteLine("可用技能列表：");
                    for (int i = 0; i < currentHero.Skills.Count; i++)
                    {
                        string cd = currentHero.SkillCooldowns.Count > i && currentHero.SkillCooldowns[i] > 0 ? $"(冷却中:{currentHero.SkillCooldowns[i]}回合)" : "";
                        Console.WriteLine($"{i}: {currentHero.Skills[i].SkillName} {cd}");
                    }
                    int giveUpIndex = currentHero.Skills.Count;
                    Console.WriteLine($"{giveUpIndex}: 放弃攻击（本回合跳过）");
                    Console.WriteLine("输入编号选择技能，或输入 d 进入技能详情模式。");
                    Console.WriteLine("请选择技能编号：");
                    string? input = Console.ReadLine();
                    if (input != null && (input.ToLower() == "d" || input.ToLower() == "detail"))
                    {
                        SafeClear();
                        // 展示被动和技能名称列表
                        if (currentHero.Passive != null)
                            Console.WriteLine($"被动技能：{currentHero.Passive.PassiveName}（输入-1或p查看详情）");
                        else
                            Console.WriteLine("无被动技能");
                        for (int i = 0; i < currentHero.Skills.Count; i++)
                        {
                            Console.WriteLine($"{i}: {currentHero.Skills[i].SkillName}");
                        }
                        // 技能详情模式
                        Console.WriteLine("进入技能详情模式，输入技能编号查看描述，输入-1或p查看被动，输入q退出。");
                        while (true)
                        {
                            string? detailInput = Console.ReadLine();
                            if (detailInput != null && (detailInput.ToLower() == "q" || detailInput.ToLower() == "exit"))
                            {
                                SafeClear();
                                // 重绘主界面日志
                                foreach (var line in eventLog) Console.WriteLine(line);
                                Console.WriteLine("退出技能详情模式。");
                                break;
                            }
                            if (detailInput != null && (detailInput == "-1" || detailInput.ToLower() == "p" || detailInput.ToLower() == "passive"))
                            {
                                if (currentHero.Passive != null)
                                {
                                    Console.WriteLine($"被动技能：{currentHero.Passive.PassiveName}\n{currentHero.Passive.Description}");
                                }
                                else
                                {
                                    Console.WriteLine("该英雄没有被动技能。");
                                }
                                continue;
                            }
                            if (int.TryParse(detailInput, out int detailIdx) && detailIdx >= 0 && detailIdx < currentHero.Skills.Count)
                            {
                                var skill = currentHero.Skills[detailIdx];
                                Console.WriteLine($"[{detailIdx}] {skill.SkillName}：{skill.Description} 冷却：{skill.Cooldown}回合");
                            }
                            else
                            {
                                Console.WriteLine("输入无效，请输入技能编号、-1或q退出。");
                            }
                        }
                        continue;
                    }
                    if (int.TryParse(input, out skillIndex) && skillIndex >= 0 && skillIndex <= currentHero.Skills.Count)
                    {
                        if (skillIndex == giveUpIndex)
                        {
                            Console.WriteLine($"{currentHero.Name} 选择放弃攻击，进入防御姿态。");
                            currentHero.LastTurnGaveUp = true;
                            break;
                        }
                        if (currentHero.SkillCooldowns.Count > skillIndex && currentHero.SkillCooldowns[skillIndex] > 0)
                        {
                            Console.WriteLine($"该技能还在冷却中，剩余 {currentHero.SkillCooldowns[skillIndex]} 回合，请重新选择。");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("输入的技能编号无效。");
                }

                if (skillIndex == currentHero.Skills.Count)
                {
                    currentHero.CooldownTick();
                    // 所有英雄隐身状态重置，确保隐身只持续一个回合
                    foreach (var h in player1Heroes) h.IsUntargetable = false;
                    foreach (var h in player2Heroes) h.IsUntargetable = false;
                    goto EndTurn;
                }

                // 目标英雄只能是对方存活且未隐身的英雄
                List<Hero> opponentHeroes = isPlayer1Turn ? player2Heroes : player1Heroes;
                var availableTargets = new List<Hero>();
                for (int i = 0; i < opponentHeroes.Count; i++)
                {
                    if (!opponentHeroes[i].IsUntargetable)
                        availableTargets.Add(opponentHeroes[i]);
                }
                if (availableTargets.Count == 0)
                {
                    Log("对方所有英雄都处于隐身状态，无法攻击，自动跳过本回合。");
                    currentHero.LastTurnGaveUp = false;
                    currentHero.CooldownTick();
                    goto EndTurn;
                }

                int heroIndex;
                while (true)
                {
                    Console.WriteLine("请选择技能目标英雄编号：");
                    for (int i = 0; i < availableTargets.Count; i++)
                    {
                        Console.WriteLine($"{i}: {availableTargets[i].Name} (生命值: {availableTargets[i].Health} 护盾值: {availableTargets[i].Shield})");
                    }
                    string? input2 = Console.ReadLine();
                    if (int.TryParse(input2, out heroIndex) && heroIndex >= 0 && heroIndex < availableTargets.Count)
                        break;
                    Console.WriteLine($"输入无效，请输入0到{availableTargets.Count - 1}之间的数字。");
                }

                Hero targetHero = availableTargets[heroIndex];
                currentHero.UseSkill(skillIndex, targetHero);
                currentHero.LastTurnGaveUp = false;

                // 回合结束，技能冷却-1
                currentHero.CooldownTick();
            EndTurn:
                // 切换回合
                if (!isPlayer1Turn)
                {
                    // 玩家2回合结束，轮到玩家1的下一个英雄
                    currentRoundIndex = (currentRoundIndex + 1) % player1Heroes.Count;
                    // 如果回合索引回到0，说明双方英雄都出完一轮，回合数加1
                    if (currentRoundIndex == 0)
                    {
                        currentTurn++;
                        Log($"第 {currentTurn} 回合开始！");
                        //护盾回复
                        for (int i = 0; i < player1Heroes.Count; i++)
                        {
                            if (player1Heroes[i].Shield <= 0) continue;//护盾破了就不回复了
                            if (player1Heroes[i].Shield + player1Heroes[i].ShieldReverse < player1Heroes[i].MaxShield)
                                player1Heroes[i].Shield += player1Heroes[i].ShieldReverse;
                            else player1Heroes[i].Shield = player1Heroes[i].MaxShield;
                        }
                        for (int i = 0; i < player2Heroes.Count; i++)
                        {
                            if (player2Heroes[i].Shield <= 0) continue;//护盾破了就不回复了
                            if (player2Heroes[i].Shield + player2Heroes[i].ShieldReverse < player2Heroes[i].MaxShield)
                                player2Heroes[i].Shield += player2Heroes[i].ShieldReverse;
                            else player2Heroes[i].Shield = player2Heroes[i].MaxShield;
                        }
                    }
                }
                isPlayer1Turn = !isPlayer1Turn;

                Console.WriteLine();
            }

            // 判断胜负
            if (player1Heroes.Count == 0 && player2Heroes.Count == 0)
            {
                Log("双方英雄全部阵亡，平局！");
            }
            else if (player1Heroes.Count == 0)
            {
                Log("游戏结束，玩家2获胜！");
            }
            else
            {
                Log("游戏结束，玩家1获胜！");
            }
        }

        private void SafeClear()
        {
            try { Console.Clear(); }
            catch { for (int i = 0; i < 100; i++) Console.WriteLine(); }
        }
    }

}