//刺客
using System;

namespace Low_magic_Fighter
{
    class Assassin : Hero
    {
        public Assassin()
        {
            Name = "刺客";
            Health = 110;
            MaxHealth = 110;
            Attack = 28;
            Defense = 6;
            Passive = new AssassinPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new CriticalStrikeSkill());
        }

        class AssassinPassive : IPassive //被动：背刺
        {
            public string PassiveName => "【被动】背刺";
            public string Description => "如果上一回合主动放弃攻击，则本回合进入隐身且下次攻击伤害提升8点。第一回合不会触发。隐身只持续一个回合。";
            private bool buffReady = false;
            public void ApplyEffect(Hero user)
            {
                // 只在上一回合主动放弃攻击且不是第一回合时激活buff和隐身
                if (user.TurnCount > 0 && user.LastTurnGaveUp)
                {
                    buffReady = true;
                    user.IsUntargetable = true;
                    Console.WriteLine($"{user.Name} 的 {PassiveName} 激活，下次攻击伤害提升并进入隐身！");
                }
                else
                {
                    buffReady = false;
                    user.IsUntargetable = false;
                }
            }
            // 提供接口给技能判断是否加伤害
            public bool IsBuffReady() => buffReady;
            public void ConsumeBuff() { buffReady = false; }
        }
        
        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "刺客的基础攻击，造成等同于攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack;
                // 检查被动
                if (user.Passive is AssassinPassive ap && ap.IsBuffReady())
                {
                    damage += 8;
                    ap.ConsumeBuff();
                }
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }
        
        class CriticalStrikeSkill : ISkill //一技能：致命一击
        {
            public string SkillName => "致命一击";
            public int Cooldown => 2;
            public string Description => "高暴击率技能，70%概率造成2.5倍伤害，否则1.5倍伤害。只有主动放弃攻击后才可触发被动。";
            public void Activate(Hero user, Hero target)
            {
                Random random = new Random();
                int critChance = random.Next(1, 101); // 1-100的随机数
                int damage;
                if (critChance <= 70)
                    damage = (int)(user.Attack * 2.5);
                else
                    damage = (int)(user.Attack * 1.5);
                // 检查被动
                if (user.Passive is AssassinPassive ap2 && ap2.IsBuffReady())
                {
                    damage += 8;
                    ap2.ConsumeBuff();
                }
                target.TakeDamage(damage);
                if (critChance <= 70)
                    Console.WriteLine($"{user.Name} 的 {SkillName} 暴击！对 {target.Name} 造成了 {damage} 点致命伤害！");
                else
                    Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害。");
            }
        }

    }

}