using STK.Formula;
using System.Collections.Generic;


namespace GenshinSim.Math
{
    public class GenshinFormulaParser : FormulaParser
    {
        protected override IEvaluable HandleUnexpectedFactor(List<FormulaToken> input, ref int index)
        {
            FormulaToken token = input[index];

            if (token.type == "FORMULA")
            {
                ++index;
                return FormulaFactory.GetCommonFormula(token.text);
            }

            return base.HandleUnexpectedFactor(input, ref index);
        }
    }
}
