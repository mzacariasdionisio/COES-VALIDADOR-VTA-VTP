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
    /// Clase de acceso a datos de la tabla EPO_REVISION_EPO
    /// </summary>
    public class EpoRevisionEpoRepository : RepositoryBase, IEpoRevisionEpoRepository
    {
        public EpoRevisionEpoRepository(string strConn) : base(strConn)
        {
        }

        EpoRevisionEpoHelper helper = new EpoRevisionEpoHelper();

        public int Save(EpoRevisionEpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Revepocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, entity.Estepocodi);
            dbProvider.AddInParameter(command, helper.Reveporevcoesfechaini, DbType.String, entity.StrReveporevcoesfechaini);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisiontit, DbType.String, entity.Reveporevcoescartarevisiontit);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisionenl, DbType.String, entity.Reveporevcoescartarevisionenl);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisionobs, DbType.String, entity.Reveporevcoescartarevisionobs);
            dbProvider.AddInParameter(command, helper.Reveporevcoesfinalizado, DbType.String, entity.Reveporevcoesfinalizado);
            dbProvider.AddInParameter(command, helper.Revepocoesfechafin, DbType.String, entity.StrRevepocoesfechafin);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvfechaini, DbType.String, entity.StrRevepoenvesttercinvfechaini);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvtit, DbType.String, entity.Revepoenvesttercinvtit);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvenl, DbType.String, entity.Revepoenvesttercinvenl);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvobs, DbType.String, entity.Revepoenvesttercinvobs);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvfinalizado, DbType.String, entity.Revepoenvesttercinvfinalizado);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvinvfechafin, DbType.String, entity.StrRevepoenvesttercinvinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfechaini, DbType.String, entity.StrReveporevterinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveporevterinvtit, DbType.String, entity.Reveporevterinvtit);
            dbProvider.AddInParameter(command, helper.Reveporevterinvenl, DbType.String, entity.Reveporevterinvenl);
            dbProvider.AddInParameter(command, helper.Reveporevterinvobs, DbType.String, entity.Reveporevterinvobs);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfinalizado, DbType.String, entity.Reveporevterinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfechafin, DbType.String, entity.StrReveporevterinvfechafin);
            dbProvider.AddInParameter(command, helper.Revepolevobsfechaini, DbType.String, entity.StrRevepolevobsfechaini);
            dbProvider.AddInParameter(command, helper.Revepolevobstit, DbType.String, entity.Revepolevobstit);
            dbProvider.AddInParameter(command, helper.Revepolevobsenl, DbType.String, entity.Revepolevobsenl);
            dbProvider.AddInParameter(command, helper.Revepolevobsobs, DbType.String, entity.Revepolevobsobs);
            dbProvider.AddInParameter(command, helper.Revepolevobsfinalizado, DbType.String, entity.Revepolevobsfinalizado);
            dbProvider.AddInParameter(command, helper.Revepolevobsfechafin, DbType.String, entity.StrRevepolevobsfechafin);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Reveporevcoesampl, DbType.Int32, entity.Reveporevcoesampl);
            dbProvider.AddInParameter(command, helper.Reveporevterinvampl, DbType.Int32, entity.Reveporevterinvampl);
            dbProvider.AddInParameter(command, helper.Revepopreampl, DbType.Int32, entity.Revepopreampl);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoRevisionEpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, entity.Estepocodi);
            dbProvider.AddInParameter(command, helper.Reveporevcoesfechaini, DbType.String, entity.StrReveporevcoesfechaini);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisiontit, DbType.String, entity.Reveporevcoescartarevisiontit);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisionenl, DbType.String, entity.Reveporevcoescartarevisionenl);
            dbProvider.AddInParameter(command, helper.Reveporevcoescartarevisionobs, DbType.String, entity.Reveporevcoescartarevisionobs);
            dbProvider.AddInParameter(command, helper.Reveporevcoesfinalizado, DbType.String, entity.Reveporevcoesfinalizado);
            dbProvider.AddInParameter(command, helper.Revepocoesfechafin, DbType.String, entity.StrRevepocoesfechafin);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvfechaini, DbType.String, entity.StrRevepoenvesttercinvfechaini);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvtit, DbType.String, entity.Revepoenvesttercinvtit);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvenl, DbType.String, entity.Revepoenvesttercinvenl);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvobs, DbType.String, entity.Revepoenvesttercinvobs);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvfinalizado, DbType.String, entity.Revepoenvesttercinvfinalizado);
            dbProvider.AddInParameter(command, helper.Revepoenvesttercinvinvfechafin, DbType.String, entity.StrRevepoenvesttercinvinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfechaini, DbType.String, entity.StrReveporevterinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveporevterinvtit, DbType.String, entity.Reveporevterinvtit);
            dbProvider.AddInParameter(command, helper.Reveporevterinvenl, DbType.String, entity.Reveporevterinvenl);
            dbProvider.AddInParameter(command, helper.Reveporevterinvobs, DbType.String, entity.Reveporevterinvobs);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfinalizado, DbType.String, entity.Reveporevterinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveporevterinvfechafin, DbType.String, entity.StrReveporevterinvfechafin);
            dbProvider.AddInParameter(command, helper.Revepolevobsfechaini, DbType.String, entity.StrRevepolevobsfechaini);
            dbProvider.AddInParameter(command, helper.Revepolevobstit, DbType.String, entity.Revepolevobstit);
            dbProvider.AddInParameter(command, helper.Revepolevobsenl, DbType.String, entity.Revepolevobsenl);
            dbProvider.AddInParameter(command, helper.Revepolevobsobs, DbType.String, entity.Revepolevobsobs);
            dbProvider.AddInParameter(command, helper.Revepolevobsfinalizado, DbType.String, entity.Revepolevobsfinalizado);
            dbProvider.AddInParameter(command, helper.Revepolevobsfechafin, DbType.String, entity.StrRevepolevobsfechafin);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Reveporevcoesampl, DbType.Int32, entity.Reveporevcoesampl);
            dbProvider.AddInParameter(command, helper.Reveporevterinvampl, DbType.Int32, entity.Reveporevterinvampl);
            dbProvider.AddInParameter(command, helper.Revepopreampl, DbType.Int32, entity.Revepopreampl);

            dbProvider.AddInParameter(command, helper.Revepocodi, DbType.Int32, entity.Revepocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revepocodi, DbType.Int32, revepocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoRevisionEpoDTO GetById(int revepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revepocodi, DbType.Int32, revepocodi);
            EpoRevisionEpoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoRevisionEpoDTO> List()
        {
            List<EpoRevisionEpoDTO> entitys = new List<EpoRevisionEpoDTO>();
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

        public List<EpoRevisionEpoDTO> GetByCriteria(int estepocodi)
        {
            List<EpoRevisionEpoDTO> entitys = new List<EpoRevisionEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, estepocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EpoRevisionEpoDTO> GetByCriteriaRevisionEstudio(int diautil, int diautilvenc)
        {
            string sql = string.Format(helper.SqlGetByCriteriaRevisionEstudio, diautil, diautilvenc);

            List<EpoRevisionEpoDTO> entitys = new List<EpoRevisionEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEpoDTO entity = new EpoRevisionEpoDTO();

                    int iRevepocodi = dr.GetOrdinal(helper.Revepocodi);
                    if (!dr.IsDBNull(iRevepocodi)) entity.Revepocodi = Convert.ToInt32(dr.GetValue(iRevepocodi));

                    int iEstepocodiusu = dr.GetOrdinal(helper.Estepocodiusu);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.Estepocodiusu = dr.GetString(iEstepocodiusu);

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iReveporevcoescartarevisiontit = dr.GetOrdinal(helper.Reveporevcoescartarevisiontit);
                    if (!dr.IsDBNull(iReveporevcoescartarevisiontit)) entity.Reveporevcoescartarevisiontit = dr.GetString(iReveporevcoescartarevisiontit);

                    int iReveporevcoescartarevisionenl = dr.GetOrdinal(helper.Reveporevcoescartarevisionenl);
                    if (!dr.IsDBNull(iReveporevcoescartarevisionenl)) entity.Reveporevcoescartarevisionenl = dr.GetString(iReveporevcoescartarevisionenl);

                    int iReveporevcoesfechaini = dr.GetOrdinal(helper.Reveporevcoesfechaini);
                    if (!dr.IsDBNull(iReveporevcoesfechaini)) entity.Reveporevcoesfechaini = Convert.ToDateTime(dr.GetValue(iReveporevcoesfechaini));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EpoRevisionEpoDTO> GetByCriteriaEnvioTerceroInv(int diautil, int diautilvenc)
        {
            string sql = string.Format(helper.SqlGetByCriteriaEnvioTerceroInv, diautil, diautilvenc);

            List<EpoRevisionEpoDTO> entitys = new List<EpoRevisionEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEpoDTO entity = new EpoRevisionEpoDTO();

                    int iRevepocodi = dr.GetOrdinal(helper.Revepocodi);
                    if (!dr.IsDBNull(iRevepocodi)) entity.Revepocodi = Convert.ToInt32(dr.GetValue(iRevepocodi));

                    int iEstepocodiusu = dr.GetOrdinal(helper.Estepocodiusu);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.Estepocodiusu = dr.GetString(iEstepocodiusu);

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iRevepoenvesttercinvtit = dr.GetOrdinal(helper.Revepoenvesttercinvtit);
                    if (!dr.IsDBNull(iRevepoenvesttercinvtit)) entity.Revepoenvesttercinvtit = dr.GetString(iRevepoenvesttercinvtit);

                    int iRevepoenvesttercinvenl = dr.GetOrdinal(helper.Revepoenvesttercinvenl);
                    if (!dr.IsDBNull(iRevepoenvesttercinvenl)) entity.Revepoenvesttercinvenl = dr.GetString(iRevepoenvesttercinvenl);

                    int iRevepoenvesttercinvfechaini = dr.GetOrdinal(helper.Revepoenvesttercinvfechaini);
                    if (!dr.IsDBNull(iRevepoenvesttercinvfechaini)) entity.Revepoenvesttercinvfechaini = Convert.ToDateTime(dr.GetValue(iRevepoenvesttercinvfechaini));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras EO-EPO-II
        public List<EpoRevisionEpoDTO> ListEposExcAbsObs()
        {
            List<EpoRevisionEpoDTO> entitys = new List<EpoRevisionEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListExcesoAbsObs);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
