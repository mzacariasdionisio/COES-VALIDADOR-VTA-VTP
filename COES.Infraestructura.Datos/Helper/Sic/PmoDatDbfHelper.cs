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
    public class PmoDatDbfHelper : HelperBase
    {
        public PmoDatDbfHelper()
            : base(Consultas.PmoDatDbf)
        {
        }

        public PmoDatDbfDTO Create(IDataReader dr)
        {
            PmoDatDbfDTO entity = new PmoDatDbfDTO();

            int iPmDbf5Codi = dr.GetOrdinal(this.PmDbf5Codi);
            if (!dr.IsDBNull(iPmDbf5Codi)) entity.PmDbf5Codi = dr.GetInt32(iPmDbf5Codi);

            int iGrupoCodi = dr.GetOrdinal(this.GrupoCodi);
            if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

            int iCodBarra = dr.GetOrdinal(this.CodBarra);
            if (!dr.IsDBNull(iCodBarra)) entity.CodBarra = dr.GetInt32(iCodBarra);

            int iNomBarra = dr.GetOrdinal(this.NomBarra);
            if (!dr.IsDBNull(iNomBarra)) entity.NomBarra = dr.GetString(iNomBarra);

            int iPmDbf5LCod = dr.GetOrdinal(this.PmDbf5LCod);
            if (!dr.IsDBNull(iPmDbf5LCod)) entity.PmDbf5LCod = dr.GetString(iPmDbf5LCod);

            int iPmDbf5FecIni = dr.GetOrdinal(this.PmDbf5FecIni);
            if (!dr.IsDBNull(iPmDbf5FecIni)) entity.PmDbf5FecIni = dr.GetDateTime(iPmDbf5FecIni);

            int iPmBloqCodi = dr.GetOrdinal(this.PmBloqCodi);
            if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = dr.GetInt32(iPmBloqCodi);

            int iPmDbf5Carga = dr.GetOrdinal(this.PmDbf5Carga);
            if (!dr.IsDBNull(iPmDbf5Carga)) entity.PmDbf5Carga = dr.GetDecimal(iPmDbf5Carga);

            int iPmDbf5ICCO = dr.GetOrdinal(this.PmDbf5ICCO);
            if (!dr.IsDBNull(iPmDbf5ICCO)) entity.PmDbf5ICCO = dr.GetInt32(iPmDbf5ICCO);

            int iNroSemana = dr.GetOrdinal(this.NroSemana);
            if (!dr.IsDBNull(iNroSemana)) entity.NroSemana = dr.GetString(iNroSemana);
            

            return entity;
        }

        public string SqlGetGrupo
        {
            get { return base.GetSqlXml("GetGrupo"); }
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

        public string Catecodi = "CATECODI";
        public string GrupoNomb = "GRUPONOMB";

        public string PmPeriCodi = "PMPERICODI";

        public string PmDbf5Codi = "PMDBF5CODI";
        public string GrupoCodi = "GRUPOCODI";
        public string GrupoCodiSDDP = "GRUPOCODISDDP";
        public string NomBarra = "NOMBARRA";
        public string CodBarra = "CODBARRA";
        public string PmDbf5LCod = "PMDBF5LCOD";
        public string PmDbf5FecIni = "PMDBF5FECINI";
        public string PmBloqCodi = "PMBLOQCODI";
        public string PmDbf5Carga = "PMDBF5CARGA";
        public string PmDbf5ICCO = "PMDBF5ICCO";

        public string BCod = "BCod";
        public string BusName = "BusName";
        public string LCod = "LCod";
        public string Fecha = "Fecha";
        public string Llev = "Llev";
        public string Load = "Load";
        public string Icca = "Icca";

        public string Cant = "CANT";

        #region NET 2019-03-04
        public string EnvioFecha = "ENVIOFECHA";
        public string NroSemana = "NROSEMANA";
        public string VPMPeriCodiVal = "V_PMPERICODI_VAL";
        
        public string SqlGetDataBase
        {
            get { return base.GetSqlXml("GetDataBase"); }
        }
        public string SqlDeleteTmp
        {
            get { return base.GetSqlXml("DeleteTmp"); }
        }
        public string SqlSaveTmp
        {
            get { return base.GetSqlXml("SaveTmp"); }
        }
        public string SqlGetDataTmpByFilter
        {
            get { return base.GetSqlXml("GetDataTmpByFilter"); }
        }
        public string SqlGetDataFinalProcesamiento
        {
            get { return base.GetSqlXml("GetDataFinalProcesamiento"); }
        }

        public string SqlCompletarBarrasModelo
        {
            get { return base.GetSqlXml("CompletarBarrasModelo"); }
        }
        
        #endregion
    }
}
