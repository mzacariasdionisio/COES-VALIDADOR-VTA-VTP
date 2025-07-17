using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_GPSAISLADO
    /// </summary>
    public class EveGpsaisladoHelper : HelperBase
    {
        public EveGpsaisladoHelper() : base(Consultas.EveGpsaisladoSql)
        {
        }

        public EveGpsaisladoDTO Create(IDataReader dr)
        {
            EveGpsaisladoDTO entity = new EveGpsaisladoDTO();

            int iGpsaisfeccreacion = dr.GetOrdinal(this.Gpsaisfeccreacion);
            if (!dr.IsDBNull(iGpsaisfeccreacion)) entity.Gpsaisfeccreacion = dr.GetDateTime(iGpsaisfeccreacion);

            int iGpsaisusucreacion = dr.GetOrdinal(this.Gpsaisusucreacion);
            if (!dr.IsDBNull(iGpsaisusucreacion)) entity.Gpsaisusucreacion = dr.GetString(iGpsaisusucreacion);

            int iGpsprincipal = dr.GetOrdinal(this.Gpsaisprincipal);
            if (!dr.IsDBNull(iGpsprincipal)) entity.Gpsaisprincipal = Convert.ToInt32(dr.GetValue(iGpsprincipal));

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iGpsaiscodi = dr.GetOrdinal(this.Gpsaiscodi);
            if (!dr.IsDBNull(iGpsaiscodi)) entity.Gpsaiscodi = Convert.ToInt32(dr.GetValue(iGpsaiscodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Gpsaisfeccreacion = "GPSAISFECCREACION";
        public string Gpsaisusucreacion = "GPSAISUSUCREACION";
        public string Gpsaisprincipal = "GPSAISPRINCIPAL";
        public string Gpscodi = "GPSCODI";
        public string Iccodi = "ICCODI";
        public string Gpsaiscodi = "GPSAISCODI";

        public string Gpsnombre = "NOMBRE";
        public string Gpsosinerg = "GPSOSINERG";

        #endregion

        public string SqlDeleteByIccodi
        {
            get { return base.GetSqlXml("DeleteByIccodi"); }
        }
    }
}
