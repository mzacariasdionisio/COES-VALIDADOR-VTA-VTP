using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_RSFEQUIVALENCIA
    /// </summary>
    public class EveRsfequivalenciaHelper : HelperBase
    {
        public EveRsfequivalenciaHelper(): base(Consultas.EveRsfequivalenciaSql)
        {
        }

        public EveRsfequivalenciaDTO Create(IDataReader dr)
        {
            EveRsfequivalenciaDTO entity = new EveRsfequivalenciaDTO();

            int iRsfequcodi = dr.GetOrdinal(this.Rsfequcodi);
            if (!dr.IsDBNull(iRsfequcodi)) entity.Rsfequcodi = Convert.ToInt32(dr.GetValue(iRsfequcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRsfequagccent = dr.GetOrdinal(this.Rsfequagccent);
            if (!dr.IsDBNull(iRsfequagccent)) entity.Rsfequagccent = dr.GetString(iRsfequagccent);

            int iRsfequagcuni = dr.GetOrdinal(this.Rsfequagcuni);
            if (!dr.IsDBNull(iRsfequagcuni)) entity.Rsfequagcuni = dr.GetString(iRsfequagcuni);

            int iRsfequlastuser = dr.GetOrdinal(this.Rsfequlastuser);
            if (!dr.IsDBNull(iRsfequlastuser)) entity.Rsfequlastuser = dr.GetString(iRsfequlastuser);

            int iRsfequlastdate = dr.GetOrdinal(this.Rsfequlastdate);
            if (!dr.IsDBNull(iRsfequlastdate)) entity.Rsfequlastdate = dr.GetDateTime(iRsfequlastdate);

            int iRsfequindicador = dr.GetOrdinal(this.Rsfequindicador);
            if (!dr.IsDBNull(iRsfequindicador)) entity.Rsfequindicador = dr.GetString(iRsfequindicador);

            int iRsfequminimo = dr.GetOrdinal(this.Rsfequminimo);
            if (!dr.IsDBNull(iRsfequminimo)) entity.Rsfequminimo = dr.GetDecimal(iRsfequminimo);

            int iRsfequmaximo = dr.GetOrdinal(this.Rsfequmaximo);
            if (!dr.IsDBNull(iRsfequmaximo)) entity.Rsfequmaximo = dr.GetDecimal(iRsfequmaximo);

            int iRsfequrecurcodi = dr.GetOrdinal(this.Rsfequrecurcodi);
            if (!dr.IsDBNull(iRsfequrecurcodi)) entity.Rsfequrecurcodi = dr.GetString(iRsfequrecurcodi);

            int iRsfequasignacion = dr.GetOrdinal(this.Rsfequasignacion);
            if (!dr.IsDBNull(iRsfequasignacion)) entity.Rsfequasignacion = Convert.ToInt32(dr.GetValue(iRsfequasignacion));

            return entity;
        }


        #region Mapeo de Campos

        public string Rsfequcodi = "RSFEQUCODI";
        public string Equicodi = "EQUICODI";
        public string Rsfequagccent = "RSFEQUAGCCENT";
        public string Rsfequagcuni = "RSFEQUAGCUNI";
        public string Rsfequlastuser = "RSFEQULASTUSER";
        public string Rsfequlastdate = "RSFEQULASTDATE";
        public string Rsfequindicador = "RSFEQUINDICADOR";
        public string ModosOperacion = "MODOSOPERACION";
        public string IndCC = "INDCC";
        public string Equipadre = "EQUIPADRE";
        public string Famcodi = "FAMCODI";

        #endregion

        #region Cambio RSF

        public string Rsfequminimo = "RSFEQUMINIMO";
        public string Rsfequmaximo = "RSFEQUMAXIMO";
        public string Rsfequrecurcodi = "RSFEQURECURCODI";
        public string Rsfequasignacion = "RSFEQUASIGNACION";

        #endregion
    }
}
