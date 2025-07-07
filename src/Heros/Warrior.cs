/*基础英雄:战士*/
using System;

namespace Low_magic_Fighter
{
    class Warrior : Hero
    {
        public Warrior()
        {
            Name = "战士";
            Health = 160;
            MaxHealth = 160;
            Attack = 22;
            Defense = 12;
            Shield = 15;
            MaxShield = 15;
            Passive = new WarriorPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new BattleCrySkill());
        }

        class WarriorPassive : IPassive //被动：狂战士之怒
        {
            public string PassiveName => "Warrior's Rage";
            public string Description => "当生命值低于最大值50%时，攻击力提升10点。";

            public void ApplyEffect(Hero user)
            {
                if (user.Health < user.MaxHealth / 2) //如果生命值小于50%
                {
                    user.Attack += 10; // 增加10点攻击力
                    Console.WriteLine($"{user.Name}'s {PassiveName} is activated!");
                }
            }
        }

        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "战士的基础攻击，造成等同于攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }

        class BattleCrySkill : ISkill //一技能：战斗怒吼
        {
            public string SkillName => "战斗怒吼";
            public int Cooldown => 2;
            public string Description => "造成1.7倍攻击伤害，并提升自身防御力。";
            public void Activate(Hero user, Hero target)
            {
                int damage = (int)(user.Attack * 1.7); // 1.7倍攻击力伤害
                target.TakeDamage(damage);
                
                // 战斗怒吼同时提升自己的防御力
                user.Defense += 3;
                Console.WriteLine($"{user.Name} 释放 {SkillName}，对 {target.Name} 造成 {damage} 点伤害，同时提升了防御力！");
            }
        }

    }

}