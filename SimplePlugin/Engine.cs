using System.Runtime.InteropServices;
using CUO_API;

namespace Assistant;

public class Engine
{
    public Actions Actions;

    private static OnPacketSendRecv? _sendToServer;
    private static OnPacketSendRecv? _recv;
    private static string? m_ClientVersion;

    public Engine()
    {
        Actions = new Actions(this);
    }

    public static unsafe void Install(PluginHeader* header)
    {
        Console.WriteLine("Simple plugin loaded.");

        //Setup various pointers
        _sendToServer = (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer(header->Send, typeof(OnPacketSendRecv));

        m_ClientVersion = new Version((byte)(header->ClientVersion >> 24), (byte)(header->ClientVersion >> 16),
            (byte)(header->ClientVersion >> 8), (byte)header->ClientVersion).ToString();

        _recv = Actions.OnPacketReceived;

        header->OnRecv = Marshal.GetFunctionPointerForDelegate(_recv);
    }
}