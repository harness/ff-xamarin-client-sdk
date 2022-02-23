using System;
using Foundation;
using ObjCRuntime;
using ff_ios_client_sdk_proxy;

namespace ff_ios_client_sdk_proxy
{
	// @protocol CfClientDelegate <NSObject>
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof(NSObject))]
	interface CfClientDelegate
	{
		// @required -(void)onInitializeSuccess;
		[Abstract]
		[Export ("onInitializeSuccess")]
		void OnInitializeSuccess ();

		// @required -(void)onStreamOpened;
		[Abstract]
		[Export ("onStreamOpened")]
		void OnStreamOpened ();

		// @required -(void)onStreamCompleted;
		[Abstract]
		[Export ("onStreamCompleted")]
		void OnStreamCompleted ();

		// @required -(void)onStreamEventReceivedWithEvaluation:(CxEvaluation * _Nullable)evaluation;
		[Abstract]
		[Export ("onStreamEventReceivedWithEvaluation:")]
		void OnStreamEventReceivedWithEvaluation ([NullAllowed] CxEvaluation evaluation);

		// @required -(void)onPollingEventReceivedWithEvaluations:(NSArray<CxEvaluation *> * _Nullable)evaluations;
		[Abstract]
		[Export ("onPollingEventReceivedWithEvaluations:")]
		void OnPollingEventReceivedWithEvaluations ([NullAllowed] CxEvaluation[] evaluations);

		// @required -(void)onMessageReceivedWithMessage:(CxMessage * _Nullable)message;
		[Abstract]
		[Export ("onMessageReceivedWithMessage:")]
		void OnMessageReceivedWithMessage ([NullAllowed] CxMessage message);

		// @required -(void)onErrorWithError:(CfErrorProxy * _Nonnull)error;
		[Abstract]
		[Export ("onErrorWithError:")]
		void OnErrorWithError (CfErrorProxy error);
	}

	// @interface CfClientProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface CfClientProxy
	{
		// @property (readonly, nonatomic, strong, class) CfClientProxy * _Nonnull shared;
		[Static]
		[Export ("shared", ArgumentSemantic.Strong)]
		CfClientProxy Shared { get; }

		// -(void)initializeWithApiKey:(NSString * _Nonnull)apiKey configuration:(CfConfigurationProxy * _Nonnull)configuration target:(CfTargetProxy * _Nonnull)target;
		[Export ("initializeWithApiKey:configuration:target:")]
		void InitializeWithApiKey (string apiKey, CfConfigurationProxy configuration, CfTargetProxy target);

		// -(void)stringVariationWithEvaluationId:(NSString * _Nonnull)evaluationId defaultValue:(NSString * _Nullable)defaultValue :(void (^ _Nonnull)(CxEvaluation * _Nullable))completion;
		[Export ("stringVariationWithEvaluationId:defaultValue::")]
		void StringVariationWithEvaluationId (string evaluationId, [NullAllowed] string defaultValue, Action<CxEvaluation> completion);

		// -(void)boolVariationWithEvaluationId:(NSString * _Nonnull)evaluationId defaultValue:(NSNumber * _Nullable)defaultValue :(void (^ _Nonnull)(CxEvaluation * _Nullable))completion;
		[Export ("boolVariationWithEvaluationId:defaultValue::")]
		void BoolVariationWithEvaluationId (string evaluationId, [NullAllowed] NSNumber defaultValue, Action<CxEvaluation> completion);

		// -(void)numberVariationWithEvaluationId:(NSString * _Nonnull)evaluationId defaultValue:(NSNumber * _Nullable)defaultValue :(void (^ _Nonnull)(CxEvaluation * _Nullable))completion;
		[Export ("numberVariationWithEvaluationId:defaultValue::")]
		void NumberVariationWithEvaluationId (string evaluationId, [NullAllowed] NSNumber defaultValue, Action<CxEvaluation> completion);

		// -(void)jsonVariationWithEvaluationId:(NSString * _Nonnull)evaluationId defaultValue:(NSDictionary<NSString *,id> * _Nullable)defaultValue :(void (^ _Nonnull)(CxEvaluation * _Nullable))completion;
		[Export ("jsonVariationWithEvaluationId:defaultValue::")]
		void JsonVariationWithEvaluationId (string evaluationId, [NullAllowed] NSDictionary<NSString, NSObject> defaultValue, Action<CxEvaluation> completion);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		CfClientDelegate Delegate { get; set; }

		// @property (nonatomic, weak) id<CfClientDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	// @interface CfConfigurationProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface CfConfigurationProxy
	{
		// @property (copy, nonatomic) NSString * _Nonnull configUrl;
		[Export ("configUrl")]
		string ConfigUrl { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull streamUrl;
		[Export ("streamUrl")]
		string StreamUrl { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull eventUrl;
		[Export ("eventUrl")]
		string EventUrl { get; set; }

		// @property (nonatomic) BOOL streamEnabled;
		[Export ("streamEnabled")]
		bool StreamEnabled { get; set; }

		// @property (nonatomic) BOOL analyticsEnabled;
		[Export ("analyticsEnabled")]
		bool AnalyticsEnabled { get; set; }

		// @property (nonatomic) NSTimeInterval pollingInterval;
		[Export ("pollingInterval")]
		double PollingInterval { get; set; }
	}

	// @interface CfErrorProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface CfErrorProxy
	{
		// @property (copy, nonatomic) NSString * _Nullable title;
		[NullAllowed, Export ("title")]
		string Title { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable localizedMessage;
		[NullAllowed, Export ("localizedMessage")]
		string LocalizedMessage { get; set; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable underlyingError;
		[NullAllowed, Export ("underlyingError")]
		string UnderlyingError { get; }

		// @property (readonly, nonatomic) NSInteger statusCode;
		[Export ("statusCode")]
		nint StatusCode { get; }

		// @property (readonly, copy, nonatomic) NSData * _Nullable data;
		[NullAllowed, Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }
	}

	// @interface CfTargetProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface CfTargetProxy
	{
		// @property (copy, nonatomic) NSString * _Nonnull identifier;
		[Export ("identifier")]
		string Identifier { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; set; }

		// @property (nonatomic) BOOL anonymous;
		[Export ("anonymous")]
		bool Anonymous { get; set; }

		// @property (copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nonnull attributes;
		[Export ("attributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Attributes { get; set; }
	}

	// @interface CxEvaluation : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface CxEvaluation
	{
		// @property (copy, nonatomic) NSString * _Nonnull flag;
		[Export ("flag")]
		string Flag { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull identifier;
		[Export ("identifier")]
		string Identifier { get; set; }

		// @property (nonatomic, strong) CxValue * _Nonnull value;
		[Export ("value", ArgumentSemantic.Strong)]
		CxValue Value { get; set; }
	}

	// @interface CxMessage : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface CxMessage
	{
		// @property (copy, nonatomic) NSString * _Nullable event;
		[NullAllowed, Export ("event")]
		string Event { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable domain;
		[NullAllowed, Export ("domain")]
		string Domain { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable identifier;
		[NullAllowed, Export ("identifier")]
		string Identifier { get; set; }

		// @property (nonatomic) double version;
		[Export ("version")]
		double Version { get; set; }
	}

	// @interface CxValue : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface CxValue : INativeObject
	{
		// @property (readonly, nonatomic, strong) NSNumber * _Nullable boolValue;
		[NullAllowed, Export ("boolValue", ArgumentSemantic.Strong)]
		NSNumber BoolValue { get; }

		// @property (readonly, nonatomic, strong) NSNumber * _Nullable intValue;
		[NullAllowed, Export ("intValue", ArgumentSemantic.Strong)]
		NSNumber IntValue { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable stringValue;
		[NullAllowed, Export ("stringValue")]
		string StringValue { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,CxValue *> * _Nullable objectValue;
		[NullAllowed, Export ("objectValue", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CxValue> ObjectValue { get; }
	}
}