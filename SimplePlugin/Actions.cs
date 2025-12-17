public static class Actions
{
    /// <summary>
    /// This happens when the server sends a packet to the client
    /// </summary>
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

    /// <summary>
    /// This happens when the client sends a packet to the server
    /// </summary>
    public static unsafe bool OnPacketSent(ref byte[] data, ref int length)
    {
        fixed (byte* ptr = data)
        {
            byte id = data[0];

            //id is the packet id being sent to the server
            //return true means the packet will be sent, return false to block it
            return true;
        }
    }

    /// <summary>
    /// This happens when a hotkey is pressed or released
    /// </summary>
    /// <param name="key">The key code</param>
    /// <param name="mod">The modifier keys (ctrl, alt, shift)</param>
    /// <param name="pressed">True when key is pressed down, false when released</param>
    /// <returns>True to let the client process the hotkey, false to block it</returns>
    public static bool OnHotkey(int key, int mod, bool pressed)
    {
        //key is the key code, mod is the modifier keys (ctrl, alt, shift), pressed is true when key is pressed down
        //return true to let the client process the hotkey, false to block it
        Log.Info($"Hotkey pressed: Key {key}, mod {mod}, pressed? {pressed}");
        return true;
    }

    /// <summary>
    /// This happens when mouse input occurs
    /// </summary>
    /// <param name="button">The mouse button (0=none, 1=left, 2=right, 3=middle, etc.)</param>
    /// <param name="wheel">The scroll wheel delta</param>
    public static void OnMouse(int button, int wheel)
    {
        //button is the mouse button (0=none, 1=left, 2=right, 3=middle, etc.)
        //wheel is the scroll wheel delta
        Log.Info($"Mouse button: {button} (Scroll: {wheel})");
    }

    /// <summary>
    /// This happens when the player's position changes
    /// </summary>
    /// <param name="x">The new X coordinate</param>
    /// <param name="y">The new Y coordinate</param>
    /// <param name="z">The new Z coordinate</param>
    public static void OnPlayerPositionChanged(int x, int y, int z)
    {
        //Called when the player's position changes
        //x, y, z are the new coordinates
        Log.Info($"Player moved: {x}, {y}");
    }

    /// <summary>
    /// This happens when the plugin is first initialized
    /// </summary>
    public static void OnInitialize()
    {
        //Called when the plugin is first initialized
        Log.Info("Plugin initialized");
    }

    /// <summary>
    /// This happens when the client connects to a server
    /// </summary>
    public static void OnConnected()
    {
        //Called when the client connects to a server
        Log.Info("Connected to server");
    }

    /// <summary>
    /// This happens when the client disconnects from a server
    /// </summary>
    public static void OnDisconnected()
    {
        //Called when the client disconnects from a server
        Log.Info("Disconnected from server");
    }

    /// <summary>
    /// This happens when the client is closing
    /// </summary>
    public static void OnClientClosing()
    {
        //Called when the client is closing
        Log.Info("Client closing");
    }

    /// <summary>
    /// This happens when the client window gains focus
    /// </summary>
    public static void OnFocusGained()
    {
        //Called when the client window gains focus
        Log.Info("Focus gained");
    }

    /// <summary>
    /// This happens when the client window loses focus
    /// </summary>
    public static void OnFocusLost()
    {
        //Called when the client window loses focus
        Log.Info("Focus lost");
    }

    /// <summary>
    /// Occurs very fast, this is the games standard update loop(Occurs many times per second)
    /// </summary>
    public static void Tick()
    {
        
    }
}