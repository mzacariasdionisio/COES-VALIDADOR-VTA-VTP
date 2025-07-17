using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_CUADRO3
    /// </summary>
    public class IndCuadro3Helper : HelperBase
    {
        public IndCuadro3Helper()
            : base(Consultas.IndCuadro3Sql)
        {
        }

        public IndCuadro3DTO Create(IDataReader dr)
        {
            IndCuadro3DTO entity = new IndCuadro3DTO();

            int iCuadr3codi = dr.GetOrdinal(this.Cuadr3codi);
            if (!dr.IsDBNull(iCuadr3codi)) entity.Cuadr3codi = Convert.ToInt32(dr.GetValue(iCuadr3codi));

            int iCuadr3potlimite = dr.GetOrdinal(this.Cuadr3potlimite);
            if (!dr.IsDBNull(iCuadr3potlimite)) entity.Cuadr3potlimite = Convert.ToDecimal(dr.GetValue(iCuadr3potlimite));

            int iCuadr3despotlimite = dr.GetOrdinal(this.Cuadr3despotlimite);
            if (!dr.IsDBNull(iCuadr3despotlimite)) entity.Cuadr3despotlimite = dr.GetString(iCuadr3despotlimite);

            int iCuadr3usumodificacion = dr.GetOrdinal(this.Cuadr3usumodificacion);
            if (!dr.IsDBNull(iCuadr3usumodificacion)) entity.Cuadr3usumodificacion = dr.GetString(iCuadr3usumodificacion);

            int iCuadr3fecmodificacion = dr.GetOrdinal(this.Cuadr3fecmodificacion);
            if (!dr.IsDBNull(iCuadr3fecmodificacion)) entity.Cuadr3fecmodificacion = dr.GetDateTime(iCuadr3fecmodificacion);

            int iCuadr3CombusElectrico = dr.GetOrdinal(this.Cuadr3Electrico);
            if (!dr.IsDBNull(iCuadr3CombusElectrico)) entity.Cuadr3Electrico = dr.GetString(iCuadr3CombusElectrico);

            return entity;
        }


        #region Mapeo de Campos

        public string Cuadr3codi = "CUADR3CODI";
        public string Cuadr3potlimite = "CUADR3POTLIMITE";
        public string Cuadr3despotlimite = "CUADR3DESPOTLIMITE";
        public string Cuadr3usumodificacion = "CUADR3USUMODIFICACION";
        public string Cuadr3fecmodificacion = "CUADR3FECMODIFICACION";
        public string Cuadr3Electrico = "CUADR3ELECTRICO";

        #endregion
    }
}
