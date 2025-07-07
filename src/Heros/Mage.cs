//法师
using System;

namespace Low_magic_Fighter
{
    class Mage : Hero
    {
        public Mage()
        {
            Name = "法师";
            Health = 120;
            MaxHealth = 120;
            Attack = 25;
            Defense = 5;
            Shield = 20;
            MaxShield = 20;
            Passive = new MagePassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new FireballSkill());
        }

        class MagePassive : IPassive //被动：魔法护盾
        {
            public string PassiveName => "【被动】魔法护盾";
            public string Description => "每回合开始时自动回复5点护盾值，直到护盾值满。";

            public void ApplyEffect(Hero user)
            {
                // 每回合开始时，如果护盾未满，自动回复护盾
                if (user.Shield < user.MaxShield)
                {
                    int restoreAmount = Math.Min(5, user.MaxShield - user.Shield);
                    user.Shield += restoreAmount;
                    Console.WriteLine($"{user.Name} 的 {PassiveName} 回复了 {restoreAmount} 点护盾值.");
                }
            }
        }

        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "法师的基础攻击，造成等同于攻击力的魔法伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 发射 {SkillName} 对 {target.Name} 造成了 {damage} 点魔法伤害.");
            }
        }

        class FireballSkill : ISkill //一技能：火球术
        {
            public string SkillName => "火球术";
            public int Cooldown => 2;
            public string Description => "造成1.8倍攻击力的魔法伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = (int)(user.Attack * 1.8); // 1.8倍攻击力伤害
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 释放 {SkillName}，炽热的火球对 {target.Name} 造成了 {damage} 点伤害！");
            }
        }

    }

}