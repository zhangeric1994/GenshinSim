using Newtonsoft.Json;
using STK.DataTable;
using System;
using System.Collections.Generic;


namespace GenshinSim
{
    public class CharacterTalentEffectData : DictionaryDataTableRow<string, CharacterTalentType>, ICustomExcelRowReading
    {
        [JsonProperty]
        private string character;
        [JsonProperty]
        private CharacterTalentType type;
        [JsonProperty]
        private LeveledEffectData effectData;


        [JsonIgnore]
        public string Character { get => character; }
        [JsonIgnore]
        public CharacterTalentType Type { get => type; }
        [JsonIgnore]
        public EffectData this[int level] => effectData[level];

        public override string Key => character;
        public override CharacterTalentType Key2 => type;


        public CharacterTalentEffectData(Metadata metadata) : base(metadata) { }


        void ICustomExcelRowReading.GenerateFromSource(Dictionary<string, object> input)
        {
            character = (input["Character"] as string).Trim(DataTableReader.TRIMED_CHARACTERS);


            if (!Enum.TryParse((input["Type"] as string).Trim(DataTableReader.TRIMED_CHARACTERS), true, out type))
            {
                throw new Exception();
            }


            string[] effectDefinitions = (input["EffectDefinition"] as string).Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.FIELD_SEPARATOR);
            if (effectDefinitions.Length != 3)
            {
                throw new Exception();
            }


            string[] statisticTypes = effectDefinitions[1].Split(DataTableReader.ARRAY_SEPARATOR);
            int numStatistics = statisticTypes.Length;
            if (numStatistics == 0)
            {
                throw new Exception();
            }


            EffectFlag flag = 0;
            foreach (string s in effectDefinitions[0].Split(DataTableReader.ARRAY_SEPARATOR))
            {
                if (!Enum.TryParse(s.Trim(DataTableReader.TRIMED_CHARACTERS), true, out EffectFlag partialType))
                {
                    throw new Exception();
                }

                flag = flag | partialType;
            }


            List<object> cellValues = input["EffectStatistics"] as List<object>;
            int N = cellValues.Count;

            Dictionary<string, float>[] statistics = new Dictionary<string, float>[N];

            for (int i = 0; i < N; ++i)
            {
                Dictionary<string, float> dictionary = new Dictionary<string, float>();

                object cellValue = cellValues[i];
                if (cellValue is string)
                {
                    string[] statisticValues = (cellValue as string).Trim(DataTableReader.TRIMED_CHARACTERS).Split(DataTableReader.ARRAY_SEPARATOR);
                    if (statisticValues.Length != numStatistics)
                    {
                        throw new Exception();
                    }

                    for (int j = 0; j < numStatistics; ++j)
                    {
                        if (float.TryParse(statisticValues[j], out float statisticValue))
                        {
                            dictionary.Add(statisticTypes[j], statisticValue);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                else if (cellValue is double)
                {
                    if (statisticTypes.Length != 1)
                    {
                        throw new Exception();
                    }

                    dictionary.Add(statisticTypes[0], (float)(double)cellValue);
                }
                else
                {
                    throw new Exception();
                }


                statistics[i] = dictionary;
            }


            string formulaString = effectDefinitions[2];


            effectData = new LeveledEffectData(flag, statistics, formulaString);
        }
    }


    public class CharacterTalentEffectDataTable : CatagorizedDataTable<CharacterTalentEffectData, string, CharacterTalentType>
    {
        public CharacterTalentEffectDataTable(string name) : base(name) { }
    }
}
