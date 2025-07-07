//弓箭手
using System;

namespace Low_magic_Fighter
{
    class Archer : Hero
    {
        public Archer()
        {
            Name = "弓箭手";
            Health = 130;
            MaxHealth = 130;
            Attack = 22;
            Defense = 8;
            Passive = new ArcherPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new MultiShotSkill());
        }

        class ArcherPassive : IPassive //被动：精准射击
        {
            public string PassiveName => "【被动】精准射击";
            public string Description => "连续命中2次后，攻击力提升5点。";
            private int consecutiveHits = 0;

            public void ApplyEffect(Hero user)
            {
                // 连续命中时增加攻击力
                if (consecutiveHits >= 2)
                {
                    user.Attack += 5;
                    Console.WriteLine($"{user.Name} 的 {PassiveName} 激活，攻击力提升5点！");
                }
                else
                {
                    consecutiveHits = 0;
                }
            }
        }
        
        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "弓箭手的基础攻击，造成等同于攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }
        
        class MultiShotSkill : ISkill //一技能：连珠箭
        {
            public string SkillName => "连珠箭";
            public int Cooldown => 3;
            public string Description => "连续射出3支箭，每支造成60%攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int baseDamage = user.Attack;
                int totalDamage = 0;
                for (int i = 1; i <= 3; i++)
                {
                    int arrowDamage = (int)(baseDamage * 0.6); // 每支箭60%攻击力
                    target.TakeDamage(arrowDamage);
                    totalDamage += arrowDamage;
                    Console.WriteLine($"第{i}支箭命中，造成 {arrowDamage} 点伤害！");
                }
                Console.WriteLine($"{user.Name} 释放 {SkillName}，总共对 {target.Name} 造成了 {totalDamage} 点伤害！");
            }
        }

    }

}