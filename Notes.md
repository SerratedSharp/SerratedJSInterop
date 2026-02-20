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

