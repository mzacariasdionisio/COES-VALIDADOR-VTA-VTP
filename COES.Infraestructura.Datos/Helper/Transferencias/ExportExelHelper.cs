using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la de las consultas para los reportes
    /// </summary>
    public class ExportExelHelper : HelperBase
    {
        public ExportExelHelper()
            : base(Consultas.ExportExcel)
        {
        }

        public ExportExcelDTO Create(IDataReader dr)
        {
            ExportExcelDTO entity = new ExportExcelDTO();

            int iCODIENTRCODI = dr.GetOrdinal(this.CODIENTRCODI);
            if (!dr.IsDBNull(iCODIENTRCODI)) entity.CodiEntreRetiCodi = dr.GetInt32(iCODIENTRCODI);

            int iEMPRNOMB = dr.GetOrdinal(this.EMPRNOMB);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

            int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.BARRNOMBBARRTRAN);
            if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

            int iCODIENTRCODIGO = dr.GetOrdinal(this.CODIENTRCODIGO);
            if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntreRetiCodigo = dr.GetString(iCODIENTRCODIGO);

            int iTipo = dr.GetOrdinal(this.Tipo);
            if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

            int iCENTGENENOMBRE = dr.GetOrdinal(this.CENTGENENOMBRE);
            if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneCliNombre = dr.GetString(iCENTGENENOMBRE);

            return entity;
        }

        #region Mapeo de Campos
        public string CODIENTRCODI = "CODENTCODI";
        public string EMPRNOMB = "EMPRNOMB";
        public string EMPRRUC = "EMPRRUC";
        public string BARRNOMBBARRTRAN = "BARRBARRATRANSFERENCIA";
        public string CODIENTRCODIGO = "CODENTCODIGO";
        public string Tipo = "TIPO";
        public string CENTGENENOMBRE = "EQUINOMB";
        public string EMPRCODI = "EMPRCODI";
        public string BARRCODI = "BARRCODI";
        //Para ExportCuadro
        public string PERICODI = "PERICODI";
        public string VTRANVERSION = "VTRANVERSION";
        public string VT = "VT";
        public string ST = "ST";
        public string SCDSC = "SCDSC";
        public string COMPENSACION = "COMPENSACION";
        public string TOTAL = "TOTAL";
        //Para Compensacion
        public string INGCOMVERSION = "INGCOMVERSION";
        public string INGCOMIMPORTE = "INGCOMIMPORTE";
        public string CABCOMNOMBRE = "CABCOMNOMBRE";
        //Matriz
        public string EMPPAGVERSION = "emppagversion";
        public string EMPRNOMBPAGO = "EMPRNOMBPAGO";
        public string EMPPAGMONTO = "EMPPAGMONTO";
        //Para Informacion Base
        public string COINFBCODI = "COINFBCODI";
        public string COINFBCODIGO = "COINFBCODIGO";

        public string EmpPagoCodi = "EmpPagoCodi";
        public string TRNENVCODI = "TRNENVCODI";
        public string TRNMODCODI = "TRNMODCODI";
        #endregion

        public string SqlGetByCriteriaListaVistaTodo
        {
            get { return base.GetSqlXml("ListVistaTodo"); }
        }

        public string SqlGetByCriteriaPeriVer
        {
            get { return base.GetSqlXml("GetByCriteriaPeriVer"); }
        }

        public string SqlGetByCriteriaEmprPeriVer
        {
            get { return base.GetSqlXml("GetByCriteriaEmprPeriVer"); }
        }

        public string SqlGetGetMatrizByCriteriPeriVer
        {
            get { return base.GetSqlXml("GetMatrizByCriteriPeriVer"); }
        }

        public string SqlGetMatrizEmprByCriteriPeriVer
        {
            get { return base.GetSqlXml("GetMatrizEmprByCriteriPeriVer"); }
        }

        public string SqlGetByListCodigoInfoBase
        {
            get { return base.GetSqlXml("ListCodigoInfoBase"); }
        }

        public string SqlGetByListCodigoInfoBaseByEnvio
        {
            get { return base.GetSqlXml("GetByListCodigoInfoBaseByEnvio"); }
        }

        public string SqlGetByListCodigoModelo
        {
            get { return base.GetSqlXml("GetByListCodigoModelo"); }
        }

        public string SqlGetByListCodigoModeloVTA
        {
            get { return base.GetSqlXml("GetByListCodigoModeloVTA"); }
        }
    }
    
}
