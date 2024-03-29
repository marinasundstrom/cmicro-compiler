using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes the 'public' attribute.
    /// </summary>
    public class PublicAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 1;
            }
        }

        #endregion

        public override string ToString()
        {
            return "public";
        }
    }

    /// <summary>
    /// Symbolizes the 'private' attribute.
    /// </summary>
    public class PrivateAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 2;
            }
        }

        #endregion

        public override string ToString()
        {
            return "private";
        }
    }

    /// <summary>
    /// Symbolizes the 'static' attribute.
    /// </summary>
    public class StaticAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 3;
            }
        }

        #endregion

        public override string ToString()
        {
            return "static";
        }
    }

    /// <summary>
    /// Symbolizes the 'abstract' attribute.
    /// </summary>
    public class AbstractAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 4;
            }
        }

        #endregion

        public override string ToString()
        {
            return "abstract";
        }
    }

    /// <summary>
    /// Symbolizes the 'constant' attribute.
    /// </summary>
    public class ConstantAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 5;
            }
        }

        #endregion

        public override string ToString()
        {
            return "constant";
        }
    }

    /// <summary>
    /// Symbolizes the 'override' attribute.
    /// </summary>
    public class OverrideAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 6;
            }
        }

        #endregion

        public override string ToString()
        {
            return "override";
        }
    }

    /// <summary>
    /// Symbolizes the 'virtual' attribute.
    /// </summary>
    public class VirtualAttribute : IAttribute
    {

        #region IAttribute Members

        public int Kind
        {
            get
            {
                return 7;
            }
        }

        #endregion

        public override string ToString()
        {
            return "virtual";
        }
    }
}
