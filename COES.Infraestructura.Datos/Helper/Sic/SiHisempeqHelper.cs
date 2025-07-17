using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPEQ
    /// </summary>
    public class SiHisempeqHelper : HelperBase
    {
        public SiHisempeqHelper() : base(Consultas.SiHisempeqSql)
        {
        }

        public SiHisempeqDTO Create(IDataReader dr)
        {
            SiHisempeqDTO entity = new SiHisempeqDTO();

            int iHempeqcodi = dr.GetOrdinal(this.Hempeqcodi);
            if (!dr.IsDBNull(iHempeqcodi)) entity.Hempeqcodi = Convert.ToInt32(dr.GetValue(iHempeqcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iHempeqfecha = dr.GetOrdinal(this.Hempeqfecha);
            if (!dr.IsDBNull(iHempeqfecha)) entity.Hempeqfecha = dr.GetDateTime(iHempeqfecha);

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iEquicodiold = dr.GetOrdinal(this.Equicodiold);
            if (!dr.IsDBNull(iEquicodiold)) entity.Equicodiold = Convert.ToInt32(dr.GetValue(iEquicodiold));

            int iHempeqestado = dr.GetOrdinal(this.Hempeqestado);
            if (!dr.IsDBNull(iHempeqestado)) entity.Hempeqestado = dr.GetString(iHempeqestado);

            int iHempeqdeleted = dr.GetOrdinal(this.Hempeqdeleted);
            if (!dr.IsDBNull(iHempeqdeleted)) entity.Hempeqdeleted = Convert.ToInt32(dr.GetValue(iHempeqdeleted));

            int iOperadoremprcodi = dr.GetOrdinal(this.Operadoremprcodi);
            if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

            return entity;
        }

        #region Mapeo de Campos
        public string Hempeqcodi = "HEMPEQCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Hempeqfecha = "HEMPEQFECHA";
        public string Migracodi = "MIGRACODI";
        public string Equicodiold = "EQUICODIOLD";
        public string Hempeqestado = "HEMPEQESTADO";
        public string Hempeqdeleted = "HEMPEQDELETED";
        public string Operadoremprcodi = "OPERADOREMPRCODI";

        public string Equinomb = "EQUINOMB";
        #endregion

        public string SqlDeleteLogico
        {
            get { return base.GetSqlXml("DeleteLogico"); }
        }

        //querys anular trasferencia
        public string SqlupdateAnular
        {
            get { return base.GetSqlXml("UpdateAnular"); }
        }

        public string SqlListEquiposXMigracion
        {
            get { return base.GetSqlXml("ListEquiposXMigracion"); }
        }

        public string SqlConsultarEquipMigracion
        {
            get { return base.GetSqlXml("ConsultarEquipMigracion"); }
        }


    }
}