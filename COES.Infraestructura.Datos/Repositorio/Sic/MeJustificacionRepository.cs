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
    /// Clase de acceso a datos de la tabla ME_JUSTIFICACION
    /// </summary>
    public class MeJustificacionRepository: RepositoryBase, IMeJustificacionRepository
    {
        public MeJustificacionRepository(string strConn): base(strConn)
        {
        }

        MeJustificacionHelper helper = new MeJustificacionHelper();

        public void Save(MeJustificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = result != null ? Convert.ToInt32(result) : 1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Justcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            //ASSETEC 201909: Nuevo atributo para distinguir la fuente de dato
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Justfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Justusucreacion, DbType.String, entity.Justusucreacion);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Justdescripcionotros, DbType.String, entity.Justdescripcionotros);
            dbProvider.AddInParameter(command, helper.Justfechainicio, DbType.DateTime, entity.Justfechainicio);
            dbProvider.AddInParameter(command, helper.Justfechafin, DbType.DateTime, entity.Justfechafin);

            dbProvider.ExecuteNonQuery(command);
        }     

        public void Update(MeJustificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            //ASSETEC 201909: Nuevo atributo para distinguir la fuente de dato
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Justfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Justusucreacion, DbType.String, entity.Justusucreacion);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Justdescripcionotros, DbType.String, entity.Justdescripcionotros);
            dbProvider.AddInParameter(command, helper.Justfechainicio, DbType.DateTime, entity.Justfechainicio);
            dbProvider.AddInParameter(command, helper.Justfechafin, DbType.DateTime, entity.Justfechafin);
            dbProvider.AddInParameter(command, helper.Justcodi, DbType.Int32, entity.Justcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        //ASSETEC 201909: obtenemos un registro de MeJustificacion
        public MeJustificacionDTO GetById(int justcodi)
        {
            MeJustificacionDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Justcodi, DbType.Int32, justcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeJustificacionDTO> List()
        {
            List<MeJustificacionDTO> entitys = new List<MeJustificacionDTO>();
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

        public List<MeJustificacionDTO> GetByCriteria()
        {
            List<MeJustificacionDTO> entitys = new List<MeJustificacionDTO>();
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

        public List<MeJustificacionDTO> ListByIdEnvio(int idEnvio)
        {
            List<MeJustificacionDTO> entitys = new List<MeJustificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByIdEnvio);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        //ASSETEC 201909: Lista por envio y Punto de medición
        public List<MeJustificacionDTO> ListByIdEnvioPtoMedicodi(int idEnvio, int idLectcodi, int idPtomedicodi)
        {
            List<MeJustificacionDTO> entitys = new List<MeJustificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByIdEnvioPtoMedicodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, idLectcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, idPtomedicodi);
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
