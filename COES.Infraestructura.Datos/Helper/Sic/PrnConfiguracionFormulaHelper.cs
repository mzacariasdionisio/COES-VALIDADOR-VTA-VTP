using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnConfiguracionFormulaHelper : HelperBase
    {
        public PrnConfiguracionFormulaHelper() : base(Consultas.PrnConfiguracionFormulaSql)
        {
        }

        public PrnConfiguracionFormulaDTO Create(IDataReader dr)
        {
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

            int iCnffrmcodi = dr.GetOrdinal(this.Cnffrmcodi);
            if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPtomedidesc = dr.GetOrdinal(this.Ptomedidesc);
            if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

            int iCnffrmfecha = dr.GetOrdinal(this.Cnffrmfecha);
            if (!dr.IsDBNull(iCnffrmfecha)) entity.Cnffrmfecha = dr.GetDateTime(iCnffrmfecha);

            int iCnffrmferiado = dr.GetOrdinal(this.Cnffrmferiado);
            if (!dr.IsDBNull(iCnffrmferiado)) entity.Cnffrmferiado = dr.GetString(iCnffrmferiado);

            int iCnffrmatipico = dr.GetOrdinal(this.Cnffrmatipico);
            if (!dr.IsDBNull(iCnffrmatipico)) entity.Cnffrmatipico = dr.GetString(iCnffrmatipico);

            int iCnffrmveda = dr.GetOrdinal(this.Cnffrmveda);
            if (!dr.IsDBNull(iCnffrmveda)) entity.Cnffrmveda = dr.GetString(iCnffrmveda);

            int iCnffrmdepauto = dr.GetOrdinal(this.Cnffrmdepauto);
            if (!dr.IsDBNull(iCnffrmdepauto)) entity.Cnffrmdepauto = dr.GetString(iCnffrmdepauto);

            int iCnffrmcargamax = dr.GetOrdinal(this.Cnffrmcargamax);
            if (!dr.IsDBNull(iCnffrmcargamax)) entity.Cnffrmcargamax = dr.GetDecimal(iCnffrmcargamax);

            int iCnffrmcargamin = dr.GetOrdinal(this.Cnffrmcargamin);
            if (!dr.IsDBNull(iCnffrmcargamin)) entity.Cnffrmcargamin = dr.GetDecimal(iCnffrmcargamin);

            int iCnffrmtolerancia = dr.GetOrdinal(this.Cnffrmtolerancia);
            if (!dr.IsDBNull(iCnffrmtolerancia)) entity.Cnffrmtolerancia = dr.GetDecimal(iCnffrmtolerancia);

            int iCnffrmusucreacion = dr.GetOrdinal(this.Cnffrmusucreacion);
            if (!dr.IsDBNull(iCnffrmusucreacion)) entity.Cnffrmusucreacion = dr.GetString(iCnffrmusucreacion);

            int iCnffrmfeccreacion = dr.GetOrdinal(this.Cnffrmfeccreacion);
            if (!dr.IsDBNull(iCnffrmfeccreacion)) entity.Cnffrmfeccreacion = dr.GetDateTime(iCnffrmfeccreacion);

            int iCnffrmusumodificacion = dr.GetOrdinal(this.Cnffrmusumodificacion);
            if (!dr.IsDBNull(iCnffrmusumodificacion)) entity.Cnffrmusumodificacion = dr.GetString(iCnffrmusumodificacion);

            int iCnffrmfecmodificacion = dr.GetOrdinal(this.Cnffrmfecmodificacion);
            if (!dr.IsDBNull(iCnffrmfecmodificacion)) entity.Cnffrmfecmodificacion = dr.GetDateTime(iCnffrmfecmodificacion);

            return entity;
        }

        public string Cnffrmcodi = "CNFFRMCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Cnffrmfecha = "CNFFRMFECHA";
        public string Cnffrmferiado = "CNFFRMFERIADO";
        public string Cnffrmatipico = "CNFFRMATIPicO";
        public string Cnffrmveda = "CNFFRMVEDA";
        public string Cnffrmdepauto = "CNFFRMDEPAUTO";
        public string Cnffrmcargamax = "CNFFRMCARGAMAX";
        public string Cnffrmcargamin = "CNFFRMCARGAMIN";
        public string Cnffrmtolerancia = "CNFFRMTOLERANCIA";
        public string Cnffrmusucreacion = "CNFFRMUSUCREACION";
        public string Cnffrmfeccreacion = "CNFFRMFECCREACION";
        public string Cnffrmusumodificacion = "CNFFRMUSUMODIFICACION";
        public string Cnffrmfecmodificacion = "CNFFRMFECMODIFICACION";
        public string Cnffrmformula = "CNFFRMFORMULA";
        public string Prruabrev = "PRRUABREV";
        public string Prrucodi = "PRRUCODI";
        public string Cnffrmdiapatron = "CNFFRMDIAPATRON";
        public string Cnffrmpatron = "CNFFRMPATRON";
        public string Cnffrmdefecto = "CNFFRMDEFECTO";

        #region Consulta a BD
        public string SqlListConfiguracionFormulaByFiltros
        {
            get { return base.GetSqlXml("ListConfiguracionFormulaByFiltros"); }
        }
        
        public string SqlGetIdByCodigoFecha
        {
            get { return base.GetSqlXml("GetIdByCodigoFecha"); }
        }
        
        public string SqlGetParametrosByIdFecha
        {
            get { return base.GetSqlXml("GetParametrosByIdFecha"); }
        }
        
        public string SqlParametrosFormulasList
        {
            get { return base.GetSqlXml("ParametrosFormulasList"); }
        }
        #endregion
    }
}
