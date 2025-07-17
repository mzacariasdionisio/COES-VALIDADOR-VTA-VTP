using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using fwapp;
using System.Configuration;
using System.Reflection;

namespace WScoes
{
    [ServiceContract]
    public class LogService : ILogService
    {
        public int ii_Version = 21001;
        public n_fw_data[] iL_data = new n_fw_data[2];
        public n_fw_app n_app = new n_fw_app();

        public LogService()
        {
            var t = typeof(Oracle.ManagedDataAccess.Client.OracleConnection);
            var myAssemblyName = AssemblyName.GetAssemblyName(t.Assembly.Location);
            iL_data[0] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SICOES"].ToString());
            iL_data[1] = new n_fw_data(DBType.Oracle, ConfigurationManager.ConnectionStrings["SCADA"].ToString());
        }

        public int nf_add_log(string as_app, string as_obj, string as_opc, string as_atrib, string as_paramdb, string as_valold, string as_valnew, string as_ip, string as_lastuser, int ai_codref)
        {
            int li_logcodi = 0;
            string commandText = "" + "insert into EXT_EVELOG(extevecodi,extapp,extobjeto,extopcion,extatrib,extparamdb,extvalold,extvalnew,extip,lastuser,excodref,lasdate) " + " values(";
            try
            {
                li_logcodi = iL_data[0].nf_get_next_key("EXT_EVELOG");

                commandText += li_logcodi + ",'" + as_app + "','" + as_obj + "','" + as_opc + "','" + as_atrib + "','" + as_paramdb + "','" + as_valold + "','" + as_valnew + "','" + as_ip + "','" + as_lastuser + "'," + ai_codref + ",sysdate)";

                int numrows = iL_data[0].nf_ExecuteNonQuery(commandText);

                if (numrows > 0)
                {
                    return numrows;
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
