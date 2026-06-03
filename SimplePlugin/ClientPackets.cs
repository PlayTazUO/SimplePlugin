namespace Assistant;

/// <summary>
/// These are ready to go packets you can send to the client.
/// </summary>
public static class ClientPackets
{
    /// <summary>
    /// Send a message to the client, leave type and serial as default to send a system message.
    /// </summary>
    public static void AsciiMessage(
        string text,
        ushort hue = 0x35, 
        ushort font = 0, 
        string name = "System", 
        MessageType type = MessageType.System, 
        uint serial = 0
    )
    {
        if (string.IsNullOrEmpty(text))
        {
            return; // Don't send empty messages
        }

        var packet = new Packet(0x1C, 45, dynamicLength: true); 
        packet.WriteBE(serial);
        packet.WriteBE((ushort)1); // Graphic, unused in client
        packet.Write((byte)type);
        packet.WriteBE(hue);
        packet.WriteBE(font);

        if (name.Length > 30)
        {
            name = name.Substring(0, 30); // Truncate to 30 characters if too long
        }

        packet.WriteAscii(name, 30); // Write name padded to 30 bytes

        packet.WriteAscii(text);

        byte[] finalPacket = packet.Compile();
        int length = finalPacket.Length;

        Engine.SendToClient(ref finalPacket, ref length);
    }
}