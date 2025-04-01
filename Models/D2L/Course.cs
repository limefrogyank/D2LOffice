using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace D2LOffice.Models.D2L
{
    public enum FileSystemObjectType
    {
        Folder = 1,
        File = 2
    }

    public class CreateFolder
    {
        public required string RelativePath { get; set; }
    }
    public class CreatedFolder
{
        public required string RelativePath { get; set; }
        public required string FolderName { get; set; }
        public required string LastModifiedBy { get; set; }
        public required string LastModifiedDate { get; set; }
    }
    public class FileSystemObject
{
        public required string Name { get; set; }
        public FileSystemObjectType FileSystemObjectType { get; set; }
    }
    public class UploadedFile
    {
        public required string RelativePath { get; set; }
        public required string FileName { get; set; }
        public int Size { get; set; }
        public required int LastModifiedBy { get; set; }
        public required string LastModifiedDate { get; set; }
    }
}
