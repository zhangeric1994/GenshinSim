using Newtonsoft.Json;
using STK.DataTable;


namespace GenshinSim
{
    public class ArtifactSetData : DictionaryDataTableRow<string>
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private EffectData[][] setEffects;


        [JsonIgnore]
        public string Name { get => name; }
        [JsonIgnore]
        public EffectData[][] SetEffects { get => setEffects; }
        [JsonIgnore]
        public override string Key { get => name; }
    }


    public class ArtifactSetDataTable : DictionaryDataTable<ArtifactSetData, string>
    {
    }
}
