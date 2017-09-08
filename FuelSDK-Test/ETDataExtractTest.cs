﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuelSDK.Test
{
    class ETDataExtractTest
    {
        private ETClient client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            client = new ETClient();

        }

        [Test()]
        public void ExtractTrackingDataTest()
        {
            ETDataExtract dataExtract = new ETDataExtract
            {
                AuthStub = client
            };
            dataExtract.OutputFileName = "DEOutput%%Year%%_%%Month%%_%%Day%%.csv";
            dataExtract.DECustomerKey = "017dce26-b61f-43c2-bb15-0e46de82d177";
            var response = dataExtract.ExtractDataExtension();
            Assert.AreEqual(response.OverallStatus,"OK");
        }

    }
}
