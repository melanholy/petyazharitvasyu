﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using filemanager.Domain.Interfaces;
using filemanager.Infrastructure;

namespace filemanager.Domain.Windows
{
    public class WinFolder : Folder
    {
        public WinFolder(MyPath path)
        {
            var info = new DirectoryInfo(path.PathStr);
            Info = new FileInfo(
                new FileSize(FileSize.DirSize), 
                info.CreationTime, info.LastWriteTime
            );
            Path = path;
        }

        public override IEnumerable<MyFile> EnumerateFiles()
        {
            var files = Directory.EnumerateFiles(Path.PathStr);
            var dirs = Directory.EnumerateDirectories(Path.PathStr);
            return files
                .Select(x => (MyFile)new WinFile(new MyPath(x)))
                .Union(dirs.Select(x => new WinFolder(new MyPath(x))));
        }

        public override void Create()
        {
            if (Directory.Exists(Path.PathStr))
                throw new FileAlreadyExistException();

            Directory.CreateDirectory(Path.PathStr);
        }

        public override void Delete()
        {
            if (!Directory.Exists(Path.PathStr))
                throw new FileNotFoundException();

            Directory.Delete(Path.PathStr, true);
        }

        public override IFileMoveProcess Move(bool keepOriginal)
        {
            if (!Directory.Exists(Path.PathStr))
                throw new FileNotFoundException();

            return new WinFileMoveProcess(this, keepOriginal);
        }

        public static IEnumerable<WinFolder> GetRootFolders()
        {
            return DriveInfo.GetDrives()
                .Where(x => x.DriveType == DriveType.Fixed)
                .Select(x => new WinFolder(new MyPath(x.Name)));
        }
    }
}