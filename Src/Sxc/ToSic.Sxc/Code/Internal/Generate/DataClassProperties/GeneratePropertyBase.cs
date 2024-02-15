﻿using ToSic.Eav.Plumbing;

namespace ToSic.Sxc.Code.Internal.Generate;

internal abstract class GeneratePropertyBase
{
    public abstract ValueTypes ForDataType { get; }

    protected CodeGenHelper CodeGenHelper => _codeGenHelper ??= new();
    private CodeGenHelper _codeGenHelper;

    public abstract List<CodeFragment> Generate(IContentTypeAttribute attribute, int tabs);

    protected CodeFragment GenPropSnip(int tabs, string returnType, string name, string method,
        NoParamOrder protector = default, string[] summary = default, string parameters = default, bool priority = true, List<string> usings = default)
    {
        var comment = CodeGenHelper.XmlComment(tabs, summary);
        return new(name, comment + GenProp(tabs, returnType, name, method, parameters: parameters), priority: priority, usings: usings);
    }

    private string GenProp(int tabs, string returnType, string name, string method, string parameters)
    {
        if (parameters.HasValue())
            parameters = ", " + parameters;

        return $"{CodeGenHelper.Indentation(tabs)}public {returnType} {name} => {method}(\"{name}\"{parameters});";
    }
}