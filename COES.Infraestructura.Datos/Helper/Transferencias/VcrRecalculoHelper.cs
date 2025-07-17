using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_RECALCULO
    /// </summary>
    public class VcrRecalculoHelper : HelperBase
    {
        public VcrRecalculoHelper(): base(Consultas.VcrRecalculoSql)
        {
        }

        public VcrRecalculoDTO Create(IDataReader dr)
        {
            VcrRecalculoDTO entity = new VcrRecalculoDTO();

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iVcrecanombre = dr.GetOrdinal(this.Vcrecanombre);
            if (!dr.IsDBNull(iVcrecanombre)) entity.Vcrecanombre = dr.GetString(iVcrecanombre);

            int iVcrecaversion = dr.GetOrdinal(this.Vcrecaversion);
            if (!dr.IsDBNull(iVcrecaversion)) entity.Vcrecaversion = Convert.ToInt32(dr.GetValue(iVcrecaversion));

            int iVcrecakcalidad = dr.GetOrdinal(this.Vcrecakcalidad);
            if (!dr.IsDBNull(iVcrecakcalidad)) entity.Vcrecakcalidad = dr.GetDecimal(iVcrecakcalidad);

            int iVcrecapaosinergmin = dr.GetOrdinal(this.Vcrecapaosinergmin);
            if (!dr.IsDBNull(iVcrecapaosinergmin)) entity.Vcrecapaosinergmin = dr.GetDecimal(iVcrecapaosinergmin);

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));

            int iVcrdsrcodi = dr.GetOrdinal(this.Vcrdsrcodi);
            if (!dr.IsDBNull(iVcrdsrcodi)) entity.Vcrdsrcodi = Convert.ToInt32(dr.GetValue(iVcrdsrcodi));

            int iVcrinccodi = dr.GetOrdinal(this.Vcrinccodi);
            if (!dr.IsDBNull(iVcrinccodi)) entity.Vcrinccodi = Convert.ToInt32(dr.GetValue(iVcrinccodi));

            int iVcrecacomentario = dr.GetOrdinal(this.Vcrecacomentario);
            if (!dr.IsDBNull(iVcrecacomentario)) entity.Vcrecacomentario = dr.GetString(iVcrecacomentario);

            int iVcrecaestado = dr.GetOrdinal(this.Vcrecaestado);
            if (!dr.IsDBNull(iVcrecaestado)) entity.Vcrecaestado = dr.GetString(iVcrecaestado);

            int iVcrecacodidestino = dr.GetOrdinal(this.Vcrecacodidestino);
            if (!dr.IsDBNull(iVcrecacodidestino)) entity.Vcrecacodidestino = Convert.ToInt32(dr.GetValue(iVcrecacodidestino));

            int iVcrecausucreacion = dr.GetOrdinal(this.Vcrecausucreacion);
            if (!dr.IsDBNull(iVcrecausucreacion)) entity.Vcrecausucreacion = dr.GetString(iVcrecausucreacion);

            int iVcrecafeccreacion = dr.GetOrdinal(this.Vcrecafeccreacion);
            if (!dr.IsDBNull(iVcrecafeccreacion)) entity.Vcrecafeccreacion = dr.GetDateTime(iVcrecafeccreacion);

            int iVcrecausumodificacion = dr.GetOrdinal(this.Vcrecausumodificacion);
            if (!dr.IsDBNull(iVcrecausumodificacion)) entity.Vcrecausumodificacion = dr.GetString(iVcrecausumodificacion);

            int iVcrecafecmodificacion = dr.GetOrdinal(this.Vcrecafecmodificacion);
            if (!dr.IsDBNull(iVcrecafecmodificacion)) entity.Vcrecafecmodificacion = dr.GetDateTime(iVcrecafecmodificacion);

            //202012
            int iVcrecaresaprimsig = dr.GetOrdinal(this.Vcrecaresaprimsig);
            if (!dr.IsDBNull(iVcrecaresaprimsig)) entity.Vcrecaresaprimsig = dr.GetDecimal(iVcrecaresaprimsig);

            int iVcrecacostoprns = dr.GetOrdinal(this.Vcrecacostoprns);
            if (!dr.IsDBNull(iVcrecacostoprns)) entity.Vcrecacostoprns = dr.GetDecimal(iVcrecacostoprns);

            int iVcrecafactcumpl = dr.GetOrdinal(this.Vcrecafactcumpl);
            if (!dr.IsDBNull(iVcrecafactcumpl)) entity.Vcrecafactcumpl = dr.GetDecimal(iVcrecafactcumpl);
            //--

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrecacodi = "VCRECACODI";
        public string Pericodi = "PERICODI";
        public string Vcrecanombre = "VCRECANOMBRE";
        public string Vcrecaversion = "VCRECAVERSION";
        public string Vcrecakcalidad = "VCRECAKCALIDAD";
        public string Vcrecapaosinergmin = "VCRECAPAOSINERGMIN";
        public string Recacodi = "RECACODI";
        public string Vcrdsrcodi = "VCRDSRCODI";
        public string Vcrinccodi = "VCRINCCODI";
        public string Vcrecacomentario = "VCRECACOMENTARIO";
        public string Vcrecaestado = "VCRECAESTADO";
        public string Vcrecacodidestino = "VCRECACODIDESTINO";
        public string Vcrecausucreacion = "VCRECAUSUCREACION";
        public string Vcrecafeccreacion = "VCRECAFECCREACION";
        public string Vcrecausumodificacion = "VCRECAUSUMODIFICACION";
        public string Vcrecafecmodificacion = "VCRECAFECMODIFICACION";
        //202012
        public string Vcrecaresaprimsig = "VCRECARESAPRIMSIG";
        public string Vcrecacostoprns = "VCRECACOSTOPRNS";
        public string Vcrecafactcumpl = "VCRECAFACTCUMPL";
        //--
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Perinombre = "PERINOMBRE";
        public string Recanombre = "RECANOMBRE";
        public string Perinombredestino = "PERINOMBREDESTINO";
        public string Vcrecanombredestino = "VCRECANOMBREDESTINO";
        public string Vcrdsrnombre = "VCRDSRNOMBRE";
        public string Vcrincnombre = "VCRINCNOMBRE";

        #endregion

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlListAllView
        {
            get { return base.GetSqlXml("ListAllView"); }
        }

        public string SqlGetByIdUpDate
        {
            get { return base.GetSqlXml("GetByIdUpDate"); }
        }

        public string SqlListInsert
        {
            get { return base.GetSqlXml("ListInsert"); }
        }

        public string SqlGetByIdViewIndex
        {
            get { return base.GetSqlXml("GetByIdViewIndex"); }
        }
    }
}
