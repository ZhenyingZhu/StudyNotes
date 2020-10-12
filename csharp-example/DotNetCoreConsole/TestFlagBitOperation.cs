using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestFlagBitOperation
    {
        public static void testMain()
        {
            FlagEnum? value = null;
            FlagEnum? flag = FlagEnum.None | FlagEnum.Enabled;
            if ((value & flag) != FlagEnum.None)
            {
                System.Console.WriteLine("Correct");
            }
        }

        [Flags]
        private enum FlagEnum
        {
            None = 1,
            Enabled = 2,
            Disabled = 4
        }
    }
}
