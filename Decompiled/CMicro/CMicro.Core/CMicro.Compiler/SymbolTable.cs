using System.Collections.Generic;

namespace CMicro.Compiler
{
	public class SymbolTable
	{
		private int _currentScope = 0;

		private List<Symbol> _symbols = new List<Symbol>();

		public int Levels => _currentScope;

		public List<Symbol> Symbols
		{
			get
			{
				return _symbols;
			}
			set
			{
				_symbols = value;
			}
		}

		public List<Symbol> CurrentScope => _symbols.FindAll((Symbol x) => x.ScopeID == _currentScope);

		public List<Symbol> GlobalScope => _symbols.FindAll((Symbol x) => x.ScopeID == 0);

		public Symbol this[int index] => _symbols[index];

		public void BeginScope()
		{
			_currentScope++;
		}

		public bool EndScope()
		{
			if (_currentScope > 0)
			{
				_symbols.RemoveAll((Symbol n) => n.ScopeID == _currentScope);
				_currentScope--;
				return true;
			}
			return false;
		}

		public void NewSymbol(string name, SymbolType type)
		{
			_symbols.Add(new Symbol(name, _currentScope, type));
		}

		public void NewSymbol(string name, SymbolType type, bool isParameter)
		{
			_symbols.Add(new Symbol(name, _currentScope, type, isParameter));
		}

		public void NewGlobalSymbol(string name, SymbolType type)
		{
			_symbols.Add(new Symbol(name, 0, type));
		}

		public bool Exists(string name)
		{
			return _symbols.Exists((Symbol n) => n.Name == name);
		}
	}
}
