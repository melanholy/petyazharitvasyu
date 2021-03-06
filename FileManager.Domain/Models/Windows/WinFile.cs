﻿using System.IO;
using FileManager.Domain.Infrastructure;
using FileManager.Domain.Models.Files;
using FileInfo = FileManager.Domain.Models.Files.FileInfo;

namespace FileManager.Domain.Models.Windows
{
    public class WinFile : TextFile
    {
        public WinFile(MyPath path)
        {
            Path = path;
            Info = GetFileInfo();
        }

        private FileInfo GetFileInfo()
        {
            var info = new System.IO.FileInfo(Path.PathStr);
            return new FileInfo(
                new FileSize(info.Length),
                info.CreationTime, info.LastWriteTime
            );
        }
        
        public override void Create(Stream contents)
        {
            Create();

            using (var s = File.OpenWrite(Path.PathStr))
                contents.CopyTo(s);
        }

        public override void Create()
        {
            if (Exists())
                throw new FileAlreadyExistException();

            File.Create(Path.PathStr);
        }

        public override void Delete()
        {
            if (!Exists())
                throw new FileNotFoundException();

            File.Delete(Path.PathStr);
        }

        public override bool Exists()
        {
            return File.Exists(Path.PathStr);
        }
    }
}
