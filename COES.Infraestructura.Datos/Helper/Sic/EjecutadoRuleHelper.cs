using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EjecutadoRuleHelper : HelperBase
    {
        public EjecutadoRuleHelper(): base(Consultas.PerfilScadaSql)
        {

        }

        public EjecutadoRuleDTO Create(IDataReader dr)
        {
            EjecutadoRuleDTO entity = new EjecutadoRuleDTO();

            int iEjruCodi = dr.GetOrdinal(this.EJRUCODI);
            if (!dr.IsDBNull(iEjruCodi)) entity.EJRUCODI = Convert.ToInt16(dr.GetValue(iEjruCodi));

            int iEjruAbrev = dr.GetOrdinal(this.EJRUABREV);
            if (!dr.IsDBNull(iEjruAbrev)) entity.EJRUABREV = dr.GetString(iEjruAbrev);

            int iEjruDetalle = dr.GetOrdinal(this.EJRUDETALLE);
            if (!dr.IsDBNull(iEjruDetalle)) entity.EJRUDETALLE = dr.GetString(iEjruDetalle);

            int iEjruFormula = dr.GetOrdinal(this.EJRUFORMULA);
            if (!dr.IsDBNull(iEjruFormula)) entity.EJRUFORMULA = dr.GetString(iEjruFormula);

            int iEjruActiva = dr.GetOrdinal(this.EJRUACTIVA);
            if (!dr.IsDBNull(iEjruActiva)) entity.EJRUACTIVA = dr.GetString(iEjruActiva);

            int iEjruLastUser = dr.GetOrdinal(this.EJRULASTUSER);
            if (!dr.IsDBNull(iEjruLastUser)) entity.EJRULASTUSER = dr.GetString(iEjruLastUser);

            int iEjruLastDate = dr.GetOrdinal(this.EJRULASTDATE);
            if (!dr.IsDBNull(iEjruLastDate)) entity.EJRULASTDATE = dr.GetDateTime(iEjruLastDate);

            return entity;
        }
        
        public string EJRUCODI = "EJRUCODI";
        public string EJRUABREV = "EJRUABREV";
        public string EJRUDETALLE = "EJRUDETALLE";
        public string EJRUFORMULA = "EJRUFORMULA";
        public string EJRUACTIVA = "EJRUACTIVA";
        public string EJRULASTUSER = "EJRULASTUSER";
        public string EJRULASTDATE = "EJRULASTDATE";
    }
}

