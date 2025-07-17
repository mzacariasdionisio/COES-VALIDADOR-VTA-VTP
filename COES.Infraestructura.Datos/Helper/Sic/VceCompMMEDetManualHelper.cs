using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VceCompMMEDetManualHelper : HelperBase
    {
        public VceCompMMEDetManualHelper() : base(Consultas.VceCompMMEDetManualSql)
        {
        }
        public VceCompMMEDetManualDetDTO Create(IDataReader dr)
        {
            VceCompMMEDetManualDetDTO entity = new VceCompMMEDetManualDetDTO();

            int iCrdettipocalc = dr.GetOrdinal(this.Cmmedmtipocalc);
            if (!dr.IsDBNull(iCrdettipocalc)) entity.Cmmedmtipocalc = dr.GetString(iCrdettipocalc);

            int iCrdetcvtbajaefic = dr.GetOrdinal(this.Cmmedmconsumocomb);
            if (!dr.IsDBNull(iCrdetcvtbajaefic)) entity.Cmmedmconsumocomb = dr.GetDecimal(iCrdetcvtbajaefic);

            int iCrdetcompensacion = dr.GetOrdinal(this.Cmmedmcompensacion);
            if (!dr.IsDBNull(iCrdetcompensacion)) entity.Cmmedmcompensacion = dr.GetDecimal(iCrdetcompensacion);

            int iCrdetcmg = dr.GetOrdinal(this.Cmmedmcmg);
            if (!dr.IsDBNull(iCrdetcmg)) entity.Cmmedmcmg = dr.GetDecimal(iCrdetcmg);

            int iCrdetcvt = dr.GetOrdinal(this.Cmmedmcvt);
            if (!dr.IsDBNull(iCrdetcvt)) entity.Cmmedmcvt = dr.GetDecimal(iCrdetcvt);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iCrdetvalor = dr.GetOrdinal(this.Cmmedmenergia);
            if (!dr.IsDBNull(iCrdetvalor)) entity.Cmmedmenergia = dr.GetDecimal(iCrdetvalor);

            int iCrdethora = dr.GetOrdinal(this.Cmmedmhora);
            if (!dr.IsDBNull(iCrdethora)) entity.Cmmedmhora = dr.GetDateTime(iCrdethora);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            int iCmmedmpotencia = dr.GetOrdinal(this.Cmmedmpotencia);
            if (!dr.IsDBNull(iCmmedmpotencia)) entity.Cmmedmpotencia = dr.GetDecimal(iCmmedmpotencia);

            int iCmmedmprecioaplic = dr.GetOrdinal(this.Cmmedmprecioaplic);
            if (!dr.IsDBNull(iCmmedmprecioaplic)) entity.Cmmedmprecioaplic = dr.GetDecimal(iCmmedmprecioaplic);

            int iCmmedmcvc = dr.GetOrdinal(this.Cmmedmcvc);
            if (!dr.IsDBNull(iCmmedmcvc)) entity.Cmmedmcvc = dr.GetDecimal(iCmmedmcvc);

            int iCmmedmcvnc = dr.GetOrdinal(this.Cmmedmcvnc);
            if (!dr.IsDBNull(iCmmedmcvnc)) entity.Cmmedmcvnc = dr.GetDecimal(iCmmedmcvnc);

            return entity;
        }

        #region Mapeo de Campos

        public string Cmmedmtipocalc = "CMMEDMTIPOCALC";
        public string Cmmedmconsumocomb = "CMMEDMCONSUMOCOMB";
        public string Cmmedmcompensacion = "CMMEDMCOMPENSACION";
        public string Cmmedmcmg = "CMMEDMCMG";
        public string Cmmedmcvt = "CMMEDMCVT";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Cmmedmenergia = "CMMEDMENERGIA";
        public string Cmmedmhora = "CMMEDMHORA";
        public string Grupocodi = "GRUPOCODI";
        public string Pecacodi = "PECACODI";

        public string Cmmedmpotencia = "CMMEDMPOTENCIA";
        public string Cmmedmprecioaplic = "CMMEDMPRECIOAPLIC";
        public string Cmmedmcvc = "CMMEDMCVC";
        public string Cmmedmcvnc = "CMMEDMCVNC";

        //Adicionales
        public string Emprnomb = "EMPRNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Subcausadesc = "SUBCAUSADESC";

        #endregion



        public string SqlSaveManual
        {
            get { return base.GetSqlXml("SaveManual"); }
        }

        
        public string SqlDeleteManual
        {
            get { return base.GetSqlXml("DeleteCompensacionManual"); }
        }

        public string SqUpdateCompensacionDet
        {
            get { return base.GetSqlXml("UpdateCompensacionDet"); }
        }

        public string SqlSaveCompensacionDet
        {
            get { return base.GetSqlXml("SaveCompensacionDet"); }
        }        

        public string SqlListCompensacionManual
        {
            get { return base.GetSqlXml("ListCompensacionManual"); }
        }

        public string SqlDeleteManualByVersion
        {
            get { return base.GetSqlXml("DeleteCompensacionManualByVersion"); }
        }

        public string SqlUpdateCompensacionRegular
        {
            get { return base.GetSqlXml("UpdateCompensacionRegular"); }
        }

        public string SqlUpdateCompensacionRegularlByVersion
        {
            get { return base.GetSqlXml("UpdateCompensacionRegularlByVersion"); }
        }
    }
}
