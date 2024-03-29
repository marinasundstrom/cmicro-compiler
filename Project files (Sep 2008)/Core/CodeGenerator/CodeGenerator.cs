using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;

using CSharp;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    public partial class CodeGenerator
    {
        /*
        private Program _internalRoot;

        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;

        public CodeGenerator(Program astRoot)
        {
            _internalRoot = astRoot;
        }

        public bool GenAssembly(string AssemblyName, AssemblyType Type)
        {
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(AssemblyName);

            this.GenGlobal();

            _moduleBuilder.CreateGlobalFunctions();

            _assemblyBuilder.Save(AssemblyName);

            return true;
        }

        private void GenGlobal()
        {
            foreach(var item in _internalRoot.Members)
            {
                if (item is ClassDeclaration)
                {
                    ClassDeclaration decl = (ClassDeclaration)item;

                    string name = string.Format("{0}.{1}", "global", decl.Name);
                    
                    TypeBuilder tb = _moduleBuilder.DefineType(name);
                    this.GenClass(tb, decl);

                }
                else if (item is MethodDeclaration)
                {
                    MethodDeclaration decl = (MethodDeclaration)item;

                    string name = string.Format("{0}.{1}", "global", decl.Name);

                    MethodBuilder mb = _moduleBuilder.DefineGlobalMethod(name, this.GenMethodAttributes(decl.Attributes) | MethodAttributes.Static, System.Type.GetType(decl.ReturnType.ToString()), this.GenMethodArguments(decl.Arguments)) ;
                    this.GenMethod(mb, decl);
                
                }
                else if (item is InterfaceDeclaration)
                {
                    
                }
                else if (item is NamespaceDeclaration)
                {

                }
            }
        }

        private System.Type GetType(IType t)
        {
            if (t is ArrayType)
            {
                ArrayType vd = (ArrayType)t;

                System.Type typeobj = System.Type.GetType(this.GetType(vd.Type).Name);
                typeobj = typeobj.MakeArrayType(vd.Dimensions);

                return typeobj;
            }
            else if (t is GenericType)
            {
                GenericType gd = (GenericType)t;

                System.Type typeobj = System.Type.GetType(this.GetType(gd.Type).Name);

                List<System.Type> parameters = new List<System.Type>();
                foreach(var param in gd.GenericParameters)
                    this.GetType(param);

                typeobj = typeobj.MakeGenericType(parameters.ToArray());

                return typeobj;
            }
            else
            {
                CSharp.Compiler.Ast.Type vd = (CSharp.Compiler.Ast.Type)t;

                System.Type typeobj = System.Type.GetType(vd.Name);

                if (vd.IsPointer)
                    typeobj = typeobj.MakePointerType();

                return typeobj;
            }
        }

        private System.Type[] GenMethodArguments(Dictionary<string, IType> dictionary)
        {
            List<System.Type> arglist = new List<System.Type>();

            foreach (var arg in dictionary)
            {
                arglist.Add(this.GetType(arg.Value));
            }

            return arglist.ToArray();
        }

        private MethodAttributes GenMethodAttributes(List<IAttribute> attributes)
        {
            MethodAttributes result = MethodAttributes.Private;;

            foreach (var attr in attributes)
            {
                if (attr is PublicAttribute)
                    result = MethodAttributes.Public;
                else if (attr is PrivateAttribute)
                    result = MethodAttributes.Private;

                if (attr is StaticAttribute)
                    result = result | MethodAttributes.Static;               
            }

            return result;
        }

        private void GenClass(TypeBuilder builder, ClassDeclaration classd)
        {

        }

        private void GenNestedClass(TypeBuilder builder, ClassDeclaration classd)
        {

        }

        private void GenMethod(MethodBuilder builder, MethodDeclaration methd)
        {
            
        }

        private void GenConstructor(ConstructorBuilder builder, ConstructorDeclaration methd)
        {

        }

        private void GenField(FieldBuilder builder, FieldVariableDeclaration fieldd)
        {

        }

        private void GenEnum(EnumBuilder builder, EnumDeclaration enumd)
        {

        }

        public enum AssemblyType
        {
            WindowsApplication, DLL
        }
         * */
    }
}
