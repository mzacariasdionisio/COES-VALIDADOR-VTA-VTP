using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_HO_UNIDAD
    /// </summary>
    public class EveHoUnidadHelper : HelperBase
    {
        public EveHoUnidadHelper() : base(Consultas.EveHoUnidadSql)
        {
        }

        public EveHoUnidadDTO Create(IDataReader dr)
        {
            EveHoUnidadDTO entity = new EveHoUnidadDTO();

            int iHopunicodi = dr.GetOrdinal(this.Hopunicodi);
            if (!dr.IsDBNull(iHopunicodi)) entity.Hopunicodi = Convert.ToInt32(dr.GetValue(iHopunicodi));

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iHopunihorordarranq = dr.GetOrdinal(this.Hopunihorordarranq);
            if (!dr.IsDBNull(iHopunihorordarranq)) entity.Hopunihorordarranq = dr.GetDateTime(iHopunihorordarranq);

            int iHopunihorini = dr.GetOrdinal(this.Hopunihorini);
            if (!dr.IsDBNull(iHopunihorini)) entity.Hopunihorini = dr.GetDateTime(iHopunihorini);

            int iHopunihorfin = dr.GetOrdinal(this.Hopunihorfin);
            if (!dr.IsDBNull(iHopunihorfin)) entity.Hopunihorfin = dr.GetDateTime(iHopunihorfin);

            int iHopunihorarranq = dr.GetOrdinal(this.Hopunihorarranq);
            if (!dr.IsDBNull(iHopunihorarranq)) entity.Hopunihorarranq = dr.GetDateTime(iHopunihorarranq);

            int iHopunihorparada = dr.GetOrdinal(this.Hopunihorparada);
            if (!dr.IsDBNull(iHopunihorparada)) entity.Hopunihorparada = dr.GetDateTime(iHopunihorparada);

            int iHopuniusucreacion = dr.GetOrdinal(this.Hopuniusucreacion);
            if (!dr.IsDBNull(iHopuniusucreacion)) entity.Hopuniusucreacion = dr.GetString(iHopuniusucreacion);

            int iHopunifeccreacion = dr.GetOrdinal(this.Hopunifeccreacion);
            if (!dr.IsDBNull(iHopunifeccreacion)) entity.Hopunifeccreacion = dr.GetDateTime(iHopunifeccreacion);

            int iHopuniusumodificacion = dr.GetOrdinal(this.Hopuniusumodificacion);
            if (!dr.IsDBNull(iHopuniusumodificacion)) entity.Hopuniusumodificacion = dr.GetString(iHopuniusumodificacion);

            int iHopunifecmodificacion = dr.GetOrdinal(this.Hopunifecmodificacion);
            if (!dr.IsDBNull(iHopunifecmodificacion)) entity.Hopunifecmodificacion = dr.GetDateTime(iHopunifecmodificacion);

            int iHopuniactivo = dr.GetOrdinal(this.Hopuniactivo);
            if (!dr.IsDBNull(iHopuniactivo)) entity.Hopuniactivo = Convert.ToInt32(dr.GetValue(iHopuniactivo));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Hopunicodi = "HOPUNICODI";
        public string Hopcodi = "HOPCODI";
        public string Equicodi = "EQUICODI";
        public string Hopunihorordarranq = "HOPUNIHORORDARRANQ";
        public string Hopunihorini = "HOPUNIHORINI";
        public string Hopunihorfin = "HOPUNIHORFIN";
        public string Hopunihorarranq = "HOPUNIHORARRANQ";
        public string Hopunihorparada = "HOPUNIHORPARADA";
        public string Hopuniusucreacion = "HOPUNIUSUCREACION";
        public string Hopunifeccreacion = "HOPUNIFECCREACION";
        public string Hopuniusumodificacion = "HOPUNIUSUMODIFICACION";
        public string Hopunifecmodificacion = "HOPUNIFECMODIFICACION";
        public string Hopuniactivo = "HOPUNIACTIVO";
        public string Emprcodi = "EMPRCODI";

        #endregion
    }
}
