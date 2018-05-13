using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Ioc.Framework.Core.Test.Interface;

namespace Unity.Ioc.Framework.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ITestInterface service = ServiceLocator.GetInstance<ITestInterface>();

            service.TestMethod("Hello world!");
        }
    }
}
