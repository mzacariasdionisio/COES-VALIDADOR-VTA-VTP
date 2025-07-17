using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.DTO.Scada;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla C_POINTS
    /// </summary>
    public class CPointsRepository: RepositoryBase, ICPointsRepository
    {
        public CPointsRepository(string strConn): base(strConn)
        {
        }

        CPointsHelper helper = new CPointsHelper();
        TrCanalSp7Helper helperCanalTrcoes = new TrCanalSp7Helper();
        TrEmpresaSp7Helper helperEmpresa = new TrEmpresaSp7Helper();

        public void Update(CPointsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.PointNumber, DbType.Int32, entity.PointNumber);
            dbProvider.AddInParameter(command, helper.PointName, DbType.String, entity.PointName);
            dbProvider.AddInParameter(command, helper.PointType, DbType.String, entity.PointType);
            dbProvider.AddInParameter(command, helper.Active, DbType.String, entity.Active);
            dbProvider.AddInParameter(command, helper.AorId, DbType.Int32, entity.AorId);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public CPointsDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            CPointsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CPointsDTO> List()
        {
            List<CPointsDTO> entitys = new List<CPointsDTO>();
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

        public List<CPointsDTO> GetByCriteria()
        {
            List<CPointsDTO> entitys = new List<CPointsDTO>();
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

        public List<TrCanalSp7DTO> ObtenerCanalesTrcoes()
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCanalesTrcoes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helperCanalTrcoes.CreateFromTrcoes(dr));
                }
            }

            return entitys;
        }

        public void CrearCanalConDataDeScada(string usuario, CPointsDTO cpoint)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlCrearCanalConDataDeScada, cpoint.PointNumber, cpoint.PointName, cpoint.PointType, usuario));
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarCanalConDataDeScada(string usuario, CPointsDTO cpoint)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCanalConDataDeScada, cpoint.PointName, cpoint.PointType, usuario, cpoint.PointNumber));
            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminadoLogicoDeCanales(string usuario, int canalcodi)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminadoLogicoDeCanales, usuario, canalcodi));
            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxCodigoTrCargaArchXmlSp7()
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetMaxCodigoTrCargaArchXmlSp7));
            object result = dbProvider.ExecuteScalar(command);
            return Convert.ToInt32(result);
        }

        public void GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio, int cantidadActualizados, string usuario, string fileXml, int tipo)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales, codigoMax, fechaInicio.ToString("yyyy-MM-dd HH:mm:ss"), cantidadActualizados, usuario, fileXml, tipo));
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCanalCambioSiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlUpdateCanalCambioSiHayActualizacionDeCanales, codigoMax, fechaInicio.ToString("yyyy-MM-dd HH:mm:ss")));
            dbProvider.ExecuteNonQuery(command);
        }


        public List<TrEmpresaSp7DTO> ObtenerEmpresasDesdeTrcoes()
        {
            List<TrEmpresaSp7DTO> entitys = new List<TrEmpresaSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasDesdeTrcoes);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helperEmpresa.CreateFromTrcoes(dr));
                }
            }

            return entitys;
        }

        public void GenerarEmpresasEnTrcoes(int emprcodi, string emprdescripcion, string siid, string usuario)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGenerarEmpresasEnTrcoes, emprcodi, emprdescripcion, siid, usuario));
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarCanalesIccpXml(string canaliccp, string pathb, string usuario, string canalremota, string canalcontenedor, string canalenlace, int canalcodigo)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCanalesIccpXml, canaliccp, pathb, usuario, canalremota, canalcontenedor, canalenlace, canalcodigo));
            dbProvider.ExecuteNonQuery(command);
        }

        public String ObtenerTotalZonasPorZonaId(int zonaId)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerTotalZonasPorZonaId, zonaId));
            object result = dbProvider.ExecuteScalar(command);
            return result.ToString();
        }

        public String ObtenerTotalEmpresaPorEmprcodi(int emprcodi)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerTotalEmpresaPorEmprcodi, emprcodi));
            object result = dbProvider.ExecuteScalar(command);
            return result.ToString();
        }

        public void GenerarRegistroZona(int zonacodi, string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGenerarRegistroZona, zonacodi, zonaabrev, zonanomb, emprcodi, zonasiid, usuario));
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarRegistroZona(string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario, int zonacodi)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarRegistroZona, zonaabrev, zonanomb, emprcodi, zonasiid, usuario, zonacodi));
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarCanalSiid(int emprcodi, int zonacodi, string canalunidad, string usuario, int canalcodi)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCanalSiid, emprcodi, zonacodi, canalunidad, usuario, canalcodi));
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
