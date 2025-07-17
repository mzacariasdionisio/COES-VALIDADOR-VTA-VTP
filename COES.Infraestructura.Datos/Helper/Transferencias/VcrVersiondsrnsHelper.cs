using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERSIONDSRNS
    /// </summary>
    public class VcrVersiondsrnsHelper : HelperBase
    {
        public VcrVersiondsrnsHelper(): base(Consultas.VcrVersiondsrnsSql)
        {
        }

        public VcrVersiondsrnsDTO Create(IDataReader dr)
        {
            VcrVersiondsrnsDTO entity = new VcrVersiondsrnsDTO();

            int iVcrdsrcodi = dr.GetOrdinal(this.Vcrdsrcodi);
            if (!dr.IsDBNull(iVcrdsrcodi)) entity.Vcrdsrcodi = Convert.ToInt32(dr.GetValue(iVcrdsrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iVcrdsrnombre = dr.GetOrdinal(this.Vcrdsrnombre);
            if (!dr.IsDBNull(iVcrdsrnombre)) entity.Vcrdsrnombre = dr.GetString(iVcrdsrnombre);

            int iVcrdsrestado = dr.GetOrdinal(this.Vcrdsrestado);
            if (!dr.IsDBNull(iVcrdsrestado)) entity.Vcrdsrestado = dr.GetString(iVcrdsrestado);

            int iVcrdsrusucreacion = dr.GetOrdinal(this.Vcrdsrusucreacion);
            if (!dr.IsDBNull(iVcrdsrusucreacion)) entity.Vcrdsrusucreacion = dr.GetString(iVcrdsrusucreacion);

            int iVcrdsrfeccreacion = dr.GetOrdinal(this.Vcrdsrfeccreacion);
            if (!dr.IsDBNull(iVcrdsrfeccreacion)) entity.Vcrdsrfeccreacion = dr.GetDateTime(iVcrdsrfeccreacion);

            int iVcrdsrusumodificacion = dr.GetOrdinal(this.Vcrdsrusumodificacion);
            if (!dr.IsDBNull(iVcrdsrusumodificacion)) entity.Vcrdsrusumodificacion = dr.GetString(iVcrdsrusumodificacion);

            int iVcrdsrfecmodificacion = dr.GetOrdinal(this.Vcrdsrfecmodificacion);
            if (!dr.IsDBNull(iVcrdsrfecmodificacion)) entity.Vcrdsrfecmodificacion = dr.GetDateTime(iVcrdsrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrdsrcodi = "VCRDSRCODI";
        public string Pericodi = "PERICODI";
        public string Vcrdsrnombre = "VCRDSRNOMBRE";
        public string Vcrdsrestado = "VCRDSRESTADO";
        public string Vcrdsrusucreacion = "VCRDSRUSUCREACION";
        public string Vcrdsrfeccreacion = "VCRDSRFECCREACION";
        public string Vcrdsrusumodificacion = "VCRDSRUSUMODIFICACION";
        public string Vcrdsrfecmodificacion = "VCRDSRFECMODIFICACION";
        
        //datos adicionales
        public string Perinombre = "PERINOMBRE";

        #endregion

        public string SqlListIndex
        {
            get { return base.GetSqlXml("ListIndex"); }
        }

        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlGetByIdEdit
        {
            get { return base.GetSqlXml("GetByIdEdit"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlGetByIdPeriodo
        {
            get { return base.GetSqlXml("GetByIdPeriodo"); }
        }
    }
}
