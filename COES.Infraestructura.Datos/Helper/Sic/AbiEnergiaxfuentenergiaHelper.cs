using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ABI_ENERGIAXFUENTENERGIA
    /// </summary>
    public class AbiEnergiaxfuentenergiaHelper : HelperBase
    {
        public AbiEnergiaxfuentenergiaHelper() : base(Consultas.AbiEnergiaxfuentenergiaSql)
        {
        }

        public AbiEnergiaxfuentenergiaDTO Create(IDataReader dr)
        {
            AbiEnergiaxfuentenergiaDTO entity = new AbiEnergiaxfuentenergiaDTO();

            int iMdfecodi = dr.GetOrdinal(this.Mdfecodi);
            if (!dr.IsDBNull(iMdfecodi)) entity.Mdfecodi = Convert.ToInt32(dr.GetValue(iMdfecodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iMdfefecha = dr.GetOrdinal(this.Mdfefecha);
            if (!dr.IsDBNull(iMdfefecha)) entity.Mdfefecha = dr.GetDateTime(iMdfefecha);

            int iMdfevalor = dr.GetOrdinal(this.Mdfevalor);
            if (!dr.IsDBNull(iMdfevalor)) entity.Mdfevalor = dr.GetDecimal(iMdfevalor);

            int iMdfemes = dr.GetOrdinal(this.Mdfemes);
            if (!dr.IsDBNull(iMdfemes)) entity.Mdfemes = dr.GetDateTime(iMdfemes);

            int iMdfeusumodificacion = dr.GetOrdinal(this.Mdfeusumodificacion);
            if (!dr.IsDBNull(iMdfeusumodificacion)) entity.Mdfeusumodificacion = dr.GetString(iMdfeusumodificacion);

            int iMdfefecmodificacion = dr.GetOrdinal(this.Mdfefecmodificacion);
            if (!dr.IsDBNull(iMdfefecmodificacion)) entity.Mdfefecmodificacion = dr.GetDateTime(iMdfefecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Mdfecodi = "MDFECODI";
        public string Fenergcodi = "FENERGCODI";
        public string Mdfefecha = "MDFEFECHA";
        public string Mdfevalor = "MDFEVALOR";
        public string Mdfemes = "MDFEMES";
        public string Mdfeusumodificacion = "MDFEUSUMODIFICACION";
        public string Mdfefecmodificacion = "MDFEFECMODIFICACION";

        #endregion

        public string SqlDeleteByMes
        {
            get { return base.GetSqlXml("DeleteByMes"); }
        }
    }
}
