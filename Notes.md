# Solution notes

## NuGet pack tasks (Task Runner Explorer)

The VS extension **NPM Task Runner** (by Mads Kristensen) is required for pack tasks to appear in Task Runner Explorer. Install from: [NPM Task Runner](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.NPMTaskRunner).

### Available tasks

| Task           | Description                                              |
|----------------|----------------------------------------------------------|
| **pack:release** | Packs all IsPackable projects as Release; outputs to `nugetpackout/` |
| **pack:debug**   | Packs all IsPackable projects as Debug; outputs to `nugetpackout/`   |

Add the `nugetpackout` folder as a NuGet package source in VS (or other solutions) to reference the built packages locally.

## Troubleshooting

Missing WebAssembly in indivdual installer:

WasmBrowser projects require the "WebAssembly Build Tools" from Visual Studio Installer Individual Components.
Also requires the wasm-experimental workload to be installed.
Sometimes these need to be reinstalled after a Visual Studio update.

WasmBrowser projects must appear alphabetically before RCL projects.  See: https://github.com/dotnet/aspnetcore/issues/55105

## Publish and run with dotnet serve

The WASM test hosts can be published (see README § AOT and trimming for AOT/trimming options) then served locally with **dotnet serve**.

### Publish

From the repo root:

```bash
dotnet publish SerratedJSInterop.Tests.BlazorWasm\SerratedJSInterop.Tests.BlazorWasm.csproj -c Release -o publish\BlazorWasm
dotnet publish SerratedJSInterop.Tests.WasmBrowser\0SerratedJSInterop.Tests.WasmBrowser.csproj -c Release -o publish\WasmBrowser
```

### Install dotnet serve

```bash
dotnet tool install --global dotnet-serve
```

### Serve the published output

Serve the **wwwroot** of each publish so that `index.html` is at the site root. From the repo root:

**Blazor WASM:**

```bash
dotnet serve -d publish\BlazorWasm\wwwroot -p 5100 -o
```

**WasmBrowser** (if that project’s `index.html` is in a `wwwroot` subfolder):

```bash
dotnet serve -d publish\WasmBrowser\wwwroot -p 5101 -o
```

If WasmBrowser puts `index.html` at the publish root, use `-d publish\WasmBrowser` instead.

- `-d` = directory to serve  
- `-p` = port  
- `-o` = open browser when server starts  

Use `-b` to enable Brotli compression on the fly (dotnet serve does not serve the pre-compressed `.br` files from publish).

