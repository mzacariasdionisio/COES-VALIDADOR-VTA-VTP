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
    /// Clase de acceso a datos de la tabla ME_MENSAJE
    /// </summary>
    public class MeMensajeRepository : RepositoryBase, IMeMensajeRepository
    {
        public MeMensajeRepository(string strConn)
            : base(strConn)
        {
        }

        MeMensajeHelper helper = new MeMensajeHelper();

        public int Save(MeMensajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Msjcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Msjdescripcion, DbType.String, entity.Msjdescripcion);
            dbProvider.AddInParameter(command, helper.Msjusucreacion, DbType.String, entity.Msjusucreacion);
            dbProvider.AddInParameter(command, helper.Msjfeccreacion, DbType.DateTime, entity.Msjfeccreacion);
            dbProvider.AddInParameter(command, helper.Msjusumodificacion, DbType.String, entity.Msjusumodificacion);
            dbProvider.AddInParameter(command, helper.Msjfecmodificacion, DbType.DateTime, entity.Msjfecmodificacion);
            dbProvider.AddInParameter(command, helper.Msjfecperiodo, DbType.DateTime, entity.Msjfecperiodo);
            dbProvider.AddInParameter(command, helper.Msjestado, DbType.String, entity.Msjestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DateTime fechaPeriodo, int emprcodi, int formatcodi, int userEmpresa)
        {
            string queryString = string.Format(helper.SqlUpdate, fechaPeriodo.ToString(ConstantesBase.FormatoFechaMes), emprcodi, formatcodi, userEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeMensajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Msjusumodificacion, DbType.String, entity.Msjusumodificacion);
            dbProvider.AddInParameter(command, helper.Msjfecmodificacion, DbType.DateTime, entity.Msjfecmodificacion);
            dbProvider.AddInParameter(command, helper.Msjestado, DbType.String, entity.Msjestado);

            dbProvider.AddInParameter(command, helper.Msjcodi, DbType.Int32, entity.Msjcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMensajeDTO GetById(int msjcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Msjcodi, DbType.Int32, msjcodi);
            MeMensajeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMensajeDTO> GetListaMensajes(string formatcodi, string emprcodi, DateTime fechaPeriodo)
        {
            List<MeMensajeDTO> entitys = new List<MeMensajeDTO>();
            string queryString = string.Format(helper.SqlGetListaMensajes, formatcodi, emprcodi, fechaPeriodo.ToString(ConstantesBase.FormatoFechaMes));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMensajeDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMensajeDTO> GetListaMensajes(DateTime fechaPeriodo, string idsEmpresa)
        {
            List<MeMensajeDTO> entitys = new List<MeMensajeDTO>();
            string queryString = string.Format(helper.SqlGetListaTodosMensajes, fechaPeriodo.ToString(ConstantesBase.FormatoFechaMes), idsEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMensajeDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}
