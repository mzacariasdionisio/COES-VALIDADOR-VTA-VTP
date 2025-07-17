using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_IEODCUADRO_DET
    /// </summary>
    public class EveIeodcuadroDetHelper : HelperBase
    {
        public EveIeodcuadroDetHelper(): base(Consultas.EveIeodcuadroDetSql)
        {
        }

        public EveIeodcuadroDetDTO Create(IDataReader dr)
        {
            EveIeodcuadroDetDTO entity = new EveIeodcuadroDetDTO();

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIcdetcheck1 = dr.GetOrdinal(this.Icdetcheck1);
            if (!dr.IsDBNull(iIcdetcheck1)) entity.Icdetcheck1 = dr.GetString(iIcdetcheck1);

            return entity;
        }


        #region Mapeo de Campos

        public string Iccodi = "ICCODI";
        public string Equicodi = "EQUICODI";
        public string Icdetcheck1 = "ICDETCHECK1";

        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";
        public string Famabrev = "FAMABREV";
        public string Equiabrev = "EQUIABREV";


        #endregion

        public string SqlGetByCriteria
        {
            get { return base.GetSqlXml("KeyObtenerPorCriterio"); }
        }


    }
}
