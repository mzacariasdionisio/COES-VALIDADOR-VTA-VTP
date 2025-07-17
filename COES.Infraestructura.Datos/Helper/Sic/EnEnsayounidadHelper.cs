using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ENSAYOUNIDAD
    /// </summary>
    public class EnEnsayounidadHelper : HelperBase
    {
        public EnEnsayounidadHelper()
            : base(Consultas.EnEnsayounidadSql)
        {
        }

        public EnEnsayounidadDTO Create(IDataReader dr)
        {
            EnEnsayounidadDTO entity = new EnEnsayounidadDTO();

            int iEnunidadcodi = dr.GetOrdinal(this.Enunidadcodi);
            if (!dr.IsDBNull(iEnunidadcodi)) entity.Enunidadcodi = Convert.ToInt32(dr.GetValue(iEnunidadcodi));

            int iEnsayocodi = dr.GetOrdinal(this.Ensayocodi);
            if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Enunidadcodi = "ENUNIDCODI";
        public string Ensayocodi = "ENSAYOCODI";
        public string Equicodi = "EQUICODI";

        #endregion


        public string SqlGetEnsayoUnidad
        {
            get { return base.GetSqlXml("TraerEnsayoUnidad"); }
        }

        public string SqlGetUnidadesXEnsayo
        {
            get { return base.GetSqlXml("GetUnidadesXEnsayo"); }
        }
    }
}
