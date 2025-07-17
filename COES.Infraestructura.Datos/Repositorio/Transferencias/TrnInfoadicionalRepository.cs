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
    /// Clase de acceso a datos de la tabla TRN_INFOADICIONAL
    /// </summary>
    public class TrnInfoadicionalRepository: RepositoryBase, ITrnInfoadicionalRepository
    {
        public TrnInfoadicionalRepository(string strConn): base(strConn)
        {
        }

        TrnInfoadicionalHelper helper = new TrnInfoadicionalHelper();

        public int Save(TrnInfoadicionalDTO entity)
        {
            int id;
            DbCommand command;
            if (entity.Infadicodi != 0)
            {
                id = entity.Infadicodi;
            }
            else
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                id = 1;
                if (result != null)id = Convert.ToInt32(result);
            }            

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Infadicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Infadinomb, DbType.String, entity.Infadinomb);
            dbProvider.AddInParameter(command, helper.Infadicodosinergmin, DbType.String, entity.Infadicodosinergmin);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Infadiestado, DbType.String, entity.Infadiestado);
            dbProvider.AddInParameter(command, helper.Usucreacion, DbType.String, entity.UsuCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrnInfoadicionalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Infadinomb, DbType.String, entity.Infadinomb);
            dbProvider.AddInParameter(command, helper.Infadicodosinergmin, DbType.String, entity.Infadicodosinergmin);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Usuupdate, DbType.String, entity.UsuUpdate);
            dbProvider.AddInParameter(command, helper.Infadicodi, DbType.Int32, entity.Infadicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int infadicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infadicodi, DbType.Int32, infadicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrnInfoadicionalDTO GetById(int infadicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infadicodi, DbType.Int32, infadicodi);
            TrnInfoadicionalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrnInfoadicionalDTO> List()
        {
            List<TrnInfoadicionalDTO> entitys = new List<TrnInfoadicionalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnInfoadicionalDTO entity = new TrnInfoadicionalDTO();
                    entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int IEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(IEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(IEmprcodosinergmin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<TrnInfoadicionalDTO> ListVersiones(int idConcepto)
        {
            List<TrnInfoadicionalDTO> entitys = new List<TrnInfoadicionalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVersion);

            dbProvider.AddInParameter(command, helper.Infadicodi, DbType.Int32, idConcepto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnInfoadicionalDTO entity = new TrnInfoadicionalDTO();
                    entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int IEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(IEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(IEmprcodosinergmin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnInfoadicionalDTO> GetByCriteria()
        {
            List<TrnInfoadicionalDTO> entitys = new List<TrnInfoadicionalDTO>();
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
    }
}
