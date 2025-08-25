using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Project/Skill/Create SkillData")]
    public class SkillData : ScriptableObject
    {
        [SerializeField] private List<SkillInfo> skills;

        public List<SkillInfo> Skills => skills;

        public SkillInfo GetSkill(SkillType type)
        {
            return skills.Find(s => s.skillType == type);
        }
    }

    [Serializable]
    public struct SkillInfo
    {
        public SkillType skillType;

        public int coinCost;

        public int maxLevel;

        public float cooldownTime;
    }

    public enum SkillType
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4
    }
}