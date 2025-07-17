using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_PEAJE_EGRESO
    /// </summary>
    public class VtpPeajeEgresoRepository: RepositoryBase, IVtpPeajeEgresoRepository
    {
        public VtpPeajeEgresoRepository(string strConn): base(strConn)
        {
        }

        VtpPeajeEgresoHelper helper = new VtpPeajeEgresoHelper();

        public int Save(VtpPeajeEgresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pegrestado, DbType.String, entity.Pegrestado);
            dbProvider.AddInParameter(command, helper.Pegrplazo, DbType.String, entity.Pegrplazo);
            dbProvider.AddInParameter(command, helper.Pegrusucreacion, DbType.String, entity.Pegrusucreacion);
            dbProvider.AddInParameter(command, helper.Pegrfeccreacion, DbType.DateTime, entity.Pegrfeccreacion);
                  
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEgresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
         
            dbProvider.AddInParameter(command, helper.Pegrestado, DbType.String, entity.Pegrestado);
            dbProvider.AddInParameter(command, helper.Pegrusumodificacion, DbType.String, entity.Pegrusumodificacion);
            dbProvider.AddInParameter(command, helper.Pegrfecmodificacion, DbType.DateTime, entity.Pegrfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, entity.Pegrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pegrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEgresoDTO GetById(int pegrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pegrcodi, DbType.Int32, pegrcodi);
            VtpPeajeEgresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                }
            }

            return entity;
        }

        public List<VtpPeajeEgresoDTO> List(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoDTO> entitys = new List<VtpPeajeEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VtpPeajeEgresoDTO> ListView(int emprcodi, int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoDTO> entitys = new List<VtpPeajeEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListView);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VtpPeajeEgresoDTO> ObtenerReporteEnvioPorEmpresa(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEgresoDTO> entitys = new List<VtpPeajeEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerReportePorEmpresa);
            
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VtpPeajeEgresoDTO GetByCriteria(int emprcodi, int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            VtpPeajeEgresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void UpdateByCriteria(int emprcodi,int pericodi,int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateByCriteria);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
          
            dbProvider.ExecuteNonQuery(command);
        }

        public List<VtpPeajeEgresoDTO> ListConsulta(int pericodi, int recpotcodi, int emprcodi, string plazo, string liquidacion)
        {
            List<VtpPeajeEgresoDTO> entitys = new List<VtpPeajeEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListConsulta);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pegrplazo, DbType.String, plazo);
            dbProvider.AddInParameter(command, helper.Pegrplazo, DbType.String, plazo);
            dbProvider.AddInParameter(command, helper.Pegrestado, DbType.String, liquidacion);
            dbProvider.AddInParameter(command, helper.Pegrestado, DbType.String, liquidacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEgresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VtpPeajeEgresoDTO GetPreviusPeriod(int pericodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPreviusPeriod);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            VtpPeajeEgresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
