using Newtonsoft.Json;
using STK.DataTable;


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
    public sealed class CharacterData : DictionaryDataTableRow<string>
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private EffectData[][] constellationEffects;


        [JsonIgnore]
        public override string Key => name;
        [JsonIgnore]
        public string Name => name;


        public CharacterData(DataTable dataTable, Metadata metadata) : base(dataTable, metadata) { }


        public EffectData[] GetConstellationEffects(int level) => level <= 0 || level > 6 ? null : constellationEffects[level - 1];
    }


    [JsonObject]
    public sealed class CharacterDataTable : DictionaryDataTable<CharacterData, string>
    {
        public CharacterDataTable(string name) : base(name) { }
    }
}
