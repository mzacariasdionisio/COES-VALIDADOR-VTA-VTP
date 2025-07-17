using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_PARAMETROPLAZO
    /// </summary>
    public class InParametroplazoHelper : HelperBase
    {
        public InParametroplazoHelper() : base(Consultas.InParametroplazoSql)
        {
        }

        public InParametroPlazoDTO Create(IDataReader dr)
        {
            InParametroPlazoDTO entity = new InParametroPlazoDTO();

            int iParplacodi = dr.GetOrdinal(this.Parplacodi);
            if (!dr.IsDBNull(iParplacodi)) entity.Parplacodi = Convert.ToInt32(dr.GetValue(iParplacodi));

            int iParpladesc = dr.GetOrdinal(this.Parpladesc);
            if (!dr.IsDBNull(iParpladesc)) entity.Parpladesc = dr.GetString(iParpladesc);

            int iParplafecdesde = dr.GetOrdinal(this.Parplafecdesde);
            if (!dr.IsDBNull(iParplafecdesde)) entity.Parplafecdesde = dr.GetDateTime(iParplafecdesde);

            int iParplafechasta = dr.GetOrdinal(this.Parplafechasta);
            if (!dr.IsDBNull(iParplafechasta)) entity.Parplafechasta = dr.GetDateTime(iParplafechasta);

            int iParplahora = dr.GetOrdinal(this.Parplahora);
            if (!dr.IsDBNull(iParplahora)) entity.Parplahora = dr.GetString(iParplahora);

            int iParplasucreacion = dr.GetOrdinal(this.Parplasucreacion);
            if (!dr.IsDBNull(iParplasucreacion)) entity.Parplasucreacion = dr.GetString(iParplasucreacion);

            int iParplafeccreacion = dr.GetOrdinal(this.Parplafeccreacion);
            if (!dr.IsDBNull(iParplafeccreacion)) entity.Parplafeccreacion = dr.GetDateTime(iParplafeccreacion);

            int iParplausumodificacion = dr.GetOrdinal(this.Parplausumodificacion);
            if (!dr.IsDBNull(iParplausumodificacion)) entity.Parplausumodificacion = dr.GetString(iParplausumodificacion);

            int iParplafecmodificacion = dr.GetOrdinal(this.Parplafecmodificacion);
            if (!dr.IsDBNull(iParplafecmodificacion)) entity.Parplafecmodificacion = dr.GetDateTime(iParplafecmodificacion);

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Parplacodi = "PARPLACODI";
        public string Parpladesc = "PARPLADESC";
        public string Parplafecdesde = "PARPLAFECDESDE";
        public string Parplafechasta = "PARPLAFECHASTA";
        public string Parplahora = "PARPLAHORA";
        public string Parplasucreacion = "PARPLASUCREACION";
        public string Parplafeccreacion = "PARPLAFECCREACION";
        public string Parplausumodificacion = "PARPLAUSUMODIFICACION";
        public string Parplafecmodificacion = "PARPLAFECMODIFICACION";

        public string Progrcodi = "PROGRCODI";

        #endregion
    }
}
