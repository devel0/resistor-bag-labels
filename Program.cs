using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using netDxf;
using netDxf.Entities;
using static System.FormattableString;

namespace resistor_bag_labels
{
    class Program
    {
        static void Main(string[] args)
        {
            var dxf = new netDxf.DxfDocument();

            var rValues1 = new List<double>()
            {
                1, 1.2, 1.5, 1.8, 2.2, 2.7, 3.3, 3.9,
                4.7, 5.6, 6.8, 8.2, 10, 12, 15, 18, 22,
                27, 33, 39, 47, 56, 68, 82, 100,
                120, 150, 180, 220, 270, 330, 390, 470,
                560, 680, 820,

                1e3, 1.2e3, 1.5e3, 1.8e3, 2.2e3, 2.7e3,
                3.3e3, 3.9e3, 4.7e3, 5.5e3, 6.8e3, 7.2e3, 8.2e3, 10e3, 12e3,
                15e3, 18e3,22e3, 27e3, 33e3, 39e3, 47e3, 56e3, 82e3, 100e3,
                120e3,150e3,180e3, 220e3, 270e3, 330e3, 380e3,
                470e3, 560e3, 680e3, 820e3,

                1e6, 1.2e6, 1.5e6, 1.8e6, 2.2e6, 2.7e6, 3.3e6, 3.9e6, 4.7e6,
                5.6e6, 6.8e6, 8.2e6, 10e6
            };

            var rValues2 = new List<double>()
            {
                7.5, 75, 200, 510,

                2e3, 3e3, 5.1e3, 7.5e3, 20e3, 51e3, 68e3, 75e3, 200e3, 510e3, 750e3,

                2e6
            };

            var rValues3 = new List<double>()
            {
                1.2, 1.5, 1.8, 2.7, 3.3, 3.9, 6.8, 12, 18, 560, 820,
                1.2e3, 1.8e3, 2.7e3, 12e3, 18e3, 27e3, 270e3, 380e3,
                5.6e3, 300e3,
                1.2e6, 1.8e6, 2.2e6, 2.7e6, 3.9e6, 6.8e6, 8.2e6
            };

            var DATA = rValues1.Union(rValues2).Union(rValues3).OrderBy(w => w).ToList();

            var PAGE_W = 210d;
            var PAGE_H = 297d;

            var LABEL_W = 25d;
            var LABEL_H = 20d;

            var MARGINS_LTRB = new[] { 10d, 10d, 10d, 10d };

            var COLS = Math.Truncate((PAGE_W - MARGINS_LTRB[0] - MARGINS_LTRB[2]) / LABEL_W);
            var ROWS = Math.Truncate((PAGE_H - MARGINS_LTRB[1] - MARGINS_LTRB[3]) / LABEL_H);

            var XORIGIN = 0d;
            var YORIGIN = 0d;

            var TXT_HEIGHT = 5d;
            var OHM_HEIGHT = 3d;
            var TXT_FONT = new netDxf.Tables.TextStyle("Ubuntu Condensed", netDxf.Tables.FontStyle.Regular);

            var x = XORIGIN;
            var y = YORIGIN;

            int page = 0;
            for (int i = 0; i < DATA.Count; ++i)
            {
                var r = DATA[i];

                var prefix = "";
                if (r >= 1e6)
                {
                    prefix = " M";
                    r /= 1e6;
                }
                else if (r >= 1e3)
                {
                    prefix = " k";
                    r /= 1e3;
                }

                var txt = Invariant($"\\H{TXT_HEIGHT};{r}{prefix} \\H{OHM_HEIGHT};Ω");

                {
                    var ent = new LwPolyline(new[]
                    {
                        new Vector2(x, y),
                        new Vector2(x + LABEL_W, y),
                        new Vector2(x + LABEL_W, y + LABEL_H),
                        new Vector2(x, y + LABEL_H),
                    }, isClosed: true);
                    dxf.AddEntity(ent);
                }

                {
                    var ent = new MText(txt)
                    {
                        Position = new Vector3(x + LABEL_W / 2, y + LABEL_H / 2, 0),
                        Height = TXT_HEIGHT,
                        RectangleWidth = LABEL_W,
                        AttachmentPoint = MTextAttachmentPoint.MiddleCenter,
                        Style = TXT_FONT
                    };
                    dxf.AddEntity(ent);
                }

                x += LABEL_W;
                if ((i + 1) % COLS == 0)
                {
                    y += LABEL_H;

                    if (y >= ROWS * LABEL_H)
                    {
                        y = YORIGIN;
                        ++page;
                    }
                    x = XORIGIN + page * (PAGE_W + LABEL_W);
                }
            }

            dxf.Save("output.dxf", isBinary: false);

            Process.Start(new ProcessStartInfo("output.dxf") { UseShellExecute = true });
        }
    }
}
