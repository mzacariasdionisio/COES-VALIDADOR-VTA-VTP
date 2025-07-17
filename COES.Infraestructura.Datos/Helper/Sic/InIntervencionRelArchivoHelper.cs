using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_INTERVENCION_REL_ARCHIVO
    /// </summary>
    public class InIntervencionRelArchivoHelper : HelperBase
    {
        public InIntervencionRelArchivoHelper() : base(Consultas.InIntervencionRelArchivoSql)
        {
        }

        public InIntervencionRelArchivoDTO Create(IDataReader dr)
        {
            InIntervencionRelArchivoDTO entity = new InIntervencionRelArchivoDTO();

            int iIrarchcodi = dr.GetOrdinal(this.Irarchcodi);
            if (!dr.IsDBNull(iIrarchcodi)) entity.Irarchcodi = Convert.ToInt32(dr.GetValue(iIrarchcodi));

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iInarchcodi = dr.GetOrdinal(this.Inarchcodi);
            if (!dr.IsDBNull(iInarchcodi)) entity.Inarchcodi = Convert.ToInt32(dr.GetValue(iInarchcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Irarchcodi = "IRARCHCODI";
        public string Intercodi = "INTERCODI";
        public string Inarchcodi = "INARCHCODI";

        #endregion
    }
}
