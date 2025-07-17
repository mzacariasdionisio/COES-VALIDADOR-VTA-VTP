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
    /// Clase de acceso a datos de la tabla VTP_INGRESO_POTUNID_PROMD
    /// </summary>
    public class VtpIngresoPotUnidPromdRepository: RepositoryBase, IVtpIngresoPotUnidPromdRepository
    {
        public VtpIngresoPotUnidPromdRepository(string strConn): base(strConn)
        {
        }

        VtpIngresoPotUnidPromdHelper helper = new VtpIngresoPotUnidPromdHelper();

        public int Save(VtpIngresoPotUnidPromdDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inpuprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Inpuprimportepromd, DbType.Decimal, entity.Inpuprimportepromd);
            dbProvider.AddInParameter(command, helper.Inpuprusucreacion, DbType.String, entity.Inpuprusucreacion);
            dbProvider.AddInParameter(command, helper.Inpuprfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Inpuprunidadnomb, DbType.String, entity.Inpuprunidadnomb);
            dbProvider.AddInParameter(command, helper.Inpuprficticio, DbType.Int32, entity.Inpuprficticio);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoPotUnidPromdDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Inpuprcodi, DbType.Int32, entity.Inpuprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Inpuprimportepromd, DbType.Decimal, entity.Inpuprimportepromd);
            dbProvider.AddInParameter(command, helper.Inpuprusucreacion, DbType.String, entity.Inpuprusucreacion);
            dbProvider.AddInParameter(command, helper.Inpuprfeccreacion, DbType.DateTime, entity.Inpuprfeccreacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Inpuprunidadnomb, DbType.String, entity.Inpuprunidadnomb);
            dbProvider.AddInParameter(command, helper.Inpuprficticio, DbType.Int32, entity.Inpuprficticio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Inpuprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inpuprcodi, DbType.Int32, Inpuprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoPotUnidPromdDTO GetById(int Inpuprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inpuprcodi, DbType.Int32, Inpuprcodi);
            VtpIngresoPotUnidPromdDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoPotUnidPromdDTO> List()
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
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

        public List<VtpIngresoPotUnidPromdDTO> ListEmpresa(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidPromdDTO entity = new VtpIngresoPotUnidPromdDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                    if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> ListEmpresaCentral(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaCentral);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidPromdDTO entity = new VtpIngresoPotUnidPromdDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                    if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> ListEmpresaCentral2(int pericodi, int recpotcodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaCentralUnidad);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoPotUnidPromdDTO entity = new VtpIngresoPotUnidPromdDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iInpuprficticioi = dr.GetOrdinal(this.helper.Inpuprficticio);
                    if (!dr.IsDBNull(iInpuprficticioi)) entity.Inpuprficticio = Convert.ToInt32(dr.GetValue(iInpuprficticioi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                    if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                    int iInpuprunidadnombb = dr.GetOrdinal(this.helper.Inpuprunidadnomb);
                    if (!dr.IsDBNull(iInpuprunidadnombb)) entity.Inpuprunidadnomb = dr.GetString(iInpuprunidadnombb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> GetByCriteria()
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
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

        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByComparative(int pericodiini, int pericodifin, int emprcodi, int equicodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIngresoPotUnidPromdByComparative);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByComparativeUnique(int pericodiini, int pericodifin, int emprcodi, int equicodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIngresoPotUnidPromdByComparativeUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        private List<VtpIngresoPotUnidPromdDTO> GetListIngresoPotByComparative(IDataReader dr)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            while (dr.Read())
            {
                VtpIngresoPotUnidPromdDTO entity = new VtpIngresoPotUnidPromdDTO();

                int iInpuprcodi = dr.GetOrdinal(this.helper.Inpuprcodi);
                if (!dr.IsDBNull(iInpuprcodi)) entity.Inpuprcodi = Convert.ToInt32(dr.GetValue(iInpuprcodi));

                int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                int iRecpotcodi = dr.GetOrdinal(this.helper.Recpotcodi);
                if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

                int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                int iRecpotnombre = dr.GetOrdinal(this.helper.Recpotnombre);
                if (!dr.IsDBNull(iRecpotnombre)) entity.Recpotnombre = dr.GetString(iRecpotnombre);

                int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                int iPerianio = dr.GetOrdinal(this.helper.Perianio);
                if (!dr.IsDBNull(iPerianio)) entity.Perianio = Convert.ToInt32(dr.GetValue(iPerianio));

                int iPerimes = dr.GetOrdinal(this.helper.Perimes);
                if (!dr.IsDBNull(iPerimes)) entity.Perimes = Convert.ToInt32(dr.GetValue(iPerimes));

                int iPerianiomes = dr.GetOrdinal(this.helper.Perianiomes);
                if (!dr.IsDBNull(iPerianiomes)) entity.Perianiomes = Convert.ToInt32(dr.GetValue(iPerianiomes));

                int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                entitys.Add(entity);
            }
            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByCompHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIngresoPotUnidPromdByCompHist);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByCompHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpIngresoPotUnidPromdDTO> entitys = new List<VtpIngresoPotUnidPromdDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIngresoPotUnidPromdByCompHistUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        //CU21
        public VtpIngresoPotUnidPromdDTO GetByCentral(int pericodi, int recpotcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentral);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VtpIngresoPotUnidPromdDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VtpIngresoPotUnidPromdDTO();
                    /*
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    */
                    int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                    if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                }
            }
            return entity;
        }

        public VtpIngresoPotUnidPromdDTO GetByCentralSumUnidades(int pericodi, int recpotcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentralSumUnidades);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VtpIngresoPotUnidPromdDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VtpIngresoPotUnidPromdDTO();
                    /*
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    */
                    int iInpuprimportepromd = dr.GetOrdinal(this.helper.Inpuprimportepromd);
                    if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

                }
            }
            return entity;
        }
    }
}
