using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Ephemera.NBagOfTricks;


namespace WinConsole
{
    public static class ColorCons
    {
        //static Dictionary<(int, int, int), KnownColor>? _colorLUT = null;

        /// <summary>Convert ConsoleColor to System.Color.</summary>
        /// <param name="conclr">In color</param>
        /// <returns>Out color</returns>
        public static Color ToSystemColor(this ConsoleColor conclr)
        {
           // System.Color names don't match ConsoleColor very well. Let's just bin them arbitrarily.

           var ret = conclr switch
           {
               // ConsoleColor.Name        System.Color                    ConsoleColor val  System.Color.Name
               ConsoleColor.Black       => ColorUtils.MakeColor(0x00, 0x00, 0x00), // 0000              Black
               ConsoleColor.DarkBlue    => ColorUtils.MakeColor(0x00, 0x00, 0x80), // 0001              Navy
               ConsoleColor.DarkGreen   => ColorUtils.MakeColor(0x00, 0x80, 0x00), // 0010              Green
               ConsoleColor.DarkCyan    => ColorUtils.MakeColor(0x00, 0x80, 0x80), // 0011              Teal
               ConsoleColor.DarkRed     => ColorUtils.MakeColor(0x80, 0x00, 0x00), // 0100              Maroon
               ConsoleColor.DarkMagenta => ColorUtils.MakeColor(0x80, 0x00, 0x80), // 0101              Purple
               ConsoleColor.DarkYellow  => ColorUtils.MakeColor(0x80, 0x80, 0x00), // 0110              Olive
               ConsoleColor.Gray        => ColorUtils.MakeColor(0xC0, 0xC0, 0xC0), // 0111              Silver 
               ConsoleColor.DarkGray    => ColorUtils.MakeColor(0x80, 0x80, 0x80), // 1000              Gray
               ConsoleColor.Blue        => ColorUtils.MakeColor(0x00, 0x00, 0xFF), // 1001              Blue
               ConsoleColor.Green       => ColorUtils.MakeColor(0x00, 0xFF, 0x00), // 1010              Lime
               ConsoleColor.Cyan        => ColorUtils.MakeColor(0x00, 0xFF, 0xFF), // 1011              Aqua
               ConsoleColor.Red         => ColorUtils.MakeColor(0xFF, 0x00, 0x00), // 1100              Red
               ConsoleColor.Magenta     => ColorUtils.MakeColor(0xFF, 0x00, 0xFF), // 1101              Fuchsia
               ConsoleColor.Yellow      => ColorUtils.MakeColor(0xFF, 0xFF, 0x00), // 1110              Yellow
               ConsoleColor.White       => ColorUtils.MakeColor(0xFF, 0xFF, 0xFF), // 1111              White
               _ => throw new ArgumentException(null, nameof(conclr)),
           };
           return ret;
        }

        /// <summary>Convert System.Color to ConsoleColor.</summary>
        /// <param name="sysclr">The color</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ConsoleColor ToConsoleColor(this Color sysclr)
        {
            ConsoleColor conclr;
            var sat = sysclr.GetSaturation(); // 0.0 => 1.0
            var brt = sysclr.GetBrightness(); // 0.0 => 1.0
            var hue = (int)Math.Round(sysclr.GetHue() / 60, MidpointRounding.AwayFromZero); // 0 => 5
            var grayish = sat < 0.5;
            var darkish = brt < 0.5;

            if (grayish)
            {
                conclr = (int)(brt * 3.5) switch
                {
                    0 => ConsoleColor.Black,
                    1 => ConsoleColor.DarkGray,
                    2 => ConsoleColor.Gray,
                    _ => ConsoleColor.White,
                };
            }
            else // color
            {
                conclr = hue switch
                {
                    1 => darkish ? ConsoleColor.DarkYellow : ConsoleColor.Yellow,
                    2 => darkish ? ConsoleColor.DarkGreen : ConsoleColor.Green,
                    3 => darkish ? ConsoleColor.DarkCyan : ConsoleColor.Cyan,
                    4 => darkish ? ConsoleColor.DarkBlue : ConsoleColor.Blue,
                    5 => darkish ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta,
                    _ => darkish ? ConsoleColor.DarkRed : ConsoleColor.Red
                };
            }

            return conclr;
        }

        /// <summary>Simple binning approach.</summary>
        /// <param name="sysclr"></param>
        /// <returns></returns>
        public static ConsoleColor ToConsoleColor_simple(this Color sysclr)
        {
            int val = (sysclr.R > 0x80 || sysclr.G > 0x80 || sysclr.B > 0x80) ? 8 : 0;
            int lim = 0x40;
            val |= sysclr.R > lim ? 4 : 0;
            val |= sysclr.G > lim ? 2 : 0;
            val |= sysclr.B > lim ? 1 : 0;
            return (ConsoleColor)val;
        }

        /// <summary>Parse console color safely.</summary>
        /// <param name="value">Color name</param>
        /// <returns>Console color or null if invalid</returns>
        public static ConsoleColor? ParseNullableConsoleColor(string value)
        {
            return Enum.TryParse(value, ignoreCase: true, out ConsoleColor result) ? result : null;
        }
    }
}
