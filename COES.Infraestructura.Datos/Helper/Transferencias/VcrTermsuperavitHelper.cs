using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_TERMSUPERAVIT
    /// </summary>
    public class VcrTermsuperavitHelper : HelperBase
    {
        public VcrTermsuperavitHelper(): base(Consultas.VcrTermsuperavitSql)
        {
        }

        public VcrTermsuperavitDTO Create(IDataReader dr)
        {
            VcrTermsuperavitDTO entity = new VcrTermsuperavitDTO();

            int iVcrtscodi = dr.GetOrdinal(this.Vcrtscodi);
            if (!dr.IsDBNull(iVcrtscodi)) entity.Vcrtscodi = Convert.ToInt32(dr.GetValue(iVcrtscodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrtsfecha = dr.GetOrdinal(this.Vcrtsfecha);
            if (!dr.IsDBNull(iVcrtsfecha)) entity.Vcrtsfecha = dr.GetDateTime(iVcrtsfecha);

            int iVcrtsmpa = dr.GetOrdinal(this.Vcrtsmpa);
            if (!dr.IsDBNull(iVcrtsmpa)) entity.Vcrtsmpa = dr.GetDecimal(iVcrtsmpa);

            int iVcrtsdefmwe = dr.GetOrdinal(this.Vcrtsdefmwe);
            if (!dr.IsDBNull(iVcrtsdefmwe)) entity.Vcrtsdefmwe = dr.GetDecimal(iVcrtsdefmwe);

            int iVcrtssupmwe = dr.GetOrdinal(this.Vcrtssupmwe);
            if (!dr.IsDBNull(iVcrtssupmwe)) entity.Vcrtssupmwe = dr.GetDecimal(iVcrtssupmwe);

            int iVcrtsrnsmwe = dr.GetOrdinal(this.Vcrtsrnsmwe);
            if (!dr.IsDBNull(iVcrtsrnsmwe)) entity.Vcrtsrnsmwe = dr.GetDecimal(iVcrtsrnsmwe);

            int iVcrtsdeficit = dr.GetOrdinal(this.Vcrtsdeficit);
            if (!dr.IsDBNull(iVcrtsdeficit)) entity.Vcrtsdeficit = dr.GetDecimal(iVcrtsdeficit);

            int iVcrtssuperavit = dr.GetOrdinal(this.Vcrtssuperavit);
            if (!dr.IsDBNull(iVcrtssuperavit)) entity.Vcrtssuperavit = dr.GetDecimal(iVcrtssuperavit);

            int iVcrtsresrvnosumn = dr.GetOrdinal(this.Vcrtsresrvnosumn);
            if (!dr.IsDBNull(iVcrtsresrvnosumn)) entity.Vcrtsresrvnosumn = dr.GetDecimal(iVcrtsresrvnosumn);

            int iVcrtsusucreacion = dr.GetOrdinal(this.Vcrtsusucreacion);
            if (!dr.IsDBNull(iVcrtsusucreacion)) entity.Vcrtsusucreacion = dr.GetString(iVcrtsusucreacion);

            int iVcrtsfeccreacion = dr.GetOrdinal(this.Vcrtsfeccreacion);
            if (!dr.IsDBNull(iVcrtsfeccreacion)) entity.Vcrtsfeccreacion = dr.GetDateTime(iVcrtsfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrtscodi = "VCRTSCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrtsfecha = "VCRTSFECHA";
        public string Vcrtsmpa = "VCRTSMPA";
        public string Vcrtsdefmwe = "VCRTSDEFMWE";
        public string Vcrtssupmwe = "VCRTSSUPMWE";
        public string Vcrtsrnsmwe = "VCRTSRNSMWE";
        public string Vcrtsdeficit = "VCRTSDEFICIT";
        public string Vcrtssuperavit = "VCRTSSUPERAVIT";
        public string Vcrtsresrvnosumn = "VCRTSRESRVNOSUMN";
        public string Vcrtsusucreacion = "VCRTSUSUCREACION";
        public string Vcrtsfeccreacion = "VCRTSFECCREACION";

        #endregion

        public string SqlListPorMesURS
        {
            get { return base.GetSqlXml("ListPorMesURS"); }
        }

        public string SqlGetByIdDia
        {
            get { return base.GetSqlXml("GetByIdDia"); }
        }
    }
}
