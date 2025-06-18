//圣骑士
using System;

namespace Low_magic_Fighter
{
    class Paladin : Hero
    {
        public Paladin()
        {
            Name = "圣骑士";
            Health = 150;
            MaxHealth = 150;
            Attack = 20;
            Defense = 10;
            Passive = new BerserkerPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new SlashSkill());
        }

        class BerserkerPassive : IPassive //被动：狂战士之怒
        {
            public string PassiveName => "Berserker's Rage";

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

            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }

        class SlashSkill : ISkill //一技能：猛击
        {
            public string SkillName => "Slash";
            public int Cooldown => 1;

            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack * 2; // 示例公式：双倍攻击力伤害
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} used {SkillName} on {target.Name} for {damage} damage.");
            }
        }

    }

}