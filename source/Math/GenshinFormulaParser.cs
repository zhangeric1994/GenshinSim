using STK.Expression;
using System.Collections.Generic;


namespace GenshinSim.Math
{
    public class GenshinFormulaParser : MathmaticalExpressionParser
    {
        protected override IEvaluable HandleUnexpectedFactor(List<Token> input, ref int index)
        {
            Token token = input[index];

            if (token.type == "FORMULA")
            {
                ++index;
                return FormulaFactory.GetCommonFormula(token.text);
            }

            return base.HandleUnexpectedFactor(input, ref index);
        }
    }
}
