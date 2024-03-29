using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a indexer declaration.
    /// </summary>
    public class IndexerDeclaration : Declaration, IClassMember
    {
        public IType ReturnType;

        public Dictionary<string, IType> IndexerArgs = new Dictionary<string, IType>();

        public List<IMethodElement> Set;
        public List<IMethodElement> Get;

        public List<IAttribute> Attributes = new List<IAttribute>();
    }
}
