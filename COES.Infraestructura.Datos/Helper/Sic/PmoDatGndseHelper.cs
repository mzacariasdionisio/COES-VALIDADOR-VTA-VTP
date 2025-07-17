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
    public class PmoDatGndseHelper : HelperBase
    {
        public PmoDatGndseHelper()
            : base(Consultas.PmoDatGndse)
        {
        }

        public PmoDatGndseDTO Create(IDataReader dr)
        {
            PmoDatGndseDTO entity = new PmoDatGndseDTO();

            //int iPmCgndCodi = dr.GetOrdinal(this.PmCgndCodi);
            //if (!dr.IsDBNull(iPmCgndCodi)) entity.PmCgndCodi = dr.GetInt32(iPmCgndCodi);

            //int iGrupoCodi = dr.GetOrdinal(this.GrupoCodi);
            //if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

            //int iCodCentral = dr.GetOrdinal(this.CodCentral);
            //if (!dr.IsDBNull(iCodCentral)) entity.CodCentral = dr.GetInt32(iCodCentral);

            //int iNombCentral = dr.GetOrdinal(this.NombCentral);
            //if (!dr.IsDBNull(iNombCentral)) entity.NombCentral = dr.GetString(iNombCentral);

            //int iCodBarra = dr.GetOrdinal(this.CodBarra);
            //if (!dr.IsDBNull(iCodBarra)) entity.CodBarra = dr.GetInt32(iCodBarra);

            //int iNombBarra = dr.GetOrdinal(this.NombBarra);
            //if (!dr.IsDBNull(iNombBarra)) entity.NombBarra = dr.GetString(iNombBarra);

            //int iPmCgndTipoPlanta = dr.GetOrdinal(this.PmCgndTipoPlanta);
            //if (!dr.IsDBNull(iPmCgndTipoPlanta)) entity.PmCgndTipoPlanta = dr.GetString(iPmCgndTipoPlanta);

            //int iPmCgndNroUnidades = dr.GetOrdinal(this.PmCgndNroUnidades);
            //if (!dr.IsDBNull(iPmCgndNroUnidades)) entity.PmCgndNroUnidades = dr.GetInt32(iPmCgndNroUnidades);

            //int iPmCgndPotInstalada = dr.GetOrdinal(this.PmCgndPotInstalada);
            //if (!dr.IsDBNull(iPmCgndPotInstalada)) entity.PmCgndPotInstalada = dr.GetDecimal(iPmCgndPotInstalada);

            //int iPmCgndFactorOpe = dr.GetOrdinal(this.PmCgndFactorOpe);
            //if (!dr.IsDBNull(iPmCgndFactorOpe)) entity.PmCgndFactorOpe = dr.GetDecimal(iPmCgndFactorOpe);

            //int iPmCgndProbFalla = dr.GetOrdinal(this.PmCgndProbFalla);
            //if (!dr.IsDBNull(iPmCgndProbFalla)) entity.PmCgndProbFalla = dr.GetDecimal(iPmCgndProbFalla);

            //int iPmCgndCorteOFalla = dr.GetOrdinal(this.PmCgndCorteOFalla);
            //if (!dr.IsDBNull(iPmCgndCorteOFalla)) entity.PmCgndCorteOFalla = dr.GetDecimal(iPmCgndCorteOFalla);

            return entity;
        }


        public string PmPeriCodi = "PMPERICODI";
        public string PmGnd5Codi = "PMGND5CODI";
        public string GrupoCodi = "GRUPOCODI";
        public string GrupoCodiSDDP = "GRUPOCODISDDP";
        public string GrupoNomb = "GRUPONOMB";
        public string PmGnd5STG = "PMGND5STG";
        public string PmGnd5SCN = "PMGND5SCN";
        public string PmBloqCodi = "PMBLOQCODI";
        public string PmGnd5PU = "PMGND5PU";

        public string GrupoAbrev = "GrupoAbrev";
        public string Cant = "CANT";

        public string Stg = "Stg";
        public string Scn = "Scn";
        public string Lblk = "LBlk";
        public string Pu = "PU";

        public string VPMPeriCodiVal = "V_PMPERICODI_VAL";
        public string SqlGetDat
        {
            get { return base.GetSqlXml("GetDat"); }
        }

        public string SqlGetCabeceras
        {
            get { return base.GetSqlXml("GetCabeceras"); }
        }

        public string SqlGetCount
        {
            get { return base.GetSqlXml("GetCount"); }
        }

        #region NET 20190306
        public string EnvioFecha = "ENVIOFECHA";
        public string SqlGetDataProcesamiento
        {
            get { return base.GetSqlXml("GetDataProcesamiento"); }
        }
        public string SqlGetDataByFilter
        {
            get { return base.GetSqlXml("GetDataByFilter"); }
        }

        public string SqlGetCentralesByPeriodo
        {
            get { return base.GetSqlXml("GetCentralesByPeriodo"); }
        }


        public string SqlListByCentral
        {
            get { return base.GetSqlXml("ListByCentral"); }
        }

        public string SqlDeleteByCentral
        {
            get { return base.GetSqlXml("DeleteByCentral"); }
        }

        public string SqlCompletarUnidadesGenModelo
        {
            get { return base.GetSqlXml("CompletarUnidadesGenModelo"); }
        }

        #endregion
    }
}
