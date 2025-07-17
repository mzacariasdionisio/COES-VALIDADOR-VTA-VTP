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
    /// Clase de acceso a datos de la tabla EVE_EVENEQUIPO
    /// </summary>
    public class EveEvenequipoRepository: RepositoryBase, IEveEvenequipoRepository
    {
        public EveEvenequipoRepository(string strConn): base(strConn)
        {
        }

        EveEvenequipoHelper helper = new EveEvenequipoHelper();

        public void Save(EveEvenequipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EveEvenequipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evencodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEquipos(int evencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteEquipo);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);        

            dbProvider.ExecuteNonQuery(command);
        }

        public EveEvenequipoDTO GetById(int evencodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EveEvenequipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveEvenequipoDTO> List()
        {
            List<EveEvenequipoDTO> entitys = new List<EveEvenequipoDTO>();
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

        public List<EveEvenequipoDTO> GetByCriteria()
        {
            List<EveEvenequipoDTO> entitys = new List<EveEvenequipoDTO>();
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

        public List<EqEquipoDTO> GetEquiposPorEvento(string idEvento)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlObtenerEquiposPorEvento, idEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            EqEquipoHelper equipoHelper = new EqEquipoHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(equipoHelper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iTareaAbrev = dr.GetOrdinal(equipoHelper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iAreaNomb = dr.GetOrdinal(equipoHelper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(equipoHelper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iFamnomb = dr.GetOrdinal(equipoHelper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iDesCentral = dr.GetOrdinal(equipoHelper.DESCENTRAL);
                    if (!dr.IsDBNull(iDesCentral)) entity.DESCENTRAL = dr.GetString(iDesCentral);

                    int iEquiAbrev = dr.GetOrdinal(equipoHelper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquitension = dr.GetOrdinal(equipoHelper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iEquiCodi = dr.GetOrdinal(equipoHelper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(equipoHelper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iEmprcodi = dr.GetOrdinal(equipoHelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
