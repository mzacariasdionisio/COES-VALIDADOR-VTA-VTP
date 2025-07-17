using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_CENTRALGEN
    /// </summary>
    public class StCentralgenRepository: RepositoryBase, IStCentralgenRepository
    {
        public StCentralgenRepository(string strConn): base(strConn)
        {
        }

        StCentralgenHelper helper = new StCentralgenHelper();

        public int Save(StCentralgenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, entity.Stgenrcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Stcntgusucreacion, DbType.String, entity.Stcntgusucreacion);
            dbProvider.AddInParameter(command, helper.Stcntgfeccreacion, DbType.DateTime, entity.Stcntgfeccreacion);
            dbProvider.AddInParameter(command, helper.Stcntgusumodificacion, DbType.String, entity.Stcntgusumodificacion);
            dbProvider.AddInParameter(command, helper.Stcntgfecmodificacion, DbType.DateTime, entity.Stcntgfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StCentralgenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, entity.Stgenrcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Stcntgusucreacion, DbType.String, entity.Stcntgusucreacion);
            dbProvider.AddInParameter(command, helper.Stcntgfeccreacion, DbType.DateTime, entity.Stcntgfeccreacion);
            dbProvider.AddInParameter(command, helper.Stcntgusumodificacion, DbType.String, entity.Stcntgusumodificacion);
            dbProvider.AddInParameter(command, helper.Stcntgfecmodificacion, DbType.DateTime, entity.Stcntgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stcntgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StCentralgenDTO GetById(int stcntgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            StCentralgenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCentralgenDTO> List(int id)
        {
            List<StCentralgenDTO> entitys = new List<StCentralgenDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Stgenrcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCentralgenDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iBarrnomb = dr.GetOrdinal(this.helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnomb)) entity.Barrnomb = dr.GetString(iBarrnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StCentralgenDTO> GetByCriteria(int strecacodi)
        {
            List<StCentralgenDTO> entitys = new List<StCentralgenDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCentralgenDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public StCentralgenDTO GetByCentNombre(string equinomb, int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByCentNombre);

            dbProvider.AddInParameter(command, helper.Equinomb, DbType.String, equinomb);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);
            StCentralgenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCentralgenDTO> GetByCriteriaReporte(int strecacodi)
        {
            List<StCentralgenDTO> entitys = new List<StCentralgenDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByCriteriaReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCentralgenDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    int idegeledistancia = dr.GetOrdinal(this.helper.Degeledistancia);
                    if (!dr.IsDBNull(idegeledistancia)) entity.Degeledistancia = Convert.ToDecimal(dr.GetValue(idegeledistancia));

                    int iStenrgrgia = dr.GetOrdinal(this.helper.Stenrgrgia);
                    if (!dr.IsDBNull(iStenrgrgia)) entity.Stenrgrgia = Convert.ToDecimal(dr.GetValue(iStenrgrgia));

                    int iGwhz = dr.GetOrdinal(this.helper.Gwhz);
                    if (!dr.IsDBNull(iGwhz)) entity.Gwhz = Convert.ToDecimal(dr.GetValue(iGwhz));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region SIOSEIN2

        public List<StCentralgenDTO> ObtenerGeneradoresCompensacion(int strecacodi, int stcompcodi)
        {
            List<StCentralgenDTO> entitys = new List<StCentralgenDTO>();
            string query = string.Format(helper.SqlReporteGeneradoresCompensacion, strecacodi, stcompcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCentralgenDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    int idegeledistancia = dr.GetOrdinal(this.helper.Degeledistancia);
                    if (!dr.IsDBNull(idegeledistancia)) entity.Degeledistancia = Convert.ToDecimal(dr.GetValue(idegeledistancia));

                    int iStenrgrgia = dr.GetOrdinal(this.helper.Stenrgrgia);
                    if (!dr.IsDBNull(iStenrgrgia)) entity.Stenrgrgia = Convert.ToDecimal(dr.GetValue(iStenrgrgia));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
