//
//  ff-ios-client-sdk-proxy.swift
//  ff-ios-client-sdk-proxy
//
//  Created by Andrija Milovanovic on 8.1.22..
//

import Foundation
import ff_ios_client_sdk


@objc(CfConfigurationProxy)
public class CfConfigurationProxy : NSObject
{
    private let builder = CfConfiguration.builder()
    
    internal func build() -> CfConfiguration {
        return builder.build()
    }
    
    @objc
    public var configUrl: String = "" { didSet { _ = builder.setConfigUrl(configUrl) } }
    @objc
    public var streamUrl: String = "" { didSet { _ = builder.setStreamUrl(streamUrl) } }
    @objc
    public var eventUrl: String = "" { didSet { _ = builder.setEventUrl(eventUrl) } }
    @objc
    public var streamEnabled: Bool = true { didSet { _ = builder.setStreamEnabled(streamEnabled) } }
    @objc
    public var analyticsEnabled: Bool = true { didSet { _ = builder.setAnalyticsEnabled(analyticsEnabled) } }
    @objc
    public var pollingInterval: TimeInterval = 10 { didSet { _ = builder.setPollingInterval(pollingInterval) } }
}


@objc(CfTargetProxy)
public class CfTargetProxy : NSObject
{

    private let builder = CfTarget.builder()
    
    internal func build() -> CfTarget {
        return builder.build()
    }
    @objc
    public var identifier: String = "" { didSet { _ = builder.setIdentifier(identifier) } }
    @objc
    public var name: String = "" { didSet { _ = builder.setName(name) } }
    @objc
    public var anonymous: Bool = false { didSet { _ = builder.setAnonymous(anonymous) } }
    @objc
    public var attributes:[String:String] = [:]{ didSet { _ = builder.setAttributes(attributes) } }

}


@objc(CfClientProxy)
public class CfClientProxy : NSObject {
 
    private let cfClient = CfClient.sharedInstance
    
    @objc
    public static let shared = CfClientProxy()
    
    @objc
    public func initialize(apiKey: String, configuration: CfConfigurationProxy, target: CfTargetProxy)
    {
        cfClient.initialize(apiKey: apiKey, configuration: configuration.build(), target: target.build(), cache:CfCache()) {[weak self] result in
            switch result
            {
            case .failure(let error):
                print("On initialize event failure");
                self?.delegate?.onError(error: CfErrorProxy(error.errorData))
            case .success:
                print("On initialize event success");
                self?.delegate?.onInitializeSuccess()
                self?.cfClient.registerEventsListener { result in
                    print("On registered event listener");
                    switch result
                    {
                    case .failure(let error):
                        self?.delegate?.onError(error: CfErrorProxy(error.errorData))
                    case .success(let event):
                        switch event
                        {
                        case .onComplete: self?.delegate?.onStreamCompleted()
                        case .onOpen: self?.delegate?.onStreamOpened()
                        case .onEventListener(let e) : self?.delegate?.onStreamEventReceived(evaluation: CxEvaluation(e) )
                        case .onPolling(let e): self?.delegate?.onPollingEventReceived(evaluations: e?.map{ CxEvaluation($0) }.compactMap({$0}) )
                        case .onMessage(let m): self?.delegate?.onMessageReceived(message: CxMessage(m))
                        }
                    }
                }
            }
        }
    }
    
    @objc
    public func stringVariation(evaluationId: String, defaultValue: String? = nil, _ completion:@escaping(_ result:CxEvaluation?)->()) {
        cfClient.stringVariation(evaluationId: evaluationId, defaultValue: defaultValue) { completion( CxEvaluation($0)) }
    }
    @objc
    public func boolVariation(evaluationId: String, defaultValue: NSNumber? = nil, _ completion:@escaping(_ result:CxEvaluation?)->()) {
        cfClient.boolVariation(evaluationId: evaluationId, defaultValue: defaultValue?.boolValue) { completion( CxEvaluation($0)) }
    }
    @objc
    public func numberVariation(evaluationId: String, defaultValue: NSNumber? = nil, _ completion:@escaping(_ result:CxEvaluation?)->()) {
        cfClient.numberVariation(evaluationId: evaluationId, defaultValue: defaultValue?.intValue) { completion( CxEvaluation($0)) }
    }
    @objc
    public func jsonVariation(evaluationId: String, defaultValue: [String:Any]? = nil, _ completion:@escaping(_ result:CxEvaluation?)->()) {
        let dv = defaultValue?.mapValues{ convert($0)}.compactMapValues{$0}
        cfClient.jsonVariation(evaluationId: evaluationId, defaultValue: dv) { completion( CxEvaluation($0)) }
    }
    
    private func convert(_ val: Any ) -> ValueType? {
        switch val
        {
            case is String: return ValueType.string(val as! String)
            case is Bool:   return ValueType.bool(val as! Bool)
            case is Int:    return ValueType.int(val as! Int)
            case is [String:Any]: return ValueType.object( (val as! [String:Any]).mapValues{ convert( $0 ) }.compactMapValues{$0} )
            default: return nil
        }
    }
    
    @objc
    public weak var delegate:CfClientDelegate? = nil
}

@objc(CfClientDelegate)
public protocol CfClientDelegate : NSObjectProtocol
{
    func onInitializeSuccess()
    func onStreamOpened()
    func onStreamCompleted()
    func onStreamEventReceived(evaluation:CxEvaluation?)
    func onPollingEventReceived(evaluations:[CxEvaluation]?)
    func onMessageReceived(message:CxMessage?)
    
    func onError(error:CfErrorProxy)
}

@objc(CxEvaluation)
public class CxEvaluation : NSObject
{
    @objc
    public var flag: String
    @objc
    public var identifier: String
    @objc
    public var value: CxValue
    
    internal init?( _ evaluation:Evaluation? ) {
        guard let evaluation = evaluation else { return nil }
 
        self.flag = evaluation.flag
        self.identifier = evaluation.identifier
        self.value = CxValue(evaluation.value)
    }
}

@objc(CxValue)
public class CxValue : NSObject
{
    internal let value:ValueType
    @objc
    public var boolValue:NSNumber? {
        return value.boolValue != nil ? NSNumber(value: value.boolValue!) : nil
    }
    @objc
    public var intValue:NSNumber? {
        return value.intValue != nil ? NSNumber(value: value.intValue!) : nil
    }
    
    @objc
    public var stringValue:String? {
        return value.stringValue
    }
    @objc
    public var objectValue:[String:CxValue]? {
        return value.objectValue?.mapValues{ CxValue($0) }
    }

    internal init(_ value:ValueType) {
        self.value = value
    }
}

@objc(CxMessage)
public class CxMessage: NSObject {
    @objc
    public var event: String?
    @objc
    public var domain: String?
    @objc
    public var identifier: String?
    @objc
    public var version: Double
    
    internal init?( _ message:Message? ) {
        guard let message = message else { return nil }
 
        self.event = message.event
        self.identifier = message.identifier
        self.domain = message.domain
        self.version = message.version ?? 0
    }
}
@objc(CfErrorProxy)
public class CfErrorProxy : NSObject
{
    @objc
    public var title:String?
    @objc
    public var localizedMessage: String?
    @objc
    public let underlyingError: String?
    @objc
    public let statusCode: Int
    @objc
    public let data: Data?
    
    internal init( _ error:ErrorData )
    {
        self.title = error.title
        self.localizedMessage = error.localizedMessage
        self.underlyingError = error.underlyingError
        self.statusCode = error.statusCode ?? 0
        self.data = error.data
    }
}
