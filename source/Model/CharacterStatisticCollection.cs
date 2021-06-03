using System;
using System.Collections;
using System.Collections.Generic;


namespace GenshinSim
{
    public enum CharacterStatistic : int
    {
        Attack_Base = 0,
        Attack_Extra,
        Attack_Percentage,

        Defense_Base,
        Defense_Extra,
        Defense_Percentage,

        Health_Base,
        Health_Extra,
        Health_Percentage,

        CriticalRate,
        CriticalDamage,

        DamageModifier_NormalAttack,
        DamageModifier_ChargedAttack,
        DamageModifier_PlungeAttack,
        DamageModifier_ElementalSkill,
        DamageModifier_ElementalBurst,

        DamageModifier_PyroDamage,
        DamageModifier_HydroDamage,
        DamageModifier_DendroDamage,
        DamageModifier_ElectroDamage,
        DamageModifier_AnemoDamage,
        DamageModifier_CryoDamage,
        DamageModifier_GeoDamage,
        DamageModifier_PhysicalDamage,
        DamageModifier_All,

        ElementalMastery,
        EnergyRecharge,

        CooldownReduction,

        ShieldStrength,
        HealingModifier,

        None
    }


    public struct CharacterStatisticPair
    {
        public static readonly CharacterStatisticPair NONE = new CharacterStatisticPair(CharacterStatistic.None, 0);


        public CharacterStatistic statistic;
        public float value;


        public bool IsNone => statistic == CharacterStatistic.None;


        public static implicit operator KeyValuePair<CharacterStatistic, float>(CharacterStatisticPair sp) => new KeyValuePair<CharacterStatistic, float>(sp.statistic, sp.value);

        public static explicit operator CharacterStatisticPair(KeyValuePair<CharacterStatistic, float> kvp) => new CharacterStatisticPair(kvp.Key, kvp.Value);


        public static implicit operator KeyValuePair<string, float>(CharacterStatisticPair sp) => new KeyValuePair<string, float>(sp.statistic == CharacterStatistic.None ? "" : sp.statistic.ToString(), sp.value);

        public static explicit operator CharacterStatisticPair(KeyValuePair<string, float> kvp)
        {
            if (Enum.TryParse(kvp.Key, true, out CharacterStatistic statisitc))
            {
                return new CharacterStatisticPair(statisitc, kvp.Value);
            }

            return NONE;
        }


        public CharacterStatisticPair(CharacterStatistic statistic, float value)
        {
            this.statistic = statistic;
            this.value = value;
        }
    }


    public class CharacterStatisticCollection : IDictionary<CharacterStatistic, float>, ICollection<KeyValuePair<CharacterStatistic, float>>, IEnumerable<KeyValuePair<CharacterStatistic, float>>, IEnumerable, IDictionary<string, float>, ICollection<KeyValuePair<string, float>>, IEnumerable<KeyValuePair<string, float>>, IEnumerable<CharacterStatisticPair>
    {
        private const int STORAGE_SIZE = (int)CharacterStatistic.None + 1;


        private float[] storage = new float[STORAGE_SIZE];


        #region Getter and Setter

        private float this[int i]
        {
            get
            {
                return storage[i];
            }
            set
            {
                storage[i] = value;
            }
        }

        public float this[CharacterStatistic key]
        {
            get
            {
                return this[(int)key];
            }
            set
            {
                this[(int)key] = value;
            }
        }

        #endregion //Getter and Setter
        #region Operator

        public static CharacterStatisticCollection operator +(CharacterStatisticCollection lhs, CharacterStatisticCollection rhs)
        {
            CharacterStatisticCollection result = new CharacterStatisticCollection(lhs);
            for (int i = 1; i < STORAGE_SIZE; i++)
            {
                result.storage[i] += rhs.storage[i];
            }
            return result;
        }

        #endregion //Operator
        #region Constructor

        public CharacterStatisticCollection()
        {
            Clear();
        }

        public CharacterStatisticCollection(CharacterStatisticCollection other)
        {
            other.storage.CopyTo(storage, 0);
        }

        #endregion //Constructor
        #region Public Methods

        public void Set(CharacterStatistic statistic, float value)
        {
            storage[(int)statistic] = value;
        }

        public void Set(CharacterStatisticPair statisticPair)
        {
            Set(statisticPair.statistic, statisticPair.value);
        }

        public void Set(string name, float value)
        {
            if (Enum.TryParse(name, true, out CharacterStatistic statistic))
            {
                Set(statistic, value);
            }
        }


        public void Modify(CharacterStatistic statistic, float value)
        {
            storage[(int)statistic] += value;
        }


        public void Increment(CharacterStatisticPair statisticPair)
        {
            Modify(statisticPair.statistic, statisticPair.value);
        }

        public void Increment(KeyValuePair<CharacterStatistic, float> keyValuePair)
        {
            Modify(keyValuePair.Key, keyValuePair.Value);
        }

        public void IncrementAll(IEnumerable<CharacterStatisticPair> statisticPairs)
        {
            foreach (CharacterStatisticPair statisticPair in statisticPairs)
            {
                Increment(statisticPair);
            }
        }

        public void IncrementAll(IEnumerable<KeyValuePair<CharacterStatistic, float>> keyValuePairs)
        {
            foreach (KeyValuePair<CharacterStatistic, float> kvp in keyValuePairs)
            {
                CharacterStatisticPair keyValuePair = (CharacterStatisticPair)kvp;
                Increment(keyValuePair);
            }
        }


        public void Decrement(CharacterStatisticPair statisticPair)
        {
            Modify(statisticPair.statistic, -statisticPair.value);
        }

        public void Decrement(KeyValuePair<CharacterStatistic, float> keyValuePair)
        {
            Modify(keyValuePair.Key, -keyValuePair.Value);
        }

        public void DecrementAll(IEnumerable<CharacterStatisticPair> statisticPairs)
        {
            foreach (CharacterStatisticPair statisticPair in statisticPairs)
            {
                Decrement(statisticPair);
            }
        }

        public void DecrementAll(IEnumerable<KeyValuePair<CharacterStatistic, float>> keyValuePairs)
        {
            foreach (KeyValuePair<CharacterStatistic, float> kvp in keyValuePairs)
            {
                CharacterStatisticPair keyValuePair = (CharacterStatisticPair)kvp;
                Decrement(keyValuePair);
            }
        }


        public void Clear()
        {
            for (int i = 0; i < STORAGE_SIZE; i++)
            {
                storage[i] = 0;
            }
        }

        public void Clear(CharacterStatistic statistic)
        {
            Set(statistic, 0);
        }

        #endregion //Public Methods
        #region Interface: IDictionary<CharacterStatistic, float>

        ICollection<CharacterStatistic> IDictionary<CharacterStatistic, float>.Keys
        {
            get
            {
                CharacterStatistic[] result = new CharacterStatistic[STORAGE_SIZE];
                for (int i = 0; i < STORAGE_SIZE; i++)
                {
                    result[i] = (CharacterStatistic)i;
                }

                return result;
            }
        }

        ICollection<float> IDictionary<CharacterStatistic, float>.Values => storage;

        int ICollection<KeyValuePair<CharacterStatistic, float>>.Count => STORAGE_SIZE;

        bool ICollection<KeyValuePair<CharacterStatistic, float>>.IsReadOnly => false;

        void IDictionary<CharacterStatistic, float>.Add(CharacterStatistic key, float value) => Modify(key, value);

        bool IDictionary<CharacterStatistic, float>.ContainsKey(CharacterStatistic key) => key != CharacterStatistic.None;


        bool IDictionary<CharacterStatistic, float>.Remove(CharacterStatistic key)
        {
            Clear(key);
            return true;
        }

        bool IDictionary<CharacterStatistic, float>.TryGetValue(CharacterStatistic key, out float value)
        {
            value = this[key];
            return key != CharacterStatistic.None;
        }

        void ICollection<KeyValuePair<CharacterStatistic, float>>.Add(KeyValuePair<CharacterStatistic, float> item)
        {
            Modify(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<CharacterStatistic, float>>.Clear() => Clear();

        bool ICollection<KeyValuePair<CharacterStatistic, float>>.Contains(KeyValuePair<CharacterStatistic, float> item)
        {
            return item.Key != CharacterStatistic.None && this[item.Key] == item.Value;
        }

        void ICollection<KeyValuePair<CharacterStatistic, float>>.CopyTo(KeyValuePair<CharacterStatistic, float>[] array, int arrayIndex)
        {
            for (int i = 0; i < STORAGE_SIZE; ++i)
            {
                array[arrayIndex + i] = new KeyValuePair<CharacterStatistic, float>((CharacterStatistic)i, storage[i]);
            }
        }

        bool ICollection<KeyValuePair<CharacterStatistic, float>>.Remove(KeyValuePair<CharacterStatistic, float> item)
        {
            Clear(item.Key);
            return item.Key != CharacterStatistic.None;
        }

        IEnumerator<KeyValuePair<CharacterStatistic, float>> IEnumerable<KeyValuePair<CharacterStatistic, float>>.GetEnumerator() => GetEnumerator();

        #endregion //Interface: IDictionary<CharacterStatistic, float>
        #region Interface: IDictionary<string, float>

        ICollection<string> IDictionary<string, float>.Keys
        {
            get
            {
                string[] result = new string[STORAGE_SIZE];
                for (int i = 0; i < STORAGE_SIZE; i++)
                {
                    result[i] = ((CharacterStatistic)i).ToString();
                }
                return result;
            }
        }

        ICollection<float> IDictionary<string, float>.Values => storage;
        int ICollection<KeyValuePair<string, float>>.Count => STORAGE_SIZE;
        bool ICollection<KeyValuePair<string, float>>.IsReadOnly => false;

        float IDictionary<string, float>.this[string key]
        {
            get
            {
                if (Enum.TryParse(key, true, out CharacterStatistic statistic))
                {
                    return this[statistic];
                }

                return 0;
            }
            set
            {
                CharacterStatistic statistic;
                if (Enum.TryParse(key, true, out statistic))
                {
                    this[statistic] = value;
                }
            }
        }

        void IDictionary<string, float>.Add(string key, float value)
        {
            if (Enum.TryParse(key, true, out CharacterStatistic statistic))
            {
                Modify(statistic, value);
            }
        }

        bool IDictionary<string, float>.ContainsKey(string key)
        {
            return Enum.TryParse(key, true, out CharacterStatistic statistic);
        }

        bool IDictionary<string, float>.Remove(string key)
        {
            if (Enum.TryParse(key, true, out CharacterStatistic statistic))
            {
                Clear(statistic);
                return true;
            }

            return false;
        }

        bool IDictionary<string, float>.TryGetValue(string key, out float value)
        {
            if (Enum.TryParse(key, true, out CharacterStatistic statistic))
            {
                value = this[statistic];
                return true;
            }

            value = 0;
            return false;
        }

        void ICollection<KeyValuePair<string, float>>.Add(KeyValuePair<string, float> item)
        {
            if (Enum.TryParse(item.Key, true, out CharacterStatistic statistic))
            {
                Modify(statistic, item.Value);
            }
        }

        bool ICollection<KeyValuePair<string, float>>.Contains(KeyValuePair<string, float> item)
        {
            return Enum.TryParse(item.Key, false, out CharacterStatistic statistic) && this[statistic] == item.Value;
        }

        void ICollection<KeyValuePair<string, float>>.CopyTo(KeyValuePair<string, float>[] array, int arrayIndex)
        {
            for (int i = 0; i < STORAGE_SIZE; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<string, float>(((CharacterStatistic)i).ToString(), storage[i]);
            }
        }

        bool ICollection<KeyValuePair<string, float>>.Remove(KeyValuePair<string, float> item)
        {
            if (Enum.TryParse(item.Key, true, out CharacterStatistic statistic))
            {
                Clear(statistic);
                return true;
            }

            return false;
        }

        IEnumerator<KeyValuePair<string, float>> IEnumerable<KeyValuePair<string, float>>.GetEnumerator() => GetEnumerator();

        #endregion //Interface: IDictionary<string, float>
        #region IEnumerable<CharacterStatisticPair>

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<CharacterStatisticPair> IEnumerable<CharacterStatisticPair>.GetEnumerator() => GetEnumerator();

        #endregion //IEnumerable<CharacterStatisticPair>
        #region Enumerator

        private struct Enumerator : IEnumerator, IEnumerator<CharacterStatisticPair>, IDisposable, IEnumerator<KeyValuePair<CharacterStatistic, float>>, IEnumerator<KeyValuePair<string, float>>
        {
            private float[] storage;
            private int index;


            #region Getter and Setter

            public CharacterStatisticPair Current => new CharacterStatisticPair((CharacterStatistic)index, storage[index]);

            #endregion //Getter and Setter
            #region Constructor

            public Enumerator(float[] storage)
            {
                this.storage = storage;
                this.index = -1;
            }

            #endregion //Constructor
            #region Public Methods

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return ++index < STORAGE_SIZE;
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion //Public Methods
            #region Interface: IEnumerator

            object IEnumerator.Current => Current;

            KeyValuePair<CharacterStatistic, float> IEnumerator<KeyValuePair<CharacterStatistic, float>>.Current => Current;

            KeyValuePair<string, float> IEnumerator<KeyValuePair<string, float>>.Current => Current;

            #endregion //interface: IEnumerator
        }


        private Enumerator GetEnumerator()
        {
            return new Enumerator(storage);
        }

        #endregion //Enumerator
    }
}
