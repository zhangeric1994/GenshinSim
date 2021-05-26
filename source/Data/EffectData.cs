using Newtonsoft.Json;
using STK.DataTable;
using STK.Expression;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace GenshinSim
{
    using Math;


    public enum EffectFlag : int
    {
        Damage          = 0x00000001,
        Shield          = 0x00000002,
        Healing         = 0x00000004,
        StatisticBoost  = 0x00000008,

        Triggerable     = 0x00000100,
        Stackable       = 0x00000200,
    }


    [JsonObject]
    public class EffectData : IDataTableColumnType
    {
        [JsonProperty]
        private EffectFlag flag;
        [JsonProperty]
        private Dictionary<string, float> statistics;
        [JsonProperty]
        private string formulaString;
        [JsonIgnore]
        private MathmaticalExpression formula;


        [JsonIgnore]
        public EffectFlag Type { get => flag; }
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


            flag = 0;
            foreach (string s in input.Substring(leftIndex, rightIndex - leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR))
            {
                if (!Enum.TryParse(s, true, out EffectFlag partialType))
                {
                    throw new Exception();
                }

                flag = flag | partialType;
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

                return input.Length - 1;
            }

            formulaString = input.Substring(leftIndex, rightIndex - leftIndex).Trim(DataTableReader.TRIMED_CHARACTERS);

            return rightIndex - 1;
        }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            formula = FormulaFactory.Parse(formulaString);
        }
    }
}
