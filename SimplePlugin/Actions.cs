using Assistant;

public class Actions(Engine engine)
{
    public static unsafe bool OnPacketReceived(ref byte[] data, ref int length)
    {
        fixed (byte* ptr = data)
        {
            byte id = data[0];

            //id is the packet id, you can filter here for specific packets. 
            //return true means the client(TazUO) will still process this packet, return false if you want the client to ignore this packet.
            return true;
        }
    }   
}