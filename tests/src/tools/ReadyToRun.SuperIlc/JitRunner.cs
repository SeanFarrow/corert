﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// No-op runner keeping the original IL assemblies to be directly run with full jitting.
/// </summary>
class JitRunner : CompilerRunner
{
    public override CompilerIndex Index => CompilerIndex.Jit;

    protected override string CompilerFileName => "clrjit.dll";

    public JitRunner(string compilerFolder, string inputFolder, string outputFolder, IReadOnlyList<string> referenceFolders) : base(compilerFolder, inputFolder, outputFolder, referenceFolders) { }

    /// <summary>
    /// JIT runner has no compilation process as it doesn't transform the source IL code in any manner.
    /// </summary>
    /// <returns></returns>
    public override ProcessInfo CompilationProcess(string assemblyFileName)
    {
        File.Copy(assemblyFileName, GetOutputFileName(assemblyFileName));
        return null;
    }

    public override ProcessInfo ExecutionProcess(string appPath, IEnumerable<string> modules, IEnumerable<string> folders, string coreRunPath)
    {
        ProcessInfo processInfo = base.ExecutionProcess(appPath, modules, folders, coreRunPath);
        processInfo.EnvironmentOverrides["COMPLUS_ReadyToRun"] = "0";
        return processInfo;
    }

    protected override IEnumerable<string> BuildCommandLineArguments(string assemblyFileName, string outputFileName)
    {
        // This should never get called as the overridden CompilationProcess returns null
        throw new NotImplementedException();
    }

}
