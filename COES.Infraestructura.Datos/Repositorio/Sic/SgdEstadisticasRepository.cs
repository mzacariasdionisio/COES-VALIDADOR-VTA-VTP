using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
	/// <summary>
	/// Clase de acceso a datos de la tabla SGD_ESTADISTICAS
	/// </summary>
	public class SgdEstadisticasRepository : RepositoryBase, ISgdEstadisticasRepository
	{

        public SgdEstadisticasRepository(string strConn): base(strConn)
		{
		}

        SgdEstadisticasHelper helper = new SgdEstadisticasHelper();



        public int Save(SgdEstadisticasDTO entity)
		{

			int id = 0;

			try
			{
				DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
				object result = dbProvider.ExecuteScalar(command);
				if (result != null) id = Convert.ToInt32(result);

                command = dbProvider.GetSqlStringCommand(helper.SqlSave);
                dbProvider.AddInParameter(command, helper.Sgdecodi, DbType.Int32, id);
                dbProvider.AddInParameter(command, helper.Sgdefecderdirresp, DbType.DateTime, entity.Sgdefecderdirresp);
                dbProvider.AddInParameter(command, helper.Sgdefecderarearesp, DbType.DateTime, entity.Sgdefecderarearesp);
                dbProvider.AddInParameter(command, helper.Sgdedirrespcodi, DbType.Int32, entity.Sgdedirrespcodi);
			    dbProvider.AddInParameter(command, helper.Sgdearearespcodi, DbType.Int32, entity.Sgdearearespcodi);
                dbProvider.AddInParameter(command, helper.Sgdediasatencion, DbType.Int32, entity.Sgdediasatencion);
                dbProvider.AddInParameter(command, helper.Sgdediasdearea, DbType.Int32, entity.Sgdediasdearea);
                dbProvider.AddInParameter(command, helper.Sgdediasdedir, DbType.Int32, entity.Sgdediasdedir);
                dbProvider.AddInParameter(command, helper.Sgdediadoc, DbType.String, entity.Sgdediadoc);
                dbProvider.AddInParameter(command, helper.Sgdedirrespnomb, DbType.String, entity.Sgdedirrespnomb);
                dbProvider.AddInParameter(command, helper.Sgdearearespnomb, DbType.String, entity.Sgdearearespnomb);
                dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
                dbProvider.AddInParameter(command, helper.Fljcodi, DbType.Int32, entity.Fljcodi);
                dbProvider.AddInParameter(command, helper.Fljdetcodi, DbType.Int32, entity.Fljdetcodi);
                dbProvider.AddInParameter(command, helper.Fljdetcodiref, DbType.Int32, entity.Fljdetcodiref);
                dbProvider.AddInParameter(command, helper.Sgdeusucreacion, DbType.String, entity.Sgdeusucreacion);
                dbProvider.AddInParameter(command, helper.Sgdefeccreacion, DbType.DateTime, entity.Sgdefeccreacion);

                dbProvider.ExecuteNonQuery(command);


            }
			catch (Exception e)
			{
				id = -1;
			}

			return id;
		}

        public void Update(SgdEstadisticasDTO entity)
		{
			string stQuery = string.Format(helper.SqlUpdate);
			DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.AddInParameter(command, helper.Sgdefecderdirresp, DbType.DateTime, entity.Sgdefecderdirresp);
            dbProvider.AddInParameter(command, helper.Sgdefecderarearesp, DbType.DateTime, entity.Sgdefecderarearesp);
            dbProvider.AddInParameter(command, helper.Sgdedirrespcodi, DbType.Int32, entity.Sgdedirrespcodi);
            dbProvider.AddInParameter(command, helper.Sgdediasatencion, DbType.Int32, entity.Sgdediasatencion);
            dbProvider.AddInParameter(command, helper.Sgdediasdearea, DbType.Int32, entity.Sgdediasdearea);
            dbProvider.AddInParameter(command, helper.Sgdediasdedir, DbType.Int32, entity.Sgdediasdedir);
            dbProvider.AddInParameter(command, helper.Sgdediadoc, DbType.String, entity.Sgdediadoc);
            dbProvider.AddInParameter(command, helper.Sgdedirrespnomb, DbType.String, entity.Sgdedirrespnomb);
            dbProvider.AddInParameter(command, helper.Sgdearearespnomb, DbType.String, entity.Sgdearearespnomb);
            dbProvider.AddInParameter(command, helper.Fljdetcodiref, DbType.Int32, entity.Fljdetcodiref);
            dbProvider.AddInParameter(command, helper.Sgdeusumodificacion, DbType.String, entity.Sgdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Sgdefecmodificacion, DbType.DateTime, entity.Sgdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Sgdecodi, DbType.Int32, entity.Sgdecodi);


            dbProvider.ExecuteNonQuery(command);
		}

		public void Delete()
		{
			DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


			dbProvider.ExecuteNonQuery(command);
		}

		public SgdEstadisticasDTO GetById(int fljcodi)
		{
			DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
			dbProvider.AddInParameter(command, helper.Sgdecodi, DbType.Int32, fljcodi);
            SgdEstadisticasDTO entity = null;

			using (IDataReader dr = dbProvider.ExecuteReader(command))
			{
				if (dr.Read())
				{
					entity = helper.Create(dr);
				}
			}

			return entity;
		}

		public List<SgdEstadisticasDTO> List(SgdEstadisticasDTO data)
		{
            List<SgdEstadisticasDTO> entitys = new List<SgdEstadisticasDTO>();
            string stQuery = "";

            if(data.ListEmprcodi.Length>0)
                stQuery = string.Format(helper.SqlList, ((DateTime)(data.FilterFechaInicio)).ToString("dd/MM/yyyy"), ((DateTime)(data.FilterFechaFin)).ToString("dd/MM/yyyy"), data.FilterTipoDoc, data.FilterTipoEmpr,data.FilterSein, data.FilterEmprCoes,data.FilterAmbito,data.FilterDomiciliada,data.ListEmprcodi,data.ListAreacodi,data.FilterRubro,data.FilterAgente);
            else
                stQuery = string.Format(helper.SqlListAllCompanies, ((DateTime)(data.FilterFechaInicio)).ToString("dd/MM/yyyy"), ((DateTime)(data.FilterFechaFin)).ToString("dd/MM/yyyy"), data.FilterTipoDoc, data.FilterTipoEmpr, data.FilterSein, data.FilterEmprCoes, data.FilterAmbito, data.FilterDomiciliada, data.ListEmprcodi, data.ListAreacodi, data.FilterRubro, data.FilterAgente);

            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);

			using (IDataReader dr = dbProvider.ExecuteReader(command))
			{
				while (dr.Read())
				{
					entitys.Add(helper.Create(dr));
				}
			}

			return entitys;
		}

		public List<SgdEstadisticasDTO> GetByCriteria()
		{
			string sqlQuery = string.Format(helper.SqlGetByCriteria);
			List<SgdEstadisticasDTO> entitys = new List<SgdEstadisticasDTO>();
			DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

			using (IDataReader dr = dbProvider.ExecuteReader(command))
			{
				while (dr.Read())
				{
					entitys.Add(helper.Create(dr));
				}
			}

			return entitys;
		}

        //ListCodigosRef(fechaInicio,fechaFin);
        public List<SgdEstadisticasDTO> ListCodigosRef(DateTime fechaini, DateTime fechafin)
        {
            List<SgdEstadisticasDTO> entitys = new List<SgdEstadisticasDTO>();
            string stQuery = string.Format(helper.SqlListCodiRef, ((DateTime)(fechaini)).ToString("dd/MM/yyyy"), ((DateTime)(fechafin)).ToString("dd/MM/yyyy"));
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateCodiref(dr));
                }
            }

            return entitys;
        }

        public void UpdateCodiref(int fljcodi,int fljcodiref)
        {
            string stQuery = string.Format(helper.SqlUpdateCodiRef);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.AddInParameter(command, helper.Fljdetcodiref, DbType.Int32, fljcodiref);
            dbProvider.AddInParameter(command, helper.Fljcodi, DbType.Int32, fljcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateNumext(int fljcodiresp, int fljcodi)
        {
            string stQuery = string.Format(helper.SqlUpdateNumext, fljcodiresp, fljcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
