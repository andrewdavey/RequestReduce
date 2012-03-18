using System.Collections.Generic;
using Moq;
using Spritastic.Facts.Utilities;
using Spritastic.Generator;
using Spritastic.ImageLoad;
using Spritastic.Parser;
using Spritastic.SpriteStore;
using Xunit;

namespace Spritastic.Facts
{
    public class SpriteGeneratorFacts
    {
        class TestableSpriteGenerator : Testable<SpriteGenerator>
        {
            public TestableSpriteGenerator()
            {
            }
        }

        public class GenerateFromCss
        {
            [Fact]
            public void WillFeedAllImagesIntoSpriteManager()
            {
                var testable = new TestableSpriteGenerator();
                var css ="fancy shmancy css";
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
    }
}
