namespace GenshinSim
{
	public class CharacterSave
	{
		public readonly string character;
		protected CharacterStatisticCollection statistics = new CharacterStatisticCollection();
		protected CharacterArtifactLayout artifacts = new CharacterArtifactLayout();


		public float this[CharacterStatistic statistic]
		{
			get
			{
				return statistics[statistic];
			}
		}
		
		public ArtifactSave this[ArtifactType artifactType]
		{
			get
			{
				return artifacts[artifactType];
			}
		}


		public void SetArtifact(ArtifactSave artifactSave)
		{
			artifacts.Set(artifactSave);
		}
	}
}
