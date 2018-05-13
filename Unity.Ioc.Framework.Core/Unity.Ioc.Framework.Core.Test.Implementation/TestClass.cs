using System;
using Unity.Ioc.Framework.Core.Test.Interface;

namespace Unity.Ioc.Framework.Core.Test.Implementation
{
    [ServiceImplementation]
    public class TestClass : ITestInterface
    {
        public void TestMethod(string a)
        {
            Console.WriteLine("input:{0}", a);
        }
    }
}
