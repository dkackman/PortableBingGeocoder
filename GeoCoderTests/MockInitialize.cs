using System;
using System.Net.Http;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeHttp;

namespace GeoCoderTests
{
    [TestClass]
    [DeploymentItem(@"FakeResponses\")]
    public class MockInitialize
    {
        class Callbacks : ResponseCallbacks
        {
            public override bool FilterParameter(string name, string value)
            {
                return "key".Equals(name, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        internal static HttpMessageHandler DefaultTestHandler { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // set the http message handler factory to the mode we want for the entire assmebly test execution
            MessageHandlerFactory.Mode = MessageHandlerMode.Online;

            // folders where mock responses are stored and where captured response should be saved
            var mockFolder = context.DeploymentDirectory; // the folder where the unit tests are running
            var captureFolder = Path.Combine(context.TestRunDirectory, @"..\..\FakeResponses\"); // kinda hacky but this should be the solution folder

            // here we don't want to serialize or include our api key in response lookups so
            // pass a lambda that will indicate to the serialzier to filter that param out
            var store = new FileSystemResponseStore(mockFolder, captureFolder, new Callbacks());

            DefaultTestHandler = MessageHandlerFactory.CreateMessageHandler(store);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (DefaultTestHandler != null)
            {
                DefaultTestHandler.Dispose();
            }
        }
    }
}
