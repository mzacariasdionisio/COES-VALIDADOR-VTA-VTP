﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COES.MVC.Extranet.srGestor {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="File", Namespace="http://schemas.datacontract.org/2004/07/wsGC")]
    [System.SerializableAttribute()]
    public partial class File : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContentTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] DataAsStreamByteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContentType {
            get {
                return this.ContentTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ContentTypeField, value) != true)) {
                    this.ContentTypeField = value;
                    this.RaisePropertyChanged("ContentType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] DataAsStreamByte {
            get {
                return this.DataAsStreamByteField;
            }
            set {
                if ((object.ReferenceEquals(this.DataAsStreamByteField, value) != true)) {
                    this.DataAsStreamByteField = value;
                    this.RaisePropertyChanged("DataAsStreamByte");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Espacio", Namespace="http://schemas.datacontract.org/2004/07/wsGC")]
    [System.SerializableAttribute()]
    public partial class Espacio : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AsuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DispositivoLegalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EntidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FechaPublicacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TituloField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Asunto {
            get {
                return this.AsuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.AsuntoField, value) != true)) {
                    this.AsuntoField = value;
                    this.RaisePropertyChanged("Asunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DispositivoLegal {
            get {
                return this.DispositivoLegalField;
            }
            set {
                if ((object.ReferenceEquals(this.DispositivoLegalField, value) != true)) {
                    this.DispositivoLegalField = value;
                    this.RaisePropertyChanged("DispositivoLegal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Entidad {
            get {
                return this.EntidadField;
            }
            set {
                if ((object.ReferenceEquals(this.EntidadField, value) != true)) {
                    this.EntidadField = value;
                    this.RaisePropertyChanged("Entidad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FechaPublicacion {
            get {
                return this.FechaPublicacionField;
            }
            set {
                if ((object.ReferenceEquals(this.FechaPublicacionField, value) != true)) {
                    this.FechaPublicacionField = value;
                    this.RaisePropertyChanged("FechaPublicacion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Path {
            get {
                return this.PathField;
            }
            set {
                if ((object.ReferenceEquals(this.PathField, value) != true)) {
                    this.PathField = value;
                    this.RaisePropertyChanged("Path");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Titulo {
            get {
                return this.TituloField;
            }
            set {
                if ((object.ReferenceEquals(this.TituloField, value) != true)) {
                    this.TituloField = value;
                    this.RaisePropertyChanged("Titulo");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="srGestor.ICMSManager")]
    public interface ICMSManager {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFile", ReplyAction="http://tempuri.org/ICMSManager/GetFileResponse")]
        byte[] GetFile(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFile", ReplyAction="http://tempuri.org/ICMSManager/GetFileResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFileWithContentAndName", ReplyAction="http://tempuri.org/ICMSManager/GetFileWithContentAndNameResponse")]
        COES.MVC.Extranet.srGestor.File GetFileWithContentAndName(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFileWithContentAndName", ReplyAction="http://tempuri.org/ICMSManager/GetFileWithContentAndNameResponse")]
        System.Threading.Tasks.Task<COES.MVC.Extranet.srGestor.File> GetFileWithContentAndNameAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetSpacesByPath", ReplyAction="http://tempuri.org/ICMSManager/GetSpacesByPathResponse")]
        System.Data.DataTable GetSpacesByPath(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetSpacesByPath", ReplyAction="http://tempuri.org/ICMSManager/GetSpacesByPathResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetSpacesByPathAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFilesByPath", ReplyAction="http://tempuri.org/ICMSManager/GetFilesByPathResponse")]
        System.Data.DataTable GetFilesByPath(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFilesByPath", ReplyAction="http://tempuri.org/ICMSManager/GetFilesByPathResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetFilesByPathAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetSpacesByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetSpacesByUUIDResponse")]
        System.Data.DataTable GetSpacesByUUID(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetSpacesByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetSpacesByUUIDResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetSpacesByUUIDAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFilesByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetFilesByUUIDResponse")]
        System.Data.DataTable GetFilesByUUID(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFilesByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetFilesByUUIDResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetFilesByUUIDAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetPathByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetPathByUUIDResponse")]
        System.Data.DataTable GetPathByUUID(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetPathByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetPathByUUIDResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetPathByUUIDAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetPathByPath", ReplyAction="http://tempuri.org/ICMSManager/GetPathByPathResponse")]
        System.Data.DataTable GetPathByPath(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetPathByPath", ReplyAction="http://tempuri.org/ICMSManager/GetPathByPathResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetPathByPathAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetDataByUUIDResponse")]
        System.Data.DataTable GetDataByUUID(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByUUID", ReplyAction="http://tempuri.org/ICMSManager/GetDataByUUIDResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetDataByUUIDAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByPath", ReplyAction="http://tempuri.org/ICMSManager/GetDataByPathResponse")]
        System.Data.DataTable GetDataByPath(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByPath", ReplyAction="http://tempuri.org/ICMSManager/GetDataByPathResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetDataByPathAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByPathWithType", ReplyAction="http://tempuri.org/ICMSManager/GetDataByPathWithTypeResponse")]
        System.Data.DataTable GetDataByPathWithType(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataByPathWithType", ReplyAction="http://tempuri.org/ICMSManager/GetDataByPathWithTypeResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetDataByPathWithTypeAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataWithMetadataByPath", ReplyAction="http://tempuri.org/ICMSManager/GetDataWithMetadataByPathResponse")]
        System.Data.DataTable GetDataWithMetadataByPath(string path, string typeMetadata);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataWithMetadataByPath", ReplyAction="http://tempuri.org/ICMSManager/GetDataWithMetadataByPathResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetDataWithMetadataByPathAsync(string path, string typeMetadata);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataAsJson", ReplyAction="http://tempuri.org/ICMSManager/GetDataAsJsonResponse")]
        COES.MVC.Extranet.srGestor.Espacio[] GetDataAsJson(string luceneQuery, int pageSize, int start);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetDataAsJson", ReplyAction="http://tempuri.org/ICMSManager/GetDataAsJsonResponse")]
        System.Threading.Tasks.Task<COES.MVC.Extranet.srGestor.Espacio[]> GetDataAsJsonAsync(string luceneQuery, int pageSize, int start);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetTotalNodes", ReplyAction="http://tempuri.org/ICMSManager/GetTotalNodesResponse")]
        int GetTotalNodes(string luceneQuery);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetTotalNodes", ReplyAction="http://tempuri.org/ICMSManager/GetTotalNodesResponse")]
        System.Threading.Tasks.Task<int> GetTotalNodesAsync(string luceneQuery);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetTotalNodesWithType", ReplyAction="http://tempuri.org/ICMSManager/GetTotalNodesWithTypeResponse")]
        int GetTotalNodesWithType(string path, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetTotalNodesWithType", ReplyAction="http://tempuri.org/ICMSManager/GetTotalNodesWithTypeResponse")]
        System.Threading.Tasks.Task<int> GetTotalNodesWithTypeAsync(string path, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetNormasLegales", ReplyAction="http://tempuri.org/ICMSManager/GetNormasLegalesResponse")]
        System.Data.DataTable GetNormasLegales(string path, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetNormasLegales", ReplyAction="http://tempuri.org/ICMSManager/GetNormasLegalesResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetNormasLegalesAsync(string path, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFileIdByName", ReplyAction="http://tempuri.org/ICMSManager/GetFileIdByNameResponse")]
        string GetFileIdByName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMSManager/GetFileIdByName", ReplyAction="http://tempuri.org/ICMSManager/GetFileIdByNameResponse")]
        System.Threading.Tasks.Task<string> GetFileIdByNameAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICMSManagerChannel : COES.MVC.Extranet.srGestor.ICMSManager, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CMSManagerClient : System.ServiceModel.ClientBase<COES.MVC.Extranet.srGestor.ICMSManager>, COES.MVC.Extranet.srGestor.ICMSManager {
        
        public CMSManagerClient() {
        }
        
        public CMSManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CMSManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CMSManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CMSManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public byte[] GetFile(string id) {
            return base.Channel.GetFile(id);
        }
        
        public System.Threading.Tasks.Task<byte[]> GetFileAsync(string id) {
            return base.Channel.GetFileAsync(id);
        }
        
        public COES.MVC.Extranet.srGestor.File GetFileWithContentAndName(string id) {
            return base.Channel.GetFileWithContentAndName(id);
        }
        
        public System.Threading.Tasks.Task<COES.MVC.Extranet.srGestor.File> GetFileWithContentAndNameAsync(string id) {
            return base.Channel.GetFileWithContentAndNameAsync(id);
        }
        
        public System.Data.DataTable GetSpacesByPath(string path) {
            return base.Channel.GetSpacesByPath(path);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetSpacesByPathAsync(string path) {
            return base.Channel.GetSpacesByPathAsync(path);
        }
        
        public System.Data.DataTable GetFilesByPath(string path) {
            return base.Channel.GetFilesByPath(path);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetFilesByPathAsync(string path) {
            return base.Channel.GetFilesByPathAsync(path);
        }
        
        public System.Data.DataTable GetSpacesByUUID(string id) {
            return base.Channel.GetSpacesByUUID(id);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetSpacesByUUIDAsync(string id) {
            return base.Channel.GetSpacesByUUIDAsync(id);
        }
        
        public System.Data.DataTable GetFilesByUUID(string id) {
            return base.Channel.GetFilesByUUID(id);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetFilesByUUIDAsync(string id) {
            return base.Channel.GetFilesByUUIDAsync(id);
        }
        
        public System.Data.DataTable GetPathByUUID(string id) {
            return base.Channel.GetPathByUUID(id);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetPathByUUIDAsync(string id) {
            return base.Channel.GetPathByUUIDAsync(id);
        }
        
        public System.Data.DataTable GetPathByPath(string path) {
            return base.Channel.GetPathByPath(path);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetPathByPathAsync(string path) {
            return base.Channel.GetPathByPathAsync(path);
        }
        
        public System.Data.DataTable GetDataByUUID(string id) {
            return base.Channel.GetDataByUUID(id);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetDataByUUIDAsync(string id) {
            return base.Channel.GetDataByUUIDAsync(id);
        }
        
        public System.Data.DataTable GetDataByPath(string id) {
            return base.Channel.GetDataByPath(id);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetDataByPathAsync(string id) {
            return base.Channel.GetDataByPathAsync(id);
        }
        
        public System.Data.DataTable GetDataByPathWithType(string path) {
            return base.Channel.GetDataByPathWithType(path);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetDataByPathWithTypeAsync(string path) {
            return base.Channel.GetDataByPathWithTypeAsync(path);
        }
        
        public System.Data.DataTable GetDataWithMetadataByPath(string path, string typeMetadata) {
            return base.Channel.GetDataWithMetadataByPath(path, typeMetadata);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetDataWithMetadataByPathAsync(string path, string typeMetadata) {
            return base.Channel.GetDataWithMetadataByPathAsync(path, typeMetadata);
        }
        
        public COES.MVC.Extranet.srGestor.Espacio[] GetDataAsJson(string luceneQuery, int pageSize, int start) {
            return base.Channel.GetDataAsJson(luceneQuery, pageSize, start);
        }
        
        public System.Threading.Tasks.Task<COES.MVC.Extranet.srGestor.Espacio[]> GetDataAsJsonAsync(string luceneQuery, int pageSize, int start) {
            return base.Channel.GetDataAsJsonAsync(luceneQuery, pageSize, start);
        }
        
        public int GetTotalNodes(string luceneQuery) {
            return base.Channel.GetTotalNodes(luceneQuery);
        }
        
        public System.Threading.Tasks.Task<int> GetTotalNodesAsync(string luceneQuery) {
            return base.Channel.GetTotalNodesAsync(luceneQuery);
        }
        
        public int GetTotalNodesWithType(string path, string type) {
            return base.Channel.GetTotalNodesWithType(path, type);
        }
        
        public System.Threading.Tasks.Task<int> GetTotalNodesWithTypeAsync(string path, string type) {
            return base.Channel.GetTotalNodesWithTypeAsync(path, type);
        }
        
        public System.Data.DataTable GetNormasLegales(string path, string type) {
            return base.Channel.GetNormasLegales(path, type);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetNormasLegalesAsync(string path, string type) {
            return base.Channel.GetNormasLegalesAsync(path, type);
        }
        
        public string GetFileIdByName(string name) {
            return base.Channel.GetFileIdByName(name);
        }
        
        public System.Threading.Tasks.Task<string> GetFileIdByNameAsync(string name) {
            return base.Channel.GetFileIdByNameAsync(name);
        }
    }
}
