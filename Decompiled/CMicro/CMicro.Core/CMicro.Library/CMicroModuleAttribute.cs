using System;

namespace CMicro.Library
{
	public class CMicroModuleAttribute : Attribute
	{
		public CMicroModuleTypes ModuleType { get; protected set; }

		public CMicroModuleAttribute(CMicroModuleTypes moduleType)
		{
			ModuleType = moduleType;
		}
	}
}
