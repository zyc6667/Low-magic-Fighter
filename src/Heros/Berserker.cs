//狂战士
using System;

namespace Low_magic_Fighter
{
    class Berserker : Hero
    {
        public Berserker()
        {
            Name = "狂战士";
            Health = 140;
            MaxHealth = 140;
            Attack = 25;
            Defense = 8;
            Passive = new BerserkerPassive();
            Skills.Add(new NormalAttack());
            Skills.Add(new BerserkerRageSkill());
        }

        class BerserkerPassive : IPassive //被动：狂战士之怒
        {
            public string PassiveName => "【被动】狂战士之怒";
            public string Description => "当生命值低于最大值50%时，攻击力提升15点。";
            private bool IsPassiveActived=false;
            public void ApplyEffect(Hero user)
            {
                if (user.Health < user.MaxHealth / 2 && IsPassiveActived == false) //如果生命值小于50%
                {
                    user.Attack += 10; // 增加10点攻击力
                    Console.WriteLine($"{user.Name}'s {PassiveName} 已激活!");
                    IsPassiveActived = true;
                }
                else if (user.Health >= user.MaxHealth / 2 && IsPassiveActived == true)
                {
                    user.Attack -= 10; // 减少10点攻击力
                    Console.WriteLine($"{user.Name}'s {PassiveName} 失效了!");
                    IsPassiveActived = false;
                }
            }
        }

        class NormalAttack : ISkill //普通攻击
        {
            public string SkillName => "普通攻击";
            public int Cooldown => 1;
            public string Description => "狂战士的基础攻击，造成等同于攻击力的伤害。";
            public void Activate(Hero user, Hero target)
            {
                int damage = user.Attack; 
                target.TakeDamage(damage);
                Console.WriteLine($"{user.Name} 使用 {SkillName} 对 {target.Name} 造成了 {damage} 点伤害.");
            }
        }

        class BerserkerRageSkill : ISkill //一技能：狂暴冲锋
        {
            public string SkillName => "狂暴冲锋";
            public int Cooldown => 2;
            public string Description => "造成2.2倍攻击伤害，并对自己造成5点伤害，提升自身攻击力。";
            public void Activate(Hero user, Hero target)
            {
                int damage = (int)(user.Attack * 2.2); // 2.2倍攻击力伤害
                target.TakeDamage(damage);
                
                // 狂暴冲锋会对自己造成少量伤害，但大幅提升攻击力
                int selfDamage = 5;
                user.TakeDirectDamage(selfDamage);
                user.Attack += 5;
                
                Console.WriteLine($"{user.Name} 释放 {SkillName}，对 {target.Name} 造成 {damage} 点伤害！");
                Console.WriteLine($"{user.Name} 受到 {selfDamage} 点反伤，但攻击力提升了！");
            }
        }

    }

}