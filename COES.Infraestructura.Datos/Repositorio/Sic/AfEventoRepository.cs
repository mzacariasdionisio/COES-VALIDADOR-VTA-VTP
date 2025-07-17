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
    /// Clase de acceso a datos de la tabla AF_EVENTO
    /// </summary>
    public class AfEventoRepository : RepositoryBase, IAfEventoRepository
    {
        public AfEventoRepository(string strConn) : base(strConn)
        {
        }

        AfEventoHelper helper = new AfEventoHelper();

        public int Save(AfEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Aferacmt, DbType.String, entity.Aferacmt);
            dbProvider.AddInParameter(command, helper.Afeeracmf, DbType.String, entity.Afeeracmf);
            dbProvider.AddInParameter(command, helper.Afermc, DbType.String, entity.Afermc);
            dbProvider.AddInParameter(command, helper.Afecorr, DbType.Int32, entity.Afecorr);
            dbProvider.AddInParameter(command, helper.Afeanio, DbType.Int32, entity.Afeanio);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Afefzamayor, DbType.String, entity.Afefzamayor);
            dbProvider.AddInParameter(command, helper.Afeestadomotivo, DbType.String, entity.Afeestadomotivo);
            dbProvider.AddInParameter(command, helper.Afeestado, DbType.String, entity.Afeestado);
            dbProvider.AddInParameter(command, helper.Afeempcompninguna, DbType.String, entity.Afeempcompninguna);
            dbProvider.AddInParameter(command, helper.Afeemprespninguna, DbType.String, entity.Afeemprespninguna);
            dbProvider.AddInParameter(command, helper.Afeitpitffecha, DbType.DateTime, entity.Afeitpitffecha);
            dbProvider.AddInParameter(command, helper.Afefzadecisfechasist, DbType.DateTime, entity.Afefzadecisfechasist);
            dbProvider.AddInParameter(command, helper.Afefzafechasist, DbType.DateTime, entity.Afefzafechasist);
            dbProvider.AddInParameter(command, helper.Afeitdecfechaelab, DbType.DateTime, entity.Afeitdecfechaelab);
            dbProvider.AddInParameter(command, helper.Afeitdecfechanominal, DbType.DateTime, entity.Afeitdecfechanominal);
            dbProvider.AddInParameter(command, helper.Afecompfechasist, DbType.DateTime, entity.Afecompfechasist);
            dbProvider.AddInParameter(command, helper.Afecompfecha, DbType.DateTime, entity.Afecompfecha);
            dbProvider.AddInParameter(command, helper.Afeitpdecisffechasist, DbType.DateTime, entity.Afeitpdecisffechasist);
            dbProvider.AddInParameter(command, helper.Afeitpitffechasist, DbType.DateTime, entity.Afeitpitffechasist);
            dbProvider.AddInParameter(command, helper.Afeconvcitacionfecha, DbType.DateTime, entity.Afeconvcitacionfecha);
            dbProvider.AddInParameter(command, helper.Aferctaeinformefecha, DbType.DateTime, entity.Aferctaeinformefecha);
            dbProvider.AddInParameter(command, helper.Aferctaeactafecha, DbType.DateTime, entity.Aferctaeactafecha);
            dbProvider.AddInParameter(command, helper.Afeimpugna, DbType.String, entity.Afeimpugna);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Afeitrdofecharecep, DbType.DateTime, entity.Afeitrdofecharecep);
            dbProvider.AddInParameter(command, helper.Afeitrdofechaenvio, DbType.DateTime, entity.Afeitrdofechaenvio);
            dbProvider.AddInParameter(command, helper.Afeitrdoestado, DbType.String, entity.Afeitrdoestado);
            dbProvider.AddInParameter(command, helper.Afeitrdjrfecharecep, DbType.DateTime, entity.Afeitrdjrfecharecep);
            dbProvider.AddInParameter(command, helper.Afeitrdjrfechaenvio, DbType.DateTime, entity.Afeitrdjrfechaenvio);
            dbProvider.AddInParameter(command, helper.Afeitrdjrestado, DbType.String, entity.Afeitrdjrestado);
            dbProvider.AddInParameter(command, helper.Afeitfechaelab, DbType.DateTime, entity.Afeitfechaelab);
            dbProvider.AddInParameter(command, helper.Afeitfechanominal, DbType.DateTime, entity.Afeitfechanominal);
            dbProvider.AddInParameter(command, helper.Aferctaeobservacion, DbType.String, entity.Aferctaeobservacion);
            dbProvider.AddInParameter(command, helper.Afeconvtiporeunion, DbType.String, entity.Afeconvtiporeunion);
            dbProvider.AddInParameter(command, helper.Afereuhoraprog, DbType.String, entity.Afereuhoraprog);
            dbProvider.AddInParameter(command, helper.Afereufechaprog, DbType.DateTime, entity.Afereufechaprog);
            dbProvider.AddInParameter(command, helper.Afereufechanominal, DbType.DateTime, entity.Afereufechanominal);
            dbProvider.AddInParameter(command, helper.Afecitfechaelab, DbType.DateTime, entity.Afecitfechaelab);
            dbProvider.AddInParameter(command, helper.Afecitfechanominal, DbType.DateTime, entity.Afecitfechanominal);
            dbProvider.AddInParameter(command, helper.Aferesponsable, DbType.String, entity.Aferesponsable);
            dbProvider.AddInParameter(command, helper.Afeedagsf, DbType.String, entity.Afeedagsf);
            dbProvider.AddInParameter(command, helper.Afeplazofecmodificacion, DbType.DateTime, entity.Afeplazofecmodificacion);
            dbProvider.AddInParameter(command, helper.Afeplazousumodificacion, DbType.String, entity.Afeplazousumodificacion);
            dbProvider.AddInParameter(command, helper.Afeplazofechaampl, DbType.DateTime, entity.Afeplazofechaampl);
            dbProvider.AddInParameter(command, helper.Afeplazofecha, DbType.DateTime, entity.Afeplazofecha);
            dbProvider.AddInParameter(command, helper.Afefechainterr, DbType.DateTime, entity.Afefechainterr);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Aferacmt, DbType.String, entity.Aferacmt);
            dbProvider.AddInParameter(command, helper.Afeeracmf, DbType.String, entity.Afeeracmf);
            dbProvider.AddInParameter(command, helper.Afermc, DbType.String, entity.Afermc);
            dbProvider.AddInParameter(command, helper.Afecorr, DbType.Int32, entity.Afecorr);
            dbProvider.AddInParameter(command, helper.Afeanio, DbType.Int32, entity.Afeanio);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);
            dbProvider.AddInParameter(command, helper.Afefzamayor, DbType.String, entity.Afefzamayor);
            dbProvider.AddInParameter(command, helper.Afeestadomotivo, DbType.String, entity.Afeestadomotivo);
            dbProvider.AddInParameter(command, helper.Afeestado, DbType.String, entity.Afeestado);
            dbProvider.AddInParameter(command, helper.Afeempcompninguna, DbType.String, entity.Afeempcompninguna);
            dbProvider.AddInParameter(command, helper.Afeemprespninguna, DbType.String, entity.Afeemprespninguna);
            dbProvider.AddInParameter(command, helper.Afeitpitffecha, DbType.DateTime, entity.Afeitpitffecha);
            dbProvider.AddInParameter(command, helper.Afefzadecisfechasist, DbType.DateTime, entity.Afefzadecisfechasist);
            dbProvider.AddInParameter(command, helper.Afefzafechasist, DbType.DateTime, entity.Afefzafechasist);
            dbProvider.AddInParameter(command, helper.Afeitdecfechaelab, DbType.DateTime, entity.Afeitdecfechaelab);
            dbProvider.AddInParameter(command, helper.Afeitdecfechanominal, DbType.DateTime, entity.Afeitdecfechanominal);
            dbProvider.AddInParameter(command, helper.Afecompfechasist, DbType.DateTime, entity.Afecompfechasist);
            dbProvider.AddInParameter(command, helper.Afecompfecha, DbType.DateTime, entity.Afecompfecha);
            dbProvider.AddInParameter(command, helper.Afeitpdecisffechasist, DbType.DateTime, entity.Afeitpdecisffechasist);
            dbProvider.AddInParameter(command, helper.Afeitpitffechasist, DbType.DateTime, entity.Afeitpitffechasist);
            dbProvider.AddInParameter(command, helper.Afeconvcitacionfecha, DbType.DateTime, entity.Afeconvcitacionfecha);
            dbProvider.AddInParameter(command, helper.Aferctaeinformefecha, DbType.DateTime, entity.Aferctaeinformefecha);
            dbProvider.AddInParameter(command, helper.Aferctaeactafecha, DbType.DateTime, entity.Aferctaeactafecha);
            dbProvider.AddInParameter(command, helper.Afeimpugna, DbType.String, entity.Afeimpugna);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Afeitrdofecharecep, DbType.DateTime, entity.Afeitrdofecharecep);
            dbProvider.AddInParameter(command, helper.Afeitrdofechaenvio, DbType.DateTime, entity.Afeitrdofechaenvio);
            dbProvider.AddInParameter(command, helper.Afeitrdoestado, DbType.String, entity.Afeitrdoestado);
            dbProvider.AddInParameter(command, helper.Afeitrdjrfecharecep, DbType.DateTime, entity.Afeitrdjrfecharecep);
            dbProvider.AddInParameter(command, helper.Afeitrdjrfechaenvio, DbType.DateTime, entity.Afeitrdjrfechaenvio);
            dbProvider.AddInParameter(command, helper.Afeitrdjrestado, DbType.String, entity.Afeitrdjrestado);
            dbProvider.AddInParameter(command, helper.Afeitfechaelab, DbType.DateTime, entity.Afeitfechaelab);
            dbProvider.AddInParameter(command, helper.Afeitfechanominal, DbType.DateTime, entity.Afeitfechanominal);
            dbProvider.AddInParameter(command, helper.Aferctaeobservacion, DbType.String, entity.Aferctaeobservacion);
            dbProvider.AddInParameter(command, helper.Afeconvtiporeunion, DbType.String, entity.Afeconvtiporeunion);
            dbProvider.AddInParameter(command, helper.Afereuhoraprog, DbType.String, entity.Afereuhoraprog);
            dbProvider.AddInParameter(command, helper.Afereufechaprog, DbType.DateTime, entity.Afereufechaprog);
            dbProvider.AddInParameter(command, helper.Afereufechanominal, DbType.DateTime, entity.Afereufechanominal);
            dbProvider.AddInParameter(command, helper.Afecitfechaelab, DbType.DateTime, entity.Afecitfechaelab);
            dbProvider.AddInParameter(command, helper.Afecitfechanominal, DbType.DateTime, entity.Afecitfechanominal);
            dbProvider.AddInParameter(command, helper.Aferesponsable, DbType.String, entity.Aferesponsable);
            dbProvider.AddInParameter(command, helper.Afeedagsf, DbType.String, entity.Afeedagsf);
            dbProvider.AddInParameter(command, helper.Afeplazofecmodificacion, DbType.DateTime, entity.Afeplazofecmodificacion);
            dbProvider.AddInParameter(command, helper.Afeplazousumodificacion, DbType.String, entity.Afeplazousumodificacion);
            dbProvider.AddInParameter(command, helper.Afeplazofechaampl, DbType.DateTime, entity.Afeplazofechaampl);
            dbProvider.AddInParameter(command, helper.Afeplazofecha, DbType.DateTime, entity.Afeplazofecha);
            dbProvider.AddInParameter(command, helper.Afefechainterr, DbType.DateTime, entity.Afefechainterr);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int afecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfEventoDTO GetById(int afecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);
            AfEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfEventoDTO> List()
        {
            List<AfEventoDTO> entitys = new List<AfEventoDTO>();
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

        public List<AfEventoDTO> GetByCriteria()
        {
            List<AfEventoDTO> entitys = new List<AfEventoDTO>();
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
