using System;
using System.Linq;
using System.Text;

namespace Assistant
{
    public static class Challenge
    {
        // Constants
        private static readonly ulong C1 = 0x8ebc6af1UL;
        private static readonly ulong C2 = 0xe7037ed1UL;
        private static readonly ulong C3 = 0x1d8e4e27UL;
        private static readonly ulong C4 = 0x589965cdUL;
        private static readonly ulong C5 = 0xa0761d65UL;
        private static readonly ulong C6 = 0xeb44accbUL;

        private static long Rand()
        {
            // Equivalent of sys.maxsize in C# is long.MaxValue
            return new Random().Next(0, int.MaxValue);
        }

        // Extracts the challenge token from the packet (bytes 1..4, big-endian)
        private static uint GetToken(byte[] packet)
        {
            if (packet.Length < 5)
                throw new ArgumentException("Packet too short");
            return (uint)((packet[1] << 24) | (packet[2] << 16) | (packet[3] << 8) | packet[4]);
        }

        // Hash function: waterhash
        private static uint Waterhash(byte[] data, ulong param3 = 0xc3a5c85c97cb3127UL)
        {
            int length = data.Length;
            int pos = 0;
            ulong v = param3;

            // Main loop (16-byte blocks)
            while (length - pos >= 16)
            {
                ulong w0 = BitConverter.ToUInt32(data, pos + 0);
                ulong w1 = BitConverter.ToUInt32(data, pos + 4);
                ulong w2 = BitConverter.ToUInt32(data, pos + 8);
                ulong w3 = BitConverter.ToUInt32(data, pos + 12);

                ulong a = ((w3 ^ C3) * (w2 ^ C4));
                ulong b = ((w1 ^ C1) * (w0 ^ C2));
                b = v + (b - (b >> 32));
                b = b * (a - (a >> 32));
                v = b - (b >> 32);
                pos += 16;
            }

            ulong res = v + C6;
            int n = length - pos;
            ulong t1 = 0, t2 = 0;
            byte[] tail = data.Skip(pos).ToArray();

            ulong rd32(int offset = 0)
            {
                byte[] buf = new byte[4];
                for (int i = 0; i < 4 && (offset + i) < tail.Length; i++)
                    buf[i] = tail[offset + i];
                return BitConverter.ToUInt32(buf, 0);
            }
            ulong rd16(int offset = 0)
            {
                byte[] buf = new byte[2];
                for (int i = 0; i < 2 && (offset + i) < tail.Length; i++)
                    buf[i] = tail[offset + i];
                return BitConverter.ToUInt16(buf, 0);
            }
            ulong rd8(int offset = 0)
            {
                return (offset < tail.Length) ? (ulong)tail[offset] : 0UL;
            }

            if (n == 0) { }
            else if (n == 1)
            {
                t1 = rd8() ^ C2;
                res ^= C1;
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 2)
            {
                t1 = rd16() ^ C3;
                res ^= C4;
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 3)
            {
                t1 = rd8(2) ^ C1;
                ulong val = rd32() & 0xFFFF;
                res ^= val;
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 4)
            {
                t1 = rd16(2) ^ C4;
                ulong val = rd32() & 0xFFFF;
                res ^= val;
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 5)
            {
                res = (res ^ rd32()) * (rd8(4) ^ C2);
                res = res - (res >> 32);
            }
            else if (n == 6)
            {
                res = (res ^ rd32()) * (rd16(4) ^ C2);
                res = res - (res >> 32);
            }
            else if (n == 7)
            {
                t1 = ((rd16(4) << 8) | rd8(6)) ^ C2;
                res ^= rd32();
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 8)
            {
                t1 = rd32(4) ^ C5;
                res ^= rd32();
                res = t1 * res - ((t1 * res) >> 32);
            }
            else if (n == 9)
            {
                t2 = rd8(8) ^ C4;
                t1 = res ^ C3;
                t2 = t2 * t1;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 10)
            {
                t2 = (rd16(8) ^ C4) * res;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 11)
            {
                t2 = ((rd16(8) << 8) | rd8(10)) ^ C4;
                t2 = t2 * res;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 12)
            {
                t2 = (rd32(8) ^ res) * C3;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 13)
            {
                t2 = rd32(8) ^ res;
                t1 = rd8(12) ^ C3;
                t2 = t2 * t1;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 14)
            {
                t2 = rd32(8) ^ res;
                t1 = rd16(12) ^ C3;
                t2 = t2 * t1;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }
            else if (n == 15)
            {
                t2 = ((rd16(12) << 8) | rd8(14)) ^ C3;
                t1 = rd32(8) ^ res;
                t2 = t2 * t1;
                t2 = t2 - (t2 >> 32);
                res = ((rd32(4) ^ C1) * (rd32() ^ res));
                res = (res - (res >> 32)) ^ t2;
            }

            // Final mixing
            ulong finalMix = ((res << 16) ^ res) * (((ulong)length ^ C5));
            finalMix = finalMix - (finalMix >> 32);

            return (uint)(finalMix & 0xFFFFFFFFUL);
        }

        // Builds the packet as "D5 XX XX XX XX XX XX XX XX"
        private static string BuildPacket(uint hash2, uint hash1)
        {
            var packet = new byte[9];
            packet[0] = 0xD5;
            Array.Copy(BitConverter.GetBytes(hash2).Reverse().ToArray(), 0, packet, 1, 4);
            Array.Copy(BitConverter.GetBytes(hash1).Reverse().ToArray(), 0, packet, 5, 4);
            return string.Join(" ", packet.Select(b => b.ToString("X2")));
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string SolveChallenge(string packetHex, string version)
        {
            string packetHexNormalized = RemoveWhitespace(packetHex);
            var challengeBytes = Enumerable.Range(0, packetHexNormalized.Length / 2)
                .Select(i => Convert.ToByte(packetHexNormalized.Substring(i * 2, 2), 16)).ToArray();

            uint token = GetToken(challengeBytes);

            var salt = Rand();
            string string1 = $"{salt}client.exe_{version}";
            uint hash1 = Waterhash(Encoding.ASCII.GetBytes(string1));
            string string2 = $"{hash1}{token}";
            uint hash2 = Waterhash(Encoding.ASCII.GetBytes(string2));
            string builtPacket = BuildPacket(hash2, hash1);
            return builtPacket;
        }

        // Example: Finds the right random value for a given target packet
        public static void BruteForceChallenge(string packetHex, string targetPacketHex, string version)
        {
            string packetHexNormalized = RemoveWhitespace(packetHex);
            var challengeBytes = Enumerable.Range(0, packetHexNormalized.Length / 2)
                .Select(i => Convert.ToByte(packetHexNormalized.Substring(i * 2, 2), 16)).ToArray();

            uint token = GetToken(challengeBytes);

            for (int i = 0; i < 32767; i++)
            {
                string string1 = $"{i}client.exe_{version}";
                uint hash1 = Waterhash(Encoding.ASCII.GetBytes(string1));
                string string2 = $"{hash1}{token}";
                uint hash2 = Waterhash(Encoding.ASCII.GetBytes(string2));
                string builtPacket = BuildPacket(hash2, hash1);
                if (builtPacket == targetPacketHex)
                {
                    Console.WriteLine($"Found random number: {i}");
                    Console.WriteLine($"string1: {string1}");
                    Console.WriteLine($"hash1: {hash1}");
                    Console.WriteLine($"string2: {string2}");
                    Console.WriteLine($"hash2: {hash2}");
                    Console.WriteLine($"packet: {builtPacket}");
                    break;
                }
            }
        }
    }
}
