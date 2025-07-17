using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_OSIG_CONSUMO_UL
    /// </summary>
    public class IioOsigConsumoUlHelper : HelperBase
    {

        public IioOsigConsumoUlHelper()
            : base(Consultas.IioOsigConsumoUlSql)
        {
        }

        #region Mapeo de Campos

        public string TableName = "IIO_OSIG_CONSUMO_UL";

        public string Psiclicodi = "PSICLICODI";
        public string Ulconcodempresa = "ULCONCODEMPRESA";
        public string Ulconcodsuministro = "ULCONCODSUMINISTRO";
        public string Ulconfecha = "ULCONFECHA";
        public string Ulconcodbarra = "ULCONCODBARRA";
        public string Ulconenergactv = "ULCONENERGACTV";
        public string Ulconenergreac = "ULCONENERGREAC";
        public string Ptomedicodi = "PTOMEDICODI";

        public string Ulconusucreacion = "ULCONUSUCREACION";
        public string Ulconfeccreacion = "ULCONFECCREACION";

        public string Emprcodisuministrador = "EMPRCODISUMINISTRADOR";

        public string Correlativo = "CORRELATIVO";

        #endregion

        public string SqlUpdateOsigConsumo
        {
            get { return base.GetSqlXml("UpdateOsigConsumo"); }
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
        public string SqlGetMaxIdIioLogImportacion
        {
            get { return GetSqlXml("GetMaxIdIioLogImportacion"); }
        }        

        public string SqlRegistrarLogimportacionPtoMedicion
        {
            get { return GetSqlXml("RegistrarLogimportacionPtoMedicion"); }
        }
        public string SqlSaveOsigConsumo
        {
            get { return GetSqlXml("SaveOsigConsumo"); }
        }
    }
}