//圣骑士
using System;

namespace Low_magic_Fighter
{
    class Paladin : Hero
    {
        public Paladin()
        {
            Name = "圣骑士";
            Health = 180;
            MaxHealth = 180;
            Attack = 18;
            Defense = 15;
            Shield = 30;
            MaxShield = 30;
            Passive = new PaladinPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new HolyStrikeSkill());
        }

        class PaladinPassive : IPassive //被动：神圣护盾
        {
            public string PassiveName => "【被动】神圣护盾";
            public string Description => "生命值低于最大值70%时，每回合自动回复8点护盾值。";

            public void ApplyEffect(Hero user)
            {
                // 每回合开始时，如果生命值低于70%，自动回复护盾
                if (user.Health < user.MaxHealth * 0.7 && user.Shield < user.MaxShield)
                {
                    int restoreAmount = Math.Min(8, user.MaxShield - user.Shield);
                    user.Shield += restoreAmount;
                    Console.WriteLine($"{user.Name} 的 {PassiveName} 激活，回复了 {restoreAmount} 点护盾值.");
                }
            }
        }

        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "圣骑士的基础攻击，造成等同于攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }

        class HolyStrikeSkill : ISkill //一技能：圣光打击
        {
            public string SkillName => "圣光打击";
            public int Cooldown => 2;
            public string Description => "造成1.6倍攻击伤害，并为自己回复护盾。";
            public void Activate(Hero user, Hero target)
            {
                int damage = (int)(user.Attack * 1.6); // 1.6倍攻击力伤害
                target.TakeDamage(damage);
                
                // 圣光打击同时为自己回复护盾
                int shieldRestore = Math.Min(10, user.MaxShield - user.Shield);
                if (shieldRestore > 0)
                {
                    user.Shield += shieldRestore;
                    Console.WriteLine($"{user.Name} 释放 {SkillName}，对 {target.Name} 造成 {damage} 点伤害，同时回复了 {shieldRestore} 点护盾！");
                }
                else
                {
                    Console.WriteLine($"{user.Name} 释放 {SkillName}，对 {target.Name} 造成了 {damage} 点伤害！");
                }
            }
        }

    }

}