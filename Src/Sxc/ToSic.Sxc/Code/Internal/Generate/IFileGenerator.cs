﻿using ToSic.Lib.Data;

namespace ToSic.Sxc.Code.Internal.Generate;

/// <summary>
/// Describes a file generator which can generate files - typically code.
/// </summary>
[PrivateApi]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public interface IFileGenerator: IHasIdentityNameId
{
    public string Name { get; }

    public string Version { get; }

    public string Description { get; }

    public string OutputLanguage { get; }
    public string OutputType { get; }

    public ICodeFileBundle[] Generate();
}