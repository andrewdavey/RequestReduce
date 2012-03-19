using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Moq;
using Spritastic.Facts.Utilities;
using Spritastic.Generator;
using Spritastic.ImageLoad;
using Spritastic.Parser;
using Xunit;

namespace Spritastic.Facts
{
    public class SpriteGeneratorFacts
    {
        class TestableSpriteGenerator : Testable<SpriteGenerator>
        {
            public TestableSpriteGenerator()
            {
                AutoMockContainer.Configure(x => { x.SelectConstructor(() => new SpriteGenerator(null, null));
                                                     x.For<Func<IImageLoader, Func<byte[], string>, ISpriteManager>>().
                                                         Use(
                                                             (loader, urlGenerator) =>
                                                             AutoMockContainer.GetInstance<ISpriteManager>());
                });
            }
        }

        public class GenerateFromCss
        {
            [Fact]
            public void WillFeedAllImagesIntoSpriteManager()
            {
                var testable = new TestableSpriteGenerator();
                testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(new List<SpritedImage>().GetEnumerator());
                var css = "fancy shmancy css";
                var images = new List<BackgroundImageClass>
                                 {
                                     new BackgroundImageClass(
                                         @"
.localnavigation {    
    background: url('http://server/image1.png') no-repeat 0 0;
    width: 50;"
                                         , 1),
                                     new BackgroundImageClass(
                                         @"
.localnavigation2 {    
    background: url('http://server/image2.png') no-repeat 0 0;
    width: 50;
}"
                                         , 2)
                                 };
                testable.Mock<ICssImageTransformer>().Setup(x => x.ExtractImageUrls(css)).Returns(images);

                testable.ClassUnderTest.GenerateFromCss(css);

                testable.Mock<ISpriteManager>().Verify(x => x.Add(images[0]), Times.Exactly(1));
                testable.Mock<ISpriteManager>().Verify(x => x.Add(images[1]), Times.Exactly(1));
            }
        }

        [Fact]
        public void WillReturnInjectedCss()
        {
            var testable = new TestableSpriteGenerator();
            var sprite1 = new SpritedImage(1, null, new Bitmap(1, 1,PixelFormat.Format8bppIndexed)) { Position = -100 };
            var sprite2 = new SpritedImage(2, null, new Bitmap(1, 1, PixelFormat.Format8bppIndexed)) { Position = -100 };
            var sprites = new List<SpritedImage> { sprite1, sprite2 };
            testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(sprites.GetEnumerator());
            testable.Mock<ICssImageTransformer>().Setup(x => x.InjectSprite(It.IsAny<string>(), It.IsAny<SpritedImage>())).Returns<string, SpritedImage>((s,i) => s + "|sprited");

            var result = testable.ClassUnderTest.GenerateFromCss("css");

            Assert.Equal("css|sprited|sprited", result.GeneratedCss);
        }

        [Fact]
        public void WillReturnSprites()
        {
            var testable = new TestableSpriteGenerator();
            testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(new List<SpritedImage>().GetEnumerator());
            var sprite1 = new Sprite("url1", new byte[] {1});
            var sprite2 = new Sprite("url2", new byte[] {2});
            testable.Mock<ISpriteManager>().Setup(x => x.Flush()).Returns(new List<Sprite> { sprite1, sprite2 });

            var result = testable.ClassUnderTest.GenerateFromCss("css");

            Assert.Equal(sprite1, result.Sprites[0]);
            Assert.Equal(sprite2, result.Sprites[1]);
        }
    }
}
