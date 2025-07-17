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
    public class GmmDatInsumoRepository : RepositoryBase, IGmmDatInsumoRepository
    {
        public GmmDatInsumoRepository(string strConn)
           : base(strConn)
        {

        }

        GmmDatInsumoHelper helper = new GmmDatInsumoHelper();
        GmmDatCalculoHelper helper2 = new GmmDatCalculoHelper();

        public List<GmmDatInsumoDTO> ListarDatosInsumoTipoSC(int anio, string mes)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper.SqlListaDatosInsumoTipoSC, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaDatosInsumo(dr));
                }
            }
            return entities;
        }
        public void UpsertDatosInsumoTipoSC(GmmDatInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatosInsumoTipoSC);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Dinsanio, DbType.Int32, entity.DINSANIO);
            dbProvider.AddInParameter(command, helper.Dinsmes, DbType.String, entity.DINSMES);
            dbProvider.AddInParameter(command, helper.Dinsvalor, DbType.Decimal, entity.DINSVALOR);
            dbProvider.AddInParameter(command, helper.Dinsusuario, DbType.String, entity.DINSUSUARIO);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<GmmDatInsumoDTO> ListarDatosEntregas(int anio, string mes)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper.SqlListaDatosEntregas, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaDatosInsumo(dr));
                }
            }
            return entities;
        }
        public void UpsertDatosEntregas(GmmDatInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatosEntregas);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Dinsanio, DbType.Int32, entity.DINSANIO);
            dbProvider.AddInParameter(command, helper.Dinsmes, DbType.String, entity.DINSMES);
            dbProvider.AddInParameter(command, helper.Dinsvalor, DbType.Decimal, entity.DINSVALOR);
            dbProvider.AddInParameter(command, helper.Dinsusuario, DbType.String, entity.DINSUSUARIO);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<GmmDatInsumoDTO> ListarDatosInflexibilidad(int anio, string mes)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper.SqlListaDatosInflexibilidad, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaDatosInsumo(dr));
                }
            }
            return entities;
        }
        public void UpsertDatosInflexibilidad(GmmDatInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatosInflexibilidad);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Dinsanio, DbType.Int32, entity.DINSANIO);
            dbProvider.AddInParameter(command, helper.Dinsmes, DbType.String, entity.DINSMES);
            dbProvider.AddInParameter(command, helper.Dinsvalor, DbType.Decimal, entity.DINSVALOR);
            dbProvider.AddInParameter(command, helper.Dinsusuario, DbType.String, entity.DINSUSUARIO);

            dbProvider.ExecuteNonQuery(command);
        }
        public List<GmmDatInsumoDTO> ListarDatosRecaudacion(int anio, string mes)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper.SqlListaDatosRecaudacion, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaDatosInsumo(dr));
                }
            }
            return entities;
        }
        public List<GmmDatInsumoDTO> ListadoInsumos(int anio, string mes)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper.SqlListadoInsumos, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListadoInsumos(dr));
                }
            }
            return entities;
        }

        public void UpsertDatosRecaudacion(GmmDatInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatosRecaudacion);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Dinsanio, DbType.Int32, entity.DINSANIO);
            dbProvider.AddInParameter(command, helper.Dinsmes, DbType.String, entity.DINSMES);
            dbProvider.AddInParameter(command, helper.Dinsvalor, DbType.Decimal, entity.DINSVALOR);
            dbProvider.AddInParameter(command, helper.Dinsusuario, DbType.String, entity.DINSUSUARIO);
            //dbProvider.AddInParameter(command, helper.Dinsusuario, DbType.String, entity.EMPRCODI );
            dbProvider.ExecuteNonQuery(command);
        }

        public bool UpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo)
        {
            string queryString = string.Format(helper2.SqlSetValoresAdicionalesInsumos, entity.DINSANIO, entity.DINSMES,
                  0, entity.DINSUSUARIO, tipo, entity.DINSVALOR, entity.EMPRCODI, entity.EMPGCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int numberOfRecords = dbProvider.ExecuteNonQuery(command);
            if (numberOfRecords > 0)
                return true;
            else
                return false;
        }
        public bool ActualizarUpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo)
        {
            string queryString = string.Format(helper2.SqlActualizarDatosCalculo, tipo, entity.DINSANIO, entity.DINSMES, entity.EMPRCODI, entity.DINSVALOR);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int numberOfRecords = dbProvider.ExecuteNonQuery(command);
            if (numberOfRecords > 0)
                return true;
            else
                return false;
        }
        public bool EliminarUpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo)
        {
            string queryString = string.Format(helper2.SqlEliminarDatosCalculo, tipo, entity.DINSANIO, entity.DINSMES, entity.EMPRCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int numberOfRecords = dbProvider.ExecuteNonQuery(command);
            if (numberOfRecords > 0)
                return true;
            else
                return false;


        }
        public List<GmmDatInsumoDTO> ListaDatosCalculo(GmmDatInsumoDTO entity, string tipo)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper2.SqlListadoDatosCalculo, tipo, entity.DINSANIO, entity.DINSMES, entity.EMPRCODI);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper2.CreateListaDatosCalculo(dr));
                }
            }
            return entities;
        }

        public bool ConsultaParticipanteExistente(int empgcodi)
        {
            List<GmmDatInsumoDTO> entities = new List<GmmDatInsumoDTO>();
            string queryString = string.Format(helper2.SqlConsultaParticipanteExistente, empgcodi);
            bool respuesta = false;
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    return true;
                }
            }
            return respuesta;
        }

        public string ConsultaEstadoPeriodo(int anio, string mes)
        {
            string queryString = string.Format(helper2.SqlConsultaEstadoPeriodo, anio, mes);
            string Estado = "C";
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iPeriEstado = dr.GetOrdinal("PERIESTADO");
                    if (!dr.IsDBNull(iPeriEstado)) Estado = dr.GetString(iPeriEstado);

                }
            }
            return Estado;
        }
    }

}

