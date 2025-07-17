using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_DESPACHOURS
    /// </summary>
    public class VcrDespachoursHelper : HelperBase
    {
        public VcrDespachoursHelper(): base(Consultas.VcrDespachoursSql)
        {
        }

        public VcrDespachoursDTO Create(IDataReader dr)
        {
            VcrDespachoursDTO entity = new VcrDespachoursDTO();

            int iVcdurscodi = dr.GetOrdinal(this.Vcdurscodi);
            if (!dr.IsDBNull(iVcdurscodi)) entity.Vcdurscodi = Convert.ToInt32(dr.GetValue(iVcdurscodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iVcdurstipo = dr.GetOrdinal(this.Vcdurstipo);
            if (!dr.IsDBNull(iVcdurstipo)) entity.Vcdurstipo = dr.GetString(iVcdurstipo);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iVcdursfecha = dr.GetOrdinal(this.Vcdursfecha);
            if (!dr.IsDBNull(iVcdursfecha)) entity.Vcdursfecha = dr.GetDateTime(iVcdursfecha);

            int iVcdursdespacho = dr.GetOrdinal(this.Vcdursdespacho);
            if (!dr.IsDBNull(iVcdursdespacho)) entity.Vcdursdespacho = dr.GetDecimal(iVcdursdespacho);

            int iVcdursusucreacion = dr.GetOrdinal(this.Vcdursusucreacion);
            if (!dr.IsDBNull(iVcdursusucreacion)) entity.Vcdursusucreacion = dr.GetString(iVcdursusucreacion);

            int iVcdursfeccreacion = dr.GetOrdinal(this.Vcdursfeccreacion);
            if (!dr.IsDBNull(iVcdursfeccreacion)) entity.Vcdursfeccreacion = dr.GetDateTime(iVcdursfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcdurscodi = "VCDURSCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Vcdurstipo = "VCDURSTIPO";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Vcdursfecha = "VCDURSFECHA";
        public string Vcdursdespacho = "VCDURSDESPACHO";
        public string Vcdursusucreacion = "VCDURSUSUCREACION";
        public string Vcdursfeccreacion = "VCDURSFECCREACION";

        //Atributo para consultas
        public string Equinomb = "EQUINOMB";
        
        #endregion

        //Metodos adicionales
        public string SqlListUnidadByUrsTipo
        {
            get { return base.GetSqlXml("ListUnidadByUrsTipo"); }
        }

        public string SqlListByUrsUnidadTipoDia
        {
            get { return base.GetSqlXml("ListByUrsUnidadTipoDia"); }
        }

        public string SqlGetByRangeDatetime
        {
            get { return base.GetSqlXml("GetByRangeDatetime"); }
        }
    }
}
