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
    /// Clase de acceso a datos de la tabla SI_CORREO
    /// </summary>
    public class SiCorreoRepository: RepositoryBase, ISiCorreoRepository
    {
        public SiCorreoRepository(string strConn): base(strConn)
        {
        }

        SiCorreoHelper helper = new SiCorreoHelper();

        public int Save(SiCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);
            dbProvider.AddInParameter(command, helper.Corrto, DbType.String, entity.Corrto);
            dbProvider.AddInParameter(command, helper.Corrfrom, DbType.String, entity.Corrfrom);
            dbProvider.AddInParameter(command, helper.Corrcc, DbType.String, entity.Corrcc);
            dbProvider.AddInParameter(command, helper.Corrfechaenvio, DbType.DateTime, entity.Corrfechaenvio);
            dbProvider.AddInParameter(command, helper.Corrbcc, DbType.String, entity.Corrbcc);
            dbProvider.AddInParameter(command, helper.Corrasunto, DbType.String, entity.Corrasunto);
            dbProvider.AddInParameter(command, helper.Corrcontenido, DbType.String, entity.Corrcontenido);
            dbProvider.AddInParameter(command, helper.Corrfechaperiodo, DbType.DateTime, entity.Corrfechaperiodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Corrusuenvio, DbType.String, entity.Corrusuenvio);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);
            dbProvider.AddInParameter(command, helper.Corrto, DbType.String, entity.Corrto);
            dbProvider.AddInParameter(command, helper.Corrfrom, DbType.String, entity.Corrfrom);
            dbProvider.AddInParameter(command, helper.Corrcc, DbType.String, entity.Corrcc);
            dbProvider.AddInParameter(command, helper.Corrfechaenvio, DbType.DateTime, entity.Corrfechaenvio);
            dbProvider.AddInParameter(command, helper.Corrbcc, DbType.String, entity.Corrbcc);
            dbProvider.AddInParameter(command, helper.Corrasunto, DbType.String, entity.Corrasunto);
            dbProvider.AddInParameter(command, helper.Corrcontenido, DbType.String, entity.Corrcontenido);
            dbProvider.AddInParameter(command, helper.Corrfechaperiodo, DbType.DateTime, entity.Corrfechaperiodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Corrusuenvio, DbType.String, entity.Corrusuenvio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int corrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, corrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCorreoDTO GetById(int corrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, corrcodi);
            SiCorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCorreoDTO> List()
        {
            List<SiCorreoDTO> entitys = new List<SiCorreoDTO>();
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

        public List<SiCorreoDTO> GetByCriteria(string strIdEmpresas, DateTime fechaIni, DateTime fechaFin, string idsPlantilla)
        {
            List<SiCorreoDTO> entitys = new List<SiCorreoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idsPlantilla, strIdEmpresas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCorreoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiCorreoDTO> ListarLogCorreo(DateTime fecha, string idsPlantilla)
        {
            List<SiCorreoDTO> entitys = new List<SiCorreoDTO>();
            string query = string.Format(helper.SqlListarLogCorreo, fecha.ToString(ConstantesBase.FormatoFecha),
                idsPlantilla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCorreoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }


            return entitys;
        }
    }
}
