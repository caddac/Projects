using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSystem
{

    class threadManager
    {
        internal string _diskName;
        internal string _fileName;
        internal byte[] _file;
        internal string _path;


        /// <summary>
        /// Instanciates a new thread manager object.  
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="fn"></param>
        /// <param name="f"></param>
        /// <param name="p"></param>
        internal threadManager(string dn = null, string fn = null, byte[]  f = null, string p = null)
        {
            _diskName = dn;
            _fileName = fn;
            _file=f;
            _path=p;
        }
        
        /// <summary>
        /// method called by a thread.  Uses the parameters from the objec
        /// </summary>
        internal void _WriteFileToDisk()
        {
            DiskController._WriteFileToDisk(_diskName, _fileName, _file);
        }

        /// <summary>
        /// method called by a thread.  Uses the parameters from the objec
        /// </summary>
        internal void _PutFileToOSFromFS()
        {
            DiskController._PutFileToOSFromFS(_diskName, _fileName, _path);
        }

        /// <summary>
        /// method called by a thread.  Uses the parameters from the objec
        /// </summary>
        internal void _DeleteFileFromDisk()
        {
            DiskController._DeleteFileFromDisk(_diskName, _fileName);
        }

    }

    public static class DiskController
    {
        private static List<Disk> Disks = new List<Disk>();
        
    #region public methods

        #region threaded Methods
        /// <summary>
        /// Creates a new threadManager object, locks the Disk being used, and puts a file to the OS from the FS.  Unlocks the disk when finished.
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        public static void PutFileToOSFromFS(string diskName, string fileName, string path)
        {
            try
            {
                Monitor.Enter(Disks[_GetDiskIndexByName(diskName)]);
                threadManager tws = new threadManager
                {
                    _diskName = diskName,
                    _fileName = fileName,
                    _path = path
                };

                Thread t = new Thread(new ThreadStart(tws._PutFileToOSFromFS));
                t.Start();
                t.Join();
            }
            finally
            {
                Monitor.Exit(Disks[_GetDiskIndexByName(diskName)]);
            }
            //_PutFileToOSFromFS(diskName, fileName, path);
        }

        /// <summary>
        /// Creates a new threadManager object, locks the Disk being used, and writes a file to the disk.  Unlocks the disk when finished.
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        public static void WriteFileToDisk(string diskName, string fileName, byte[] file)
        {
            try
            {
                Monitor.Enter(Disks[_GetDiskIndexByName(diskName)]);
                threadManager tws = new threadManager
                {
                    _diskName = diskName,
                    _fileName = fileName,
                    _file = file
                };

                Thread t = new Thread(new ThreadStart(tws._WriteFileToDisk));
                t.Start();
                t.Join();
            }
            finally
            {
                
                Monitor.Exit(Disks[_GetDiskIndexByName(diskName)]);
            }

            //return _WriteFileToDisk(diskName, fileName, file);
        }

        /// <summary>
        /// Creates a new threadManager object, locks the Disk being used, and deletes a file from the disk.  Unlocks the disk when finished.
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        public static void DeleteFileFromDisk(string diskName, string fileName)
        {
            try
            {
                Monitor.Enter(Disks[_GetDiskIndexByName(diskName)]);
                threadManager tws = new threadManager
                {
                    _diskName = diskName,
                    _fileName = fileName
                };

                Thread t = new Thread(new ThreadStart(tws._DeleteFileFromDisk));
                t.Start();
                t.Join();
            }
            finally
            {
                Monitor.Exit(Disks[_GetDiskIndexByName(diskName)]);
            }
            // _DeleteFileFromDisk(diskName, fileName);
        }



        #endregion

        #region unthreaded methods

        /// <summary>
        /// calls the private method to return all mounted disk names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMountedDiskNames()
        {
            return _GetMountedDiskNames();
        }

        /// <summary>
        /// calls the private method to get all files on a disk as strings
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns></returns>
        public static List<string> GetListFilesOnDiskAsStrings(string diskName)
        {
            return _GetListFilesOnDiskAsStrings(diskName);
        }

        /// <summary>
        /// calls the private method to get all mounted disks as objects
        /// </summary>
        /// <returns></returns>
        public static List<Disk> GetMountedDisksAsObjects()
        {
            return _GetMountedDisksAsObjects();
        }

        /// <summary>
        /// calls the private method to get a disk's index
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetDiskIndexByName(string name)
        {
            return _GetDiskIndexByName(name);
        }

        /// <summary>
        /// calls the private method to get a disk by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Disk GetDiskObjectByName(string name)
        {
            return _GetDiskObjectByName(name);
        }


        /// <summary>
        /// calls the private method to mount an existing disk.  Unthreaded because it is a diskcontroller level method
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool MountExistingDiskToFS(string path)
        {
            return _MountExistingDiskToFS(path);
        }

        /// <summary>
        /// Calls the private method to unmount a disk.
        /// </summary>
        /// <param name="diskName"></param>
        public static void UnMountDiskFromFSByName(string diskName)
        {
            _UnMountDiskFromFSByName(diskName);
        }

        /// <summary>
        /// calls the private method to save a filesystem to the OS
        /// </summary>
        /// <param name="path"></param>
        /// <param name="diskName"></param>
        /// <returns></returns>
        public static string SaveFsToOS(string path, string diskName)
        {
            return _SaveFsToOS(path, diskName);
        }

        /// <summary>
        /// callls the private method to save a fs and unmount
        /// </summary>
        /// <param name="path"></param>
        /// <param name="diskName"></param>
        /// <returns></returns>
        public static string SaveFsAndUnMount(string path, string diskName)
        {
            return _SaveFsAndUnMount(path, diskName);
        }

        /// <summary>
        /// call the private method to create a new disk
        /// </summary>
        /// <param name="size"></param>
        /// <param name="name"></param>
        public static void CreateDisk(int size, string name)
        {
            _CreateDisk(size, name);
        }
        
        #endregion

    #endregion

        #region private methods
        /// <summary>
        /// mounts the disk at path
        /// </summary>
        /// <param name="path"></param>
        internal static bool _MountExistingDiskToFS(string path)
        {
            string[] parts = path.Split('\\');
            List<string> partslist = parts.ToList();
            string name = partslist[partslist.Count - 1];
            if (_GetMountedDiskNames().Contains(name))
                return false;

            Disks.Add(Disk.DeSerializeDiskFromPath(path));
            return true;
        }

        /// <summary>
        /// writes the file "fileName" from disk "diskName" to the folder path
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        internal static void _PutFileToOSFromFS(string diskName, string fileName, string path)
        {
            byte[] file = Disks[_GetDiskIndexByName(diskName)].ReadFile(fileName);

            File.WriteAllBytes(path + "\\" + fileName, file);
            

        }

        /// <summary>
        /// deletes the file "fileName" from the disk "diskName"
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        internal static void _DeleteFileFromDisk(string diskName, string fileName)
        {
            Disks[_GetDiskIndexByName(diskName)].DeleteFile(fileName);
        }

        /// <summary>
        /// writes a file "file" named "fileName" to the disk "diskName"
        /// </summary>
        /// <param name="diskName"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <returns>Whether file will fit on the disk.</returns>
        internal static bool _WriteFileToDisk(string diskName, string fileName, byte[] file)
        {
            if (fileName.Length > 16)
                return false;
            if (!Disks[_GetDiskIndexByName(diskName)].FileFitOnDisk(file))
                return false;
            Disks[_GetDiskIndexByName(diskName)].WriteFileToDisk(file, fileName);
            return true;
        }

        /// <summary>
        /// gets all the file names on the disk "diskName"
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns></returns>
        private static List<string> _GetListFilesOnDiskAsStrings(string diskName)
        {
            if (string.IsNullOrEmpty(diskName))
                return new List<string>();
            return Disks[GetDiskIndexByName(diskName)].GetFileNames();
        }


        /// <summary>
        /// unmounts a disk. Does not save the disk first.
        /// </summary>
        /// <param name="diskName"></param>
        private static void _UnMountDiskFromFSByName(string diskName)
        {
            Disks.Remove(GetDiskObjectByName(diskName));
        }

        /// <summary>
        /// writes the disk "diskName" to the folder at path.  
        /// </summary>
        /// <param name="path"></param>
        /// <param name="diskName"></param>
        /// <returns></returns>
        private static string _SaveFsToOS(string path, string diskName)
        {
            return Disks[GetDiskIndexByName(diskName)].SerializeDiskToPath(path);
        }

        /// <summary>
        /// writes the disk "diskName" to the folder at path.  Unmounts the disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string _SaveFsAndUnMount(string path, string diskName)
        {
            try
            {
                return Disks[_GetDiskIndexByName(diskName)].SerializeDiskToPath(path);
            }
            finally
            {
                _UnMountDiskFromFSByName(diskName);
            }
        }

        /// <summary>
        /// get disk "name"s index in the mounted disk list
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns></returns>
        private static int _GetDiskIndexByName(string diskName)
        {
            return Disks.FindIndex(d => d.diskName.Equals(diskName));
        }

        /// <summary>
        /// Get a Disk object by disk name
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns></returns>
        internal static Disk _GetDiskObjectByName(string diskName)
        {
            return Disks.SingleOrDefault(d => d.diskName.ToLower().Equals(diskName.ToLower()));
        }

        /// <summary>
        /// returns the list of mounted disk names
        /// </summary>
        /// <returns></returns>
        private static List<string> _GetMountedDiskNames()
        {
            return Disks.Where(d => d != null).Select(d => d.diskName).ToList();
        }

        /// <summary>
        /// Get the entire list of Disks
        /// </summary>
        /// <returns></returns>
        private static List<Disk> _GetMountedDisksAsObjects()
        {
            return Disks;
        }

        /// <summary>
        /// Creates a new Disk and adds to list of mounted disks
        /// </summary>
        /// <param name="size"></param>
        /// <param name="diskName"></param>
        private static void _CreateDisk(int size, string diskName)
        {
            Disks.Add(new Disk(size, diskName));
        }
#endregion

    }



    [Serializable]
    public class Disk
    {
        #region attributes
        private string _diskName;
        public string diskName
        {
            get
            {
                return _diskName;
            }

        }
        internal int diskSize;
        internal FNT fileNameTable;
        internal ABPT attributeBlockPointerTable;
        internal DataBlocks dataBlocks;
        #endregion

        internal Disk(int numBlocks, string diskName)
        {
            this._diskName = diskName;
            this.diskSize = numBlocks;

            //sizes of abpt and fnt can be changed here easily
            int abptSize = (numBlocks / 8) + 1;
            int fntSize = (numBlocks / 16) + 1;

            this.dataBlocks = new DataBlocks(numBlocks);
            this.attributeBlockPointerTable = new ABPT(abptSize);
            this.fileNameTable = new FNT(fntSize);
        }

        /// <summary>
        /// gets the list of file names on this disk
        /// </summary>
        /// <returns></returns>
        internal List<string> GetFileNames()
        {
            return this.fileNameTable.GetFileNames();
        }

        /// <summary>
        /// reads a disk from a file and returns the disk as an object
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Disk DeSerializeDiskFromPath(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            Disk obj = (Disk)formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }

        /// <summary>
        /// serializes this object to the path given, naming the file with the disks name
        /// </summary>
        /// <param name="path"></param>
        internal string SerializeDiskToPath(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path + "\\" + this.diskName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();

            return path + "\\" + this.diskName;
        }

        /// <summary>
        /// writes a new file to the disk.  Does not verify available space on disk
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        internal void WriteFileToDisk(byte[] file, string fileName)
        {
            this.WriteFileToFNT(fileName, this.WriteFileToABPT(this.WriteBlocks(file)));
        }

        /// <summary>
        /// deletes the file from the FNT, ABPT and datablocks where FNT pointer is index
        /// </summary>
        /// <param name="index"></param>
        internal void DeleteFile(string fileName)
        {
            int index = fileNameTable.GetEntryIndexByName(fileName);
            int FNThead = fileNameTable.GetAbptPointer(fileName);
            List<int> abtpEntries = new List<int> { FNThead };
            List<int> blockEntries = new List<int>();
            
            while (attributeBlockPointerTable.GetExtentPointer(abtpEntries.Last()) != -1)
            {
                abtpEntries.Add(attributeBlockPointerTable.GetExtentPointer(abtpEntries.Last()));
            }

            foreach (int i in abtpEntries)
            {
                blockEntries.AddRange(attributeBlockPointerTable.GetBlockPointers(i));
                attributeBlockPointerTable.ClearEntry(i);
            }
            foreach (int i in blockEntries)
            {
                dataBlocks.clearBlock(i);
            }
            fileNameTable.ClearEntry(index);


        }

        /// <summary>
        /// gets the file "fileName" as a byte[]
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal byte[] ReadFile(string fileName)
        {
            int index = fileNameTable.GetEntryIndexByName(fileName);
            int FNThead = fileNameTable.GetAbptPointer(fileName);
            List<int> abtpEntries = new List<int> { FNThead };
            List<int> blockEntries = new List<int>();

            while (attributeBlockPointerTable.GetExtentPointer(abtpEntries.Last()) != -1)
            {
                abtpEntries.Add(attributeBlockPointerTable.GetExtentPointer(abtpEntries.Last()));
            }
            foreach (int i in abtpEntries)
            {
                blockEntries.AddRange(attributeBlockPointerTable.GetBlockPointers(i));
            }

            return this.dataBlocks.ReadBlocks(blockEntries);
        }


        /// <summary>
        /// writes a new file name entry to the FNT.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abptHead"></param>
        /// <returns></returns>
        internal int WriteFileToFNT(string name, int abptHead)
        {
            int index = fileNameTable.GetNextEmptyEntry();

            fileNameTable.WriteEntry(index, name, abptHead);
            return index;
        }

        /// <summary>
        /// writes all abpt entries needed and returns the first entry.
        /// </summary>
        /// <param name="blockIndexes"></param>
        /// <returns></returns>
        internal int WriteFileToABPT(List<int> blockIndexes)
        {
            int extra = blockIndexes.Count % 8;
            while (!(extra == 0))
            {
                blockIndexes.Add(-1);
                extra = blockIndexes.Count % 8;
            }

            int numEntriesNeeded = (int) Math.Ceiling((float)(blockIndexes.Count/8));
            int  firstEntry = -1 , i = -1 , last;

            for (int a = 0; a < numEntriesNeeded; a++)
            {
                last = i;
                i = attributeBlockPointerTable.GetNextEmptyEntry();

                if (a == 0)
                    firstEntry = i;
                else
                    attributeBlockPointerTable.updateExtentPntr(last, i);
 
                attributeBlockPointerTable.WriteEntry(i, blockIndexes.Count, DateTime.Now, blockIndexes[0 + (a * 8)],
                                                                                            blockIndexes[1 + (a * 8)],
                                                                                            blockIndexes[2 + (a * 8)],
                                                                                            blockIndexes[3 + (a * 8)],
                                                                                            blockIndexes[4 + (a * 8)],
                                                                                            blockIndexes[5 + (a * 8)],
                                                                                            blockIndexes[6 + (a * 8)],
                                                                                            blockIndexes[7 + (a * 8)]);
            }
            return firstEntry;
        }

        /// <summary>
        /// writes a byte[] to the blocks, writes to the first avaialble free blocks (not contiguous).  
        /// </summary>
        /// <param name="file"></param>
        /// <returns>the indexes of the blocks in order</returns>
        internal List<int> WriteBlocks(byte[] file)
        {
            byte[] temp = new byte[256];
            List<int> writeLocations = new List<int>();
            int i;
            foreach (var block in chunk.Chunk(file, 256))
            {
                temp = new List<byte>(block).ToArray();
                i = dataBlocks.FindNextOpenBlock();
                dataBlocks.WriteBlock(i, temp);
                writeLocations.Add(i);
            }
            return writeLocations;
        }

        /// <summary>
        /// determines if there are enough free blocks, free ABPT entries, and free FNT entries for a new file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal bool FileFitOnDisk(byte[] file)
        {
            double numBlocks = file.Length / 256;
            double numABPTs = numBlocks/8;
            
            int fileSize =(int) Math.Ceiling(numBlocks);
            int abptSize = (int)Math.Ceiling(numABPTs);

            if (this.dataBlocks.GetNumberFreeBlock() >= numBlocks)
                if (this.attributeBlockPointerTable.GetNumberBlankEntries() >= numABPTs)
                    if (this.fileNameTable.GetNumberBlankEntries() >= 1)
                        return true;
            return false;
        }
    }



    [Serializable]
    public class FNT
    {
        private List<entry> table;

        public FNT(int size)
        {
            this.table = new List<entry>(size);
            for (int i = 0; i < size; i++)
                table.Insert(i, null);
        }

        /// <summary>
        /// returns all the names of the files in the table
        /// </summary>
        /// <returns></returns>
        internal List<string> GetFileNames()
        {
            return table.Where(e => e != null).Select(e => e.GetFileName()).ToList();            
        }

        /// <summary>
        /// does nothing yet, not sure how I want to read an entry
        /// </summary>
        /// <param name="entryIndex"></param>
        internal entry GetEntry(int entryIndex)
        {
            if (entryIndex > table.Capacity) 
                return null;
            return table[entryIndex];

        }

        /// <summary>
        /// returns the pointer to the ABPT table
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal int GetAbptPointer(string fileName)
        {
            return table[GetEntryIndexByName(fileName)].GetPointer();
        }

        /// <summary>
        /// gets the table index of the filename entry "fileName"
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal int GetEntryIndexByName(string fileName)
        {

            return table.FindIndex(e => (e == null) ? false : e.GetFileName().Equals(fileName)); //Where(e=>e!= null).ToList().FindIndex(e => e.GetFileName().Equals(fileName));
        }

        /// <summary>
        /// returns the number of free entries in the table
        /// </summary>
        /// <returns></returns>
        internal int GetNumberBlankEntries()
        {
            return table.Count(e => e == null);
        }

        /// <summary>
        /// returns the index of the next free entry
        /// </summary>
        /// <returns></returns>
        internal int GetNextEmptyEntry()
        {
            return table.FindIndex(e => e == null);
        }

        /// <summary>
        /// set the entry at entryIndex to null
        /// </summary>
        /// <param name="entryIndex"></param>
        /// <returns></returns>
        internal bool ClearEntry(int entryIndex)
        {
            if (entryIndex > table.Capacity)
                return false;
            table[entryIndex] = null;
            return true;
        }

        /// <summary>
        /// writes a new entry at entryIndex
        /// </summary>
        /// <param name="entryIndex"></param>
        /// <param name="fileName"></param>
        /// <param name="ptr"></param>
        internal void WriteEntry(int entryIndex, string fileName, int ptr = -1)
        {
            if (table.ElementAtOrDefault(entryIndex) == null)
                FormatEntry(entryIndex);
            table[entryIndex] = new entry(fileName, ptr);
        }

        /// <summary>
        /// formats an entry with all entries and pointers null;
        /// </summary>
        /// <param name="entryIndex"></param>
        internal bool FormatEntry(int entryIndex)
        {
            if (entryIndex > table.Capacity)
                return false;
            table[entryIndex] = null;
            return true;
        }

        [Serializable]
        internal class entry
        {
            public entry()
            {
            }

            public entry(string fileName, int ptr)
            {
                if (fileName.Length > 16)
                    fileName = fileName.Substring(fileName.Length - 16, 16);
                this.fileName=fileName;
                this.ptr=ptr;
            }

            string fileName { get; set; }
            int ptr { get; set; }

            public int GetPointer() { return ptr; }
            internal string GetFileName() { return fileName; }
        }
    }



    [Serializable]
    public class ABPT 
    {
        private List<entry> table;

        internal ABPT(int size)
        {
            this.table = new List<entry>(size);
            for (int i = 0; i < size; i++)
                table.Insert(i, null);
        }

        /// <summary>
        /// gets the ext pointer for the entry at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal int GetExtentPointer(int index)
        {
            return table[index].GetExtPtr();
        }

        /// <summary>
        /// gets the block pointers for the entry at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal List<int> GetBlockPointers(int index)
        {
            return table[index].GetPointers();
        }

        /// <summary>
        /// updates the extent ptr at index to extPtr
        /// </summary>
        /// <param name="index"></param>
        /// <param name="extPtr"></param>
        
        internal void updateExtentPntr(int index, int extPtr)
        {
            table[index].updateExtent(extPtr);
        }

        ///// <summary>
        ///// does nothing yet, not sure how I want to read an entry
        ///// </summary>
        ///// <param name="entryIndex"></param>
        //internal entry GetEntry(int entryIndex)
        //{
        //    if (entryIndex > table.Capacity) 
        //        return null;
        //    return table[entryIndex];

        //}

        /// <summary>
        /// returns the number of free entries in the table
        /// </summary>
        /// <returns></returns>
        internal int GetNumberBlankEntries()
        {
            return table.Count(e => e == null);
        }

        /// <summary>
        /// returns the index of the next free entry
        /// </summary>
        /// <returns></returns>
        internal int GetNextEmptyEntry()
        {
            return table.FindIndex(e => e == null);
        }

        /// <summary>
        /// set the entry at entryIndex to null
        /// </summary>
        /// <param name="entryIndex"></param>
        /// <returns></returns>
        internal bool ClearEntry(int entryIndex)
        {
            if (entryIndex > table.Capacity)
                return false;
            table[entryIndex] = null;
            return true;
        }

        /// <summary>
        /// writes a new entry at index entryIndex.
        /// </summary>
        /// <param name="entryIndex"></param>
        /// <param name="fileSize"></param>
        /// <param name="dateTime"></param>
        /// <param name="ptr1"></param>
        /// <param name="ptr2"></param>
        /// <param name="ptr3"></param>
        /// <param name="ptr4"></param>
        /// <param name="ptr5"></param>
        /// <param name="ptr6"></param>
        /// <param name="ptr7"></param>
        /// <param name="ptr8"></param>
        /// <param name="ptrExt"></param>
        internal void WriteEntry(int entryIndex, int fileSize, DateTime dateTime, int ptr1 = -1,
                                                         int ptr2 = -1,
                                                         int ptr3 = -1,
                                                         int ptr4 = -1,
                                                         int ptr5 = -1,
                                                         int ptr6 = -1,
                                                         int ptr7 = -1,
                                                         int ptr8 = -1,
                                                         int ptrExt = -1)
        {
            if (table.ElementAtOrDefault(entryIndex) == null)
                FormatEntry(entryIndex);
            table[entryIndex] = new entry(fileSize, dateTime, ptr1, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, ptrExt);
        }

        /// <summary>
        /// formats an entry with all entries and pointers null;
        /// </summary>
        /// <param name="entryIndex"></param>
        internal bool FormatEntry(int entryIndex)
        {
            if (entryIndex > table.Capacity)
                return false;
            table[entryIndex] = null;
            return true;
        }
        [Serializable]
        class entry
        {
            private int fileSize;
            private DateTime dateTime;
            private int ptr1;
            private int ptr2;
            private int ptr3;
            private int ptr4;
            private int ptr5;
            private int ptr6;
            private int ptr7;
            private int ptr8;
            private int ptrExt;

            public entry()
            {
            }

            public entry(
                        int fileSize, 
                        DateTime dateTime, 
                        int ptr1 = -1,
                        int ptr2 = -1,
                        int ptr3 = -1,
                        int ptr4 = -1,
                        int ptr5 = -1,
                        int ptr6 = -1,
                        int ptr7 = -1,
                        int ptr8 = -1,
                        int ptrExt = -1
                         )
            {
                this.fileSize=fileSize;
                this.dateTime=dateTime;
                this.ptr1=ptr1;
                this.ptr2=ptr2;
                this.ptr3=ptr3;
                this.ptr4=ptr4;
                this.ptr5=ptr5;
                this.ptr6=ptr6;
                this.ptr7=ptr7;
                this.ptr8=ptr8;
                this.ptrExt=ptrExt;
            }

            public void updateExtent(int ext) { this.ptrExt = ext; }
            public int GetExtPtr() { return ptrExt; }
            public List<int> GetPointers() { return new List<int> { ptr1, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8 }; }
        }

    }



    [Serializable]
    public class DataBlocks  
    {
        static readonly int blockSizeInBytes = 256;
        private List<byte[]> blocks;

        /// <summary>
        /// Creates a new set of datablocks
        /// </summary>
        /// <param name="numBlocks"></param>
        internal DataBlocks(int numBlocks)
        {
            this.blocks = new List<byte[]>(numBlocks);
            for (int i = 0; i < numBlocks; i++)
                blocks.Insert(i, null);
        }

        /// <summary>
        /// returns the index of the next free block
        /// </summary>
        internal int FindNextOpenBlock()
        {
            return blocks.FindIndex(b => b == null);
        }

        /// <summary>
        /// returns the number of free (unformatted) blocks
        /// </summary>
        /// <returns></returns>
        internal int GetNumberFreeBlock()
        {
            return blocks.Count(b => b == null);
        }

        /// <summary>
        /// Deletes the data at blockIndex. Unformats the block.
        /// </summary>
        /// <param name="blockIndex">index of block to clear</param>
        internal bool clearBlock(int blockIndex)
        {
            if (blockIndex > blocks.Capacity || blockIndex < 0)
                return false;
            blocks[blockIndex] = null;
            return true;
        }

        /// <summary>
        /// Formats the block at blockIndex.
        /// </summary>
        /// <param name="blockIndex">index of block to format</param>
        internal bool FormatBlock(int blockIndex)
        {
            if (blockIndex > blocks.Capacity)
                return false;
            blocks[blockIndex] = new byte[blockSizeInBytes];
            return true;
        }

        /// <summary>
        /// writes data to the block at blockIndex.  Formats the block if it is not formatted.
        /// </summary>
        /// <param name="blockIndex">the block index to write to</param>
        /// <param name="data">blockSize or less bytes of data</param>
        internal bool WriteBlock(int blockIndex, byte[] data)
        {
            if (blocks.ElementAtOrDefault(blockIndex) == null)
                FormatBlock(blockIndex);
            if (data.Length > blockSizeInBytes)
                return false;
            blocks[blockIndex] = data;
            return true;
        }

        /// <summary>
        /// Returns all the blocks of a file
        /// </summary>
        /// <param name="locs">the indexes of all the blocks of a file</param>
        /// <returns></returns>
        internal byte[] ReadBlocks(List<int> locs)
        {
            List<byte> file = new List<byte>();
            foreach (int i in locs)
            {
                if (i < 0) break;
                file.AddRange(blocks[i]);
            }
            return file.ToArray();
        }
    }



    /// <summary>
    /// helper class that chunks a list into pieces
    /// </summary>
    internal static class chunk
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int chunkSize)
        {
            int i = 0;
            var chunks = from name in list
                         group name by i++ / chunkSize into part
                         select part.AsEnumerable();
            return chunks;
        }
    }
}
