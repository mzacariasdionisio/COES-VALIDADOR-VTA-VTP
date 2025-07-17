using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_OBSXARCHIVO
    /// </summary>
    public class CbObsxarchivoHelper : HelperBase
    {
        public CbObsxarchivoHelper() : base(Consultas.CbObsxarchivoSql)
        {
        }

        public CbObsxarchivoDTO Create(IDataReader dr)
        {
            CbObsxarchivoDTO entity = new CbObsxarchivoDTO();

            int iCbobscodi = dr.GetOrdinal(this.Cbobscodi);
            if (!dr.IsDBNull(iCbobscodi)) entity.Cbobscodi = Convert.ToInt32(dr.GetValue(iCbobscodi));

            int iCbobsacodi = dr.GetOrdinal(this.Cbobsacodi);
            if (!dr.IsDBNull(iCbobsacodi)) entity.Cbobsacodi = Convert.ToInt32(dr.GetValue(iCbobsacodi));

            int iCbobsanombreenvio = dr.GetOrdinal(this.Cbobsanombreenvio);
            if (!dr.IsDBNull(iCbobsanombreenvio)) entity.Cbobsanombreenvio = dr.GetString(iCbobsanombreenvio);

            int iCbobsanombrefisico = dr.GetOrdinal(this.Cbobsanombrefisico);
            if (!dr.IsDBNull(iCbobsanombrefisico)) entity.Cbobsanombrefisico = dr.GetString(iCbobsanombrefisico);

            int iCbobsaorden = dr.GetOrdinal(this.Cbobsaorden);
            if (!dr.IsDBNull(iCbobsaorden)) entity.Cbobsaorden = Convert.ToInt32(dr.GetValue(iCbobsaorden));

            int iCbobsaestado = dr.GetOrdinal(this.Cbobsaestado);
            if (!dr.IsDBNull(iCbobsaestado)) entity.Cbobsaestado = Convert.ToInt32(dr.GetValue(iCbobsaestado));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbobscodi = "CBOBSCODI";
        public string Cbobsacodi = "CBOBSACODI";
        public string Cbobsanombreenvio = "CBOBSANOMBREENVIO";
        public string Cbobsanombrefisico = "CBOBSANOMBREFISICO";
        public string Cbobsaorden = "CBOBSAORDEN";
        public string Cbobsaestado = "CBOBSAESTADO";

        public string Equicodi = "EQUICODI";
        public string Ccombcodi = "CCOMBCODI";

        #endregion

        public string SqlListByCbvercodi
        {
            get { return base.GetSqlXml("ListByCbvercodi"); }
        }
    }
}
