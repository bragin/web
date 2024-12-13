using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using HtmlParserSharp.Common;
using HtmlParserSharp.Core;
using System.Reflection;
using System.Drawing.Drawing2D;
using System;
using System.Xml.Linq;
using web;

namespace SkiaSharpOpenGLBenchmark
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        HtmlContent content;
        Plotter Plot;

        private FormElements floatingWindow;

        public Form1()
        {
            // Disable DPI scaling
            AutoScaleMode = AutoScaleMode.None;

            InitializeComponent();

            //CodeGenerator.GeneratePropertyParsers();
            //CodeGenerator.GenerateCodeStubs2();

            // Manually add another SkiaSharp control
            skglControl2 = new SkiaSharp.Views.Desktop.SKGLControl();

            skglControl2.AutoScaleMode = AutoScaleMode.None;

            skglControl2.BackColor = System.Drawing.Color.Gray;
            skglControl2.Location = new Point(10, 10);
            skglControl2.Margin = new Padding(10, 10, 10, 10);
            skglControl2.Name = "skglControl2";
            skglControl2.Size = new Size(100, 100);
            skglControl2.TabIndex = 4;
            skglControl2.VSync = false;
            skglControl2.PaintSurface += skglControl2_PaintSurface;

            //skglControl2.Height = 100;
            //skglControl2.Scale(new SizeF(0.666f, 0.666f));
            //Controls.Add(skglControl2);


            //Scale(0.666f);
            //skglControl1.Scale(new SizeF(0.666f, 0.666f));

            // Create a floating window for the TreeView
            floatingWindow = new FormElements();
            floatingWindow.Show();

            // Load and parse HTML into the TreeView
            floatingWindow.LoadHtmlToTreeView("<html><body><p>Hello, World!</p></body></html>"); // Replace with your HTML source
        }

        private void skglControl2_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
        }

        Random rand = new Random(0);
        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var csize = skglControl1.CanvasSize;
            var surface = e.Surface;

            //surface.Canvas.Scale(0.666f);


            Plot.Surface.Draw(surface.Canvas, 0, 0, null);


            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(0xffdddddd)
            };

            var rect = new SKRect(100, 100, 110, 110);

            surface.Canvas.DrawRect(rect, paint);

            /*
            surface.Canvas.Clear(SKColor.Parse("#003366"));
            for (int i = 0; i < lineCount; i++)
            {
                var paint = new SKPaint
                {
                    Color = new SKColor(
                        red: (byte)rand.Next(255),
                        green: (byte)rand.Next(255),
                        blue: (byte)rand.Next(255),
                        alpha: (byte)rand.Next(255)),
                    StrokeWidth = rand.Next(1, 10),
                    IsAntialias = true
                };
                surface.Canvas.DrawLine(
                    x0: rand.Next(skglControl1.Width),
                    y0: rand.Next(skglControl1.Height),
                    x1: rand.Next(skglControl1.Width),
                    y1: rand.Next(skglControl1.Height),
                    paint: paint);
            }

            var paint2 = new SKPaint();
            //paint2.StrikeThruText = true;
            paint2.TextSize = 24;
            paint2.Color = SKColors.Yellow;
            //paint2.UnderlineText = true;
            paint2.IsAntialias = true;
            paint2.Typeface = SKTypeface.FromFamilyName(
                "Arial",
                SKFontStyleWeight.Bold,
                SKFontStyleWidth.Normal,
                SKFontStyleSlant.Italic);

            surface.Canvas.DrawText("Fancy Text", 30, 30, paint2);

            // Measure text
            var s1 = "Test String";
            //SkString text(s1);
            var paint3 = new SKPaint();
            //paint3.setTextSize(SkIntToScalar(font_size));
            paint3.TextSize = 14;
            //paint3.getFontMetrics(&metrics);
            SKFontMetrics metrics;
            paint3.GetFontMetrics(out metrics);

            SKRect bounds = new SKRect();
            float textWidth = paint3.MeasureText(s1, ref bounds);
            */
        }

        int lineCount;
        List<double> renderTimesMsec = new List<double>();
        private void Benchmark(int lineCount, int times = 10)
        {
            rand = new Random(0);
            renderTimesMsec.Clear();
            this.lineCount = lineCount;
            Stopwatch stopwatch = new Stopwatch();
            for (int i = 0; i < times; i++)
            {
                stopwatch.Restart();
                skglControl1.Invalidate();
                Application.DoEvents();
                stopwatch.Stop();

                renderTimesMsec.Add(1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
                double mean = renderTimesMsec.Sum() / renderTimesMsec.Count();
                Debug.WriteLine($"Render {renderTimesMsec.Count:00} " +
                    $"took {renderTimesMsec.Last():0.000} ms " +
                    $"(running mean: {mean:0.000} ms)");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Benchmark(10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Benchmark(1_000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Benchmark(10_000);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Benchmark(100_000);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();

            Plot = new Plotter();

            content = new HtmlContent(Plot);
            content.LoadDocument();
        }
    }
}
