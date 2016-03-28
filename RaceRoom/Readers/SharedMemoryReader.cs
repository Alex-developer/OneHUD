using System;
using System.Linq;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace RaceRoom.Readers
{
    public class SharedMemoryReader
    {
        private readonly string _filename;
        private MemoryMappedFile _file;
        private MemoryMappedViewStream _stream;
        private int _bufferSize;
        private byte[] _buffer;

        public SharedMemoryReader(string fileName, int bufferSize)
        {
            _filename = fileName;
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];
        }

        public bool Connect()
        {
            try
            {
                Open();
                return true;
            }
            catch (FileNotFoundException ex)
            {
                return false;
            }
            catch
            {
                throw;
            }
        }

        public void Open()
        {
            if (_file == null)
            {
                _file = MemoryMappedFile.OpenExisting(_filename, MemoryMappedFileRights.Read, HandleInheritability.None);
                _stream = this._file.CreateViewStream(0L, 0L, MemoryMappedFileAccess.Read);
            }
        }

        public byte[] Read()
        {
            if (_stream == null)
            {
            }
            else
            {
                _stream.Position = (long)0;
                _stream.Read(_buffer, 0, _bufferSize);
            }
            return _buffer;
        }
    }
}
