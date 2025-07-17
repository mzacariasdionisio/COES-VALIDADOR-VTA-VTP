using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_PARAMETRO_PRIMA
    /// </summary>
    public class RerParametroPrimaHelper : HelperBase
    {
        public RerParametroPrimaHelper() : base(Consultas.RerParametroPrimaSql)
        {
        }

        public RerParametroPrimaDTO Create(IDataReader dr)
        {
            RerParametroPrimaDTO entity = new RerParametroPrimaDTO();

            int iRerpprcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerpprcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerpprcodi));

            int iReravcodi = dr.GetOrdinal(this.Reravcodi);
            if (!dr.IsDBNull(iReravcodi)) entity.Reravcodi = Convert.ToInt32(dr.GetValue(iReravcodi));

            int iRerpprmes = dr.GetOrdinal(this.Rerpprmes);
            if (!dr.IsDBNull(iRerpprmes)) entity.Rerpprmes = Convert.ToInt32(dr.GetValue(iRerpprmes));

            int iRerpprmesaniodesc = dr.GetOrdinal(this.Rerpprmesaniodesc);
            if (!dr.IsDBNull(iRerpprmesaniodesc)) entity.Rerpprmesaniodesc = dr.GetString(iRerpprmesaniodesc);

            int iRerpprtipocambio = dr.GetOrdinal(this.Rerpprtipocambio);
            if (!dr.IsDBNull(iRerpprtipocambio)) entity.Rerpprtipocambio = Convert.ToDecimal(dr.GetValue(iRerpprtipocambio));

            int iRerpprorigen = dr.GetOrdinal(this.Rerpprorigen);
            if (!dr.IsDBNull(iRerpprorigen)) entity.Rerpprorigen = dr.GetString(iRerpprorigen);

            int iRerpprrevision = dr.GetOrdinal(this.Rerpprrevision);
            if (!dr.IsDBNull(iRerpprrevision)) entity.Rerpprrevision = dr.GetString(iRerpprrevision);

            int iRerpprusucreacion = dr.GetOrdinal(this.Rerpprusucreacion);
            if (!dr.IsDBNull(iRerpprusucreacion)) entity.Rerpprusucreacion = dr.GetString(iRerpprusucreacion);

            int iRerpprfeccreacion = dr.GetOrdinal(this.Rerpprfeccreacion);
            if (!dr.IsDBNull(iRerpprfeccreacion)) entity.Rerpprfeccreacion = dr.GetDateTime(iRerpprfeccreacion);

            int iRerpprusumodificacion = dr.GetOrdinal(this.Rerpprusumodificacion);
            if (!dr.IsDBNull(iRerpprusumodificacion)) entity.Rerpprusumodificacion = dr.GetString(iRerpprusumodificacion);

            int iRerPprFecModificacion = dr.GetOrdinal(this.Rerpprfecmodificacion);
            if (!dr.IsDBNull(iRerPprFecModificacion)) entity.Rerpprfecmodificacion = dr.GetDateTime(iRerPprFecModificacion);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));
            return entity;
        }

        #region Mapeo de Campos

        public string Rerpprcodi = "RERPPRCODI";
        public string Reravcodi = "RERAVCODI"; 
        public string Rerpprmes = "RERPPRMES";
        public string Rerpprmesaniodesc = "RERPPRMESANIODESC";
        public string Rerpprtipocambio = "RERPPRTIPOCAMBIO";
        public string Rerpprorigen = "RERPPRORIGEN";
        public string Rerpprrevision = "RERPPRREVISION";
        public string Rerpprusucreacion = "RERPPRUSUCREACION";
        public string Rerpprfeccreacion = "RERPPRFECCREACION";
        public string Rerpprusumodificacion = "RERPPRUSUMODIFICACION";
        public string Rerpprfecmodificacion = "RERPPRFECMODIFICACION";
        public string Pericodi = "PERICODI";
        public string Recacodi = "RECACODI";

        //iltros para consulta
        public string Reravaniotarif = "RERAVANIOTARIF";
        #endregion
        public string SqlGetByAnioVersion
        {
            get { return base.GetSqlXml("GetByAnioVersion"); }
        }

        public string SqlGetByAnioVersionByMes
        {
            get { return base.GetSqlXml("GetByAnioVersionByMes"); }
        }

        public string SqllistaParametroPrimaRerByAnio
        {
            get { return base.GetSqlXml("listaParametroPrimaRerByAnio"); }
        }
    }
}