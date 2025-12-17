using System.Text;

namespace Assistant
{
    public class Packet : IDisposable
    {
        private MemoryStream _ms;
        private BinaryWriter _bw;
        private bool _dynamicLength;
        public long Length => _ms.Length;
        public byte[] Buffer => _ms.ToArray();

        public Packet(byte id, long capacity = 0, bool dynamicLength = false)
        {
            _dynamicLength = dynamicLength;
            _ms = capacity > 0 ? new MemoryStream((int)capacity) : new MemoryStream();
            _bw = new BinaryWriter(_ms);
            _bw.Write(id);
            if (_dynamicLength)
            {
                _bw.Write((ushort)0); // Length placeholder
            }
        }

        public Packet(byte[] data, int length, bool dynamicLength)
        {
            _dynamicLength = dynamicLength;
            _ms = new MemoryStream();
            _bw = new BinaryWriter(_ms);
            _bw.Write(data, 0, length);
            if (_dynamicLength)
            {
                _ms.Position = 1;
                _bw.Write((ushort)length);
                _ms.Position = length;
            }
        }

        public void Write(byte value) => _bw.Write(value);
        public void Write(ushort value) => _bw.Write(value);
        public void Write(short value) => _bw.Write(value);
        public void Write(uint value) => _bw.Write(value);
        public void Write(int value) => _bw.Write(value);
        public void Write(long value) => _bw.Write(value);
        public void Write(ulong value) => _bw.Write(value);
        public void Write(byte[] buffer) => _bw.Write(buffer);
        public void Write(byte[] buffer, int index, int count) => _bw.Write(buffer, index, count);
        public void WriteAscii(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bw.Write(Encoding.ASCII.GetBytes(value));
            }
        }
        public void WriteUnicode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bw.Write(Encoding.Unicode.GetBytes(value));
            }
        }

        public byte[] Compile()
        {
            if (_dynamicLength)
            {
                _ms.Position = 1;
                _bw.Write((ushort)_ms.Length);
            }
            return _ms.ToArray();
        }

        public void Dispose()
        {
            _bw?.Dispose();
            _ms?.Dispose();
        }
    }
}
