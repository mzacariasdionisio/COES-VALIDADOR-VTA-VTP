using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_CONFIGURACION_SENIAL
    /// </summary>
    public class CoConfiguracionSenialHelper : HelperBase
    {
        public CoConfiguracionSenialHelper(): base(Consultas.CoConfiguracionSenialSql)
        {
        }

        public CoConfiguracionSenialDTO Create(IDataReader dr)
        {
            CoConfiguracionSenialDTO entity = new CoConfiguracionSenialDTO();

            int iConsencodi = dr.GetOrdinal(this.Consencodi);
            if (!dr.IsDBNull(iConsencodi)) entity.Consencodi = Convert.ToInt32(dr.GetValue(iConsencodi));

            int iCourdecodi = dr.GetOrdinal(this.Courdecodi);
            if (!dr.IsDBNull(iCourdecodi)) entity.Courdecodi = Convert.ToInt32(dr.GetValue(iCourdecodi));

            int iCotidacodi = dr.GetOrdinal(this.Cotidacodi);
            if (!dr.IsDBNull(iCotidacodi)) entity.Cotidacodi = Convert.ToInt32(dr.GetValue(iCotidacodi));

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iConsenvalinicial = dr.GetOrdinal(this.Consenvalinicial);
            if (!dr.IsDBNull(iConsenvalinicial)) entity.Consenvalinicial = dr.GetDecimal(iConsenvalinicial);

            int iConsenusucreacion = dr.GetOrdinal(this.Consenusucreacion);
            if (!dr.IsDBNull(iConsenusucreacion)) entity.Consenusucreacion = dr.GetString(iConsenusucreacion);

            int iConsenfeccreacion = dr.GetOrdinal(this.Consenfeccreacion);
            if (!dr.IsDBNull(iConsenfeccreacion)) entity.Consenfeccreacion = dr.GetDateTime(iConsenfeccreacion);

            int iConsenusumodificacion = dr.GetOrdinal(this.Consenusumodificacion);
            if (!dr.IsDBNull(iConsenusumodificacion)) entity.Consenusumodificacion = dr.GetString(iConsenusumodificacion);

            int iConsenfecmodificacion = dr.GetOrdinal(this.Consenfecmodificacion);
            if (!dr.IsDBNull(iConsenfecmodificacion)) entity.Consenfecmodificacion = dr.GetDateTime(iConsenfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Consencodi = "CONSENCODI";
        public string Courdecodi = "COURDECODI";
        public string Cotidacodi = "COTIDACODI";
        public string Zonacodi = "ZONACODI";
        public string Canalcodi = "CANALCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Consenvalinicial = "CONSENVALINICIAL";
        public string Consenusucreacion = "CONSENUSUCREACION";
        public string Consenfeccreacion = "CONSENFECCREACION";
        public string Consenusumodificacion = "CONSENUSUMODIFICACION";
        public string Consenfecmodificacion = "CONSENFECMODIFICACION";

        public string Coperanio = "COPERANIO";
        public string Copermes = "COPERMES";
        public string Covercodi = "COVERCODI";
        public string Canalnomb = "CANALNOMB";

        #endregion

        public string SqlListarSeniales
        {
            get { return base.GetSqlXml("ListarSeniales"); }
        }

        public string SqlListarSenialesPeriodosAnteriores
        {
            get { return base.GetSqlXml("ListarSenialesPeriodosAnteriores"); }
        }

        public string SqlObtenerCanalesPorURS
        {
            get { return base.GetSqlXml("ObtenerCanalesPorURS"); }
        }
        
    }
}
