using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_EVENTO
    /// </summary>
    public class IndEventoRepository : RepositoryBase, IIndEventoRepository
    {
        public IndEventoRepository(string strConn) : base(strConn)
        {
        }

        IndEventoHelper helper = new IndEventoHelper();

        public int Save(IndEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ieventcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ieventtipoindisp, DbType.String, entity.Ieventtipoindisp);
            dbProvider.AddInParameter(command, helper.Ieventpr, DbType.Decimal, entity.Ieventpr);
            dbProvider.AddInParameter(command, helper.Ieventusucreacion, DbType.String, entity.Ieventusucreacion);
            dbProvider.AddInParameter(command, helper.Ieventfeccreacion, DbType.DateTime, entity.Ieventfeccreacion);
            dbProvider.AddInParameter(command, helper.Ieventusumodificacion, DbType.String, entity.Ieventusumodificacion);
            dbProvider.AddInParameter(command, helper.Ieventfecmodificacion, DbType.DateTime, entity.Ieventfecmodificacion);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Ieventcomentario, DbType.String, entity.Ieventcomentario);
            dbProvider.AddInParameter(command, helper.Ieventestado, DbType.String, entity.Ieventestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ieventusumodificacion, DbType.String, entity.Ieventusumodificacion);
            dbProvider.AddInParameter(command, helper.Ieventfecmodificacion, DbType.DateTime, entity.Ieventfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ieventestado, DbType.String, entity.Ieventestado);
            dbProvider.AddInParameter(command, helper.Ieventcodi, DbType.Int32, entity.Ieventcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ieventcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ieventcodi, DbType.Int32, ieventcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndEventoDTO GetById(int ieventcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ieventcodi, DbType.Int32, ieventcodi);
            IndEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndEventoDTO> List()
        {
            List<IndEventoDTO> entitys = new List<IndEventoDTO>();
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

        public List<IndEventoDTO> ListHistoricoByEvencodi(int evencodi)
        {
            List<IndEventoDTO> entitys = new List<IndEventoDTO>();
            var sql = string.Format(helper.SqlListHistoricoByEvencodi, evencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndEventoDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndEventoDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string idsEmpresa, string idsTipoEquipo, string idstipoMantto)
        {
            List<IndEventoDTO> entitys = new List<IndEventoDTO>();
            var sql = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                                                            , idsEmpresa, idsTipoEquipo, idstipoMantto);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndEventoDTO entity = helper.Create(dr);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);
                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);
                    int iEvenasunto = dr.GetOrdinal(helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));
                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
