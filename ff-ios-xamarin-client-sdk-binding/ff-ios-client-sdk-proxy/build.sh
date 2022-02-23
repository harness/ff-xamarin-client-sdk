rm -r build/*
rm -f ApiDefinitions.cs

xcodebuild archive -sdk iphoneos -scheme ff-ios-client-sdk-proxy  -archivePath build/ff-ios-client-sdk-proxy-iphoneos.xcarchive -configuration Release SKIP_INSTALL=NO
xcodebuild archive -sdk iphonesimulator -scheme ff-ios-client-sdk-proxy  -archivePath build/ff-ios-client-sdk-proxy-simulator.xcarchive -configuration Release SKIP_INSTALL=NO

cp -R build/ff-ios-client-sdk-proxy-iphoneos.xcarchive/Products/Library/Frameworks/ff_ios_client_sdk_proxy.framework build/ff_ios_client_sdk_proxy_iphoneos.framework
cp -R build/ff-ios-client-sdk-proxy-simulator.xcarchive/Products/Library/Frameworks/ff_ios_client_sdk_proxy.framework build/ff_ios_client_sdk_proxy_simulator.framework

framework_name=ff_ios_client_sdk_proxy

cp -R build/ff_ios_client_sdk_proxy_simulator.framework build/$framework_name.framework

lipo -create -output "build/$framework_name.framework/$framework_name" \
"build/ff_ios_client_sdk_proxy_simulator.framework/$framework_name" \
"build/ff_ios_client_sdk_proxy_iphoneos.framework/$framework_name" 

rm -r build/*.xcarchive
rm -r build/ff_ios_client_sdk_proxy_simulator.framework
rm -r build/ff_ios_client_sdk_proxy_iphoneos.framework


sharpie bind -sdk iphoneos --namespace="ff_ios_client_sdk_proxy" build/ff_ios_client_sdk_proxy.framework/Headers/ff_ios_client_sdk_proxy-Swift.h -scope build/ff_ios_client_sdk_proxy.framework/Headers
