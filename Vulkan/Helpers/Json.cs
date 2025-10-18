using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vulkan.Helpers;

internal static class Json 
{
	public static readonly JsonSerializerOptions SerializerOptions;

	private class IntPtrJsonConverter : JsonConverter<nint>
	{
		public override void Write(Utf8JsonWriter writer, nint value, JsonSerializerOptions options) => writer.WriteNumberValue((long)value);
		public override nint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
	}

	private class UIntPtrJsonConverter : JsonConverter<nuint>
	{
		public override void Write(Utf8JsonWriter writer, nuint value, JsonSerializerOptions options) => writer.WriteNumberValue((ulong)value);
		public override nuint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
	}

	public static string Serialize<T>(T value) => JsonSerializer.Serialize<T>(value, Json.SerializerOptions);

	static Json() 
	{
		Json.SerializerOptions = new() 
		{
			IncludeFields = true,
			IgnoreReadOnlyProperties = false,
			IndentCharacter = ' ',
			IndentSize = 4,
			WriteIndented = true
		};

		Json.SerializerOptions.Converters.Add(new IntPtrJsonConverter());
		Json.SerializerOptions.Converters.Add(new UIntPtrJsonConverter());
		Json.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
	}
}
