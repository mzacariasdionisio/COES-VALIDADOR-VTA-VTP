using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{

    /// <summary>
    /// Clase que contiene el mapeo de la tabla GMM_DATINSUMO
    /// </summary>
    public class GmmDatInsumoHelper : HelperBase
    {
        public GmmDatInsumoHelper()
            : base(Consultas.GmmDatInsumoSql)
        {
        }
        #region Mapeo de Campos
        public string Dcalcodi = "DCALCODI";
        public string Datinscodi = "DATINSCODI";
        public string Dinsvalor = "DINSVALOR";
        public string Empgcodi = "EMPGCODI";
        public string Pericodi = "PERICODI";
        public string Dcalvalor = "DCALVALOR";
        public string Tinscodi = "TINSCODI";
        public string Dinsanio = "DINSANIO";
        public string Dinsmes = "DINSMES";
        public string Dinsusucreacion = "DINSUSUCREACION";
        public string Dinsfeccreacion = "DINSFECCREACION";
        public string Dinsusumodificacion = "DINSUSUMODIFICACION";
        public string Dinsfecmodificacion = "DINSFECMODIFICACION";

        public string Listemprnomb = "LISTEMPRNOMB";
        public string Listempgcodi = "LISTEMPGCODI";
        public string Listdinsvalor = "LISTDINSVALOR";

        public string ENTREGAS = "ENTREGAS";
        public string SC = "SC";
        public string INFLEX = "INFLEX";
        public string RECAU = "RECAU";

        public string Dinsusuario = "DINSUSUARIO";
        #endregion

        public GmmDatInsumoDTO CreateListaDatosInsumo(IDataReader dr)
        {
            GmmDatInsumoDTO entity = new GmmDatInsumoDTO();

            int iListemprnomb = dr.GetOrdinal(this.Listemprnomb);
            if (!dr.IsDBNull(iListemprnomb)) entity.LISTEMPRNOMB = dr.GetString(iListemprnomb);

            int iListempgcodi = dr.GetOrdinal(this.Listempgcodi);
            if (!dr.IsDBNull(iListempgcodi)) entity.LISTEMPGCODI = dr.GetInt32(iListempgcodi);

            int iIncumInforme = dr.GetOrdinal(this.Listdinsvalor);
            if (!dr.IsDBNull(iIncumInforme)) entity.LISTDINSVALOR = dr.GetDecimal(iIncumInforme);

            return entity;
        }
        public GmmDatInsumoDTO CreateListadoInsumos(IDataReader dr)
        {
            GmmDatInsumoDTO entity = new GmmDatInsumoDTO();

            int iListemprnomb = dr.GetOrdinal(this.Listemprnomb);
            if (!dr.IsDBNull(iListemprnomb)) entity.LISTEMPRNOMB = dr.GetString(iListemprnomb);

            int iListempgcodi = dr.GetOrdinal(this.Listempgcodi);
            if (!dr.IsDBNull(iListempgcodi)) entity.LISTEMPGCODI = dr.GetInt32(iListempgcodi);

            int iENTREGAS = dr.GetOrdinal(this.ENTREGAS);
            if (!dr.IsDBNull(iENTREGAS)) entity.ENTREGAS = dr.GetDecimal(iENTREGAS);

            int iSC = dr.GetOrdinal(this.SC);
            if (!dr.IsDBNull(iSC)) entity.SC = dr.GetDecimal(iSC);

            int iINFLEX = dr.GetOrdinal(this.INFLEX);
            if (!dr.IsDBNull(iINFLEX)) entity.INFLEX = dr.GetDecimal(iINFLEX);

            int iRECAU = dr.GetOrdinal(this.RECAU);
            if (!dr.IsDBNull(iRECAU)) entity.RECAU = dr.GetDecimal(iRECAU);

            return entity;
        }
        public string SqlListaDatosInsumoTipoSC
        {
            get { return base.GetSqlXml("ListadoDatosInsumoTipoSC"); }
        }
        public string SqlUpsertDatosInsumoTipoSC
        {
            get { return base.GetSqlXml("UpsertDatosInsumoTipoSC"); }
        }
        public string SqlListaDatosEntregas
        {
            get { return base.GetSqlXml("ListadoDatosEntregas"); }
        }
        public string SqlUpsertDatosEntregas
        {
            get { return base.GetSqlXml("UpsertDatosEntregas"); }
        }
        public string SqlListaDatosInflexibilidad
        {
            get { return base.GetSqlXml("ListadoDatosInflexibilidad"); }
        }
        public string SqlUpsertDatosInflexibilidad
        {
            get { return base.GetSqlXml("UpsertDatosInflexibilidad"); }
        }
        public string SqlListaDatosRecaudacion
        {
            get { return base.GetSqlXml("ListadoDatosRecaudacion"); }
        }
        public string SqlListadoInsumos
        {
            get { return base.GetSqlXml("ListadoInsumos"); }
        }
        public string SqlUpsertDatosRecaudacion
        {
            get { return base.GetSqlXml("UpsertDatosRecaudacion"); }
        }
       
    }
}