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
    /// Clase de acceso a datos de la tabla CAI_SDDP_PARAMDIA
    /// </summary>

    public class CaiSddpParamdiaRepository : RepositoryBase, ICaiSddpParamdiaRepository
    {

        public CaiSddpParamdiaRepository(string strConn)
                : base(strConn)
            {
            }

        CaiSddpParamdiaHelper helper = new CaiSddpParamdiaHelper();

            public int Save(CaiSddpParamdiaDTO entity)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                int id = 1;
                if (result != null) id = Convert.ToInt32(result);

                command = dbProvider.GetSqlStringCommand(helper.SqlSave);

                dbProvider.AddInParameter(command, helper.Sddppdcodi, DbType.Int32, id);
                dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
                dbProvider.AddInParameter(command, helper.Sddppddia, DbType.DateTime, entity.Sddppddia);
                dbProvider.AddInParameter(command, helper.Sddppdlaboral, DbType.Int32, entity.Sddppdlaboral);
                dbProvider.AddInParameter(command, helper.Sddppdusucreacion, DbType.String, entity.Sddppdusucreacion);
                dbProvider.AddInParameter(command, helper.Sddppdfeccreacion, DbType.DateTime, entity.Sddppdfeccreacion);
                
                dbProvider.ExecuteNonQuery(command);
                return id;
            }

            public void Update(CaiSddpParamdiaDTO entity)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

                dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
                dbProvider.AddInParameter(command, helper.Sddppddia, DbType.DateTime, entity.Sddppddia);
                dbProvider.AddInParameter(command, helper.Sddppdlaboral, DbType.Int32, entity.Sddppdlaboral);
                dbProvider.AddInParameter(command, helper.Sddppdusucreacion, DbType.String, entity.Sddppdusucreacion);
                dbProvider.AddInParameter(command, helper.Sddppdfeccreacion, DbType.DateTime, entity.Sddppdfeccreacion);
                dbProvider.AddInParameter(command, helper.Sddppdcodi, DbType.Int32, entity.Sddppdcodi);

                dbProvider.ExecuteNonQuery(command);
            }

            public void Delete()
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

                dbProvider.ExecuteNonQuery(command);
            }

            public CaiSddpParamdiaDTO GetById(int caiajcodi)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

                dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
                CaiSddpParamdiaDTO entity = null;

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    if (dr.Read())
                    {
                        entity = helper.Create(dr);
                    }
                }

                return entity;
            }

            public List<CaiSddpParamdiaDTO> List(int Sddppicodi)
            {
                List<CaiSddpParamdiaDTO> entitys = new List<CaiSddpParamdiaDTO>();
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
                dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, Sddppicodi);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entitys.Add(helper.Create(dr));
                    }
                }

                return entitys;
            }

            public List<CaiSddpParamdiaDTO> GetByCriteria(int caiajcodi)
            {
                List<CaiSddpParamdiaDTO> entitys = new List<CaiSddpParamdiaDTO>();
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
                dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entitys.Add(helper.Create(dr));
                    }
                }

                return entitys;
            }

            public CaiSddpParamdiaDTO GetByDiaCaiSddpParamdia(DateTime sddppddia)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByDiaCaiSddpParamdia);

                dbProvider.AddInParameter(command, helper.Sddppddia, DbType.DateTime, sddppddia);
                CaiSddpParamdiaDTO entity = null;

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    if (dr.Read())
                    {
                        entity = helper.Create(dr);
                    }
                }

                return entity;
            }

        
        
    }
 }

