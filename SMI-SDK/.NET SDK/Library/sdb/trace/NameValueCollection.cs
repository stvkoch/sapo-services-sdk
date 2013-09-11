using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pt.sapo.sdb.trace
{
	[CollectionDataContract(Namespace = TracingContractConstants.Namespace)]
	[JsonConverter(typeof(NameValueCollectionJsonConverter))]
	public class NameValueCollection : List<NameValuePair>
	{
		private class NameValueCollectionJsonConverter : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(NameValueCollection);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				NameValueCollection result = null;
				if (reader.TokenType == JsonToken.StartObject)
				{
					result = new NameValueCollection();

					while (reader.Read() && reader.TokenType != JsonToken.EndObject)
					{
						var name = (string)reader.Value;
						reader.Read();
						if (reader.TokenType == JsonToken.StartObject)
						{
							var value = serializer.Deserialize<NameValueCollection>(reader);
							result.Add(name, value);
						}
						else
						{
							var value = serializer.Deserialize<string>(reader);
							result.Add(name, value);
						}
					}
				}
				return result;
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				writer.WriteStartObject();
				foreach (var item in (NameValueCollection)value)
				{
					writer.WritePropertyName(item.Name);
					if (item.ValuePairs != null)
					{
						serializer.Serialize(writer, item.ValuePairs);
					}
					else
					{
						writer.WriteValue(item.ValueString);
					}
				}
				writer.WriteEndObject();
			}
		}

		public void Add(string key, string value)
		{
			Add(new NameValuePair { Name = key, ValueString = value });
		}

		public void Add(string key, NameValueCollection value)
		{
			Add(new NameValuePair { Name = key, ValuePairs = value });
		}
	}
}