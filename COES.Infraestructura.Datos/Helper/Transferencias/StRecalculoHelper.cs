using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_RECALCULO
    /// </summary>
    public class StRecalculoHelper : HelperBase
    {
        public StRecalculoHelper(): base(Consultas.StRecalculoSql)
        {
        }

        public StRecalculoDTO Create(IDataReader dr)
        {
            StRecalculoDTO entity = new StRecalculoDTO();

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iStpercodi = dr.GetOrdinal(this.Stpercodi);
            if (!dr.IsDBNull(iStpercodi)) entity.Stpercodi = Convert.ToInt32(dr.GetValue(iStpercodi));

            int iSstversion = dr.GetOrdinal(this.Sstversion);
            if (!dr.IsDBNull(iSstversion)) entity.Sstversion = Convert.ToInt32(dr.GetValue(iSstversion));

            int iStrecanombre = dr.GetOrdinal(this.Strecanombre);
            if (!dr.IsDBNull(iStrecanombre)) entity.Strecanombre = dr.GetString(iStrecanombre);

            int iStrecainforme = dr.GetOrdinal(this.Strecainforme);
            if (!dr.IsDBNull(iStrecainforme)) entity.Strecainforme = dr.GetString(iStrecainforme);

            int iStrecafacajuste = dr.GetOrdinal(this.Strecafacajuste);
            if (!dr.IsDBNull(iStrecafacajuste)) entity.Strecafacajuste = dr.GetDecimal(iStrecafacajuste);

            int iStrecacomentario = dr.GetOrdinal(this.Strecacomentario);
            if (!dr.IsDBNull(iStrecacomentario)) entity.Strecacomentario = dr.GetString(iStrecacomentario);

            int iStrecausucreacion = dr.GetOrdinal(this.Strecausucreacion);
            if (!dr.IsDBNull(iStrecausucreacion)) entity.Strecausucreacion = dr.GetString(iStrecausucreacion);

            int iStrecafeccreacion = dr.GetOrdinal(this.Strecafeccreacion);
            if (!dr.IsDBNull(iStrecafeccreacion)) entity.Strecafeccreacion = dr.GetDateTime(iStrecafeccreacion);

            int iStrecausumodificacion = dr.GetOrdinal(this.Strecausumodificacion);
            if (!dr.IsDBNull(iStrecausumodificacion)) entity.Strecausumodificacion = dr.GetString(iStrecausumodificacion);

            int iStrecafecmodificacion = dr.GetOrdinal(this.Strecafecmodificacion);
            if (!dr.IsDBNull(iStrecafecmodificacion)) entity.Strecafecmodificacion = dr.GetDateTime(iStrecafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Strecacodi = "STRECACODI";
        public string Stpercodi = "STPERCODI";
        public string Sstversion = "SSTVERSION";
        public string Strecanombre = "STRECANOMBRE";
        public string Strecainforme = "STRECAINFORME";
        public string Strecafacajuste = "STRECAFACAJUSTE";
        public string Strecacomentario = "STRECACOMENTARIO";
        public string Strecausucreacion = "STRECAUSUCREACION";
        public string Strecafeccreacion = "STRECAFECCREACION";
        public string Strecausumodificacion = "STRECAUSUMODIFICACION";
        public string Strecafecmodificacion = "STRECAFECMODIFICACION";
        //Variables para los reportes
        public string Stpernombre = "STPERNOMBRE";

        #endregion

        public string SqlListByStPericodi
        {
            get { return base.GetSqlXml("ListByStPericodi"); }
        }
        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }
    }
}
