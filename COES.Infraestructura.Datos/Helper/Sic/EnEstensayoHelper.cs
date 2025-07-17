using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ESTENSAYO
    /// </summary>
    public class EnEstensayoHelper : HelperBase
    {
        public EnEstensayoHelper(): base(Consultas.EnEstensayoSql)
        {
        }

        public EnEstensayoDTO Create(IDataReader dr)
        {
            EnEstensayoDTO entity = new EnEstensayoDTO();

            int iEnsayocodi = dr.GetOrdinal(this.Ensayocodi);
            if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iEstensayofecha = dr.GetOrdinal(this.Estensayofecha);
            if (!dr.IsDBNull(iEstensayofecha)) entity.Estensayofecha = dr.GetDateTime(iEstensayofecha);

            int iEstensayouser = dr.GetOrdinal(this.Estensayouser);
            if (!dr.IsDBNull(iEstensayouser)) entity.Estensayouser = dr.GetString(iEstensayouser);

            return entity;
        }


        #region Mapeo de Campos

        public string Ensayocodi = "ENSAYOCODI";
        public string Estadocodi = "ESTADOCODI";
        public string Estensayofecha = "ESTENSAYOFECHA";
        public string Estensayouser = "ESTENSAYOUSER";

        #endregion
    }
}
