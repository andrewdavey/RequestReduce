using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Moq;
using Spritastic.Facts.Utilities;
using Spritastic.Generator;
using Spritastic.Parser;
using Xunit;
using Xunit.Sdk;

namespace Spritastic.Facts
{
    public class SpriteGeneratorFacts
    {
        class TestableSpriteGenerator : Testable<SpriteGenerator>
        {
            public TestableSpriteGenerator()
            {
                AutoMockContainer.Configure(x => { x.SelectConstructor(() => new SpriteGenerator(null, null));
                                                     x.For<Func<string, ISpriteManager>>().
                                                         Use(cssPath => AutoMockContainer.GetInstance<ISpriteManager>());
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
                testable.Mock<ICssImageExtractor>().Setup(x => x.ExtractImageUrls(css)).Returns(images);

                testable.ClassUnderTest.GenerateFromCss(css, "http://server/test.css");

                testable.Mock<ISpriteManager>().Verify(x => x.Add(images[0]), Times.Exactly(1));
                testable.Mock<ISpriteManager>().Verify(x => x.Add(images[1]), Times.Exactly(1));
            }

            [Fact]
            public void WillReturnInjectedCss()
            {
                var testable = new TestableSpriteGenerator();
                var sprite1 = new Mock<SpritedImage>(1, null, new Bitmap(1, 1, PixelFormat.Format8bppIndexed));
                sprite1.Setup(s => s.InjectIntoCss(It.IsAny<string>())).Returns<string>(css => css + "|sprited");
                var sprite2 = new Mock<SpritedImage>(2, null, new Bitmap(1, 1, PixelFormat.Format8bppIndexed));
                sprite2.Setup(s => s.InjectIntoCss(It.IsAny<string>())).Returns<string>(css => css + "|sprited");
                
                var sprites = new List<SpritedImage> { sprite1.Object, sprite2.Object };
                testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(sprites.GetEnumerator());

                var result = testable.ClassUnderTest.GenerateFromCss("css", "http://server/test.css");

                Assert.Equal("css|sprited|sprited", result.GeneratedCss);
            }

            [Fact]
            public void WillReturnSprites()
            {
                var testable = new TestableSpriteGenerator();
                testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(new List<SpritedImage>().GetEnumerator());
                var sprite1 = new Sprite("url1", new byte[] { 1 });
                var sprite2 = new Sprite("url2", new byte[] { 2 });
                testable.Mock<ISpriteManager>().Setup(x => x.Flush()).Returns(new List<Sprite> { sprite1, sprite2 });

                var result = testable.ClassUnderTest.GenerateFromCss("css", "http://server/test.css");

                Assert.Equal(sprite1, result.Sprites[0]);
                Assert.Equal(sprite2, result.Sprites[1]);
            }

            [Fact]
            public void WillPassSpriteErrorsIntoResult()
            {
                var testable = new TestableSpriteGenerator();
                testable.Mock<ISpriteManager>().Setup(x => x.GetEnumerator()).Returns(new List<SpritedImage>().GetEnumerator());
                testable.Mock<ISpriteManager>().SetupGet(x => x.Errors).Returns(new List<SpriteException> { new SpriteException("my error", new EmptyException()) });

                var result = testable.ClassUnderTest.GenerateFromCss("css", "http://server/test.css");

                Assert.Equal("my error", result.Exceptions[0].Message);
            }

        }
    }
}
