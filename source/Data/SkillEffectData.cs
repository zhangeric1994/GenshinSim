using Newtonsoft.Json;
using STK.DataTable;
using STK.Expression;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace GenshinSim
{
    using Math;


    public enum SkillEffectType : int
    {
        Damage          = 0x00000001,
        Shield          = 0x00000002,
        Healing         = 0x00000004,
        StatisticBoost  = 0x00000008,

        Triggerable     = 0x00000100,
        Stackable       = 0x00000200,
    }


    [JsonObject]
    public class SkillEffectData : IDataTableColumnType
    {
        [JsonProperty]
        private SkillEffectType type;
        [JsonProperty]
        private Dictionary<string, float> statistics;
        [JsonProperty]
        private string formulaString;
        [JsonIgnore]
        private MathmaticalExpression formula;


        [JsonIgnore]
        public SkillEffectType Type { get => type; }
        [JsonIgnore]
        public Dictionary<string, float> Statistics { get => statistics; }
        [JsonIgnore]
        public MathmaticalExpression Formula { get => formula; }


        int IDataTableColumnType.GenerateFromSource(string input, int leftIndex)
        {
            int rightIndex = input.IndexOf(';', leftIndex);
            if (rightIndex == -1)
            {
                throw new Exception();
            }


            type = 0;
            foreach (string s in input.Substring(leftIndex, rightIndex - leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR))
            {
                if (!Enum.TryParse(s, true, out SkillEffectType partialType))
                {
                    throw new Exception();
                }

                type = type | partialType;
            }


            leftIndex = rightIndex + 1;
            rightIndex = input.IndexOf(DataTableReader.FIELD_SEPARATOR, leftIndex);
            if (rightIndex == -1)
            {
                throw new Exception();
            }


            statistics = new Dictionary<string, float>();
            foreach (string s in input.Substring(leftIndex, rightIndex - leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR))
            {
                string[] ss = s.Split(DataTableReader.DESCRIPTOR);
                if (ss.Length != 2)
                {
                    throw new Exception();
                }


                if (!float.TryParse(ss[1], out float v))
                {
                    throw new Exception();
                }

                statistics.Add(ss[0], v);
            }


            leftIndex = rightIndex + 1;
            rightIndex = input.IndexOf(DataTableReader.ARRAY_SEPARATOR, leftIndex);


            if (rightIndex == -1)
            {
                formulaString = input.Substring(leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS);
                ParseFormulaString();

                return input.Length - 1;
            }

            formulaString = input.Substring(leftIndex, rightIndex - leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS);
            ParseFormulaString();

            return rightIndex - 1;
        }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            ParseFormulaString();
        }


        private void ParseFormulaString()
        {
            switch (type)
            {
                case SkillEffectType.StatisticBoost:
                    formula = MathmaticalExpression.NONE;
                    break;


                default:
                    formula = FormulaFactory.Parse(formulaString);
                    break;
            }
        }
    }
}
