using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_EVENTO_REQ
    /// </summary>
    public class FtExtEventoReqHelper : HelperBase
    {
        public FtExtEventoReqHelper() : base(Consultas.FtExtEventoReqSql)
        {
        }

        public FtExtEventoReqDTO Create(IDataReader dr)
        {
            FtExtEventoReqDTO entity = new FtExtEventoReqDTO();

            int iFtevcodi = dr.GetOrdinal(this.Ftevcodi);
            if (!dr.IsDBNull(iFtevcodi)) entity.Ftevcodi = Convert.ToInt32(dr.GetValue(iFtevcodi));

            int iFevrqcodi = dr.GetOrdinal(this.Fevrqcodi);
            if (!dr.IsDBNull(iFevrqcodi)) entity.Fevrqcodi = Convert.ToInt32(dr.GetValue(iFevrqcodi));

            int iFevrqliteral = dr.GetOrdinal(this.Fevrqliteral);
            if (!dr.IsDBNull(iFevrqliteral)) entity.Fevrqliteral = dr.GetString(iFevrqliteral);

            int iFevrqdesc = dr.GetOrdinal(this.Fevrqdesc);
            if (!dr.IsDBNull(iFevrqdesc)) entity.Fevrqdesc = dr.GetString(iFevrqdesc);

            int iFevrqflaghidro = dr.GetOrdinal(this.Fevrqflaghidro);
            if (!dr.IsDBNull(iFevrqflaghidro)) entity.Fevrqflaghidro = dr.GetString(iFevrqflaghidro);

            int iFevrqflagtermo = dr.GetOrdinal(this.Fevrqflagtermo);
            if (!dr.IsDBNull(iFevrqflagtermo)) entity.Fevrqflagtermo = dr.GetString(iFevrqflagtermo);

            int iFevrqflagsolar = dr.GetOrdinal(this.Fevrqflagsolar);
            if (!dr.IsDBNull(iFevrqflagsolar)) entity.Fevrqflagsolar = dr.GetString(iFevrqflagsolar);

            int iFevrqflageolico = dr.GetOrdinal(this.Fevrqflageolico);
            if (!dr.IsDBNull(iFevrqflageolico)) entity.Fevrqflageolico = dr.GetString(iFevrqflageolico);

            int iFevrqestado = dr.GetOrdinal(this.Fevrqestado);
            if (!dr.IsDBNull(iFevrqestado)) entity.Fevrqestado = dr.GetString(iFevrqestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftevcodi = "FTEVCODI";
        public string Fevrqcodi = "FEVRQCODI";
        public string Fevrqliteral = "FEVRQLITERAL";
        public string Fevrqdesc = "FEVRQDESC";
        public string Fevrqflaghidro = "FEVRQFLAGHIDRO";
        public string Fevrqflagtermo = "FEVRQFLAGTERMO";
        public string Fevrqflagsolar = "FEVRQFLAGSOLAR";
        public string Fevrqflageolico = "FEVRQFLAGEOLICO";
        public string Fevrqestado = "FEVRQESTADO";

        #endregion
    }
}
