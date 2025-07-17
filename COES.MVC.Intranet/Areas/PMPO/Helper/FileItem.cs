using COES.MVC.Intranet.Helper;
using System;
using System.Runtime.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Helper
{
    [DataContract(Name = "file")]
    public class FileItem
    {
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "tmpFileName")]
        public string TmpFileName { get; set; }
    }
}