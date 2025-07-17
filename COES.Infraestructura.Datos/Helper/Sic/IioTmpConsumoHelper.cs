using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_TMP_CONSUMO
    /// </summary>
    public class IioTmpConsumoHelper : HelperBase
    {

        public IioTmpConsumoHelper()
            : base(Consultas.IioTmpConsumoSql)
        {
        }

        #region Mapeo de Campos

        public string TableName = "IIO_TMP_CONSUMO";
        public string Psiclicodi = "PSICLICODI";
        public string Uconempcodi = "UCONEMPCODI";
        public string Sumucodi = "SUMUCODI";
        public string Uconfecha = "UCONFECHA";
        public string Uconptosumincodi = "UCONPTOSUMINCODI";
        public string Uconenergactv = "UCONENERGACTV";
        public string Uconenergreac = "UCONENERGREAC";
        public string Ptomedicodi = "PTOMEDICODI";

        #endregion

        public string SqlUpdateTmpConsumo
        {
            get { return base.GetSqlXml("UpdateTmpConsumo"); }
        }
        public string SqlGetSumucodi
        {
            get { return base.GetSqlXml("GetSumucodi"); }
        }
        public string SqlDeleteMeMedicion96
        {
            get { return base.GetSqlXml("DeleteMeMedicion96"); }
        }
        public string SqlSaveMedicion96
        {
            get { return base.GetSqlXml("SaveMedicion96"); }
        }
        public string SqlUpdateH96
        {
            get { return base.GetSqlXml("UpdateH96"); }
        }
    }
}