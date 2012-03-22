using Spritastic.Generator;
using Spritastic.Parser;
using Xunit;

namespace Spritastic.Facts.Generator
{
    public class SpritedImageFacts
    {
        public class InjectSprite
        {
            [Fact]
            public void WillReplaceFormerUrlWithSpriteUrlAndPositionOffset()
            {
                var css =
                    @"
.localnavigation {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}";
                var expected =
                    @"
.localnavigation {    
    background: url('spriteUrl') no-repeat 0 -30px;
    width: 50;
;background-position: -120px 0;}";
                var sprite = new SpritedImage(1, new BackgroundImageClass(css, 0), null) { Url = "spriteUrl", Position = 120 };

                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WillNotReplaceClassWithSameBodyAndDifferentSelector()
            {
                var css =
                    @"
.localnavigation {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}

.localnavigation2 {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}";
                var imageCss = @".localnavigation {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}";
                var expected =
                    @"
.localnavigation {    
    background: url('spriteUrl') no-repeat 0 -30px;
    width: 50;
;background-position: -120px 0;}

.localnavigation2 {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}";
                var sprite = new SpritedImage(1, new BackgroundImageClass(imageCss, 0), null) { Url = "spriteUrl", Position = 120 };

                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WillAddUrlWithSpriteUrlAndIfItIsNotInCss()
            {
                var css =
                    @"
.Localnavigation {    
    background-position: 0 -30px;
    width: 50;
}";
                var expected =
                    @"
.Localnavigation {    
    background-position: 0 -30px;
    width: 50;
;background-image: url('spriteUrl');background-position: -120px 0;}";
                var sprite = new SpritedImage(1, new BackgroundImageClass(css, 0) { ImageUrl = "nonRRsprite" }, null) { Url = "spriteUrl", Position = 120 };

                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WillDefaultYOffsetToZero()
            {
                var css =
                    @"
.localnavigation {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat;
    width: 50;
}";
                var expected =
                    @"
.localnavigation {    
    background: url('spriteUrl') no-repeat;
    width: 50;
;background-position: -120px 0;}";
                var sprite = new SpritedImage(1, new BackgroundImageClass(css, 0), null) { Url = "spriteUrl", Position = 120 };


                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expected, result);
            }

            [Fact]
            public void WillSetImageAbsoluteUrlFromBackgroundImageStyleAndReplaceRelativeUrl()
            {
                var css =
                    @"
.LocalNavigation .TabOn,.LocalNavigation .TabOn:hover {
    background-image: url(""subnav_on_technet.png"");
}";
                var expectedCss =
                    @"
.LocalNavigation .TabOn,.LocalNavigation .TabOn:hover {
    background-image: url(""newUrl"");
;background-position: -0px 0;}";
                var backgroundImage = new BackgroundImageClass(css, 0);
                var sprite = new SpritedImage(1, backgroundImage, null) { Url = "newUrl", Position = 0 };

                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expectedCss, result);
            }

            [Fact]
            public void WillAddImportanceDirectiveIfImportant()
            {
                var css =
                    @"
.localnavigation {    
    background: url('http://i1.social.microsoft.com/contentservice/798d3f43-7d1e-41a1-9b09-9dad00d8a996/subnav_technet.png') no-repeat 0 -30px;
    width: 50;
}";
                var expected =
                    @"
.localnavigation {    
    background: url('spriteUrl') no-repeat 0 -30px;
    width: 50;
;background-position: -120px 0 !important;}";
                var sprite = new SpritedImage(1, new BackgroundImageClass(css, 0) { Important = true }, null) { Url = "spriteUrl", Position = 120 };

                var result = sprite.InjectIntoCss(css);

                Assert.Equal(expected, result);
            }

        }
    }
}