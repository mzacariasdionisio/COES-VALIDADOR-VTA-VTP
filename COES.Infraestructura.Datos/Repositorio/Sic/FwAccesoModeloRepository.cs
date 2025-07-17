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
    /// Clase de acceso a datos de la tabla FW_ACCESO_MODELO
    /// </summary>
    public class FwAccesoModeloRepository: RepositoryBase, IFwAccesoModeloRepository
    {
        public FwAccesoModeloRepository(string strConn): base(strConn)
        {
        }

        FwAccesoModeloHelper helper = new FwAccesoModeloHelper();

        public int Save(FwAccesoModeloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Acmodcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Acmodfecinicio, DbType.DateTime, entity.Acmodfecinicio);
            dbProvider.AddInParameter(command, helper.Acmodfin, DbType.DateTime, entity.Acmodfin);
            dbProvider.AddInParameter(command, helper.Acmodestado, DbType.String, entity.Acmodestado);
            dbProvider.AddInParameter(command, helper.Acmodkey, DbType.String, entity.Acmodkey);
            dbProvider.AddInParameter(command, helper.Acmodnrointentos, DbType.Int32, entity.Acmodnrointentos);
            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, entity.Empcorcodi);
            dbProvider.AddInParameter(command, helper.Acmodusucreacion, DbType.String, entity.Acmodusucreacion);
            dbProvider.AddInParameter(command, helper.Acmodfeccreacion, DbType.DateTime, entity.Acmodfeccreacion);
            dbProvider.AddInParameter(command, helper.Acmodusumodificacion, DbType.String, entity.Acmodusumodificacion);
            dbProvider.AddInParameter(command, helper.Acmodfecmodificacion, DbType.DateTime, entity.Acmodfecmodificacion);
            dbProvider.AddInParameter(command, helper.Acmodveces, DbType.Int32, entity.Acmodveces);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FwAccesoModeloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, entity.Empcorcodi);            
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateClave(FwAccesoModeloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateClave);

            dbProvider.AddInParameter(command, helper.Acmodkey, DbType.String, entity.Acmodkey);
            dbProvider.AddInParameter(command, helper.Acmodusumodificacion, DbType.String, entity.Acmodusumodificacion);
            dbProvider.AddInParameter(command, helper.Acmodfecmodificacion, DbType.DateTime, entity.Acmodfecmodificacion);
            dbProvider.AddInParameter(command, helper.Acmodcodi, DbType.Int32, entity.Acmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int acmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Acmodcodi, DbType.Int32, acmodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminarPorContacto(int idEmpresaCorreo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePorContacto);

            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, idEmpresaCorreo);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwAccesoModeloDTO GetById(int idEmpresa, int idModulo, int idEmpresaCorreo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, idModulo);
            dbProvider.AddInParameter(command, helper.Empcorcodi, DbType.Int32, idEmpresaCorreo);
            FwAccesoModeloDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwAccesoModeloDTO> List()
        {
            List<FwAccesoModeloDTO> entitys = new List<FwAccesoModeloDTO>();
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

        public List<FwAccesoModeloDTO> GetByCriteria(int idEmpresa, int idModulo)
        {
            List<FwAccesoModeloDTO> entitys = new List<FwAccesoModeloDTO>();
            string sql = String.Format(helper.SqlGetByCriteria, idEmpresa, idModulo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FwAccesoModeloDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iModnomb = dr.GetOrdinal(helper.Modnomb);
                    if (!dr.IsDBNull(iModnomb)) entity.Modnomb = dr.GetString(iModnomb);

                    int iContactonomb = dr.GetOrdinal(helper.Contactonomb);
                    if (!dr.IsDBNull(iContactonomb)) entity.Contactonomb = dr.GetString(iContactonomb);

                    int iContactocorreo = dr.GetOrdinal(helper.Contactocorreo);
                    if (!dr.IsDBNull(iContactocorreo)) entity.Contactocorreo = dr.GetString(iContactocorreo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
