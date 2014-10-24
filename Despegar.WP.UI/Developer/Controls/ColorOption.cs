using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Despegar.WP.UI.Developer.Controls
{
    public class ColorOption
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public SolidColorBrush Brush { get; set; }

        public static List<ColorOption> ColorData()
            {
            string[] colorNames =
               {
                "White","Black","Yellow","BananaYellow","LaserLemon","Jasmine","Green","Emerald",
                "GreenYellow","Lime","Chartreuse","LimeGreen","SpringGreen","LightGreen",
                "MediumSeaGreen","MediumSpringGreen","Olive","SeaGreen","Red","OrangeRed",
                "DarkOrange","Orange","ImperialRed","Maroon","Brown","Chocolate",
                "Coral","Crimson","DarkSalmon","DeepPink","Firebrick","HotPink",
                "IndianRed","LightCoral","LightPink","LightSalmon","Magenta","MediumVioletRed",
                "Orchid","PaleVioletRed","Salmon","SandyBrown","Navy","Indigo",
                "MidnightBlue","Blue","Purple","BlueViolet","CornflowerBlue","Cyan",
                "DarkCyan","DarkSlateBlue","DeepSkyBlue","DodgerBlue","LightBlue","LightSeaGreen",
                "LightSkyBlue","LightSteelBlue","Mauve","MediumSlateBlue","RoyalBlue","SlateBlue",
                "SlateGray","SteelBlue","Teal","Turquoise","DarkGrey","LightGray"
                };

        string[] uintColors =
            { 
                    "#FFFFFFFF","#FF000000","#FFFFFF00","#FFFFE135","#FFFFFF66","#FFF8DE7E",
                    "#FF008000","#FF008A00","#FFADFF2F","#FF00FF00","#FF7FFF00","#FF32CD32",
                    "#FF00FF7F","#FF90EE90","#FF3CB371","#FF00FA9A","#FF808000","#FF2E8B57",
                    "#FFFF0000","#FFFF4500",
                    "#FFFF8C00","#FFFFA500","#FFED2939","#FF800000","#FFA52A2A","#FFD2691E",
                    "#FFFF7F50","#FFDC143C","#FFE9967A","#FFFF1493","#FFB22222","#FFFF69B4",
                    "#FFCD5C5C","#FFF08080","#FFFFB6C1","#FFFFA07A","#FFFF00FF","#FFC71585",
                    "#FFDA70D6","#FFDB7093","#FFFA8072","#FFF4A460","#FF000080","#FF4B0082",
                    "#FF191970","#FF0000FF","#FF800080","#FF8A2BE2","#FF6495ED","#FF00FFFF",
                    "#FF008B8B","#FF483D8B","#FF00BFFF","#FF1E90FF","#FFADD8E6","#FF20B2AA",
                    "#FF87CEFA","#FFB0C4DE","#FF76608A","#FF7B68EE","#FF4169E1","#FF6A5ACD",
                    "#FF708090","#FF4682B4","#FF008080","#FF40E0D0","#FFA9A9A9","#FFD3D3D3"
            };

       // i variable depends on how many color you want to add in my case i have 67 colors
        var data = new List<ColorOption>();
        for (int i = 0; i < 68; i++) {
            data.Add(new ColorOption(colorNames[i], uintColors[i]));
        }

        return data;
    }

        public ColorOption(string name, string color)
        {
            Name = name;
            int argb = Int32.Parse(color.Replace("#", ""), NumberStyles.HexNumber);
            Color = Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                                  (byte)((argb & 0xff0000) >> 0x10),
                                  (byte)((argb & 0xff00) >> 8),
                                  (byte)(argb & 0xff));

            Brush = new SolidColorBrush(Color);
         }
    }
}