﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34011
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfQuery.BDWMInterestService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://api.baidu.com/sem/nms/v2", ConfigurationName="BDWMInterestService.InterestService")]
    public interface InterestService {
        
        // CODEGEN: 消息 getInterestRequest 的包装名称(getInterestRequest)以后生成的消息协定与默认值(getInterest)不匹配
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getInterest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WpfQuery.BDWMInterestService.getInterestResponse getInterest(WpfQuery.BDWMInterestService.getInterestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getInterest", ReplyAction="*")]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getInterestResponse> getInterestAsync(WpfQuery.BDWMInterestService.getInterestRequest request);
        
        // CODEGEN: 消息 getAllCustomInterestRequest 的包装名称(getAllCustomInterestRequest)以后生成的消息协定与默认值(getAllCustomInterest)不匹配
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getAllCustomInterest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WpfQuery.BDWMInterestService.getAllCustomInterestResponse getAllCustomInterest(WpfQuery.BDWMInterestService.getAllCustomInterestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getAllCustomInterest", ReplyAction="*")]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getAllCustomInterestResponse> getAllCustomInterestAsync(WpfQuery.BDWMInterestService.getAllCustomInterestRequest request);
        
        // CODEGEN: 消息 getCustomInterestRequest 的包装名称(getCustomInterestRequest)以后生成的消息协定与默认值(getCustomInterest)不匹配
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getCustomInterest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WpfQuery.BDWMInterestService.getCustomInterestResponse getCustomInterest(WpfQuery.BDWMInterestService.getCustomInterestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://api.baidu.com/sem/nms/v2/InterestService/getCustomInterest", ReplyAction="*")]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getCustomInterestResponse> getCustomInterestAsync(WpfQuery.BDWMInterestService.getCustomInterestRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
    public partial class AuthHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string usernameField;
        
        private string passwordField;
        
        private string tokenField;
        
        private string targetField;
        
        private string accessTokenField;
        
        private int account_typeField;
        
        private bool account_typeFieldSpecified;
        
        private string actionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
                this.RaisePropertyChanged("username");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("password");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
                this.RaisePropertyChanged("token");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string target {
            get {
                return this.targetField;
            }
            set {
                this.targetField = value;
                this.RaisePropertyChanged("target");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string accessToken {
            get {
                return this.accessTokenField;
            }
            set {
                this.accessTokenField = value;
                this.RaisePropertyChanged("accessToken");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int account_type {
            get {
                return this.account_typeField;
            }
            set {
                this.account_typeField = value;
                this.RaisePropertyChanged("account_type");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool account_typeSpecified {
            get {
                return this.account_typeFieldSpecified;
            }
            set {
                this.account_typeFieldSpecified = value;
                this.RaisePropertyChanged("account_typeSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string action {
            get {
                return this.actionField;
            }
            set {
                this.actionField = value;
                this.RaisePropertyChanged("action");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.baidu.com/sem/nms/v2")]
    public partial class CustomInterestCollectionType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private InterestType[] interestsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("interests", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public InterestType[] interests {
            get {
                return this.interestsField;
            }
            set {
                this.interestsField = value;
                this.RaisePropertyChanged("interests");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.baidu.com/sem/nms/v2")]
    public partial class InterestType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long interestIdField;
        
        private bool interestIdFieldSpecified;
        
        private string interestNameField;
        
        private int parentIdField;
        
        private bool parentIdFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long interestId {
            get {
                return this.interestIdField;
            }
            set {
                this.interestIdField = value;
                this.RaisePropertyChanged("interestId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool interestIdSpecified {
            get {
                return this.interestIdFieldSpecified;
            }
            set {
                this.interestIdFieldSpecified = value;
                this.RaisePropertyChanged("interestIdSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string interestName {
            get {
                return this.interestNameField;
            }
            set {
                this.interestNameField = value;
                this.RaisePropertyChanged("interestName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public int parentId {
            get {
                return this.parentIdField;
            }
            set {
                this.parentIdField = value;
                this.RaisePropertyChanged("parentId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentIdSpecified {
            get {
                return this.parentIdFieldSpecified;
            }
            set {
                this.parentIdFieldSpecified = value;
                this.RaisePropertyChanged("parentIdSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.baidu.com/sem/nms/v2")]
    public partial class CustomInterestType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long customItIdField;
        
        private bool customItIdFieldSpecified;
        
        private string customItNameField;
        
        private CustomInterestCollectionType[] customItCollectionsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long customItId {
            get {
                return this.customItIdField;
            }
            set {
                this.customItIdField = value;
                this.RaisePropertyChanged("customItId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customItIdSpecified {
            get {
                return this.customItIdFieldSpecified;
            }
            set {
                this.customItIdFieldSpecified = value;
                this.RaisePropertyChanged("customItIdSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string customItName {
            get {
                return this.customItNameField;
            }
            set {
                this.customItNameField = value;
                this.RaisePropertyChanged("customItName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("customItCollections", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public CustomInterestCollectionType[] customItCollections {
            get {
                return this.customItCollectionsField;
            }
            set {
                this.customItCollectionsField = value;
                this.RaisePropertyChanged("customItCollections");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
    public partial class Failure : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int codeField;
        
        private string messageField;
        
        private string positionField;
        
        private string contentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string position {
            get {
                return this.positionField;
            }
            set {
                this.positionField = value;
                this.RaisePropertyChanged("position");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
                this.RaisePropertyChanged("content");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
    public partial class ResHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string descField;
        
        private Failure[] failuresField;
        
        private int oprsField;
        
        private bool oprsFieldSpecified;
        
        private int oprtimeField;
        
        private bool oprtimeFieldSpecified;
        
        private int quotaField;
        
        private bool quotaFieldSpecified;
        
        private int rquotaField;
        
        private bool rquotaFieldSpecified;
        
        private int statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string desc {
            get {
                return this.descField;
            }
            set {
                this.descField = value;
                this.RaisePropertyChanged("desc");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("failures", Order=1)]
        public Failure[] failures {
            get {
                return this.failuresField;
            }
            set {
                this.failuresField = value;
                this.RaisePropertyChanged("failures");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int oprs {
            get {
                return this.oprsField;
            }
            set {
                this.oprsField = value;
                this.RaisePropertyChanged("oprs");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool oprsSpecified {
            get {
                return this.oprsFieldSpecified;
            }
            set {
                this.oprsFieldSpecified = value;
                this.RaisePropertyChanged("oprsSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int oprtime {
            get {
                return this.oprtimeField;
            }
            set {
                this.oprtimeField = value;
                this.RaisePropertyChanged("oprtime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool oprtimeSpecified {
            get {
                return this.oprtimeFieldSpecified;
            }
            set {
                this.oprtimeFieldSpecified = value;
                this.RaisePropertyChanged("oprtimeSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int quota {
            get {
                return this.quotaField;
            }
            set {
                this.quotaField = value;
                this.RaisePropertyChanged("quota");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotaSpecified {
            get {
                return this.quotaFieldSpecified;
            }
            set {
                this.quotaFieldSpecified = value;
                this.RaisePropertyChanged("quotaSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int rquota {
            get {
                return this.rquotaField;
            }
            set {
                this.rquotaField = value;
                this.RaisePropertyChanged("rquota");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool rquotaSpecified {
            get {
                return this.rquotaFieldSpecified;
            }
            set {
                this.rquotaFieldSpecified = value;
                this.RaisePropertyChanged("rquotaSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getInterestRequest", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getInterestRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.AuthHeader AuthHeader;
        
        public getInterestRequest() {
        }
        
        public getInterestRequest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader) {
            this.AuthHeader = AuthHeader;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getInterestResponse", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getInterestResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.ResHeader ResHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.baidu.com/sem/nms/v2", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("interestTypes", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WpfQuery.BDWMInterestService.InterestType[] interestTypes;
        
        public getInterestResponse() {
        }
        
        public getInterestResponse(WpfQuery.BDWMInterestService.ResHeader ResHeader, WpfQuery.BDWMInterestService.InterestType[] interestTypes) {
            this.ResHeader = ResHeader;
            this.interestTypes = interestTypes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllCustomInterestRequest", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getAllCustomInterestRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.AuthHeader AuthHeader;
        
        public getAllCustomInterestRequest() {
        }
        
        public getAllCustomInterestRequest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader) {
            this.AuthHeader = AuthHeader;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllCustomInterestResponse", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getAllCustomInterestResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.ResHeader ResHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.baidu.com/sem/nms/v2", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("customerInterestTypes", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes;
        
        public getAllCustomInterestResponse() {
        }
        
        public getAllCustomInterestResponse(WpfQuery.BDWMInterestService.ResHeader ResHeader, WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes) {
            this.ResHeader = ResHeader;
            this.customerInterestTypes = customerInterestTypes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCustomInterestRequest", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getCustomInterestRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.AuthHeader AuthHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.baidu.com/sem/nms/v2", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("customItIds", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long[] customItIds;
        
        public getCustomInterestRequest() {
        }
        
        public getCustomInterestRequest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader, long[] customItIds) {
            this.AuthHeader = AuthHeader;
            this.customItIds = customItIds;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCustomInterestResponse", WrapperNamespace="https://api.baidu.com/sem/nms/v2", IsWrapped=true)]
    public partial class getCustomInterestResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://api.baidu.com/sem/common/v2")]
        public WpfQuery.BDWMInterestService.ResHeader ResHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.baidu.com/sem/nms/v2", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("customerInterestTypes", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes;
        
        public getCustomInterestResponse() {
        }
        
        public getCustomInterestResponse(WpfQuery.BDWMInterestService.ResHeader ResHeader, WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes) {
            this.ResHeader = ResHeader;
            this.customerInterestTypes = customerInterestTypes;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface InterestServiceChannel : WpfQuery.BDWMInterestService.InterestService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InterestServiceClient : System.ServiceModel.ClientBase<WpfQuery.BDWMInterestService.InterestService>, WpfQuery.BDWMInterestService.InterestService {
        
        public InterestServiceClient() {
        }
        
        public InterestServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InterestServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InterestServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InterestServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WpfQuery.BDWMInterestService.getInterestResponse WpfQuery.BDWMInterestService.InterestService.getInterest(WpfQuery.BDWMInterestService.getInterestRequest request) {
            return base.Channel.getInterest(request);
        }
        
        public WpfQuery.BDWMInterestService.ResHeader getInterest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader, out WpfQuery.BDWMInterestService.InterestType[] interestTypes) {
            WpfQuery.BDWMInterestService.getInterestRequest inValue = new WpfQuery.BDWMInterestService.getInterestRequest();
            inValue.AuthHeader = AuthHeader;
            WpfQuery.BDWMInterestService.getInterestResponse retVal = ((WpfQuery.BDWMInterestService.InterestService)(this)).getInterest(inValue);
            interestTypes = retVal.interestTypes;
            return retVal.ResHeader;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getInterestResponse> WpfQuery.BDWMInterestService.InterestService.getInterestAsync(WpfQuery.BDWMInterestService.getInterestRequest request) {
            return base.Channel.getInterestAsync(request);
        }
        
        public System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getInterestResponse> getInterestAsync(WpfQuery.BDWMInterestService.AuthHeader AuthHeader) {
            WpfQuery.BDWMInterestService.getInterestRequest inValue = new WpfQuery.BDWMInterestService.getInterestRequest();
            inValue.AuthHeader = AuthHeader;
            return ((WpfQuery.BDWMInterestService.InterestService)(this)).getInterestAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WpfQuery.BDWMInterestService.getAllCustomInterestResponse WpfQuery.BDWMInterestService.InterestService.getAllCustomInterest(WpfQuery.BDWMInterestService.getAllCustomInterestRequest request) {
            return base.Channel.getAllCustomInterest(request);
        }
        
        public WpfQuery.BDWMInterestService.ResHeader getAllCustomInterest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader, out WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes) {
            WpfQuery.BDWMInterestService.getAllCustomInterestRequest inValue = new WpfQuery.BDWMInterestService.getAllCustomInterestRequest();
            inValue.AuthHeader = AuthHeader;
            WpfQuery.BDWMInterestService.getAllCustomInterestResponse retVal = ((WpfQuery.BDWMInterestService.InterestService)(this)).getAllCustomInterest(inValue);
            customerInterestTypes = retVal.customerInterestTypes;
            return retVal.ResHeader;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getAllCustomInterestResponse> WpfQuery.BDWMInterestService.InterestService.getAllCustomInterestAsync(WpfQuery.BDWMInterestService.getAllCustomInterestRequest request) {
            return base.Channel.getAllCustomInterestAsync(request);
        }
        
        public System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getAllCustomInterestResponse> getAllCustomInterestAsync(WpfQuery.BDWMInterestService.AuthHeader AuthHeader) {
            WpfQuery.BDWMInterestService.getAllCustomInterestRequest inValue = new WpfQuery.BDWMInterestService.getAllCustomInterestRequest();
            inValue.AuthHeader = AuthHeader;
            return ((WpfQuery.BDWMInterestService.InterestService)(this)).getAllCustomInterestAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WpfQuery.BDWMInterestService.getCustomInterestResponse WpfQuery.BDWMInterestService.InterestService.getCustomInterest(WpfQuery.BDWMInterestService.getCustomInterestRequest request) {
            return base.Channel.getCustomInterest(request);
        }
        
        public WpfQuery.BDWMInterestService.ResHeader getCustomInterest(WpfQuery.BDWMInterestService.AuthHeader AuthHeader, long[] customItIds, out WpfQuery.BDWMInterestService.CustomInterestType[] customerInterestTypes) {
            WpfQuery.BDWMInterestService.getCustomInterestRequest inValue = new WpfQuery.BDWMInterestService.getCustomInterestRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.customItIds = customItIds;
            WpfQuery.BDWMInterestService.getCustomInterestResponse retVal = ((WpfQuery.BDWMInterestService.InterestService)(this)).getCustomInterest(inValue);
            customerInterestTypes = retVal.customerInterestTypes;
            return retVal.ResHeader;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getCustomInterestResponse> WpfQuery.BDWMInterestService.InterestService.getCustomInterestAsync(WpfQuery.BDWMInterestService.getCustomInterestRequest request) {
            return base.Channel.getCustomInterestAsync(request);
        }
        
        public System.Threading.Tasks.Task<WpfQuery.BDWMInterestService.getCustomInterestResponse> getCustomInterestAsync(WpfQuery.BDWMInterestService.AuthHeader AuthHeader, long[] customItIds) {
            WpfQuery.BDWMInterestService.getCustomInterestRequest inValue = new WpfQuery.BDWMInterestService.getCustomInterestRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.customItIds = customItIds;
            return ((WpfQuery.BDWMInterestService.InterestService)(this)).getCustomInterestAsync(inValue);
        }
    }
}