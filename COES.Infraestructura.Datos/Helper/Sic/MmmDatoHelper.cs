using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_FACTABLE
    /// </summary>
    public class MmmDatoHelper : HelperBase
    {
        public MmmDatoHelper()
            : base(Consultas.MmmDatoSql)
        {
        }

        public MmmDatoDTO Create(IDataReader dr)
        {
            MmmDatoDTO entity = new MmmDatoDTO();

            int iMmmdatocodi = dr.GetOrdinal(this.Mmmdatcodi);
            if (!dr.IsDBNull(iMmmdatocodi)) entity.Mmmdatcodi = Convert.ToInt32(dr.GetValue(iMmmdatocodi));

            int iMmmdatofecha = dr.GetOrdinal(this.Mmmdatfecha);
            if (!dr.IsDBNull(iMmmdatofecha)) entity.Mmmdatfecha = dr.GetDateTime(iMmmdatofecha);

            int iMmmdatohoraindex = dr.GetOrdinal(this.Mmmdathoraindex);
            if (!dr.IsDBNull(iMmmdatohoraindex)) entity.Mmmdathoraindex = Convert.ToInt32(dr.GetValue(iMmmdatohoraindex));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iMogrupocodi = dr.GetOrdinal(this.Mogrupocodi);
            if (!dr.IsDBNull(iMogrupocodi)) entity.Mogrupocodi = Convert.ToInt32(dr.GetValue(iMogrupocodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iMmmdatomwejec = dr.GetOrdinal(this.Mmmdatmwejec);
            if (!dr.IsDBNull(iMmmdatomwejec)) entity.Mmmdatmwejec = dr.GetDecimal(iMmmdatomwejec);

            int iMmmdatocmgejec = dr.GetOrdinal(this.Mmmdatcmgejec);
            if (!dr.IsDBNull(iMmmdatocmgejec)) entity.Mmmdatcmgejec = dr.GetDecimal(iMmmdatocmgejec);

            int iMmmdatomwprog = dr.GetOrdinal(this.Mmmdatmwprog);
            if (!dr.IsDBNull(iMmmdatomwprog)) entity.Mmmdatmwprog = dr.GetDecimal(iMmmdatomwprog);

            int iMmmdatocmgprog = dr.GetOrdinal(this.Mmmdatcmgprog);
            if (!dr.IsDBNull(iMmmdatocmgprog)) entity.Mmmdatcmgprog = dr.GetDecimal(iMmmdatocmgprog);

            int iCvar = dr.GetOrdinal(this.Mmmdatocvar);
            if (!dr.IsDBNull(iCvar)) entity.Mmmdatocvar = dr.GetDecimal(iCvar);

            return entity;
        }


        #region Mapeo de Campos

        public string Mmmdatcodi = "Mmmdatcodi";
        public string Mmmdatfecha = "Mmmdatfecha";
        public string Mmmdathoraindex = "Mmmdathoraindex";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Mogrupocodi = "MOGRUPOCODI";
        public string Barrcodi = "BARRCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Mmmdatmwejec = "Mmmdatmwejec";
        public string Mmmdatcmgejec = "Mmmdatcmgejec";
        public string Mmmdatmwprog = "Mmmdatmwprog";
        public string Mmmdatcmgprog = "Mmmdatcmgprog";
        public string Mmmdatocvar = "Mmmdatcvar";

        public string Catecodi = "CATECODI";
        public string Grupopadre = "GRUPOPADRE";
        public string Gruponomb = "GRUPONOMB";
        public string Barrnombre = "BARRNOMBRE";

        #endregion

        #region Mapeo de Consultas

        public string SqlListPeriodo
        {
            get { return GetSqlXml("ListPeriodo"); }
        }
        #endregion

    }
}
