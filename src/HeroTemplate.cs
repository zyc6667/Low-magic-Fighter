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
        public List<int> SkillCooldowns { get; set; } = new List<int>(); //每个技能的剩余冷却回合数
        public bool IsUntargetable { get; set; } = false; //本回合是否无法被选为目标
        public int TurnCount { get; set; } = 0; //该英雄已行动的回合数
        public bool LastTurnGaveUp { get; set; } = false; //上一回合是否主动放弃攻击

        //构造函数后初始化冷却
        public void InitCooldowns()
        {
            SkillCooldowns = new List<int>();
            for (int i = 0; i < Skills.Count; i++)
                SkillCooldowns.Add(0);
        }

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
                if (SkillCooldowns.Count != Skills.Count) InitCooldowns();
                if (SkillCooldowns[skillIndex] > 0)
                {
                    Console.WriteLine($"技能 {Skills[skillIndex].SkillName} 还在冷却中，剩余 {SkillCooldowns[skillIndex]} 回合。");
                    LastTurnGaveUp = false;
                    return;
                }
                Skills[skillIndex].Activate(this, target);
                SkillCooldowns[skillIndex] = Skills[skillIndex].Cooldown;
                LastTurnGaveUp = false;
                TurnCount++;
            }
        }

        //每回合结束时冷却-1
        public void CooldownTick()
        {
            for (int i = 0; i < SkillCooldowns.Count; i++)
            {
                if (SkillCooldowns[i] > 0) SkillCooldowns[i]--;
            }
        }
    }

    public interface IPassive
    {
        string PassiveName { get; }
        string Description { get; } //被动技能详细描述
        void ApplyEffect(Hero user); // 回合开始时应用
    }
    
    public interface ISkill
    {
        string SkillName { get; }
        int Cooldown { get; } //冷却
        string Description { get; } //技能详细描述
        void Activate(Hero user, Hero target);
    }

}
