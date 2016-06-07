﻿using filemanager.Infrastructure;

namespace filemanager.Domain.Interfaces
{
    public abstract class MyFile
    {
        public MyPath Path { get; protected set; }
        public FileInfo Info { get; protected set; }
        public abstract void Create();
        public abstract void Delete();
        public abstract IFileMoveProcess Move(bool keepOriginal);
    }
}
