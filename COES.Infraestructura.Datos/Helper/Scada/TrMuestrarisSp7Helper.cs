using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_MUESTRARIS_SP7
    /// </summary>
    public class TrMuestrarisSp7Helper : HelperBase
    {
        public TrMuestrarisSp7Helper(): base(Consultas.TrMuestrarisSp7Sql)
        {
        }

        public TrMuestrarisSp7DTO Create(IDataReader dr)
        {
            TrMuestrarisSp7DTO entity = new TrMuestrarisSp7DTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCanalfecha = dr.GetOrdinal(this.Canalfecha);
            if (!dr.IsDBNull(iCanalfecha)) entity.Canalfecha = dr.GetDateTime(iCanalfecha);

            int iCanalcalidad = dr.GetOrdinal(this.Canalcalidad);
            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

            int iCanalfhora = dr.GetOrdinal(this.Canalfhora);
            if (!dr.IsDBNull(iCanalfhora)) entity.Canalfhora = dr.GetDateTime(iCanalfhora);

            int iCanalfhora2 = dr.GetOrdinal(this.Canalfhora2);
            if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

            int iCanalnomb = dr.GetOrdinal(this.Canalnomb);
            if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

            int iCanaliccp = dr.GetOrdinal(this.Canaliccp);
            if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCanalvalor = dr.GetOrdinal(this.Canalvalor);
            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

            int iEstado = dr.GetOrdinal(this.Estado);
            if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

            int iMuerisusucreacion = dr.GetOrdinal(this.Muerisusucreacion);
            if (!dr.IsDBNull(iMuerisusucreacion)) entity.Muerisusucreacion = dr.GetString(iMuerisusucreacion);

            int iMuerisfeccreacion = dr.GetOrdinal(this.Muerisfeccreacion);
            if (!dr.IsDBNull(iMuerisfeccreacion)) entity.Muerisfeccreacion = dr.GetDateTime(iMuerisfeccreacion);

            int iMuerisusumodificacion = dr.GetOrdinal(this.Muerisusumodificacion);
            if (!dr.IsDBNull(iMuerisusumodificacion)) entity.Muerisusumodificacion = dr.GetString(iMuerisusumodificacion);

            int iMuerisfecmodificacion = dr.GetOrdinal(this.Muerisfecmodificacion);
            if (!dr.IsDBNull(iMuerisfecmodificacion)) entity.Muerisfecmodificacion = dr.GetDateTime(iMuerisfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Canalfecha = "CANALFECHA";
        public string Canalcalidad = "CANALCALIDAD";
        public string Canalfhora = "CANALFHORA";
        public string Canalfhora2 = "CANALFHORA2";
        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Emprcodi = "EMPRCODI";
        public string Canalvalor = "CANALVALOR";
        public string Estado = "ESTADO";
        public string Muerisusucreacion = "MUERISUSUCREACION";
        public string Muerisfeccreacion = "MUERISFECCREACION";
        public string Muerisusumodificacion = "MUERISUSUMODIFICACION";
        public string Muerisfecmodificacion = "MUERISFECMODIFICACION";



        #endregion

        public string SqlGetMuestraRis
        {
            get { return base.GetSqlXml("GetMuestraRis"); }
        }


        public string SqlObtenerPaginado
        {
            get { return base.GetSqlXml("ObtenerPaginado"); }
        }











    }
}
