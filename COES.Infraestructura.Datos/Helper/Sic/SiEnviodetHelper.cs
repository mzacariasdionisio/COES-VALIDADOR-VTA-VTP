using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_ENVIO
    /// </summary>
    public class SiEnviodetHelper : HelperBase
    {
        public SiEnviodetHelper(): base(Consultas.SiEnviodetSql)
        {
        }

        public SiEnviodetDTO Create(IDataReader dr)
        {
            SiEnviodetDTO entity = new SiEnviodetDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iFdatpkcodi = dr.GetOrdinal(this.Fdatpkcodi);
            if (!dr.IsDBNull(iFdatpkcodi)) entity.Fdatpkcodi = Convert.ToInt32(dr.GetValue(iFdatpkcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Fdatpkcodi = "FDATPKCODI";
        

        #endregion        
    }
}