﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace TestProject.ProductService {
	[DebuggerStepThrough()]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="Product", Namespace="http://schemas.datacontract.org/2004/07/NorthwindModel.Models")]
    [Serializable()]
    public partial class Product : object, IExtensibleDataObject, INotifyPropertyChanged {
        
        [NonSerialized()]
        private ExtensionDataObject extensionDataField;
        
        [OptionalField()]
        private Category CategoryField;
        
        [OptionalField()]
        private int ProductIDField;
        
        [OptionalField()]
        private string ProductNameField;
        
        [OptionalField()]
        private Nullable<decimal> UnitPriceField;
        
        [Browsable(false)]
        public ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [DataMember()]
        public Category Category {
            get {
                return this.CategoryField;
            }
            set {
                if ((ReferenceEquals(this.CategoryField, value) != true)) {
                    this.CategoryField = value;
                    this.RaisePropertyChanged("Category");
                }
            }
        }
        
        [DataMember()]
        public int ProductID {
            get {
                return this.ProductIDField;
            }
            set {
                if ((this.ProductIDField.Equals(value) != true)) {
                    this.ProductIDField = value;
                    this.RaisePropertyChanged("ProductID");
                }
            }
        }
        
        [DataMember()]
        public string ProductName {
            get {
                return this.ProductNameField;
            }
            set {
                if ((ReferenceEquals(this.ProductNameField, value) != true)) {
                    this.ProductNameField = value;
                    this.RaisePropertyChanged("ProductName");
                }
            }
        }
        
        [DataMember()]
        public Nullable<decimal> UnitPrice {
            get {
                return this.UnitPriceField;
            }
            set {
                if ((this.UnitPriceField.Equals(value) != true)) {
                    this.UnitPriceField = value;
                    this.RaisePropertyChanged("UnitPrice");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [DebuggerStepThrough()]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="Category", Namespace="http://schemas.datacontract.org/2004/07/NorthwindModel.Models")]
    [Serializable()]
    public partial class Category : BasicCategory {
        
        [OptionalField()]
        private string DescriptionField;
        
        [DataMember()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
    }
    
    [DebuggerStepThrough()]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="BasicCategory", Namespace="http://schemas.datacontract.org/2004/07/NorthwindModel.Models.CustomModels")]
    [Serializable()]
    [KnownType(typeof(Category))]
    public partial class BasicCategory : object, IExtensibleDataObject, INotifyPropertyChanged {
        
        [NonSerialized()]
        private ExtensionDataObject extensionDataField;
        
        [OptionalField()]
        private int CategoryIDField;
        
        [OptionalField()]
        private string CategoryNameField;
        
        [Browsable(false)]
        public ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [DataMember()]
        public int CategoryID {
            get {
                return this.CategoryIDField;
            }
            set {
                if ((this.CategoryIDField.Equals(value) != true)) {
                    this.CategoryIDField = value;
                    this.RaisePropertyChanged("CategoryID");
                }
            }
        }
        
        [DataMember()]
        public string CategoryName {
            get {
                return this.CategoryNameField;
            }
            set {
                if ((ReferenceEquals(this.CategoryNameField, value) != true)) {
                    this.CategoryNameField = value;
                    this.RaisePropertyChanged("CategoryName");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(ConfigurationName="ProductService.IProductService")]
    public interface IProductService {
        
        [OperationContract(Action="http://tempuri.org/IProductService/GetAllProducts", ReplyAction="http://tempuri.org/IProductService/GetAllProductsResponse")]
        Product[] GetAllProducts();
        
        [OperationContract(Action="http://tempuri.org/IProductService/GetAllProducts", ReplyAction="http://tempuri.org/IProductService/GetAllProductsResponse")]
        Task<Product[]> GetAllProductsAsync();
    }
    
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface IProductServiceChannel : IProductService, IClientChannel {
    }
    
    [DebuggerStepThrough()]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public partial class ProductServiceClient : ClientBase<IProductService>, IProductService {
        
        public ProductServiceClient() {
        }
        
        public ProductServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProductServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductServiceClient(Binding binding, EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Product[] GetAllProducts() {
            return base.Channel.GetAllProducts();
        }
        
        public Task<Product[]> GetAllProductsAsync() {
            return base.Channel.GetAllProductsAsync();
        }
    }
}
