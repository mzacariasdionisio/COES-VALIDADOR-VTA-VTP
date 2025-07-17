using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_VERSION
    /// </summary>
    public class CbVersionHelper : HelperBase
    {
        public CbVersionHelper() : base(Consultas.CbVersionSql)
        {
        }

        public CbVersionDTO Create(IDataReader dr)
        {
            CbVersionDTO entity = new CbVersionDTO();

            int iCbenvcodi = dr.GetOrdinal(this.Cbenvcodi);
            if (!dr.IsDBNull(iCbenvcodi)) entity.Cbenvcodi = Convert.ToInt32(dr.GetValue(iCbenvcodi));

            int iCbvercodi = dr.GetOrdinal(this.Cbvercodi);
            if (!dr.IsDBNull(iCbvercodi)) entity.Cbvercodi = Convert.ToInt32(dr.GetValue(iCbvercodi));

            int iCbvernumversion = dr.GetOrdinal(this.Cbvernumversion);
            if (!dr.IsDBNull(iCbvernumversion)) entity.Cbvernumversion = Convert.ToInt32(dr.GetValue(iCbvernumversion));

            int iCbverestado = dr.GetOrdinal(this.Cbverestado);
            if (!dr.IsDBNull(iCbverestado)) entity.Cbverestado = dr.GetString(iCbverestado);

            int iCbverusucreacion = dr.GetOrdinal(this.Cbverusucreacion);
            if (!dr.IsDBNull(iCbverusucreacion)) entity.Cbverusucreacion = dr.GetString(iCbverusucreacion);

            int iCbverfeccreacion = dr.GetOrdinal(this.Cbverfeccreacion);
            if (!dr.IsDBNull(iCbverfeccreacion)) entity.Cbverfeccreacion = dr.GetDateTime(iCbverfeccreacion);

            int iCbveroperacion = dr.GetOrdinal(this.Cbveroperacion);
            if (!dr.IsDBNull(iCbveroperacion)) entity.Cbveroperacion = Convert.ToInt32(dr.GetValue(iCbveroperacion));

            int iCbverdescripcion = dr.GetOrdinal(this.Cbverdescripcion);
            if (!dr.IsDBNull(iCbverdescripcion)) entity.Cbverdescripcion = dr.GetString(iCbverdescripcion);

            int iCbverconexion = dr.GetOrdinal(this.Cbverconexion);
            if (!dr.IsDBNull(iCbverconexion)) entity.Cbverconexion = Convert.ToInt32(dr.GetValue(iCbverconexion));

            int iCbvertipo = dr.GetOrdinal(this.Cbvertipo);
            if (!dr.IsDBNull(iCbvertipo)) entity.Cbvertipo = Convert.ToInt32(dr.GetValue(iCbvertipo));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbenvcodi = "CBENVCODI";
        public string Cbvercodi = "CBVERCODI";
        public string Cbvernumversion = "CBVERNUMVERSION";
        public string Cbverestado = "CBVERESTADO";
        public string Cbverusucreacion = "CBVERUSUCREACION";
        public string Cbverfeccreacion = "CBVERFECCREACION";
        public string Cbveroperacion = "CBVEROPERACION";
        public string Cbverdescripcion = "CBVERDESCRIPCION";
        public string Cbverconexion = "CBVERCONEXION";
        public string Cbvertipo = "CBVERTIPO";

        #endregion

        public string SqlCambiarEstado
        {
            get { return GetSqlXml("CambiarEstado"); }
        }

        public string SqlGetByPeriodoyEstado
        {
            get { return GetSqlXml("GetByPeriodoyEstado"); }
        }
    }
}
