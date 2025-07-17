using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOINFORMACION
    /// </summary>
    public class SiTipoinformacionHelper : HelperBase
    {
        public SiTipoinformacionHelper(): base(Consultas.SiTipoinformacionSql)
        {
        }

        public SiTipoinformacionDTO Create(IDataReader dr)
        {
            SiTipoinformacionDTO entity = new SiTipoinformacionDTO();

            int iTipoinfoabrev = dr.GetOrdinal(this.Tipoinfoabrev);
            if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

            int iTipoinfodesc = dr.GetOrdinal(this.Tipoinfodesc);
            if (!dr.IsDBNull(iTipoinfodesc)) entity.Tipoinfodesc = dr.GetString(iTipoinfodesc);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iTinfcanalunidad = dr.GetOrdinal(this.Tinfcanalunidad);
            if (!dr.IsDBNull(iTinfcanalunidad)) entity.Tinfcanalunidad = dr.GetString(iTinfcanalunidad);
            entity.Canalunidad = entity.Tinfcanalunidad;

            return entity;
        }


        #region Mapeo de Campos

        public string Tipoinfoabrev = "TIPOINFOABREV";
        public string Tipoinfodesc = "TIPOINFODESC";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Tinfcanalunidad = "TINFCANALUNIDAD";

        #endregion

    }
}
