using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using CSharp;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    /// <summary>
    /// Generates Xml out of the Ast nodes defined in the Ast namespace.
    /// </summary>
    public class CodeXMLGenerator
    {
        public Program _ast;
        public XElement _doc;

        public CodeXMLGenerator(Program ProgramAst)
        {
            this._ast = ProgramAst;
        }

        /// <summary>
        /// Generates Xml and returns the root XElement.
        /// </summary>
        /// <returns>The root XElement.</returns>
        public XElement Generate()
        {
            this._doc = new XElement("code");

            foreach (var refe in this._ast.References)
                this._doc.Add(new XElement("import", refe));

            foreach (IProgramDeclaration member in this._ast.Members)
            {
                if (member is ClassDeclaration)
                {
                    GenClass((ClassDeclaration)member, this._doc);
                }
                else if (member is NamespaceDeclaration)
                {
                    GenNamespace((NamespaceDeclaration)member, this._doc);
                }
                else if (member is MethodDeclaration)
                {
                    GenMethod((MethodDeclaration)member, this._doc);
                }
                else if (member is InterfaceDeclaration)
                {
                    GenInterface((InterfaceDeclaration)member, this._doc);
                }
                else if (member is EnumDeclaration)
                {
                    GenEnum((EnumDeclaration)member, this._doc);
                }
                else if (member is DelegateDeclaration)
                {
                    GenDelegate((DelegateDeclaration)member, this._doc);
                }
            }

            return this._doc;
        }

        /// <summary>
        /// Generates a Xml and save it as a file
        /// </summary>
        /// <param name="toFile">The disired path you want to save the output to.</param>
        public void GenerateAndSave(string toFile)
        {
            this.Generate();

            this._doc.Save(toFile);
        }

        private void GenNamespace(NamespaceDeclaration namespaced, XElement parent)
        {
            XElement element = new XElement("namespace");
            element.SetAttributeValue("name", namespaced.Name);

            if (namespaced.Internal)
                element.SetAttributeValue("internal", namespaced.Internal);

            //Members
            foreach (INamespaceMember member in namespaced.Members)
            {
                if (member is ClassDeclaration)
                {
                    GenClass((ClassDeclaration)member, element);
                }
                else if (member is MethodDeclaration)
                {
                    GenMethod((MethodDeclaration)member, element);
                }
                else if (member is InterfaceDeclaration)
                {
                    GenInterface((InterfaceDeclaration)member, element);
                }
                else if (member is EnumDeclaration)
                {
                    GenEnum((EnumDeclaration)member, element);
                }
                else if (member is DelegateDeclaration)
                {
                    GenDelegate((DelegateDeclaration)member, this._doc);
                }
            }

            parent.Add(element);
        }

        private void GenMethod(MethodDeclaration methd, XElement parent)
        {
            XElement element = new XElement("method");
            element.SetAttributeValue("name", methd.Name);
            element.SetAttributeValue("returns", methd.ReturnType.TypeName);

            //Modifiers
            string _mod = "";
            foreach (var mod in methd.Attributes)
            {
                if (mod is StaticAttribute)
                {
                    element.SetAttributeValue("static", "true");
                }
                else if (mod is VirtualAttribute)
                {
                    element.SetAttributeValue("virtual", "true");
                }
                else
                {
                    _mod += mod.ToString() + "; ";
                }
            }

            //Generics
            if (methd.IsGeneric)
            {
                string _param = "";
                foreach (var par in methd.GenricParameters)
                {
                    _param += par.TypeName + "; ";
                }

                element.SetAttributeValue("genericparams", _param);
            }

            //Parameters
            if (methd.Arguments.Count > 0)
            {
                string _args = "";
                foreach (var arg in methd.Arguments)
                {
                    _args += arg.Key + ":" + arg.Value.TypeName + "; ";
                }

                element.SetAttributeValue("parameters", _args);
            }

            element.Add("Code");

            parent.Add(element);
        }

        private void GenClass(ClassDeclaration classd, XElement parent)
        {
            XElement element = new XElement("class");
            element.SetAttributeValue("name", classd.Name);

            //Modifiers
            if (classd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in classd.Attributes)
                {
                    if (mod is AbstractAttribute)
                    {
                        element.SetAttributeValue("abstract", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }
                element.SetAttributeValue("modifiers", _mod);
            }

            //Generics
            if (classd.IsGeneric)
            {
                string _param = "";
                foreach (var par in classd.GenricParameters)
                {
                    _param += par + "; ";
                }

                element.SetAttributeValue("genericparams", _param);
            }

            //Extends
            if (classd.Inherits != null)
                element.SetAttributeValue("extends", classd.Inherits.TypeName);

            //Implements
            if (classd.Implements.Count > 0)
            {
                string _impl = "";
                foreach (var mod in classd.Implements)
                {
                    _impl += mod.TypeName + "; ";
                }

                element.SetAttributeValue("implements", _impl);
            }



            //Members
            foreach (IClassMember member in classd.Members)
            {
                if (member is ClassDeclaration)
                {
                    GenClass((ClassDeclaration)member, element);
                }
                else if (member is MethodDeclaration)
                {
                    GenMethod((MethodDeclaration)member, element);
                }
                else if (member is ConstructorDeclaration)
                {
                    GenConstructor((ConstructorDeclaration)member, element);
                }
                else if (member is PropertyDeclaration)
                {
                    GenProperty((PropertyDeclaration)member, element);
                }
                else if (member is IndexerDeclaration)
                {
                    GenIndexer((IndexerDeclaration)member, element);
                }
                else if (member is FieldVariableDeclaration)
                {
                    GenField((FieldVariableDeclaration)member, element);
                }
                else if (member is InterfaceDeclaration)
                {
                    GenInterface((InterfaceDeclaration)member, element);
                }
                else if (member is EnumDeclaration)
                {
                    GenEnum((EnumDeclaration)member, element);
                }
                else if (member is DelegateDeclaration)
                {
                    GenDelegate((DelegateDeclaration)member, this._doc);
                }
            }

            parent.Add(element);
        }

        private void GenConstructor(ConstructorDeclaration methd, XElement parent)
        {
            XElement element = new XElement("constructor");
            element.SetAttributeValue("name", methd.Name);

            //Modifiers
            if (methd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in methd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }
                element.SetAttributeValue("modifiers", _mod);
            }

            //Parameters
            if (methd.Arguments.Count > 0)
            {
                string _args = "";
                foreach (var arg in methd.Arguments)
                {
                    _args += arg.Key + ":" + arg.Value.TypeName + "; ";
                }

                element.SetAttributeValue("parameters", _args);
            }

            parent.Add(element);
        }

        private void GenProperty(PropertyDeclaration propd, XElement parent)
        {
            XElement element = new XElement("property");
            element.SetAttributeValue("name", propd.Name);
            element.SetAttributeValue("returns", propd.ReturnType.TypeName);

            //Modifiers
            if (propd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in propd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    else if (mod is VirtualAttribute)
                    {
                        element.SetAttributeValue("virtual", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }
                element.SetAttributeValue("modifiers", _mod);
            }

            if (propd.Get != null)
            {
                element.Add(new XElement("get", "Code"));
            }

            if (propd.Set != null)
            {
                element.Add(new XElement("set", "Code"));
            }

            parent.Add(element);
        }

        private void GenField(FieldVariableDeclaration fieldd, XElement parent)
        {
            XElement element = new XElement("field");
            element.SetAttributeValue("name", fieldd.Name);
            element.SetAttributeValue("type", fieldd.Type.TypeName);

            //Modifiers
            if (fieldd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in fieldd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    if (mod is ConstantAttribute)
                    {
                        element.SetAttributeValue("constant", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                    element.SetAttributeValue("modifiers", _mod);
                }
            }

            parent.Add(element);
        }

        private void GenIndexer(IndexerDeclaration indexerd, XElement parent)
        {
            XElement element = new XElement("indexer");
            element.SetAttributeValue("type", indexerd.ReturnType.TypeName);

            //Modifiers

            if (indexerd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in indexerd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    else if (mod is VirtualAttribute)
                    {
                        element.SetAttributeValue("virtual", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }
                element.SetAttributeValue("modifiers", _mod);
            }

            if (indexerd.IndexerArgs.Count > 0)
            {
                string _args = "";
                foreach (var mod in indexerd.IndexerArgs)
                {
                        _args += mod.Key + ":" + mod.Value.TypeName + "; ";
                }
                element.SetAttributeValue("indexerparams", _args);
            }

            if (indexerd.Get != null)
            {
                element.Add(new XElement("get", "Code"));
            }

            if (indexerd.Set != null)
            {
                element.Add(new XElement("set", "Code"));
            }

            parent.Add(element);
        }


        private void GenDelegate(DelegateDeclaration methd, XElement parent)
        {
            XElement element = new XElement("delegate");
            element.SetAttributeValue("name", methd.Name);
            element.SetAttributeValue("returns", methd.ReturnType.TypeName);

            //Modifiers
            if (methd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in methd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    else if (mod is VirtualAttribute)
                    {
                        element.SetAttributeValue("virtual", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }
            }

            //Generics
            if (methd.IsGeneric)
            {
                string _param = "";
                foreach (var par in methd.GenricParameters)
                {
                    _param += par.TypeName + "; ";
                }

                element.SetAttributeValue("genericparams", _param);
            }

            //Parameters
            string _args = "";
            foreach (var arg in methd.Arguments)
            {
                _args += arg.Key + ":" + arg.Value.TypeName + "; ";
            }

            element.SetAttributeValue("parameters", _args);

            parent.Add(element);
        }

        private void GenInterface(InterfaceDeclaration interfd, XElement parent)
        {
            XElement element = new XElement("interface");
            element.SetAttributeValue("name", interfd.Name);

            //Modifiers
            string _mod = "";
            foreach (var mod in interfd.Attributes)
            {
                if (mod is AbstractAttribute)
                {
                    element.SetAttributeValue("abstract", "true");
                }
                else
                {
                    _mod += mod.ToString() + "; ";
                }
            }

            element.SetAttributeValue("modifiers", _mod);

            //Implements
            if (interfd.Implements.Count > 0)
            {
                string _impl = "";
                foreach (var mod in interfd.Implements)
                {
                    _impl += mod.TypeName + "; ";
                }

                element.SetAttributeValue("implements", _impl);
            }

            if (interfd.Members != null)
            {
                foreach (var member in interfd.Members)
                {
                    if (member is InterfaceMethod)
                    {
                        InterfaceMethod methd = (InterfaceMethod)member;

                        XElement innerelement = new XElement("method");
                        innerelement.SetAttributeValue("name", methd.Name);
                        innerelement.SetAttributeValue("returns", methd.ReturnType.TypeName);

                        //Generics
                        if (methd.IsGeneric)
                        {
                            string _param = "";
                            foreach (var par in methd.GenericParameters)
                            {
                                _param += par.TypeName + "; ";
                            }

                            innerelement.SetAttributeValue("genericparams", _param);
                        }

                        //Parameters
                        string _args = "";
                        foreach (var arg in methd.Arguments)
                        {
                            _args += arg.Key + ":" + arg.Value.TypeName + "; ";
                        }

                        innerelement.SetAttributeValue("parameters", _args);

                        element.Add(innerelement);

                    }
                    if (member is InterfaceProperty)
                    {
                        InterfaceProperty propd = (InterfaceProperty)member;

                        XElement innerelement = new XElement("property");
                        innerelement.SetAttributeValue("name", propd.Name);
                        innerelement.SetAttributeValue("returns", propd.ReturnType.TypeName);

                        innerelement.SetAttributeValue("modifiers", _mod);

                        if (propd.HasGetter)
                        {
                            innerelement.Add(new XElement("get"));
                        }

                        if (propd.HasSetter)
                        {
                            innerelement.Add(new XElement("set"));
                        }

                        element.Add(innerelement);
                    }
                }
            }

            parent.Add(element);
        }

        private void GenEnum(EnumDeclaration enumd, XElement parent)
        {
            XElement element = new XElement("enum");
            element.SetAttributeValue("name", enumd.Name);

            //Modifiers
            if (enumd.Attributes.Count > 0)
            {
                string _mod = "";
                foreach (var mod in enumd.Attributes)
                {
                    if (mod is StaticAttribute)
                    {
                        element.SetAttributeValue("static", "true");
                    }
                    else
                    {
                        _mod += mod.ToString() + "; ";
                    }
                }

                element.SetAttributeValue("modifiers", _mod);
            }

            foreach (var member in enumd.Members)
            {
                XElement melement = new XElement("member");
                melement.SetAttributeValue("name", member.Key);
                melement.SetAttributeValue("value", member.Value);

                element.Add(melement);
            }

            parent.Add(element);
        }
    }
}
