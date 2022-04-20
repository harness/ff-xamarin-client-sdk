# Xamarin CF Android and iOS SDK

[Harness](https://www.harness.io/) is a feature management platform that helps teams to build better software and to test features quicker.

## Setup

Xamarin client uses Harness Binding library NuGet package for iOS and Android application.

To reference Android binding library add package:

```
Install-Package ff-android-xamarin-client-sdk -Version 0.5.2
```

Note: To be able to use the Android binding library the Google GSON bindings must be provided:

```
Install-Package GoogleGson -Version 2.8.5
```

To reference iOS binding library add package:
```
Install-Package ff-ios-xamarin-client-sdk -Version 0.5.0
```

## Usage

### iOS library

Library Initialization snippet

```
    // Create configuration:
    var config = new CfConfigurationProxy
    {
        StreamEnabled = true,
        AnalyticsEnabled = true
    };

    // Set selected identifer:
    var target = new CfTargetProxy
    {
        Identifier = "target_identifier",
        Name = "target_name"
    };

    // Initialize authentication. Update API_KEY with your key:
    CfClientProxy.Shared.InitializeWithApiKey(API_KEY, config, target);
```

Evaluating the flag values:

```
bool flag1 = CfClient.Instance.BoolVariation("flag1", false);
```

Available public variation methods are:

- BoolVariation
- NumberVariation
- StringVariation
- JsonVariation

Each accepts the flag name and the default value as the method arguments.

Subscribing on receiving library events:

```
using System;
using ff_ios_client_sdk_proxy;

public class CfListener : CfClientDelegate
{

    public CfListener()
    {
        // Subscribe on getting events from native iOS library
        CfClientProxy.Shared.Delegate = this;
    }
    // Received in case of error
    public override void OnErrorWithError(CfErrorProxy error){}

    // Contains array with flags received each pooling interval
    public override void OnPollingEventReceivedWithEvaluations(CxEvaluation[] evaluations){}

    // Event when flag value is changed
    public override void OnStreamEventReceivedWithEvaluation(CxEvaluation evaluation){}

    // Message received from library event
    public override void OnMessageReceivedWithMessage(CxMessage message){}

    // On Stream opened event
    public override void OnStreamOpened() {}

    // On Stream closed event
    public override void OnStreamCompleted() {}
}
```

### Android library

Library Initialization snippet:

```
    CfConfiguration configuration = new CfConfiguration.Builder()
        .EnableStream(true)
        .EnableAnalytics(true)
        .Build();

    Target target = new Target()
        .InvokeName(account)
        .InvokeIdentifier(account);

     // Initialize authentication. Update API_KEY with your key.
     // Listener object should implement IAuthCallback interface:
    CfClient.Instance.Initialize(this.context, API_KEY, configuration, target, listener);
```

Evaluating the flag values:

```
bool flag1 = CfClient.Instance.BoolVariation("flag1", false);
```

Available public variation methods are:

- BoolVariation
- NumberVariation
- StringVariation
- JsonVariation

Each accepts the flag name and the default value as the method arguments.

Subscribing on receiving library events:

```
using System;
using System.Linq;
using IO.Harness.Cfsdk.Cloud.Events;
using IO.Harness.Cfsdk.Cloud.Model;
using IO.Harness.Cfsdk.Cloud.Oksse;
using IO.Harness.Cfsdk.Cloud.Oksse.Model;
using Java.Interop;
using Java.Util;
using IO.Harness.Cfsdk;

public class CfListener : Java.Lang.Object, IAuthCallback, IEventsListener
{

    public CfListener()
    {
         // Subscribe on getting events from native Android library
        CfClient.Instance.RegisterEventsListener(this);
    }
    public void AuthorizationSuccess(AuthInfo p0, AuthResult p1)
    {
        // p1.Success contains status of authorization
        // In case of error p1.Error contains error message
    }
    public void OnEventReceived(StatusEvent p0)
    {
        var eventType = p0.EventType;
        if(StatusEvent.EVENT_TYPE.SseStart == eventType)
        {
            // Stream started
        }
        else if( StatusEvent.EVENT_TYPE.SseEnd == eventType)
        {
            // Stream Ended
        }
        else if (StatusEvent.EVENT_TYPE.EvaluationChange == eventType)
        {
            Java.Lang.Object payload = p0.ExtractPayload();
            var ev = payload as IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation;
            // Flag changed event
        }
        else if (StatusEvent.EVENT_TYPE.EvaluationReload == eventType)
        {
            Java.Lang.Object payload = p0.ExtractPayload();

            var t = payload.JavaCast<ArrayList>();
            var arr = t.ToEnumerable<IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation>().ToArray();

            // Each pulling interval we will receive array of available flags.

        }
    }
}
```
# Instructions on generating iOS and Android Xamaring Binding SDK

## iOS

1. Generating wrapper framework arround ff_ios_client_sdk library.

Open project `ff-ios-xamarin-client-sdk-binding/ff-ios-client-sdk-proxy/ff-ios-client-sdk-proxy.xcodeproj` in XCode.

Go to `File->Packages->Update` to Latest Package Versions to pick the latest iOS client sdk. Observer version of ff-ios-client-sdk, and make sure it mathces with the latest on published to iOS Swift Package Manager.

Update version of ff-ios-client-sdk-proxy by clicking on project file in XCode, selecting proper target and updating Version and Buid number from General tab. Suggestion is to keep this value in sync with value of iOS Xamarin binding published through nuget.



To build a library use build.sh script located in `ff-ios-xamarin-client-sdk-binding/ff-ios-client-sdk-proxy` folder. Before launching make sure that sharpie tool is installed from https://aka.ms/objective-sharpie

After being launched, script will:
* Build library for both simulator and device architecture, and create a fat library. Output build is located at ff-ios-xamarin-client-sdk-binding/ff-ios-client-sdk-proxy/build
* Run sharpie tool, which generates .NET ApiDefinitions.cs file which is used in binding proces. In case if there is no changes in interface, ApiDefinitions.cs will stay the same. NOTE: After running sharpie, user need manuall to add base interface INativeObject to CxValue class.

<br/>
<br/>
2. Creating iOS Xamarin binding SDK

Open Solution file ff-xamarin-client-sdk-binding.sln and open Options of ff-ios-xamarin-client-sdk-binding project. In NuGet Package section, change Version and details related with nuget package.

Select Release configuration, then right clik on ff-ios-xamarin-client-sdk-binding project and select "Create Nuget Package". As a result nuget package will be generated at `ff-ios-xamarin-client-sdk-binding/bin/Release/ff-ios-xamarin-client-sdk.x.y.z.nupkg`, where x.y.z corresponds to version entered in NuGet Package section.


3. Publishing package.

Package is published from command line by running

__nuget push ff-ios-xamarin-client-sdk.x.y.z api_key  -Source https://api.nuget.org/v3/index.json__

from location where nuget package is generated. api_key should be acquired on nuget platform.

## Android

1. Adding new Android library to binding project.

Get proper Android FF SDK binary library (ff-android-client-sdk-1.0.9.aar), and copy to Jars folder. Delete older version.

Open solution file, and from project file in Jars folder, delete old .aar file, and using Add->New File menu add new library. Select file and from properties menu make sure that build action "LibraryProjectZip" is selected.

2. Creating Android Xamarin binding SDK

Open Solution file ff-xamarin-client-sdk-binding.sln and open Options of ff-android-xamarin-client-sdk-binding project. In NuGet Package section, change Version and details related with nuget package.


Select Release configuration, then right clik on ff-android-xamarin-client-sdk-binding project and select "Create Nuget Package". As a result nuget package will be generated at `ff-android-xamarin-client-sdk-binding/bin/Release/ff-android-xamarin-client-sdk.x.y.z.nupkg`, where x.y.z corresponds to version entered in NuGet Package section.


3. Publishing package.

Same as for iOS, package is published from command line by running

__nuget push ff-android-xamarin-client-sdk.x.y.z api_key  -Source https://api.nuget.org/v3/index.json__

from location where nuget package is generated. api_key should be acquired on nuget platform.
