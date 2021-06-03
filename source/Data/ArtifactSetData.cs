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
        public override string Key { get => name; }


        public ArtifactSetData(Metadata metadata) : base(metadata) { }


        public EffectData[] GetSetEffects(int numPieces) => numPieces <= 0 || numPieces > 5 ? null : setEffects[numPieces - 1];
    }


    [JsonObject]
    public class ArtifactSetDataTable : DictionaryDataTable<ArtifactSetData, string>
    {
        public ArtifactSetDataTable(string name) : base(name) { }
    }
}
