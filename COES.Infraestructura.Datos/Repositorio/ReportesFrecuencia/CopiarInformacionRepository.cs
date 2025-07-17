using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class CopiarInformacionRepository : RepositoryBase, ICopiarInformacionRepository
    {
        public CopiarInformacionRepository(string strConn) : base(strConn)
        {
        }

        CopiarInformacionHelper helper = new CopiarInformacionHelper();

        public CopiarInformacionDTO GetById(int IdCopia)
        {
            CopiarInformacionDTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.IdCopia, DbType.Int32, IdCopia);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }

        public List<CopiarInformacionDTO> GetListaCopiarInformacion(string FechaInicial, string FechaFinal, string CodEquipoOrigen, string CodEquipoDestino)
        {
            int intCodEquipoOrigen = 0;
            if (!string.IsNullOrEmpty(CodEquipoOrigen))
            {
                intCodEquipoOrigen = Convert.ToInt32(CodEquipoOrigen);
            }
            int intCodEquipoDestino = 0;
            if (!string.IsNullOrEmpty(CodEquipoDestino))
            {
                intCodEquipoDestino = Convert.ToInt32(CodEquipoDestino);
            }
            List<CopiarInformacionDTO> entitys = new List<CopiarInformacionDTO>();
            var query = string.Format(helper.SqlListaCopiarInformacion,
                                           FechaInicial,
                                           FechaFinal,
                                           intCodEquipoOrigen,
                                           intCodEquipoDestino
                                           );
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

        

        public CopiarInformacionDTO SaveUpdate(CopiarInformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            var query = string.Format(helper.SqlSave, id,
                                            entity.GPSCodiOrigen,
                                            entity.GPSCodiDest,
                                            entity.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                                            entity.FechaHoraFin.ToString("dd/MM/yyyy HH:mm:ss"),
                                            entity.Motivo,
                                            entity.Estado,
                                            entity.UsuarioCreacion
                                            );

            DbCommand command2 = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command2);

            this.GetListaLecturaOrigen(entity.FechaHoraInicio.ToString("dd-MM-yyyy HH:mm:ss"), entity.FechaHoraFin.ToString("dd-MM-yyyy HH:mm:ss"), entity.GPSCodiOrigen.ToString(), entity.GPSCodiDest.ToString());

            return entity;
        }

        public List<LecturaDTO> GetListaLecturaOrigen(string FechaInicial, string FechaFinal, string CodEquipoOrigen, string CodEquipoDestino)
        {
            int intCodEquipoOrigen = 0;
            if (!string.IsNullOrEmpty(CodEquipoOrigen))
            {
                intCodEquipoOrigen = Convert.ToInt32(CodEquipoOrigen);
            }
            int intCodEquipoDestino = 0;
            if (!string.IsNullOrEmpty(CodEquipoDestino))
            {
                intCodEquipoDestino = Convert.ToInt32(CodEquipoDestino);
            }
            List<LecturaDTO> entitys = new List<LecturaDTO>();
            var query = string.Format(helper.SqlListaLecturaOrigen,
                                           intCodEquipoOrigen,
                                           FechaInicial,
                                           FechaFinal
                                           );
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.CreateLecturaOrigen(dr));
                    LecturaDTO lecturaDTO = new LecturaDTO();
                    lecturaDTO = helper.CreateLecturaOrigen(dr);
                    lecturaDTO.FecHora = lecturaDTO.FechaHora.ToString("dd-MM-yyyy HH:mm:ss");
                    lecturaDTO.GPSCodi = intCodEquipoDestino;
                    this.SaveLectura(lecturaDTO);

                }
            }
            return entitys;
        }

        public LecturaDTO SaveLectura(LecturaDTO entity)
        {
            string query = string.Format(helper.SqlSaveLectura, entity.FecHora, entity.GPSCodi, entity.VSF, entity.Maximo, entity.Minimo, entity.Voltaje, entity.Num, entity.Desv, entity.H0, entity.H1, entity.H2, entity.H3, entity.H4, entity.H5, entity.H6, entity.H7, entity.H8, entity.H9, entity.H10, entity.H11, entity.H12, entity.H13, entity.H14, entity.H15, entity.H16, entity.H17, entity.H18, entity.H19, entity.H20, entity.H21, entity.H22, entity.H23, entity.H24, entity.H25, entity.H26, entity.H27, entity.H28, entity.H29, entity.H30, entity.H31, entity.H32, entity.H33, entity.H34, entity.H35, entity.H36, entity.H37, entity.H38, entity.H39, entity.H40, entity.H41, entity.H42, entity.H43, entity.H44, entity.H45, entity.H46, entity.H47, entity.H48, entity.H49, entity.H50, entity.H51, entity.H52, entity.H53, entity.H54, entity.H55, entity.H56, entity.H57, entity.H58, entity.H59, entity.DevSec);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            return entity;
        }

        public int GrabarLectura(CopiarInformacionDTO entity)
        {
            var query = string.Format(helper.SqlSaveLectura, entity.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm:ss")
                , entity.FechaHoraFin.ToString("dd/MM/yyyy HH:mm:ss"), entity.GPSCodiOrigen, entity.GPSCodiDest, entity.UsuarioCreacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddOutParameter(command, "P_MENSAJE", DbType.String, 200);
            return dbProvider.ExecuteNonQuery(command);
            //var aa = dbProvider.GetParameterValue(command, "P_MENSAJE") == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, "P_MENSAJE");
        }

        public CopiarInformacionDTO Eliminar(CopiarInformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.IdCopia, DbType.Int32, entity.IdCopia);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, "E");
            dbProvider.AddInParameter(command, helper.UsuCreacion, DbType.String, entity.UsuarioCreacion);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

    }
}
