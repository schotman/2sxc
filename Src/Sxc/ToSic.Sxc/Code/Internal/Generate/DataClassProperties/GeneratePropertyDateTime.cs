﻿namespace ToSic.Sxc.Code.Internal.Generate;

internal class GeneratePropertyDateTime: GeneratePropertyBase
{
    public override ValueTypes ForDataType => ValueTypes.DateTime;

    public override List<CodeFragment> Generate(IContentTypeAttribute attribute, int tabs)
    {
        var name = attribute.Name;

        return
        [
            GenPropSnip(tabs, "DateTime", name, "DateTime", usings: UsingDateTime, summary:
            [
                $"Get the DateTime of {name}.",
            ]),
        ];
    }

    private List<string> UsingDateTime { get; } = ["System"];
}