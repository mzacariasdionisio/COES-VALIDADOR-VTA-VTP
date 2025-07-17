using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnVersionRepository: RepositoryBase, IPrnVersionRepository
    {
        public PrnVersionRepository(string strConn)
       : base(strConn)
        {
        }

        PrnVersionHelper helper = new PrnVersionHelper();

        public List<PrnVersionDTO> List()
        {
            List<PrnVersionDTO> entitys = new List<PrnVersionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersionDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prnvernomb, DbType.String, entity.Prnvernomb);
            dbProvider.AddInParameter(command, helper.Prnverestado, DbType.String, entity.Prnverestado);
            dbProvider.AddInParameter(command, helper.Prnverusucreacion, DbType.String, entity.Prnverusucreacion);
            dbProvider.AddInParameter(command, helper.Prnverfeccreacion, DbType.DateTime, entity.Prnverfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }
        public void Delete(int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate );

            dbProvider.AddInParameter(command, helper.Prnvernomb, DbType.String, entity.Prnvernomb);
            dbProvider.AddInParameter(command, helper.Prnverestado, DbType.String, entity.Prnverestado);
            dbProvider.AddInParameter(command, helper.Prnverusumodificacion, DbType.String, entity.Prnverusumodificacion);
            dbProvider.AddInParameter(command, helper.Prnverfecmodificacion, DbType.DateTime, entity.Prnverfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, entity.Prnvercodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnVersionDTO GetById(int codigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, codigo);
            PrnVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnVersionDTO> GetModeloActivo(string idBarra)
        {
            List<PrnVersionDTO> entitys = new List<PrnVersionDTO>();
            string query = string.Format(helper.SqlGetModeloActivo, idBarra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersionDTO entity = new PrnVersionDTO();

                    int iPrnredbarracp = dr.GetOrdinal(helper.Prnredbarracp);
                    if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrnredbarrapm = dr.GetOrdinal(helper.Prnredbarrapm);
                    if (!dr.IsDBNull(iPrnredbarrapm)) entity.Prnredbarrapm = Convert.ToInt32(dr.GetValue(iPrnredbarrapm));

                    int iPrnredgauss = dr.GetOrdinal(helper.Prnredgauss);
                    if (!dr.IsDBNull(iPrnredgauss)) entity.Prnredgauss = dr.GetDecimal(iPrnredgauss);

                    int iPrnredperdida = dr.GetOrdinal(helper.Prnredperdida);
                    if (!dr.IsDBNull(iPrnredperdida)) entity.Prnredperdida = dr.GetDecimal(iPrnredperdida);

                    int iPrnredtipo = dr.GetOrdinal(helper.Prnredtipo);
                    if (!dr.IsDBNull(iPrnredtipo)) entity.Prnredtipo = dr.GetString(iPrnredtipo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateAllVersionInactivo(string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateAllVersionInactivo);

            dbProvider.AddInParameter(command, helper.Prnverestado, DbType.String, estado);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
