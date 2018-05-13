# Unity.Ioc.Framework.Core

a unity framework extension for Microsoft.Practices.Unity  Support .NET Core/ netstandard 2.0

# Simple code

```C#
    [ServiceImplementation]
    public class TestClass : ITestInterface
    {
        public void TestMethod(string a)
        {
            Console.WriteLine("input:{0}", a);
        }
    }
           
   ITestInterface service = ServiceLocator.GetInstance<ITestInterface>();
   service.TestMethod("Hello world!");
```
     
# Config file
Config name must be named "Unity.config" in root directory of project
```xml
<configuration>
  <configSections>
    <section name="packing" type="Unity.Ioc.Framework.Core.Configuration.PackingSection,Unity.Ioc.Framework.Core"/>
  </configSections>
  <packing>
    <moduleAssemblies>
      <add name="Unity.Ioc.Framework.Core.Test.Implementation"/>
    </moduleAssemblies>
  </packing>
 
</configuration>
```
