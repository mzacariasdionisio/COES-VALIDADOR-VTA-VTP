using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_BARRA
    /// </summary>
    public class StBarraRepository : RepositoryBase, IStBarraRepository
    {
        public StBarraRepository(string strConn)
            : base(strConn)
        {
        }

        StBarraHelper helper = new StBarraHelper();

        public int Save(StBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stbarrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Stbarrusucreacion, DbType.String, entity.Stbarrusucreacion);
            dbProvider.AddInParameter(command, helper.Stbarrfeccreacion, DbType.DateTime, entity.Stbarrfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Stbarrusucreacion, DbType.String, entity.Stbarrusucreacion);
            dbProvider.AddInParameter(command, helper.Stbarrfeccreacion, DbType.DateTime, entity.Stbarrfeccreacion);
            dbProvider.AddInParameter(command, helper.Stbarrcodi, DbType.Int32, entity.Stbarrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stranscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stbarrcodi, DbType.Int32, stranscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteDstEleDet(int barrcodi, int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteDstEleDet);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<StBarraDTO> List(int strecacodi)
        {
            List<StBarraDTO> entitys = new List<StBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StBarraDTO entity = helper.Create(dr);

                    int iBarrnomb = dr.GetOrdinal(this.helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnomb)) entity.Barrnomb = dr.GetString(iBarrnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public StBarraDTO GetById(int stbarrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stbarrcodi, DbType.Int32, stbarrcodi);
            StBarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StBarraDTO> GetByCriteria(int strecacodi)
        {
            List<StBarraDTO> entitys = new List<StBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StBarraDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnomb = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //public StBarraDTO GetBySisBarrNomb(string BarrNomb)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBySisBarrNombre);

        //    dbProvider.AddInParameter(command, helper.Barrnomb, DbType.String, BarrNomb);
        //    StBarraDTO entity = null;

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        if (dr.Read())
        //        {
        //            entity = helper.Create(dr);
        //        }
        //    }

        //    return entity;
        //}
        public List<StBarraDTO> ListByStBarraVersion(int strecacodi)
        {
            List<StBarraDTO> entitys = new List<StBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStBarraVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StBarraDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       
      
    }
}
