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
    /// Clase de acceso a datos de la tabla VTP_INGRESO_POTENCIA
    /// </summary>
    public class VtpIngresoPotenciaRepository : RepositoryBase, IVtpIngresoPotenciaRepository
    {
        public VtpIngresoPotenciaRepository(string strConn) : base(strConn)
        {
        }

        VtpIngresoPotenciaHelper helper = new VtpIngresoPotenciaHelper();

        public int Save(VtpIngresoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Potipcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Potipimporte, DbType.Decimal, entity.Potipimporte);
            dbProvider.AddInParameter(command, helper.Potipporcentaje, DbType.Decimal, entity.Potipporcentaje);
            dbProvider.AddInParameter(command, helper.Potipusucreacion, DbType.String, entity.Potipusucreacion);
            dbProvider.AddInParameter(command, helper.Potipfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Potipcodi, DbType.Int32, entity.Potipcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Potipimporte, DbType.Decimal, entity.Potipimporte);
            dbProvider.AddInParameter(command, helper.Potipporcentaje, DbType.Decimal, entity.Potipporcentaje);
            dbProvider.AddInParameter(command, helper.Potipusucreacion, DbType.String, entity.Potipusucreacion);
            dbProvider.AddInParameter(command, helper.Potipfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int potipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Potipcodi, DbType.Int32, potipcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoPotenciaDTO GetById(int potipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Potipcodi, DbType.Int32, potipcodi);
            VtpIngresoPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoPotenciaDTO> List()
        {
            List<VtpIngresoPotenciaDTO> entitys = new List<VtpIngresoPotenciaDTO>();
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

        public List<VtpIngresoPotenciaDTO> ListEmpresa(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotenciaDTO> entitys = new List<VtpIngresoPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotenciaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    //ASSETEC 20190627: muestra si la empresa tiene un saldo asignado de otro periodo
                    int iPotipsaldoanterior = dr.GetOrdinal(this.helper.Potipsaldoanterior);
                    if (!dr.IsDBNull(iPotipsaldoanterior)) entity.Potipsaldoanterior = dr.GetDecimal(iPotipsaldoanterior);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotenciaDTO> GetByCriteria()
        {
            List<VtpIngresoPotenciaDTO> entitys = new List<VtpIngresoPotenciaDTO>();
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
