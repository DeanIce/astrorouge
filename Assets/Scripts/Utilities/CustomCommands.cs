using System.Linq;
using IngameDebugConsole;
using Managers;
using UnityEngine.Scripting;

namespace Utilities
{
    public static class CustomCommands
    {
        [ConsoleMethod("test", "test command")]
        [Preserve]
        public static string TestCommand(string key)
        {
            return "result";
        }

        [ConsoleMethod("level.restart", "reload the current level")]
        [Preserve]
        public static string LevelRestart(string key)
        {
            LevelManager.Instance.LoadLevel();
            return "done";
        }

        [ConsoleMethod("level.list", "list levels in the game")]
        [Preserve]
        public static string LevelList(string key)
        {
            var result = string.Join("\n",
                LevelManager.Instance.levels.Select((description, i) => $"{i}: {description.displayName}"));

            return result;
        }

        /*[ConsoleMethod( "prefs.int", "Sets the value of an Integer PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static void PlayerPrefsSetInt( string key, int value )
        {
            PlayerPrefs.SetInt( key, value );
        }

        [ConsoleMethod( "prefs.float", "Returns the value of a Float PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static string PlayerPrefsGetFloat( string key )
        {
            if( !PlayerPrefs.HasKey( key ) ) return "Key Not Found";
            return PlayerPrefs.GetFloat( key ).ToString();
        }

        [ConsoleMethod( "prefs.float", "Sets the value of a Float PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static void PlayerPrefsSetFloat( string key, float value )
        {
            PlayerPrefs.SetFloat( key, value );
        }

        [ConsoleMethod( "prefs.string", "Returns the value of a String PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static string PlayerPrefsGetString( string key )
        {
            if( !PlayerPrefs.HasKey( key ) ) return "Key Not Found";
            return PlayerPrefs.GetString( key );
        }

        [ConsoleMethod( "prefs.string", "Sets the value of a String PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static void PlayerPrefsSetString( string key, string value )
        {
            PlayerPrefs.SetString( key, value );
        }

        [ConsoleMethod( "prefs.delete", "Deletes a PlayerPrefs field" ), UnityEngine.Scripting.Preserve]
        public static void PlayerPrefsDelete( string key )
        {
            PlayerPrefs.DeleteKey( key );
        }

        [ConsoleMethod( "prefs.clear", "Deletes all PlayerPrefs fields" ), UnityEngine.Scripting.Preserve]
        public static void PlayerPrefsClear()
        {
            PlayerPrefs.DeleteAll();
        }*/
    }
}