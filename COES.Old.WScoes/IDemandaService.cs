using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDemandaService" in both code and config file together.
    [ServiceContract]
    public interface IDemandaService
    {
        [OperationContract]
        CEquipoDemanda GetEquipo(int ai_codigoEquipo);

        [OperationContract]
        DataTable nf_get_empresa_detalles(int pi_emprcodi);

        [OperationContract]
        DataTable nf_DemandaBarraSemanal48FHora(DateTime pdt_fecha_rep, int pi_lectcodi, int pi_tipoinf, int pi_ptomedicodi);

        [OperationContract]
        DataTable nf_DemandaBarraReporteDiario48FHora(DateTime pdt_fini, DateTime pdt_ffin, int pi_lectcodi, int pi_tipoinf, int pi_ptomedicodi);

        [OperationContract]
        DataTable nf_DemandaBarraDiario48FHora(DateTime pdt_fini, DateTime pdt_ffin, int pi_lectcodi, int pi_tipoinf, int pi_ptomedicodi);

        [OperationContract]
        DataTable nf_DemandaBarraDiario48FHora(DateTime pdt_fecha_rep, int pi_lectcodi_real, int pi_lectcodi_prog, int pi_tipoinf_mw, int pi_ptomedicodi);


        [OperationContract]
        DataTable nf_get_medicion48(DateTime adt_fechaIni, DateTime adt_fechaFin);

        [OperationContract]
        DataTable nf_get_medicion48(DateTime adt_fini, DateTime adt_ffin, int ai_lectcodi, int ai_tipoinf, int ai_ptomedicodi);

        [OperationContract]
        DataTable nf_get_PuntoMedicion(int pi_codigoPtoMedicion, int pi_codigoEmpresa, int pi_codigoTipoEmpresa);

        [OperationContract]
        DataTable nf_get_puntos_medicion_x_empresa(int ai_emprcodi, int ai_tipo_empresa_codigo);

        [OperationContract]
        DataTable nf_get_PuntoMedicion(int ai_codigoTipoEmpresa);

        [OperationContract]
        int nf_set_insert_envio(int pi_earcodi, int pi_etacodi, int pi_tipoenvio, int pi_plazo, DateTime pdt_fechaenvio, DateTime pdt_fechareporte, string ps_lastuser);

        [OperationContract]
        int nf_set_insert_archivo_envio(int ai_etacodi, string as_eararchnomb, double as_eararchtammb, string as_eararchver, string as_eararchruta, int ai_usercode, string as_earip, string as_lastuser, DateTime adt_earfecha, int ai_estenvcodi, string as_earcopiado, int ai_emprcodi);

        [OperationContract]
        int nf_upd_env_copiado(int ai_earcodi, bool ab_copiado, string as_lastuser);

        [OperationContract]
        int nf_upd_env_estado(int ai_earcodi, int ai_estado, string as_lastuser);

        [OperationContract]
        DataSet nf_get_FileDemandaExcel(string ps_path, string ps_extension);

        [OperationContract]
        DataTable nf_get_sql(string query);
    }
}
