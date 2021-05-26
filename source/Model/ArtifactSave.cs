using System;
using System.Collections.Generic;


namespace GenshinSim
{
    public enum ArtifactType
    {
        FlowerOfLife,
        PlumeOfDeath,
        SandsOfEon,
        GobletOfEonothem,
        CircletOfLogos
    }


    public class ArtifactStatisticException : ArgumentException
    {
        public ArtifactStatisticException(ArtifactType artifactType, CharacterStatistic statistic, bool isMainStatistic = true) : base(string.Format("[Artifact] {0} cannot have {1} as its {2} statistic", artifactType.ToString(), statistic.ToString(), isMainStatistic ? "main" : "secondary"))
        {
        }
    }


    public class ArtifactSave
    {
        public const int STORAGE_SIZE = 5;
        public static readonly HashSet<CharacterStatistic> VALID_SECONDARY_STATISTICS = new HashSet<CharacterStatistic> { CharacterStatistic.Attack_Extra,
                                                                                                                          CharacterStatistic.Attack_Percentage,
                                                                                                                          CharacterStatistic.Defense_Extra,
                                                                                                                          CharacterStatistic.Defense_Percentage,
                                                                                                                          CharacterStatistic.Health_Extra,
                                                                                                                          CharacterStatistic.Health_Percentage,
                                                                                                                          CharacterStatistic.CriticalRate,
                                                                                                                          CharacterStatistic.CriticalDamage,
                                                                                                                          CharacterStatistic.ElementalMastery,
                                                                                                                          CharacterStatistic.EnergyRecharge
                                                                                                                        };


        public readonly ArtifactType type;
        public readonly Grade grade;
        public readonly ArtifactSetData set;
        public readonly CharacterStatisticPair[] statistics = new CharacterStatisticPair[5];


        protected ArtifactSave(ArtifactType type, Grade grade, ArtifactSetData set)
        {
            this.type = type;
            this.grade = grade;
            this.set = set;
        }


        public ArtifactSave(ArtifactType type, Grade grade, ArtifactSetData set, CharacterStatistic mainStatistic, float mainStatisticValue, params object[] secondaryStatistics) : this(type, grade, set)
        {
            switch (this.type)
            {
                case ArtifactType.FlowerOfLife:
                    statistics[0] = new CharacterStatisticPair(CharacterStatistic.Health_Extra, mainStatisticValue);
                    break;


                case ArtifactType.PlumeOfDeath:
                    statistics[0] = new CharacterStatisticPair(CharacterStatistic.Attack_Extra, mainStatisticValue);
                    break;


                case ArtifactType.SandsOfEon:
                    if (!SandsOfEonArtifactSave.VALID_MAIN_STATISTICS.Contains(mainStatistic))
                    {
                        throw new ArtifactStatisticException(ArtifactType.SandsOfEon, mainStatistic, true);
                    }
                    statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
                    break;


                case ArtifactType.GobletOfEonothem:
                    if (!GobletOfEonothemArtifactSave.VALID_MAIN_STATISTICS.Contains(mainStatistic))
                    {
                        throw new ArtifactStatisticException(ArtifactType.GobletOfEonothem, mainStatistic, true);
                    }
                    statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
                    break;


                case ArtifactType.CircletOfLogos:
                    if (!CircletOfLogosArtifactSave.VALID_MAIN_STATISTICS.Contains(mainStatistic))
                    {
                        throw new ArtifactStatisticException(ArtifactType.CircletOfLogos, mainStatistic, true);
                    }
                    statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
                    break;
            }

            SetSecondaryStatisitcs(secondaryStatistics);
        }


        protected void SetSecondaryStatisitcs(params object[] args)
        {
            int maxNumSecondaryStatistics = (int)grade;
            for (int i = 1; i < 5; i++)
            {
                bool flag = i > maxNumSecondaryStatistics;
                if (flag)
                {
                    statistics[i] = CharacterStatisticPair.NONE;
                }
                else
                {
                    int j = i * 2;
                    CharacterStatistic statistic = (CharacterStatistic)args[j - 2];

                    if (!VALID_SECONDARY_STATISTICS.Contains(statistic))
                    {
                        throw new ArtifactStatisticException(type, statistic, false);
                    }
                    statistics[i] = new CharacterStatisticPair(statistic, (float)args[j - 1]);
                }
            }
        }
    }


    public class FlowerOfLifeArtifactSave : ArtifactSave
    {
        public FlowerOfLifeArtifactSave(Grade grade, ArtifactSetData set, float mainStatisticValue, params object[] secondaryStatistics) : base(ArtifactType.FlowerOfLife, grade, set)
        {
            statistics[0] = new CharacterStatisticPair(CharacterStatistic.Health_Extra, mainStatisticValue);
            SetSecondaryStatisitcs(secondaryStatistics);
        }


        public const CharacterStatistic VALID_MAIN_STATISTIC = CharacterStatistic.Health_Extra;
    }


    public class PlumeOfDeathArtifactSave : ArtifactSave
    {
        public const CharacterStatistic VALID_MAIN_STATISTIC = CharacterStatistic.Attack_Extra;


        public PlumeOfDeathArtifactSave(Grade grade, ArtifactSetData set, float mainStatisticValue, params object[] secondaryStatistics) : base(ArtifactType.PlumeOfDeath, grade, set)
        {
            statistics[0] = new CharacterStatisticPair(CharacterStatistic.Attack_Extra, mainStatisticValue);
            SetSecondaryStatisitcs(secondaryStatistics);
        }
    }


    public class SandsOfEonArtifactSave : ArtifactSave
    {
        public static readonly HashSet<CharacterStatistic> VALID_MAIN_STATISTICS = new HashSet<CharacterStatistic> { CharacterStatistic.Attack_Percentage,
                                                                                                                     CharacterStatistic.Defense_Percentage,
                                                                                                                     CharacterStatistic.Health_Percentage,
                                                                                                                     CharacterStatistic.ElementalMastery,
                                                                                                                     CharacterStatistic.EnergyRecharge
                                                                                                                   };


        public SandsOfEonArtifactSave(Grade grade, ArtifactSetData set, CharacterStatistic mainStatistic, float mainStatisticValue, params object[] secondaryStatistics) : base(ArtifactType.SandsOfEon, grade, set)
        {
            if (VALID_MAIN_STATISTICS.Contains(mainStatistic))
            {
                statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
            }
            else
            {
                statistics[0] = CharacterStatisticPair.NONE;
            }

            SetSecondaryStatisitcs(secondaryStatistics);
        }
    }


    public class GobletOfEonothemArtifactSave : ArtifactSave
    {
        public static readonly HashSet<CharacterStatistic> VALID_MAIN_STATISTICS = new HashSet<CharacterStatistic> { CharacterStatistic.Attack_Percentage,
                                                                                                                     CharacterStatistic.Defense_Percentage,
                                                                                                                     CharacterStatistic.Health_Percentage,
                                                                                                                     CharacterStatistic.DamageModifier_PyroDamage,
                                                                                                                     CharacterStatistic.DamageModifier_HydroDamage,
                                                                                                                     CharacterStatistic.DamageModifier_DendroDamage,
                                                                                                                     CharacterStatistic.DamageModifier_ElectroDamage,
                                                                                                                     CharacterStatistic.DamageModifier_AnemoDamage,
                                                                                                                     CharacterStatistic.DamageModifier_CyroDamage,
                                                                                                                     CharacterStatistic.DamageModifier_GeoDamage,
                                                                                                                     CharacterStatistic.DamageModifier_PhysicalDamage,
                                                                                                                     CharacterStatistic.ElementalMastery
                                                                                                                   };


        public GobletOfEonothemArtifactSave(Grade grade, ArtifactSetData set, CharacterStatistic mainStatistic, float mainStatisticValue, params object[] secondaryStatistics) : base(ArtifactType.GobletOfEonothem, grade, set)
        {
            if (VALID_MAIN_STATISTICS.Contains(mainStatistic))
            {
                statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
            }
            else
            {
                statistics[0] = CharacterStatisticPair.NONE;
            }

            SetSecondaryStatisitcs(secondaryStatistics);
        }
    }


    public class CircletOfLogosArtifactSave : ArtifactSave
    {
        public static readonly HashSet<CharacterStatistic> VALID_MAIN_STATISTICS = new HashSet<CharacterStatistic> { CharacterStatistic.Attack_Percentage,
                                                                                                                     CharacterStatistic.Defense_Percentage,
                                                                                                                     CharacterStatistic.Health_Percentage,
                                                                                                                     CharacterStatistic.CriticalRate,
                                                                                                                     CharacterStatistic.CriticalDamage,
                                                                                                                     CharacterStatistic.ElementalMastery
                                                                                                                   };


        public CircletOfLogosArtifactSave(Grade grade, ArtifactSetData set, CharacterStatistic mainStatistic, float mainStatisticValue, params object[] secondaryStatistics) : base(ArtifactType.CircletOfLogos, grade, set)
        {
            if (VALID_MAIN_STATISTICS.Contains(mainStatistic))
            {
                statistics[0] = new CharacterStatisticPair(mainStatistic, mainStatisticValue);
            }
            else
            {
                statistics[0] = CharacterStatisticPair.NONE;
            }

            SetSecondaryStatisitcs(secondaryStatistics);
        }
    }
}
