using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PmoDatCgndHelper : HelperBase
    {
        public PmoDatCgndHelper()
            : base(Consultas.PmoDatCgnd)
        {
        }

        public PmoDatCgndDTO Create(IDataReader dr)
        {
            PmoDatCgndDTO entity = new PmoDatCgndDTO();

            int iPmCgndCodi = dr.GetOrdinal(this.PmCgndCodi);
            if (!dr.IsDBNull(iPmCgndCodi)) entity.PmCgndCodi = dr.GetInt32(iPmCgndCodi);

            int iGrupoCodi = dr.GetOrdinal(this.GrupoCodi);
            if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

            int iCodCentral = dr.GetOrdinal(this.CodCentral);
            if (!dr.IsDBNull(iCodCentral)) entity.CodCentral = dr.GetInt32(iCodCentral);

            int iNombCentral = dr.GetOrdinal(this.NombCentral);
            if (!dr.IsDBNull(iNombCentral)) entity.NombCentral = dr.GetString(iNombCentral);

            int iCodBarra = dr.GetOrdinal(this.CodBarra);
            if (!dr.IsDBNull(iCodBarra)) entity.CodBarra = dr.GetInt32(iCodBarra);

            int iNombBarra = dr.GetOrdinal(this.NombBarra);
            if (!dr.IsDBNull(iNombBarra)) entity.NombBarra = dr.GetString(iNombBarra);

            int iPmCgndTipoPlanta = dr.GetOrdinal(this.PmCgndTipoPlanta);
            if (!dr.IsDBNull(iPmCgndTipoPlanta)) entity.PmCgndTipoPlanta = dr.GetString(iPmCgndTipoPlanta);

            int iPmCgndNroUnidades = dr.GetOrdinal(this.PmCgndNroUnidades);
            if (!dr.IsDBNull(iPmCgndNroUnidades)) entity.PmCgndNroUnidades = dr.GetInt32(iPmCgndNroUnidades);

            int iPmCgndPotInstalada = dr.GetOrdinal(this.PmCgndPotInstalada);
            if (!dr.IsDBNull(iPmCgndPotInstalada)) entity.PmCgndPotInstalada = dr.GetDecimal(iPmCgndPotInstalada);

            int iPmCgndFactorOpe = dr.GetOrdinal(this.PmCgndFactorOpe);
            if (!dr.IsDBNull(iPmCgndFactorOpe)) entity.PmCgndFactorOpe = dr.GetDecimal(iPmCgndFactorOpe);

            int iPmCgndProbFalla = dr.GetOrdinal(this.PmCgndProbFalla);
            if (!dr.IsDBNull(iPmCgndProbFalla)) entity.PmCgndProbFalla = dr.GetDecimal(iPmCgndProbFalla);

            int iPmCgndCorteOFalla = dr.GetOrdinal(this.PmCgndCorteOFalla);
            if (!dr.IsDBNull(iPmCgndCorteOFalla)) entity.PmCgndCorteOFalla = dr.GetDecimal(iPmCgndCorteOFalla);

            return entity;
        }

        #region Mapeo de Campos PmoDatCgnd

        public string PmCgndCodi = "PMCGNDCODI";
        public string GrupoCodi = "GRUPOCODI";
        public string PmCgndGrupoCodiBarra = "PMCGNDGRUPOCODIBARRA";
        public string PmCgndTipoPlanta = "PMCGNDTIPOPLANTA";
        public string PmCgndNroUnidades = "PMCGNDNROUNIDADES";
        public string PmCgndPotInstalada = "PMCGNDPOTINSTALADA";
        public string PmCgndFactorOpe = "PMCGNDFACTOROPE";
        public string PmCgndProbFalla = "PMCGNDPROBFALLA";
        public string PmCgndCorteOFalla = "PMCGNDCORTEOFALLA";

        //adicionales list
        public string CodCentral = "CODCENTRAL";
        public string NombCentral = "NOMBCENTRAL";
        public string CodBarra = "CODBARRA";
        public string NombBarra = "NOMBBARRA";

        #endregion

        public string GrupoNomb = "GRUPONOMB";

        public string Num = "Num";
        public string Name = "Name";
        public string Bus = "Bus";
        public string Tipo = "Tipo";
        public string Uni = "Uni";
        public string PotIns = "PotIns";
        public string FatOpe = "FatOpe";
        public string ProbFal = "ProbFal";
        public string SFal = "SFal";

        public string Cant = "CANT";
        public string PmPeriCodi = "PMPERICODI";
        public string SqlGetBarra
        {
            get { return base.GetSqlXml("GetBarra"); }
        }

        public string SqlGetDat
        {
            get { return base.GetSqlXml("GetDat"); }
        }

        public string SqlGetCount
        {
            get { return base.GetSqlXml("GetCount"); }
        }

        public string SqlGetGrupoCodi
        {
            get { return base.GetSqlXml("GetGrupoCodi"); }
        }
        
    }
}
