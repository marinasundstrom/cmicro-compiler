using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Base class of type classes.
    /// </summary>
    public class IType : Expression
    {
        public virtual string TypeName
        {
            get { return "IType"; }
        }
    }

    /// <summary>
    /// Symbolizes a simple type.
    /// </summary>
    public class Type : IType
    {
        public Type(string Name)
        {
            this.Name = Name;
        }

        public string Name;

        public bool IsPointer;

        public override string ToString()
        {
            return IsPointer? Name + "*" : Name;
        }

        public override string TypeName
        {
            get { return this.ToString(); }
        }
    }

    /// <summary>
    /// Symbolizes an array type.
    /// </summary>
    public class ArrayType : IType
    {
        public IType Type;
        public int Dimensions = 1;

        public bool IsPointer;

        public override string ToString()
        {
            if(IsPointer)
                return string.Format("{0}[]*", Type);
            else
                return string.Format("{0}[]", Type);
        }

        public override string TypeName
        {
            get { return this.ToString(); }
        }
    }

    /// <summary>
    /// Symbolizes a generic type.
    /// </summary>
    public class GenericType : IType
    {
        public IType Type;
        public List<IType> GenericParameters = new List<IType>();

        public override string ToString()
        {
            return string.Format("{0}<{1}>", Type, this.getGenericParams());
        }

        public override string TypeName
        {
            get { return string.Format("{0}({1})", Type, this.getGenericParams()); }
        }

        private string getGenericParams()
        {
            string param = "";

            int i = 0;
            while(i < this.GenericParameters.Count)
            {
                param += this.GenericParameters[i].TypeName;

                if (i < this.GenericParameters.Count-1)
                    param += ", ";

                i++;
            }

            return param;
        }
    }

    //public enum Type
    //{

    //public enum Type
    //{
    //    Int,
    //    Double,
    //    Bool,
    //    Char,
    //    String,
    //    Struct,
    //    Class
    //}
 
    //    Int = 0,
    //    SignedInt = 0,
    //    UnsignedInt = 1,
    //    LongInt = 2,
    //    ShortInt = 3,
    //    Float = 4,
    //    Double = 5,
    //    Char = 6,
    //    SignedChar = 6,
    //    UnsignedChar = 7,
    //    Struct = 8,
    //    Union = 9
    //}
}
