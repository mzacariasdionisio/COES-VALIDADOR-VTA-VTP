using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_BARRA_AREA
    /// </summary>
    public class TrnBarraAreaHelper : HelperBase
    {
        public TrnBarraAreaHelper(): base(Consultas.TrnBarraAreaSql)
        {
        }

        public TrnBarraAreaDTO Create(IDataReader dr)
        {
            TrnBarraAreaDTO entity = new TrnBarraAreaDTO();

            int iBararecodi = dr.GetOrdinal(this.Bararecodi);
            if (!dr.IsDBNull(iBararecodi)) entity.Bararecodi = Convert.ToInt32(dr.GetValue(iBararecodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iBararearea = dr.GetOrdinal(this.Bararearea);
            if (!dr.IsDBNull(iBararearea)) entity.Bararearea = dr.GetString(iBararearea);

            int iBarareejecutiva = dr.GetOrdinal(this.Barareejecutiva);
            if (!dr.IsDBNull(iBarareejecutiva)) entity.Barareejecutiva = dr.GetString(iBarareejecutiva);

            int iBarareestado = dr.GetOrdinal(this.Barareestado);
            if (!dr.IsDBNull(iBarareestado)) entity.Barareestado = dr.GetString(iBarareestado);

            int iBarareusucreacion = dr.GetOrdinal(this.Barareusucreacion);
            if (!dr.IsDBNull(iBarareusucreacion)) entity.Barareusucreacion = dr.GetString(iBarareusucreacion);

            int iBararefeccreacion = dr.GetOrdinal(this.Bararefeccreacion);
            if (!dr.IsDBNull(iBararefeccreacion)) entity.Bararefeccreacion = dr.GetDateTime(iBararefeccreacion);

            int iBarareusumodificacion = dr.GetOrdinal(this.Barareusumodificacion);
            if (!dr.IsDBNull(iBarareusumodificacion)) entity.Barareusumodificacion = dr.GetString(iBarareusumodificacion);

            int iBararefecmodificacion = dr.GetOrdinal(this.Bararefecmodificacion);
            if (!dr.IsDBNull(iBararefecmodificacion)) entity.Bararefecmodificacion = dr.GetDateTime(iBararefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Bararecodi = "BARARECODI";
        public string Barrcodi = "BARRCODI";
        public string Bararearea = "BARAREAREA";
        public string Barareejecutiva = "BARAREEJECUTIVA";
        public string Barareestado = "BARAREESTADO";
        public string Barareusucreacion = "BARAREUSUCREACION";
        public string Bararefeccreacion = "BARAREFECCREACION";
        public string Barareusumodificacion = "BARAREUSUMODIFICACION";
        public string Bararefecmodificacion = "BARAREFECMODIFICACION";

        #endregion
    }
}
