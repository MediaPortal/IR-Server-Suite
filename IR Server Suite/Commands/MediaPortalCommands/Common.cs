using MediaPortal.Configuration;

namespace Commands.MediaPortal
{
  internal static class Common
  {
    /// <summary>
    /// Folder for Input Device data default files.
    /// </summary>
    public static readonly string CustomInputDefault = Config.GetFolder(Config.Dir.CustomInputDefault) + "\\";

    /// <summary>
    /// Folder for Custom Input Device data files.
    /// </summary>
    public static readonly string CustomInputDevice = Config.GetFolder(Config.Dir.CustomInputDevice) + "\\";

    /// <summary>
    /// Path to the MediaPortal configuration file.
    /// </summary>
    public static readonly string MPConfigFile = Config.GetFolder(Config.Dir.Config) + "\\MediaPortal.xml";
  }
}