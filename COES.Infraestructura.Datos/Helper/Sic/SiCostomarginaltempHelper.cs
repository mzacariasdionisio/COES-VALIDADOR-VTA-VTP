using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class SiCostomarginaltempHelper : HelperBase
    {
        public SiCostomarginaltempHelper(): base(Consultas.SiCostomarginaltempSql)
        {

        }

        public SiCostomarginaltempDTO Create(IDataReader dr)
        {
            SiCostomarginaltempDTO entity = new SiCostomarginaltempDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCmgtenergia = dr.GetOrdinal(this.Cmgtenergia);
            if (!dr.IsDBNull(iCmgtenergia)) entity.Cmgtenergia = dr.GetDecimal(iCmgtenergia);

            int iCmgtcongestion = dr.GetOrdinal(this.Cmgtcongestion);
            if (!dr.IsDBNull(iCmgtcongestion)) entity.Cmgtcongestion = dr.GetDecimal(iCmgtcongestion);

            int iCmgttotal = dr.GetOrdinal(this.Cmgttotal);
            if (!dr.IsDBNull(iCmgttotal)) entity.Cmgttotal = dr.GetDecimal(iCmgttotal);

            int iCmgtcorrelativo = dr.GetOrdinal(this.Cmgtcorrelativo);
            if (!dr.IsDBNull(iCmgtcorrelativo)) entity.Cmgtcorrelativo = Convert.ToInt32(dr.GetValue(iCmgtcorrelativo));

            int iCmgtfecha = dr.GetOrdinal(this.Cmgtfecha);
            if (!dr.IsDBNull(iCmgtfecha)) entity.Cmgtfecha = dr.GetDateTime(iCmgtfecha);

            return entity;
        }

        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Barrcodi = "BARRCODI";
        public string Cmgtenergia = "CMGTENERGIA";
        public string Cmgtcongestion = "CMGTCONGESTION";
        public string Cmgttotal = "CMGTTOTAL";
        public string Cmgtcorrelativo = "CMGTCORRELATIVO";
        public string Cmgtfecha = "CMGTFECHA";
        public string TableName = "SI_COSTOMARGINALTEMP";
        #endregion
    }
}
