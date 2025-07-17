using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERSIONINCPL
    /// </summary>
    public class VcrVersionincplHelper : HelperBase
    {
        public VcrVersionincplHelper(): base(Consultas.VcrVersionincplSql)
        {
        }

        public VcrVersionincplDTO Create(IDataReader dr)
        {
            VcrVersionincplDTO entity = new VcrVersionincplDTO();

            int iVcrinccodi = dr.GetOrdinal(this.Vcrinccodi);
            if (!dr.IsDBNull(iVcrinccodi)) entity.Vcrinccodi = Convert.ToInt32(dr.GetValue(iVcrinccodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iVcrincnombre = dr.GetOrdinal(this.Vcrincnombre);
            if (!dr.IsDBNull(iVcrincnombre)) entity.Vcrincnombre = dr.GetString(iVcrincnombre);

            int iVcrincestado = dr.GetOrdinal(this.Vcrincestado);
            if (!dr.IsDBNull(iVcrincestado)) entity.Vcrincestado = dr.GetString(iVcrincestado);

            int iVcrincusucreacion = dr.GetOrdinal(this.Vcrincusucreacion);
            if (!dr.IsDBNull(iVcrincusucreacion)) entity.Vcrincusucreacion = dr.GetString(iVcrincusucreacion);

            int iVcrincfeccreacion = dr.GetOrdinal(this.Vcrincfeccreacion);
            if (!dr.IsDBNull(iVcrincfeccreacion)) entity.Vcrincfeccreacion = dr.GetDateTime(iVcrincfeccreacion);

            int iVcrincusumodificacion = dr.GetOrdinal(this.Vcrincusumodificacion);
            if (!dr.IsDBNull(iVcrincusumodificacion)) entity.Vcrincusumodificacion = dr.GetString(iVcrincusumodificacion);

            int iVcrincfecmodificacion = dr.GetOrdinal(this.Vcrincfecmodificacion);
            if (!dr.IsDBNull(iVcrincfecmodificacion)) entity.Vcrincfecmodificacion = dr.GetDateTime(iVcrincfecmodificacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Vcrinccodi = "VCRINCCODI";
        public string Pericodi = "PERICODI";
        public string Vcrincnombre = "VCRINCNOMBRE";
        public string Vcrincestado = "VCRINCESTADO";
        public string Vcrincusucreacion = "VCRINCUSUCREACION";
        public string Vcrincfeccreacion = "VCRINCFECCREACION";
        public string Vcrincusumodificacion = "VCRINCUSUMODIFICACION";
        public string Vcrincfecmodificacion = "VCRINCFECMODIFICACION";

        //datos adicionales
        public string Perinombre = "PERINOMBRE";
        #endregion

        //agregado para hacer el combo
        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlListIndex
        {
            get { return base.GetSqlXml("ListIndex"); }
        }

        public string SqlGetByIdEdit
        {
            get { return base.GetSqlXml("GetByIdEdit"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }
    }
}
