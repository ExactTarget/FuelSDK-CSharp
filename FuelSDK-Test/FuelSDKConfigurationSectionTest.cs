﻿using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FuelSDK.Test
{
    [TestFixture]
    class FuelSDKConfigurationSectionTest
    {
        private readonly string emptyConfigFileName = "empty.config";
        private readonly string missingRequiredAppSignaturePropertyConfigFileName = "missingRequiredAppSignatureProperty.config";
        private readonly string missingRequiredClientIdConfigFileName = "missingRequiredClientIdProperty.config";
        private readonly string missingRequiredClientSecretConfigFileName = "missingRequiredClientSecretProperty.config";
        private readonly string requiredPropertiesOnlyConfigFileName = "requiredPropertiesOnly.config";
        private readonly string allPropertiesSetConfigFileName = "allPropertiesSet.config";

        [Test()]
        public void NoCustomConfigSection()
        {
            FuelSDKConfigurationSection section = GetCustomConfigurationSectionFromConfigFile(emptyConfigFileName);
            Assert.IsNull(section);
        }

        [Test()]
        public void MissingRequiredAppSignaturePropertyFromConfigSection()
        {
            Assert.That(() => GetCustomConfigurationSectionFromConfigFile(missingRequiredAppSignaturePropertyConfigFileName), Throws.TypeOf<ConfigurationErrorsException>());
        }

        [Test()]
        public void MissingRequiredClientIdPropertyFromConfigSection()
        {
            Assert.That(() => GetCustomConfigurationSectionFromConfigFile(missingRequiredClientIdConfigFileName), Throws.TypeOf<ConfigurationErrorsException>());
        }

        [Test()]
        public void MissingRequiredClientSecretPropertyFromConfigSection()
        {
            Assert.That(() => GetCustomConfigurationSectionFromConfigFile(missingRequiredClientSecretConfigFileName), Throws.TypeOf<ConfigurationErrorsException>());
        }

        [Test()]
        public void MissingSoapEndPointPropertyFromConfigSection()
        {
            FuelSDKConfigurationSection section = GetCustomConfigurationSectionFromConfigFile(requiredPropertiesOnlyConfigFileName);
            var attribute = section.GetType().GetProperty("SoapEndPoint").GetCustomAttributes(typeof(ConfigurationPropertyAttribute), false).Single() as ConfigurationPropertyAttribute;
            Assert.AreEqual(section.SoapEndPoint, attribute.DefaultValue);
        }

        [Test()]
        public void MissingAuthEndPointPropertyFromConfigSection()
        {
            FuelSDKConfigurationSection section = GetCustomConfigurationSectionFromConfigFile(requiredPropertiesOnlyConfigFileName);
            var attribute = section.GetType().GetProperty("AuthenticationEndPoint").GetCustomAttributes(typeof(ConfigurationPropertyAttribute), false).Single() as ConfigurationPropertyAttribute;
            Assert.AreEqual(section.AuthenticationEndPoint, attribute.DefaultValue);
        }

        [Test()]
        public void MissingRestEndPointPropertyFromConfigSection()
        {
            FuelSDKConfigurationSection section = GetCustomConfigurationSectionFromConfigFile(requiredPropertiesOnlyConfigFileName);
            var attribute = section.GetType().GetProperty("RestEndPoint").GetCustomAttributes(typeof(ConfigurationPropertyAttribute), false).Single() as ConfigurationPropertyAttribute;
            Assert.AreEqual(section.RestEndPoint, attribute.DefaultValue);
        }

        [Test()]
        public void AllPropertiesSetInConfigSection()
        {
            FuelSDKConfigurationSection section = GetCustomConfigurationSectionFromConfigFile(allPropertiesSetConfigFileName);
            Assert.AreEqual(section.AppSignature, "none");
            Assert.AreEqual(section.ClientId, "abc");
            Assert.AreEqual(section.ClientSecret, "cde");
            Assert.AreEqual(section.SoapEndPoint, "https://soapendpoint.com");
            Assert.AreEqual(section.AuthenticationEndPoint, "https://authendpoint.com");
            Assert.AreEqual(section.RestEndPoint, "https://restendpoint.com");
        }

        private FuelSDKConfigurationSection GetCustomConfigurationSectionFromConfigFile(string configFileName)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConfigFiles", configFileName);

            Configuration config
              = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                ConfigurationUserLevel.None);

            return config.GetSection("fuelSDK") as FuelSDKConfigurationSection;
        }
    }
}