using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Photos.SapoServices.Utils
{
    /// <summary>
    /// Contains user credentials.
    /// </summary>
    [DataContract(Namespace = "http://services.sapo.pt/definitions", Name = "ESBCredentials")]
    public class EsbCredentials
    {
        /// <summary />
        [XmlElement("ESBUsername")]
        [DataMember(Order = 0, IsRequired = false, EmitDefaultValue = false, Name = "ESBUsername")]
        public string Username { get; set; }

        /// <summary />
        [XmlElement("ESBPassword")]
        [DataMember(Order = 1, IsRequired = false, EmitDefaultValue = false, Name = "ESBPassword")]
        public string Password { get; set; }

        /// <summary />
        [XmlElement("ESBToken")]
        [DataMember(Order = 3, IsRequired = false, EmitDefaultValue = false, Name = "ESBToken")]
        public string Token { get; set; }

        /// <summary />
        [XmlElement("ESBRoles")]
        [DataMember(Order = 4, IsRequired = false, EmitDefaultValue = false, Name = "ESBRoles")]
        public EsbRoles Roles { get; set; }

        /// <summary />
        [XmlElement("ESBTokenTimeToLive")]
        [DataMember(Order = 5, IsRequired = false, EmitDefaultValue = false, Name = "ESBTokenTimeToLive")]
        public string TokenTimeToLive { get; set; }

        /// <summary />
        [XmlElement("ESBTokenExtraInfo")]
        [DataMember(Order = 6, IsRequired = false, EmitDefaultValue = false, Name = "ESBTokenExtraInfo")]
        public string TokenExtraInfo { get; set; }

        /// <summary />
        [XmlElement("ESBPrimaryId")]
        [DataMember(Order = 7, IsRequired = false, EmitDefaultValue = false, Name = "ESBPrimaryId")]
        public string PrimaryId { get; set; }

        /// <summary />
        [XmlElement("ESBUserType")]
        [DataMember(Order = 8, IsRequired = false, EmitDefaultValue = false, Name = "ESBUserType")]
        public string UserType { get; set; }

        /// <summary />
        [XmlElement("ESBCredentialsStore")]
        [DataMember(Order = 9, IsRequired = false, EmitDefaultValue = false, Name = "ESBCredentialsStore")]
        public string CredentialsStore { get; set; }

     
        public override string ToString()
        {
            return string.Format(@"{0}\{1}", CredentialsStore, PrimaryId);
        }
    }

    /// <summary />
    [DataContract(Namespace = "http://services.sapo.pt/definitions", Name = "ESBRoles")]
    public class EsbRoles
    {
        /// <summary />
        [XmlElement("ESBRole")]
        [DataMember(Order = 0, IsRequired = false, EmitDefaultValue = false, Name = "ESBRole")]
        public string[] Roles { get; set; }
    }
}