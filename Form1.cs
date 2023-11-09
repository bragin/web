﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using OpenTK.Graphics.ES20;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkiaSharpOpenGLBenchmark
{
    public partial class Form1 : Form
    {
        public async Task Run()
        {
            // We require a custom configuration with JavaScript, CSS and the default loader
            var config = Configuration.Default
                                      //.WithJs()
                                      .WithCss()
                                      .WithDefaultLoader();

            // We create a new context
            var context = BrowsingContext.New(config);

            // The address we want to use
            //var address = Url.Create("http://html5test.com");
            var address = Url.Create("http://nginx.org");

            // Load the document
            var document = await context.OpenAsync(address);

            // Get the scored points
            var points = document.QuerySelector("#score > .pointsPanel > h2 > strong").TextContent;

            // Print it out
            Console.WriteLine("AngleSharp received {0} points form HTML5Test.com", points);
        }

        public Form1()
        {
            InitializeComponent();

            var c = new HtmlContent();
            c.LoadDocument();
        }

        Random rand = new Random(0);
        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var surface = e.Surface;
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
            Benchmark(100_000);
        }
    }
}
