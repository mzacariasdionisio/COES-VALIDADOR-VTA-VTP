using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EpoPuntoConexionHelper : HelperBase
    {
        public EpoPuntoConexionHelper() : base(Consultas.EpoPuntoConexionSql)
        {
        }
        public EpoPuntoConexionDTO Create(IDataReader dr)
        {
            EpoPuntoConexionDTO entity = new EpoPuntoConexionDTO();

            int iIndcodi = dr.GetOrdinal(this.PuntCodi);
            if (!dr.IsDBNull(iIndcodi)) entity.PuntCodi = Convert.ToInt32(dr.GetValue(iIndcodi));

            int iIndDescrip = dr.GetOrdinal(this.PuntDescripcion);
            if (!dr.IsDBNull(iIndDescrip)) entity.PuntDescripcion = dr.GetString(iIndDescrip);

            int iZonCodi = dr.GetOrdinal(this.ZonCodi);
            if (!dr.IsDBNull(iZonCodi)) entity.ZonCodi = Convert.ToInt32(dr.GetValue(iZonCodi));

            return entity;
        }

        #region Mapeo de Campos
        public string PuntCodi = "PUNTCODI";
        public string PuntDescripcion = "PUNTDESCRIPCION";
        public string PuntActivo = "PUNTACTIVO";
        public string LastDate = "LASTDATE";
        public string LastUser = "LASTUSER";
        public string ZonCodi = "ZONCODI";
        public string ZonDescripcion = "ZONDESCRIPCION";

        #endregion
    }
}
