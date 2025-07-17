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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public class VtpPagoEgresoRepository: RepositoryBase, IVtpPagoEgresoRepository
    {
        public VtpPagoEgresoRepository(string strConn): base(strConn)
        {
        }

        VtpPagoEgresoHelper helper = new VtpPagoEgresoHelper();

        public int Save(VtpPagoEgresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pagegrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pagegregreso, DbType.Decimal, entity.Pagegregreso);
            dbProvider.AddInParameter(command, helper.Pagegrsaldo, DbType.Decimal, entity.Pagegrsaldo);
            dbProvider.AddInParameter(command, helper.Pagegrpagoegreso, DbType.Decimal, entity.Pagegrpagoegreso);
            dbProvider.AddInParameter(command, helper.Pagegrusucreacion, DbType.String, entity.Pagegrusucreacion);
            dbProvider.AddInParameter(command, helper.Pagegrfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPagoEgresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pagegregreso, DbType.Decimal, entity.Pagegregreso);
            dbProvider.AddInParameter(command, helper.Pagegrsaldo, DbType.Decimal, entity.Pagegrsaldo);
            dbProvider.AddInParameter(command, helper.Pagegrpagoegreso, DbType.Decimal, entity.Pagegrpagoegreso);
            dbProvider.AddInParameter(command, helper.Pagegrusucreacion, DbType.String, entity.Pagegrusucreacion);
            dbProvider.AddInParameter(command, helper.Pagegrfeccreacion, DbType.DateTime, entity.Pagegrfeccreacion);

            dbProvider.AddInParameter(command, helper.Pagegrcodi, DbType.Int32, entity.Pagegrcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pagegrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pagegrcodi, DbType.Int32, Pagegrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPagoEgresoDTO GetById(int Pagegrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pagegrcodi, DbType.Int32, Pagegrcodi);
            VtpPagoEgresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPagoEgresoDTO> List(int pericodi, int recpotcodi)
        {
            List<VtpPagoEgresoDTO> entitys = new List<VtpPagoEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPagoEgresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPagoEgresoDTO> GetByCriteria(int pericodi, int recpotcodi)
        {
            List<VtpPagoEgresoDTO> entitys = new List<VtpPagoEgresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPagoEgresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
