using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_PROCESO
    /// </summary>
    public class NrProcesoHelper : HelperBase
    {
        public NrProcesoHelper(): base(Consultas.NrProcesoSql)
        {
        }

        public NrProcesoDTO Create(IDataReader dr)
        {
            NrProcesoDTO entity = new NrProcesoDTO();

            int iNrprccodi = dr.GetOrdinal(this.Nrprccodi);
            if (!dr.IsDBNull(iNrprccodi)) entity.Nrprccodi = Convert.ToInt32(dr.GetValue(iNrprccodi));

            int iNrpercodi = dr.GetOrdinal(this.Nrpercodi);
            if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iNrcptcodi = dr.GetOrdinal(this.Nrcptcodi);
            if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));

            int iNrprcfechainicio = dr.GetOrdinal(this.Nrprcfechainicio);
            if (!dr.IsDBNull(iNrprcfechainicio)) entity.Nrprcfechainicio = dr.GetDateTime(iNrprcfechainicio);

            int iNrprcfechafin = dr.GetOrdinal(this.Nrprcfechafin);
            if (!dr.IsDBNull(iNrprcfechafin)) entity.Nrprcfechafin = dr.GetDateTime(iNrprcfechafin);

            int iNrprchoraunidad = dr.GetOrdinal(this.Nrprchoraunidad);
            if (!dr.IsDBNull(iNrprchoraunidad)) entity.Nrprchoraunidad = dr.GetDecimal(iNrprchoraunidad);

            int iNrprchoracentral = dr.GetOrdinal(this.Nrprchoracentral);
            if (!dr.IsDBNull(iNrprchoracentral)) entity.Nrprchoracentral = dr.GetDecimal(iNrprchoracentral);

            int iNrprcpotencialimite = dr.GetOrdinal(this.Nrprcpotencialimite);
            if (!dr.IsDBNull(iNrprcpotencialimite)) entity.Nrprcpotencialimite = dr.GetDecimal(iNrprcpotencialimite);

            int iNrprcpotenciarestringida = dr.GetOrdinal(this.Nrprcpotenciarestringida);
            if (!dr.IsDBNull(iNrprcpotenciarestringida)) entity.Nrprcpotenciarestringida = dr.GetDecimal(iNrprcpotenciarestringida);

            int iNrprcpotenciaadjudicada = dr.GetOrdinal(this.Nrprcpotenciaadjudicada);
            if (!dr.IsDBNull(iNrprcpotenciaadjudicada)) entity.Nrprcpotenciaadjudicada = dr.GetDecimal(iNrprcpotenciaadjudicada);

            int iNrprcpotenciaefectiva = dr.GetOrdinal(this.Nrprcpotenciaefectiva);
            if (!dr.IsDBNull(iNrprcpotenciaefectiva)) entity.Nrprcpotenciaefectiva = dr.GetDecimal(iNrprcpotenciaefectiva);

            int iNrprcpotenciaprommedidor = dr.GetOrdinal(this.Nrprcpotenciaprommedidor);
            if (!dr.IsDBNull(iNrprcpotenciaprommedidor)) entity.Nrprcpotenciaprommedidor = dr.GetDecimal(iNrprcpotenciaprommedidor);

            int iNrprcprctjrestringefect = dr.GetOrdinal(this.Nrprcprctjrestringefect);
            if (!dr.IsDBNull(iNrprcprctjrestringefect)) entity.Nrprcprctjrestringefect = dr.GetDecimal(iNrprcprctjrestringefect);

            int iNrprcvolumencombustible = dr.GetOrdinal(this.Nrprcvolumencombustible);
            if (!dr.IsDBNull(iNrprcvolumencombustible)) entity.Nrprcvolumencombustible = dr.GetDecimal(iNrprcvolumencombustible);

            int iNrprcrendimientounidad = dr.GetOrdinal(this.Nrprcrendimientounidad);
            if (!dr.IsDBNull(iNrprcrendimientounidad)) entity.Nrprcrendimientounidad = dr.GetDecimal(iNrprcrendimientounidad);

            int iNrprcede = dr.GetOrdinal(this.Nrprcede);
            if (!dr.IsDBNull(iNrprcede)) entity.Nrprcede = dr.GetDecimal(iNrprcede);

            int iNrprcpadre = dr.GetOrdinal(this.Nrprcpadre);
            if (!dr.IsDBNull(iNrprcpadre)) entity.Nrprcpadre = Convert.ToInt32(dr.GetValue(iNrprcpadre));

            int iNrprcexceptuacoes = dr.GetOrdinal(this.Nrprcexceptuacoes);
            if (!dr.IsDBNull(iNrprcexceptuacoes)) entity.Nrprcexceptuacoes = dr.GetString(iNrprcexceptuacoes);

            int iNrprcexceptuaosinergmin = dr.GetOrdinal(this.Nrprcexceptuaosinergmin);
            if (!dr.IsDBNull(iNrprcexceptuaosinergmin)) entity.Nrprcexceptuaosinergmin = dr.GetString(iNrprcexceptuaosinergmin);

            int iNrprctipoingreso = dr.GetOrdinal(this.Nrprctipoingreso);
            if (!dr.IsDBNull(iNrprctipoingreso)) entity.Nrprctipoingreso = dr.GetString(iNrprctipoingreso);

            int iNrprchorafalla = dr.GetOrdinal(this.Nrprchorafalla);
            if (!dr.IsDBNull(iNrprchorafalla)) entity.Nrprchorafalla = dr.GetString(iNrprchorafalla);

            int iNrprcsobrecosto = dr.GetOrdinal(this.Nrprcsobrecosto);
            if (!dr.IsDBNull(iNrprcsobrecosto)) entity.Nrprcsobrecosto = dr.GetDecimal(iNrprcsobrecosto);

            int iNrprcobservacion = dr.GetOrdinal(this.Nrprcobservacion);
            if (!dr.IsDBNull(iNrprcobservacion)) entity.Nrprcobservacion = dr.GetString(iNrprcobservacion);

            int iNrprcnota = dr.GetOrdinal(this.Nrprcnota);
            if (!dr.IsDBNull(iNrprcnota)) entity.Nrprcnota = dr.GetString(iNrprcnota);

            int iNrprcnotaautomatica = dr.GetOrdinal(this.Nrprcnotaautomatica);
            if (!dr.IsDBNull(iNrprcnotaautomatica)) entity.Nrprcnotaautomatica = dr.GetString(iNrprcnotaautomatica);

            int iNrprcfiltrado = dr.GetOrdinal(this.Nrprcfiltrado);
            if (!dr.IsDBNull(iNrprcfiltrado)) entity.Nrprcfiltrado = dr.GetString(iNrprcfiltrado);

            int iNrprcrpf = dr.GetOrdinal(this.Nrprcrpf);
            if (!dr.IsDBNull(iNrprcrpf)) entity.Nrprcrpf = dr.GetDecimal(iNrprcrpf);

            int iNrprctolerancia = dr.GetOrdinal(this.Nrprctolerancia);
            if (!dr.IsDBNull(iNrprctolerancia)) entity.Nrprctolerancia = dr.GetDecimal(iNrprctolerancia);

            int iNrprcusucreacion = dr.GetOrdinal(this.Nrprcusucreacion);
            if (!dr.IsDBNull(iNrprcusucreacion)) entity.Nrprcusucreacion = dr.GetString(iNrprcusucreacion);

            int iNrprcfeccreacion = dr.GetOrdinal(this.Nrprcfeccreacion);
            if (!dr.IsDBNull(iNrprcfeccreacion)) entity.Nrprcfeccreacion = dr.GetDateTime(iNrprcfeccreacion);

            int iNrprcusumodificacion = dr.GetOrdinal(this.Nrprcusumodificacion);
            if (!dr.IsDBNull(iNrprcusumodificacion)) entity.Nrprcusumodificacion = dr.GetString(iNrprcusumodificacion);

            int iNrprcfecmodificacion = dr.GetOrdinal(this.Nrprcfecmodificacion);
            if (!dr.IsDBNull(iNrprcfecmodificacion)) entity.Nrprcfecmodificacion = dr.GetDateTime(iNrprcfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrprccodi = "NRPRCCODI";
        public string Nrpercodi = "NRPERCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Nrcptcodi = "NRCPTCODI";
        public string Nrprcfechainicio = "NRPRCFECHAINICIO";
        public string Nrprcfechafin = "NRPRCFECHAFIN";
        public string Nrprchoraunidad = "NRPRCHORAUNIDAD";
        public string Nrprchoracentral = "NRPRCHORACENTRAL";
        public string Nrprcpotencialimite = "NRPRCPOTENCIALIMITE";
        public string Nrprcpotenciarestringida = "NRPRCPOTENCIARESTRINGIDA";
        public string Nrprcpotenciaadjudicada = "NRPRCPOTENCIAADJUDICADA";
        public string Nrprcpotenciaefectiva = "NRPRCPOTENCIAEFECTIVA";
        public string Nrprcpotenciaprommedidor = "NRPRCPOTENCIAPROMMEDIDOR";
        public string Nrprcprctjrestringefect = "NRPRCPRCTJRESTRINGEFECT";
        public string Nrprcvolumencombustible = "NRPRCVOLUMENCOMBUSTIBLE";
        public string Nrprcrendimientounidad = "NRPRCRENDIMIENTOUNIDAD";
        public string Nrprcede = "NRPRCEDE";
        public string Nrprcpadre = "NRPRCPADRE";
        public string Nrprcexceptuacoes = "NRPRCEXCEPTUACOES";
        public string Nrprcexceptuaosinergmin = "NRPRCEXCEPTUAOSINERGMIN";
        public string Nrprctipoingreso = "NRPRCTIPOINGRESO";
        public string Nrprchorafalla = "NRPRCHORAFALLA";
        public string Nrprcsobrecosto = "NRPRCSOBRECOSTO";
        public string Nrprcobservacion = "NRPRCOBSERVACION";
        public string Nrprcnota = "NRPRCNOTA";
        public string Nrprcnotaautomatica = "NRPRCNOTAAUTOMATICA";
        public string Nrprcfiltrado = "NRPRCFILTRADO";
        public string Nrprcrpf = "NRPRCRPF";
        public string Nrprctolerancia = "NRPRCTOLERANCIA";
        public string Nrprcusucreacion = "NRPRCUSUCREACION";
        public string Nrprcfeccreacion = "NRPRCFECCREACION";
        public string Nrprcusumodificacion = "NRPRCUSUMODIFICACION";
        public string Nrprcfecmodificacion = "NRPRCFECMODIFICACION";
        public string Nrpermes = "NRPERMES";
        public string Gruponomb = "GRUPONOMB";
        public string Nrcptabrev = "NRCPTABREV";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlDeletePeriodoConcepto
        {
            get { return GetSqlXml("DeletePeriodoConcepto"); }
        }

        public string SqlListObservacion
        {
            get { return GetSqlXml("ListObservacion"); }
        }

        public string SqlReservaDiariaRSF
        {
            get { return base.GetSqlXml("ReservaDiariaRSF"); }
        }



        #endregion
    }
}
