using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_BANDEJAMENSAJE_USER
    /// </summary>
    public class SiBandejamensajeUserHelper : HelperBase
    {
        public SiBandejamensajeUserHelper()
            : base(Consultas.SiBandejamensajeUserSql)
        {
        }

        public SiBandejamensajeUserDTO Create(IDataReader dr)
        {
            SiBandejamensajeUserDTO entity = new SiBandejamensajeUserDTO();

            int iBandcodi = dr.GetOrdinal(this.Bandcodi);
            if (!dr.IsDBNull(iBandcodi)) entity.Bandcodi = Convert.ToInt32(dr.GetValue(iBandcodi));

            int iBandnombre = dr.GetOrdinal(this.Bandnombre);
            if (!dr.IsDBNull(iBandnombre)) entity.Bandnombre = dr.GetString(iBandnombre);

            int iBandusucreacion = dr.GetOrdinal(this.Bandusucreacion);
            if (!dr.IsDBNull(iBandusucreacion)) entity.Bandusucreacion = dr.GetString(iBandusucreacion);

            int iBandfeccreacion = dr.GetOrdinal(this.Bandfeccreacion);
            if (!dr.IsDBNull(iBandfeccreacion)) entity.Bandfeccreacion = dr.GetDateTime(iBandfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Bandcodi = "BANDCODI";
        public string Modcodi = "MODCODI";
        public string Bandnombre = "BANDNOMBRE";
        public string Bandusucreacion = "BANDUSUCREACION";
        public string Bandfeccreacion = "BANDFECCREACION";

        public string Cantidad = "CANTIDAD";
        #endregion

        public string SqllistaCantEnCarpetaPorModYUser
        {
            get { return base.GetSqlXml("listaCantEnCarpetaPorModYUser"); }
        }

    }
}
