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
    public class PmoDatDbfRepository : RepositoryBase, IPmoDatDbfRepository
    {
        public PmoDatDbfRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatDbfHelper helper = new PmoDatDbfHelper();

        public List<PmoDatDbfDTO> ListDbf(int codigoPeriodo,string nombarra)
        {
            List<PmoDatDbfDTO> entitys = new List<PmoDatDbfDTO>();
            string queryString = string.Format(helper.SqlList, codigoPeriodo, nombarra.ToUpper().Trim());
            //string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            //dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, codigoPeriodo);
            //dbProvider.AddInParameter(command, helper.GrupoNomb, DbType.String, nombarra.ToUpper());

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbfDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmoDatDbfDTO> ListDatDbf(int codigoPeriodo)
        {
            List<PmoDatDbfDTO> entitys = new List<PmoDatDbfDTO>();
            string queryString = string.Format(helper.SqlGetDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, codigoPeriodo);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbfDTO entity = new PmoDatDbfDTO();

                    int iBCod = dr.GetOrdinal(helper.BCod);
                    if (!dr.IsDBNull(iBCod)) entity.BCod = dr.GetString(iBCod);

                    int iBusName = dr.GetOrdinal(helper.BusName);
                    if (!dr.IsDBNull(iBusName)) entity.BusName = dr.GetString(iBusName);

                    int iLCod = dr.GetOrdinal(helper.LCod);
                    if (!dr.IsDBNull(iLCod)) entity.LCod = dr.GetString(iLCod);

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetString(iFecha);

                    int iLlev = dr.GetOrdinal(helper.Llev);
                    if (!dr.IsDBNull(iLlev)) entity.Llev = dr.GetString(iLlev);

                    int iLoad = dr.GetOrdinal(helper.Load);
                    if (!dr.IsDBNull(iLoad)) entity.Load = dr.GetString(iLoad);

                    int iPmDbf5Carga = dr.GetOrdinal(helper.PmDbf5Carga);//20190308 - NET: Adecuaciones a los archivos .DAT
                    if (!dr.IsDBNull(iPmDbf5Carga)) entity.PmDbf5Carga = dr.GetDecimal(iPmDbf5Carga);//20190308 - NET: Adecuaciones a los archivos .DAT

                    int iIcca = dr.GetOrdinal(helper.Icca);
                    if (!dr.IsDBNull(iIcca)) entity.Icca = dr.GetString(iIcca);

                    int iGrupoCodiSDDP = dr.GetOrdinal(helper.GrupoCodiSDDP);
                    if (!dr.IsDBNull(iGrupoCodiSDDP)) entity.GrupoCodiSDDP = dr.GetInt32(iGrupoCodiSDDP);

                    int iPmDbf5FecIni = dr.GetOrdinal(helper.PmDbf5FecIni);
                    if (!dr.IsDBNull(iPmDbf5FecIni)) entity.PmDbf5FecIni = dr.GetDateTime(iPmDbf5FecIni);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListGrupoDbf(int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);

            PrGrupoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoNomb = dr.GetOrdinal(helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountDatDbf(int periCodi)
        {
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

        public int Delete(int periCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int Save(PmoDatDbfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PmDbf5Codi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5LCod, DbType.String, entity.PmDbf5LCod);
            dbProvider.AddInParameter(command, helper.PmDbf5FecIni, DbType.DateTime, entity.PmDbf5FecIni);
            dbProvider.AddInParameter(command, helper.PmBloqCodi, DbType.Int32, entity.PmBloqCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5Carga, DbType.Decimal, entity.PmDbf5Carga);
            dbProvider.AddInParameter(command, helper.PmDbf5ICCO, DbType.Int32, entity.PmDbf5ICCO);

            return dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupoDTO GetGrupoCodi(int grupoCodiSddp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetGrupoCodi);

            dbProvider.AddInParameter(command, helper.GrupoCodiSDDP, DbType.Int32, grupoCodiSddp);

            PrGrupoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoNomb = dr.GetOrdinal(helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                }
            }

            return entity;
        }


        #region NET 2019-03-04

        public List<PmoDatDbfDTO> GetDataBaseDbf(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PmoDatDbfDTO> entitys = new List<PmoDatDbfDTO>();
            string queryString = string.Format(helper.SqlGetDataBase, fechaInicio.ToString(ConstantesBase.FormatoFechaHora), fechaFin.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbfDTO entity = new PmoDatDbfDTO();

                    /*int iBCod = dr.GetOrdinal(helper.BCod);
                    if (!dr.IsDBNull(iBCod)) entity.BCod = dr.GetString(iBCod);
                    */

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupoCodi));
                    
                    int iPmDbf5LCod = dr.GetOrdinal(helper.PmDbf5LCod);
                    if (!dr.IsDBNull(iPmDbf5LCod)) entity.PmDbf5LCod = dr.GetString(iPmDbf5LCod);

                    int iPmDbf5FecIni = dr.GetOrdinal(helper.PmDbf5FecIni);
                    if (!dr.IsDBNull(iPmDbf5FecIni)) entity.PmDbf5FecIni = dr.GetDateTime(iPmDbf5FecIni);

                    int iPmBloqCodi = dr.GetOrdinal(helper.PmBloqCodi);
                    if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = Convert.ToInt32(dr.GetValue(iPmBloqCodi));

                    int iPmDbf5Carga = dr.GetOrdinal(helper.PmDbf5Carga);
                    if (!dr.IsDBNull(iPmDbf5Carga)) entity.PmDbf5Carga = dr.GetDecimal(iPmDbf5Carga);

                    int iIcca = dr.GetOrdinal(helper.PmDbf5ICCO);
                    if (!dr.IsDBNull(iIcca)) entity.PmDbf5ICCO = Convert.ToInt32(dr.GetValue(iIcca));

                    int iEnvioFecha = dr.GetOrdinal(helper.EnvioFecha);
                    if (!dr.IsDBNull(iEnvioFecha)) entity.EnvioFecha = dr.GetDateTime(iEnvioFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int DeleteDataTmp(int periCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteTmp);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            return dbProvider.ExecuteNonQuery(command);
        }
        public void SqlSaveTmp(PmoDatDbfDTO entity)
        {
           
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveTmp);

            dbProvider.AddInParameter(command, helper.PmDbf5Codi, DbType.Int32, 0);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5LCod, DbType.String, entity.PmDbf5LCod);
            dbProvider.AddInParameter(command, helper.PmDbf5FecIni, DbType.DateTime, entity.PmDbf5FecIni);
            dbProvider.AddInParameter(command, helper.PmBloqCodi, DbType.Int32, entity.PmBloqCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5Carga, DbType.Decimal, entity.PmDbf5Carga);
            dbProvider.AddInParameter(command, helper.PmDbf5ICCO, DbType.Int32, entity.PmDbf5ICCO);
            dbProvider.ExecuteNonQuery(command);
        }
        //
        public List<PmoDatDbfDTO> GetDataTmpByFilter(int codigoPeriodo, int grupoCodi,DateTime fechaIni, int bloqCodi,string lcod)
        {
            List<PmoDatDbfDTO> entitys = new List<PmoDatDbfDTO>();
            string queryString = string.Format(helper.SqlGetDataTmpByFilter, codigoPeriodo, grupoCodi, fechaIni.ToString(ConstantesBase.FormatoFechaHora), bloqCodi,lcod);
                        
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbfDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmoDatDbfDTO> GetDataFinalProcesamiento(int codigoPeriodo)
        {
            List<PmoDatDbfDTO> entitys = new List<PmoDatDbfDTO>();
            string queryString = string.Format(helper.SqlGetDataFinalProcesamiento, codigoPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbfDTO entity = new PmoDatDbfDTO();
                                       
                    int iPmPeriCodi = dr.GetOrdinal(helper.PmPeriCodi);
                    if (!dr.IsDBNull(iPmPeriCodi)) entity.PeriCodi = Convert.ToInt32(dr.GetValue(iPmPeriCodi));

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupoCodi));

                    int iPmDbf5LCod = dr.GetOrdinal(helper.PmDbf5LCod);
                    if (!dr.IsDBNull(iPmDbf5LCod)) entity.PmDbf5LCod = dr.GetString(iPmDbf5LCod);

                    int iPmDbf5FecIni = dr.GetOrdinal(helper.PmDbf5FecIni);
                    if (!dr.IsDBNull(iPmDbf5FecIni)) entity.PmDbf5FecIni = dr.GetDateTime(iPmDbf5FecIni);

                    int iPmBloqCodi = dr.GetOrdinal(helper.PmBloqCodi);
                    if (!dr.IsDBNull(iPmBloqCodi)) entity.PmBloqCodi = Convert.ToInt32(dr.GetValue(iPmBloqCodi));                                       

                    int iIcco = dr.GetOrdinal(helper.PmDbf5ICCO);
                    if (!dr.IsDBNull(iIcco)) entity.PmDbf5ICCO = Convert.ToInt32(dr.GetValue(iIcco));

                    int iPmDbf5Carga = dr.GetOrdinal(helper.PmDbf5Carga);
                    if (!dr.IsDBNull(iPmDbf5Carga)) entity.PmDbf5Carga = dr.GetDecimal(iPmDbf5Carga);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Update(PmoDatDbfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5LCod, DbType.String, entity.PmDbf5LCod);
            dbProvider.AddInParameter(command, helper.PmDbf5FecIni, DbType.DateTime, entity.PmDbf5FecIni);
            dbProvider.AddInParameter(command, helper.PmBloqCodi, DbType.Int32, entity.PmBloqCodi);
            dbProvider.AddInParameter(command, helper.PmDbf5Carga, DbType.Decimal, entity.PmDbf5Carga);
            dbProvider.AddInParameter(command, helper.PmDbf5ICCO, DbType.Int32, entity.PmDbf5ICCO);
            dbProvider.AddInParameter(command, helper.PmDbf5Codi, DbType.Int32, entity.PmDbf5Codi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void CompletarBarrasModelo(int PeriCodi)
        {
            string queryString = string.Format(helper.SqlCompletarBarrasModelo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                        
            dbProvider.AddInParameter(command, helper.VPMPeriCodiVal, DbType.Int32, PeriCodi);
            
            var result = dbProvider.ExecuteNonQuery(command);
        }

        #endregion
    }
}
