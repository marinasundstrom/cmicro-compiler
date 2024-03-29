using System;

namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public static class Types
	{
		public static string getTypename(object obj)
		{
			return obj.GetType().Name;
		}

		public static object @typeof(object obj)
		{
			return obj.GetType();
		}

		public static string getClrTypename(object obj)
		{
			return obj.GetType().FullName;
		}

		public static bool isPrimitive(object obj)
		{
			return obj.GetType().IsPrimitive;
		}

		public static bool isValueType(object obj)
		{
			return obj.GetType().IsValueType;
		}

		public static bool isPointer(object obj)
		{
			return obj.GetType().IsPointer;
		}

		public static bool isEnum(object obj)
		{
			return obj.GetType().IsEnum;
		}

		public static bool isClass(object obj)
		{
			return obj.GetType().IsClass;
		}

		public static bool isArray(object obj)
		{
			return obj.GetType().IsArray;
		}

		public static void array_reverse(object[] obj)
		{
			Array.Reverse((Array)obj);
		}
	}
}
