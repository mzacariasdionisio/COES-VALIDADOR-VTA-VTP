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
    /// Clase de acceso a datos de la tabla VTP_INGRESO_POTUNID_INTERVL
    /// </summary>
    public class VtpIngresoPotUnidIntervlRepository: RepositoryBase, IVtpIngresoPotUnidIntervlRepository
    {
        public VtpIngresoPotUnidIntervlRepository(string strConn): base(strConn)
        {
        }

        VtpIngresoPotUnidIntervlHelper helper = new VtpIngresoPotUnidIntervlHelper();

        public int Save(VtpIngresoPotUnidIntervlDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inpuincodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, entity.Ipefrcodi);
            dbProvider.AddInParameter(command, helper.Inpuinintervalo, DbType.Int32, entity.Inpuinintervalo);
            dbProvider.AddInParameter(command, helper.Inpuindia, DbType.Int32, entity.Inpuindia);
            dbProvider.AddInParameter(command, helper.Inpuinimporte, DbType.Decimal, entity.Inpuinimporte);
            dbProvider.AddInParameter(command, helper.Inpuinusucreacion, DbType.String, entity.Inpuinusucreacion);
            dbProvider.AddInParameter(command, helper.Inpuinfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Inpuinunidadnomb, DbType.String, entity.Inpuinunidadnomb);
            dbProvider.AddInParameter(command, helper.Inpuinficticio, DbType.Int32, entity.Inpuinficticio);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoPotUnidIntervlDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Inpuincodi, DbType.Int32, entity.Inpuincodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, entity.Ipefrcodi);
            dbProvider.AddInParameter(command, helper.Inpuinintervalo, DbType.Int32, entity.Inpuinintervalo);
            dbProvider.AddInParameter(command, helper.Inpuindia, DbType.Int32, entity.Inpuindia);
            dbProvider.AddInParameter(command, helper.Inpuinimporte, DbType.Decimal, entity.Inpuinimporte);
            dbProvider.AddInParameter(command, helper.Inpuinusucreacion, DbType.String, entity.Inpuinusucreacion);
            dbProvider.AddInParameter(command, helper.Inpuinfeccreacion, DbType.DateTime, entity.Inpuinfeccreacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Inpuinunidadnomb, DbType.String, entity.Inpuinunidadnomb);
            dbProvider.AddInParameter(command, helper.Inpuinficticio, DbType.Int32, entity.Inpuinficticio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Inpuincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inpuincodi, DbType.Int32, Inpuincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoPotUnidIntervlDTO GetById(int Inpuincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inpuincodi, DbType.Int32, Inpuincodi);
            VtpIngresoPotUnidIntervlDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoPotUnidIntervlDTO> List()
        {
            List<VtpIngresoPotUnidIntervlDTO> entitys = new List<VtpIngresoPotUnidIntervlDTO>();
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

        public List<VtpIngresoPotUnidIntervlDTO> ListSumIntervl(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotUnidIntervlDTO> entitys = new List<VtpIngresoPotUnidIntervlDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSumIntervl);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidIntervlDTO entity = new VtpIngresoPotUnidIntervlDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iInpuinunidadnomb = dr.GetOrdinal(this.helper.Inpuinunidadnomb);
                    if (!dr.IsDBNull(iInpuinunidadnomb)) entity.Inpuinunidadnomb = dr.GetString(iInpuinunidadnomb);

                    int iInpuinficticio = dr.GetOrdinal(this.helper.Inpuinficticio);
                    if (!dr.IsDBNull(iInpuinficticio)) entity.Inpuinficticio = Convert.ToInt32(dr.GetValue(iInpuinficticio));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iInpuinimporte = dr.GetOrdinal(this.helper.Inpuinimporte);
                    if (!dr.IsDBNull(iInpuinimporte)) entity.Inpuinimporte = dr.GetDecimal(iInpuinimporte);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidIntervlDTO> ListSumIntervlEmpresa(int pericodi, int recpotcodi, int emprcodi, int ipefrcodi)
        {
            List<VtpIngresoPotUnidIntervlDTO> entitys = new List<VtpIngresoPotUnidIntervlDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSumIntervlEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidIntervlDTO entity = new VtpIngresoPotUnidIntervlDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iInpuinimporte = dr.GetOrdinal(this.helper.Inpuinimporte);
                    if (!dr.IsDBNull(iInpuinimporte)) entity.Inpuinimporte = dr.GetDecimal(iInpuinimporte);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidIntervlDTO> GetByCriteria(int pericodi, int recpotcodi, int emprcodi, int equicodi, int ipefrcodi)
        {
            List<VtpIngresoPotUnidIntervlDTO> entitys = new List<VtpIngresoPotUnidIntervlDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Ipefrcodi, DbType.Int32, ipefrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidIntervlDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }
    }
}
