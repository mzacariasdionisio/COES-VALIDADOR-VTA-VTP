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
    /// Clase de acceso a datos de la tabla EVE_PTOINTERRUP
    /// </summary>
    public class EvePtointerrupRepository: RepositoryBase, IEvePtointerrupRepository
    {
        public EvePtointerrupRepository(string strConn): base(strConn)
        {
        }

        EvePtointerrupHelper helper = new EvePtointerrupHelper();

        public int Save(EvePtointerrupDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, entity.Ptoentregacodi);
            dbProvider.AddInParameter(command, helper.Ptointerrupnomb, DbType.String, entity.Ptointerrupnomb);
            dbProvider.AddInParameter(command, helper.Ptointerrupsectip, DbType.Int32, entity.Ptointerrupsectip);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EvePtointerrupDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, entity.Ptointerrcodi);
            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, entity.Ptoentregacodi);
            dbProvider.AddInParameter(command, helper.Ptointerrupnomb, DbType.String, entity.Ptointerrupnomb);
            dbProvider.AddInParameter(command, helper.Ptointerrupsectip, DbType.Int32, entity.Ptointerrupsectip);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptointerrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, ptointerrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EvePtointerrupDTO GetById(int ptointerrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, ptointerrcodi);
            EvePtointerrupDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EvePtointerrupDTO> List()
        {
            List<EvePtointerrupDTO> entitys = new List<EvePtointerrupDTO>();
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

        public List<EvePtointerrupDTO> GetByCriteria()
        {
            List<EvePtointerrupDTO> entitys = new List<EvePtointerrupDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EvePtointerrupDTO entity = helper.Create(dr);

                    int iPtoentrenomb = dr.GetOrdinal(this.helper.Ptoentrenomb);
                    if (!dr.IsDBNull(iPtoentrenomb)) entity.Ptoentrenomb = dr.GetString(iPtoentrenomb);
                                        
                    int iClientecodi = dr.GetOrdinal(this.helper.Clientecodi);
                    if (!dr.IsDBNull(iClientecodi)) entity.Clientecodi = Convert.ToInt32(dr.GetValue(iClientecodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if(!dr.IsDBNull(iEquicodi))entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
