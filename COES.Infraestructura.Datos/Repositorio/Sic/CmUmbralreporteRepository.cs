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
    /// Clase de acceso a datos de la tabla CM_UMBRALREPORTE
    /// </summary>
    public class CmUmbralreporteRepository: RepositoryBase, ICmUmbralreporteRepository
    {
        public CmUmbralreporteRepository(string strConn): base(strConn)
        {
        }

        CmUmbralreporteHelper helper = new CmUmbralreporteHelper();

        public int Save(CmUmbralreporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmurminbarra, DbType.Decimal, entity.Cmurminbarra);
            dbProvider.AddInParameter(command, helper.Cmurmaxbarra, DbType.Decimal, entity.Cmurmaxbarra);
            dbProvider.AddInParameter(command, helper.Cmurminenergia, DbType.Decimal, entity.Cmurminenergia);
            dbProvider.AddInParameter(command, helper.Cmurmaxenergia, DbType.Decimal, entity.Cmurmaxenergia);
            dbProvider.AddInParameter(command, helper.Cmurminconges, DbType.Decimal, entity.Cmurminconges);
            dbProvider.AddInParameter(command, helper.Cmurmaxconges, DbType.Decimal, entity.Cmurmaxconges);
            dbProvider.AddInParameter(command, helper.Cmurdiferencia, DbType.Decimal, entity.Cmurdiferencia);
            dbProvider.AddInParameter(command, helper.Cmurestado, DbType.String, entity.Cmurestado);
            dbProvider.AddInParameter(command, helper.Cmurvigencia, DbType.DateTime, entity.Cmurvigencia);
            dbProvider.AddInParameter(command, helper.Cmurexpira, DbType.DateTime, entity.Cmurexpira);
            dbProvider.AddInParameter(command, helper.Cmurusucreacion, DbType.String, entity.Cmurusucreacion);
            dbProvider.AddInParameter(command, helper.Cmurfeccreacion, DbType.DateTime, entity.Cmurfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmurusumodificacion, DbType.String, entity.Cmurusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmurfecmodificacion, DbType.DateTime, entity.Cmurfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmUmbralreporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmurminbarra, DbType.Decimal, entity.Cmurminbarra);
            dbProvider.AddInParameter(command, helper.Cmurmaxbarra, DbType.Decimal, entity.Cmurmaxbarra);
            dbProvider.AddInParameter(command, helper.Cmurminenergia, DbType.Decimal, entity.Cmurminenergia);
            dbProvider.AddInParameter(command, helper.Cmurmaxenergia, DbType.Decimal, entity.Cmurmaxenergia);
            dbProvider.AddInParameter(command, helper.Cmurminconges, DbType.Decimal, entity.Cmurminconges);
            dbProvider.AddInParameter(command, helper.Cmurmaxconges, DbType.Decimal, entity.Cmurmaxconges);
            dbProvider.AddInParameter(command, helper.Cmurdiferencia, DbType.Decimal, entity.Cmurdiferencia);
            dbProvider.AddInParameter(command, helper.Cmurestado, DbType.String, entity.Cmurestado);
            dbProvider.AddInParameter(command, helper.Cmurvigencia, DbType.DateTime, entity.Cmurvigencia);
            dbProvider.AddInParameter(command, helper.Cmurexpira, DbType.DateTime, entity.Cmurexpira);           
            dbProvider.AddInParameter(command, helper.Cmurusumodificacion, DbType.String, entity.Cmurusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmurfecmodificacion, DbType.DateTime, entity.Cmurfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, entity.Cmurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, cmurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmUmbralreporteDTO GetById(int cmurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, cmurcodi);
            CmUmbralreporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmUmbralreporteDTO> List()
        {
            List<CmUmbralreporteDTO> entitys = new List<CmUmbralreporteDTO>();
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

        public List<CmUmbralreporteDTO> GetByCriteria(DateTime fecha)
        {
            List<CmUmbralreporteDTO> entitys = new List<CmUmbralreporteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmUmbralreporteDTO entity = helper.Create(dr);

                    entity.Vigencia = (entity.Cmurvigencia != null) ?
                      ((DateTime)entity.Cmurvigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmurfecmodificacion != null) ?
                        ((DateTime)entity.Cmurfecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmUmbralreporteDTO> ObtenerHistorico()
        {
            List<CmUmbralreporteDTO> entitys = new List<CmUmbralreporteDTO>();

            string sql = string.Format(helper.SqlObtenerHistorico);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmUmbralreporteDTO entity = helper.Create(dr);

                    entity.Vigencia = (entity.Cmurvigencia != null) ?
                      ((DateTime)entity.Cmurvigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmurfecmodificacion != null) ?
                        ((DateTime)entity.Cmurfecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Expiracion = (entity.Cmurexpira != null) ?
                     ((DateTime)entity.Cmurexpira).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
