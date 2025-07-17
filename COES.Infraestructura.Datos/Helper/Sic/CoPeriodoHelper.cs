using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_PERIODO
    /// </summary>
    public class CoPeriodoHelper : HelperBase
    {
        public CoPeriodoHelper(): base(Consultas.CoPeriodoSql)
        {
        }

        public CoPeriodoDTO Create(IDataReader dr)
        {
            CoPeriodoDTO entity = new CoPeriodoDTO();

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCoperanio = dr.GetOrdinal(this.Coperanio);
            if (!dr.IsDBNull(iCoperanio)) entity.Coperanio = Convert.ToInt32(dr.GetValue(iCoperanio));

            int iCopermes = dr.GetOrdinal(this.Copermes);
            if (!dr.IsDBNull(iCopermes)) entity.Copermes = Convert.ToInt32(dr.GetValue(iCopermes));

            int iCopernomb = dr.GetOrdinal(this.Copernomb);
            if (!dr.IsDBNull(iCopernomb)) entity.Copernomb = dr.GetString(iCopernomb);

            int iCoperestado = dr.GetOrdinal(this.Coperestado);
            if (!dr.IsDBNull(iCoperestado)) entity.Coperestado = dr.GetString(iCoperestado);

            int iCoperusucreacion = dr.GetOrdinal(this.Coperusucreacion);
            if (!dr.IsDBNull(iCoperusucreacion)) entity.Coperusucreacion = dr.GetString(iCoperusucreacion);

            int iCopperfeccreacion = dr.GetOrdinal(this.Copperfeccreacion);
            if (!dr.IsDBNull(iCopperfeccreacion)) entity.Copperfeccreacion = dr.GetDateTime(iCopperfeccreacion);

            int iCopperusumodificacion = dr.GetOrdinal(this.Copperusumodificacion);
            if (!dr.IsDBNull(iCopperusumodificacion)) entity.Copperusumodificacion = dr.GetString(iCopperusumodificacion);

            int iCopperfecmodificacion = dr.GetOrdinal(this.Copperfecmodificacion);
            if (!dr.IsDBNull(iCopperfecmodificacion)) entity.Copperfecmodificacion = dr.GetDateTime(iCopperfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Copercodi = "COPERCODI";
        public string Coperanio = "COPERANIO";
        public string Copermes = "COPERMES";
        public string Copernomb = "COPERNOMB";
        public string Coperestado = "COPERESTADO";
        public string Coperusucreacion = "COPERUSUCREACION";
        public string Copperfeccreacion = "COPPERFECCREACION";
        public string Copperusumodificacion = "COPPERUSUMODIFICACION";
        public string Copperfecmodificacion = "COPPERFECMODIFICACION";
        public string Ultimaversion = "ULTIMAVERSION";


        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlGetMensualByFecha
        {
            get { return base.GetSqlXml("GetMensualByFecha"); }
        }
        

        #endregion
    }
}
