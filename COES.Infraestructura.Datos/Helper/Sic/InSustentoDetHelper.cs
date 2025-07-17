using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SUSTENTO_DET
    /// </summary>
    public class InSustentoDetHelper : HelperBase
    {
        public InSustentoDetHelper() : base(Consultas.InSustentoDetSql)
        {
        }

        public InSustentoDetDTO Create(IDataReader dr)
        {
            InSustentoDetDTO entity = new InSustentoDetDTO();

            int iInstcodi = dr.GetOrdinal(this.Instcodi);
            if (!dr.IsDBNull(iInstcodi)) entity.Instcodi = Convert.ToInt32(dr.GetValue(iInstcodi));

            int iInpsticodi = dr.GetOrdinal(this.Inpsticodi);
            if (!dr.IsDBNull(iInpsticodi)) entity.Inpsticodi = Convert.ToInt32(dr.GetValue(iInpsticodi));

            int iInstdcodi = dr.GetOrdinal(this.Instdcodi);
            if (!dr.IsDBNull(iInstdcodi)) entity.Instdcodi = Convert.ToInt32(dr.GetValue(iInstdcodi));

            int iInstdrpta = dr.GetOrdinal(this.Instdrpta);
            if (!dr.IsDBNull(iInstdrpta)) entity.Instdrpta = dr.GetString(iInstdrpta);

            return entity;
        }

        #region Mapeo de Campos

        public string Instcodi = "INSTCODI";
        public string Inpsticodi = "INPSTICODI";
        public string Instdcodi = "INSTDCODI";
        public string Instdrpta = "INSTDRPTA";

        public string Inpstidesc = "INPSTIDESC";
        public string Inpstitipo = "INPSTITIPO";

        #endregion

    }
}
