using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_UNIDAD
    /// </summary>
    public class IndUnidadHelper : HelperBase
    {
        public IndUnidadHelper() : base(Consultas.IndUnidadSql)
        {
        }

        public IndUnidadDTO Create(IDataReader dr)
        {
            IndUnidadDTO entity = new IndUnidadDTO();

            int iIunicodi = dr.GetOrdinal(this.Iunicodi);
            if (!dr.IsDBNull(iIunicodi)) entity.Iunicodi = Convert.ToInt32(dr.GetValue(iIunicodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iIuniunidadnomb = dr.GetOrdinal(this.Iuniunidadnomb);
            if (!dr.IsDBNull(iIuniunidadnomb)) entity.Iuniunidadnomb = dr.GetString(iIuniunidadnomb);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iIuninombcentral = dr.GetOrdinal(this.Iuninombcentral);
            if (!dr.IsDBNull(iIuninombcentral)) entity.Iuninombcentral = dr.GetString(iIuninombcentral);

            int iIuninombunidad = dr.GetOrdinal(this.Iuninombunidad);
            if (!dr.IsDBNull(iIuninombunidad)) entity.Iuninombunidad = dr.GetString(iIuninombunidad);

            int iIuniactivo = dr.GetOrdinal(this.Iuniactivo);
            if (!dr.IsDBNull(iIuniactivo)) entity.Iuniactivo = Convert.ToInt32(dr.GetValue(iIuniactivo));

            int iIuniusucreacion = dr.GetOrdinal(this.Iuniusucreacion);
            if (!dr.IsDBNull(iIuniusucreacion)) entity.Iuniusucreacion = dr.GetString(iIuniusucreacion);

            int iIunifeccreacion = dr.GetOrdinal(this.Iunifeccreacion);
            if (!dr.IsDBNull(iIunifeccreacion)) entity.Iunifeccreacion = dr.GetDateTime(iIunifeccreacion);

            int iIuniusumodificacion = dr.GetOrdinal(this.Iuniusumodificacion);
            if (!dr.IsDBNull(iIuniusumodificacion)) entity.Iuniusumodificacion = dr.GetString(iIuniusumodificacion);

            int iIunifecmodificacion = dr.GetOrdinal(this.Iunifecmodificacion);
            if (!dr.IsDBNull(iIunifecmodificacion)) entity.Iunifecmodificacion = dr.GetDateTime(iIunifecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Iunicodi = "IUNICODI";
        public string Equipadre = "EQUIPADRE";
        public string Iuniunidadnomb = "Iuniunidadnomb";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Iuninombcentral = "IUNINOMBCENTRAL";
        public string Iuninombunidad = "IUNINOMBUNIDAD";
        public string Iuniactivo = "IUNIACTIVO";
        public string Iuniusucreacion = "IUNIUSUCREACION";
        public string Iunifeccreacion = "IUNIFECCREACION";
        public string Iuniusumodificacion = "IUNIUSUMODIFICACION";
        public string Iunifecmodificacion = "IUNIFECMODIFICACION";

        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "Gruponomb";

        #endregion
    }
}
