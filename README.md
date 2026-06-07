# SimplePlugin

A template plugin for TazUO that demonstrates the CUO_API plugin interface.

## Getting Started

### Option 1: Fork this Repository (Recommended)

1. Click the "Fork" button at the top of this repository
2. Clone your fork to your local machine:
   ```bash
   git clone https://github.com/YOUR-USERNAME/SimplePlugin.git
   cd SimplePlugin
   ```
3. Start coding your plugin!

### Option 3: Clone Directly

1. Clone this repository:
   ```bash
   git clone https://github.com/PlayTazUO/SimplePlugin.git
   cd SimplePlugin
   ```


## Building Your Plugin

### Prerequisites

- .NET 10 SDK
- TazUO client
- The `cuoapi.dll` file (included in this repository)

### Build Instructions

1. Open the solution in your preferred IDE (Visual Studio, Rider, VS Code)
2. Build the project:
   ```bash
   dotnet build -c Release
   ```
3. The compiled plugin DLL will be in `SimplePlugin/bin/Release/net10.0/`

### Installing the Plugin

1. Copy the compiled `SimplePlugin.dll` to your TazUO plugins directory
2. Launch TazUO
3. The plugin will load automatically and you should see "Simple plugin loaded" in the console

## Project Structure

```
SimplePlugin/
├── SimplePlugin/
│   ├── Actions.cs           # Event handlers and callbacks <-- start here with your plugin
│   ├── Engine.cs            # Main plugin engine and initialization
│   ├── Packet.cs            # Packet creation/manipulation utilities
│   ├── SPlugin.cs           # Entry point
│   └── SimplePlugin.csproj  # Project configuration
├── cuoapi.dll               # TazUO API library
└── README.md
```

## Available Callbacks

The plugin template includes stubs for all CUO_API callbacks in `Actions.cs`:

### Packet Handling
- **`OnPacketReceived`** - Intercept packets from server to client
- **`OnPacketSent`** - Intercept packets from client to server

### Input Events
- **`OnHotkey`** - Handle keyboard hotkeys
- **`OnMouse`** - Handle mouse events

### Game Events
- **`OnPlayerPositionChanged`** - Track player movement
- **`OnInitialize`** - Called when plugin loads
- **`OnConnected`** - Called when connecting to server
- **`OnDisconnected`** - Called when disconnecting
- **`OnClientClosing`** - Called when client closes
- **`OnFocusGained`** - Window focus gained
- **`OnFocusLost`** - Window focus lost

## Available Client Functions

The plugin can call back into the TazUO client using these functions (defined in `Engine.cs`):

- **`_sendToClient`** - Inject packets to the client
- **`_sendToServer`** - Send packets to the server
- **`_getPacketLength`** - Get packet length by ID
- **`_getPlayerPosition`** - Get current player coordinates
- **`_castSpell`** - Cast a spell by index
- **`_getStaticImage`** - Get static image/art data
- **`_tick`** - Execute on game tick
- **`_requestMove`** - Request player movement
- **`_setTitle`** - Set window title
- **`_getUOFilePath`** - Get UO installation path

## Example: Simple Packet Logger

```csharp
public static unsafe bool OnPacketReceived(ref byte[] data, ref int length)
{
    byte id = data[0];
    Console.WriteLine($"Received packet 0x{id:X2}, length: {length}");
    return true; // Let client process the packet
}
```

## Example: Block Specific Packets

```csharp
public static unsafe bool OnPacketSent(ref byte[] data, ref int length)
{
    byte id = data[0];

    // Block movement packets (0x02)
    if (id == 0x02)
    {
        Console.WriteLine("Blocked movement packet");
        return false; // Block the packet
    }

    return true; // Allow other packets
}
```

## Customizing Your Plugin

1. **Implement callbacks**: Add your logic to the callback methods in `Actions.cs`
2. **Add new classes**: Create additional helper classes as needed
3. **Use the Packet class**: Build and send custom packets using the `Packet` utility
4. **Update plugin info**: Modify the console messages to reflect your plugin name

## Renaming your plugin

Inside SimplePlugin.csproj you can change the name inside the `<AssemblyName>` tags.

## Debugging

To debug your plugin:

1. Build in Debug configuration
2. Attach your debugger to the TazUO process
3. Set breakpoints in your plugin code
4. Launch TazUO

Console output will appear in the TazUO console window.

## Contributing

This is a template repository. Feel free to modify it for your needs. If you create improvements to the base template structure, consider contributing back!

## License

See [LICENSE](LICENSE) file for details.