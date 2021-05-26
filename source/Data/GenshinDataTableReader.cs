using STK.DataTable;
using System;


namespace GenshinSim
{
    public sealed class GenshinDataTableReader : DataTableReader
    {
        public static readonly GenshinDataTableReader Instance = new GenshinDataTableReader();


        static GenshinDataTableReader() { }


        private GenshinDataTableReader() { }


        protected override Type GetType(string name)
        {
            return Type.GetType(string.Format("GenshinSim.{0}, GenshinSim", name), true, true);
        }
    }
}
