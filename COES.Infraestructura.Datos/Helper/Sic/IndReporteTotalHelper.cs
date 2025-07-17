using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_REPORTE_TOTAL
    /// </summary>
    public class IndReporteTotalHelper : HelperBase
    {
        public IndReporteTotalHelper() : base(Consultas.IndReporteTotalSql)
        {
        }

        public IndReporteTotalDTO Create(IDataReader dr)
        {
            IndReporteTotalDTO entity = new IndReporteTotalDTO();

            int iItotcodi = dr.GetOrdinal(this.Itotcodi);
            if (!dr.IsDBNull(iItotcodi)) entity.Itotcodi = Convert.ToInt32(dr.GetValue(iItotcodi));

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iItotunidadnomb = dr.GetOrdinal(this.Itotunidadnomb);
            if (!dr.IsDBNull(iItotunidadnomb)) entity.Itotunidadnomb = dr.GetString(iItotunidadnomb);

            int itotopcom = dr.GetOrdinal(this.Itotopcom);
            if (!dr.IsDBNull(itotopcom)) entity.Itotopcom = dr.GetString(itotopcom);

            int iItotincremental = dr.GetOrdinal(this.Itotincremental);
            if (!dr.IsDBNull(iItotincremental)) entity.Itotincremental = Convert.ToInt32(dr.GetValue(iItotincremental));

            int iItotminip = dr.GetOrdinal(this.Itotminip);
            if (!dr.IsDBNull(iItotminip)) entity.Itotminip = dr.GetDecimal(iItotminip);

            int iItotminif = dr.GetOrdinal(this.Itotminif);
            if (!dr.IsDBNull(iItotminif)) entity.Itotminif = dr.GetDecimal(iItotminif);

            int iItotminipparcial = dr.GetOrdinal(this.Itotminipparcial);
            if (!dr.IsDBNull(iItotminipparcial)) entity.Itotminipparcial = dr.GetDecimal(iItotminipparcial);

            int iItotminifparcial = dr.GetOrdinal(this.Itotminifparcial);
            if (!dr.IsDBNull(iItotminifparcial)) entity.Itotminifparcial = dr.GetDecimal(iItotminifparcial);

            int iItotpe = dr.GetOrdinal(this.Itotpe);
            if (!dr.IsDBNull(iItotpe)) entity.Itotpe = dr.GetDecimal(iItotpe);

            int iItotfactork = dr.GetOrdinal(this.Itotfactork);
            if (!dr.IsDBNull(iItotfactork)) entity.Itotfactork = dr.GetDecimal(iItotfactork);

            int iItotfactorif = dr.GetOrdinal(this.Itotfactorif);
            if (!dr.IsDBNull(iItotfactorif)) entity.Itotfactorif = dr.GetDecimal(iItotfactorif);

            int iItotfactoripm = dr.GetOrdinal(this.Itotfactoripm);
            if (!dr.IsDBNull(iItotfactoripm)) entity.Itotfactoripm = dr.GetDecimal(iItotfactoripm);

            int iItotfactoripa = dr.GetOrdinal(this.Itotfactoripa);
            if (!dr.IsDBNull(iItotfactoripa)) entity.Itotfactoripa = dr.GetDecimal(iItotfactoripa);

            int iItotcr = dr.GetOrdinal(this.Itotcr);
            if (!dr.IsDBNull(iItotcr)) entity.Itotcr = dr.GetString(iItotcr);

            int iItotindmas15d = dr.GetOrdinal(this.Itotindmas15d);
            if (!dr.IsDBNull(iItotindmas15d)) entity.Itotindmas15d = dr.GetString(iItotindmas15d);

            int iItotinddiasxmes = dr.GetOrdinal(this.Itotinddiasxmes);
            if (!dr.IsDBNull(iItotinddiasxmes)) entity.Itotinddiasxmes = Convert.ToInt32(dr.GetValue(iItotinddiasxmes));

            int iItotfactorpresm = dr.GetOrdinal(this.Itotfactorpresm);
            if (!dr.IsDBNull(iItotfactorpresm)) entity.Itotfactorpresm = dr.GetDecimal(iItotfactorpresm);

            int iItotnumho = dr.GetOrdinal(this.Itotnumho);
            if (!dr.IsDBNull(iItotnumho)) entity.Itotnumho = dr.GetDecimal(iItotnumho);

            int iItotnumarranq = dr.GetOrdinal(this.Itotnumarranq);
            if (!dr.IsDBNull(iItotnumarranq)) entity.Itotnumarranq = Convert.ToInt32(dr.GetValue(iItotnumarranq));

            int iItotdescadic = dr.GetOrdinal(this.Itotdescadic);
            if (!dr.IsDBNull(iItotdescadic)) entity.Itotdescadic = dr.GetString(iItotdescadic);

            int iItotjustf = dr.GetOrdinal(this.Itotjustf);
            if (!dr.IsDBNull(iItotjustf)) entity.Itotjustf = dr.GetString(iItotjustf);

            int iItotcodiold = dr.GetOrdinal(this.Itotcodiold);
            if (!dr.IsDBNull(iItotcodiold)) entity.Itotcodiold = Convert.ToInt32(dr.GetValue(iItotcodiold));

            int iItottipocambio = dr.GetOrdinal(this.Itottipocambio);
            if (!dr.IsDBNull(iItottipocambio)) entity.Itottipocambio = dr.GetString(iItottipocambio);

            //Inicio: IND.PR25.2022
            int iItotpcm3 = dr.GetOrdinal(this.Itotpcm3);
            if (!dr.IsDBNull(iItotpcm3)) entity.Itotpcm3 = dr.GetDecimal(iItotpcm3);

            int iItot1ltvalor = dr.GetOrdinal(this.Itot1ltvalor);
            if (!dr.IsDBNull(iItot1ltvalor)) entity.Itot1ltvalor = dr.GetDecimal(iItot1ltvalor);

            int iItot1ltunidad = dr.GetOrdinal(this.Itot1ltunidad);
            if (!dr.IsDBNull(iItot1ltunidad)) entity.Itot1ltunidad = dr.GetString(iItot1ltunidad);

            int iItotfgte = dr.GetOrdinal(this.Itotfgte);
            if (!dr.IsDBNull(iItotfgte)) entity.Itotfgte = dr.GetDecimal(iItotfgte);

            int iItotfrc = dr.GetOrdinal(this.Itotfrc);
            if (!dr.IsDBNull(iItotfrc)) entity.Itotfrc = dr.GetDecimal(iItotfrc);

            int iItotconsval = dr.GetOrdinal(this.Itotconsval);
            if (!dr.IsDBNull(iItotconsval)) entity.Itotconsval = Convert.ToInt32(dr.GetValue(iItotconsval));
            //Fin: IND.PR25.2022

            return entity;
        }

        #region Mapeo de Campos

        public string Itotcodi = "ITOTCODI";
        public string Irptcodi = "IRPTCODI";
        public string Famcodi = "FAMCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Itotopcom = "ITOTOPCOM";
        public string Itotincremental = "ITOTINCREMENTAL";
        public string Itotunidadnomb = "ITOTUNIDADNOMB";
        public string Itotminip = "ITOTMINIP";
        public string Itotminif = "ITOTMINIF";
        public string Itotminipparcial = "ITOTMINIPPARCIAL";
        public string Itotminifparcial = "ITOTMINIFPARCIAL";
        public string Itotpe = "ITOTPE";
        public string Itotpa = "ITOTPA";
        public string Itotfactork = "ITOTFACTORK";
        public string Itotfactorif = "ITOTFACTORIF";
        public string Itotfactoripm = "ITOTFACTORIPM";
        public string Itotfactoripa = "ITOTFACTORIPA";
        public string Itotcr = "ITOTCR";
        public string Itotindmas15d = "ITOTINDMAS15D";
        public string Itotinddiasxmes = "ITOTINDDIASXMES";
        public string Itotfactorpresm = "ITOTFACTORPRESM";
        public string Itotnumho = "ITOTNUMHO";
        public string Itotnumarranq = "ITOTNUMARRANQ";
        public string Itotdescadic = "ITOTDESCADIC";
        public string Itotjustf = "ITOTJUSTF";
        public string Itotcodiold = "Itotcodiold";
        public string Itottipocambio = "Itottipocambio";

        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "Gruponomb";
        public string Irecafechaini = "IRECAFECHAINI";
        public string Irecafechafin = "IRECAFECHAFIN";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Fenergcodi = "FENERGCODI";

        //INICIO: IND.PR25.2022
        public string Itotpcm3 = "ITOTPCM3";
        public string Itot1ltvalor = "ITOT1LTVALOR";
        public string Itot1ltunidad = "ITOT1LTUNIDAD";
        public string Itotfgte = "ITOTFGTE";
        public string Itotfrc = "ITOTFRC";

        public string Itotconsval = "ITOTCONSVAL";
        //FIN: IND.PR25.2022
        #endregion


        //Assetec[IND.PR25.2022]
        public string SqlListConservarValorByPeriodoCuadro
        {
            get { return base.GetSqlXml("ListConservarValorByPeriodoCuadro"); }
        }
    }
}
