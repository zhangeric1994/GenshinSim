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
        StatisticChange = 0x00000008,

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


        public EffectData() { }

        public EffectData(EffectFlag flag, Dictionary<string, float> statistics, string formulaString, MathmaticalExpression formula)
        {
            this.flag = flag;
            this.statistics = statistics;
            this.formulaString = "";
            this.formula = formula;
        }


        int IDataTableColumnType.GenerateFromSource(string input, int leftIndex)
        {
            string[] splitedInput;
            if (leftIndex == -1)
            {
                GenerateFromSource(input.Split(DataTableReader.FIELD_SEPARATOR));
                return -1;
            }


            splitedInput = new string[3] { null, null, null };


            int rightIndex = input.IndexOf(DataTableReader.FIELD_SEPARATOR, leftIndex);
            if (rightIndex == -1)
            {
                throw new Exception();
            }

            splitedInput[0] = input.Substring(leftIndex, rightIndex - leftIndex);


            leftIndex = rightIndex + 1;
            rightIndex = input.IndexOf(DataTableReader.FIELD_SEPARATOR, leftIndex);
            if (rightIndex == -1)
            {
                throw new Exception();
            }

            splitedInput[1] = input.Substring(leftIndex, rightIndex - leftIndex);


            leftIndex = rightIndex + 1;
            rightIndex = input.IndexOf(DataTableReader.ARRAY_SEPARATOR, leftIndex);
            if (rightIndex == -1)
            {
                splitedInput[2] = input.Substring(leftIndex);

                GenerateFromSource(splitedInput);

                return input.Length - 1;
            }

            splitedInput[2] = input.Substring(leftIndex, rightIndex - leftIndex);


            GenerateFromSource(splitedInput);


            return rightIndex - 1;
        }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            formula = FormulaFactory.Parse(formulaString);
            formulaString = null;
        }


        private void GenerateFromSource(string[] splitedInput)
        {
            flag = 0;
            foreach (string s in splitedInput[0].Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR))
            {
                if (!Enum.TryParse(s, true, out EffectFlag partialType))
                {
                    throw new Exception();
                }

                flag = flag | partialType;
            }


            statistics = new Dictionary<string, float>();
            if (splitedInput[1] != null)
            {
                foreach (string s in splitedInput[1].Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR))
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
            }


            formulaString = splitedInput[2].Trim(DataTableReader.TRIMED_CHARACTERS);


            formula = FormulaFactory.Parse(formulaString);
        }
    }


    [JsonObject]
    public class LeveledEffectData
    {
        [JsonProperty]
        private EffectFlag flag;
        [JsonProperty]
        private Dictionary<string, float>[] statistics;
        [JsonProperty]
        private string formulaString;
        [JsonIgnore]
        private MathmaticalExpression formula;


        [JsonIgnore]
        public EffectFlag Type { get => flag; }
        [JsonIgnore]
        public MathmaticalExpression Formula { get => formula; }


        public EffectData this[int level] => new EffectData(flag, statistics[level - 1], formulaString, formula);


        public LeveledEffectData(EffectFlag flag, Dictionary<string, float>[] statistics, string formulaString)
        {
            this.flag = flag;
            this.statistics = statistics;
            this.formulaString = formulaString;
            this.formula = FormulaFactory.Parse(formulaString);
        }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            formula = FormulaFactory.Parse(formulaString);
        }
    }
}
