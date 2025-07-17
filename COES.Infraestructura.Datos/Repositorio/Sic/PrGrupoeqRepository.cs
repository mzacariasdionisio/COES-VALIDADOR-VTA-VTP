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
    /// Clase de acceso a datos de la tabla PR_GRUPOEQ
    /// </summary>
    public class PrGrupoeqRepository : RepositoryBase, IPrGrupoeqRepository
    {
        public PrGrupoeqRepository(string strConn) : base(strConn)
        {
        }

        PrGrupoeqHelper helper = new PrGrupoeqHelper();

        public int Save(PrGrupoeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Geqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Geqfeccreacion, DbType.DateTime, entity.Geqfeccreacion);
            dbProvider.AddInParameter(command, helper.Gequsucreacion, DbType.String, entity.Gequsucreacion);
            dbProvider.AddInParameter(command, helper.Geqfecmodificacion, DbType.DateTime, entity.Geqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Gequsumodificacion, DbType.String, entity.Gequsumodificacion);
            dbProvider.AddInParameter(command, helper.Geqactivo, DbType.Int32, entity.Geqactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrGrupoeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Geqfeccreacion, DbType.DateTime, entity.Geqfeccreacion);
            dbProvider.AddInParameter(command, helper.Gequsucreacion, DbType.String, entity.Gequsucreacion);
            dbProvider.AddInParameter(command, helper.Geqfecmodificacion, DbType.DateTime, entity.Geqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Gequsumodificacion, DbType.String, entity.Gequsumodificacion);
            dbProvider.AddInParameter(command, helper.Geqactivo, DbType.Int32, entity.Geqactivo);

            dbProvider.AddInParameter(command, helper.Geqcodi, DbType.Int32, entity.Geqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int geqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Geqcodi, DbType.Int32, geqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupoeqDTO GetById(int geqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Geqcodi, DbType.Int32, geqcodi);
            PrGrupoeqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGrupoeqDTO> List()
        {
            List<PrGrupoeqDTO> entitys = new List<PrGrupoeqDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iGrupotipomodo = dr.GetOrdinal(this.helper.Grupotipomodo);
                    if (!dr.IsDBNull(iGrupotipomodo)) entity.Grupotipomodo = dr.GetString(iGrupotipomodo);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoeqDTO> GetByCriteria(int grupocodi, int equipadre)
        {
            List<PrGrupoeqDTO> entitys = new List<PrGrupoeqDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, grupocodi, equipadre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiestado = dr.GetOrdinal(helper.Equiestado);
                    if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.Grupoestado = dr.GetString(iGrupoEstado);
                    int iGrupotipomodo = dr.GetOrdinal(this.helper.Grupotipomodo);
                    if (!dr.IsDBNull(iGrupotipomodo)) entity.Grupotipomodo = dr.GetString(iGrupotipomodo);
                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
