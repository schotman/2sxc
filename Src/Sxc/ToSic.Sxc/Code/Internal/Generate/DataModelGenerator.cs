﻿using System.Text;
using ToSic.Eav.Apps;
using ToSic.Eav.Context;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Services;
using ToSic.Sxc.Code.Internal.HotBuild;
using ToSic.Sxc.Internal;

namespace ToSic.Sxc.Code.Internal.Generate;

/// <summary>
/// Experimental
/// </summary>
public class DataModelGenerator(IUser user, IAppStates appStates) : ServiceBase(SxcLogging.SxcLogName + ".DMoGen")
{
    internal const int DepthNamespace = 0;
    internal const int DepthClass = 1;
    internal const int DepthProperty = 2;
    internal const int Indent = 4;
    internal const string DataNamespace = ThisAppLoader.ThisAppBase + ".Data";
    internal const string AutoGeneratedSuffix = "AutoGenerated";
    internal const string InheritsDataItem16 = "Custom.Data.Item16";

    internal IUser User = user;
    internal CodeGenHelper CodeGenHelper = new();

    public DataModelGenerator Setup(int appId, string edition)
    {
        var appCache = appStates.GetCacheState(appId);
        AppState = appStates.ToReader(appCache);
        Edition = edition;
        return this;
    }

    public string Edition { get; private set; }

    public IAppState AppState { get; private set; }

    public string Dump()
    {
        // TODO: @2dm
        // - check Equals of the new objects
        // - check serialization
        var sb = new StringBuilder();

        var classFiles = DataFiles();
        foreach (var classSb in classFiles)
        {
            sb.AppendLine($"// ----------------------- file: {classSb.FileName} ----------------------- ");
            sb.AppendLine(classSb.FileContents);
            sb.AppendLine();
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private List<CodeFileRaw> DataFiles()
    {
        // Generate classes for all types in scope Default
        var types = AppState.ContentTypes.OfScope(Scopes.Default).ToList();
        var appResources = AppState.GetContentType(AppLoadConstants.TypeAppResources);
        if (appResources != null) types.Add(appResources);
        var appSettings = AppState.GetContentType(AppLoadConstants.TypeAppSettings);
        if (appSettings != null) types.Add(appSettings);

        var classFiles = types
            .Select(t => new DataClassGenerator(this, t, t.Name?.Replace("-", ""), Edition).PrepareFile())
            .ToList();
        return classFiles;
    }


    internal CodeFragment NamespaceWrapper(string @namespace)
    {
        return new("namespace", $"namespace {@namespace}" + "\n{", closing: "}");
    }

    internal CodeFragment ClassWrapper(string className, bool isAbstract, bool partial, string inherits)
    {
        var indent = CodeGenHelper.Indentation(DepthClass);
        var specifiers = (isAbstract ? "abstract " : "") + (partial ? "partial " : "");
        inherits = inherits.NullOrUse(i => $": {i}");

        var start = $$"""
                      {{indent}}public {{specifiers}}class {{className}}{{inherits}}
                      {{indent}}{
                      """;
        return new("class", start, closing: $"{indent}}}\n");
    }

}