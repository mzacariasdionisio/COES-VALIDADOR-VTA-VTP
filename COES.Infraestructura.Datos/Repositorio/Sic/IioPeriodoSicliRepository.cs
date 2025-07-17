using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CLO_PERIODO_REMISION
    /// </summary>
    public class IioPeriodoSicliRepository : RepositoryBase, IIioPeriodoSicliRepository
    {
        private readonly IioPeriodoSicliHelper helper = new IioPeriodoSicliHelper();

        public IioPeriodoSicliRepository(string strConn) : base(strConn)
        {
            
        }

        public List<IioPeriodoSicliDTO> GetByCriteria(string anio)
        {
            var entities = new List<IioPeriodoSicliDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.Anio, DbType.String, anio);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    var entity = helper.Create(dataReader);
                    

                    int iPsicliAnioMesPerrem = dataReader.GetOrdinal(IioPeriodoSicliHelper.PSicliFecSincronizacion);
                    if (!dataReader.IsDBNull(iPsicliAnioMesPerrem)) entity.PSicliFecSincronizacion = dataReader.GetDateTime(iPsicliAnioMesPerrem);

                    int iTablasEmpresasProcesar = dataReader.GetOrdinal(IioPeriodoSicliHelper.TablasEmpresasProcesar);
                    if (!dataReader.IsDBNull(iTablasEmpresasProcesar)) entity.TablasEmpresasProcesar = dataReader.GetInt32(iTablasEmpresasProcesar);


                    entities.Add(entity);
                }
            }
            return entities;
        }

        public IioPeriodoSicliDTO GetById(IioPeriodoSicliDTO iioPeriodoSicliDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliAnioMesPerrem, DbType.String,
                iioPeriodoSicliDto.PsicliAnioMesPerrem);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return (dataReader.Read() ? helper.Create(dataReader) : null);
            }
        }

        public void Save(IioPeriodoSicliDTO iioPeriodoSicliDto)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliAnioMesPerrem, DbType.String, iioPeriodoSicliDto.PsicliAnioMesPerrem);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliEstado, DbType.String, iioPeriodoSicliDto.PsicliEstado);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliEstRegistro, DbType.String, iioPeriodoSicliDto.PsicliEstRegistro);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliUsuCreacion, DbType.String, iioPeriodoSicliDto.PsicliUsuCreacion);
            dbProvider.ExecuteNonQuery(command);
        }
        
        public List<string> ListAnios()
        {
            var anios = new List<string>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlListAnios);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    anios.Add(dataReader.GetString(0));
                }
            }
            return anios;
        }

        public void Update(IioPeriodoSicliDTO iioPeriodoSicliDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliAnioMesPerrem, DbType.String, iioPeriodoSicliDto.PsicliAnioMesPerrem);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliFecUltActCoes, DbType.DateTime, iioPeriodoSicliDto.PsicliFecUltActCoes);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliFecUltActOsi, DbType.DateTime, iioPeriodoSicliDto.PsicliFecUltActOsi);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliEstado, DbType.String, iioPeriodoSicliDto.PsicliEstado);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliEstRegistro, DbType.String, iioPeriodoSicliDto.PsicliEstRegistro);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliUsuModificacion, DbType.String, iioPeriodoSicliDto.PsicliUsuModificacion);            
            //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento. El orden de los parámetros es clave para que funcione.
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCerrado, DbType.String, iioPeriodoSicliDto.PSicliCerrado);
            //- HDT Fin
            //dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PSicliFecSincronizacion, DbType.DateTime, iioPeriodoSicliDto.PSicliFecSincronizacion);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, iioPeriodoSicliDto.PsicliCodi);

            int filasAfectadas = dbProvider.ExecuteNonQuery(command);
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public List<IioPeriodoSicliDTO> ListaPeriodoActivo()
        {
            var listaPeriodo = new List<IioPeriodoSicliDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlListaPeriodoActivo);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    IioPeriodoSicliDTO iioPeriodoSicliDTO = helper.Create(dataReader);
                    int ianho = Int32.Parse(iioPeriodoSicliDTO.PsicliAnioMesPerrem.Substring(0, 4));
                    int imes = Int32.Parse(iioPeriodoSicliDTO.PsicliAnioMesPerrem.Substring(4));
                    DateTime fechaProceso = new DateTime(ianho, imes, 1);
                    string fechaEtiqueta = fechaProceso.ToString("MM yyyy");

                    iioPeriodoSicliDTO.EtiquetaPeriodo = fechaEtiqueta;

                    listaPeriodo.Add(iioPeriodoSicliDTO);
                }
            }
            return listaPeriodo;
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public IioPeriodoSicliDTO PeriodoGetByCodigo(int pSicliCodi)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, pSicliCodi);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return (dataReader.Read() ? helper.Create(dataReader) : null);
            }
        }

        // Cambio Mejoras PR16 05/08/2019
        public IioPeriodoSicliDTO PeriodoGetByCodigo(string periodoSicli)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodo);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliAnioMesPerrem, DbType.String, periodoSicli);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return (dataReader.Read() ? helper.Create(dataReader) : null);
            }
        }
    }
}