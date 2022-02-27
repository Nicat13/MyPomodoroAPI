using System.Collections.Generic;

namespace MyPomodoro.Domain.Enums
{
    public class PomodoroColors
    {
        public int Id { get; private set; }
        public string BgColor { get; private set; }
        public string TxtColor { get; private set; }
        public static readonly List<PomodoroColors> Colors = new List<PomodoroColors>
        { new PomodoroColors {Id=1, BgColor = "black", TxtColor = "yellow" },
          new PomodoroColors {Id=2, BgColor = "red", TxtColor = "black" }
        };
    }
}