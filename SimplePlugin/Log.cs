using Assistant;

public static class Log
{
    public static void Info(string text) => Console.WriteLine($"[{Engine.PLUGIN_NAME}]  (INFO): {text}");
    public static void Warning(string text) => Console.WriteLine($"[{Engine.PLUGIN_NAME}]  (WARN): {text}");
    public static void Error(string text) => Console.WriteLine($"[{Engine.PLUGIN_NAME}] (ERROR): {text}");
}