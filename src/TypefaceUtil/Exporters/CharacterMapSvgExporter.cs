﻿using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;

namespace TypefaceUtil
{
    public static class CharacterMapSvgExporter
    {
        public static void Save(Dictionary<int, ushort> characterToGlyphMap, SKTypeface typeface, float textSize, string fill, StreamWriter streamWriter)
        {
            var skColor = new SKColor(0x00, 0x00, 0x00);

            using var skTextPaint = new SKPaint
            {
                IsAntialias = true,
                Color = skColor,
                Typeface = typeface,
                TextEncoding = SKTextEncoding.Utf32,
                TextSize = textSize,
                TextAlign = SKTextAlign.Center,
                LcdRenderText = true,
                SubpixelText = true
            };

            var metrics = skTextPaint.FontMetrics;
            var mAscent = metrics.Ascent;
            var mDescent = metrics.Descent;

            foreach (var kvp in characterToGlyphMap)
            {
                var charCode = kvp.Key;
                var utf32 = Char.ConvertFromUtf32((int)charCode);
                float x = 0;
                float y = (mAscent / 2.0f) - mDescent / 2.0f;

                using var outlinePath = skTextPaint.GetTextPath(utf32, x, y);
                using var fillPath = skTextPaint.GetFillPath(outlinePath);

                fillPath.Transform(SKMatrix.MakeTranslation(-fillPath.Bounds.Left, -fillPath.Bounds.Top));

                var bounds = fillPath.Bounds;
                var svgPathData = fillPath.ToSvgPathData();

                streamWriter.WriteLine($"[{utf32}]");
                streamWriter.WriteLine($"<svg viewBox=\"{bounds.Left} {bounds.Top} {bounds.Width} {bounds.Height}\" xmlns=\"http://www.w3.org/2000/svg\">"); // width=\"{bounds.Width}\" height=\"{bounds.Height}\"
                streamWriter.WriteLine($"  <path fill=\"{fill}\" d=\"{svgPathData}\"/>");
                streamWriter.WriteLine($"</svg>");
            }
        }
    }
}
