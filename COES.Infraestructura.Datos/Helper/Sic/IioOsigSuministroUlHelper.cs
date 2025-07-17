using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_OSIG_SUMINISTRO_UL
    /// </summary>
    public class IioOsigSuministroUlHelper : HelperBase
    {

        public IioOsigSuministroUlHelper()
            : base(Consultas.IioOsigSuministroUlSql)
        {
        }

        #region Mapeo de Campos

        public string TableName = "IIO_OSIG_SUMINISTRO_UL";

        public string Psiclicodi = "PSICLICODI";
        public string Ulsumicodempresa = "ULSUMICODEMPRESA"; 
        public string Ulsumicodsuministro = "ULSUMICODSUMINISTRO";
        public string Ulsuminombreusuariolibre = "ULSUMINOMBREUSUARIOLIBRE";
        public string Ulsumidireccionsuministro = "ULSUMIDIRECCIONSUMINISTRO";
        public string Ulsuminrosuministroemp = "ULSUMINROSUMINISTROEMP";
        public string Ulsumiubigeo = "ULSUMIUBIGEO";
        public string Ulsumicodusuariolibre = "ULSUMICODUSUARIOLIBRE";
        public string Ulsumicodciiu = "ULSUMICODCIIU";
        public string Equicodi = "EQUICODI";
        public string Ulsumiusucreacion = "ULSUMIUSUCREACION";
        public string Ulsumifeccreacion = "ULSUMIFECCREACION";
        public string Ulsumiusumodificacion = "ULSUMIUSUMODIFICACION";
        public string Ulsumifecmodificacion = "ULSUMIFECMODIFICACION";

        public string Correlativo = "CORRELATIVO";

        #endregion

        public string SqlUpdateOsigSuministro
        {
            get { return GetSqlXml("UpdateOsigSuministro"); }
        }

        public string SqlValidarEquipos
        {
            get { return GetSqlXml("ValidarEquipos"); }
        }
        public string SqlGetMaxIdIioLogImportacion
        {
            get { return GetSqlXml("GetMaxIdIioLogImportacion"); }
        }

        public string SqlRegistrarLogimportacionEquipo
        {
            get { return GetSqlXml("RegistrarLogimportacionEquipo"); }
        }

    }
}