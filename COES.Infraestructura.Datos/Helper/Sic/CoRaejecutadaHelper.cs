using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_RAEJECUTADA
    /// </summary>
    public class CoRaejecutadaHelper : HelperBase
    {
        public CoRaejecutadaHelper(): base(Consultas.CoRaejecutadaSql)
        {
        }

        public CoRaejecutadaDTO Create(IDataReader dr)
        {
            CoRaejecutadaDTO entity = new CoRaejecutadaDTO();

            int iCoraejcodi = dr.GetOrdinal(this.Coraejcodi);
            if (!dr.IsDBNull(iCoraejcodi)) entity.Coraejcodi = Convert.ToInt32(dr.GetValue(iCoraejcodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iCoraejrasub = dr.GetOrdinal(this.Coraejrasub);
            if (!dr.IsDBNull(iCoraejrasub)) entity.Coraejrasub = dr.GetDecimal(iCoraejrasub);

            int iCoraejrabaj = dr.GetOrdinal(this.Coraejrabaj);
            if (!dr.IsDBNull(iCoraejrabaj)) entity.Coraejrabaj = dr.GetDecimal(iCoraejrabaj);

            int iCoraejfecha = dr.GetOrdinal(this.Coraejfecha);
            if (!dr.IsDBNull(iCoraejfecha)) entity.Coraejfecha = dr.GetDateTime(iCoraejfecha);

            int iCoraejfecini = dr.GetOrdinal(this.Coraejfecini);
            if (!dr.IsDBNull(iCoraejfecini)) entity.Coraejfecini = dr.GetDateTime(iCoraejfecini);

            int iCoraejfecfin = dr.GetOrdinal(this.Coraejfecfin);
            if (!dr.IsDBNull(iCoraejfecfin)) entity.Coraejfecfin = dr.GetDateTime(iCoraejfecfin);

            int iCoraejusucreacion = dr.GetOrdinal(this.Coraejusucreacion);
            if (!dr.IsDBNull(iCoraejusucreacion)) entity.Coraejusucreacion = dr.GetString(iCoraejusucreacion);

            int iCoraejfeccreacion = dr.GetOrdinal(this.Coraejfeccreacion);
            if (!dr.IsDBNull(iCoraejfeccreacion)) entity.Coraejfeccreacion = dr.GetDateTime(iCoraejfeccreacion);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Coraejcodi = "CORAEJCODI";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Coraejrasub = "CORAEJRASUB";
        public string Coraejrabaj = "CORAEJRABAJ";
        public string Coraejfecha = "CORAEJFECHA";
        public string Coraejfecini = "CORAEJFECINI";
        public string Coraejfecfin = "CORAEJFECFIN";
        public string Coraejusucreacion = "CORAEJUSUCREACION";
        public string Coraejfeccreacion = "CORAEJFECCREACION";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";

        #endregion
    }
}
