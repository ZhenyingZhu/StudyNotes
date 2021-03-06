using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DotNetCoreConsole
{
    public class TestJsonSchema
    {
        public static void TestMain()
        {
            string jsonStr =
@"{
  'name': 'James',
  'hobbies': ['.NET', 'Blogging', 'Reading', 'Xbox', 'LOLCATS']
}";
            System.Console.WriteLine(JsonSchemaValidate(jsonStr));
        }

        private static bool JsonSchemaValidate(string jsonStr)
        {
            string schemaJson =
@"{
  'description': 'A person',
  'type': 'object',
  'properties': {
    'name': {'type': 'string'},
    'hobbies': {
      'type': 'array',
      'items': {'type': 'string'}
    }
  }
}";
            JSchema schema = JSchema.Parse(schemaJson);

            JObject person = JObject.Parse(jsonStr);
            return person.IsValid(schema);
        }
    }
}
