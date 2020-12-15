using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Authentication.Keycloak.Providers.KeycloakConfiguration
{
    public class KeycloakConfigurationProvider : JsonConfigurationProvider
    {
        internal const string CONFIGURATION_PREFIX = "Keycloak";
        private const char KEYCLOAK_PROPERTY_DELIMITER = '-';
        private const char NESTED_CONFIGURATION_DELIMITER = ':';
        private const int UTF8_LOWCASE_DISTANT = 32;
        private readonly StringBuilder _sb;

        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public KeycloakConfigurationProvider(KeycloakConfigurationSource source) : base(source)
        {
            _sb = new StringBuilder();
        }

        /// <summary>
        /// Loads the JSON data from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public override void Load(Stream stream)
        {
            base.Load(stream);
            Data = Data.ToDictionary(
                x => NormalizeKey(x.Key),
                x => x.Value,
                StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string NormalizeKey(string key)
        {
            foreach (var section in key.ToUpper().Split(NESTED_CONFIGURATION_DELIMITER))
            {
                if (_sb.Length != 0)
                {
                    _sb.Append(NESTED_CONFIGURATION_DELIMITER);
                }

                foreach (var x in section.Split(KEYCLOAK_PROPERTY_DELIMITER))
                {
                    for (var i = 0; i < x.Length; i++)
                    {
                        var @char = x[i];
                        if (i == 0)
                        {
                            _sb.Append(@char);
                        }
                        else
                        {
                            _sb.Append((char)(@char + UTF8_LOWCASE_DISTANT));
                        }
                    }
                }
            }

            var result = CONFIGURATION_PREFIX + NESTED_CONFIGURATION_DELIMITER + _sb.ToString();
            _sb.Clear();
            return result;
        }
    }
}
