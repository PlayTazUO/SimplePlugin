using System.Runtime.InteropServices;
using CUO_API;

namespace Assistant;

public class Engine
{
    public const string PLUGIN_NAME = "Simple Plugin";

    // Delegates for calling back into the client
    private static OnPacketSendRecv? _sendToClient;
    private static OnPacketSendRecv? _sendToServer;
    private static OnGetPacketLength? _getPacketLength;
    private static OnGetPlayerPosition? _getPlayerPosition;
    private static OnCastSpell? _castSpell;
    private static OnGetStaticImage? _getStaticImage;
    private static OnTick? _tick;
    private static RequestMove? _requestMove;
    private static OnSetTitle? _setTitle;
    private static OnGetUOFilePath? _getUOFilePath;

    // Delegates for our plugin callbacks
    private static OnPacketSendRecv? _onRecv;
    private static OnPacketSendRecv? _onSend;
    private static OnHotkey? _onHotkey;
    private static OnMouse? _onMouse;
    private static OnUpdatePlayerPosition? _onPlayerPositionChanged;
    private static OnClientClose? _onClientClosing;
    private static OnInitialize? _onInitialize;
    private static OnConnected? _onConnected;
    private static OnDisconnected? _onDisconnected;
    private static OnFocusGained? _onFocusGained;
    private static OnFocusLost? _onFocusLost;

    private static string? m_ClientVersion;

    public Engine()
    {
    }

    public static unsafe void Install(PluginHeader* header)
    {
        Log.Info("Loaded.");

        // Get client version
        m_ClientVersion = new Version((byte)(header->ClientVersion >> 24), (byte)(header->ClientVersion >> 16),
            (byte)(header->ClientVersion >> 8), (byte)header->ClientVersion).ToString();

        // Setup pointers to call back into the client
        _sendToClient = (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer(header->Recv, typeof(OnPacketSendRecv));
        _sendToServer = (OnPacketSendRecv)Marshal.GetDelegateForFunctionPointer(header->Send, typeof(OnPacketSendRecv));
        _getPacketLength = (OnGetPacketLength)Marshal.GetDelegateForFunctionPointer(header->GetPacketLength, typeof(OnGetPacketLength));
        _getPlayerPosition = (OnGetPlayerPosition)Marshal.GetDelegateForFunctionPointer(header->GetPlayerPosition, typeof(OnGetPlayerPosition));
        _castSpell = (OnCastSpell)Marshal.GetDelegateForFunctionPointer(header->CastSpell, typeof(OnCastSpell));
        _getStaticImage = (OnGetStaticImage)Marshal.GetDelegateForFunctionPointer(header->GetStaticImage, typeof(OnGetStaticImage));
        _requestMove = (RequestMove)Marshal.GetDelegateForFunctionPointer(header->RequestMove, typeof(RequestMove));
        _setTitle = (OnSetTitle)Marshal.GetDelegateForFunctionPointer(header->SetTitle, typeof(OnSetTitle));
        _getUOFilePath = (OnGetUOFilePath)Marshal.GetDelegateForFunctionPointer(header->GetUOFilePath, typeof(OnGetUOFilePath));

        // Setup our plugin's callbacks
        _onRecv = Actions.OnPacketReceived;
        _onSend = Actions.OnPacketSent;
        _onHotkey = Actions.OnHotkey;
        _onMouse = Actions.OnMouse;
        _onPlayerPositionChanged = Actions.OnPlayerPositionChanged;
        _onClientClosing = Actions.OnClientClosing;
        _onInitialize = Actions.OnInitialize;
        _onConnected = Actions.OnConnected;
        _onDisconnected = Actions.OnDisconnected;
        _onFocusGained = Actions.OnFocusGained;
        _onFocusLost = Actions.OnFocusLost;
        _tick = Actions.Tick;

        // Wire up the callbacks to the client
        header->OnRecv = Marshal.GetFunctionPointerForDelegate(_onRecv);
        header->OnSend = Marshal.GetFunctionPointerForDelegate(_onSend);
        header->OnHotkeyPressed = Marshal.GetFunctionPointerForDelegate(_onHotkey);
        header->OnMouse = Marshal.GetFunctionPointerForDelegate(_onMouse);
        header->OnPlayerPositionChanged = Marshal.GetFunctionPointerForDelegate(_onPlayerPositionChanged);
        header->OnClientClosing = Marshal.GetFunctionPointerForDelegate(_onClientClosing);
        header->OnInitialize = Marshal.GetFunctionPointerForDelegate(_onInitialize);
        header->OnConnected = Marshal.GetFunctionPointerForDelegate(_onConnected);
        header->OnDisconnected = Marshal.GetFunctionPointerForDelegate(_onDisconnected);
        header->OnFocusGained = Marshal.GetFunctionPointerForDelegate(_onFocusGained);
        header->OnFocusLost = Marshal.GetFunctionPointerForDelegate(_onFocusLost);
    }
}