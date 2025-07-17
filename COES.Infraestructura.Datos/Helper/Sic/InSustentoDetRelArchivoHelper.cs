using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SUSTENTO_DET_REL_ARCHIVO
    /// </summary>
    public class InSustentoDetRelArchivoHelper : HelperBase
    {
        public InSustentoDetRelArchivoHelper() : base(Consultas.InSustentoDetRelArchivoSql)
        {
        }

        public InSustentoDetRelArchivoDTO Create(IDataReader dr)
        {
            InSustentoDetRelArchivoDTO entity = new InSustentoDetRelArchivoDTO();

            int iInstdcodi = dr.GetOrdinal(this.Instdcodi);
            if (!dr.IsDBNull(iInstdcodi)) entity.Instdcodi = Convert.ToInt32(dr.GetValue(iInstdcodi));

            int iInarchcodi = dr.GetOrdinal(this.Inarchcodi);
            if (!dr.IsDBNull(iInarchcodi)) entity.Inarchcodi = Convert.ToInt32(dr.GetValue(iInarchcodi));

            int iIsdarcodi = dr.GetOrdinal(this.Isdarcodi);
            if (!dr.IsDBNull(iIsdarcodi)) entity.Isdarcodi = Convert.ToInt32(dr.GetValue(iIsdarcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Instdcodi = "INSTDCODI";
        public string Inarchcodi = "INARCHCODI";
        public string Isdarcodi = "ISDARCODI";

        #endregion

    }
}
