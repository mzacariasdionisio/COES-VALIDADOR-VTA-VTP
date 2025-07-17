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
    public class PmoDatMgndHelper : HelperBase
    {
        public PmoDatMgndHelper()
            : base(Consultas.PmoDatMgndpe)
        {
        }

        public PmoDatMgndDTO Create(IDataReader dr)
        {
            PmoDatMgndDTO entity = new PmoDatMgndDTO();

            int iPmMgndCodi = dr.GetOrdinal(this.PmMgndCodi);
            if (!dr.IsDBNull(iPmMgndCodi)) entity.PmMgndCodi = dr.GetInt32(iPmMgndCodi);

            int iGrupoCodi = dr.GetOrdinal(this.GrupoCodi);
            if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

            int iPmMgndFecha = dr.GetOrdinal(this.PmMgndFecha);
            if (!dr.IsDBNull(iPmMgndFecha)) entity.PmMgndFecha = dr.GetDateTime(iPmMgndFecha);

            int iCodCentral = dr.GetOrdinal(this.CodCentral);
            if (!dr.IsDBNull(iCodCentral)) entity.CodCentral = dr.GetInt32(iCodCentral);

            int iNombCentral = dr.GetOrdinal(this.NombCentral);
            if (!dr.IsDBNull(iNombCentral)) entity.NombCentral = dr.GetString(iNombCentral);

            int iCodBarra = dr.GetOrdinal(this.CodBarra);
            if (!dr.IsDBNull(iCodBarra)) entity.CodBarra = dr.GetInt32(iCodBarra);

            int iNombBarra = dr.GetOrdinal(this.NombBarra);
            if (!dr.IsDBNull(iNombBarra)) entity.NombBarra = dr.GetString(iNombBarra);

            int iPmMgndTipoPlanta = dr.GetOrdinal(this.PmMgndTipoPlanta);
            if (!dr.IsDBNull(iPmMgndTipoPlanta)) entity.PmMgndTipoPlanta = dr.GetString(iPmMgndTipoPlanta);

            int iPmMgndNroUnidades = dr.GetOrdinal(this.PmMgndNroUnidades);
            if (!dr.IsDBNull(iPmMgndNroUnidades)) entity.PmMgndNroUnidades = dr.GetInt32(iPmMgndNroUnidades);

            int iPmMgndPotInstalada = dr.GetOrdinal(this.PmMgndPotInstalada);
            if (!dr.IsDBNull(iPmMgndPotInstalada)) entity.PmMgndPotInstalada = dr.GetDecimal(iPmMgndPotInstalada);

            int iPmMgndFactorOpe = dr.GetOrdinal(this.PmMgndFactorOpe);
            if (!dr.IsDBNull(iPmMgndFactorOpe)) entity.PmMgndFactorOpe = dr.GetDecimal(iPmMgndFactorOpe);

            int iPmMgndProbFalla = dr.GetOrdinal(this.PmMgndProbFalla);
            if (!dr.IsDBNull(iPmMgndProbFalla)) entity.PmMgndProbFalla = dr.GetDecimal(iPmMgndProbFalla);

            int iPmMgndCorteOFalla = dr.GetOrdinal(this.PmMgndCorteOFalla);
            if (!dr.IsDBNull(iPmMgndCorteOFalla)) entity.PmMgndCorteOFalla = dr.GetDecimal(iPmMgndCorteOFalla);

            return entity;
        }

        public string PmMgndCodi = "PMMGNDCODI";
        public string PmMgndFecha = "PMMGNDFECHA";
        public string CodCentral = "CODCENTRAL";
        public string NombCentral = "NOMBCENTRAL";
        public string CodBarra = "PMMGNDGRUPOCODIBARRA";//20190317 - NET : Corrección
        public string NombBarra = "NOMBBARRA";
        public string PmMgndTipoPlanta = "PMMGNDTIPOPLANTA";
        public string PmMgndNroUnidades = "PMMGNDNROUNIDADES";
        public string PmMgndPotInstalada = "PMMGNDPOTINSTALADA";
        public string PmMgndFactorOpe = "PMMGNDFACTOROPE";
        public string PmMgndProbFalla = "PMMGNDPROBFALLA";
        public string PmMgndCorteOFalla = "PMMGNDCORTEOFALLA";
        

        public string GrupoCodi = "GRUPOCODI";
        public string GrupoNomb = "GRUPONOMB";

        public string Fecha = "Fecha";
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
    }
}
