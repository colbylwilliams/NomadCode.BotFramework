using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace NomadCode.BotFramework
{
	public partial class Entity
	{

		[JsonExtensionData (ReadData = true, WriteData = true)]
		public JObject Properties { get; set; }

		public T GetAs<T> ()
		{
			return Properties.ToObject<T> ();
		}

		public void SetAs<T> (T obj)
		{
			Properties = JObject.FromObject (obj);
		}
	}
}
