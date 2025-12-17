using System.Runtime.InteropServices;
using CUO_API;

namespace Assistant;

public class Engine
{
    private static OnPacketSendRecv _sendToServer, _recv, _send;

    private static string m_ClientVersion;

    public Engine()
    {
    }

    public static unsafe void Install(PluginHeader* header)
    {
        Console.WriteLine("OSI Challenger plugin loaded.");
        _sendToServer =
            (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer(header->Send, typeof(OnPacketSendRecv));

        m_ClientVersion = new Version((byte)(header->ClientVersion >> 24), (byte)(header->ClientVersion >> 16),
            (byte)(header->ClientVersion >> 8), (byte)header->ClientVersion).ToString();

        _recv = OnRecv;

        header->OnRecv = Marshal.GetFunctionPointerForDelegate(_recv);
    }


    private static unsafe bool OnRecv(ref byte[] data, ref int length)
    {
        fixed (byte* ptr = data)
        {
            bool result = true;
            byte id = data[0];

            if (id != 0xD5) return true;

            // Log for debug:
            Console.WriteLine("Received D5: " + BitConverter.ToString(data));

            // Call your challenge solver (adapt as needed)
            string responseHex =
                Challenge.SolveChallenge(BitConverter.ToString(data).Replace("-", " "), m_ClientVersion);
            Console.WriteLine("Challenge Response: " + responseHex);

            // Convert responseHex ("D5 XX XX XX XX XX XX XX XX") to byte array:
            byte[] responseBytes = responseHex
                .Split(' ')
                .Select(hex => Convert.ToByte(hex, 16))
                .ToArray();

            var packet = new Packet(responseBytes, responseBytes.Length, false);

            var len = (int)packet.Length;
            var pack = packet.Compile();
            _sendToServer?.Invoke(ref pack, ref len);

            return true;
        }
    }
}