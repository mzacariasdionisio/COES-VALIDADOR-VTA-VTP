using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_ESCENARIO
    /// </summary>
    public class PrEscenarioHelper : HelperBase
    {
        public PrEscenarioHelper(): base(Consultas.PrEscenarioSql)
        {
        }

        public PrEscenarioDTO Create(IDataReader dr)
        {
            PrEscenarioDTO entity = new PrEscenarioDTO();

            int iEscefecha = dr.GetOrdinal(this.Escefecha);
            if (!dr.IsDBNull(iEscefecha)) entity.Escefecha = dr.GetDateTime(iEscefecha);

            int iEscecodi = dr.GetOrdinal(this.Escecodi);
            if (!dr.IsDBNull(iEscecodi)) entity.Escecodi = Convert.ToInt32(dr.GetValue(iEscecodi));

            int iEscenum = dr.GetOrdinal(this.Escenum);
            if (!dr.IsDBNull(iEscenum)) entity.Escenum = Convert.ToInt32(dr.GetValue(iEscenum));

            int iEscenomb = dr.GetOrdinal(this.Escenomb);
            if (!dr.IsDBNull(iEscenomb)) entity.Escenomb = dr.GetString(iEscenomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Escefecha = "ESCEFECHA";
        public string Escecodi = "ESCECODI";
        public string Escenum = "ESCENUM";
        public string Escenomb = "ESCENOMB";

        #endregion
        public string SqlEscenarioPorFecha
        {
            get { return base.GetSqlXml("EscenarioPorFecha"); }
        }
        
    }
}
