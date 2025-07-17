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
    /// Clase que contiene el mapeo de la tabla TRN_BARRA_URS
    /// </summary>
    public class TrnBarraUrsRepository : RepositoryBase, ITrnBarraUrsRepository
    {

        public TrnBarraUrsRepository(string strConn): base(strConn)
        {
        }

        TrnBarraUrsHelper helper = new TrnBarraUrsHelper();

        public int Save(TrnBarraursDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.EquiCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BarUrsusucreacion, DbType.String, entity.BarUrsUsuCreacion);
            dbProvider.AddInParameter(command, helper.BarUrsfeccreacion, DbType.DateTime, entity.BarUrsFecCreacion);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnBarraursDTO> List(int id)
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnBarraursDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnBarraursDTO> ListURS()
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListURS);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnBarraursDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnBarraursDTO> ListURSCostoMarginal(int pericodi, int vcrecacodi)
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListURSCostoMarginal);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnBarraursDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public TrnBarraursDTO GetById(int barrcodi, int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            TrnBarraursDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public TrnBarraursDTO GetByIdGrupoCodi(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdGrupoCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            TrnBarraursDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new TrnBarraursDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = Convert.ToString(dr.GetValue(iGruponomb));
                }
            }

            return entity;
        }

        public void Delete(int barrcodi, int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int barrcodi, int grupocodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.BarUrsusucreacion, DbType.String, username);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnBarraursDTO> GetEmpresas()
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnBarraursDTO entity = new TrnBarraursDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnBarraursDTO> ListbyEquicodi(int equicodi)
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListURSbyEquicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnBarraursDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Implementaciones para la tabla PR_GRUPO
        public TrnBarraursDTO GetByNombrePrGrupo(string grupocnomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombrePrGrupo);

            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, grupocnomb);
            TrnBarraursDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new TrnBarraursDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = Convert.ToString(dr.GetValue(iGruponomb));
                }
            }

            return entity;
        }

        public List<TrnBarraursDTO> ListPrGrupo()
        {
            List<TrnBarraursDTO> entitys = new List<TrnBarraursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPrGrupo);
            TrnBarraursDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new TrnBarraursDTO();
                    //TrnBarraursDTO urs = helper.Create(dr); 
                   
                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = Convert.ToString(dr.GetValue(iGruponomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public TrnBarraursDTO GetByIdGrupoCodiTRN(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByGrupoCodiTRN);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            TrnBarraursDTO entity = null;

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
