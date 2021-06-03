using Newtonsoft.Json;
using STK.DataTable;


namespace GenshinSim
{
    [JsonObject]
    public class CharacterData : DictionaryDataTableRow<string>
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private string displayName;
        [JsonProperty]
        private ElementType elementType;
        [JsonProperty]
        private WeaponType weaponType;
        [JsonProperty]
        private EffectData[][] passiveTalentEffects;
        [JsonProperty]
        private EffectData[][] constellationEffects;


        public override string Key => name;


        public CharacterData(Metadata metadata) : base(metadata) { }
    }


    [JsonObject]
    public class CharacterDataTable : DictionaryDataTable<CharacterData, string>
    {
        public CharacterDataTable(string name) : base(name) { }
    }
}
