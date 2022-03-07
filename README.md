# Instructions on generating iOS and Android Xamaring Binding SDK 

## iOS 

1. Generating wrapper framework arround ff_ios_client_sdk library.

Open project `ff-ios-xamarin-client-sdk-binding/ff-ios-client-sdk-proxy/ff-ios-client-sdk-proxy.xcodeproj` in XCode.

Go to `File->Packages->Update` to Latest Package Versions to pick the latest iOS client sdk. Observer version of ff-ios-client-sdk, and make sure it mathces with the latest on published to iOS Swift Package Manager.

Update version of ff-ios-client-sdk-proxy by clicking on project file in XCode, selecting proper target and updating Version and Buid number from General tab. Suggestion is to keep this value in sync with value of iOS Xamarin binding published through nuget.

To build a library use build.sh script located in `ff-ios-xamarin-client-sdk-binding/ff-ios-client-sdk-proxy` folder. After being launched, script will:
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

Get proper Android FF SDK binary library (ff-android-client-sdk-1.0.7.aar), and copy to Jars folder. Delete older version.

Open solution file, and from project file in Jars folder, delete old .aar file, and using Add->New File menu add new library. Select file and from properties menu make sure that build action "LibraryProjectZip" is selected.

2. Creating Android Xamarin binding SDK

Open Solution file ff-xamarin-client-sdk-binding.sln and open Options of ff-android-xamarin-client-sdk-binding project. In NuGet Package section, change Version and details related with nuget package. 


Select Release configuration, then right clik on ff-android-xamarin-client-sdk-binding project and select "Create Nuget Package". As a result nuget package will be generated at `ff-android-xamarin-client-sdk-binding/bin/Release/ff-android-xamarin-client-sdk.x.y.z.nupkg`, where x.y.z corresponds to version entered in NuGet Package section.


3. Publishing package.

Same as for iOS, package is published from command line by running

__nuget push ff-android-xamarin-client-sdk.x.y.z api_key  -Source https://api.nuget.org/v3/index.json__

from location where nuget package is generated. api_key should be acquired on nuget platform.