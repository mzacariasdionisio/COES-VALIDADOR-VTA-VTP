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
    /// Clase de acceso a datos de la tabla EQ_GRUPO_LINEA
    /// </summary>
    public class EqGrupoLineaRepository: RepositoryBase, IEqGrupoLineaRepository
    {
        public EqGrupoLineaRepository(string strConn): base(strConn)
        {
        }

        EqGrupoLineaHelper helper = new EqGrupoLineaHelper();

        public int Save(EqGrupoLineaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grulinnombre, DbType.String, entity.Grulinnombre);
            dbProvider.AddInParameter(command, helper.Grulinvallintrans, DbType.Decimal, entity.Grulinvallintrans);
            dbProvider.AddInParameter(command, helper.Grulinporlimtrans, DbType.Decimal, entity.Grulinporlimtrans);
            dbProvider.AddInParameter(command, helper.Grulinestado, DbType.String, entity.Grulinestado);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Grulintipo, DbType.String, entity.Grulintipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqGrupoLineaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Grulinnombre, DbType.String, entity.Grulinnombre);
            dbProvider.AddInParameter(command, helper.Grulinvallintrans, DbType.Decimal, entity.Grulinvallintrans);
            dbProvider.AddInParameter(command, helper.Grulinporlimtrans, DbType.Decimal, entity.Grulinporlimtrans);
            dbProvider.AddInParameter(command, helper.Grulinestado, DbType.String, entity.Grulinestado);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Grulintipo, DbType.String, entity.Grulintipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grulincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, grulincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqGrupoLineaDTO GetById(int grulincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, grulincodi);
            EqGrupoLineaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqGrupoLineaDTO> List()
        {
            List<EqGrupoLineaDTO> entitys = new List<EqGrupoLineaDTO>();
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

        public List<EqGrupoLineaDTO> GetByCriteria(string tipo)
        {
            List<EqGrupoLineaDTO> entitys = new List<EqGrupoLineaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Grulintipo, DbType.String, tipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
