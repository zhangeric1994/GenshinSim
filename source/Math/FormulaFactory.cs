using STK.Expression;
using System.Collections.Generic;


namespace GenshinSim.Math
{
    public enum DamageType : int
    {
        Noncritical,
        Critical,
        Expected,
    }


    public enum CharacterActionType : int
    {
        NormalAttack,
        ChargedAttack,
        PlungeAttack,
        ElementalSkill,
        ElementalBurst
    }


    public sealed class FormulaFactory
    {
        public static readonly Dictionary<string, MathmaticalExpression> STANDARD_FORMULAS;


        private static GenshinLexer lexer = new GenshinLexer();
        private static GenshinFormulaParser parser = new GenshinFormulaParser();


        static FormulaFactory()
        {
            STANDARD_FORMULAS = new Dictionary<string, MathmaticalExpression> { { "Attack_Total", Parse("[Attack_Base]*(1+[Attack_Percentage])+[Attack_Extra]") },
                                                                                { "Defense_Total", Parse("[Defense_Base]*(1+[Defense_Percentage])+[Defense_Extra]") },
                                                                                { "Health_Total", Parse("[Health_Base]*(1+[Health_Percentage])+[Health_Extra]") },
                                                                                { "CriticalExpectation", Parse("1+[CriticalRate]*[CriticalDamage]")},
                                                                                { "CriticalDamageMultiplier", Parse("1+[CriticalDamage]")} };


            for (CharacterActionType characterActionType = CharacterActionType.NormalAttack; characterActionType <= CharacterActionType.ElementalBurst; ++characterActionType)
            {
                for (ElementType elementType = ElementType.Physical; elementType <= ElementType.Geo; ++elementType)
                {
                    string damageModifierName = string.Format("DamageModifier_{0}_{1}", characterActionType, elementType);
                    STANDARD_FORMULAS.Add(damageModifierName, Parse(string.Format("1+[DamageModifier_{0}]+[DamageModifier_{1}Damage]+[DamageModifier_All]", characterActionType, elementType)));

                    string noncriticalFormulaName = GetStandardDamageFormulaName(DamageType.Noncritical, characterActionType, elementType);
                    STANDARD_FORMULAS.Add(noncriticalFormulaName, Parse("{Attack_Total}*[SkillMultiplier]*{" + damageModifierName + "}"));

                    STANDARD_FORMULAS.Add(GetStandardDamageFormulaName(DamageType.Expected, characterActionType, elementType), Parse("{" + noncriticalFormulaName + "}*{CriticalExpectation}"));
                    STANDARD_FORMULAS.Add(GetStandardDamageFormulaName(DamageType.Critical, characterActionType, elementType), Parse("{" + noncriticalFormulaName + "}*{CriticalDamageMultiplier}"));
                }
            }

            int x = 1;
        }


        public static MathmaticalExpression Parse(string input)
        {
            return parser.Parse(lexer.GenerateTokens(input));
        }

        public static MathmaticalExpression GetCommonFormula(string name)
        {
            if (STANDARD_FORMULAS.TryGetValue(name, out MathmaticalExpression result))
            {
                return result;
            }
            
            return MathmaticalExpression.NONE;
        }

        public static string GetStandardDamageFormulaName(DamageType damageType, CharacterActionType characterActionType, ElementType elementType) => string.Format("Standard{0}Damage_{1}_{2}", damageType, characterActionType, elementType);
    }
}
