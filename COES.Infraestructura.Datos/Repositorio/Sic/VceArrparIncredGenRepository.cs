// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{

    public class VceArrparIncredGenRepository : RepositoryBase, IVceArrparIncredGenRepository
    {
        VceArrparIncredGenHelper helper = new VceArrparIncredGenHelper();

        public VceArrparIncredGenRepository(string strConn)
            : base(strConn)
        {
        }

        public void Save(VceArrparIncredGenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apinrefecha, DbType.DateTime, entity.Apinrefecha);

            //- Otros:
            dbProvider.AddInParameter(command, helper.Apinrenuminc, DbType.Int32, entity.Apinrenuminc);
            dbProvider.AddInParameter(command, helper.Apinrenumdis, DbType.Int32, entity.Apinrenumdis);
            dbProvider.AddInParameter(command, helper.Apinreusucreacion, DbType.String, entity.Apinreusucreacion);
            dbProvider.AddInParameter(command, helper.Apinrefeccreacion, DbType.DateTime, entity.Apinrefeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceArrparIncredGenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Apinrenuminc, DbType.Int32, entity.Apinrenuminc);
            dbProvider.AddInParameter(command, helper.Apinrenumdis, DbType.Int32, entity.Apinrenumdis);
            dbProvider.AddInParameter(command, helper.Apinreusucreacion, DbType.String, entity.Apinreusucreacion);
            dbProvider.AddInParameter(command, helper.Apinrefeccreacion, DbType.DateTime, entity.Apinrefeccreacion);
            dbProvider.AddInParameter(command, helper.Apinreusumodificacion, DbType.String, entity.Apinreusumodificacion);
            dbProvider.AddInParameter(command, helper.Apinrefecmodificacion, DbType.DateTime, entity.Apinrefecmodificacion);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apinrefecha, DbType.DateTime, entity.Apinrefecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(VceArrparIncredGenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apinrefecha, DbType.DateTime, entity.Apinrefecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<VceArrparIncredGenDTO> List()
        {
            throw new NotImplementedException();
        }

        public List<VceArrparIncredGenDTO> GetByCriteria()
        {
            throw new NotImplementedException();
        }

        public List<VceArrparIncredGenDTO> GetByPeriod(int codPeriodo)
        {
            List<VceArrparIncredGenDTO> entities = new List<VceArrparIncredGenDTO>();

            string queryString = string.Format(helper.SqlGetListaPorPeriodo, codPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public VceArrparIncredGenDTO GetById(int PecaCodi, int GrupoCodi, string ApinrefechaDesc)
        {
            VceArrparIncredGenDTO entity = null;

            string queryString = string.Format(helper.SqlGetById, PecaCodi, GrupoCodi, ApinrefechaDesc);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }

}
