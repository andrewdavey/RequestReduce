﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Moq;
using Spritastic;
using Spritastic.Generator;
using Spritastic.Parser;
using Xunit;
using Xunit.Extensions;
using System;

namespace Spriting.Facts
{
    public class SpriteContainerFacts
    {
        class TestableSpriteContainer
        {
            public SpriteContainer ClassUnderTest { get; set; }
            public SpritingSettings Settings { get; set; }
            public Dictionary<string, byte[]> Images { get; private set; }
            public int DownloadCount { get; set; }

            public TestableSpriteContainer()
            {
                Settings = new SpritingSettings
                {
                    IsFullTrust = true
                };
                Images = new Dictionary<string, byte[]>();
                ClassUnderTest = new SpriteContainer(DownloadImage, Settings);
            }

            byte[] DownloadImage(string url)
            {
                DownloadCount++;
                return Images[url];
            }

            public byte[] Image15X17 = File.ReadAllBytes(string.Format("{0}\\testimages\\delete.png", AppDomain.CurrentDomain.BaseDirectory));
            public byte[] Image18X18 = File.ReadAllBytes(string.Format("{0}\\testimages\\emptyStar.png", AppDomain.CurrentDomain.BaseDirectory));

            public static byte[] GetFiveColorImage()
            {
                using(var bitmap = new Bitmap(3, 3, PixelFormat.Format32bppArgb))
                {
                    bitmap.SetPixel(1, 1, Color.Tomato);
                    bitmap.SetPixel(1, 2, Color.Wheat);
                    bitmap.SetPixel(2, 1, Color.Violet);
                    bitmap.SetPixel(2, 2, Color.Teal);
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        return stream.GetBuffer();
                    }
                }
            }

            public static byte[] GetFourColorImage()
            {
                using (var bitmap = new Bitmap(2, 2, PixelFormat.Format32bppArgb))
                {
                    bitmap.SetPixel(1, 1, Color.Turquoise);
                    bitmap.SetPixel(1, 0, Color.DeepSkyBlue);
                    bitmap.SetPixel(0, 1, Color.Violet);
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        return stream.GetBuffer();
                    }
                }
            }

            public static byte[] GetHalfvioletHalfGreyImageImage(Color darkViolet)
            {
                using (var bitmap = new Bitmap(2, 2, PixelFormat.Format32bppArgb))
                {
                    bitmap.SetPixel(1, 1, Color.DarkViolet);
                    bitmap.SetPixel(1, 0, Color.DarkViolet);
                    bitmap.SetPixel(0, 0, Color.DimGray);
                    bitmap.SetPixel(0, 1, Color.DimGray);
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        return stream.GetBuffer();
                    }
                }
            }
        }

        public class AddImage
        {
            [Fact]
            public void SizeWillBeAggregateOfAddedImages()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) {ImageUrl = "url1"};
                var image2 = new BackgroundImageClass("", 0) {ImageUrl = "url2"};
                testable.Images["url1"] = testable.Image15X17;
                testable.Images["url2"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.Equal(testable.Image15X17.Length + testable.Image18X18.Length, testable.ClassUnderTest.Size);
            }

            [Fact]
            public void RightPositionedImagesWillBeRightAlligned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, XOffset = new Position(){PositionMode = PositionMode.Direction, Direction = Direction.Right}};
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image15X17));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(15, 0, 15, 17), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void RightPositionedImagesLargerThanWidthWillBeRightAlligned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Right } };
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image15X17));
                Assert.Equal(image2.Clone(new Rectangle(5,0,10,17), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(0, 0, 10, 17), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void BottomPositionedImagesWillBeBottomAlligned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, ExplicitHeight = 30, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Right }, YOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Bottom } };
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image15X17));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(15, 13, 15, 17), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void BottomPositionedImagesLargerThanHeightWillBeBottomAlligned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, ExplicitHeight = 10, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Right }, YOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Bottom } };
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image15X17));
                Assert.Equal(image2.Clone(new Rectangle(5, 7, 10, 10), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(0, 0, 10, 10), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void HorizontalyCenteredImagesWillBeCenteredInClonedImageSentToWriter()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(6, 0, 18, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void HorizontalyCenteredImagesLargerThanWidthWillBeCentered()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.Clone(new Rectangle(4, 0, 10, 18), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(0, 0, 10, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void VerticallyCenteredImagesWillBeCenteredInClonedImageSentToWriter()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, ExplicitHeight = 30, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center }, YOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(6, 6, 18, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void VerticallyCenteredImagesLargerThanWidthWillBeCentered()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, ExplicitHeight = 10, XOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center }, YOffset = new Position() { PositionMode = PositionMode.Direction, Direction = Direction.Center } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.Clone(new Rectangle(4, 4, 10, 10), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(0, 0, 10, 10), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void PercentageImagesWillBecorrectlyPositionedInClonedImageSentToWriter()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, ExplicitHeight = 30, XOffset = new Position() { PositionMode = PositionMode.Percent, Offset = 33}, YOffset = new Position() { PositionMode = PositionMode.Percent, Offset = 33} };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(4, 4, 18, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void PercentagedImagesLargerThanWidthWillBeCorrectlyPositioned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, ExplicitHeight = 10, XOffset = new Position() { PositionMode = PositionMode.Percent, Offset = 40}, YOffset = new Position() { PositionMode = PositionMode.Percent, Offset = 40} };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.Clone(new Rectangle(3, 3, 10, 10), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(0, 0, 10, 10), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void PositivelyYOffsetImagesWillNotBeOffsetInClonedImageSentToWriter()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, ExplicitHeight = 30, XOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 5 }, YOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 10 } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(5, 0, 18, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void PositivelyXOffsetImagesWillBecorrectlyPositionedInClonedImageSentToWriter()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 30, ExplicitHeight = 30, XOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 5 }, YOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 0 } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.GraphicsImage(), image.Image.Clone(new Rectangle(5, 0, 18, 18), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void PositivelyXOffsetImagesLargerThanWidthWillBeCorrectlyPositioned()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, ExplicitHeight = 10, XOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 2 }, YOffset = new Position() { PositionMode = PositionMode.Unit, Offset = 0 } };
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                var image = testable.ClassUnderTest.First();
                var image2 = new Bitmap(new MemoryStream(testable.Image18X18));
                Assert.Equal(image2.Clone(new Rectangle(0, 0, 8, 7), image2.PixelFormat).GraphicsImage(), image.Image.Clone(new Rectangle(2, 0, 8, 7), image2.PixelFormat), new BitmapPixelComparer(true));
            }

            [Fact]
            public void WidthWillBeAggregateOfAddedImageWidthsPlusOnePixelEach()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) {ImageUrl = "url1"};
                var image2 = new BackgroundImageClass("", 0) {ImageUrl = "url2"};
                testable.Images["url1"] = testable.Image15X17;
                testable.Images["url2"] = testable.Image18X18;;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.Equal(35, testable.ClassUnderTest.Width);
            }

            [Theory,
             InlineData(10),
             InlineData(20)]
            public void WidthWillBeSizeOfBackgroundClassPluOneIfDifferentThanImageWidth(int width)
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {ImageUrl = "url1", ExplicitWidth = width};
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(width + 1, testable.ClassUnderTest.Width);
            }

            [Fact]
            public void WillAutoCorrectWidthIfWidthAndOffsetAreGreaterThanOriginal()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 10, XOffset = new Position(){PositionMode = PositionMode.Unit, Offset = -10}};
                testable.Images["url1"]= testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(11, testable.ClassUnderTest.Width);
            }

            [Fact]
            public void WillAutoCorrectHeightIfHeightAndOffsetAreGreaterThanOriginal()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1", ExplicitWidth = 15, ExplicitHeight = 10, YOffset = new Position() { PositionMode = PositionMode.Unit, Offset = -10 } };
                testable.Images["url1"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(10, testable.ClassUnderTest.Height);
            }

            [Fact]
            public void WillClipLeftEdgeOfBackgroundClassWhenOffsetIsNegative()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {
                                     ImageUrl = "url1",
                                     ExplicitWidth = 5,
                                     XOffset = new Position {PositionMode = PositionMode.Unit, Offset = -5}
                                 };
                testable.Images["url1"] = testable.Image15X17;
                var bitMap = new Bitmap(new MemoryStream(testable.Image15X17));

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(bitMap.Clone(new Rectangle(5, 0, 5, 17), bitMap.PixelFormat).GraphicsImage(),
                             testable.ClassUnderTest.First().Image, new BitmapPixelComparer(true));
            }

            [Fact]
            public void WillNotClipLeftEdgeOfBackgroundClassWhenOffsetIsPositive()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {
                                     ImageUrl = "url1",
                                     ExplicitWidth = 5,
                                     XOffset = new Position() {PositionMode = PositionMode.Percent, Offset = 0}
                                 };
                testable.Images["url1"] = testable.Image15X17;
                var bitMap = new Bitmap(new MemoryStream(testable.Image15X17));

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(bitMap.Clone(new Rectangle(0, 0, 5, 17), bitMap.PixelFormat).GraphicsImage(),
                             testable.ClassUnderTest.First().Image, new BitmapPixelComparer(true));
            }

            [Fact]
            public void WillClipUpperEdgeOfBackgroundClassWhenOffsetIsNegative()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {
                                     ImageUrl = "url1",
                                     ExplicitHeight = 5,
                                     YOffset = new Position() {PositionMode = PositionMode.Unit, Offset = -5}
                                 };
                testable.Images["url1"] = testable.Image15X17;
                var bitMap = new Bitmap(new MemoryStream(testable.Image15X17));

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(bitMap.Clone(new Rectangle(0, 5, 15, 5), bitMap.PixelFormat).GraphicsImage(),
                             testable.ClassUnderTest.First().Image, new BitmapPixelComparer(true));
            }

            [Fact]
            public void WillNotClipUpperEdgeOfBackgroundClassWhenOffsetIsPositive()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {
                                     ImageUrl = "url1",
                                     ExplicitHeight = 5,
                                     YOffset = new Position() {PositionMode = PositionMode.Percent, Offset = 0}
                                 };
                testable.Images["url1"] = testable.Image15X17;
                var bitMap = new Bitmap(new MemoryStream(testable.Image15X17));

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(bitMap.Clone(new Rectangle(0, 0, 15, 5), bitMap.PixelFormat).GraphicsImage(),
                             testable.ClassUnderTest.First().Image, new BitmapPixelComparer(true));
            }

            [Theory,
             InlineData(10),
             InlineData(20)]
            public void HeightWillBeSizeOfBackgroundClassIfDifferentThanImageHeight(int height)
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0)
                                 {ImageUrl = "url1", ExplicitHeight = height};
                testable.Images["url1"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);

                Assert.Equal(height, testable.ClassUnderTest.Height);
            }

            [Fact]
            public void HeightWillBeTheTallestOfAddedImages()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) {ImageUrl = "url1"};
                var image2 = new BackgroundImageClass("", 0) {ImageUrl = "url2"};
                testable.Images["url1"] = testable.Image15X17;
                testable.Images["url2"] = testable.Image18X18;;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.Equal(18, testable.ClassUnderTest.Height);
            }

            [Fact]
            public void WillCountColorsOfAddedImage()
            {
                var testable = new TestableSpriteContainer();
                var fiveColorImage = new BackgroundImageClass("image1", 0){ImageUrl = "url"};
                testable.Images["url"] = TestableSpriteContainer.GetFiveColorImage();

                testable.ClassUnderTest.AddImage(fiveColorImage);

                Assert.Equal(5, testable.ClassUnderTest.Colors);
            }

            [Fact]
            public void ColorCountWillBe0InRestrictedTrust()
            {
                var testable = new TestableSpriteContainer();
                var fiveColorImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url" };
                testable.Images["url"] = TestableSpriteContainer.GetFiveColorImage();
                testable.Settings.IsFullTrust = false;

                testable.ClassUnderTest.AddImage(fiveColorImage);

                Assert.Equal(0, testable.ClassUnderTest.Colors);
            }

            [Fact]
            public void WillCountUniqueColorsOfAddedImages()
            {
                var testable = new TestableSpriteContainer();
                var fiveColorImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url" };
                testable.Images["url"] = TestableSpriteContainer.GetFiveColorImage();
                var fourColorImage = new BackgroundImageClass("image2", 0) { ImageUrl = "url2" };
                testable.Images["url2"] = TestableSpriteContainer.GetFourColorImage();

                testable.ClassUnderTest.AddImage(fiveColorImage);
                testable.ClassUnderTest.AddImage(fourColorImage);

                Assert.Equal(7, testable.ClassUnderTest.Colors);
            }

            [Fact]
            public void UniqueColorsOfAddedImagesWillBe0WhenNotInFullTrust()
            {
                var testable = new TestableSpriteContainer();
                var fiveColorImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url" };
                testable.Images["url"] = TestableSpriteContainer.GetFiveColorImage();
                var fourColorImage = new BackgroundImageClass("image2", 0) { ImageUrl = "url2" };
                testable.Images["url2"] = TestableSpriteContainer.GetFourColorImage();
                testable.Settings.IsFullTrust = false;

                testable.ClassUnderTest.AddImage(fiveColorImage);
                testable.ClassUnderTest.AddImage(fourColorImage);

                Assert.Equal(0, testable.ClassUnderTest.Colors);
            }

            [Fact]
            public void WillCalculateAverageColorsOfAddedImages()
            {
                var testable = new TestableSpriteContainer();
                var halfvioletHalfGreyImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url" };
                testable.Images["url"] = TestableSpriteContainer.GetHalfvioletHalfGreyImageImage(Color.DarkViolet);
                var color1 = Color.DarkViolet.ToArgb();
                var color2 = Color.DimGray.ToArgb();

                var result = testable.ClassUnderTest.AddImage(halfvioletHalfGreyImage);

                Assert.Equal((color1+color2)/2, result.AverageColor);
            }

            [Fact]
            public void AverageColorsOfAddedImagesWillBe0WhenNotInFullTrust()
            {
                var testable = new TestableSpriteContainer();
                var halfvioletHalfGreyImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url" };
                testable.Images["url"] = TestableSpriteContainer.GetHalfvioletHalfGreyImageImage(Color.DarkViolet);
                testable.Settings.IsFullTrust = false;

                var result = testable.ClassUnderTest.AddImage(halfvioletHalfGreyImage);

                Assert.Equal(0, result.AverageColor);
            }

            [Fact]
            public void WillNotDownloadSameUrlTwice()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("image1", 0) { ImageUrl = "url", IsSprite = true};
                var image2 = new BackgroundImageClass("image2", 0) { ImageUrl = "url", IsSprite = true };
                testable.Images["url"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.Equal(1, testable.DownloadCount);
            }

            [Fact]
            public void WillNotCacheNonSprites()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("image1", 0) { ImageUrl = "url", IsSprite = true };
                var image2 = new BackgroundImageClass("image2", 0) { ImageUrl = "url2", IsSprite = false };
                testable.Images["url"] = testable.Image15X17;
                testable.Images["url2"] = testable.Image15X17;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.True(testable.ClassUnderTest.DownloadedImages.ContainsKey("url"));
                Assert.False(testable.ClassUnderTest.DownloadedImages.ContainsKey("url2"));
            }

            [Fact]
            public void WillThrowInvalidOperationExceptionIfCloningThrowsOutOfMemory()
            {
                var testable = new TestableSpriteContainer();
                var fiveColorImage = new BackgroundImageClass("image1", 0) { ImageUrl = "url", ExplicitWidth = 15, XOffset = new Position() { Offset = -16, Direction = Direction.Left } };
                testable.Images["url"] = testable.Image15X17;

                var ex = Record.Exception(() => testable.ClassUnderTest.AddImage(fiveColorImage)) as InvalidOperationException;

                Assert.NotNull(ex);
                Assert.Contains("url", ex.Message);
            }


        }

        public class Enumerator
        {
            [Fact]
            public void WillReturnAllImages()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new BackgroundImageClass("", 0) { ImageUrl = "url1" };
                var image2 = new BackgroundImageClass("", 0) { ImageUrl = "url2" };
                testable.Images["url1"] = testable.Image15X17;
                testable.Images["url2"] = testable.Image18X18;

                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                Assert.Contains(image1, testable.ClassUnderTest.Select(x => x.CssClass));
                Assert.Contains(image2, testable.ClassUnderTest.Select(x => x.CssClass));
            }

            [Fact]
            public void WillReturnAllImagesOrderedByColor()
            {
                var testable = new TestableSpriteContainer();
                var image1 = new SpritedImage(20, null, null);
                var image2 = new SpritedImage(10, null, null);
                testable.ClassUnderTest.AddImage(image1);
                testable.ClassUnderTest.AddImage(image2);

                var results = testable.ClassUnderTest.ToArray();

                Assert.Equal(image1, results[1]);
                Assert.Equal(image2, results[0]);
            }
        }
    }
}
