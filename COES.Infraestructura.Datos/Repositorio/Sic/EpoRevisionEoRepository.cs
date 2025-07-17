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
    /// Clase de acceso a datos de la tabla EPO_REVISION_EO
    /// </summary>
    public class EpoRevisionEoRepository : RepositoryBase, IEpoRevisionEoRepository
    {
        public EpoRevisionEoRepository(string strConn) : base(strConn)
        {
        }

        EpoRevisionEoHelper helper = new EpoRevisionEoHelper();

        public int Save(EpoRevisionEoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reveocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            dbProvider.AddInParameter(command, helper.Reveorevcoesfechaini, DbType.String, entity.StrReveorevcoesfechaini);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisiontit, DbType.String, entity.Reveorevcoescartarevisiontit);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisionenl, DbType.String, entity.Reveorevcoescartarevisionenl);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisionobs, DbType.String, entity.Reveorevcoescartarevisionobs);
            dbProvider.AddInParameter(command, helper.Reveorevcoesfinalizado, DbType.String, entity.Reveorevcoesfinalizado);
            dbProvider.AddInParameter(command, helper.Reveocoesfechafin, DbType.String, entity.StrReveocoesfechafin);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvfechaini, DbType.String, entity.StrReveoenvesttercinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvtit, DbType.String, entity.Reveoenvesttercinvtit);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvenl, DbType.String, entity.Reveoenvesttercinvenl);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvobs, DbType.String, entity.Reveoenvesttercinvobs);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvfinalizado, DbType.String, entity.Reveoenvesttercinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvinvfechafin, DbType.String, entity.StrReveoenvesttercinvinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfechaini, DbType.String, entity.StrReveorevterinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveorevterinvtit, DbType.String, entity.Reveorevterinvtit);
            dbProvider.AddInParameter(command, helper.Reveorevterinvenl, DbType.String, entity.Reveorevterinvenl);
            dbProvider.AddInParameter(command, helper.Reveorevterinvobs, DbType.String, entity.Reveorevterinvobs);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfinalizado, DbType.String, entity.Reveorevterinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfechafin, DbType.String, entity.StrReveorevterinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveolevobsfechaini, DbType.String, entity.StrReveolevobsfechaini);
            dbProvider.AddInParameter(command, helper.Reveolevobstit, DbType.String, entity.Reveolevobstit);
            dbProvider.AddInParameter(command, helper.Reveolevobsenl, DbType.String, entity.Reveolevobsenl);
            dbProvider.AddInParameter(command, helper.Reveolevobsobs, DbType.String, entity.Reveolevobsobs);
            dbProvider.AddInParameter(command, helper.Reveolevobsfinalizado, DbType.String, entity.Reveolevobsfinalizado);
            dbProvider.AddInParameter(command, helper.Reveolevobsfechafin, DbType.String, entity.StrReveolevobsfechafin);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Reveorevcoesampl, DbType.Int32, entity.Reveorevcoesampl);
            dbProvider.AddInParameter(command, helper.Reveorevterinvampl, DbType.Int32, entity.Reveorevterinvampl);
            dbProvider.AddInParameter(command, helper.Reveopreampl, DbType.Int32, entity.Reveopreampl);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoRevisionEoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            dbProvider.AddInParameter(command, helper.Reveorevcoesfechaini, DbType.String, entity.StrReveorevcoesfechaini);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisiontit, DbType.String, entity.Reveorevcoescartarevisiontit);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisionenl, DbType.String, entity.Reveorevcoescartarevisionenl);
            dbProvider.AddInParameter(command, helper.Reveorevcoescartarevisionobs, DbType.String, entity.Reveorevcoescartarevisionobs);
            dbProvider.AddInParameter(command, helper.Reveorevcoesfinalizado, DbType.String, entity.Reveorevcoesfinalizado);
            dbProvider.AddInParameter(command, helper.Reveocoesfechafin, DbType.String, entity.StrReveocoesfechafin);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvfechaini, DbType.String, entity.StrReveoenvesttercinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvtit, DbType.String, entity.Reveoenvesttercinvtit);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvenl, DbType.String, entity.Reveoenvesttercinvenl);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvobs, DbType.String, entity.Reveoenvesttercinvobs);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvfinalizado, DbType.String, entity.Reveoenvesttercinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveoenvesttercinvinvfechafin, DbType.String, entity.StrReveoenvesttercinvinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfechaini, DbType.String, entity.StrReveorevterinvfechaini);
            dbProvider.AddInParameter(command, helper.Reveorevterinvtit, DbType.String, entity.Reveorevterinvtit);
            dbProvider.AddInParameter(command, helper.Reveorevterinvenl, DbType.String, entity.Reveorevterinvenl);
            dbProvider.AddInParameter(command, helper.Reveorevterinvobs, DbType.String, entity.Reveorevterinvobs);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfinalizado, DbType.String, entity.Reveorevterinvfinalizado);
            dbProvider.AddInParameter(command, helper.Reveorevterinvfechafin, DbType.String, entity.StrReveorevterinvfechafin);
            dbProvider.AddInParameter(command, helper.Reveolevobsfechaini, DbType.String, entity.StrReveolevobsfechaini);
            dbProvider.AddInParameter(command, helper.Reveolevobstit, DbType.String, entity.Reveolevobstit);
            dbProvider.AddInParameter(command, helper.Reveolevobsenl, DbType.String, entity.Reveolevobsenl);
            dbProvider.AddInParameter(command, helper.Reveolevobsobs, DbType.String, entity.Reveolevobsobs);
            dbProvider.AddInParameter(command, helper.Reveolevobsfinalizado, DbType.String, entity.Reveolevobsfinalizado);
            dbProvider.AddInParameter(command, helper.Reveolevobsfechafin, DbType.String, entity.StrReveolevobsfechafin);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Reveorevcoesampl, DbType.Int32, entity.Reveorevcoesampl);
            dbProvider.AddInParameter(command, helper.Reveorevterinvampl, DbType.Int32, entity.Reveorevterinvampl);
            dbProvider.AddInParameter(command, helper.Reveopreampl, DbType.Int32, entity.Reveopreampl);
            dbProvider.AddInParameter(command, helper.Reveocodi, DbType.Int32, entity.Reveocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reveocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reveocodi, DbType.Int32, reveocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoRevisionEoDTO GetById(int reveocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reveocodi, DbType.Int32, reveocodi);
            EpoRevisionEoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoRevisionEoDTO> List()
        {
            List<EpoRevisionEoDTO> entitys = new List<EpoRevisionEoDTO>();
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

        public List<EpoRevisionEoDTO> GetByCriteria(int esteocodi)
        {
            List<EpoRevisionEoDTO> entitys = new List<EpoRevisionEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, esteocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEoDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EpoRevisionEoDTO> GetByCriteriaRevisionEstudio(int diautil, int diautilvenc)
        {
            string sql = string.Format(helper.SqlGetByCriteriaRevisionEstudio, diautil, diautilvenc);

            List<EpoRevisionEoDTO> entitys = new List<EpoRevisionEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEoDTO entity = new EpoRevisionEoDTO();

                    int iReveocodi = dr.GetOrdinal(helper.Reveocodi);
                    if (!dr.IsDBNull(iReveocodi)) entity.Reveocodi = Convert.ToInt32(dr.GetValue(iReveocodi));

                    int iEsteocodiusu = dr.GetOrdinal(helper.Esteocodiusu);
                    if (!dr.IsDBNull(iReveocodi)) entity.Esteocodiusu = dr.GetString(iEsteocodiusu);

                    int iEsteonomb = dr.GetOrdinal(helper.Esteonomb);
                    if (!dr.IsDBNull(iEsteonomb)) entity.Esteonomb = dr.GetString(iEsteonomb);

                    int iReveorevcoescartarevisiontit = dr.GetOrdinal(helper.Reveorevcoescartarevisiontit);
                    if (!dr.IsDBNull(iReveorevcoescartarevisiontit)) entity.Reveorevcoescartarevisiontit = dr.GetString(iReveorevcoescartarevisiontit);

                    int iReveorevcoescartarevisionenl = dr.GetOrdinal(helper.Reveorevcoescartarevisionenl);
                    if (!dr.IsDBNull(iReveorevcoescartarevisionenl)) entity.Reveorevcoescartarevisionenl = dr.GetString(iReveorevcoescartarevisionenl);

                    int iReveorevcoesfechaini = dr.GetOrdinal(helper.Reveorevcoesfechaini);
                    if (!dr.IsDBNull(iReveorevcoesfechaini)) entity.Reveorevcoesfechaini = Convert.ToDateTime(dr.GetValue(iReveorevcoesfechaini));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EpoRevisionEoDTO> GetByCriteriaEnvioTerceroInv(int diautil, int diautilvenc)
        {
            string sql = string.Format(helper.SqlGetByCriteriaEnvioTerceroInv, diautil, diautilvenc);

            List<EpoRevisionEoDTO> entitys = new List<EpoRevisionEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEoDTO entity = new EpoRevisionEoDTO();

                    int iReveocodi = dr.GetOrdinal(helper.Reveocodi);
                    if (!dr.IsDBNull(iReveocodi)) entity.Reveocodi = Convert.ToInt32(dr.GetValue(iReveocodi));

                    int iEsteocodiusu = dr.GetOrdinal(helper.Esteocodiusu);
                    if (!dr.IsDBNull(iReveocodi)) entity.Esteocodiusu = dr.GetString(iEsteocodiusu);

                    int iEsteonomb = dr.GetOrdinal(helper.Esteonomb);
                    if (!dr.IsDBNull(iEsteonomb)) entity.Esteonomb = dr.GetString(iEsteonomb);

                    int iReveoenvesttercinvtit = dr.GetOrdinal(helper.Reveoenvesttercinvtit);
                    if (!dr.IsDBNull(iReveoenvesttercinvtit)) entity.Reveoenvesttercinvtit = dr.GetString(iReveoenvesttercinvtit);

                    int iReveoenvesttercinvenl = dr.GetOrdinal(helper.Reveoenvesttercinvenl);
                    if (!dr.IsDBNull(iReveoenvesttercinvenl)) entity.Reveoenvesttercinvenl = dr.GetString(iReveoenvesttercinvenl);

                    int iReveoenvesttercinvfechaini = dr.GetOrdinal(helper.Reveoenvesttercinvfechaini);
                    if (!dr.IsDBNull(iReveoenvesttercinvfechaini)) entity.Reveoenvesttercinvfechaini = Convert.ToDateTime(dr.GetValue(iReveoenvesttercinvfechaini));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras EO-EPO-II
        public List<EpoRevisionEoDTO> ListEosExcAbsObs()
        {
            List<EpoRevisionEoDTO> entitys = new List<EpoRevisionEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListExcesoAbsObs);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoRevisionEoDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
