namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public class Handle
	{
		private object _handle;

		public Handle(object obj)
		{
			_handle = obj;
		}

		public void SetReference(string obj)
		{
			_handle = obj;
		}

		public object GetReference()
		{
			return _handle;
		}

		public override string ToString()
		{
			if (_handle != null)
			{
				return _handle.GetType().Name;
			}
			return "null";
		}
	}
}
