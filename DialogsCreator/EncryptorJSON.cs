using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;
using Newtonsoft.Json;

namespace DialogsCreator
{
    public class EncryptJSON
    {
        //public static void ain()
        //{
        //    //var data = new { name = "John Doe", age = 35 };
        //    var key = new byte[32] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32 }; // Key should be 32 bytes long
        //    string data = "{\r\n  \"language\": \"ru\",\r\n  \"positionCanvas\": \"10010,50013\",\r\n  \"elements\": [\r\n    {\r\n      \"idElement\": 1,\r\n      \"pathToSound\": null,\r\n      \"pathToImage\": null,\r\n      \"author\": \"1\",\r\n      \"question\": {\r\n        \"idElement\": 1,\r\n        \"text\": \"2\",\r\n        \"type\": 1,\r\n        \"nextElement\": {\r\n          \"idElement\": -1,\r\n          \"type\": 0,\r\n          \"textElement\": null\r\n        },\r\n        \"requests\": []\r\n      },\r\n      \"answers\": [\r\n        {\r\n          \"idElement\": 1,\r\n          \"text\": \"3\",\r\n          \"type\": 2,\r\n          \"nextElement\": {\r\n            \"idElement\": 2,\r\n            \"type\": 1,\r\n            \"textElement\": \"22\"\r\n          },\r\n          \"requests\": []\r\n        },\r\n        {\r\n          \"idElement\": 1,\r\n          \"text\": \"4\",\r\n          \"type\": 2,\r\n          \"nextElement\": {\r\n            \"idElement\": 3,\r\n            \"type\": 1,\r\n            \"textElement\": \"222\"\r\n          },\r\n          \"requests\": []\r\n        }\r\n      ],\r\n      \"point\": \"10237,50163\"\r\n    },\r\n    {\r\n      \"idElement\": 2,\r\n      \"pathToSound\": null,\r\n      \"pathToImage\": null,\r\n      \"author\": \"11\",\r\n      \"question\": {\r\n        \"idElement\": 2,\r\n        \"text\": \"22\",\r\n        \"type\": 1,\r\n        \"nextElement\": {\r\n          \"idElement\": -1,\r\n          \"type\": 0,\r\n          \"textElement\": null\r\n        },\r\n        \"requests\": [\r\n          {\r\n            \"idElement\": 1,\r\n            \"type\": 2,\r\n            \"textElement\": \"3\"\r\n          }\r\n        ]\r\n      },\r\n      \"answers\": [\r\n        {\r\n          \"idElement\": 2,\r\n          \"text\": \"33\",\r\n          \"type\": 2,\r\n          \"nextElement\": {\r\n            \"idElement\": -1,\r\n            \"type\": 0,\r\n            \"textElement\": null\r\n          },\r\n          \"requests\": []\r\n        }\r\n      ],\r\n      \"point\": \"11225,50138\"\r\n    },\r\n    {\r\n      \"idElement\": 3,\r\n      \"pathToSound\": null,\r\n      \"pathToImage\": null,\r\n      \"author\": \"111\",\r\n      \"question\": {\r\n        \"idElement\": 3,\r\n        \"text\": \"222\",\r\n        \"type\": 1,\r\n        \"nextElement\": {\r\n          \"idElement\": -1,\r\n          \"type\": 0,\r\n          \"textElement\": null\r\n        },\r\n        \"requests\": [\r\n          {\r\n            \"idElement\": 1,\r\n            \"type\": 2,\r\n            \"textElement\": \"4\"\r\n          }\r\n        ]\r\n      },\r\n      \"answers\": [\r\n        {\r\n          \"idElement\": 3,\r\n          \"text\": \"333\",\r\n          \"type\": 2,\r\n          \"nextElement\": {\r\n            \"idElement\": -1,\r\n            \"type\": 0,\r\n            \"textElement\": null\r\n          },\r\n          \"requests\": []\r\n        },\r\n        {\r\n          \"idElement\": 3,\r\n          \"text\": \"444\",\r\n          \"type\": 2,\r\n          \"nextElement\": {\r\n            \"idElement\": -1,\r\n            \"type\": 0,\r\n            \"textElement\": null\r\n          },\r\n          \"requests\": []\r\n        }\r\n      ],\r\n      \"point\": \"11048,50590\"\r\n    }\r\n  ],\r\n  \"linkeds\": [\r\n    {\r\n      \"OutIdDialogView\": 1,\r\n      \"OutIdOptionView\": 0,\r\n      \"OutIdBindingView\": 1,\r\n      \"InIdDialogView\": 2,\r\n      \"InIdOptionView\": -1,\r\n      \"InIdBindingView\": 0,\r\n      \"LinesCoords\": [\r\n        {\r\n          \"X1\": 10652.0,\r\n          \"X2\": 11210.0,\r\n          \"Y1\": 50263.30333333334,\r\n          \"Y2\": 50162.08666666667\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"OutIdDialogView\": 1,\r\n      \"OutIdOptionView\": 1,\r\n      \"OutIdBindingView\": 1,\r\n      \"InIdDialogView\": 3,\r\n      \"InIdOptionView\": -1,\r\n      \"InIdBindingView\": 0,\r\n      \"LinesCoords\": [\r\n        {\r\n          \"X1\": 10652.0,\r\n          \"X2\": 11033.0,\r\n          \"Y1\": 50318.433333333334,\r\n          \"Y2\": 50614.08666666667\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
        //    // Serialize JSON to string
        //    var json = JsonConvert.SerializeObject(data);

        //    // Encrypt JSON string
        //    var encryptedJson = Encrypt(json, key);

        //    // Write encrypted JSON to file
        //    File.WriteAllBytes("encrypted.json", encryptedJson);

        //    // Decrypt JSON string
        //    var decryptedJson = Decrypt(encryptedJson, key);

        //    // Deserialize JSON from string
        //    var decryptedData = JsonConvert.DeserializeObject<string>(decryptedJson);

        //    //Console.WriteLine(decryptedData.name);
        //    //Console.WriteLine(decryptedData.age);
        //}

        public static byte[] Encrypt(string plainText, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();

            var encrypted = ms.ToArray();
            var result = new byte[aes.IV.Length + encrypted.Length];
            aes.IV.CopyTo(result, 0);
            encrypted.CopyTo(result, aes.IV.Length);
            return result;
        }

        public static string Decrypt(byte[] cipherText, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            var iv = new byte[aes.BlockSize / 8];
            var encrypted = new byte[cipherText.Length - iv.Length];
            Array.Copy(cipherText, 0, iv, 0, iv.Length);
            Array.Copy(cipherText, iv.Length, encrypted, 0, encrypted.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(encrypted);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}