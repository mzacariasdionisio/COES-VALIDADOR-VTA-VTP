using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_EVENTO
    /// </summary>
    public class IndEventoHelper : HelperBase
    {
        public IndEventoHelper(): base(Consultas.IndEventoSql)
        {
        }

        public IndEventoDTO Create(IDataReader dr)
        {
            IndEventoDTO entity = new IndEventoDTO();

            int iIeventcodi = dr.GetOrdinal(this.Ieventcodi);
            if (!dr.IsDBNull(iIeventcodi)) entity.Ieventcodi = Convert.ToInt32(dr.GetValue(iIeventcodi));

            int iIeventtipoindisp = dr.GetOrdinal(this.Ieventtipoindisp);
            if (!dr.IsDBNull(iIeventtipoindisp)) entity.Ieventtipoindisp = dr.GetString(iIeventtipoindisp);

            int iIeventmw = dr.GetOrdinal(this.Ieventpr);
            if (!dr.IsDBNull(iIeventmw)) entity.Ieventpr = dr.GetDecimal(iIeventmw);

            int iIeventusucreacion = dr.GetOrdinal(this.Ieventusucreacion);
            if (!dr.IsDBNull(iIeventusucreacion)) entity.Ieventusucreacion = dr.GetString(iIeventusucreacion);

            int iIeventfeccreacion = dr.GetOrdinal(this.Ieventfeccreacion);
            if (!dr.IsDBNull(iIeventfeccreacion)) entity.Ieventfeccreacion = dr.GetDateTime(iIeventfeccreacion);

            int iIeventusumodificacion = dr.GetOrdinal(this.Ieventusumodificacion);
            if (!dr.IsDBNull(iIeventusumodificacion)) entity.Ieventusumodificacion = dr.GetString(iIeventusumodificacion);

            int iIeventfecmodificacion = dr.GetOrdinal(this.Ieventfecmodificacion);
            if (!dr.IsDBNull(iIeventfecmodificacion)) entity.Ieventfecmodificacion = dr.GetDateTime(iIeventfecmodificacion);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iIeventcomentario = dr.GetOrdinal(this.Ieventcomentario);
            if (!dr.IsDBNull(iIeventcomentario)) entity.Ieventcomentario = dr.GetString(iIeventcomentario);

            int iIeventestado = dr.GetOrdinal(this.Ieventestado);
            if (!dr.IsDBNull(iIeventestado)) entity.Ieventestado = dr.GetString(iIeventestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Ieventcodi = "IEVENTCODI";
        public string Ieventtipoindisp = "IEVENTTIPOINDISP";
        public string Ieventpr = "IEVENTPR";
        public string Ieventusucreacion = "IEVENTUSUCREACION";
        public string Ieventfeccreacion = "IEVENTFECCREACION";
        public string Ieventusumodificacion = "IEVENTUSUMODIFICACION";
        public string Ieventfecmodificacion = "IEVENTFECMODIFICACION";
        public string Evencodi = "EVENCODI";
        public string Ieventcomentario = "IEVENTCOMENTARIO";
        public string Ieventestado = "IEVENTESTADO";

        public string Evenini = "EVENINI";
        public string Evenfin = "EVENFIN";
        public string Evenasunto = "EVENASUNTO";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Equicodi = "Equicodi";
        public string Equipadre = "Equipadre";
        public string Emprcodi = "Emprcodi";
        public string Equiabrev = "Equiabrev";
        public string Grupocodi = "Grupocodi";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Areadesc = "AREADESC";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Equinomb = "EQUINOMB";
        public string Evenclaseabrev = "EVENCLASEABREV";
        public string Famabrev = "FAMABREV";
        public string Central = "CENTRAL";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";

        #endregion

        public string SqlListHistoricoByEvencodi
        {
            get { return base.GetSqlXml("ListHistoricoByEvencodi"); }
        }
    }
}
