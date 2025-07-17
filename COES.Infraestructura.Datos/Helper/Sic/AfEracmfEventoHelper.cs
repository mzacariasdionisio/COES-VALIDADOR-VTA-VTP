using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_ERACMF_EVENTO
    /// </summary>
    public class AfEracmfEventoHelper : HelperBase
    {
        public AfEracmfEventoHelper() : base(Consultas.AfEracmfEventoSql)
        {
        }

        public AfEracmfEventoDTO Create(IDataReader dr)
        {
            AfEracmfEventoDTO entity = new AfEracmfEventoDTO();

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEracmfusumodificacion = dr.GetOrdinal(this.Eracmfusumodificacion);
            if (!dr.IsDBNull(iEracmfusumodificacion)) entity.Eracmfusumodificacion = dr.GetString(iEracmfusumodificacion);

            int iEracmfusucreacion = dr.GetOrdinal(this.Eracmfusucreacion);
            if (!dr.IsDBNull(iEracmfusucreacion)) entity.Eracmfusucreacion = dr.GetString(iEracmfusucreacion);

            int iEracmffecmodificacion = dr.GetOrdinal(this.Eracmffecmodificacion);
            if (!dr.IsDBNull(iEracmffecmodificacion)) entity.Eracmffecmodificacion = dr.GetDateTime(iEracmffecmodificacion);

            int iEracmffeccreacion = dr.GetOrdinal(this.Eracmffeccreacion);
            if (!dr.IsDBNull(iEracmffeccreacion)) entity.Eracmffeccreacion = dr.GetDateTime(iEracmffeccreacion);

            int iEracmfcodrele = dr.GetOrdinal(this.Eracmfcodrele);
            if (!dr.IsDBNull(iEracmfcodrele)) entity.Eracmfcodrele = dr.GetString(iEracmfcodrele);

            int iEracmftiporegistro = dr.GetOrdinal(this.Eracmftiporegistro);
            if (!dr.IsDBNull(iEracmftiporegistro)) entity.Eracmftiporegistro = dr.GetString(iEracmftiporegistro);

            int iEracmffechretiro = dr.GetOrdinal(this.Eracmffechretiro);
            if (!dr.IsDBNull(iEracmffechretiro)) entity.Eracmffechretiro = dr.GetString(iEracmffechretiro);

            int iEracmffechingreso = dr.GetOrdinal(this.Eracmffechingreso);
            if (!dr.IsDBNull(iEracmffechingreso)) entity.Eracmffechingreso = dr.GetString(iEracmffechingreso);

            int iEracmffechimplementacion = dr.GetOrdinal(this.Eracmffechimplementacion);
            if (!dr.IsDBNull(iEracmffechimplementacion)) entity.Eracmffechimplementacion = dr.GetString(iEracmffechimplementacion);

            int iEracmfobservaciones = dr.GetOrdinal(this.Eracmfobservaciones);
            if (!dr.IsDBNull(iEracmfobservaciones)) entity.Eracmfobservaciones = dr.GetString(iEracmfobservaciones);

            int iEracmfsuministrador = dr.GetOrdinal(this.Eracmfsuministrador);
            if (!dr.IsDBNull(iEracmfsuministrador)) entity.Eracmfsuministrador = dr.GetString(iEracmfsuministrador);

            int iEracmfdreferencia = dr.GetOrdinal(this.Eracmfdreferencia);
            if (!dr.IsDBNull(iEracmfdreferencia)) entity.Eracmfdreferencia = dr.GetDecimal(iEracmfdreferencia);

            int iEracmfmindregistrada = dr.GetOrdinal(this.Eracmfmindregistrada);
            if (!dr.IsDBNull(iEracmfmindregistrada)) entity.Eracmfmindregistrada = dr.GetDecimal(iEracmfmindregistrada);

            int iEracmfmediadregistrada = dr.GetOrdinal(this.Eracmfmediadregistrada);
            if (!dr.IsDBNull(iEracmfmediadregistrada)) entity.Eracmfmediadregistrada = dr.GetDecimal(iEracmfmediadregistrada);

            int iEracmfmaxdregistrada = dr.GetOrdinal(this.Eracmfmaxdregistrada);
            if (!dr.IsDBNull(iEracmfmaxdregistrada)) entity.Eracmfmaxdregistrada = dr.GetDecimal(iEracmfmaxdregistrada);

            int iEracmftiemporderivada = dr.GetOrdinal(this.Eracmftiemporderivada);
            if (!dr.IsDBNull(iEracmftiemporderivada)) entity.Eracmftiemporderivada = dr.GetDecimal(iEracmftiemporderivada);

            int iEracmfdfdtrderivada = dr.GetOrdinal(this.Eracmfdfdtrderivada);
            if (!dr.IsDBNull(iEracmfdfdtrderivada)) entity.Eracmfdfdtrderivada = dr.GetDecimal(iEracmfdfdtrderivada);

            int iEracmfarranqrderivada = dr.GetOrdinal(this.Eracmfarranqrderivada);
            if (!dr.IsDBNull(iEracmfarranqrderivada)) entity.Eracmfarranqrderivada = dr.GetDecimal(iEracmfarranqrderivada);

            int iEracmftiemporumbral = dr.GetOrdinal(this.Eracmftiemporumbral);
            if (!dr.IsDBNull(iEracmftiemporumbral)) entity.Eracmftiemporumbral = dr.GetDecimal(iEracmftiemporumbral);

            int iEracmfarranqrumbral = dr.GetOrdinal(this.Eracmfarranqrumbral);
            if (!dr.IsDBNull(iEracmfarranqrumbral)) entity.Eracmfarranqrumbral = dr.GetDecimal(iEracmfarranqrumbral);

            int iEracmfnumetapa = dr.GetOrdinal(this.Eracmfnumetapa);
            if (!dr.IsDBNull(iEracmfnumetapa)) entity.Eracmfnumetapa = dr.GetString(iEracmfnumetapa);

            int iEracmfcodinterruptor = dr.GetOrdinal(this.Eracmfcodinterruptor);
            if (!dr.IsDBNull(iEracmfcodinterruptor)) entity.Eracmfcodinterruptor = dr.GetString(iEracmfcodinterruptor);

            int iEracmfciralimentador = dr.GetOrdinal(this.Eracmfciralimentador);
            if (!dr.IsDBNull(iEracmfciralimentador)) entity.Eracmfciralimentador = dr.GetString(iEracmfciralimentador);

            int iEracmfnivtension = dr.GetOrdinal(this.Eracmfnivtension);
            if (!dr.IsDBNull(iEracmfnivtension)) entity.Eracmfnivtension = dr.GetString(iEracmfnivtension);

            int iEracmfsubestacion = dr.GetOrdinal(this.Eracmfsubestacion);
            if (!dr.IsDBNull(iEracmfsubestacion)) entity.Eracmfsubestacion = dr.GetString(iEracmfsubestacion);

            int iEracmfnroserie = dr.GetOrdinal(this.Eracmfnroserie);
            if (!dr.IsDBNull(iEracmfnroserie)) entity.Eracmfnroserie = dr.GetString(iEracmfnroserie);

            int iEracmfmodelo = dr.GetOrdinal(this.Eracmfmodelo);
            if (!dr.IsDBNull(iEracmfmodelo)) entity.Eracmfmodelo = dr.GetString(iEracmfmodelo);

            int iEracmfmarca = dr.GetOrdinal(this.Eracmfmarca);
            if (!dr.IsDBNull(iEracmfmarca)) entity.Eracmfmarca = dr.GetString(iEracmfmarca);

            int iEracmfzona = dr.GetOrdinal(this.Eracmfzona);
            if (!dr.IsDBNull(iEracmfzona)) entity.Eracmfzona = dr.GetString(iEracmfzona);

            int iEracmfemprnomb = dr.GetOrdinal(this.Eracmfemprnomb);
            if (!dr.IsDBNull(iEracmfemprnomb)) entity.Eracmfemprnomb = dr.GetString(iEracmfemprnomb);

            int iEracmfcodi = dr.GetOrdinal(this.Eracmfcodi);
            if (!dr.IsDBNull(iEracmfcodi)) entity.Eracmfcodi = Convert.ToInt32(dr.GetValue(iEracmfcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Evencodi = "EVENCODI";
        public string Eracmfusumodificacion = "ERACMFUSUMODIFICACION";
        public string Eracmfusucreacion = "ERACMFUSUCREACION";
        public string Eracmffecmodificacion = "ERACMFFECMODIFICACION";
        public string Eracmffeccreacion = "ERACMFFECCREACION";
        public string Eracmfcodrele = "ERACMFCODRELE";
        public string Eracmftiporegistro = "ERACMFTIPOREGISTRO";
        public string Eracmffechretiro = "ERACMFFECHRETIRO";
        public string Eracmffechingreso = "ERACMFFECHINGRESO";
        public string Eracmffechimplementacion = "ERACMFFECHIMPLEMENTACION";
        public string Eracmfobservaciones = "ERACMFOBSERVACIONES";
        public string Eracmfsuministrador = "ERACMFSUMINISTRADOR";
        public string Eracmfdreferencia = "ERACMFDREFERENCIA";
        public string Eracmfmindregistrada = "ERACMFMINDREGISTRADA";
        public string Eracmfmediadregistrada = "ERACMFMEDIADREGISTRADA";
        public string Eracmfmaxdregistrada = "ERACMFMAXDREGISTRADA";
        public string Eracmftiemporderivada = "ERACMFTIEMPORDERIVADA";
        public string Eracmfdfdtrderivada = "ERACMFDFDTRDERIVADA";
        public string Eracmfarranqrderivada = "ERACMFARRANQRDERIVADA";
        public string Eracmftiemporumbral = "ERACMFTIEMPORUMBRAL";
        public string Eracmfarranqrumbral = "ERACMFARRANQRUMBRAL";
        public string Eracmfnumetapa = "ERACMFNUMETAPA";
        public string Eracmfcodinterruptor = "ERACMFCODINTERRUPTOR";
        public string Eracmfciralimentador = "ERACMFCIRALIMENTADOR";
        public string Eracmfnivtension = "ERACMFNIVTENSION";
        public string Eracmfsubestacion = "ERACMFSUBESTACION";
        public string Eracmfnroserie = "ERACMFNROSERIE";
        public string Eracmfmodelo = "ERACMFMODELO";
        public string Eracmfmarca = "ERACMFMARCA";
        public string Eracmfzona = "ERACMFZONA";
        public string Eracmfemprnomb = "ERACMFEMPRNOMB";
        public string Eracmfcodi = "ERACMFCODI";

        #endregion


        public string SqlGetByEvencodi
        {
            get { return base.GetSqlXml("GetbyEvencodi"); }
        }



        #region Analisis Fallas

        public string SqlGetByEvento => GetSqlXml("GetByEvento");

        #endregion
    }
}
