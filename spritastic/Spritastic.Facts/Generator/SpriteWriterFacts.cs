﻿using System.Drawing;
using System.IO;
using Spritastic.Facts.Utilities;
using Spritastic.Generator;
using Xunit;

namespace Spriting.Facts
{
    public class SpriteWriterFacts
    {
        class TestableSpriteWriter : Testable<SpriteWriter>
        {
            public TestableSpriteWriter()
            {
                
            }
            public static Bitmap Image15X17 = CreateFileImage("testimages\\delete.png");
            public static Bitmap Image18X18 = CreateFileImage("testimages\\emptyStar.png");

            private static Bitmap CreateFileImage(string path)
            {
                return new Bitmap(new MemoryStream(File.ReadAllBytes(path)));
            }
        }

        public class Ctor
        {
            [Fact]
            public void WillCreateDrawingSurfaceWithDimensionsPassed()
            {
                var testable = new SpriteWriter(10, 20);

                Assert.Equal(10, testable.SpriteImage.Width);
                Assert.Equal(20, testable.SpriteImage.Height);
            }
        }

        public class WriteImage
        {
            [Fact]
            public void WillWriteImageToSurfaceAtTheCorrectOffset()
            {
                var testable = new SpriteWriter(35, 18);
                testable.WriteImage(TestableSpriteWriter.Image15X17);

                testable.WriteImage(TestableSpriteWriter.Image18X18);

                Assert.Equal(TestableSpriteWriter.Image18X18.GraphicsImage(), testable.SpriteImage.Clone(new Rectangle(16, 0, 18, 18), TestableSpriteWriter.Image18X18.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void WillWriteImageToSurfaceAtTheCorrectOffsetWhenProvidedExplicitOffsets()
            {
                var testable = new SpriteWriter(35, 28);

                testable.WriteImage(TestableSpriteWriter.Image18X18, 10, 5);

                Assert.Equal(TestableSpriteWriter.Image18X18.GraphicsImage(), testable.SpriteImage.Clone(new Rectangle(10, 5, 18, 18), TestableSpriteWriter.Image18X18.PixelFormat), new BitmapPixelComparer(true));
            }

        }
    }
}
