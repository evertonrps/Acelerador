// Acelerador/Helpers/DirectoryHelper.cs
using System;
using System.IO;

namespace Acelerador;

public static class DirectoryHelper
{
    public static string GetBaseDirectory()
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    }

    public static string GetProjectDirectory(string baseDir, string apiName)
    {
        return Path.Combine(baseDir, apiName, "src");
    }
}
