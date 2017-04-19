using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NomadCode.BotFramework.iOS
{
	public static class UIConstants
	{
		public static bool IS_LANDSCAPE => UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight;
		public static bool IS_IPAD => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
		public static bool IS_IPHONE => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
		public static bool IS_IPHONE4 => (IS_IPHONE && KeyWindowBounds.Height < 568.0);
		public static bool IS_IPHONE5 => (IS_IPHONE && KeyWindowBounds.Height == 568.0);
		public static bool IS_IPHONE6 => (IS_IPHONE && KeyWindowBounds.Height == 667.0);
		public static bool IS_IPHONE6PLUS => (IS_IPHONE && KeyWindowBounds.Height == 736.0 || KeyWindowBounds.Width == 736.0);// Both orientations
		public static bool IS_IOS8_AND_HIGHER => UIDevice.CurrentDevice.CheckSystemVersion (8, 0);
		public static bool IS_IOS9_AND_HIGHER => UIDevice.CurrentDevice.CheckSystemVersion (9, 0);

		//#define KEYBOARD_NOTIFICATION_DEBUG     DEBUG && 0  // Logs every keyboard notification being sent

		//#if __has_attribute(objc_designated_initializer)
		//#define DESIGNATED_INITIALIZER __attribute__((objc_designated_initializer))
		//#endif

		public static string TextViewControllerDomain = @"com.slack.TextViewController";

		/**
		 Returns a constant font size difference reflecting the current accessibility settings.
		 
		 @param category A content size category constant string.
		 @returns A float constant font size difference.
		 */
		public static nfloat PointSizeDifferenceForCategory (NSString category)
		{
			var contentSize = category.ToString ();

			switch (contentSize)
			{
				case "UICTContentSizeCategoryXS": return -3.0f;
				case "UICTContentSizeCategoryS": return -2.0f;
				case "UICTContentSizeCategoryM": return -1.0f;
				case "UICTContentSizeCategoryL": return 0.0f;
				case "UICTContentSizeCategoryXL": return 2.0f;
				case "UICTContentSizeCategoryXXL": return 4.0f;
				case "UICTContentSizeCategoryXXXL": return 6.0f;
				case "UICTContentSizeCategoryAccessibilityM": return 8.0f;
				case "UICTContentSizeCategoryAccessibilityL": return 10.0f;
				case "UICTContentSizeCategoryAccessibilityXL": return 11.0f;
				case "UICTContentSizeCategoryAccessibilityXXL": return 12.0f;
				case "UICTContentSizeCategoryAccessibilityXXXL": return 13.0f;
				default: return 0.0f;
			}
		}

		public static CGRect KeyWindowBounds => UIApplication.SharedApplication.KeyWindow.Bounds;

		public static CGRect RectInvert (CGRect rect) => new CGRect (rect.Y, rect.X, rect.Height, rect.Width);
	}
}
