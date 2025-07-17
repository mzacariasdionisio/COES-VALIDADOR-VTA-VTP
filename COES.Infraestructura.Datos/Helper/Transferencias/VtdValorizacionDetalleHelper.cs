using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_VALORIZACIONDETALLE
    /// </summary>
    public class VtdValorizacionDetalleHelper : HelperBase
    {
        public VtdValorizacionDetalleHelper(): base(Consultas.VtdValorizacionDetalleSql)
        {
        }

        public VtdValorizacionDetalleDTO Create(IDataReader dr)
        {
            VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();

            int iValdcodi = dr.GetOrdinal(this.Valdcodi);
            if (!dr.IsDBNull(iValdcodi)) entity.Valdcodi = Convert.ToInt32(dr.GetValue(iValdcodi));

            int iValocodi = dr.GetOrdinal(this.Valocodi);
            if (!dr.IsDBNull(iValocodi)) entity.Valocodi = Convert.ToInt32(dr.GetValue(iValocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValdretiro = dr.GetOrdinal(this.Valdretiro);
            if (!dr.IsDBNull(iValdretiro)) entity.Valdretiro = Convert.ToDecimal(dr.GetValue(iValdretiro));

            int iValdentrega = dr.GetOrdinal(this.Valdentrega);
            if (!dr.IsDBNull(iValdentrega)) entity.Valdentrega = Convert.ToDecimal(dr.GetValue(iValdentrega));

            int iValdpfirremun = dr.GetOrdinal(this.Valdpfirremun);
            if (!dr.IsDBNull(iValdpfirremun)) entity.Valdpfirremun = Convert.ToDecimal(dr.GetValue(iValdpfirremun));

            int iValddemandacoincidente = dr.GetOrdinal(this.Valddemandacoincidente);
            if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = Convert.ToDecimal(dr.GetValue(iValddemandacoincidente));

            int iValdmoncapacidad = dr.GetOrdinal(this.Valdmoncapacidad);
            if (!dr.IsDBNull(iValdmoncapacidad)) entity.Valdmoncapacidad = Convert.ToDecimal(dr.GetValue(iValdmoncapacidad));

            int iValdpeajeuni = dr.GetOrdinal(this.Valdpeajeuni);
            if (!dr.IsDBNull(iValdpeajeuni)) entity.Valdpeajeuni = Convert.ToDecimal(dr.GetValue(iValdpeajeuni));

            int iValdfactorp = dr.GetOrdinal(this.Valdfactorp);
            if (!dr.IsDBNull(iValdfactorp)) entity.Valdfactorp = Convert.ToDecimal(dr.GetValue(iValdfactorp));

            int iValdpagoio = dr.GetOrdinal(this.Valdpagoio);
            if (!dr.IsDBNull(iValdpagoio)) entity.Valdpagoio = Convert.ToDecimal(dr.GetValue(iValdpagoio));

            int iValdpaosc = dr.GetOrdinal(this.Valdpagosc);
            if (!dr.IsDBNull(iValdpaosc)) entity.Valdpagosc = Convert.ToDecimal(dr.GetValue(iValdpaosc));

            int iValdfpgm = dr.GetOrdinal(this.Valdfpgm);
            if (!dr.IsDBNull(iValdfpgm)) entity.Valdfpgm = Convert.ToDecimal(dr.GetValue(iValdfpgm));

            int iValdmcio = dr.GetOrdinal(this.Valdmcio);
            if (!dr.IsDBNull(iValdmcio)) entity.Valdmcio = Convert.ToDecimal(dr.GetValue(iValdmcio));

            int iValdpdsc = dr.GetOrdinal(this.Valdpdsc);
            if (!dr.IsDBNull(iValdpdsc)) entity.Valdpdsc = Convert.ToDecimal(dr.GetValue(iValdpdsc));

            int iValdcargoconsumo = dr.GetOrdinal(this.Valdcargoconsumo);
            if (!dr.IsDBNull(iValdcargoconsumo)) entity.Valdcargoconsumo = Convert.ToDecimal(dr.GetValue(iValdcargoconsumo));

            int iValdportesadicional = dr.GetOrdinal(this.Valdportesadicional);
            if (!dr.IsDBNull(iValdportesadicional)) entity.Valdaportesadicional = Convert.ToDecimal(dr.GetValue(iValdportesadicional));

            int iValdsucreacion = dr.GetOrdinal(this.Valdusucreacion);
            if (!dr.IsDBNull(iValdsucreacion)) entity.Valdusucreacion = dr.GetString(iValdsucreacion);

            int iValdfeccreacion = dr.GetOrdinal(this.Valdfeccreacion);
            if (!dr.IsDBNull(iValdfeccreacion)) entity.Valdfeccreacion =dr.GetDateTime(iValdfeccreacion);

            int iValdusumodificacion = dr.GetOrdinal(this.Valdusumodificacion);
            if (!dr.IsDBNull(iValdusumodificacion)) entity.Valdusumodificacion = dr.GetString(iValdusumodificacion);

            int iValdfecmodificacion = dr.GetOrdinal(this.Valdfecmodificacion);
            if (!dr.IsDBNull(iValdfecmodificacion)) entity.Valdfecmodificacion = dr.GetDateTime(iValdfecmodificacion);
            

            return entity;
        }

        #region Mapeo de Campos

        public string Valdcodi = "VALDCODI";
        public string Valocodi = "VALOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Valdretiro = "VALDRETIRO";
        public string Valdentrega = "VALDENTREGA";
        public string Valdpfirremun = "VALDPFIRREMUN";
        public string Valddemandacoincidente = "VALDDEMANDACOINCIDENTE";
        public string Valdmoncapacidad = "VALDMONCAPACIDAD";
        public string Valdpeajeuni = "VALDPEAJEUNI";
        public string Valdfactorp = "VALDFACTORP";
        public string Valdpagoio = "VALDPAGOIO";
        public string Valdpagosc = "VALDPAGOSC";
        public string Valdfpgm = "VALDFPGM";
        public string Valdmcio = "VALDMCIO";        
        public string Valdpdsc = "VALDPDSC";
        public string Valdcargoconsumo = "VALDCARGOCONSUMO";
        public string Valdportesadicional = "VALDAPORTESADICIONAL";
        public string Valdusucreacion = "VALDUSUCREACION";
        public string Valdfeccreacion = "VALDFECCREACION";
        public string Valdusumodificacion = "VALDUSUMODIFICACION";
        public string Valdfecmodificacion = "VALDFECMODIFICACION";

        #endregion

        public string SqlGetValorizacionDetalleporFechaParticipante
        {
            get { return base.GetSqlXml("GetValorizacionDetalleporFechaParticipante"); }
        }

        public string SqlGetEnergiaPrevistaRetirarTotal
        {
            get { return base.GetSqlXml("GetEnergiaPrevistaRetirarTotal"); }
        }
    }
}
