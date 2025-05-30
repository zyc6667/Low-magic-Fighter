/*定义了英雄模板*/
using System;

namespace Low_magic_Fighter
{
    public abstract class Hero
    {
        public string? Name { get; set; }
        public int HeroIndex { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public List<ISkill> Skills { get; set; } = new List<ISkill>();
        public IPassive? Passive { get; set; }

        public virtual void TakeDamage(int damage) //virtual关键字表面该函数可被override
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
