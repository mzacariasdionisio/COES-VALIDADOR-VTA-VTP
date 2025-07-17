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
    /// Clase de acceso a datos de la tabla CM_COSTOMARGINALPROG
    /// </summary>
    public class CmCostomarginalprogRepository: RepositoryBase, ICmCostomarginalprogRepository
    {
        public CmCostomarginalprogRepository(string strConn): base(strConn)
        {
        }

        CmCostomarginalprogHelper helper = new CmCostomarginalprogHelper();

        public int Save(CmCostomarginalprogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmarprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmarprtotal, DbType.Decimal, entity.Cmarprtotal);
            dbProvider.AddInParameter(command, helper.Cmarprfecha, DbType.DateTime, entity.Cmarprfecha);
            dbProvider.AddInParameter(command, helper.Cmarprlastuser, DbType.String, entity.Cmarprlastuser);
            dbProvider.AddInParameter(command, helper.Cmarprlastdate, DbType.DateTime, entity.Cmarprlastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmCostomarginalprogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmarprtotal, DbType.Decimal, entity.Cmarprtotal);
            dbProvider.AddInParameter(command, helper.Cmarprfecha, DbType.DateTime, entity.Cmarprfecha);
            dbProvider.AddInParameter(command, helper.Cmarprlastuser, DbType.String, entity.Cmarprlastuser);
            dbProvider.AddInParameter(command, helper.Cmarprlastdate, DbType.DateTime, entity.Cmarprlastdate);
            dbProvider.AddInParameter(command, helper.Cmarprcodi, DbType.Int32, entity.Cmarprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmarprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmarprcodi, DbType.Int32, cmarprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCostomarginalprogDTO GetById(int cmarprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmarprcodi, DbType.Int32, cmarprcodi);
            CmCostomarginalprogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmCostomarginalprogDTO> List()
        {
            List<CmCostomarginalprogDTO> entitys = new List<CmCostomarginalprogDTO>();
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

        public List<CmCostomarginalprogDTO> GetByCriteria()
        {
            List<CmCostomarginalprogDTO> entitys = new List<CmCostomarginalprogDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region MonitoreoMME
        public List<CmCostomarginalprogDTO> ListPeriodoCostoMarProg(DateTime fechaPeriodo)
        {
            List<CmCostomarginalprogDTO> entitys = new List<CmCostomarginalprogDTO>();

            string queryString = string.Format(helper.SqlListCostMarProg, fechaPeriodo.ToString(ConstantesBase.FormatoFechaBase));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostomarginalprogDTO entity = new CmCostomarginalprogDTO();
                    entity = helper.Create(dr);
                    int iGrupoCodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupoCodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region SIOSEIN

        public List<CmCostomarginalprogDTO> GetByBarratranferencia(string barrcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmCostomarginalprogDTO> entitys = new List<CmCostomarginalprogDTO>();

            string queryString = string.Format(helper.SqlGetByBarratranferencia, barrcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                CmCostomarginalprogDTO entity = new CmCostomarginalprogDTO();
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iCnfbarnombre = dr.GetOrdinal(this.helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region IMME

        public void GrabarDatosBulk(List<CmCostomarginalprogDTO> entitys, DateTime fechaInicio, DateTime fechaFin)
        {

            string sqlDelete = string.Format(helper.SqlDeleteDias, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
            command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command.CommandTimeout = 0;

            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();
            foreach(var reg in entitys)
            {
                reg.Cmarprcodi = id++;
            }

            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Cmarprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cnfbarcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmarprtotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmarprfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Cmarprlastuser, DbType.String);
            dbProvider.AddColumnMapping(helper.Cmarprlastdate, DbType.DateTime);


            #endregion

            dbProvider.BulkInsert<CmCostomarginalprogDTO>(entitys, helper.NombreTabla);
        }

        public List<CmCostomarginalprogDTO> ListaTotalXDia(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmCostomarginalprogDTO> entitys = new List<CmCostomarginalprogDTO>();
            CmCostomarginalprogDTO entity = new CmCostomarginalprogDTO();
            string queryString = string.Format(helper.SqlListaTotalXDia,  fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CmCostomarginalprogDTO();
                    int iFechadia = dr.GetOrdinal(helper.Fechadia);
                    if (!dr.IsDBNull(iFechadia)) entity.Fechadia = dr.GetString(iFechadia);
                    int iTotalregdia = dr.GetOrdinal(helper.Totalregdia);
                    if (!dr.IsDBNull(iTotalregdia)) entity.Totalregdia = Convert.ToInt32(dr.GetValue(iTotalregdia));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
