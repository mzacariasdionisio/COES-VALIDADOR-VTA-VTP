using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroHistoricoHelper : HelperBase
    {
        public CpaParametroHistoricoHelper() : base(Consultas.CpaParametroHistoricoSql)
        {
        }

        #region Mapeo de Campos
        public string Cpaphscodi = "CPAPHSCODI";
        public string Cpaprmcodi = "CPAPRMCODI";
        public string Cpaphstipo = "CPAPHSTIPO";
        public string Cpaphsusuario = "CPAPHSUSUARIO";
        public string Cpaphsfecha = "CPAPHSFECHA";

        #endregion

        public string SqlListaParametrosHistoricos
        {
            get { return base.GetSqlXml("ListaParametrosHistoricos"); }
        }
    }
}
