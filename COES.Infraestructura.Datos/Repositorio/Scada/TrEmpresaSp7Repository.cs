using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_EMPRESA_SP7
    /// </summary>
    public class TrEmpresaSp7Repository : RepositoryBase, ITrEmpresaSp7Repository
    {
        public TrEmpresaSp7Repository(string strConn)
            : base(strConn)
        {
        }

        TrEmpresaSp7Helper helper = new TrEmpresaSp7Helper();

        public int Save(TrEmpresaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, entity.Emprenomb);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Emprsiid, DbType.Int32, entity.Emprsiid);
            dbProvider.AddInParameter(command, helper.Emprusucreacion, DbType.String, entity.Emprusucreacion);
            dbProvider.AddInParameter(command, helper.Empriccppri, DbType.String, entity.Empriccppri);
            dbProvider.AddInParameter(command, helper.Empriccpsec, DbType.String, entity.Empriccpsec);
            dbProvider.AddInParameter(command, helper.Empriccpconect, DbType.String, entity.Empriccpconect);
            dbProvider.AddInParameter(command, helper.Empriccplastdate, DbType.DateTime, entity.Empriccplastdate);
            dbProvider.AddInParameter(command, helper.Emprinvertrealq, DbType.String, entity.Emprinvertrealq);
            dbProvider.AddInParameter(command, helper.Emprinvertstateq, DbType.String, entity.Emprinvertstateq);
            dbProvider.AddInParameter(command, helper.Emprconec, DbType.String, entity.Emprconec);
            dbProvider.AddInParameter(command, helper.Linkcodi, DbType.Int32, entity.Linkcodi);
            dbProvider.AddInParameter(command, helper.Emprstateqgmt, DbType.String, entity.Emprstateqgmt);
            dbProvider.AddInParameter(command, helper.Emprrealqgmt, DbType.String, entity.Emprrealqgmt);
            dbProvider.AddInParameter(command, helper.Emprreenviar, DbType.String, entity.Emprreenviar);
            dbProvider.AddInParameter(command, helper.Emprlatencia, DbType.Int32, entity.Emprlatencia);
            dbProvider.AddInParameter(command, helper.Emprfeccreacion, DbType.DateTime, entity.Emprfeccreacion);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrEmpresaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, entity.Emprenomb);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Emprsiid, DbType.Int32, entity.Emprsiid);
            dbProvider.AddInParameter(command, helper.Emprusucreacion, DbType.String, entity.Emprusucreacion);
            dbProvider.AddInParameter(command, helper.Empriccppri, DbType.String, entity.Empriccppri);
            dbProvider.AddInParameter(command, helper.Empriccpsec, DbType.String, entity.Empriccpsec);
            dbProvider.AddInParameter(command, helper.Empriccpconect, DbType.String, entity.Empriccpconect);
            dbProvider.AddInParameter(command, helper.Empriccplastdate, DbType.DateTime, entity.Empriccplastdate);
            dbProvider.AddInParameter(command, helper.Emprinvertrealq, DbType.String, entity.Emprinvertrealq);
            dbProvider.AddInParameter(command, helper.Emprinvertstateq, DbType.String, entity.Emprinvertstateq);
            dbProvider.AddInParameter(command, helper.Emprconec, DbType.String, entity.Emprconec);
            dbProvider.AddInParameter(command, helper.Linkcodi, DbType.Int32, entity.Linkcodi);
            dbProvider.AddInParameter(command, helper.Emprstateqgmt, DbType.String, entity.Emprstateqgmt);
            dbProvider.AddInParameter(command, helper.Emprrealqgmt, DbType.String, entity.Emprrealqgmt);
            dbProvider.AddInParameter(command, helper.Emprreenviar, DbType.String, entity.Emprreenviar);
            dbProvider.AddInParameter(command, helper.Emprlatencia, DbType.Int32, entity.Emprlatencia);
            dbProvider.AddInParameter(command, helper.Emprfeccreacion, DbType.DateTime, entity.Emprfeccreacion);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrEmpresaSp7DTO GetById(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            TrEmpresaSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrEmpresaSp7DTO> List()
        {
            List<TrEmpresaSp7DTO> entitys = new List<TrEmpresaSp7DTO>();
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

        public List<TrEmpresaSp7DTO> GetByCriteria()
        {
            List<TrEmpresaSp7DTO> entitys = new List<TrEmpresaSp7DTO>();
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

        public void ActualizarNombreEmpresa(int emprcodi, string emprnomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarNombreEmpresa);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, emprnomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #region Mejoras IEOD

        public List<TrEmpresaSp7DTO> ListarEmpresaCanal()
        {
            List<TrEmpresaSp7DTO> entitys = new List<TrEmpresaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresaCanal);
            TrEmpresaSp7DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new TrEmpresaSp7DTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprenomb = dr.GetOrdinal(helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrEmpresaSp7DTO> ListarEmpresaCanalBdTreal()
        {
            List<TrEmpresaSp7DTO> entitys = new List<TrEmpresaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresaCanalBdTreal);
            TrEmpresaSp7DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new TrEmpresaSp7DTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprenomb = dr.GetOrdinal(helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
