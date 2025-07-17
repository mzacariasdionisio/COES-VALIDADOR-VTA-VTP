    using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EQUIPOMIGRAR
    /// </summary>
    public class SiEquipomigrarHelper : HelperBase
    {
        public SiEquipomigrarHelper(): base(Consultas.SiEquipomigrarSql)
        {
        }

        public SiEquipomigrarDTO Create(IDataReader dr)
        {
            SiEquipomigrarDTO entity = new SiEquipomigrarDTO();

            int iEqumigcodi = dr.GetOrdinal(this.Equmigcodi);
            if (!dr.IsDBNull(iEqumigcodi)) entity.Equmigcodi = Convert.ToInt32(dr.GetValue(iEqumigcodi));

            int iMigempcodi = dr.GetOrdinal(this.Migempcodi);
            if (!dr.IsDBNull(iMigempcodi)) entity.Migempcodi = Convert.ToInt32(dr.GetValue(iMigempcodi));

            int iEquicodimigra = dr.GetOrdinal(this.Equicodimigra);
            if (!dr.IsDBNull(iEquicodimigra)) entity.Equicodimigra = Convert.ToInt32(dr.GetValue(iEquicodimigra));

            int iEquicodibajanuevo = dr.GetOrdinal(this.Equicodibajanuevo);
            if (!dr.IsDBNull(iEquicodibajanuevo)) entity.Equicodibajanuevo = Convert.ToInt32(dr.GetValue(iEquicodibajanuevo));

            int iEqumigusucreacion = dr.GetOrdinal(this.Equmigusucreacion);
            if (!dr.IsDBNull(iEqumigusucreacion)) entity.Equmigusucreacion = dr.GetString(iEqumigusucreacion);

            int iEqumigfeccreacion = dr.GetOrdinal(this.Equmigfeccreacion);
            if (!dr.IsDBNull(iEqumigfeccreacion)) entity.Equmigfeccreacion = dr.GetDateTime(iEqumigfeccreacion);

            int iEqumigusumodificacion = dr.GetOrdinal(this.Equmigusumodificacion);
            if (!dr.IsDBNull(iEqumigusumodificacion)) entity.Equmigusumodificacion = dr.GetString(iEqumigusumodificacion);

            int iEqumigfecmodificacion = dr.GetOrdinal(this.Equmigfecmodificacion);
            if (!dr.IsDBNull(iEqumigfecmodificacion)) entity.Equmigfecmodificacion = dr.GetDateTime(iEqumigfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Equmigcodi = "EQUMIGCODI";
        public string Migempcodi = "MIGEMPCODI";
        public string Equicodimigra = "EQUICODIMIGRA";
        public string Equicodibajanuevo = "EQUICODIBAJANUEVO";
        public string Equmigusucreacion = "EQUMIGUSUCREACION";
        public string Equmigfeccreacion = "EQUMIGFECCREACION";
        public string Equmigusumodificacion = "EQUMIGUSUMODIFICACION";
        public string Equmigfecmodificacion = "EQUMIGFECMODIFICACION";

        #endregion
    }
}
