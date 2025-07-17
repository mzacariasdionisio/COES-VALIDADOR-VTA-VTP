using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamPeriodoRepository : RepositoryBase, ICamPeriodoRepository
    {

        public CamPeriodoRepository(string strConn) : base(strConn) 
        { }

        CamPeriodoHelper Helper = new CamPeriodoHelper();
        CamDetallePeriodoHelper DetalleHelper = new CamDetallePeriodoHelper();

        public List<PeriodoDTO> GetPeriodos()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetListaPeriodos);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PeriodoDTO ob = new PeriodoDTO();
                    ob.PeriCodigo = Int32.Parse(dr["PERICODI"].ToString());
                    ob.PeriNombre = dr["PERINOMBRE"].ToString();
                    ob.PeriFechaInicio = DateTime.Parse(dr["PERIFECHAINICIO"].ToString());
                    ob.PeriFechaFin = DateTime.Parse(dr["PERIFECHAFIN"].ToString());
                    ob.PeriHoraFin = DateTime.Parse(dr["PERIHORAFIN"].ToString());
                    ob.PeriHorizonteAtras = Int32.Parse(dr["PERIHORIZONTEATRAS"].ToString());
                    ob.PeriHorizonteAdelante = Int32.Parse(dr["PERIHORIZONTEADELANTE"].ToString());
                    ob.PeriEstado = dr["PERIESTADO"] != null && dr["PERIESTADO"].ToString().Equals(Constantes.Activo) ? Constantes.Activo : Constantes.Cerrado;
                    ob.PeriComentario = dr["PERICOMENTARIO"].ToString();
                    entitys.Add(ob);
                }
            }

            return entitys;
        }

        public List<PeriodoDTO> GetPeriodosByAnioAndEstado(int anio, string estado)
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetListaPeriodosByAnioAndEstado);
            dbProvider.AddInParameter(command, "ANIOINI", DbType.Int32, anio);
            dbProvider.AddInParameter(command, "ANIO", DbType.Int32, anio);
            dbProvider.AddInParameter(command, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PeriodoDTO ob = new PeriodoDTO();
                    ob.PeriCodigo = Int32.Parse(dr["PERICODI"].ToString());
                    ob.PeriNombre = dr["PERINOMBRE"].ToString();
                    ob.PeriFechaInicio = DateTime.Parse(dr["PERIFECHAINICIO"].ToString());
                    ob.PeriFechaFin = DateTime.Parse(dr["PERIFECHAFIN"].ToString());
                    ob.PeriHoraFin = DateTime.Parse(dr["PERIHORAFIN"].ToString());
                    ob.PeriHorizonteAtras = Int32.Parse(dr["PERIHORIZONTEATRAS"].ToString());
                    ob.PeriHorizonteAdelante = Int32.Parse(dr["PERIHORIZONTEADELANTE"].ToString());
                    ob.PeriEstado = dr["PERIESTADO"] != null && dr["PERIESTADO"].ToString().Equals(Constantes.Activo) ? Constantes.Activo : Constantes.Cerrado;
                    ob.PeriComentario = dr["PERICOMENTARIO"].ToString();
                    entitys.Add(ob);
                }
            }

            return entitys;
        }

        public List<PeriodoDTO> GetPeriodosByAnio(int anio)
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetListaPeriodosByAnio);
            dbProvider.AddInParameter(command, "ANIOINI", DbType.Int32, anio);
            dbProvider.AddInParameter(command, "ANIO", DbType.Int32, anio);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PeriodoDTO ob = new PeriodoDTO();
                    ob.PeriCodigo = Int32.Parse(dr["PERICODI"].ToString());
                    ob.PeriNombre = dr["PERINOMBRE"].ToString();
                    ob.PeriFechaInicio = DateTime.Parse(dr["PERIFECHAINICIO"].ToString());
                    ob.PeriFechaFin = DateTime.Parse(dr["PERIFECHAFIN"].ToString());
                    ob.PeriHoraFin = DateTime.Parse(dr["PERIHORAFIN"].ToString());
                    ob.PeriHorizonteAtras = Int32.Parse(dr["PERIHORIZONTEATRAS"].ToString());
                    ob.PeriHorizonteAdelante = Int32.Parse(dr["PERIHORIZONTEADELANTE"].ToString());
                    ob.PeriEstado = dr["PERIESTADO"] != null && dr["PERIESTADO"].ToString().Equals(Constantes.Activo) ? Constantes.Activo : Constantes.Cerrado;
                    ob.PeriComentario = dr["PERICOMENTARIO"].ToString();
                    entitys.Add(ob);
                }
            }

            return entitys;
        }

        public bool SavePeriodo(PeriodoDTO periodoDTO)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SQLSave);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, periodoDTO.PeriCodigo);
            dbProvider.AddInParameter(dbcommad, "PERINOMBRE", DbType.String, periodoDTO.PeriNombre);
            dbProvider.AddInParameter(dbcommad, "PERIFECHAINICIO", DbType.DateTime, periodoDTO.PeriFechaInicio);
            dbProvider.AddInParameter(dbcommad, "PERIFECHAFIN", DbType.DateTime, periodoDTO.PeriFechaFin);
            dbProvider.AddInParameter(dbcommad, "PERIHORAFIN", DbType.DateTime, periodoDTO.PeriHoraFin);
            dbProvider.AddInParameter(dbcommad, "PERIESTADO", DbType.String, periodoDTO.PeriEstado);
            dbProvider.AddInParameter(dbcommad, "PERIHORIZONTEATRAS", DbType.Int32, periodoDTO.PeriHorizonteAtras);
            dbProvider.AddInParameter(dbcommad, "PERIHORIZONTEADELANTE", DbType.Int32, periodoDTO.PeriHorizonteAdelante);
            dbProvider.AddInParameter(dbcommad, "PERICOMENTARIO", DbType.String, periodoDTO.PeriComentario);
            dbProvider.AddInParameter(dbcommad, "USU_CREACION", DbType.String, periodoDTO.UsuarioCreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, periodoDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public int GetPeriodoId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetPeriodoId);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }


        public bool DeletePeriodoById(int pericodi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeletePeriodoById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, pericodi);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public PeriodoDTO GetPeriodoById(int pericodi)
        {
            PeriodoDTO ob = new PeriodoDTO();
            DbCommand commandPeriodo = dbProvider.GetSqlStringCommand(Helper.SqlGetPeriodoById);
            dbProvider.AddInParameter(commandPeriodo, "ID", DbType.Int32, pericodi);
            dbProvider.AddInParameter(commandPeriodo, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandPeriodo);
            using (IDataReader dr = dbProvider.ExecuteReader(commandPeriodo))
            {
                if (dr.Read())
                {
                    ob.PeriCodigo = Int32.Parse(dr["PERICODI"].ToString());
                    ob.PeriNombre = dr["PERINOMBRE"].ToString();
                    ob.PeriFechaInicio = DateTime.Parse(dr["PERIFECHAINICIO"].ToString());
                    ob.PeriFechaFin = DateTime.Parse(dr["PERIFECHAFIN"].ToString());
                    ob.PeriHoraFin = DateTime.Parse(dr["PERIHORAFIN"].ToString());
                    ob.PeriHorizonteAtras = Int32.Parse(dr["PERIHORIZONTEATRAS"].ToString());
                    ob.PeriHorizonteAdelante = Int32.Parse(dr["PERIHORIZONTEADELANTE"].ToString());
                    ob.PeriEstado = dr["PERIESTADO"].ToString();
                    ob.PeriComentario = dr["PERICOMENTARIO"].ToString();
                }
            }

            DbCommand commandFicha = dbProvider.GetSqlStringCommand(DetalleHelper.SqlGetDetalleByPericodi);
            dbProvider.AddInParameter(commandFicha, "PERICODI", DbType.String, pericodi.ToString());
            dbProvider.AddInParameter(commandFicha, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandFicha);
            List<DetallePeriodoDTO> detallePeriodoDTOs = new List<DetallePeriodoDTO>();

            using (IDataReader drF = dbProvider.ExecuteReader(commandFicha))
            {
                while (drF.Read())
                {
                    DetallePeriodoDTO obF = new DetallePeriodoDTO();
                    obF.DetPeriCodigo = Int32.Parse(drF["DETPERICODI"].ToString());
                    obF.PeriCodigo = Int32.Parse(drF["PERICODI"].ToString());
                    obF.HojaCodigo = Int32.Parse(drF["HOJACODI"].ToString());
                    detallePeriodoDTOs.Add(obF);
                }
            }

            ob.ListaDetallePeriodo = detallePeriodoDTOs;
            return ob;
        }

        public bool UpdatePeriodo(PeriodoDTO periodoDTO)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SqlUpdatePeriodo);
            dbProvider.AddInParameter(dbcommad, "PERINOMBRE", DbType.String, periodoDTO.PeriNombre);
            dbProvider.AddInParameter(dbcommad, "PERIFECHAINICIO", DbType.DateTime, periodoDTO.PeriFechaInicio);
            dbProvider.AddInParameter(dbcommad, "PERIFECHAFIN", DbType.DateTime, periodoDTO.PeriFechaFin);
            dbProvider.AddInParameter(dbcommad, "PERIHORAFIN", DbType.DateTime, periodoDTO.PeriHoraFin);
            dbProvider.AddInParameter(dbcommad, "PERIESTADO", DbType.String, periodoDTO.PeriEstado);
            dbProvider.AddInParameter(dbcommad, "PERIHORIZONTEATRAS", DbType.Int32, periodoDTO.PeriHorizonteAtras);
            dbProvider.AddInParameter(dbcommad, "PERIHORIZONTEADELANTE", DbType.Int32, periodoDTO.PeriHorizonteAdelante);
            dbProvider.AddInParameter(dbcommad, "PERICOMENTARIO", DbType.String, periodoDTO.PeriComentario);
            dbProvider.AddInParameter(dbcommad, "USU_MODIFICACION", DbType.String, periodoDTO.UsuarioModificacion);
            dbProvider.AddInParameter(dbcommad, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, periodoDTO.PeriCodigo);

            dbProvider.ExecuteNonQuery(dbcommad);

            return true;
        }

        public int GetPeriodoByDate(DateTime perifechaini, DateTime perifechafin, int id)
        {
            int count = 0;
            DbCommand commandPeriodo = dbProvider.GetSqlStringCommand(Helper.SqlGetPeriodoByDate);
            dbProvider.AddInParameter(commandPeriodo, "PERIFECHAINICIO", DbType.DateTime, perifechaini);
            dbProvider.AddInParameter(commandPeriodo, "PERIFECHAFIN", DbType.DateTime, perifechafin);
            dbProvider.AddInParameter(commandPeriodo, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(commandPeriodo, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandPeriodo);
            object result = dbProvider.ExecuteScalar(commandPeriodo);

            if (result != null)
            {
                count = Convert.ToInt32(result);
            }
            else
            {
                count = 1;
            }
            return count;
        }

    }
}
