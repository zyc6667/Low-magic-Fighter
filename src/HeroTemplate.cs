/*定义了英雄模板*/
using System;

namespace Low_magic_Fighter
{
    public abstract class Hero
    {
        public string? Name { get; set; }
        public int Shield { get; set; } //护盾
        public int MaxShield{ get; set; }//最大护盾值
        public int ShieldReverse { get; set; } = 3;//护盾回复速度，护盾被击碎后不再回复。
        public int Health { get; set; }//生命值，不会回复
        public int MaxHealth { get; set; }//最大生命值
        public int Attack { get; set; }
        public int Defense { get; set; } //防御力
        public List<ISkill> Skills { get; set; } = new List<ISkill>();
        public IPassive? Passive { get; set; } //每个英雄只能有一个被动

        //承受伤害
        public virtual void TakeDamage(int damage) //virtual关键字表明该函数可被override
        {
            int realDamage = Math.Max(damage - Defense, 0);
            if (Shield > realDamage)
            {
                Shield -= realDamage;
            }
            else if (Shield > 0) //护盾不足以完全承受伤害
            {
                int remainDamage = realDamage - Shield;
                Shield =0;
                Health-=remainDamage;
            }
            else
            {
                Health -= realDamage;
            }
        }

        //无视护盾直接对生命值造成伤害
        public virtual void TakeDirectDamage(int damage)
        {
            Health -= Math.Max(damage - Defense, 0);
        }

        //入参：技能序号，目标
        public virtual void UseSkill(int skillIndex, Hero target)
        {
            if (skillIndex >= 0 && skillIndex < Skills.Count)
            {
                Skills[skillIndex].Activate(this, target);
            }
        }
    }

    public interface IPassive
    {
        string PassiveName { get; }
        void ApplyEffect(Hero user); // 回合开始时应用
    }
    
    public interface ISkill
    {
        string SkillName { get; }
        int Cooldown { get; } //冷却
        void Activate(Hero user, Hero target);
    }

}
