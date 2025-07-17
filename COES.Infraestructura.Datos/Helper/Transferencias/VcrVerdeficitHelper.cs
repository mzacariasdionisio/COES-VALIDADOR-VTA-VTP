using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERDEFICIT
    /// </summary>
    public class VcrVerdeficitHelper : HelperBase
    {
        public VcrVerdeficitHelper(): base(Consultas.VcrVerdeficitSql)
        {
        }

        public VcrVerdeficitDTO Create(IDataReader dr)
        {
            VcrVerdeficitDTO entity = new VcrVerdeficitDTO();

            int iVcrvdecodi = dr.GetOrdinal(this.Vcrvdecodi);
            if (!dr.IsDBNull(iVcrvdecodi)) entity.Vcrvdecodi = Convert.ToInt32(dr.GetValue(iVcrvdecodi));

            int iVcrdsrcodi = dr.GetOrdinal(this.Vcrdsrcodi);
            if (!dr.IsDBNull(iVcrdsrcodi)) entity.Vcrdsrcodi = Convert.ToInt32(dr.GetValue(iVcrdsrcodi));

            int iVcrvdefecha = dr.GetOrdinal(this.Vcrvdefecha);
            if (!dr.IsDBNull(iVcrvdefecha)) entity.Vcrvdefecha = dr.GetDateTime(iVcrvdefecha);

            int iVcrvdehorinicio = dr.GetOrdinal(this.Vcrvdehorinicio);
            if (!dr.IsDBNull(iVcrvdehorinicio)) entity.Vcrvdehorinicio = dr.GetDateTime(iVcrvdehorinicio);

            int iVcrvdehorfinal = dr.GetOrdinal(this.Vcrvdehorfinal);
            if (!dr.IsDBNull(iVcrvdehorfinal)) entity.Vcrvdehorfinal = dr.GetDateTime(iVcrvdehorfinal);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrvdedeficit = dr.GetOrdinal(this.Vcrvdedeficit);
            if (!dr.IsDBNull(iVcrvdedeficit)) entity.Vcrvdedeficit = dr.GetDecimal(iVcrvdedeficit);

            int iVcrvdeusucreacion = dr.GetOrdinal(this.Vcrvdeusucreacion);
            if (!dr.IsDBNull(iVcrvdeusucreacion)) entity.Vcrvdeusucreacion = dr.GetString(iVcrvdeusucreacion);

            int iVcrvdefeccreacion = dr.GetOrdinal(this.Vcrvdefeccreacion);
            if (!dr.IsDBNull(iVcrvdefeccreacion)) entity.Vcrvdefeccreacion = dr.GetDateTime(iVcrvdefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrvdecodi = "VCRVDECODI";
        public string Vcrdsrcodi = "VCRDSRCODI";
        public string Vcrvdefecha = "VCRVDEFECHA";
        public string Vcrvdehorinicio = "VCRVDEHORINICIO";
        public string Vcrvdehorfinal = "VCRVDEHORFINAL";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrvdedeficit = "VCRVDEDEFICIT";
        public string Vcrvdeusucreacion = "VCRVDEUSUCREACION";
        public string Vcrvdefeccreacion = "VCRVDEFECCREACION";
        //atributos adicionales
        public string EmprNombre = "EMPRNOMBRE";

        #endregion

        //Metodos de la clase
        public string SqlListDia
        {
            get { return base.GetSqlXml("ListDia"); }
        }

        public string SqlListDiaHFHI
        {
            get { return base.GetSqlXml("ListDiaHFHI"); }
        }
    }
}
