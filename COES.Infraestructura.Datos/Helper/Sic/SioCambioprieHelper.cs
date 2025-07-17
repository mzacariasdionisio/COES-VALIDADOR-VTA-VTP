using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_CAMBIOPRIE
    /// </summary>
    public class SioCambioprieHelper : HelperBase
    {
        public SioCambioprieHelper() : base(Consultas.SioCambioprieSql)
        {
        }

        public SioCambioprieDTO Create(IDataReader dr)
        {
            SioCambioprieDTO entity = new SioCambioprieDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCamprifecmodificacion = dr.GetOrdinal(this.Camprifecmodificacion);
            if (!dr.IsDBNull(iCamprifecmodificacion)) entity.Camprifecmodificacion = dr.GetDateTime(iCamprifecmodificacion);

            int iCampriusumodificacion = dr.GetOrdinal(this.Campriusumodificacion);
            if (!dr.IsDBNull(iCampriusumodificacion)) entity.Campriusumodificacion = dr.GetString(iCampriusumodificacion);

            int iCamprivalor = dr.GetOrdinal(this.Camprivalor);
            if (!dr.IsDBNull(iCamprivalor)) entity.Camprivalor = dr.GetString(iCamprivalor);

            int iCabpricodi = dr.GetOrdinal(this.Cabpricodi);
            if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

            int iCampricodi = dr.GetOrdinal(this.Campricodi);
            if (!dr.IsDBNull(iCampricodi)) entity.Campricodi = Convert.ToInt32(dr.GetValue(iCampricodi));

            return entity;
        }


        #region Mapeo de Campos
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Barrcodi = "BARRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprcodi2 = "EMPRCODI2";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Camprifecmodificacion = "CAMPRIFECMODIFICACION";
        public string Campriusumodificacion = "CAMPRIUSUMODIFICACION";
        public string Camprivalor = "CAMPRIVALOR";
        public string Cabpricodi = "CABPRICODI";
        public string Campricodi = "CAMPRICODI";

        #endregion
    }
}
