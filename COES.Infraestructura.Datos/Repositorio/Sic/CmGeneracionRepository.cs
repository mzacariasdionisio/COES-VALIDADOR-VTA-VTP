using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CM_GENERACION
    /// </summary>
    public class CmGeneracionRepository : RepositoryBase, ICmGeneracionRepository
    {
        public CmGeneracionRepository(string strConn) : base(strConn)
        {
        }

        CmGeneracionHelper helper = new CmGeneracionHelper();

        public int Save(CmGeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Genefecha, DbType.DateTime, entity.Genefecha);
            dbProvider.AddInParameter(command, helper.Geneintervalo, DbType.Int32, entity.Geneintervalo);
            dbProvider.AddInParameter(command, helper.Genevalor, DbType.Decimal, entity.Genevalor);
            dbProvider.AddInParameter(command, helper.Genesucreacion, DbType.String, entity.Genesucreacion);
            dbProvider.AddInParameter(command, helper.Genefeccreacion, DbType.DateTime, entity.Genefeccreacion);
            dbProvider.AddInParameter(command, helper.Geneusumodificacion, DbType.String, entity.Geneusumodificacion);
            dbProvider.AddInParameter(command, helper.Genefecmodificacion, DbType.DateTime, entity.Genefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmGeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Genecodi, DbType.Int32, entity.Genecodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Genefecha, DbType.DateTime, entity.Genefecha);
            dbProvider.AddInParameter(command, helper.Geneintervalo, DbType.Int32, entity.Geneintervalo);
            dbProvider.AddInParameter(command, helper.Genevalor, DbType.Decimal, entity.Genevalor);
            dbProvider.AddInParameter(command, helper.Genesucreacion, DbType.String, entity.Genesucreacion);
            dbProvider.AddInParameter(command, helper.Genefeccreacion, DbType.DateTime, entity.Genefeccreacion);
            dbProvider.AddInParameter(command, helper.Geneusumodificacion, DbType.String, entity.Geneusumodificacion);
            dbProvider.AddInParameter(command, helper.Genefecmodificacion, DbType.DateTime, entity.Genefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Genecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genecodi, DbType.Int32, Genecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmGeneracionDTO GetById(int Genecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genecodi, DbType.Int32, Genecodi);
            CmGeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmGeneracionDTO> List()
        {
            List<CmGeneracionDTO> entitys = new List<CmGeneracionDTO>();
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

        public List<CmGeneracionDTO> GetByCriteria()
        {
            List<CmGeneracionDTO> entitys = new List<CmGeneracionDTO>();
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

        public void DeleteByCriteria(int intervalo, DateTime fecha)
        {            
            string sql = string.Format(helper.SqlDeleteByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), intervalo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CmGeneracionDTO> ListByEmpresa(int emprcodi, DateTime fecha)
        {
            List<CmGeneracionDTO> entitys = new List<CmGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlListByEmpresa, fecha.ToString(ConstantesBase.FormatoFecha)));

            dbProvider.AddInParameter(command, "EMPRCODI", DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmGeneracionDTO entity = new CmGeneracionDTO();

                    int iGenevalor = dr.GetOrdinal(helper.Genevalor);
                    if (!dr.IsDBNull(iGenevalor)) entity.Genevalor = dr.GetDecimal(iGenevalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal ProducionEnergiaByDate(DateTime date)
        {
            decimal prod = 0;

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlProducionEnergiaByDate, date.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {

                    int iGenevalor = dr.GetOrdinal(helper.Genevalor);
                    if (!dr.IsDBNull(iGenevalor)) prod = dr.GetDecimal(iGenevalor);
                }
            }

            return prod;
        }
    }
}
