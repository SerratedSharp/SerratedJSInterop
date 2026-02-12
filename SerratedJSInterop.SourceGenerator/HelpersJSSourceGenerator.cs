using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SerratedSharp.SerratedJSInterop.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class HelpersJSSourceGenerator : IIncrementalGenerator
{
    private const string AgnosticModuleReferenceAttributeName = "SerratedSharp.SerratedJSInterop.AgnosticModuleReferenceAttribute";
    private const string AgnosticJSImportAttributeName = "SerratedSharp.SerratedJSInterop.AgnosticJSImportAttribute";
    private const string AgnosticJSMarshalAsAttributeName = "SerratedSharp.SerratedJSInterop.AgnosticJSMarshalAsAttribute`1";
    private const string GeneratedMarkerFileName = ".generated";

    /// <summary>Format for JSMarshalAs type arguments so we emit JSType.Any not Any, and JSType.Array&lt;JSType.Any&gt; not Array&lt;Any&gt;.</summary>
    private static readonly SymbolDisplayFormat JSTypeArgumentFormat = new SymbolDisplayFormat(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

    private static void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { /* generator cannot fail the build */ }
    }

    /// <summary>Base name for generated types: strip "Source" suffix if present.</summary>
    private static string GetBaseName(string typeName)
    {
        const string suffix = "Source";
        if (typeName.Length > suffix.Length && typeName.EndsWith(suffix, StringComparison.Ordinal))
            return typeName.Substring(0, typeName.Length - suffix.Length);
        return typeName;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0,
                static (ctx, ct) => GetClassWithModuleRef(ctx, ct))
            .Where(static x => x is not null)
            .Collect();

        var outputPathProvider = context.AdditionalTextsProvider
            .Collect()
            .Select(static (texts, _) =>
            {
                foreach (var t in texts)
                {
                    var path = t.Path;
                    if (string.IsNullOrEmpty(path)) continue;
                    if (path.EndsWith(GeneratedMarkerFileName, StringComparison.OrdinalIgnoreCase) ||
                        (path.Contains("SerratedJSInterop") && path.Contains("Generated")))
                    {
                        var dir = Path.GetDirectoryName(path);
                        if (!string.IsNullOrEmpty(dir)) return dir;
                    }
                }
                return string.Empty;
            });

        var combined = classProvider.Combine(outputPathProvider);

        context.RegisterSourceOutput(combined, static (sp, pair) =>
        {
            var (allClasses, outputPath) = pair;
            var classes = allClasses.Where(c => c is not null).Cast<HelpersClassInfo>().ToImmutableArray();
            if (classes.Length == 0) return;

            var hasOutput = !string.IsNullOrWhiteSpace(outputPath);
            if (hasOutput)
            {
                var dir = outputPath.Trim().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                if (string.IsNullOrEmpty(dir)) return;
                try
                {
                    Directory.CreateDirectory(dir);
                    // Remove legacy single-purpose files so we don't get duplicate type definitions
                    TryDelete(Path.Combine(dir, "HelpersProxy.g.cs"));
                    TryDelete(Path.Combine(dir, "InstanceHelper.g.cs"));
                    TryDelete(Path.Combine(dir, "InstanceHelperProxy.g.cs"));
                    var agnosticRuntime = GenerateAgnosticRuntime();
                    File.WriteAllText(Path.Combine(dir, "AgnosticRuntime.g.cs"), agnosticRuntime, Encoding.UTF8);
                    foreach (var classInfo in classes)
                    {
                        var baseName = GetBaseName(classInfo.TypeName);
                        var fileName = baseName + ".g.cs";
                        string? content = GenerateFileContent(classInfo, baseName);
                        if (content is not null)
                            File.WriteAllText(Path.Combine(dir, fileName), content, Encoding.UTF8);
                    }
                }
                catch { /* generator cannot fail the build */ }
            }
            else
            {
                var agnosticRuntime = GenerateAgnosticRuntime();
                sp.AddSource("AgnosticRuntime.g.cs", SourceText.From(agnosticRuntime, Encoding.UTF8));
                foreach (var classInfo in classes)
                {
                    var baseName = GetBaseName(classInfo.TypeName);
                    var fileName = baseName + ".g.cs";
                    string? content = GenerateFileContent(classInfo, baseName);
                    if (content is not null)
                        sp.AddSource(fileName, SourceText.From(content, Encoding.UTF8));
                }
            }
        });
    }

    private static string GenerateAgnosticRuntime()
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System.Runtime.InteropServices.JavaScript;");
        sb.AppendLine();
        sb.AppendLine("namespace SerratedSharp.SerratedJSInterop");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>Shared runtime flag for Uno vs .NET WASM. Used by generated proxy routing.</summary>");
        sb.AppendLine("    public static class AgnosticRuntime");
        sb.AppendLine("    {");
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Gets a value indicating whether being used from Uno.Wasm.Bootstrap rather than .NET8 wasmbrowser.");
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        internal static bool IsUnoWasmBootstrapLoaded => isUnoWasmBootstrapLoaded.Value;");
        sb.AppendLine("        private static Lazy<bool> isUnoWasmBootstrapLoaded = new Lazy<bool>(() =>");
        sb.AppendLine("        {");
        sb.AppendLine("            bool isUnoPresent = false;");
        sb.AppendLine("            if (JSHost.GlobalThis.HasProperty(\"IsFromUno\"))");
        sb.AppendLine("            {");
        sb.AppendLine("                if (JSHost.GlobalThis.GetPropertyAsBoolean(\"IsFromUno\"))");
        sb.AppendLine("                    isUnoPresent = true;");
        sb.AppendLine("            }");
        sb.AppendLine("            return isUnoPresent;");
        sb.AppendLine("        });");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>Returns content for a single {BaseName}.g.cs file, or null if type has no methods.</summary>
    private static string? GenerateFileContent(HelpersClassInfo classInfo, string baseName)
    {
        if (classInfo.Methods.Length == 0)
            return null;

        var sb = new StringBuilder();
        var proxyForDotNetName = baseName + "ProxyForDotNet";
        var proxyForUnoName = baseName + "ProxyForUno";
        bool nest = classInfo.NestProxiesInGlobalProxy;
        string proxyPrefix = nest ? "GlobalProxy." : "";

        // Consumed API: routing class named {baseName}
        sb.AppendLine("using System.Runtime.InteropServices.JavaScript;");
        sb.AppendLine();
        sb.AppendLine("namespace SerratedSharp.SerratedJSInterop");
        sb.AppendLine("{");
        sb.AppendLine($"    public static partial class {baseName}");
        sb.AppendLine("    {");
        foreach (var m in classInfo.Methods)
        {
            if (m.ReturnTypeIsTask)
                sb.AppendLine($"        public static async {m.ReturnType} {m.MethodName}({m.ParameterList})");
            else
                sb.AppendLine($"        public static {m.ReturnType} {m.MethodName}({m.ParameterList})");
            sb.AppendLine("        {");
            sb.AppendLine("            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)");
            if (m.ReturnTypeIsTask)
                sb.AppendLine($"                await {proxyPrefix}{proxyForUnoName}.{m.MethodName}({m.ArgumentList});");
            else
                sb.AppendLine($"                return {proxyPrefix}{proxyForUnoName}.{m.MethodName}({m.ArgumentList});");
            sb.AppendLine("            else");
            if (m.ReturnTypeIsTask)
                sb.AppendLine($"                await {proxyPrefix}{proxyForDotNetName}.{m.MethodName}({m.ArgumentList});");
            else
                sb.AppendLine($"                return {proxyPrefix}{proxyForDotNetName}.{m.MethodName}({m.ArgumentList});");
            sb.AppendLine("        }");
            sb.AppendLine();
        }
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();

        string paramList(MethodInfo m) => m.ParameterListForImport ?? m.ParameterList;

        if (nest)
        {
            sb.AppendLine("namespace SerratedSharp.SerratedJSInterop");
            sb.AppendLine("{");
            sb.AppendLine("    internal static partial class GlobalProxy");
            sb.AppendLine("    {");
            sb.AppendLine($"        internal partial class {proxyForDotNetName}");
            sb.AppendLine("        {");
            sb.AppendLine($"            private const string baseJSNamespace = \"{classInfo.BaseJSNamespace}\";");
            sb.AppendLine($"            private const string moduleName = \"{classInfo.ModuleName}\";");
            sb.AppendLine();
            foreach (var m in classInfo.Methods)
            {
                sb.AppendLine($"            [JSImport(baseJSNamespace + \".{m.JsName}\", moduleName)]");
                if (m.ReturnMarshalAttribute is not null)
                    sb.AppendLine("            " + m.ReturnMarshalAttribute);
                sb.AppendLine($"            public static partial {m.ReturnType} {m.MethodName}({paramList(m)});");
                sb.AppendLine();
            }
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        internal partial class {proxyForUnoName}");
            sb.AppendLine("        {");
            sb.AppendLine($"            private const string baseJSNamespace = \"globalThis.{classInfo.BaseJSNamespace}\";");
            sb.AppendLine();
            foreach (var m in classInfo.Methods)
            {
                sb.AppendLine($"            [JSImport(baseJSNamespace + \".{m.JsName}\")]");
                if (m.ReturnMarshalAttribute is not null)
                    sb.AppendLine("            " + m.ReturnMarshalAttribute);
                sb.AppendLine($"            public static partial {m.ReturnType} {m.MethodName}({paramList(m)});");
                sb.AppendLine();
            }
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
        }
        else
        {
            sb.AppendLine("namespace SerratedSharp.SerratedJSInterop");
            sb.AppendLine("{");
            sb.AppendLine($"    public static partial class {proxyForDotNetName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        private const string baseJSNamespace = \"{classInfo.BaseJSNamespace}\";");
            sb.AppendLine($"        private const string moduleName = \"{classInfo.ModuleName}\";");
            sb.AppendLine();
            foreach (var m in classInfo.Methods)
            {
                sb.AppendLine($"        [JSImport(baseJSNamespace + \".{m.JsName}\", moduleName)]");
                if (m.ReturnMarshalAttribute is not null)
                    sb.AppendLine("        " + m.ReturnMarshalAttribute);
                sb.AppendLine($"        public static partial {m.ReturnType} {m.MethodName}({paramList(m)});");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine($"    public static partial class {proxyForUnoName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        private const string baseJSNamespace = \"globalThis.{classInfo.BaseJSNamespace}\";");
            sb.AppendLine("        private const string moduleName = \"\";");
            sb.AppendLine();
            foreach (var m in classInfo.Methods)
            {
                sb.AppendLine($"        [JSImport(baseJSNamespace + \".{m.JsName}\", moduleName)]");
                if (m.ReturnMarshalAttribute is not null)
                    sb.AppendLine("        " + m.ReturnMarshalAttribute);
                sb.AppendLine($"        public static partial {m.ReturnType} {m.MethodName}({paramList(m)});");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    private static HelpersClassInfo? GetClassWithModuleRef(GeneratorSyntaxContext ctx, CancellationToken cancellationToken)
    {
        var symbol = ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancellationToken) as INamedTypeSymbol;
        if (symbol is null) return null;

        var compilation = ctx.SemanticModel.Compilation;
        var moduleRefAttr = compilation.GetTypeByMetadataName(AgnosticModuleReferenceAttributeName);
        var jsImportAttr = compilation.GetTypeByMetadataName(AgnosticJSImportAttributeName);
        var agnosticMarshalAttr = compilation.GetTypeByMetadataName(AgnosticJSMarshalAsAttributeName);
        if (moduleRefAttr is null || jsImportAttr is null) return null;

        var classAttr = symbol.GetAttributes().FirstOrDefault(a =>
            SymbolEqualityComparer.Default.Equals(a.AttributeClass, moduleRefAttr));
        if (classAttr is null || classAttr.ConstructorArguments.Length < 2) return null;

        var baseNs = classAttr.ConstructorArguments[0].Value as string;
        var moduleName = classAttr.ConstructorArguments[1].Value as string;
        if (string.IsNullOrEmpty(baseNs) || string.IsNullOrEmpty(moduleName)) return null;

        bool nestProxiesInGlobalProxy = classAttr.ConstructorArguments.Length >= 3 && classAttr.ConstructorArguments[2].Value is true;

        var methods = new List<MethodInfo>();
        var baseJSNamespace = baseNs!;
        var moduleNameVal = moduleName!;
        foreach (var member in symbol.GetMembers().OfType<IMethodSymbol>())
        {
            if (member.MethodKind != MethodKind.Ordinary || !member.IsStatic) continue;
            var methodAttr = member.GetAttributes().FirstOrDefault(a =>
                SymbolEqualityComparer.Default.Equals(a.AttributeClass, jsImportAttr));
            if (methodAttr is null) continue;

            string jsName = member.Name;
            if (methodAttr.ConstructorArguments.Length > 0 && methodAttr.ConstructorArguments[0].Value is string nameArg)
                jsName = nameArg;

            var ret = member.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            var parameters = string.Join(", ", member.Parameters.Select(p =>
                $"{p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {p.Name}"));
            var arguments = string.Join(", ", member.Parameters.Select(p => p.Name));
            bool returnTypeIsTask = member.ReturnType.ToDisplayString().StartsWith("System.Threading.Tasks.Task", StringComparison.Ordinal);
            bool needsJSMarshal = member.ReturnType.ToDisplayString().Contains("JSObject[]");

            string? parameterListForImport = TryGetParameterListWithMarshal(member, agnosticMarshalAttr);
            string? returnMarshalAttribute = TryGetReturnMarshalFromAttribute(member, agnosticMarshalAttr);
            if (parameterListForImport is null)
                parameterListForImport = string.Join(", ", member.Parameters.Select(p => FormatInstanceHelperParameter(p)));
            if (returnMarshalAttribute is null)
                returnMarshalAttribute = InferReturnMarshal(member.ReturnType);
            if (parameterListForImport is null)
                parameterListForImport = parameters;
            if (returnMarshalAttribute is null && needsJSMarshal)
                returnMarshalAttribute = "[return: JSMarshalAs<JSType.Array<JSType.Object>>]";

            methods.Add(new MethodInfo(
                member.Name,
                jsName,
                ret,
                parameters,
                arguments,
                returnTypeIsTask,
                needsJSMarshal,
                parameterListForImport,
                returnMarshalAttribute));
        }

        return new HelpersClassInfo(symbol.Name, baseJSNamespace, moduleNameVal, methods.ToImmutableArray(), nestProxiesInGlobalProxy);
    }

    private static string? TryGetReturnMarshalFromAttribute(IMethodSymbol method, INamedTypeSymbol? agnosticMarshalAttr)
    {
        if (agnosticMarshalAttr is null) return null;
        var attr = method.GetAttributes().FirstOrDefault(a =>
            a.AttributeClass?.IsGenericType == true &&
            SymbolEqualityComparer.Default.Equals(a.AttributeClass.OriginalDefinition, agnosticMarshalAttr));
        if (attr?.AttributeClass?.TypeArguments is not { Length: 1 } args) return null;
        var typeDisplay = args[0].ToDisplayString(JSTypeArgumentFormat);
        return "[return: JSMarshalAs<" + typeDisplay + ">]";
    }

    private static string? TryGetParameterListWithMarshal(IMethodSymbol method, INamedTypeSymbol? agnosticMarshalAttr)
    {
        if (agnosticMarshalAttr is null || method.Parameters.Length == 0) return null;
        var paramFormat = SymbolDisplayFormat.MinimallyQualifiedFormat;
        var parts = new List<string>();
        bool anyMarshal = false;
        foreach (var p in method.Parameters)
        {
            var paramAttr = p.GetAttributes().FirstOrDefault(a =>
                a.AttributeClass?.IsGenericType == true &&
                SymbolEqualityComparer.Default.Equals(a.AttributeClass.OriginalDefinition, agnosticMarshalAttr));
            if (paramAttr?.AttributeClass?.TypeArguments is { Length: 1 } args)
            {
                anyMarshal = true;
                var typeDisplay = args[0].ToDisplayString(JSTypeArgumentFormat);
                parts.Add("[JSMarshalAs<" + typeDisplay + ">] " + p.Type.ToDisplayString(paramFormat) + " " + p.Name);
            }
            else
                parts.Add(p.Type.ToDisplayString(paramFormat) + " " + p.Name);
        }
        return anyMarshal ? string.Join(", ", parts) : null;
    }

    private static string FormatInstanceHelperParameter(IParameterSymbol p)
    {
        var typeDisplay = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        string marshal = typeDisplay switch
        {
            "object" => "[JSMarshalAs<JSType.Any>] ",
            "object[]" => "[JSMarshalAs<JSType.Array<JSType.Any>>] ",
            _ => ""
        };
        return marshal + typeDisplay + " " + p.Name;
    }

    private static string? InferReturnMarshal(ITypeSymbol returnType)
    {
        var display = returnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        return display switch
        {
            "object" => "[return: JSMarshalAs<JSType.Any>]",
            "object[]" => "[return: JSMarshalAs<JSType.Array<JSType.Any>>]",
            _ => null
        };
    }

    private sealed class HelpersClassInfo
    {
        public string TypeName { get; }
        public string BaseJSNamespace { get; }
        public string ModuleName { get; }
        public ImmutableArray<MethodInfo> Methods { get; }
        public bool NestProxiesInGlobalProxy { get; }
        public HelpersClassInfo(string typeName, string baseJSNamespace, string moduleName, ImmutableArray<MethodInfo> methods, bool nestProxiesInGlobalProxy = false)
        {
            TypeName = typeName;
            BaseJSNamespace = baseJSNamespace;
            ModuleName = moduleName;
            Methods = methods;
            NestProxiesInGlobalProxy = nestProxiesInGlobalProxy;
        }
    }

    private sealed class MethodInfo
    {
        public string MethodName { get; }
        public string JsName { get; }
        public string ReturnType { get; }
        public string ParameterList { get; }
        public string ArgumentList { get; }
        public bool ReturnTypeIsTask { get; }
        public bool NeedsJSMarshalArrayObject { get; }
        public string? ParameterListForImport { get; }
        public string? ReturnMarshalAttribute { get; }
        public MethodInfo(string methodName, string jsName, string returnType, string parameterList, string argumentList, bool returnTypeIsTask, bool needsJSMarshalArrayObject, string? parameterListForImport = null, string? returnMarshalAttribute = null)
        {
            MethodName = methodName;
            JsName = jsName;
            ReturnType = returnType;
            ParameterList = parameterList;
            ArgumentList = argumentList;
            ReturnTypeIsTask = returnTypeIsTask;
            NeedsJSMarshalArrayObject = needsJSMarshalArrayObject;
            ParameterListForImport = parameterListForImport;
            ReturnMarshalAttribute = returnMarshalAttribute;
        }
    }
}
