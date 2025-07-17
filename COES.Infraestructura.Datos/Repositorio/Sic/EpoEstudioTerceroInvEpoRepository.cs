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
    /// Clase de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioTerceroInvEpoRepository : RepositoryBase, IEpoEstudioTerceroInvEpoRepository
    {
        string sConn = string.Empty;
        public EpoEstudioTerceroInvEpoRepository(string strConn) : base(strConn)
        {
            sConn = strConn;
        }

        EpoEstudioTerceroInvEpoHelper helper = new EpoEstudioTerceroInvEpoHelper();

        public int Save(EpoEstudioTerceroInvEpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Invepocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, entity.Estepocodi);
            dbProvider.AddInParameter(command, helper.Estepoemprcodi, DbType.Int32, entity.Estepoemprcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int estepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, estepocodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<EpoEstudioTerceroInvEpoDTO> GetByCriteria(int estepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, estepocodi);

            List<EpoEstudioTerceroInvEpoDTO> entitys = new List<EpoEstudioTerceroInvEpoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioTerceroInvEpoDTO entity = helper.Create(dr);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
