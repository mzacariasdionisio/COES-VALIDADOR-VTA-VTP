using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_ARCHIVOENVIO
    /// </summary>
    public class CbArchivoenvioHelper : HelperBase
    {
        public CbArchivoenvioHelper() : base(Consultas.CbArchivoEnvioSql)
        {
        }

        public CbArchivoenvioDTO Create(IDataReader dr)
        {
            CbArchivoenvioDTO entity = new CbArchivoenvioDTO();

            int iCbvercodi = dr.GetOrdinal(this.Cbvercodi);
            if (!dr.IsDBNull(iCbvercodi)) entity.Cbvercodi = Convert.ToInt32(dr.GetValue(iCbvercodi));

            int iCbarchcodi = dr.GetOrdinal(this.Cbarchcodi);
            if (!dr.IsDBNull(iCbarchcodi)) entity.Cbarchcodi = Convert.ToInt32(dr.GetValue(iCbarchcodi));

            int iCbarchnombreenvio = dr.GetOrdinal(this.Cbarchnombreenvio);
            if (!dr.IsDBNull(iCbarchnombreenvio)) entity.Cbarchnombreenvio = dr.GetString(iCbarchnombreenvio);

            int iCbarchnombrefisico = dr.GetOrdinal(this.Cbarchnombrefisico);
            if (!dr.IsDBNull(iCbarchnombrefisico)) entity.Cbarchnombrefisico = dr.GetString(iCbarchnombrefisico);

            int iCbarchorden = dr.GetOrdinal(this.Cbarchorden);
            if (!dr.IsDBNull(iCbarchorden)) entity.Cbarchorden = Convert.ToInt32(dr.GetValue(iCbarchorden));

            int iCbarchestado = dr.GetOrdinal(this.Cbarchestado);
            if (!dr.IsDBNull(iCbarchestado)) entity.Cbarchestado = Convert.ToInt32(dr.GetValue(iCbarchestado));

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCbarchconfidencial = dr.GetOrdinal(this.Cbarchconfidencial);
            if (!dr.IsDBNull(iCbarchconfidencial)) entity.Cbarchconfidencial = Convert.ToInt32(dr.GetValue(iCbarchconfidencial));

            int iCbarchobs = dr.GetOrdinal(this.Cbarchobs);
            if (!dr.IsDBNull(iCbarchobs)) entity.Cbarchobs = dr.GetString(iCbarchobs);

            int iCorrcodi = dr.GetOrdinal(this.Corrcodi);
            if (!dr.IsDBNull(iCorrcodi)) entity.Corrcodi = Convert.ToInt32(dr.GetValue(iCorrcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbvercodi = "CBVERCODI";
        public string Cbarchcodi = "CBARCHCODI";
        public string Cbarchnombreenvio = "CBARCHNOMBREENVIO";
        public string Cbarchnombrefisico = "CBARCHNOMBREFISICO";
        public string Cbarchorden = "CBARCHORDEN";
        public string Cbarchestado = "CBARCHESTADO";
        public string Ccombcodi = "CCOMBCODI";
        public string Cbarchconfidencial = "CBARCHCONFIDENCIAL";
        public string Cbarchobs = "CBARCHOBS";
        public string Corrcodi = "CORRCODI";

        #endregion

        public string SqlGetByCorreo
        {
            get { return base.GetSqlXml("GetByCorreo"); }
        }
    }
}
