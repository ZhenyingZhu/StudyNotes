using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace TestFlagBitOperation
{
    public class TestMain
    {
        [Flags]
        public enum FlagEnum
        {
            None = 1,
            Enabled = 2,
            Disabled = 4
        }

        public static void testMain()
        {
            FlagEnum? value = null;
            FlagEnum? flag = FlagEnum.None | FlagEnum.Enabled;
            if ((value & flag) != FlagEnum.None)
            {
                System.Console.WriteLine("Correct");
            }
        }
    }
}
