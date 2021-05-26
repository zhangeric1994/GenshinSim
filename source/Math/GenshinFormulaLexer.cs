using STK.Formula;
using System.Collections.Generic;


namespace GenshinSim.Math
{
    public class GenshinFormulaLexer : FormulaLexer
    {
        public const char FORMULA_START = '{';
        public const char FORMULA_END = '}';


        protected override bool HandleUnexpectedCharacter(string input, ref List<FormulaToken> output, ref int index)
        {
            if (input[index] == FORMULA_START)
            {
                ++index;


                int count = 0;
                for (; index + count < input.Length; ++count)
                {
                    if (input[index + count] == FORMULA_END)
                    {
                        if (count == 0)
                        {
                            return false;
                        }

                        break;
                    }
                }


                output.Add(new FormulaToken("FORMULA", input.Substring(index, count)));


                index += count + 1;


                return true;
            }
            
            
            return base.HandleUnexpectedCharacter(input, ref output, ref index);
        }
    }
}
