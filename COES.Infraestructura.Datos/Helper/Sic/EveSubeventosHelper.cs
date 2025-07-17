using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_SUBEVENTOS
    /// </summary>
    public class EveSubeventosHelper : HelperBase
    {
        public EveSubeventosHelper(): base(Consultas.EveSubeventosSql)
        {
        }

        public EveSubeventosDTO Create(IDataReader dr)
        {
            EveSubeventosDTO entity = new EveSubeventosDTO();

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSubevedescrip = dr.GetOrdinal(this.Subevedescrip);
            if (!dr.IsDBNull(iSubevedescrip)) entity.Subevedescrip = dr.GetString(iSubevedescrip);

            int iSubevenfin = dr.GetOrdinal(this.Subevenfin);
            if (!dr.IsDBNull(iSubevenfin)) entity.Subevenfin = dr.GetDateTime(iSubevenfin);

            int iSubevenini = dr.GetOrdinal(this.Subevenini);
            if (!dr.IsDBNull(iSubevenini)) entity.Subevenini = dr.GetDateTime(iSubevenini);

            return entity;
        }


        #region Mapeo de Campos

        public string Evencodi = "EVENCODI";
        public string Equicodi = "EQUICODI";
        public string Subevedescrip = "SUBEVEDESCRIP";
        public string Subevenfin = "SUBEVENFIN";
        public string Subevenini = "SUBEVENINI";
        public string EquiAbrev = "EQUIABREV";
        public string EmprNomb = "EMPRNOMB";
        public string AreaNomb = "AREANOMB";

        #endregion
    }
}

