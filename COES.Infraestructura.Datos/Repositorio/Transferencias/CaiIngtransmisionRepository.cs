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
    /// Clase de acceso a datos de la tabla CAI_INGTRANSMISION
    /// </summary>
    public class CaiIngtransmisionRepository: RepositoryBase, ICaiIngtransmisionRepository
    {
        public CaiIngtransmisionRepository(string strConn): base(strConn)
        {
        }

        CaiIngtransmisionHelper helper = new CaiIngtransmisionHelper();

        public Int32 GetCodigoGenerado()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            Int32 id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CaiIngtransmisionDTO entity)
        {
            Int32 id = GetCodigoGenerado();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caitrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caitrcalidadinfo, DbType.String, entity.Caitrcalidadinfo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caitrmes, DbType.Int32, entity.Caitrmes);
            dbProvider.AddInParameter(command, helper.Caitringreso, DbType.Decimal, entity.Caitringreso);
            dbProvider.AddInParameter(command, helper.Caitrtipoinfo, DbType.String, entity.Caitrtipoinfo);
            dbProvider.AddInParameter(command, helper.Caitrusucreacion, DbType.String, entity.Caitrusucreacion);
            dbProvider.AddInParameter(command, helper.Caitrfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void SaveCaiIngtransmisionAsSelectMeMedicion1(int caitrcodi, int caiajcodi, string caitrcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlSaveAsSelectMeMedicion1, caitrcodi, caiajcodi, caitrcalidadinfo, tipodato, user, Formatcodi, Lectcodi, FechaInicio, FechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        

        public void Update(CaiIngtransmisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caitrcalidadinfo, DbType.String, entity.Caitrcalidadinfo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Caitrmes, DbType.Int32, entity.Caitrmes);
            dbProvider.AddInParameter(command, helper.Caitringreso, DbType.Decimal, entity.Caitringreso);
            dbProvider.AddInParameter(command, helper.Caitrtipoinfo, DbType.String, entity.Caitrtipoinfo);
            dbProvider.AddInParameter(command, helper.Caitrusucreacion, DbType.String, entity.Caitrusucreacion);
            dbProvider.AddInParameter(command, helper.Caitrfeccreacion, DbType.DateTime, entity.Caitrfeccreacion);
            dbProvider.AddInParameter(command, helper.Caitrcodi, DbType.Int32, entity.Caitrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiIngtransmisionDTO GetById(int caitrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caitrcodi, DbType.Int32, caitrcodi);
            CaiIngtransmisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiIngtransmisionDTO> List()
        {
            List<CaiIngtransmisionDTO> entitys = new List<CaiIngtransmisionDTO>();
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

        public List<CaiIngtransmisionDTO> GetByCriteria()
        {
            List<CaiIngtransmisionDTO> entitys = new List<CaiIngtransmisionDTO>();
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
