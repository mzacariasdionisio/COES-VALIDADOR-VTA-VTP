using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_MIGRACION
    /// </summary>
    public class TrnMigracionHelper : HelperBase
    {
        public TrnMigracionHelper(): base(Consultas.TrnMigracionSql)
        {
        }

        public TrnMigracionDTO Create(IDataReader dr)
        {
            TrnMigracionDTO entity = new TrnMigracionDTO();

            int iTrnmigcodi = dr.GetOrdinal(this.Trnmigcodi);
            if (!dr.IsDBNull(iTrnmigcodi)) entity.Trnmigcodi = Convert.ToInt32(dr.GetValue(iTrnmigcodi));

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iEmprcodiorigen = dr.GetOrdinal(this.Emprcodiorigen);
            if (!dr.IsDBNull(iEmprcodiorigen)) entity.Emprcodiorigen = Convert.ToInt32(dr.GetValue(iEmprcodiorigen));

            int iEmprcodidestino = dr.GetOrdinal(this.Emprcodidestino);
            if (!dr.IsDBNull(iEmprcodidestino)) entity.Emprcodidestino = Convert.ToInt32(dr.GetValue(iEmprcodidestino));

            int iTrnmigdescripcion = dr.GetOrdinal(this.Trnmigdescripcion);
            if (!dr.IsDBNull(iTrnmigdescripcion)) entity.Trnmigdescripcion = dr.GetString(iTrnmigdescripcion);

            int iTrnmigsql = dr.GetOrdinal(this.Trnmigsql);
            if (!dr.IsDBNull(iTrnmigsql)) entity.Trnmigsql = dr.GetString(iTrnmigsql);

            int iTrnmigestado = dr.GetOrdinal(this.Trnmigestado);
            if (!dr.IsDBNull(iTrnmigestado)) entity.Trnmigestado = dr.GetString(iTrnmigestado);

            int iTrnmigusucreacion = dr.GetOrdinal(this.Trnmigusucreacion);
            if (!dr.IsDBNull(iTrnmigusucreacion)) entity.Trnmigusucreacion = dr.GetString(iTrnmigusucreacion);

            int iTrnmigfeccreacion = dr.GetOrdinal(this.Trnmigfeccreacion);
            if (!dr.IsDBNull(iTrnmigfeccreacion)) entity.Trnmigfeccreacion = dr.GetDateTime(iTrnmigfeccreacion);

            int iTrnmigusumodificacion = dr.GetOrdinal(this.Trnmigusumodificacion);
            if (!dr.IsDBNull(iTrnmigusumodificacion)) entity.Trnmigusumodificacion = dr.GetString(iTrnmigusumodificacion);

            int iTrnmigfecmodificacion = dr.GetOrdinal(this.Trnmigfecmodificacion);
            if (!dr.IsDBNull(iTrnmigfecmodificacion)) entity.Trnmigfecmodificacion = dr.GetDateTime(iTrnmigfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Trnmigcodi = "TRNMIGCODI";
        public string Migracodi = "MIGRACODI";
        public string Emprcodiorigen = "EMPRCODIORIGEN";
        public string Emprcodidestino = "EMPRCODIDESTINO";
        public string Trnmigdescripcion = "TRNMIGDESCRIPCION";
        public string Trnmigsql = "TRNMIGSQL";
        public string Trnmigestado = "TRNMIGESTADO";
        public string Trnmigusucreacion = "TRNMIGUSUCREACION";
        public string Trnmigfeccreacion = "TRNMIGFECCREACION";
        public string Trnmigusumodificacion = "TRNMIGUSUMODIFICACION";
        public string Trnmigfecmodificacion = "TRNMIGFECMODIFICACION";

        #endregion
    }
}
