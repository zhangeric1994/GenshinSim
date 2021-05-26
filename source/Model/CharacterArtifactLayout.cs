using System.Collections.Generic;


namespace GenshinSim
{
	public class CharacterArtifactLayout
	{
		public const int STORAGE_SIZE = 5;
		private ArtifactSave[] storage = new ArtifactSave[5];
		private Dictionary<string, int> artifactSetPieceCount = new Dictionary<string, int>();
		private CharacterStatisticCollection statistics = new CharacterStatisticCollection();

		
		public ArtifactSave this[ArtifactType artifactType]
		{
			get
			{
				return storage[(int)artifactType];
			}
		}


		public CharacterArtifactLayout()
		{
			Clear();
		}


		public void Set(ArtifactSave artifact)
		{
			int i = (int)artifact.type;

			ArtifactSave previousArtifact = storage[i];
			if (previousArtifact != null)
			{
				statistics.DecrementAll(previousArtifact.statistics);
			}


			storage[i] = artifact;
			statistics.IncrementAll(artifact.statistics);
		}

		
		public void Remove(ArtifactType artifactType)
		{
			ArtifactSave artifact = storage[(int)artifactType];
			if (artifact != null)
			{
				statistics.DecrementAll(artifact.statistics);
			}


			storage[(int)artifactType] = null;
		}

		
		public void Clear()
		{
			for (int i = 0; i < 5; i++)
			{
				storage[i] = null;
			}

			statistics.Clear();
		}
	}
}
