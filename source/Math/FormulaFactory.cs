using STK.Formula;
using System.Collections.Generic;


namespace GenshinSim.Math
{
    public sealed class FormulaFactory
    {
        public static readonly Dictionary<string, Formula> COMMON_FORMULAS;


        private static GenshinFormulaLexer lexer = new GenshinFormulaLexer();
        private static GenshinFormulaParser parser = new GenshinFormulaParser();


        static FormulaFactory()
        {
            COMMON_FORMULAS = new Dictionary<string, Formula> { { "ATTACK_TOTAL" , Parse("[Attack_Base]*(1+[Attack_Percentage])+[Attack_Extra]") },
                                                                { "DEFENSE_TOTAL", Parse("[Defense_Base]*(1+[Defense_Percentage])+[Defense_Extra]") },
                                                                { "HEALTH_TOTAL" , Parse("[Health_Base]*(1+[Health_Percentage])+[Health_Extra]") } };

            COMMON_FORMULAS.Add("STANDARD_NONCRITICAL_VALUE_ATTACK", Parse("({ATTACK_TOTAL}*[SkillMultiplier])*(1+[DamageModifier_NormalAttack]+[DamageModifier_PhysicalDamage]+[DamageModifier_All])"));
            COMMON_FORMULAS.Add("STANDARD_NONCRITICAL_VALUE_DEFENSE", Parse("({DEFENSE_TOTAL}*[SkillMultiplier])*(1+[DamageModifier_NormalAttack]+[DamageModifier_PhysicalDamage]+[DamageModifier_All])"));
            COMMON_FORMULAS.Add("STANDARD_NONCRITICAL_VALUE_HEALTH", Parse("({HEALTH_TOTAL}*[SkillMultiplier])*(1+[DamageModifier_NormalAttack]+[DamageModifier_PhysicalDamage]+[DamageModifier_All])"));

            COMMON_FORMULAS.Add("STANDARD_EXPECTED_VALUE_ATTACK", Parse("{STANDARD_NONCRITICAL_VALUE_ATTACK}*(1+[CriticalRate]*[CriticalDamage])"));
            COMMON_FORMULAS.Add("STANDARD_EXPECTED_VALUE_DEFENSE", Parse("{STANDARD_NONCRITICAL_VALUE_DEFENSE}*(1+[CriticalRate]*[CriticalDamage])"));
            COMMON_FORMULAS.Add("STANDARD_EXPECTED_VALUE_HEALTH", Parse("{STANDARD_NONCRITICAL_VALUE_HEALTH}*(1+[CriticalRate]*[CriticalDamage])"));

            COMMON_FORMULAS.Add("STANDARD_CRITICAL_VALUE_ATTACK", Parse("{STANDARD_NONCRITICAL_VALUE_ATTACK}*(1+[CriticalDamage])"));
            COMMON_FORMULAS.Add("STANDARD_CRITICAL_VALUE_DEFENSE", Parse("{STANDARD_NONCRITICAL_VALUE_DEFENSE}*(1+[CriticalDamage])"));
            COMMON_FORMULAS.Add("STANDARD_CRITICAL_VALUE_HEALTH", Parse("{STANDARD_NONCRITICAL_VALUE_HEALTH}*(1+[CriticalDamage])"));
        }


        public static Formula Parse(string input)
        {
            return parser.Parse(lexer.GenerateTokens(input));
        }

        public static Formula GetCommonFormula(string name)
        {
            Formula result;
            if (COMMON_FORMULAS.TryGetValue(name.ToUpper(), out result))
            {
                return result;
            }

            return Formula.NONE;
        }
    }
}
