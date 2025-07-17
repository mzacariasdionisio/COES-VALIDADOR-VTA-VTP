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
    public class EtapaERARepository : RepositoryBase, IEtapaERARepository
    {
        public EtapaERARepository(string strConn) : base(strConn)
        {
        }

        EtapaERAHelper helper = new EtapaERAHelper();

        public EtapaERADTO GetById(int EtapaCodi)
        {
            EtapaERADTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.EtapaCodi, DbType.Int32, EtapaCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }

        public List<EtapaERADTO> GetListaEtapas()
        {

            List<EtapaERADTO> entitys = new List<EtapaERADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaEtapas);
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

        public int ValidarNombreEtapa(EtapaERADTO entity)
        {
            int resultado = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarNombreEtapa);

            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.NombreEtapa);
            dbProvider.AddInParameter(command, helper.EtapaCodi, DbType.Int32, entity.EtapaCodi);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.Int32, 11);
            dbProvider.ExecuteNonQuery(command);
            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? 0 : (Int32)dbProvider.GetParameterValue(command, helper.Resultado);
            return resultado;
        }

        public EtapaERADTO SaveUpdate(EtapaERADTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdate);

            dbProvider.AddInParameter(command, helper.EtapaCodi, DbType.Int32, entity.EtapaCodi);
            dbProvider.AddInParameter(command, helper.Nombre, DbType.String, entity.NombreEtapa);
            dbProvider.AddInParameter(command, helper.Umbral, DbType.Decimal, entity.Umbral);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.EtapaEstado);
            dbProvider.AddInParameter(command, helper.UsuCreacion, DbType.String, entity.UsuarioRegi);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

        public EtapaERADTO Eliminar(EtapaERADTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.EtapaCodi, DbType.Int32, entity.EtapaCodi);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }
    }
}
