using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PmoDatGndseRepository : RepositoryBase, IPmoDatGndseRepository
    {
        public PmoDatGndseRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatGndseHelper helper = new PmoDatGndseHelper();

        public List<PmoDatGndseDTO> ListGndse(int periCodi,string central)
        {
            List<PmoDatGndseDTO> entitys = new List<PmoDatGndseDTO>();
            //string queryString = string.Format(helper.SqlList);
            //string queryString = string.Format(helper.SqlList, periCodi, central.ToUpper().Trim());            

            string queryString ="";
            if(central.Equals("0"))            
                queryString = string.Format(helper.SqlList, periCodi);
            else
                queryString = string.Format(helper.SqlListByCentral, periCodi, central);
            
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);            
            PmoDatGndseDTO entity = null;            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatGndseDTO();

                    int iPmGnd5Codi = dr.GetOrdinal(helper.PmGnd5Codi);
                    if (!dr.IsDBNull(iPmGnd5Codi)) entity.PmGnd5Codi = dr.GetInt32(iPmGnd5Codi);

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoCodiSDDP = dr.GetOrdinal(helper.GrupoCodiSDDP);
                    if (!dr.IsDBNull(iGrupoCodiSDDP)) entity.GrupoCodiSDDP = dr.GetInt32(iGrupoCodiSDDP);

                    int iGrupoNomb = dr.GetOrdinal(helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    int iPmGnd5STG = dr.GetOrdinal(helper.PmGnd5STG);
                    if (!dr.IsDBNull(iPmGnd5STG)) entity.PmGnd5STG = dr.GetString(iPmGnd5STG);

                    int iPmGnd5SCN = dr.GetOrdinal(helper.PmGnd5SCN);
                    if (!dr.IsDBNull(iPmGnd5SCN)) entity.PmGnd5SCN = dr.GetString(iPmGnd5SCN);

                    int iPmBloqCodi = dr.GetOrdinal(helper.PmBloqCodi);
                    if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = dr.GetInt32(iPmBloqCodi);

                    int iPmGnd5PU = dr.GetOrdinal(helper.PmGnd5PU);
                    if (!dr.IsDBNull(iPmGnd5PU)) entity.PmGnd5PU = dr.GetDecimal(iPmGnd5PU);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
         //Método modificado - NET 2019-03-06
        public List<PmoDatGndseDTO> ListDatGndse(int periCodi)
        {
            List<PmoDatGndseDTO> entitys = new List<PmoDatGndseDTO>();
            string queryString = string.Format(helper.SqlGetDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            PmoDatGndseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatGndseDTO();

                    int iStg = dr.GetOrdinal(helper.Stg);
                    if (!dr.IsDBNull(iStg)) entity.Stg = dr.GetString(iStg);

                    int iScn = dr.GetOrdinal(helper.Scn);
                    if (!dr.IsDBNull(iScn)) entity.Scn = dr.GetString(iScn);

                    int iLblk = dr.GetOrdinal(helper.Lblk);
                    if (!dr.IsDBNull(iLblk)) entity.Lblk = dr.GetString(iLblk);

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

                    int iPu = dr.GetOrdinal(helper.Pu);
                    if (!dr.IsDBNull(iPu)) entity.Pu = dr.GetString(iPu);

                    #region 20190308 - NET: Adecuaciones a los archivos .DAT
                    int iPmGnd5PU = dr.GetOrdinal(helper.PmGnd5PU);
                    if (!dr.IsDBNull(iPmGnd5PU)) entity.PmGnd5PU = dr.GetDecimal(iPmGnd5PU);
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListDatGndseCabeceras(int periCodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetCabeceras);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoAbrev = dr.GetOrdinal(helper.GrupoAbrev);
                    if (!dr.IsDBNull(iGrupoAbrev)) entity.Grupoabrev = dr.GetString(iGrupoAbrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountDatGndse(int periCodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetCount);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int Cantidad = 0;
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCant = dr.GetOrdinal(helper.Cant);
                    if (!dr.IsDBNull(iCant)) Cantidad = dr.GetInt32(iCant);
                }
            }

            return Cantidad;
        }

        public int Delete(int periCodi, string central)
        {
            //string queryString = string.Format(helper.SqlDelete, periCodi, central.ToUpper().Trim());
            string queryString = "";
            if (central.Equals("0"))
                queryString = string.Format(helper.SqlDelete, periCodi);
            else
                queryString = string.Format(helper.SqlDeleteByCentral, periCodi, central);
            
            
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);            
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            //dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);            

            return dbProvider.ExecuteNonQuery(command);
        }

        public int Save(PmoDatGndseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PmGnd5Codi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PmGnd5STG, DbType.String, entity.PmGnd5STG);
            dbProvider.AddInParameter(command, helper.PmGnd5SCN, DbType.String, entity.PmGnd5SCN);
            dbProvider.AddInParameter(command, helper.PmBloqCodi, DbType.Int32, entity.PmBloqCodi);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmGnd5PU, DbType.Decimal, entity.PmGnd5PU);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PmPeriCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        #region NET 2019-03-06

        public List<PmoDatGndseDTO> GetDataProcesamiento(DateTime fechaInicio, DateTime fechaFin, DateTime Fechaperiodo)
        {
            List<PmoDatGndseDTO> entitys = new List<PmoDatGndseDTO>();
            string queryString = string.Format(helper.SqlGetDataProcesamiento, fechaInicio.ToString(ConstantesBase.FormatoFechaHora), fechaFin.ToString(ConstantesBase.FormatoFechaHora), Fechaperiodo.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PmoDatGndseDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatGndseDTO();
                    

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);
                    
                    int iPmGnd5STG = dr.GetOrdinal(helper.PmGnd5STG);
                    if (!dr.IsDBNull(iPmGnd5STG)) entity.PmGnd5STG = (dr.GetInt32(iPmGnd5STG)).ToString();

                    int iPmGnd5SCN = dr.GetOrdinal(helper.PmGnd5SCN);
                    if (!dr.IsDBNull(iPmGnd5SCN)) entity.PmGnd5SCN = (dr.GetInt32(iPmGnd5SCN)).ToString();

                    int iPmBloqCodi = dr.GetOrdinal(helper.PmBloqCodi);
                    if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = dr.GetInt32(iPmBloqCodi);

                    int iPmGnd5PU = dr.GetOrdinal(helper.PmGnd5PU);
                    if (!dr.IsDBNull(iPmGnd5PU)) entity.PmGnd5PU = dr.GetDecimal(iPmGnd5PU);

                    int iEnvioFecha = dr.GetOrdinal(helper.EnvioFecha);
                    if (!dr.IsDBNull(iEnvioFecha)) entity.EnvioFecha = dr.GetDateTime(iEnvioFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmoDatGndseDTO> GetDataByFilter(int codigoPeriodo, int grupoCodi, string numSemana, int bloqCodi)
        {
            List<PmoDatGndseDTO> entitys = new List<PmoDatGndseDTO>();
            string queryString = string.Format(helper.SqlGetDataByFilter, codigoPeriodo, grupoCodi, numSemana, bloqCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            
            PmoDatGndseDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatGndseDTO();

                    int iPmGnd5Codi = dr.GetOrdinal(helper.PmGnd5Codi);
                    if (!dr.IsDBNull(iPmGnd5Codi)) entity.PmGnd5Codi = dr.GetInt32(iPmGnd5Codi);

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoCodiSDDP = dr.GetOrdinal(helper.GrupoCodiSDDP);
                    if (!dr.IsDBNull(iGrupoCodiSDDP)) entity.GrupoCodiSDDP = dr.GetInt32(iGrupoCodiSDDP);

                    int iGrupoNomb = dr.GetOrdinal(helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    int iPmGnd5STG = dr.GetOrdinal(helper.PmGnd5STG);
                    if (!dr.IsDBNull(iPmGnd5STG)) entity.PmGnd5STG = dr.GetString(iPmGnd5STG);

                    int iPmGnd5SCN = dr.GetOrdinal(helper.PmGnd5SCN);
                    if (!dr.IsDBNull(iPmGnd5SCN)) entity.PmGnd5SCN = dr.GetString(iPmGnd5SCN);

                    int iPmBloqCodi = dr.GetOrdinal(helper.PmBloqCodi);
                    if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = dr.GetInt32(iPmBloqCodi);

                    int iPmGnd5PU = dr.GetOrdinal(helper.PmGnd5PU);
                    if (!dr.IsDBNull(iPmGnd5PU)) entity.PmGnd5PU = dr.GetDecimal(iPmGnd5PU);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrGrupoDTO> GetCentralesByPeriodo(int codigoPeriodo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetCentralesByPeriodo, codigoPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PrGrupoHelper helperGrupo = new PrGrupoHelper();            

            PrGrupoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helperGrupo.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGrupoNomb = dr.GetOrdinal(helperGrupo.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void CompletarUnidadesGenModelo(int PeriCodi)
        {
            string queryString = string.Format(helper.SqlCompletarUnidadesGenModelo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.VPMPeriCodiVal, DbType.Int32, PeriCodi);

            var result = dbProvider.ExecuteNonQuery(command);
        }

        #endregion

    }
}
