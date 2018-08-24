using AutoMapper;
using Lykke.Service.PayAuth.AutoMapperProfiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.PayAuth.Tests
{
    [TestClass]
    public class TestAssembly
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<DefaultProfile>());
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void AutoMapper_OK()
        {
            Assert.IsTrue(true);
        }
    }
}
