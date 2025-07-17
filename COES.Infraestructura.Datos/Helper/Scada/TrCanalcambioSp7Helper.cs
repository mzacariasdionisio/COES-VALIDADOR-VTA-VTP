using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public class TrCanalcambioSp7Helper : HelperBase
    {
        public TrCanalcambioSp7Helper(): base(Consultas.TrCanalcambioSp7Sql)
        {
        }

        public TrCanalcambioSp7DTO Create(IDataReader dr)
        {
            TrCanalcambioSp7DTO entity = new TrCanalcambioSp7DTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCanalcmfeccreacion = dr.GetOrdinal(this.Canalcmfeccreacion);
            if (!dr.IsDBNull(iCanalcmfeccreacion)) entity.Canalcmfeccreacion = dr.GetDateTime(iCanalcmfeccreacion);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iCanalnomb = dr.GetOrdinal(this.Canalnomb);
            if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

            int iCanaliccp = dr.GetOrdinal(this.Canaliccp);
            if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

            int iCanalabrev = dr.GetOrdinal(this.Canalabrev);
            if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

            int iCanalunidad = dr.GetOrdinal(this.Canalunidad);
            if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

            int iCanalpathb = dr.GetOrdinal(this.Canalpathb);
            if (!dr.IsDBNull(iCanalpathb)) entity.Canalpathb = dr.GetString(iCanalpathb);

            int iCanalpointtype = dr.GetOrdinal(this.Canalpointtype);
            if (!dr.IsDBNull(iCanalpointtype)) entity.Canalpointtype = dr.GetString(iCanalpointtype);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEmprcodiant = dr.GetOrdinal(this.Emprcodiant);
            if (!dr.IsDBNull(iEmprcodiant)) entity.Emprcodiant = Convert.ToInt32(dr.GetValue(iEmprcodiant));

            int iZonacodiant = dr.GetOrdinal(this.Zonacodiant);
            if (!dr.IsDBNull(iZonacodiant)) entity.Zonacodiant = Convert.ToInt32(dr.GetValue(iZonacodiant));

            int iCanalnombant = dr.GetOrdinal(this.Canalnombant);
            if (!dr.IsDBNull(iCanalnombant)) entity.Canalnombant = dr.GetString(iCanalnombant);

            int iCanaliccpant = dr.GetOrdinal(this.Canaliccpant);
            if (!dr.IsDBNull(iCanaliccpant)) entity.Canaliccpant = dr.GetString(iCanaliccpant);

            int iCanalabrevant = dr.GetOrdinal(this.Canalabrevant);
            if (!dr.IsDBNull(iCanalabrevant)) entity.Canalabrevant = dr.GetString(iCanalabrevant);

            int iCanalunidadant = dr.GetOrdinal(this.Canalunidadant);
            if (!dr.IsDBNull(iCanalunidadant)) entity.Canalunidadant = dr.GetString(iCanalunidadant);

            int iCanalpathbant = dr.GetOrdinal(this.Canalpathbant);
            if (!dr.IsDBNull(iCanalpathbant)) entity.Canalpathbant = dr.GetString(iCanalpathbant);

            int iCanalpointtypeant = dr.GetOrdinal(this.Canalpointtypeant);
            if (!dr.IsDBNull(iCanalpointtypeant)) entity.Canalpointtypeant = dr.GetString(iCanalpointtypeant);

            int iLastuserant = dr.GetOrdinal(this.Lastuserant);
            if (!dr.IsDBNull(iLastuserant)) entity.Lastuserant = dr.GetString(iLastuserant);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Canalcmfeccreacion = "CANALCMFECCREACION";
        public string Emprcodi = "EMPRCODI";
        public string EmpresaNombre = "EMPRENOM_ACTUAL";
        public string Zonacodi = "ZONACODI";
        public string ZonaNombre = "ZONACODI_ACTUAL";
        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Canalabrev = "CANALABREV";
        public string Canalunidad = "CANALUNIDAD";
        public string Canalpathb = "CANALPATHB";
        public string Canalpointtype = "CANALPOINTTYPE";
        public string Lastuser = "LASTUSER";
        public string Emprcodiant = "EMPRCODIANT";
        public string EmpresaNombreant = "EMPRENOM_ANTERIOR";
        public string Zonacodiant = "ZONACODIANT";
        public string ZonaNombreant = "ZONACODI_ANTERIOR";
        public string Canalnombant = "CANALNOMBANT";
        public string Canaliccpant = "CANALICCPANT";
        public string Canalabrevant = "CANALABREVANT";
        public string Canalunidadant = "CANALUNIDADANT";
        public string Canalpathbant = "CANALPATHBANT";
        public string Canalpointtypeant = "CANALPOINTTYPEANT";
        public string Lastuserant = "LASTUSERANT";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }
        public string GetByFecha
        {
            get { return base.GetSqlXml("GetByFecha"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }
        

        #endregion
    }
}
