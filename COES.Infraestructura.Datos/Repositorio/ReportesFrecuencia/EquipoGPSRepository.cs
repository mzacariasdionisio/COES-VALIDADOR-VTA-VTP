using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class EquipoGPSRepository : RepositoryBase, IEquipoGPSRepository
    {
        public EquipoGPSRepository(string strConn) : base(strConn)
        {
        }

        EquipoGPSHelper helper = new EquipoGPSHelper();
        
        public EquipoGPSDTO GetById(int GPSCodi)
        {
            EquipoGPSDTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.GPSCodi, DbType.Int32, GPSCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }

        /*public List<PeriodoDeclaracionDTO> GetListaCombobox()
        {
            List<PeriodoDeclaracionDTO> entitys = new List<PeriodoDeclaracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCombobox);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }*/

        public List<EquipoGPSDTO> GetListaEquipoGPS()
        {

            List<EquipoGPSDTO> entitys = new List<EquipoGPSDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaEquipoGPS);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<EquipoGPSDTO> GetListaEquipoGPSPorFiltro(int codEquipo, string IndOficial)
        {

            List<EquipoGPSDTO> entitys = new List<EquipoGPSDTO>();
            string strIndOficial = string.Empty;
            if (string.IsNullOrEmpty(IndOficial))
            {
                strIndOficial = "";
            } else
            {
                strIndOficial = IndOficial;
            }
            var query = string.Format(helper.SqlListaEquipoGPSPorFiltro,
                                            codEquipo,
                                            strIndOficial);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public int GetUltimoCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUltimoCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetNumeroRegistrosPorEquipo(int GPSCodi)
        {
            int numRegistros = 0;
            var query = string.Format(helper.SqlNumRegistrosPorEquipo, GPSCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            numRegistros = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());
            return numRegistros;
        }

        public int ValidarNombreEquipoGPS(EquipoGPSDTO entity)
        {
            int resultado = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarNombreEquipoGPS);

            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.NombreEquipo);
            dbProvider.AddInParameter(command, helper.GPSCodi, DbType.Int32, entity.GPSCodiOriginal);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.Int32, 11);
            dbProvider.ExecuteNonQuery(command);
            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? 0 : (Int32)dbProvider.GetParameterValue(command, helper.Resultado);
            return resultado;
        }

        public EquipoGPSDTO SaveUpdate(EquipoGPSDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdate);

            dbProvider.AddInParameter(command, helper.GPSCodi, DbType.Int32, entity.GPSCodiOriginal);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmpresaCodi);
            dbProvider.AddInParameter(command, helper.EquiCodi, DbType.Int32, entity.EquipoCodi);
            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.NombreEquipo);
            dbProvider.AddInParameter(command, helper.GPSOficial, DbType.String, entity.GPSOficial);
            dbProvider.AddInParameter(command, helper.GPSOSINERG, DbType.String, entity.GPSOSINERG);
            dbProvider.AddInParameter(command, helper.GPSEstado, DbType.String, entity.GPSEstado);
            dbProvider.AddInParameter(command, helper.GPSTipo, DbType.String, entity.GPSTipo);
            dbProvider.AddInParameter(command, helper.GPSGenAlarma, DbType.String, entity.GPSGenAlarma);
            //dbProvider.AddInParameter(command, helper.GPSFecCreacion, DbType.DateTime, System.DateTime.Now);
            dbProvider.AddInParameter(command, helper.GPSUsuCreacion, DbType.String, entity.GPSUsuarioRegi);
            dbProvider.AddInParameter(command, helper.RutaFile, DbType.String, entity.RutaFile);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

        public EquipoGPSDTO Eliminar(EquipoGPSDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.GPSCodi, DbType.Int32, entity.GPSCodiOriginal);
            dbProvider.AddInParameter(command, helper.GPSEstado, DbType.String, "E");
            dbProvider.AddInParameter(command, helper.GPSUsuCreacion, DbType.String, entity.GPSUsuarioRegi);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

    }
}
