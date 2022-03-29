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
        public static string LevelRestart()
        {
            EventManager.Instance.Play();
            // LevelManager.Instance.StartCoroutineLoadLevel();
            return "done";
        }

        [ConsoleMethod("level.list", "list levels in the game")]
        [Preserve]
        public static string LevelList()
        {
            string result = string.Join("\n",
                LevelSelect.Instance.levels.Select((description, i) => $"{i}: {description.name}"));

            return result;
        }

        [ConsoleMethod("level.load", "load level by index (see level.list)")]
        [Preserve]
        public static string LevelList(int level)
        {
            if (level >= LevelSelect.Instance.levels.Count)
                return $"Level index {level} out of bounds. See the `level.list` command.";


            EventManager.Instance.Play();
            // LevelManager.Instance.current = level;
            // LevelManager.Instance.StartCoroutineLoadLevel();
            return "done";
        }

        [ConsoleMethod("event", "trigger an event")]
        [Preserve]
        public static string Event(string ev)
        {
            switch (ev)
            {
                case "pause":
                    EventManager.Instance.Pause();
                    break;
                case "play":
                    EventManager.Instance.Play();
                    break;
                case "menu":
                    EventManager.Instance.Menu();
                    break;
                case "win":
                    EventManager.Instance.Win();
                    break;
                case "recap":
                    EventManager.Instance.Recap();
                    break;
                case "exit":
                    EventManager.Instance.Exit();
                    break;

                default:
                    return "event must be pause|play|menu|win|recap|exit";
            }


            return "done";
        }
    }
}