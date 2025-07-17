using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_REPORTE_DET
    /// </summary>
    public class IndReporteDetHelper : HelperBase
    {
        public IndReporteDetHelper() : base(Consultas.IndReporteDetSql)
        {
        }

        public IndReporteDetDTO Create(IDataReader dr)
        {
            IndReporteDetDTO entity = new IndReporteDetDTO();

            int iIdetcodi = dr.GetOrdinal(this.Idetcodi);
            if (!dr.IsDBNull(iIdetcodi)) entity.Idetcodi = Convert.ToInt32(dr.GetValue(iIdetcodi));

            int iItotcodi = dr.GetOrdinal(this.Itotcodi);
            if (!dr.IsDBNull(iItotcodi)) entity.Itotcodi = Convert.ToInt32(dr.GetValue(iItotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iIdetopcom = dr.GetOrdinal(this.Idetopcom);
            if (!dr.IsDBNull(iIdetopcom)) entity.Idetopcom = dr.GetString(iIdetopcom);

            int iIdetincremental = dr.GetOrdinal(this.Idetincremental);
            if (!dr.IsDBNull(iIdetincremental)) entity.Idetincremental = Convert.ToInt32(dr.GetValue(iIdetincremental));

            int iIdetdia = dr.GetOrdinal(this.Idetdia);
            if (!dr.IsDBNull(iIdetdia)) entity.Idetdia = Convert.ToInt32(dr.GetValue(iIdetdia));

            int iIdettipoindisp = dr.GetOrdinal(this.Idettipoindisp);
            if (!dr.IsDBNull(iIdettipoindisp)) entity.Idettipoindisp = dr.GetString(iIdettipoindisp);

            int iIdettieneexc = dr.GetOrdinal(this.Idettieneexc);
            if (!dr.IsDBNull(iIdettieneexc)) entity.Idettieneexc = dr.GetString(iIdettieneexc);

            int iIdethoraini = dr.GetOrdinal(this.Idethoraini);
            if (!dr.IsDBNull(iIdethoraini)) entity.Idethoraini = dr.GetDateTime(iIdethoraini);

            int iIdethorafin = dr.GetOrdinal(this.Idethorafin);
            if (!dr.IsDBNull(iIdethorafin)) entity.Idethorafin = dr.GetDateTime(iIdethorafin);

            int iIdetmin = dr.GetOrdinal(this.Idetmin);
            if (!dr.IsDBNull(iIdetmin)) entity.Idetmin = Convert.ToInt32(dr.GetValue(iIdetmin));

            int iIdetmw = dr.GetOrdinal(this.Idetmw);
            if (!dr.IsDBNull(iIdetmw)) entity.Idetmw = dr.GetDecimal(iIdetmw);

            int iIdetpr = dr.GetOrdinal(this.Idetpr);
            if (!dr.IsDBNull(iIdetpr)) entity.Idetpr = dr.GetDecimal(iIdetpr);

            int iIdetminparcial = dr.GetOrdinal(this.Idetminparcial);
            if (!dr.IsDBNull(iIdetminparcial)) entity.Idetminparcial = dr.GetDecimal(iIdetminparcial);

            int iIdetminifparcial = dr.GetOrdinal(this.Idetminifparcial);
            if (!dr.IsDBNull(iIdetminifparcial)) entity.Idetminifparcial = dr.GetDecimal(iIdetminifparcial);

            int iIdetminipparcial = dr.GetOrdinal(this.Idetminipparcial);
            if (!dr.IsDBNull(iIdetminipparcial)) entity.Idetminipparcial = dr.GetDecimal(iIdetminipparcial); 

            int iIdettienedisp = dr.GetOrdinal(this.Idettienedisp);
            if (!dr.IsDBNull(iIdettienedisp)) entity.Idettienedisp = Convert.ToInt32(dr.GetValue(iIdettienedisp));

            int iIdetfactork = dr.GetOrdinal(this.Idetfactork);
            if (!dr.IsDBNull(iIdetfactork)) entity.Idetfactork = dr.GetDecimal(iIdetfactork);

            int iIdetpe = dr.GetOrdinal(this.Idetpe);
            if (!dr.IsDBNull(iIdetpe)) entity.Idetpe = dr.GetDecimal(iIdetpe);

            int iIdetpa = dr.GetOrdinal(this.Idetpa);
            if (!dr.IsDBNull(iIdetpa)) entity.Idetpa = dr.GetDecimal(iIdetpa);

            int iIdetminif = dr.GetOrdinal(this.Idetminif);
            if (!dr.IsDBNull(iIdetminif)) entity.Idetminif = dr.GetDecimal(iIdetminif);

            int iIdetminip = dr.GetOrdinal(this.Idetminip);
            if (!dr.IsDBNull(iIdetminip)) entity.Idetminip =dr.GetDecimal(iIdetminip);

            int iIdetnumho = dr.GetOrdinal(this.Idetnumho);
            if (!dr.IsDBNull(iIdetnumho)) entity.Idetnumho =dr.GetDecimal(iIdetnumho);

            int iIdetnumarranq = dr.GetOrdinal(this.Idetnumarranq);
            if (!dr.IsDBNull(iIdetnumarranq)) entity.Idetnumarranq = Convert.ToInt32(dr.GetValue(iIdetnumarranq));

            int iIdetfechainifort7d = dr.GetOrdinal(this.Idetfechainifort7d);
            if (!dr.IsDBNull(iIdetfechainifort7d)) entity.Idetfechainifort7d = dr.GetDateTime(iIdetfechainifort7d);

            int iIdetfechafinfort7d = dr.GetOrdinal(this.Idetfechafinfort7d);
            if (!dr.IsDBNull(iIdetfechafinfort7d)) entity.Idetfechafinfort7d = dr.GetDateTime(iIdetfechafinfort7d);

            int iIdetfechainiprog7d = dr.GetOrdinal(this.Idetfechainiprog7d);
            if (!dr.IsDBNull(iIdetfechainiprog7d)) entity.Idetfechainiprog7d = dr.GetDateTime(iIdetfechainiprog7d);

            int iIdetfechafinprog7d = dr.GetOrdinal(this.Idetfechafinprog7d);
            if (!dr.IsDBNull(iIdetfechafinprog7d)) entity.Idetfechafinprog7d = dr.GetDateTime(iIdetfechafinprog7d);

            int iIdetdescadic = dr.GetOrdinal(this.Idetdescadic);
            if (!dr.IsDBNull(iIdetdescadic)) entity.Idetdescadic = dr.GetString(iIdetdescadic);

            int iIdetjustf = dr.GetOrdinal(this.Idetjustf);
            if (!dr.IsDBNull(iIdetjustf)) entity.Idetjustf = dr.GetString(iIdetjustf);

            int iIdetcodiold = dr.GetOrdinal(this.Idetcodiold);
            if (!dr.IsDBNull(iIdetcodiold)) entity.Idetcodiold = Convert.ToInt32(dr.GetValue(iIdetcodiold));

            int iIdettipocambio = dr.GetOrdinal(this.Idettipocambio);
            if (!dr.IsDBNull(iIdettipocambio)) entity.Idettipocambio = dr.GetString(iIdettipocambio);
            //Se agrega nuevo campo -Assetec (RAC)
            int iIdetconsval = dr.GetOrdinal(this.Idetconsval);
            if (!dr.IsDBNull(iIdetconsval)) entity.Idetconsval = Convert.ToInt32(dr.GetValue(iIdetconsval));

            return entity;
        }

        #region Mapeo de Campos

        public string Idetcodi = "IDETCODI";
        public string Itotcodi = "ITOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Idetopcom = "IDETOPCOM";
        public string Idetincremental = "IDETINCREMENTAL";
        public string Idetdia = "IDETDIA";
        public string Idettipoindisp = "IDETTIPOINDISP";
        public string Idettieneexc = "IDETTIENEEXC";
        public string Idethoraini = "IDETHORAINI";
        public string Idethorafin = "IDETHORAFIN";
        public string Idetmin = "IDETMIN";
        public string Idethorainiold = "IDETHORAINIOLD";
        public string Idethorafinold = "IDETHORAFINOLD";
        public string Idetminold = "IDETMINOLD";
        public string Idetmw = "IDETMW";
        public string Idetpr = "IDETPR";
        public string Idetminipparcial = "IDETMINIPPARCIAL";
        public string Idetminifparcial = "IDETMINIFPARCIAL";
        public string Idetminparcial = "IDETMINPARCIAL";
        public string Idettienedisp = "IDETTIENEDISP";
        public string Idetfactork = "IDETFACTORK";
        public string Idetpe = "IDETPE";
        public string Idetpa = "IDETPA";
        public string Idetminif = "IDETMINIF";
        public string Idetminip = "IDETMINIP";
        public string Idetnumho = "IDETNUMHO";
        public string Idetnumarranq = "IDETNUMARRANQ";
        public string Idetfechainifort7d = "IDETFECHAINIFORT7D";
        public string Idetfechafinfort7d = "IDETFECHAFINFORT7D";
        public string Idetfechainiprog7d = "IDETFECHAINIPROG7D";
        public string Idetfechafinprog7d = "IDETFECHAFINPROG7D";
        public string Idetdescadic = "IDETDESCADIC";
        public string Idetjustf = "IDETJUSTF";
        public string Idetcodiold = "Idetcodiold";
        public string Idettipocambio = "Idettipocambio";
        //Se agrega nuevo campo -Assetec (RAC)
        public string Idetconsval = "IDETCONSVAL";

        #endregion


        //Assetec[IND.PR25.2022]
        public string SqlListConservarValorByPeriodoCuadro
        {
            get { return base.GetSqlXml("ListConservarValorByPeriodoCuadro"); }
        }
    }
}
