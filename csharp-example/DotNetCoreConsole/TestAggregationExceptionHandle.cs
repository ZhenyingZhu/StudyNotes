using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestAggregationExceptionHandle
    {
        public static void testMain()
        {
            TestAggregationExceptionHandle myClass = new TestAggregationExceptionHandle();
            try
            {
                myClass.ThrowAggreationException(false);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is ArgumentException)
                    {
                        Console.WriteLine(ex.Message);
                        return true;
                    }
                    return false;
                });
            }
        }

        private void ThrowAggreationException(bool expected)
        {
            Exception innerEx;
            if (expected)
            {
                innerEx = new ArgumentException("Argument exception is expected");
            }
            else
            {
                innerEx = new ApplicationException("App exception is not expected");
            }

            throw new AggregateException(innerEx);
        }
    }
}