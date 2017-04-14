# NomadCode.BotFramework

`NomadCode.BotFramework` is a simple library to make working with [Microsoft's Bot Framework][0] on iOS and Android much easier.

Instead of polling GET requests, the library implements WebSockets using [SocketRocket][1] on iOS, and [OkHttp3][2] on Android.

# Installation

To make it easy to use anywhere, `NomadCode.BotFramework` is a [shared project][3] with only a few files.  

To use in your project, you can either, clone, [download][4], etc. and reference your local copy int your project.    

Or, if you're using Git, you can add it as a submodule with the following command:

```shell
cd /path/to/your/projects/root

git submodule add https://github.com/colbylwilliams/NomadCode.BotFramework NomadCode.BotFramework
```

# Use

NomadCode.BotFramework only contains a few classes, you'll primarily work with: `BotClient` and `BotMessage`.

`BotClient` is the class you'll use to interact with your bot. It handles sending messages to, and receiving messages from the [Direct Line][5] channel, including managing the state of the web socket, authentication, etc.

`BotMessage` is a wrapper around the incoming and outgoing [Activities][6] that make up a conversation, and contains additional metadata useful for displaying rich message formats in your app UI.



[0]:https://dev.botframework.com/
[1]:https://github.com/facebook/SocketRocket
[2]:https://github.com/square/okhttp

[3]:https://developer.xamarin.com/guides/cross-platform/application_fundamentals/shared_projects/
[4]:https://github.com/colbylwilliams/NomadCode.BotFramework/archive/master.zip

[5]:https://docs.botframework.com/en-us/restapi/directline3/
[6]:https://docs.botframework.com/en-us/core-concepts/reference/#activity


[10]:https://github.com/colbylwilliams/NomadCode.Azure/blob/master/LICENSE
[11]:https://github.com/colbylwilliams

[12]:https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-how-to-use-client-library#serverflow
[13]:https://msdn.microsoft.com/library/azure/microsoft.windowsazure.mobileservices.mobileserviceauthenticationprovider(v=azure.10).aspx