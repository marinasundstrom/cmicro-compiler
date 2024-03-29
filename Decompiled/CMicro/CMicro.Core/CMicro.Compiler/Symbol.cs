namespace CMicro.Compiler
{
	public class Symbol
	{
		public string Name { get; protected set; }

		public bool IsParameter { get; protected set; }

		public int ScopeID { get; protected set; }

		public SymbolType Type { get; protected set; }

		public Symbol(string name, int scopeId, SymbolType symbolType)
		{
			Name = name;
			ScopeID = scopeId;
			Type = symbolType;
		}

		public Symbol(string name, int scopeId, SymbolType symbolType, bool isParameter)
		{
			Name = name;
			ScopeID = scopeId;
			Type = symbolType;
			IsParameter = isParameter;
		}
	}
}
