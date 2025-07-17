using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Runtime.Remoting.Messaging;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_GPS
    /// </summary>
    public class MeGpsHelper : HelperBase
    {
        public MeGpsHelper(): base(Consultas.MeGpsSql)
        {
        }

        public MeGpsDTO Create(IDataReader dr)
        {
            MeGpsDTO entity = new MeGpsDTO();

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iNombre = dr.GetOrdinal(this.Nombre);
            if (!dr.IsDBNull(iNombre)) entity.Nombre = dr.GetString(iNombre);

            int iGpsoficial = dr.GetOrdinal(this.Gpsoficial);
            if (!dr.IsDBNull(iGpsoficial)) entity.Gpsoficial = dr.GetString(iGpsoficial);

            int iGpsosinerg = dr.GetOrdinal(this.Gpsosinerg);
            if (!dr.IsDBNull(iGpsosinerg)) entity.Gpsosinerg = dr.GetString(iGpsosinerg);

            int iGpsestado = dr.GetOrdinal(this.Gpsestado);
            if (!dr.IsDBNull(iGpsestado)) entity.Gpsestado = dr.GetString(iGpsestado);

            int iGpsindieod = dr.GetOrdinal(this.Gpsindieod);
            if (!dr.IsDBNull(iGpsindieod)) entity.Gpsindieod = dr.GetString(iGpsindieod);

            return entity;
        }

        public MeGpsDTO Create2(IDataReader dr)
        {
            MeGpsDTO entity = new MeGpsDTO();

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iNombre = dr.GetOrdinal(this.Nombre);
            if (!dr.IsDBNull(iNombre)) entity.Nombre = dr.GetString(iNombre);

            int iGpsoficial = dr.GetOrdinal(this.Gpsoficial);
            if (!dr.IsDBNull(iGpsoficial)) entity.Gpsoficial = dr.GetString(iGpsoficial);

            int iGpsosinerg = dr.GetOrdinal(this.Gpsosinerg);
            if (!dr.IsDBNull(iGpsosinerg)) entity.Gpsosinerg = dr.GetString(iGpsosinerg);

            return entity;
        }


        #region Mapeo de Campos

        public string Gpscodi = "GPSCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Nombre = "NOMBRE";
        public string Gpsoficial = "GPSOFICIAL";
        public string Gpsosinerg = "GPSOSINERG";
        public string Gpsindieod = "GPSINDIEOD";
        public string Gpsestado = "GPSESTADO";

        #endregion

        public string SqlObtenerListadoGPS
        {
            get { return base.GetSqlXml("ObtenerListadoGPS"); }
        }

        public string SqlActualizarGPSIEOD
        {
            get { return base.GetSqlXml("ActualizarGPSIEOD"); }
        }
    }
}
