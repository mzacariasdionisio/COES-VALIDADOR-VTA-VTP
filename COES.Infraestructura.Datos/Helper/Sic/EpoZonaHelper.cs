using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EpoZonaHelper:HelperBase
    {

        public EpoZonaHelper() : base(Consultas.EpoZonaSql)
        {
        }
        public EpoZonaDTO Create(IDataReader dr)
        {
            EpoZonaDTO entity = new EpoZonaDTO();

            int iIndcodi = dr.GetOrdinal(this.ZonCod);
            if (!dr.IsDBNull(iIndcodi)) entity.Zoncodi = Convert.ToInt32(dr.GetValue(iIndcodi));

            int iIndDescrip = dr.GetOrdinal(this.ZonaDescripcion);
            if (!dr.IsDBNull(iIndDescrip)) entity.ZonDescripcion = dr.GetString(iIndDescrip);
            return entity;
        }

        #region Mapeo de Campos
        public string ZonCod = "ZONCODI";
        public string ZonaDescripcion = "ZONDESCRIPCION";
        public string ZonActivo = "ZONACTIVO";
        public string LastDate = "LASTDATE";
        public string LastUser = "LASTUSER";
        public string PuntCodi = "PUNTCODI";
        #endregion
    }
}
