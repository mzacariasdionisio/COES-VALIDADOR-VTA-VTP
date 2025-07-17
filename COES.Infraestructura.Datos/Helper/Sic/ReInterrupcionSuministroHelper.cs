using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INTERRUPCION_SUMINISTRO
    /// </summary>
    public class ReInterrupcionSuministroHelper : HelperBase
    {
        public ReInterrupcionSuministroHelper(): base(Consultas.ReInterrupcionSuministroSql)
        {
        }

        public ReInterrupcionSuministroDTO Create(IDataReader dr)
        {
            ReInterrupcionSuministroDTO entity = new ReInterrupcionSuministroDTO();

            int iReintcodi = dr.GetOrdinal(this.Reintcodi);
            if (!dr.IsDBNull(iReintcodi)) entity.Reintcodi = Convert.ToInt32(dr.GetValue(iReintcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReintpadre = dr.GetOrdinal(this.Reintpadre);
            if (!dr.IsDBNull(iReintpadre)) entity.Reintpadre = Convert.ToInt32(dr.GetValue(iReintpadre));

            int iReintfinal = dr.GetOrdinal(this.Reintfinal);
            if (!dr.IsDBNull(iReintfinal)) entity.Reintfinal = dr.GetString(iReintfinal);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReintestado = dr.GetOrdinal(this.Reintestado);
            if (!dr.IsDBNull(iReintestado)) entity.Reintestado = dr.GetString(iReintestado);

            int iReintmotivoanulacion = dr.GetOrdinal(this.Reintmotivoanulacion);
            if (!dr.IsDBNull(iReintmotivoanulacion)) entity.Reintmotivoanulacion = dr.GetString(iReintmotivoanulacion);

            int iReintusueliminacion = dr.GetOrdinal(this.Reintusueliminacion);
            if (!dr.IsDBNull(iReintusueliminacion)) entity.Reintusueliminacion = dr.GetString(iReintusueliminacion);

            int iReintfecanulacion = dr.GetOrdinal(this.Reintfecanulacion);
            if (!dr.IsDBNull(iReintfecanulacion)) entity.Reintfecanulacion = dr.GetDateTime(iReintfecanulacion);

            int iReintcorrelativo = dr.GetOrdinal(this.Reintcorrelativo);
            if (!dr.IsDBNull(iReintcorrelativo)) entity.Reintcorrelativo = Convert.ToInt32(dr.GetValue(iReintcorrelativo));

            int iReinttipcliente = dr.GetOrdinal(this.Reinttipcliente);
            if (!dr.IsDBNull(iReinttipcliente)) entity.Reinttipcliente = dr.GetString(iReinttipcliente);

            int iReintcliente = dr.GetOrdinal(this.Reintcliente);
            if (!dr.IsDBNull(iReintcliente)) entity.Reintcliente = Convert.ToInt32(dr.GetValue(iReintcliente));

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iReintptoentrega = dr.GetOrdinal(this.Reintptoentrega);
            if (!dr.IsDBNull(iReintptoentrega)) entity.Reintptoentrega = dr.GetString(iReintptoentrega);

            int iReintnrosuministro = dr.GetOrdinal(this.Reintnrosuministro);
            if (!dr.IsDBNull(iReintnrosuministro)) entity.Reintnrosuministro = dr.GetString(iReintnrosuministro);

            int iRentcodi = dr.GetOrdinal(this.Rentcodi);
            if (!dr.IsDBNull(iRentcodi)) entity.Rentcodi = Convert.ToInt32(dr.GetValue(iRentcodi));

            int iReintaplicacionnumeral = dr.GetOrdinal(this.Reintaplicacionnumeral);
            if (!dr.IsDBNull(iReintaplicacionnumeral)) entity.Reintaplicacionnumeral = Convert.ToInt32(dr.GetValue(iReintaplicacionnumeral));

            int iReintenergiasemestral = dr.GetOrdinal(this.Reintenergiasemestral);
            if (!dr.IsDBNull(iReintenergiasemestral)) entity.Reintenergiasemestral = dr.GetDecimal(iReintenergiasemestral);

            int iReintinctolerancia = dr.GetOrdinal(this.Reintinctolerancia);
            if (!dr.IsDBNull(iReintinctolerancia)) entity.Reintinctolerancia = dr.GetString(iReintinctolerancia);

            int iRetintcodi = dr.GetOrdinal(this.Retintcodi);
            if (!dr.IsDBNull(iRetintcodi)) entity.Retintcodi = Convert.ToInt32(dr.GetValue(iRetintcodi));

            int iRecintcodi = dr.GetOrdinal(this.Recintcodi);
            if (!dr.IsDBNull(iRecintcodi)) entity.Recintcodi = Convert.ToInt32(dr.GetValue(iRecintcodi));

            int iReintni = dr.GetOrdinal(this.Reintni);
            if (!dr.IsDBNull(iReintni)) entity.Reintni = dr.GetDecimal(iReintni);

            int iReintki = dr.GetOrdinal(this.Reintki);
            if (!dr.IsDBNull(iReintki)) entity.Reintki = dr.GetDecimal(iReintki);

            int iReintfejeinicio = dr.GetOrdinal(this.Reintfejeinicio);
            if (!dr.IsDBNull(iReintfejeinicio)) entity.Reintfejeinicio = dr.GetDateTime(iReintfejeinicio);

            int iReintfejefin = dr.GetOrdinal(this.Reintfejefin);
            if (!dr.IsDBNull(iReintfejefin)) entity.Reintfejefin = dr.GetDateTime(iReintfejefin);

            int iReintfproginicio = dr.GetOrdinal(this.Reintfproginicio);
            if (!dr.IsDBNull(iReintfproginicio)) entity.Reintfproginicio = dr.GetDateTime(iReintfproginicio);

            int iReintfprogfin = dr.GetOrdinal(this.Reintfprogfin);
            if (!dr.IsDBNull(iReintfprogfin)) entity.Reintfprogfin = dr.GetDateTime(iReintfprogfin);

            int iReintcausaresumida = dr.GetOrdinal(this.Reintcausaresumida);
            if (!dr.IsDBNull(iReintcausaresumida)) entity.Reintcausaresumida = dr.GetString(iReintcausaresumida);

            int iReinteie = dr.GetOrdinal(this.Reinteie);
            if (!dr.IsDBNull(iReinteie)) entity.Reinteie = dr.GetDecimal(iReinteie);

            int iReintresarcimiento = dr.GetOrdinal(this.Reintresarcimiento);
            if (!dr.IsDBNull(iReintresarcimiento)) entity.Reintresarcimiento = dr.GetDecimal(iReintresarcimiento);

            int iReintevidencia = dr.GetOrdinal(this.Reintevidencia);
            if (!dr.IsDBNull(iReintevidencia)) entity.Reintevidencia = dr.GetString(iReintevidencia);

            int iReintdescontroversia = dr.GetOrdinal(this.Reintdescontroversia);
            if (!dr.IsDBNull(iReintdescontroversia)) entity.Reintdescontroversia = dr.GetString(iReintdescontroversia);

            int iReintcomentario = dr.GetOrdinal(this.Reintcomentario);
            if (!dr.IsDBNull(iReintcomentario)) entity.Reintcomentario = dr.GetString(iReintcomentario);

            int iReintusucreacion = dr.GetOrdinal(this.Reintusucreacion);
            if (!dr.IsDBNull(iReintusucreacion)) entity.Reintusucreacion = dr.GetString(iReintusucreacion);

            int iReintfeccreacion = dr.GetOrdinal(this.Reintfeccreacion);
            if (!dr.IsDBNull(iReintfeccreacion)) entity.Reintfeccreacion = dr.GetDateTime(iReintfeccreacion);

            int iReintreftrimestral = dr.GetOrdinal(this.Reintreftrimestral);
            if (!dr.IsDBNull(iReintreftrimestral)) entity.Reintreftrimestral = dr.GetString(iReintreftrimestral);

            return entity;
        }


        #region Mapeo de Campos

        public string Reintcodi = "REINTCODI";
        public string Repercodi = "REPERCODI";
        public string Reintpadre = "REINTPADRE";
        public string Reintfinal = "REINTFINAL";
        public string Emprcodi = "EMPRCODI";
        public string Reintestado = "REINTESTADO";
        public string Reintmotivoanulacion = "REINTMOTIVOANULACION";
        public string Reintusueliminacion = "REINTUSUELIMINACION";
        public string Reintfecanulacion = "REINTFECANULACION";
        public string Reintcorrelativo = "REINTCORRELATIVO";
        public string Reinttipcliente = "REINTTIPCLIENTE";
        public string Reintcliente = "REINTCLIENTE";
        public string Repentcodi = "REPENTCODI";
        public string Reintptoentrega = "REINTPTOENTREGA";
        public string Reintnrosuministro = "REINTNROSUMINISTRO";
        public string Rentcodi = "RENTCODI";
        public string Reintaplicacionnumeral = "REINTAPLICACIONNUMERAL";
        public string Reintenergiasemestral = "REINTENERGIASEMESTRAL";
        public string Reintinctolerancia = "REINTINCTOLERANCIA";
        public string Retintcodi = "RETINTCODI";
        public string Recintcodi = "RECINTCODI";
        public string Reintni = "REINTNI";
        public string Reintki = "REINTKI";
        public string Reintfejeinicio = "REINTFEJEINICIO";
        public string Reintfejefin = "REINTFEJEFIN";
        public string Reintfproginicio = "REINTFPROGINICIO";
        public string Reintfprogfin = "REINTFPROGFIN";
        public string Reintcausaresumida = "REINTCAUSARESUMIDA";
        public string Reinteie = "REINTEIE";
        public string Reintresarcimiento = "REINTRESARCIMIENTO";
        public string Reintevidencia = "REINTEVIDENCIA";
        public string Reintdescontroversia = "REINTDESCONTROVERSIA";
        public string Reintcomentario = "REINTCOMENTARIO";
        public string Reintusucreacion = "REINTUSUCREACION";
        public string Reintfeccreacion = "REINTFECCREACION";
        public string Emprnomb = "EMPRNOMB";
        public string Emprresponsable = "EMPRRESPONSABLE";

        public string Cliente = "CLIENTE";
        public string NivelTension = "RENTABREV";
        public string TipoInterrupcion = "RETINTNOMBRE";
        public string CausaInterrupcion = "RECINTNOMBRE";

        public string Reintreftrimestral = "REINTREFTRIMESTRAL";

        #endregion

        public string SqlObtenerPorEmpresaPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorEmpresaPeriodo"); }
        }

        public string SqlAnularInterrupcion
        {
            get { return base.GetSqlXml("AnularInterrupcion"); }
        }

        public string SqlObtenerInterrupcionPorResponsable
        {
            get { return base.GetSqlXml("ObtenerInterrupcionPorResponsable"); }
        }

        public string SqlObtenerConsolidado
        {
            get { return base.GetSqlXml("ObtenerConsolidado"); }
        }

        public string SqlActualizarDecisionControversia
        {
            get { return base.GetSqlXml("ActualizarDecisionControversia"); }
        }

        public string SqlObtenerTrazabilidad
        {
            get { return base.GetSqlXml("ObtenerTrazabilidad"); }
        }

        public string SqlActualizarArchivo
        {
            get { return base.GetSqlXml("ActualizarArchivo"); }
        }

        public string SqlObtenerNotificacionInterrupcion
        {
            get { return base.GetSqlXml("ObtenerNotificacionInterrupcion"); }
        }

        public string SqlActualizarResarcimiento
        {
            get { return base.GetSqlXml("ActualizarResarcimiento"); }
        }
    }
}
