using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PmoDatGenerateHelper : HelperBase
    {
        public PmoDatGenerateHelper()
            : base(Consultas.PmoDatGenerate)
        {
        }
        public string SqlGenerateDat
        {
            get { return base.GetSqlXml("GenerateDat"); }
        }
        #region GDC 20190225 - Procesamiento de archivos DAT
        public string SqlDeleteDataDisponibilidadPorPeriodoYtipo
        {
            get { return base.GetSqlXml("DeleteDataDisponibilidadPorPeriodoYtipo"); }
        }
        public string SqlGetFechasProcesamientoDisponibilidad
        {
            get { return base.GetSqlXml("GetFechasProcesamientoDisponibilidad"); }
        }

        public string SqlGetDataGrupoRelaso
        {
            get { return base.GetSqlXml("GetDataGrupoRelaso"); }
        }

        public string SqlGenerateDatCgnd
        {
            get { return base.GetSqlXml("GenerateDatCgnd"); }
        }

        public string SqlGenerateDatMgnd
        {
            get { return base.GetSqlXml("GenerateDatMgnd"); }
        }
        public string SqlGenerateDatMgndPtoInstFactOpe
        {
            get { return base.GetSqlXml("GenerateDatMgndPtoInstFactOpe"); }
        }
        

        public string Pmperianiomes = "PMPERIANIOMES";
        public string Rdgfechainicio = "RDGFECHAINICIO";
        public string Rdgfechafin = "RDGFECHAFIN";
        public string Fechaperiodo = "FECHAPERIODO";
        public string Mantfechainicio = "MANTFECHAINICIO";
        public string Mantfechafin = "MANTFECHAFIN";

        public string anio = "ANIO";
        public string mes = "MES";

        #endregion

        public string VTableNameVal = "V_TABLE_NAME_VAL";
        public string VPMPeriCodiVal = "V_PMPERICODI_VAL";
        public string VUsuarioVal = "V_USUARIO_VAL";
        public string VFechaVal = "V_FECHA_VAL";

        public string Tsddpcodi = "TSDDPCODI";
        public string Sddpcodi = "SDDPCODI";
        public string Sddpnum = "SDDPNUM";
        public string Sddpnomb = "SDDPNOMB";
    }
}
