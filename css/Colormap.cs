using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public struct Colormap
    {
        public static uint[] Values = {
        0xfff0f8ff, /* ALICEBLUE */
		0xfffaebd7, /* ANTIQUEWHITE */
		0xff00ffff, /* AQUA */
		0xff7fffd4, /* AQUAMARINE */
		0xfff0ffff, /* AZURE */
		0xfff5f5dc, /* BEIGE */
		0xffffe4c4, /* BISQUE */
		0xff000000, /* BLACK */
		0xffffebcd, /* BLANCHEDALMOND */
		0xff0000ff, /* BLUE */
		0xff8a2be2, /* BLUEVIOLET */
		0xffa52a2a, /* BROWN */
		0xffdeb887, /* BURLYWOOD */
		0xff5f9ea0, /* CADETBLUE */
		0xff7fff00, /* CHARTREUSE */
		0xffd2691e, /* CHOCOLATE */
		0xffff7f50, /* CORAL */
		0xff6495ed, /* CORNFLOWERBLUE */
		0xfffff8dc, /* CORNSILK */
		0xffdc143c, /* CRIMSON */
		0xff00ffff, /* CYAN */
		0xff00008b, /* DARKBLUE */
		0xff008b8b, /* DARKCYAN */
		0xffb8860b, /* DARKGOLDENROD */
		0xffa9a9a9, /* DARKGRAY */
		0xff006400, /* DARKGREEN */
		0xffa9a9a9, /* DARKGREY */
		0xffbdb76b, /* DARKKHAKI */
		0xff8b008b, /* DARKMAGENTA */
		0xff556b2f, /* DARKOLIVEGREEN */
		0xffff8c00, /* DARKORANGE */
		0xff9932cc, /* DARKORCHID */
		0xff8b0000, /* DARKRED */
		0xffe9967a, /* DARKSALMON */
		0xff8fbc8f, /* DARKSEAGREEN */
		0xff483d8b, /* DARKSLATEBLUE */
		0xff2f4f4f, /* DARKSLATEGRAY */
		0xff2f4f4f, /* DARKSLATEGREY */
		0xff00ced1, /* DARKTURQUOISE */
		0xff9400d3, /* DARKVIOLET */
		0xffff1493, /* DEEPPINK */
		0xff00bfff, /* DEEPSKYBLUE */
		0xff696969, /* DIMGRAY */
		0xff696969, /* DIMGREY */
		0xff1e90ff, /* DODGERBLUE */
		0xffd19275, /* FELDSPAR */
		0xffb22222, /* FIREBRICK */
		0xfffffaf0, /* FLORALWHITE */
		0xff228b22, /* FORESTGREEN */
		0xffff00ff, /* FUCHSIA */
		0xffdcdcdc, /* GAINSBORO */
		0xfff8f8ff, /* GHOSTWHITE */
		0xffffd700, /* GOLD */
		0xffdaa520, /* GOLDENROD */
		0xff808080, /* GRAY */
		0xff008000, /* GREEN */
		0xffadff2f, /* GREENYELLOW */
		0xff808080, /* GREY */
		0xfff0fff0, /* HONEYDEW */
		0xffff69b4, /* HOTPINK */
		0xffcd5c5c, /* INDIANRED */
		0xff4b0082, /* INDIGO */
		0xfffffff0, /* IVORY */
		0xfff0e68c, /* KHAKI */
		0xffe6e6fa, /* LAVENDER */
		0xfffff0f5, /* LAVENDERBLUSH */
		0xff7cfc00, /* LAWNGREEN */
		0xfffffacd, /* LEMONCHIFFON */
		0xffadd8e6, /* LIGHTBLUE */
		0xfff08080, /* LIGHTCORAL */
		0xffe0ffff, /* LIGHTCYAN */
		0xfffafad2, /* LIGHTGOLDENRODYELLOW */
		0xffd3d3d3, /* LIGHTGRAY */
		0xff90ee90, /* LIGHTGREEN */
		0xffd3d3d3, /* LIGHTGREY */
		0xffffb6c1, /* LIGHTPINK */
		0xffffa07a, /* LIGHTSALMON */
		0xff20b2aa, /* LIGHTSEAGREEN */
		0xff87cefa, /* LIGHTSKYBLUE */
		0xff8470ff, /* LIGHTSLATEBLUE */
		0xff778899, /* LIGHTSLATEGRAY */
		0xff778899, /* LIGHTSLATEGREY */
		0xffb0c4de, /* LIGHTSTEELBLUE */
		0xffffffe0, /* LIGHTYELLOW */
		0xff00ff00, /* LIME */
		0xff32cd32, /* LIMEGREEN */
		0xfffaf0e6, /* LINEN */
		0xffff00ff, /* MAGENTA */
		0xff800000, /* MAROON */
		0xff66cdaa, /* MEDIUMAQUAMARINE */
		0xff0000cd, /* MEDIUMBLUE */
		0xffba55d3, /* MEDIUMORCHID */
		0xff9370db, /* MEDIUMPURPLE */
		0xff3cb371, /* MEDIUMSEAGREEN */
		0xff7b68ee, /* MEDIUMSLATEBLUE */
		0xff00fa9a, /* MEDIUMSPRINGGREEN */
		0xff48d1cc, /* MEDIUMTURQUOISE */
		0xffc71585, /* MEDIUMVIOLETRED */
		0xff191970, /* MIDNIGHTBLUE */
		0xfff5fffa, /* MINTCREAM */
		0xffffe4e1, /* MISTYROSE */
		0xffffe4b5, /* MOCCASIN */
		0xffffdead, /* NAVAJOWHITE */
		0xff000080, /* NAVY */
		0xfffdf5e6, /* OLDLACE */
		0xff808000, /* OLIVE */
		0xff6b8e23, /* OLIVEDRAB */
		0xffffa500, /* ORANGE */
		0xffff4500, /* ORANGERED */
		0xffda70d6, /* ORCHID */
		0xffeee8aa, /* PALEGOLDENROD */
		0xff98fb98, /* PALEGREEN */
		0xffafeeee, /* PALETURQUOISE */
		0xffdb7093, /* PALEVIOLETRED */
		0xffffefd5, /* PAPAYAWHIP */
		0xffffdab9, /* PEACHPUFF */
		0xffcd853f, /* PERU */
		0xffffc0cb, /* PINK */
		0xffdda0dd, /* PLUM */
		0xffb0e0e6, /* POWDERBLUE */
		0xff800080, /* PURPLE */
		0xffff0000, /* RED */
		0xffbc8f8f, /* ROSYBROWN */
		0xff4169e1, /* ROYALBLUE */
		0xff8b4513, /* SADDLEBROWN */
		0xfffa8072, /* SALMON */
		0xfff4a460, /* SANDYBROWN */
		0xff2e8b57, /* SEAGREEN */
		0xfffff5ee, /* SEASHELL */
		0xffa0522d, /* SIENNA */
		0xffc0c0c0, /* SILVER */
		0xff87ceeb, /* SKYBLUE */
		0xff6a5acd, /* SLATEBLUE */
		0xff708090, /* SLATEGRAY */
		0xff708090, /* SLATEGREY */
		0xfffffafa, /* SNOW */
		0xff00ff7f, /* SPRINGGREEN */
		0xff4682b4, /* STEELBLUE */
		0xffd2b48c, /* TAN */
		0xff008080, /* TEAL */
		0xffd8bfd8, /* THISTLE */
		0xffff6347, /* TOMATO */
		0xff40e0d0, /* TURQUOISE */
		0xffee82ee, /* VIOLET */
		0xffd02090, /* VIOLETRED */
		0xfff5deb3, /* WHEAT */
		0xffffffff, /* WHITE */
		0xfff5f5f5, /* WHITESMOKE */
		0xffffff00, /* YELLOW */
		0xff9acd32  /* YELLOWGREEN */
	};

		public static uint Transparent = 0x01000000; // Not a CSS Color!
    }
}
