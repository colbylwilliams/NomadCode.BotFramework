
namespace NomadCode.BotFramework
{
    public static class Colors
    {
#if __IOS__
        public static UIKit.UIColor MessageColor = UIKit.UIColor.FromRGBA (44f / 255f, 45f / 255f, 48f / 255f, 255f / 255f);
        public static UIKit.UIColor MessageLinkColor = UIKit.UIColor.FromRGBA (43f / 255f, 128f / 255f, 185f / 255f, 255f / 255f);
        public static UIKit.UIColor MessagePreColor = UIKit.UIColor.FromWhiteAlpha (0.9f, 0.9f);
#elif __ANDROID__

#endif
    }
}
