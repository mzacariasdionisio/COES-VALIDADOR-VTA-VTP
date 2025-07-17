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
    /// Clase de acceso a datos de la tabla CAI_PORCTAPORTE
    /// </summary>
    public class CaiPorctaporteRepository: RepositoryBase, ICaiPorctaporteRepository
    {
        public CaiPorctaporteRepository(string strConn): base(strConn)
        {
        }

        CaiPorctaporteHelper helper = new CaiPorctaporteHelper();

        public int Save(CaiPorctaporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caipacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Caipaimpaporte, DbType.Decimal, entity.Caipaimpaporte);
            dbProvider.AddInParameter(command, helper.Caipapctaporte, DbType.Decimal, entity.Caipapctaporte);
            dbProvider.AddInParameter(command, helper.Caipausucreacion, DbType.String, entity.Caipausucreacion);
            dbProvider.AddInParameter(command, helper.Caipafeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiPorctaporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Caipaimpaporte, DbType.Decimal, entity.Caipaimpaporte);
            dbProvider.AddInParameter(command, helper.Caipapctaporte, DbType.Decimal, entity.Caipapctaporte);
            dbProvider.AddInParameter(command, helper.Caipausucreacion, DbType.String, entity.Caipausucreacion);
            dbProvider.AddInParameter(command, helper.Caipafeccreacion, DbType.DateTime, entity.Caipafeccreacion);
            dbProvider.AddInParameter(command, helper.Caipacodi, DbType.Int32, entity.Caipacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiPorctaporteDTO GetById(int caipacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caipacodi, DbType.Int32, caipacodi);
            CaiPorctaporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiPorctaporteDTO> List()
        {
            List<CaiPorctaporteDTO> entitys = new List<CaiPorctaporteDTO>();
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

        public List<CaiPorctaporteDTO> GetByCriteria()
        {
            List<CaiPorctaporteDTO> entitys = new List<CaiPorctaporteDTO>();
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

        public List<CaiPorctaporteDTO> ByEmpresaImporte(int caiajcodi)
        {
            List<CaiPorctaporteDTO> entitys = new List<CaiPorctaporteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlByEmpresaImporte);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiPorctaporteDTO entity = new CaiPorctaporteDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCaipaimpaporte = dr.GetOrdinal(this.helper.Caipaimpaporte);
                    if (!dr.IsDBNull(iCaipaimpaporte)) entity.Caipaimpaporte = dr.GetDecimal(iCaipaimpaporte);

            
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
    }
}
