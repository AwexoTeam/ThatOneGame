using Newtonsoft.Json;
using RpgGame.Script.Utils.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public class Entity
    {
        public string name;
        public Dictionary<Stats, float> stats;

        public void Initialize(string name)
        {
            this.name = name;
            stats = new Dictionary<Stats, float>();

            var values = Enum.GetValues(typeof(Stats)).Cast<Stats>();
            foreach (var stat in values)
            {
                if (stats.ContainsKey(stat))
                    continue;

                int id = (int)stat;
                float value = id < 10 ? 1 : 0;

                stats.Add(stat, value);
            }
            SetStartStats();
            AdjustStats();
        }

        public void SetStartStats()
        {

            stats[Stats.Max_Hp] = 100;
            stats[Stats.Max_Mp] = 100;
            stats[Stats.Max_Sp] = 100;

            stats[Stats.Hp] = stats[Stats.Max_Hp];
            stats[Stats.Mp] = stats[Stats.Max_Mp];
            stats[Stats.Sp] = stats[Stats.Max_Sp];
        }

        public bool isDead() => GetStat(Stats.Hp) <= 0;


        public void AdjustStats()
        {
            var values = Enum.GetValues(typeof(Stats)).Cast<Stats>();
            foreach (var stat in values)
            {
                if (stats.ContainsKey(stat))
                    continue;

                stats.Add(stat, 1);
            }

            stats[Stats.Hp] = stats[Stats.Max_Hp];
            stats[Stats.Mp] = stats[Stats.Max_Mp];
            stats[Stats.Sp] = stats[Stats.Max_Sp];
        }

        public float GetStat(Stats stat)
        {
            if (!stats.ContainsKey(stat))
            {
                Debug.LogError("Tried to get stat " + stat + " on " + name);
                return 0;
            }

            return stats[stat];
        }

        public void SetStat(Stats stat, float val, bool doStatCheck = true)
        {
            if (!stats.ContainsKey(stat))
            {
                Debug.LogError("Tried to set stat " + stat + " on " + name);
                return;
            }

            stats[stat] = val;
            if (!doStatCheck)
                return;

            int id = (int)stat;

            if (id <= 299 && id >= 200)
                CalcBaseStats();

            if (id < 100)
                CheckVitals();
        }

        private void CheckVitals()
        {
            float currHp = GetStat(Stats.Hp);
            float effectiveMaxHp = GetStat(Stats.Max_Hp);
            effectiveMaxHp -= GetStat(Stats.Wound);

            if (currHp > effectiveMaxHp)
                SetStat(Stats.Hp, effectiveMaxHp, false);

            float curr = GetStat(Stats.Mp);
            float max = GetStat(Stats.Max_Mp);

            if (curr > max)
                SetStat(Stats.Mp, max, false);

            curr = GetStat(Stats.Sp);
            max = GetStat(Stats.Max_Sp);

            if (curr > max)
                SetStat(Stats.Sp, max, false);
        }

        public void ModifyStat(Stats stat, float val)
        {
            float curr = GetStat(stat);
            curr += val;

            SetStat(stat, curr);
        }

        public virtual float GetDamage(Entity attacker)
        {
            float min = GetStat(Stats.Min_Damage);
            float max = GetStat(Stats.Max_Damage);

            float balance = GetStat(Stats.Balance);
            if (balance > 0)
            {
                balance /= 100;
                float diff = max - min;
                float adjusted = balance * diff;

                min += adjusted;
            }

            float dmg = Globals.random.NextFloat(min, max);
            float critNum = Globals.random.NextFloat(1, 100);
            float critChance = GetStat(Stats.Critical_Chance);

            if (critNum <= critChance)
            {
                float multiplier = GetStat(Stats.Critical_Damage);
                multiplier /= 100;
                multiplier += 1;

                dmg *= multiplier;
            }

            return dmg;
        }

        public void DoDamage(Entity attacker)
        {
            float dmg = attacker.GetDamage(attacker);
            float def = GetStat(Stats.Defence);

            if (dmg - def <= 0)
                return;

            dmg -= def;


            float prot = GetStat(Stats.Damage_Reduction);
            prot -= attacker.GetStat(Stats.Armor_Pierce);

            if (prot > 0)
            {
                prot /= 100;
                dmg -= prot * dmg;
            }

            if (dmg <= 0)
                return;

            float hpLeech = GetStat(Stats.HP_Leech);
            float mpLeech = GetStat(Stats.MP_Leech);
            float spLeech = GetStat(Stats.SP_Leech);

            hpLeech /= 100;
            mpLeech /= 100;
            spLeech /= 100;

            float mHp = hpLeech * dmg;
            float mMp = mpLeech * dmg;
            float mSp = spLeech * dmg;

            ModifyStat(Stats.Hp, mHp);
            ModifyStat(Stats.Mp, mMp);
            ModifyStat(Stats.Sp, mSp);

            float wound = GetWoundDamage(dmg);

            ModifyStat(Stats.Wound, wound);
            ModifyStat(Stats.Hp, dmg * -1);
        }

        public float GetWoundDamage(float dmg)
        {
            var min = GetStat(Stats.Min_Injury);
            var max = GetStat(Stats.Max_Injury);

            var injury = Globals.random.NextFloat(min, max);
            injury /= 100;

            return dmg * injury;
        }

        public static string GetDefaultJson()
        {
            Entity enemy = new Entity();
            enemy.Initialize("Default");

            var values = Enum.GetValues(typeof(Stats)).Cast<Stats>();
            foreach (var stat in values)
            {
                if (enemy.stats.ContainsKey(stat))
                    continue;

                enemy.stats.Add(stat, 1);
            }

            return JsonConvert.SerializeObject(enemy, Formatting.Indented);
        }

        public void RegenTick()
        {
            float hpRegen = GetStat(Stats.Health_Regen);
            float mpRegen = GetStat(Stats.Mana_Regen);
            float spRegen = GetStat(Stats.Special_Regen);

            ModifyStat(Stats.Hp, hpRegen);
            ModifyStat(Stats.Mp, mpRegen);
            ModifyStat(Stats.Sp, spRegen);
        }

        public void CalcBaseStats()
        {

        }
    }

}
