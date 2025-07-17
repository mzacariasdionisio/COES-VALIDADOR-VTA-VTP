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
    /// Clase de acceso a datos de la tabla FW_AREA
    /// </summary>
    public class FwAreaRepository: RepositoryBase, IFwAreaRepository
    {
        public FwAreaRepository(string strConn): base(strConn)
        {
        }

        FwAreaHelper helper = new FwAreaHelper();

        public int Save(FwAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Areaname, DbType.String, entity.Areaname);
            dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            dbProvider.AddInParameter(command, helper.Flagreclamos, DbType.String, entity.Flagreclamos);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.Areapadre);
            dbProvider.AddInParameter(command, helper.Areaorder, DbType.Int32, entity.Areaorder);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FwAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Areaname, DbType.String, entity.Areaname);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            dbProvider.AddInParameter(command, helper.Flagreclamos, DbType.String, entity.Flagreclamos);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.Areapadre);
            dbProvider.AddInParameter(command, helper.Areaorder, DbType.Int32, entity.Areaorder);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int areacode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwAreaDTO GetById(int areacode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);
            FwAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public int GetDirResponsable(int areacode)
        {

            int dircode = 0;
            string sqlStr = string.Format(helper.SqlGetDirResp, areacode);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlStr);

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) //dircode = Convert.ToInt32(result);
            {
                if (!(result is DBNull))
                    dircode = Convert.ToInt32(result);
            }

            //dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);
            //FwAreaDTO entity;

            //using (IDataReader dr = dbProvider.ExecuteReader(command))
            //{
            //    if (dr.Read())
            //    {
            //        //entity = new FwAreaDTO();
            //        //entity = helper.Create(dr);
            //        int iDircode = dr.GetOrdinal(helper.Areacode);
            //        if (!dr.IsDBNull(iDircode)) dircode = Convert.ToInt32(dr.GetValue(iDircode));
            //    }
            //}

            return dircode;
        }

        public List<FwAreaDTO> List()
        {
            List<FwAreaDTO> entitys = new List<FwAreaDTO>();
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

        public List<FwAreaDTO> GetByCriteria(int idEmpresa)
        {
            List<FwAreaDTO> entitys = new List<FwAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, idEmpresa);
        
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FwAreaDTO> ListAreaXFormato(int idOrigen)
        {
            string sqlStr = string.Format(helper.SqlListAreaXFormato, idOrigen);
            List<FwAreaDTO> entitys = new List<FwAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlStr);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new FwAreaDTO();
                    entity.Areaname = dr.GetString(dr.GetOrdinal("Areaname"));
                    int iAreacode = dr.GetOrdinal("Areacode");
                    if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FwAreaDTO> ListarArea()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarArea);
            var areas = new List<FwAreaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                var area = new FwAreaDTO();
                while (dr.Read())
                {
                    area = new FwAreaDTO();

                    int iAreaname = dr.GetOrdinal(helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) area.Areaname = dr.GetString(iAreaname);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) area.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreacode = dr.GetOrdinal(helper.Areacode);
                    if (!dr.IsDBNull(iAreacode)) area.Areacode = dr.GetInt32(iAreacode);

                    areas.Add(area);
                }
            }

            return areas;
        }
    }
}
