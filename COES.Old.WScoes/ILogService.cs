using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using fwapp;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IManttoService" in both code and config file together.
    [ServiceContract]
    public interface ILogService
    {
        [OperationContract]
        int nf_add_log(string as_app, string as_obj, string as_opc, string as_atrib, string as_paramdb, string as_valold, string as_valnew, string as_ip, string as_lastuser, int ai_codref);
    }

}