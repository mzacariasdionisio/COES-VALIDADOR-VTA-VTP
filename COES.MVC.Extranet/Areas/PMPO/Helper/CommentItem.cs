using COES.MVC.Extranet.Helper;
using System;
using System.Runtime.Serialization;

namespace COES.MVC.Extranet.Areas.PMPO.Helper
{
    [DataContract(Name = "comment")]
    public class CommentItem
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "informationTypeName")]
        public string InformationTypeName { get; set; }

        [IgnoreDataMember]
        public DateTime DateSend { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "dateSend")]
        public string DateSendToString
        {
            get
            {
                return DateSend.ToString(Constantes.FormatoFechaFull);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [DataMember(Name = "isAgent")]
        public bool IsAgent { get; set; }

        [DataMember(Name = "isViewed")]
        public bool IsViewed { get; set; }

        [DataMember(Name = "senderName")]
        public string SenderName { get; set; }

        [DataMember(Name = "fileName")]
        public string FileName { get; set; }
    }
}