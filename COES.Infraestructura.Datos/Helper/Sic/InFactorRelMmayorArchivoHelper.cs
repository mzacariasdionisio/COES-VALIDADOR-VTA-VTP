using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_FACTOR_REL_MMAYOR_ARCHIVO
    /// </summary>
    public class InFactorRelMmayorArchivoHelper : HelperBase
    {
        public InFactorRelMmayorArchivoHelper(): base(Consultas.InFactorRelMmayorArchivoSql)
        {
        }

        public InFactorRelMmayorArchivoDTO Create(IDataReader dr)
        {
            InFactorRelMmayorArchivoDTO entity = new InFactorRelMmayorArchivoDTO();

            int iIrmarcodi = dr.GetOrdinal(this.Irmarcodi);
            if (!dr.IsDBNull(iIrmarcodi)) entity.Irmarcodi = Convert.ToInt32(dr.GetValue(iIrmarcodi));

            int iInfmmcodi = dr.GetOrdinal(this.Infmmcodi);
            if (!dr.IsDBNull(iInfmmcodi)) entity.Infmmcodi = Convert.ToInt32(dr.GetValue(iInfmmcodi));

            int iInarchcodi = dr.GetOrdinal(this.Inarchcodi);
            if (!dr.IsDBNull(iInarchcodi)) entity.Inarchcodi = Convert.ToInt32(dr.GetValue(iInarchcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Irmarcodi = "IRMARCODI";
        public string Infmmcodi = "INFMMCODI";
        public string Inarchcodi = "INARCHCODI";
        public string Infvercodi = "INFVERCODI";

        #endregion
    }
}
