using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERRNS
    /// </summary>
    public class VcrVerrnsHelper : HelperBase
    {
        public VcrVerrnsHelper(): base(Consultas.VcrVerrnsSql)
        {
        }

        public VcrVerrnsDTO Create(IDataReader dr)
        {
            VcrVerrnsDTO entity = new VcrVerrnsDTO();

            int iVcvrnscodi = dr.GetOrdinal(this.Vcvrnscodi);
            if (!dr.IsDBNull(iVcvrnscodi)) entity.Vcvrnscodi = Convert.ToInt32(dr.GetValue(iVcvrnscodi));

            int iVcrdsrcodi = dr.GetOrdinal(this.Vcrdsrcodi);
            if (!dr.IsDBNull(iVcrdsrcodi)) entity.Vcrdsrcodi = Convert.ToInt32(dr.GetValue(iVcrdsrcodi));

            int iVcvrnsfecha = dr.GetOrdinal(this.Vcvrnsfecha);
            if (!dr.IsDBNull(iVcvrnsfecha)) entity.Vcvrnsfecha = dr.GetDateTime(iVcvrnsfecha);

            int iVcvrnshorinicio = dr.GetOrdinal(this.Vcvrnshorinicio);
            if (!dr.IsDBNull(iVcvrnshorinicio)) entity.Vcvrnshorinicio = dr.GetDateTime(iVcvrnshorinicio);

            int iVcvrnshorfinal = dr.GetOrdinal(this.Vcvrnshorfinal);
            if (!dr.IsDBNull(iVcvrnshorfinal)) entity.Vcvrnshorfinal = dr.GetDateTime(iVcvrnshorfinal);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcvrnsrns = dr.GetOrdinal(this.Vcvrnsrns);
            if (!dr.IsDBNull(iVcvrnsrns)) entity.Vcvrnsrns = dr.GetDecimal(iVcvrnsrns);

            int iVcvrnsusucreacion = dr.GetOrdinal(this.Vcvrnsusucreacion);
            if (!dr.IsDBNull(iVcvrnsusucreacion)) entity.Vcvrnsusucreacion = dr.GetString(iVcvrnsusucreacion);

            int iVcvrnsfeccreacion = dr.GetOrdinal(this.Vcvrnsfeccreacion);
            if (!dr.IsDBNull(iVcvrnsfeccreacion)) entity.Vcvrnsfeccreacion = dr.GetDateTime(iVcvrnsfeccreacion);

            int iVcvrnstipocarga = dr.GetOrdinal(this.Vcvrnstipocarga);
            if (!dr.IsDBNull(iVcvrnstipocarga)) entity.Vcvrnstipocarga = Convert.ToInt32(dr.GetValue(iVcvrnstipocarga));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcvrnscodi = "VCVRNSCODI";
        public string Vcrdsrcodi = "VCRDSRCODI";
        public string Vcvrnsfecha = "VCVRNSFECHA";
        public string Vcvrnshorinicio = "VCVRNSHORINICIO";
        public string Vcvrnshorfinal = "VCVRNSHORFINAL";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcvrnsrns = "VCVRNSRNS";
        public string Vcvrnsusucreacion = "VCVRNSUSUCREACION";
        public string Vcvrnsfeccreacion = "VCVRNSFECCREACION";
        public string Vcvrnstipocarga = "VCVRNSTIPOCARGA";
        //atributos adicionales
        public string EmprNombre = "EMPRNOMBRE";

        #endregion

        //Metodos de la clase
        public string SqlListDia
        {
            get { return base.GetSqlXml("ListDia"); }
        }
    }
}
