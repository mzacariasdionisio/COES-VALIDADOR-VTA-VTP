using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_RECURSO
    /// </summary>
    public class CpRecursoHelper : HelperBase
    {
        public CpRecursoHelper() : base(Consultas.CpRecursoSql)
        {
        }

        public CpRecursoDTO Create(IDataReader dr)
        {
            CpRecursoDTO entity = new CpRecursoDTO();

            int iRecurconsideragams = dr.GetOrdinal(this.Recurconsideragams);
            if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));

            int iRecurfamsic = dr.GetOrdinal(this.Recurfamsic);
            if (!dr.IsDBNull(iRecurfamsic)) entity.Recurfamsic = Convert.ToInt32(dr.GetValue(iRecurfamsic));

            int iRecurlogico = dr.GetOrdinal(this.Recurlogico);
            if (!dr.IsDBNull(iRecurlogico)) entity.Recurlogico = Convert.ToInt32(dr.GetValue(iRecurlogico));

            int iRecurformula = dr.GetOrdinal(this.Recurformula);
            if (!dr.IsDBNull(iRecurformula)) entity.Recurformula = dr.GetString(iRecurformula);

            int iRecurtoescenario = dr.GetOrdinal(this.Recurtoescenario);
            if (!dr.IsDBNull(iRecurtoescenario)) entity.Recurtoescenario = Convert.ToInt32(dr.GetValue(iRecurtoescenario));

            int iRecurorigen3 = dr.GetOrdinal(this.Recurorigen3);
            if (!dr.IsDBNull(iRecurorigen3)) entity.Recurorigen3 = Convert.ToInt32(dr.GetValue(iRecurorigen3));

            int iRecurorigen2 = dr.GetOrdinal(this.Recurorigen2);
            if (!dr.IsDBNull(iRecurorigen2)) entity.Recurorigen2 = Convert.ToInt32(dr.GetValue(iRecurorigen2));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCatcodi = dr.GetOrdinal(this.Catcodi);
            if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iRecurestado = dr.GetOrdinal(this.Recurestado);
            if (!dr.IsDBNull(iRecurestado)) entity.Recurestado = Convert.ToInt32(dr.GetValue(iRecurestado));

            int iTablasicoes = dr.GetOrdinal(this.Tablasicoes);
            if (!dr.IsDBNull(iTablasicoes)) entity.Tablasicoes = dr.GetString(iTablasicoes);

            int iRecurcodisicoes = dr.GetOrdinal(this.Recurcodisicoes);
            if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodisicoes));

            int iRecurorigen = dr.GetOrdinal(this.Recurorigen);
            if (!dr.IsDBNull(iRecurorigen)) entity.Recurorigen = Convert.ToInt32(dr.GetValue(iRecurorigen));

            int iRecurpadre = dr.GetOrdinal(this.Recurpadre);
            if (!dr.IsDBNull(iRecurpadre)) entity.Recurpadre = Convert.ToInt32(dr.GetValue(iRecurpadre));

            int iRecurnombre = dr.GetOrdinal(this.Recurnombre);
            if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Recurconsideragams = "RECURCONSIDERAGAMS";
        public string Recurfamsic = "RECURFAMSIC";
        public string Recurlogico = "RECURLOGICO";
        public string Recurformula = "RECURFORMULA";
        public string Recurtoescenario = "RECURTOESCENARIO";
        public string Recurorigen3 = "RECURORIGEN3";
        public string Recurorigen2 = "RECURORIGEN2";
        public string Topcodi = "TOPCODI";
        public string Catcodi = "CATCODI";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Recurestado = "RECURESTADO";
        public string Tablasicoes = "TABLASICOES";
        public string Recurcodisicoes = "RECURCODISICOES";
        public string Recurorigen = "RECURORIGEN";
        public string Recurpadre = "RECURPADRE";
        public string Recurnombre = "RECURNOMBRE";
        public string Recurcodi = "RECURCODI";

        public string Equinomb = "EQUINOMB";
        public string Propcodi = "PROPCODI";
        public new string Valor = "VALOR";
        public string Equipadre = "EQUIPADRE";

        public string Recurcodicentral = "RECURCODICENTRAL";
        public string Recurcodisicoescentral = "RECURCODISICOESCENTRAL";
        public string Recurcodibarra = "RECURCODIBARRA";
        public string Recurcodisicoesbarra = "RECURCODISICOESBARRA";

        //Yupama
        public string Ursmax = "URSMAX";
        public string Ursmin = "URSMIN";
        public string Gequicodi = "GEQUICODI";
        public string Gequinomb = "GEQUINOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Centralnomb = "CENTRALNOMB";

        //Yupana Continuo
        public string Recurnombsicoes = "RECURNOMBSICOES";

        #endregion

        /// <summary>
        /// Retorna las sentencias SQL
        /// </summary>
        #region Sentencias SQL

        public string SqlObtenerPorTopologiaYCategoria => GetSqlXml("ObtenerPorTopologiaYCategoria");
        public string SqlObtenerListaRelacionBarraCentral => GetSqlXml("ObtenerListaRelacionBarraCentral");
        //Yupana
        public string SqlListaUrsEmpresaAnexo5 => GetSqlXml("ListaUrsEmpresaAnexo5");
        #region Yupana Continuo
        public string GetSqlRecursoxTopologia
        {
            get { return base.GetSqlXml("ListarRecursoxTopologia"); }
        }

        public string SqlListaCategoria
        {
            get { return base.GetSqlXml("ListaCategoria"); }
        }

        public string GetSqlListarLinea01
        {
            get { return base.GetSqlXml("ListarLinea01"); }
        }

        public string GetSqlListarLinea02
        {
            get { return base.GetSqlXml("ListarLinea02"); }
        }

        public string SqlRecursoxCategoriaGrupo
        {
            get { return base.GetSqlXml("RecursoxCategoriaGrupo"); }
        }

        public string GetSqlRecursoxCategoria2
        {
            get { return base.GetSqlXml("RecursoxCategoria2"); }
        }

        public string GetSqlRecursoxCategoria4
        {
            get { return base.GetSqlXml("RecursoxCategoria4"); }
        }

        public string SqlListaModosXNodoT
        {
            get { return base.GetSqlXml("ListaModosXNodoT"); }
        }

        public string SqlEquiposConecANodoTop
        {
            get { return base.GetSqlXml("EquiposConecANodoTop"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        #endregion
        #endregion

        #region CMgCP_PR07

        public string SqlObtenerEmbalsesYUPANA
        {
            get { return base.GetSqlXml("ObtenerEmbalsesYupana"); }
        }

        #endregion
    }
}
