

namespace NomadCode.BotFramework
{
	public static class Colors
	{
#if __IOS__
        public static UIKit.UIColor MessageColor = UIKit.UIColor.FromRGBA (44f / 255f, 45f / 255f, 48f / 255f, 255f / 255f);
        public static UIKit.UIColor MessageLinkColor = UIKit.UIColor.FromRGBA (43f / 255f, 128f / 255f, 185f / 255f, 255f / 255f);
        public static UIKit.UIColor MessagePreColor = UIKit.UIColor.FromWhiteAlpha (0.9f, 0.9f);
#elif __ANDROID__
		public static Android.Graphics.Color MessageColor = Android.Graphics.Color.Rgb (44 / 255, 45 / 255, 48 / 255);
		public static Android.Graphics.Color MessageLinkColor = Android.Graphics.Color.Rgb (43 / 255, 128 / 255, 185 / 255);
		public static Android.Graphics.Color MessagePreColor = Android.Graphics.Color.WhiteSmoke; //.Rgb (43 / 255, 128 / 255, 185 / 255);
#endif
	}
}
