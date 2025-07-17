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
    /// Clase de acceso a datos de la tabla TRN_MODELO_ENVIO
    /// </summary>
    public class TrnModeloEnvioRepository: RepositoryBase, ITrnModeloEnvioRepository
    {
        public TrnModeloEnvioRepository(string strConn): base(strConn)
        {
        }

        TrnModeloEnvioHelper helper = new TrnModeloEnvioHelper();

        public int Save(TrnModeloEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Modenvversion, DbType.Int32, entity.Modenvversion);
            dbProvider.AddInParameter(command, helper.Modenvusuario, DbType.String, entity.Modenvusuario);
            dbProvider.AddInParameter(command, helper.Modenvfecenvio, DbType.DateTime, entity.Modenvfecenvio);
            dbProvider.AddInParameter(command, helper.Modenvestado, DbType.String, entity.Modenvestado);
            dbProvider.AddInParameter(command, helper.Modenvextension, DbType.String, entity.Modenvextension);
            dbProvider.AddInParameter(command, helper.Modendfile, DbType.String, entity.Modendfile);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.String, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Trnmodcodi, DbType.String, entity.Trnmodcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnModeloEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Modenvversion, DbType.Int32, entity.Modenvversion);
            dbProvider.AddInParameter(command, helper.Modenvusuario, DbType.String, entity.Modenvusuario);
            dbProvider.AddInParameter(command, helper.Modenvfecenvio, DbType.DateTime, entity.Modenvfecenvio);
            dbProvider.AddInParameter(command, helper.Modenvestado, DbType.String, entity.Modenvestado);
            dbProvider.AddInParameter(command, helper.Modenvextension, DbType.String, entity.Modenvextension);
            dbProvider.AddInParameter(command, helper.Modendfile, DbType.String, entity.Modendfile);
            dbProvider.AddInParameter(command, helper.Modenvcodi, DbType.Int32, entity.Modenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int modenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modenvcodi, DbType.Int32, modenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrnModeloEnvioDTO GetById(int modenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modenvcodi, DbType.Int32, modenvcodi);
            TrnModeloEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnModeloEnvioDTO> List()
        {
            List<TrnModeloEnvioDTO> entitys = new List<TrnModeloEnvioDTO>();
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

        public List<TrnModeloEnvioDTO> GetByCriteria(int empresa, int periodo, int version)
        {
            List<TrnModeloEnvioDTO> entitys = new List<TrnModeloEnvioDTO>();
            string query = string.Format(helper.SqlGetByCriteria, empresa, periodo, version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnModeloEnvioDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPerinomb = dr.GetOrdinal(helper.Perinomb);
                    if (!dr.IsDBNull(iPerinomb)) entity.Perinomb = dr.GetString(iPerinomb);

                    int iVersionnomb = dr.GetOrdinal(helper.Versionnomb);
                    if (!dr.IsDBNull(iVersionnomb)) entity.Versionnomb = dr.GetString(iVersionnomb);

                    int iTrnmodnombe = dr.GetOrdinal(helper.Trnmodnombre);
                    if (!dr.IsDBNull(iTrnmodnombe)) entity.Trnmodnombre = dr.GetString(iTrnmodnombe);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
