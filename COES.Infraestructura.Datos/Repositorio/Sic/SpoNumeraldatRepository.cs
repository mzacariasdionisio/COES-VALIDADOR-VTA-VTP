using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SPO_NUMERALDAT
    /// </summary>
    public class SpoNumeraldatRepository : RepositoryBase, ISpoNumeraldatRepository
    {
        public SpoNumeraldatRepository(string strConn) : base(strConn)
        {
        }

        SpoNumeraldatHelper helper = new SpoNumeraldatHelper();

        public int Save(SpoNumeraldatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Numdatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, entity.Sconcodi);
            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, entity.Clasicodi);
            dbProvider.AddInParameter(command, helper.Numdatvalor, DbType.Decimal, entity.Numdatvalor);
            dbProvider.AddInParameter(command, helper.Numdatfechainicio, DbType.DateTime, entity.Numdatfechainicio);
            dbProvider.AddInParameter(command, helper.Numdatfechafin, DbType.DateTime, entity.Numdatfechafin);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumeraldatDTO entity)
        {
            string sqlUpdate = string.Empty;
            string valor = "null";
            if (entity.Numdatvalor != null)
            {
                sqlUpdate = string.Format(helper.SqlUpdate, entity.Tipoinfocodi, entity.Sconcodi, entity.Clasicodi, entity.Numdatfechainicio.ToString(ConstantesBase.FormatoFecha), entity.Numdatvalor);
            }
            else
            {
                sqlUpdate = string.Format(helper.SqlUpdate, entity.Tipoinfocodi, entity.Sconcodi, entity.Clasicodi, entity.Numdatfechainicio.ToString(ConstantesBase.FormatoFecha), valor);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int numdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Numdatcodi, DbType.Int32, numdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumeraldatDTO GetById(int numdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Numdatcodi, DbType.Int32, numdatcodi);
            SpoNumeraldatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumeraldatDTO> List()
        {
            List<SpoNumeraldatDTO> entitys = new List<SpoNumeraldatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SpoNumeraldatDTO> GetByCriteria(int numecodi, DateTime fechaini, DateTime fechafin)
        {
            string strSql = string.Format(helper.SqlGetByCriteria, numecodi, fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha));
            List<SpoNumeraldatDTO> entitys = new List<SpoNumeraldatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SpoNumeraldatDTO> GetDataNumerales(int numecodi, DateTime fechaini, DateTime fechafin)
        {
            List<SpoNumeraldatDTO> entitys = new List<SpoNumeraldatDTO>();
            string sqlQuery = string.Format(helper.SqlGetDataNumerales, numecodi, fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            SpoNumeraldatDTO entity = new SpoNumeraldatDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iClasinombre = dr.GetOrdinal(helper.Clasinombre);
                    if (!dr.IsDBNull(iClasinombre)) entity.Clasinombre = dr.GetString(iClasinombre);
                    int iSconnomb = dr.GetOrdinal(helper.Sconnomb);
                    if (!dr.IsDBNull(iSconnomb)) entity.Sconnomb = dr.GetString(iSconnomb);
                    int iSconactivo = dr.GetOrdinal(helper.Sconactivo);
                    if (!dr.IsDBNull(iSconactivo)) entity.Sconactivo = Convert.ToInt32(dr.GetValue(iSconactivo));
                    int iNumcdescrip = dr.GetOrdinal(helper.Numcdescrip);
                    if (!dr.IsDBNull(iNumcdescrip)) entity.Numcdescrip = dr.GetString(iNumcdescrip);
                    int iNumecodi = dr.GetOrdinal(helper.Numecodi);
                    if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));
                    int iNumccodi = dr.GetOrdinal(helper.Numccodi);
                    if (!dr.IsDBNull(iNumccodi)) entity.Numccodi = Convert.ToInt32(dr.GetValue(iNumccodi));
                    int iSconorden = dr.GetOrdinal(helper.Sconorden);
                    if (!dr.IsDBNull(iSconorden)) entity.Sconorden = Convert.ToInt32(dr.GetValue(iSconorden));
                    int iVerncodi = dr.GetOrdinal(helper.Verncodi);
                    if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SpoNumeraldatDTO> GetDataVAlorAgua(DateTime fechaini, DateTime fechafin)
        {
            List<SpoNumeraldatDTO> entitys = new List<SpoNumeraldatDTO>();
            string sqlQuery = string.Format(helper.SqlGetDataVAlorAgua, fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            SpoNumeraldatDTO entity = new SpoNumeraldatDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


    }
}
