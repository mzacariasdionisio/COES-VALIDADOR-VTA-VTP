using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_BARRA_RELACION
    /// </summary>
    public class CmBarraRelacionHelper : HelperBase
    {
        public CmBarraRelacionHelper(): base(Consultas.CmBarraRelacionSql)
        {
        }

        public CmBarraRelacionDTO Create(IDataReader dr)
        {
            CmBarraRelacionDTO entity = new CmBarraRelacionDTO();

            int iCmbarecodi = dr.GetOrdinal(this.Cmbarecodi);
            if (!dr.IsDBNull(iCmbarecodi)) entity.Cmbarecodi = Convert.ToInt32(dr.GetValue(iCmbarecodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iCmbaretipreg = dr.GetOrdinal(this.Cmbaretipreg);
            if (!dr.IsDBNull(iCmbaretipreg)) entity.Cmbaretipreg = dr.GetString(iCmbaretipreg);

            int iBarrcodi2 = dr.GetOrdinal(this.Barrcodi2);
            if (!dr.IsDBNull(iBarrcodi2)) entity.Barrcodi2 = Convert.ToInt32(dr.GetValue(iBarrcodi2));

            int iCmbaretiprel = dr.GetOrdinal(this.Cmbaretiprel);
            if (!dr.IsDBNull(iCmbaretiprel)) entity.Cmbaretiprel = dr.GetString(iCmbaretiprel);

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iCmbarevigencia = dr.GetOrdinal(this.Cmbarevigencia);
            if (!dr.IsDBNull(iCmbarevigencia)) entity.Cmbarevigencia = dr.GetDateTime(iCmbarevigencia);

            int iCmbareexpira = dr.GetOrdinal(this.Cmbareexpira);
            if (!dr.IsDBNull(iCmbareexpira)) entity.Cmbareexpira = dr.GetDateTime(iCmbareexpira);

            int iCmbareestado = dr.GetOrdinal(this.Cmbareestado);
            if (!dr.IsDBNull(iCmbareestado)) entity.Cmbareestado = dr.GetString(iCmbareestado);

            int iCmbareusucreacion = dr.GetOrdinal(this.Cmbareusucreacion);
            if (!dr.IsDBNull(iCmbareusucreacion)) entity.Cmbareusucreacion = dr.GetString(iCmbareusucreacion);

            int iCmbarefeccreacion = dr.GetOrdinal(this.Cmbarefeccreacion);
            if (!dr.IsDBNull(iCmbarefeccreacion)) entity.Cmbarefeccreacion = dr.GetDateTime(iCmbarefeccreacion);

            int iCmbareusumodificacion = dr.GetOrdinal(this.Cmbareusumodificacion);
            if (!dr.IsDBNull(iCmbareusumodificacion)) entity.Cmbareusumodificacion = dr.GetString(iCmbareusumodificacion);

            int iCmbarefecmodificacion = dr.GetOrdinal(this.Cmbarefecmodificacion);
            if (!dr.IsDBNull(iCmbarefecmodificacion)) entity.Cmbarefecmodificacion = dr.GetDateTime(iCmbarefecmodificacion);

            int iCnfbarcodi2 = dr.GetOrdinal(this.Cnfbarcodi2);
            if (!dr.IsDBNull(iCnfbarcodi2)) entity.Cnfbarcodi2 = Convert.ToInt32(dr.GetValue(iCnfbarcodi2));

            #region Ticket_6245           
            int iCmbarereporte = dr.GetOrdinal(this.Cmbarereporte);
            if (!dr.IsDBNull(iCmbarereporte)) entity.Cmbarereporte = dr.GetString(iCmbarereporte);
            #endregion

            return entity;
        }


        #region Mapeo de Campos

        public string Cmbarecodi = "CMBARECODI";
        public string Barrcodi = "BARRCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Cmbaretipreg = "CMBARETIPREG";
        public string Barrcodi2 = "BARRCODI2";
        public string Cmbaretiprel = "CMBARETIPREL";
        public string Configcodi = "CONFIGCODI";
        public string Cmbarevigencia = "CMBAREVIGENCIA";
        public string Cmbareexpira = "CMBAREEXPIRA";
        public string Cmbareestado = "CMBAREESTADO";
        public string Cmbareusucreacion = "CMBAREUSUCREACION";
        public string Cmbarefeccreacion = "CMBAREFECCREACION";
        public string Cmbareusumodificacion = "CMBAREUSUMODIFICACION";
        public string Cmbarefecmodificacion = "CMBAREFECMODIFICACION";
        public string Barrnomb = "BARRNOMBRE";
        public string TipoRelacion = "TIPORELACION";
        public string Cnfbarnombre = "CNFBARNOMBRE";
        public string Barrnomb2 = "BARRNOMBRE2";
        public string Equinomb = "EQUINOMB";
        public string Cnfbarcodi2 = "CNFBARCODI2";

        #region Ticket_6245
        public string Cmbarereporte = "CMBAREREPORTE";
        #endregion

        #endregion

        public string SqlObtenerPorBarra
        {
            get { return base.GetSqlXml("ObtenerPorBarra"); }    
        }

        public string SqlObtenerHistorico
        {
            get { return base.GetSqlXml("ObtenerHistorico"); }
        }
    }
}
