using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Utils;
using System.IO;

namespace RgssSharp
{
	public static class Ruby
	{
		private static ScriptRuntime _runtime;
		private static ScriptEngine _engine;
		private static ScriptScope _scope;
		private static RubyContext _context;
		private static dynamic _marshal;

		internal static void Initialize()
		{
			_runtime = IronRuby.Ruby.CreateRuntime();
			_engine = IronRuby.Ruby.GetEngine(_runtime);
			_scope = _engine.CreateScope();
			_context = (RubyContext) HostingHelpers.GetLanguageContext(_engine);
			ContractUtils.RequiresNotNull(_runtime, "runtime");

			Eval(@"load_assembly 'IronRuby.Libraries', 'IronRuby.StandardLibrary.Win32API'");
			Eval(@"load_assembly 'IronRuby.Libraries', 'IronRuby.StandardLibrary.Zlib'");

			
			_marshal = Eval("Marshal");
		}

		public static dynamic Eval(string code)
		{
			return _engine.Execute(code, _scope);
		}

		public static T Eval<T>(string code)
		{
			return _engine.Execute<T>(code, _scope);
		}

		public static MutableString Pack(RubyArray array, string formatString)
		{
			var format = MutableString.Create(formatString);
			var integerConversion = new ConversionStorage<IntegerValue>(_context);
			var floatConversion = new ConversionStorage<double>(_context);
			var stringCast = new ConversionStorage<MutableString>(_context);
			var tosConversion = new ConversionStorage<MutableString>(_context);
			return ArrayOps.Pack(integerConversion, floatConversion, stringCast, tosConversion, array, format);
		}

		public static RubyArray Unpack(MutableString str, string formatString)
		{
			var format = MutableString.Create(formatString);
			return MutableStringOps.Unpack(str, format);
		}

		#region Marshal

		/// <summary>
		/// Uses Marshal to serialize an object and return as an array of bytes
		/// </summary>
		/// <param name="data">Object to serialize</param>
		/// <returns>Array of bytes representing the serialized object</returns>
		public static byte[] MarshalDump(object data)
		{
			return _marshal.dump(data).ToByteArray();
		}

		/// <summary>
		/// Uses Marshal to serialize an object and save it to disk
		/// </summary>
		/// <param name="filename">Filename where serialized data will be saved</param>
		/// <param name="data">Object to serialize and save</param>
		public static void MarshalDump(string filename, object data)
		{
			File.WriteAllBytes(filename, _marshal.dump(data));
		}

		/// <summary>
		/// Deserializes a Marshaled object and returns it
		/// </summary>
		/// <param name="filename">Filename of Marshaled object</param>
		/// <returns>Deserialized object</returns>
		public static dynamic MarshalLoad(string filename)
		{
			return _marshal.load(File.ReadAllBytes(filename));
		}

		/// <summary>
		/// Deserializes a Marshaled object and returns it
		/// </summary>
		/// <param name="data">Byte array of data to deserialize</param>
		/// <returns>Deserialized object</returns>
		public static dynamic MarshalLoad(byte[] data)
		{
			return _marshal.load(data);
		}

		#endregion
	}
}
