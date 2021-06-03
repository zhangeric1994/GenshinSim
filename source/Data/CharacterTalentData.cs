using Newtonsoft.Json;
using STK.DataTable;
using System;
using System.Runtime.Serialization;


namespace GenshinSim
{
    public enum CharacterTalentType : int
    {
        Attack = 0,
        ElementalSkill = 1,
        ElementalBurst = 2,
        Passive1 = 3,
        Passive2 = 4
    }


    [JsonObject]
    public sealed class CharacterTalentEffectText : IDataTableColumnType
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private string formatterString;


        [JsonIgnore]
        public Text Name { get; private set; }
        [JsonIgnore]
        public TextFormatter Formatter { get; private set; }


        int IDataTableColumnType.GenerateFromSource(string input, int leftIndex)
        {
            if (leftIndex == -1)
            {
                GenerateFromSource(input);
                return -1;
            }


            int rightIndex = input.IndexOf(DataTableReader.ARRAY_SEPARATOR, leftIndex);
            if (rightIndex == -1)
            {
                GenerateFromSource(input.Substring(leftIndex));
                return input.Length - 1;
            }


            GenerateFromSource(input.Substring(leftIndex, rightIndex - leftIndex));


            return rightIndex - 1;
        }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Name = TextManager.Instance.GetText(name);

            Formatter = new TextFormatter();
            ((IDataTableColumnType)Formatter).GenerateFromSource(formatterString);


            name = null;
            formatterString = null;
        }


        private void GenerateFromSource(string input)
        {
            string[] splitedInput = input.Split(DataTableReader.FIELD_SEPARATOR);
            if (splitedInput.Length != 2)
            {
                throw new Exception();
            }


            name = splitedInput[0].Trim(DataTableReader.TRIMED_CHARACTERS);
            formatterString = splitedInput[1].Trim(DataTableReader.TRIMED_CHARACTERS);


            Formatter = new TextFormatter();
            ((IDataTableColumnType)Formatter).GenerateFromSource(formatterString);
        }
    }


    [JsonObject]
    public sealed class CharacterTalentData : DictionaryDataTableRow<string, CharacterTalentType>
    {
        [JsonProperty]
        private string character;
        [JsonProperty]
        private CharacterTalentType type;
        [JsonProperty]
        private string name;
        [JsonProperty]
        private string description;
        [JsonProperty]
        private CharacterTalentEffectText[] effectTexts;


        [JsonIgnore]
        public string Character => character;
        [JsonIgnore]
        public CharacterTalentType Type => type;
        [JsonIgnore]
        public Text Name { get; private set; }
        [JsonIgnore]
        public Text Description { get; private set; }
        [JsonIgnore]
        public CharacterTalentEffectText[] EffectTexts => effectTexts;

        [JsonIgnore]
        public override string Key => character;
        [JsonIgnore]
        public override CharacterTalentType Key2 => type;


        public CharacterTalentData(Metadata metadata) : base(metadata) { }


        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Name = TextManager.Instance.GetText(name);
            Description = TextManager.Instance.GetText(description);

            name = null;
            description = null;
        }
    }


    [JsonObject]
    public sealed class CharacterTalentDataTable : DictionaryDataTable<CharacterTalentData, string, CharacterTalentType>
    {
        public CharacterTalentDataTable(string name) : base(name) { }
    }
}
