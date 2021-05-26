using Newtonsoft.Json;
using STK.DataTable;


namespace GenshinSim
{
    [JsonObject]
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


        public ArtifactSetData(DataTable dataTable, Metadata metadata) : base(dataTable, metadata) { }
    }


    [JsonObject]
    public class ArtifactSetDataTable : DictionaryDataTable<ArtifactSetData, string>
    {
        public ArtifactSetDataTable(string name) : base(name) { }
    }
}
