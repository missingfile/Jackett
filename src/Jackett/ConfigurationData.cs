﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackett
{
    public abstract class ConfigurationData
    {
        public enum ItemType
        {
            InputString,
            InputBool,
            DisplayImage,
            DisplayInfo,
            HiddenData
        }

        public void LoadValuesFromJson(JToken json)
        {
            // todo: match up ids with items and fill values
            IDictionary<string, JToken> dictionary = (JObject)json;
            foreach (var item in GetItems())
            {
                switch (item.ItemType)
                {
                    case ItemType.InputString:
                        ((StringItem)item).Value = (string)dictionary[item.ID];
                        break;
                    case ItemType.InputBool:
                        ((BoolItem)item).Value = (bool)dictionary[item.ID];
                        break;
                }
            }
        }

        public JToken ToJson()
        {
            var items = GetItems();
            var jArray = new JArray();
            foreach (var item in items)
            {
                var jObject = new JObject();
                jObject["id"] = item.ID;
                jObject["type"] = item.ItemType.ToString().ToLower();
                jObject["name"] = item.Name;
                switch (item.ItemType)
                {
                    case ItemType.InputString:
                    case ItemType.HiddenData:
                    case ItemType.DisplayInfo:
                        jObject["value"] = ((StringItem)item).Value;
                        break;
                    case ItemType.InputBool:
                        jObject["value"] = ((BoolItem)item).Value;
                        break;
                    case ItemType.DisplayImage:
                        string dataUri = "data:image/jpeg;base64," + Convert.ToBase64String(((ImageItem)item).Value);
                        jObject["value"] = dataUri;
                        break;
                }
                jArray.Add(jObject);
            }
            return jArray;
        }

        public class Item
        {
            public ItemType ItemType { get; set; }
            public string Name { get; set; }
            public string ID { get { return Name.Replace(" ", "").ToLower(); } }
        }

        public class StringItem : Item
        {
            public string Value { get; set; }
        }

        public class BoolItem : Item
        {
            public bool Value { get; set; }
        }

        public class ImageItem : Item
        {
            public byte[] Value { get; set; }
        }

        public abstract Item[] GetItems();

    }
}
