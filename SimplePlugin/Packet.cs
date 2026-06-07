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

        public Packet(IncomingPackets id, long capacity = 0, bool dynamicLength = true) : this((byte)id, capacity, dynamicLength) {}
        public Packet(byte id, long capacity = 0, bool dynamicLength = true)
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

        public void PadToPosition(int position)
        {
            while (_ms.Length < position)
            {
                _bw.Write((byte)0);
            }
        }

        public void Write(byte value) => _bw.Write(value);
        public void Write(ushort value) => _bw.Write(value);
        public void WriteBE(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            _bw.Write(bytes);
        }
        public void Write(short value) => _bw.Write(value);
        public void Write(uint value) => _bw.Write(value);
        public void WriteBE(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            _bw.Write(bytes);
        }
        public void Write(int value) => _bw.Write(value);
        public void Write(long value) => _bw.Write(value);
        public void Write(ulong value) => _bw.Write(value);
        public void Write(byte[] buffer) => _bw.Write(buffer);
        public void Write(byte[] buffer, int index, int count) => _bw.Write(buffer, index, count);
        public void WriteAscii(string value, int padding = 0)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bw.Write(Encoding.ASCII.GetBytes(value));
                if (padding > 0 && value.Length < padding)
                {
                    _bw.Write(new byte[padding - value.Length]); // Pad with zeros if needed
                }
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
                
                // This cleanly converts 0x0044 into 0x4400 
                ushort bigEndianLength = BitConverter.IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReverseEndianness((ushort)_ms.Length) : (ushort)_ms.Length;
                
                _bw.Write(bigEndianLength);
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
