#if __IOS__ || __ANDROID__

using System;
using System.Collections.Generic;

#if __IOS__
using static Foundation.NSUserDefaults;
#elif __ANDROID__
using Android.App;
using Android.Preferences;
#endif

namespace NomadCode.BotFramework
{
    public partial class BotClient // preference store
    {

        const string hyphen = "-";

        const string currentUserIdStorageKey = "NomadCodeBotFrameworkCurrentUserIdStorageKey";

        const string currentUserNameStorageKey = "NomadCodeBotFrameworkCurrentUserNameStorageKey";

        const string currentUserEmailStorageKey = "NomadCodeBotFrameworkCurrentUserEmailStorageKey";

        const string currentUserAvatarStorageKey = "NomadCodeBotFrameworkCurrentUserAvatarStorageKey";

        const string conversationIdStorageKey = "NomadCodeBotFrameworkConversationIdStorageKey";

        const string resetConversationStorageKey = "NomadCodeBotFrameworkResetConversationStorageKey";


#if __IOS__

        void setSetting (string key, bool value) => StandardUserDefaults.SetBool (value, key);

        void setSetting (string key, string value) => StandardUserDefaults.SetString (value, key);

        void setSetting (string key, DateTime value) => setSetting (key, value.ToString ());


        bool boolForKey (string key) => StandardUserDefaults.BoolForKey (key);

        string stringForKey (string key) => StandardUserDefaults.StringForKey (key);


#elif __ANDROID__

        void setSetting (string key, bool value)
        {
            using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (Application.Context))
            using (var sharedPreferencesEditor = sharedPreferences.Edit ())
            {
                sharedPreferencesEditor.PutBoolean (key, value);
                sharedPreferencesEditor.Commit ();
            }
        }

        void setSetting (string key, string value)
        {
            using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (Application.Context))
            using (var sharedPreferencesEditor = sharedPreferences.Edit ())
            {
                sharedPreferencesEditor.PutString (key, value);
                sharedPreferencesEditor.Commit ();
            }
        }

        void setSetting (string key, DateTime value)
        {
            using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (Application.Context))
            using (var sharedPreferencesEditor = sharedPreferences.Edit ())
            {
                sharedPreferencesEditor.PutString (key, value.ToString ());
                sharedPreferencesEditor.Commit ();
            }
        }


        bool boolForKey (string key)
        {
            using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (Application.Context))
                return sharedPreferences.GetBoolean (key, false);
        }

        string stringForKey (string key)
        {
            using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (Application.Context))
                return sharedPreferences.GetString (key, string.Empty);
        }

#endif

        DateTime dateTimeForKey (string key) => DateTime.TryParse (stringForKey (key), out DateTime outDateTime) ? outDateTime : default (DateTime);

        public static Dictionary<string, string> UserImageDictionary { get; set; }

        public void ResetCurrentUser ()
        {
            CurrentUserId = string.Empty;
            CurrentUserName = string.Empty;
            CurrentUserEmail = string.Empty;
        }

        public string CurrentUserId
        {
            get => stringForKey (currentUserIdStorageKey);
            set => setSetting (currentUserIdStorageKey, value ?? string.Empty);
        }

        public string CurrentUserName
        {
            get => stringForKey (currentUserNameStorageKey);
            set => setSetting (currentUserNameStorageKey, value ?? string.Empty);
        }

        public string CurrentUserEmail
        {
            get => stringForKey (currentUserEmailStorageKey);
            set => setSetting (currentUserEmailStorageKey, value ?? string.Empty);
        }

        public string GetAvatarUrl (string sid) => stringForKey ($"currentUserAvatarStorageKey{sid}");

        public void SetAvatarUrl (string sid, string url) => setSetting ($"currentUserAvatarStorageKey{sid}", url ?? string.Empty);

        public string ConversationId
        {
            get => stringForKey (conversationIdStorageKey);
            set => setSetting (conversationIdStorageKey, value ?? string.Empty);
        }

        public bool ResetConversation
        {
            get
            {
                var reset = boolForKey (resetConversationStorageKey);

                if (reset) // only return true once
                {
                    setSetting (resetConversationStorageKey, false);
                }

                return reset;
            }
            set => setSetting (resetConversationStorageKey, value);
        }


        public bool HasValidCurrentUser => !(string.IsNullOrWhiteSpace (CurrentUserId) || string.IsNullOrWhiteSpace (CurrentUserName));
    }
}

#endif