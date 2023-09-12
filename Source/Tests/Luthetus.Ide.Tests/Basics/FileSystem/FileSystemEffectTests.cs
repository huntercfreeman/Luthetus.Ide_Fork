﻿using Luthetus.Common.RazorLib.FileSystem.Classes.FilePath;
using Xunit;

namespace Luthetus.Ide.Tests.Basics.FileSystem;

public class FileSystemEffectTests : LuthetusFileSystemTestingBase
{
    [Fact]
    public void SaveFile()
    {
        var content = "abc123";

        var absoluteFilePath = new AbsolutePath(
            @"C:\Users\hunte\Desktop\TestLuthetus\apple.txt",
            true,
            EnvironmentProvider);

        var saveFileAction = new ClassLib.Store.FileSystemCase.FileSystemRegistry.SaveFileAction(
            absoluteFilePath,
            content,
            writtenDateTime => { });

        Dispatcher.Dispatch(saveFileAction);
    }
}