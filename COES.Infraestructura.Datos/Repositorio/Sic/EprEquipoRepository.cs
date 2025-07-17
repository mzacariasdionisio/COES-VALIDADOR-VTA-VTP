using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPR_EQUIPO
    /// </summary>
    public class EprEquipoRepository : RepositoryBase, IEprEquipoRepository
    {
        public EprEquipoRepository(string strConn) : base(strConn)
        {
        }

        EprEquipoHelper helper = new EprEquipoHelper();

        public int Save(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Epequicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Epequinomb, DbType.String, entity.Epequinomb);
            dbProvider.AddInParameter(command, helper.Epequiestregistro, DbType.String, entity.Epequiestregistro);
            dbProvider.AddInParameter(command, helper.Epequiusucreacion, DbType.String, entity.Epequiusucreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Epequinomb, DbType.String, entity.Epequinomb);
            dbProvider.AddInParameter(command, helper.Epequiestregistro, DbType.String, entity.Epequiestregistro);
            dbProvider.AddInParameter(command, helper.Epequiusumodificacion, DbType.String, entity.Epequiusumodificacion);
            dbProvider.AddInParameter(command, helper.Epequicodi, DbType.Int32, entity.Epequicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Epequiestregistro, DbType.String, entity.Epequiestregistro);
            dbProvider.AddInParameter(command, helper.Epequiusumodificacion, DbType.String, entity.Epequiusumodificacion);
            dbProvider.AddInParameter(command, helper.Epequicodi, DbType.Int32, entity.Epequicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EprEquipoDTO GetById(int epequicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Epequicodi, DbType.Int32, epequicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EprEquipoDTO> ListArbol(int Idzona, string Ubicacion)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListArbol); 
            dbProvider.AddInParameter(command, helper.Idzona, DbType.Int32, Idzona);
            dbProvider.AddInParameter(command, helper.Idzona, DbType.Int32, Idzona);
            dbProvider.AddInParameter(command, helper.Ubicacion, DbType.String, Ubicacion);
            dbProvider.AddInParameter(command, helper.Ubicacion, DbType.String, Ubicacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodipadre = dr.GetOrdinal(helper.Equicodipadre);
                    if (!dr.IsDBNull(iEquicodipadre)) entity.Equicodipadre = Convert.ToInt32(dr.GetValue(iEquicodipadre));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iNivel = dr.GetOrdinal(helper.Nivel);
                    if (!dr.IsDBNull(iNivel)) entity.Nivel = Convert.ToInt32(dr.GetValue(iNivel));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListCelda(int areacodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCelda);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.String, areacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListInterruptor(int areacodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListInterruptor);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.String, areacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO GetByIdCelda(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetIdCelda);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                }
            }

            return entity;
        }

        public string UpdateRele(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.UpdateRele);

            dbProvider.AddInParameter(command, helper.MemoriaCalculo, DbType.String, equipo.MemoriaCalculo);

            dbProvider.AddInParameter(command, helper.EquicodiRele, DbType.Int32, equipo.EquiCodiRele);
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.Int32, equipo.IdCelda);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, equipo.Codigo);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.IdTitular, DbType.Int32, equipo.IdTitular);
            dbProvider.AddInParameter(command, helper.Tension, DbType.String, equipo.Tension);
            dbProvider.AddInParameter(command, helper.IdSistermaRele, DbType.String, equipo.IdSistermaRele);
            dbProvider.AddInParameter(command, helper.IdMarca, DbType.String, equipo.IdMarca);
            dbProvider.AddInParameter(command, helper.Modelo, DbType.String, equipo.Modelo);

            dbProvider.AddInParameter(command, helper.IdTipoUso, DbType.String, equipo.IdTipoUso);
            dbProvider.AddInParameter(command, helper.RtcPrimario, DbType.String, equipo.RtcPrimario);
            dbProvider.AddInParameter(command, helper.RtcSecundario, DbType.String, equipo.RtcSecundario);
            dbProvider.AddInParameter(command, helper.RttPrimario, DbType.String, equipo.RttPrimario);
            dbProvider.AddInParameter(command, helper.RttSecundario, DbType.String, equipo.RttSecundario);

            dbProvider.AddInParameter(command, helper.ProtCondinables, DbType.String, equipo.ProtCondinables);
            dbProvider.AddInParameter(command, helper.SincroCheckActivo, DbType.String, equipo.SincroCheckActivo);
            dbProvider.AddInParameter(command, helper.IdInterruptor, DbType.String, equipo.IdInterruptor);
            dbProvider.AddInParameter(command, helper.DeltaTension, DbType.String, equipo.DeltaTension);
            dbProvider.AddInParameter(command, helper.DeltaAngulo, DbType.String, equipo.DeltaAngulo);
            dbProvider.AddInParameter(command, helper.DeltaFrecuencia, DbType.String, equipo.DeltaFrecuencia);
            dbProvider.AddInParameter(command, helper.SobreCCheckActivo, DbType.String, equipo.SobreCCheckActivo);

            dbProvider.AddInParameter(command, helper.SobreCI, DbType.String, equipo.SobreCI);
            dbProvider.AddInParameter(command, helper.SobreTCheckActivo, DbType.String, equipo.SobreTCheckActivo);

            dbProvider.AddInParameter(command, helper.SobreTU, DbType.String, equipo.SobreTU);
            dbProvider.AddInParameter(command, helper.SobreTT, DbType.String, equipo.SobreTT);
            dbProvider.AddInParameter(command, helper.SobreTUU, DbType.String, equipo.SobreTUU);
            dbProvider.AddInParameter(command, helper.SobreTTT, DbType.String, equipo.SobreTTT);


            dbProvider.AddInParameter(command, helper.PmuCheckActivo, DbType.String, equipo.PmuCheckActivo);
            dbProvider.AddInParameter(command, helper.PmuAccion, DbType.String, equipo.PmuAccion);
            dbProvider.AddInParameter(command, helper.IdInterruptorMS, DbType.String, equipo.IdInterruptorMS);
            dbProvider.AddInParameter(command, helper.IdMandoSincronizado, DbType.String, equipo.IdMandoSincronizado);
            dbProvider.AddInParameter(command, helper.MedidaMitigacion, DbType.String, equipo.MedidaMitigacion);
            dbProvider.AddInParameter(command, helper.ReleTorsImpl, DbType.String, equipo.ReleTorsImpl);
            dbProvider.AddInParameter(command, helper.RelePmuAccion, DbType.String, equipo.RelePmuAccion);
            dbProvider.AddInParameter(command, helper.RelePmuImpl, DbType.String, equipo.RelePmuImpl);
            dbProvider.AddInParameter(command, helper.Epequiusucreacion, DbType.String, equipo.Epequiusucreacion);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }

        public string SaveRele(EprEquipoDTO equipo)
        {

            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SaveRele);
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.Int32, equipo.IdCelda);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, equipo.Codigo);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.IdTitular, DbType.Int32, equipo.IdTitular);
            dbProvider.AddInParameter(command, helper.Tension, DbType.String, equipo.Tension);
            dbProvider.AddInParameter(command, helper.IdSistermaRele, DbType.String, equipo.IdSistermaRele);
            dbProvider.AddInParameter(command, helper.IdMarca, DbType.String, equipo.IdMarca);
            dbProvider.AddInParameter(command, helper.Modelo, DbType.String, equipo.Modelo);
            dbProvider.AddInParameter(command, helper.IdTipoUso, DbType.String, equipo.IdTipoUso);
            dbProvider.AddInParameter(command, helper.RtcPrimario, DbType.String, equipo.RtcPrimario);
            dbProvider.AddInParameter(command, helper.RtcSecundario, DbType.String, equipo.RtcSecundario);
            dbProvider.AddInParameter(command, helper.RttPrimario, DbType.String, equipo.RttPrimario);
            dbProvider.AddInParameter(command, helper.RttSecundario, DbType.String, equipo.RttSecundario);
            dbProvider.AddInParameter(command, helper.ProtCondinables, DbType.String, equipo.ProtCondinables);
            dbProvider.AddInParameter(command, helper.SincroCheckActivo, DbType.String, equipo.SincroCheckActivo);
            dbProvider.AddInParameter(command, helper.IdInterruptor, DbType.String, equipo.IdInterruptor);
            dbProvider.AddInParameter(command, helper.DeltaTension, DbType.String, equipo.DeltaTension);
            dbProvider.AddInParameter(command, helper.DeltaAngulo, DbType.String, equipo.DeltaAngulo);
            dbProvider.AddInParameter(command, helper.DeltaFrecuencia, DbType.String, equipo.DeltaFrecuencia);
            dbProvider.AddInParameter(command, helper.SobreCCheckActivo, DbType.String, equipo.SobreCCheckActivo);
            dbProvider.AddInParameter(command, helper.SobreCI, DbType.String, equipo.SobreCI);
            dbProvider.AddInParameter(command, helper.SobreTCheckActivo, DbType.String, equipo.SobreTCheckActivo);
            dbProvider.AddInParameter(command, helper.SobreTU, DbType.String, equipo.SobreTU);
            dbProvider.AddInParameter(command, helper.SobreTT, DbType.String, equipo.SobreTT);
            dbProvider.AddInParameter(command, helper.SobreTUU, DbType.String, equipo.SobreTUU);
            dbProvider.AddInParameter(command, helper.SobreTTT, DbType.String, equipo.SobreTTT);
            dbProvider.AddInParameter(command, helper.PmuCheckActivo, DbType.String, equipo.PmuCheckActivo);
            dbProvider.AddInParameter(command, helper.PmuAccion, DbType.String, equipo.PmuAccion);
            dbProvider.AddInParameter(command, helper.IdInterruptorMS, DbType.String, equipo.IdInterruptorMS);
            dbProvider.AddInParameter(command, helper.IdMandoSincronizado, DbType.String, equipo.IdMandoSincronizado);
            dbProvider.AddInParameter(command, helper.MedidaMitigacion, DbType.String, equipo.MedidaMitigacion);
            dbProvider.AddInParameter(command, helper.ReleTorsImpl, DbType.String, equipo.ReleTorsImpl);
            dbProvider.AddInParameter(command, helper.RelePmuAccion, DbType.String, equipo.RelePmuAccion);
            dbProvider.AddInParameter(command, helper.RelePmuImpl, DbType.String, equipo.RelePmuImpl);
            dbProvider.AddInParameter(command, helper.MemoriaCalculo, DbType.String, equipo.MemoriaCalculo);
            dbProvider.AddInParameter(command, helper.Epequiusucreacion, DbType.String, equipo.Epequiusucreacion);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;
        }

        public List<EprEquipoDTO> ListEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListEquipoProtGrilla);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, helper.NombSubestacion, DbType.String, nombSubestacion);
            dbProvider.AddInParameter(command, helper.NombSubestacion, DbType.String, nombSubestacion);
            dbProvider.AddInParameter(command, helper.Celda, DbType.String, celda);
            dbProvider.AddInParameter(command, helper.Celda, DbType.String, celda);
            dbProvider.AddInParameter(command, helper.Rele, DbType.String, rele);
            dbProvider.AddInParameter(command, helper.Rele, DbType.String, rele);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

          

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iSistema = dr.GetOrdinal(helper.Sistema);
                    if (!dr.IsDBNull(iSistema)) entity.Sistema = Convert.ToString(dr.GetValue(iSistema));

                    int iMarca = dr.GetOrdinal(helper.Marca);
                    if (!dr.IsDBNull(iMarca)) entity.Marca = Convert.ToString(dr.GetValue(iMarca));

                    int iModelo = dr.GetOrdinal(helper.Modelo);
                    if (!dr.IsDBNull(iModelo)) entity.Modelo = Convert.ToString(dr.GetValue(iModelo));

                    int iTipoUso = dr.GetOrdinal(helper.TipoUso);
                    if (!dr.IsDBNull(iTipoUso)) entity.TipoUso = Convert.ToString(dr.GetValue(iTipoUso));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        
         public List<EprEquipoDTO> ReporteEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ReporteEquipoProtGrilla);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, helper.NombSubestacion, DbType.String, nombSubestacion);
            dbProvider.AddInParameter(command, helper.NombSubestacion, DbType.String, nombSubestacion);
            dbProvider.AddInParameter(command, helper.Celda, DbType.String, celda);
            dbProvider.AddInParameter(command, helper.Celda, DbType.String, celda);
            dbProvider.AddInParameter(command, helper.Rele, DbType.String, rele);
            dbProvider.AddInParameter(command, helper.Rele, DbType.String, rele);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.String, nivel);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iSistema = dr.GetOrdinal(helper.Sistema);
                    if (!dr.IsDBNull(iSistema)) entity.Sistema = Convert.ToString(dr.GetValue(iSistema));

                    int iMarca = dr.GetOrdinal(helper.Marca);
                    if (!dr.IsDBNull(iMarca)) entity.Marca = Convert.ToString(dr.GetValue(iMarca));

                    int iModelo = dr.GetOrdinal(helper.Modelo);
                    if (!dr.IsDBNull(iModelo)) entity.Modelo = Convert.ToString(dr.GetValue(iModelo));

                    int iTipoUso = dr.GetOrdinal(helper.TipoUso);
                    if (!dr.IsDBNull(iTipoUso)) entity.TipoUso = Convert.ToString(dr.GetValue(iTipoUso));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iRtcPrimario = dr.GetOrdinal(helper.RtcPrimario);
                    if (!dr.IsDBNull(iRtcPrimario)) entity.RtcPrimario = Convert.ToString(dr.GetValue(iRtcPrimario));

                    int iRtcSecundario = dr.GetOrdinal(helper.RtcSecundario);
                    if (!dr.IsDBNull(iRtcSecundario)) entity.RtcSecundario = Convert.ToString(dr.GetValue(iRtcSecundario));

                    int iRttPrimario = dr.GetOrdinal(helper.RttPrimario);
                    if (!dr.IsDBNull(iRttPrimario)) entity.RttPrimario = Convert.ToString(dr.GetValue(iRttPrimario));

                    int iRttSecundario = dr.GetOrdinal(helper.RttSecundario);
                    if (!dr.IsDBNull(iRttSecundario)) entity.RttSecundario = Convert.ToString(dr.GetValue(iRttSecundario));

                    int iProtCondinables = dr.GetOrdinal(helper.ProtCondinables);
                    if (!dr.IsDBNull(iProtCondinables)) entity.ProtCondinables = Convert.ToString(dr.GetValue(iProtCondinables));

                    int iSincroCheckActivo = dr.GetOrdinal(helper.SincroCheckActivo);
                    if (!dr.IsDBNull(iSincroCheckActivo)) entity.SincroCheckActivo = Convert.ToString(dr.GetValue(iSincroCheckActivo));

                    int iIdInterruptor = dr.GetOrdinal(helper.IdInterruptor);
                    if (!dr.IsDBNull(iIdInterruptor)) entity.IdInterruptor = Convert.ToString(dr.GetValue(iIdInterruptor));

                    int iDeltaTension = dr.GetOrdinal(helper.DeltaTension);
                    if (!dr.IsDBNull(iDeltaTension)) entity.DeltaTension = Convert.ToString(dr.GetValue(iDeltaTension));

                    int iDeltaAngulo = dr.GetOrdinal(helper.DeltaAngulo);
                    if (!dr.IsDBNull(iDeltaAngulo)) entity.DeltaAngulo = Convert.ToString(dr.GetValue(iDeltaAngulo));

                    int iDeltaFrecuencia = dr.GetOrdinal(helper.DeltaFrecuencia);
                    if (!dr.IsDBNull(iDeltaFrecuencia)) entity.DeltaFrecuencia = Convert.ToString(dr.GetValue(iDeltaFrecuencia));

                    int iSobreTCheckActivo = dr.GetOrdinal(helper.SobreTCheckActivo);
                    if (!dr.IsDBNull(iSobreTCheckActivo)) entity.SobreTCheckActivo = Convert.ToString(dr.GetValue(iSobreTCheckActivo));

                    int iSobreTU = dr.GetOrdinal(helper.SobreTU);
                    if (!dr.IsDBNull(iSobreTU)) entity.SobreTU = Convert.ToString(dr.GetValue(iSobreTU));

                    int iSobreTT = dr.GetOrdinal(helper.SobreTT);
                    if (!dr.IsDBNull(iSobreTT)) entity.SobreTT = Convert.ToString(dr.GetValue(iSobreTT));

                    int iSobreTUU = dr.GetOrdinal(helper.SobreTUU);
                    if (!dr.IsDBNull(iDeltaTension)) entity.SobreTUU = Convert.ToString(dr.GetValue(iSobreTUU));

                    int iSobreTTT = dr.GetOrdinal(helper.SobreTTT);
                    if (!dr.IsDBNull(iSobreTTT)) entity.SobreTTT = Convert.ToString(dr.GetValue(iSobreTTT));

                    int iSobreCCheckActivo = dr.GetOrdinal(helper.SobreCCheckActivo);
                    if (!dr.IsDBNull(iSobreCCheckActivo)) entity.SobreCCheckActivo = Convert.ToString(dr.GetValue(iSobreCCheckActivo));

                    int iSobreCI = dr.GetOrdinal(helper.SobreCI);
                    if (!dr.IsDBNull(iSobreCI)) entity.SobreCI = Convert.ToString(dr.GetValue(iSobreCI));

                    int iPmuCheckActivo = dr.GetOrdinal(helper.PmuCheckActivo);
                    if (!dr.IsDBNull(iPmuCheckActivo)) entity.PmuCheckActivo = Convert.ToString(dr.GetValue(iPmuCheckActivo));

                    int iPmuAccion = dr.GetOrdinal(helper.PmuAccion);
                    if (!dr.IsDBNull(iPmuAccion)) entity.PmuAccion = Convert.ToString(dr.GetValue(iPmuAccion));

                    int iIdInterruptorMS = dr.GetOrdinal(helper.IdInterruptorMS);
                    if (!dr.IsDBNull(iIdInterruptorMS)) entity.IdInterruptorMS = Convert.ToString(dr.GetValue(iIdInterruptorMS));

                    int iIdMandoSincronizado = dr.GetOrdinal(helper.IdMandoSincronizado);
                    if (!dr.IsDBNull(iIdMandoSincronizado)) entity.IdMandoSincronizado = Convert.ToString(dr.GetValue(iIdMandoSincronizado));

                    int iMedidaMitigacion = dr.GetOrdinal(helper.MedidaMitigacion);
                    if (!dr.IsDBNull(iMedidaMitigacion)) entity.MedidaMitigacion = Convert.ToString(dr.GetValue(iMedidaMitigacion));

                    int iReleTorsImpl = dr.GetOrdinal(helper.ReleTorsImpl);
                    if (!dr.IsDBNull(iReleTorsImpl)) entity.ReleTorsImpl = Convert.ToString(dr.GetValue(iReleTorsImpl));

                    int iRelePmuAccion = dr.GetOrdinal(helper.RelePmuAccion);
                    if (!dr.IsDBNull(iRelePmuAccion)) entity.RelePmuAccion = Convert.ToString(dr.GetValue(iRelePmuAccion));

                    int iRelePmuImpl = dr.GetOrdinal(helper.RelePmuImpl);
                    if (!dr.IsDBNull(iRelePmuImpl)) entity.RelePmuImpl = Convert.ToString(dr.GetValue(iRelePmuImpl));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));

                    int iProyectoCreador = dr.GetOrdinal(helper.ProyectoCreador);
                    if (!dr.IsDBNull(iProyectoCreador)) entity.ProyectoCreador = Convert.ToString(dr.GetValue(iProyectoCreador));

                    int iFechacreacionstr = dr.GetOrdinal(helper.Fechacreacionstr);
                    if (!dr.IsDBNull(iFechacreacionstr)) entity.Fechacreacionstr = Convert.ToString(dr.GetValue(iFechacreacionstr));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fechamodificacionstr);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fechamodificacionstr = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iProyectoActualizador = dr.GetOrdinal(helper.ProyectoActualizador);
                    if (!dr.IsDBNull(iProyectoActualizador)) entity.ProyectoActualizador = Convert.ToString(dr.GetValue(iProyectoActualizador));
                    

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EprEquipoDTO> ArchivoZipHistarialCambio(int areacodi, int zonacodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ArchivoZipHistarialCambio);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.String, areacodi);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.String, areacodi);
            dbProvider.AddInParameter(command, "id_area", DbType.String, zonacodi);
            dbProvider.AddInParameter(command, "id_area", DbType.String, zonacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iZona = dr.GetOrdinal(helper.Zona);
                    if (!dr.IsDBNull(iZona)) entity.Zona = Convert.ToString(dr.GetValue(iZona));

                    int iSubestacionNomb = dr.GetOrdinal(helper.SubestacionNomb);
                    if (!dr.IsDBNull(iSubestacionNomb)) entity.SubestacionNomb = Convert.ToString(dr.GetValue(iSubestacionNomb));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iNombreArchivo = dr.GetOrdinal(helper.NombreArchivo);
                    if (!dr.IsDBNull(iNombreArchivo)) entity.NombreArchivo = Convert.ToString(dr.GetValue(iNombreArchivo));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToString(dr.GetValue(iTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        

        public List<EprEquipoDTO> ReporteEquipoProtGrillaProyecto(int epproycodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ReporteEquipoProtGrillaProyecto);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.String, epproycodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iSistema = dr.GetOrdinal(helper.Sistema);
                    if (!dr.IsDBNull(iSistema)) entity.Sistema = Convert.ToString(dr.GetValue(iSistema));

                    int iMarca = dr.GetOrdinal(helper.Marca);
                    if (!dr.IsDBNull(iMarca)) entity.Marca = Convert.ToString(dr.GetValue(iMarca));

                    int iModelo = dr.GetOrdinal(helper.Modelo);
                    if (!dr.IsDBNull(iModelo)) entity.Modelo = Convert.ToString(dr.GetValue(iModelo));

                    int iTipoUso = dr.GetOrdinal(helper.TipoUso);
                    if (!dr.IsDBNull(iTipoUso)) entity.TipoUso = Convert.ToString(dr.GetValue(iTipoUso));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iRtcPrimario = dr.GetOrdinal(helper.RtcPrimario);
                    if (!dr.IsDBNull(iRtcPrimario)) entity.RtcPrimario = Convert.ToString(dr.GetValue(iRtcPrimario));

                    int iRtcSecundario = dr.GetOrdinal(helper.RtcSecundario);
                    if (!dr.IsDBNull(iRtcSecundario)) entity.RtcSecundario = Convert.ToString(dr.GetValue(iRtcSecundario));

                    int iRttPrimario = dr.GetOrdinal(helper.RttPrimario);
                    if (!dr.IsDBNull(iRttPrimario)) entity.RttPrimario = Convert.ToString(dr.GetValue(iRttPrimario));

                    int iRttSecundario = dr.GetOrdinal(helper.RttSecundario);
                    if (!dr.IsDBNull(iRttSecundario)) entity.RttSecundario = Convert.ToString(dr.GetValue(iRttSecundario));

                    int iProtCondinables = dr.GetOrdinal(helper.ProtCondinables);
                    if (!dr.IsDBNull(iProtCondinables)) entity.ProtCondinables = Convert.ToString(dr.GetValue(iProtCondinables));

                    int iSincroCheckActivo = dr.GetOrdinal(helper.SincroCheckActivo);
                    if (!dr.IsDBNull(iSincroCheckActivo)) entity.SincroCheckActivo = Convert.ToString(dr.GetValue(iSincroCheckActivo));

                    int iIdInterruptor = dr.GetOrdinal(helper.IdInterruptor);
                    if (!dr.IsDBNull(iIdInterruptor)) entity.IdInterruptor = Convert.ToString(dr.GetValue(iIdInterruptor));

                    int iDeltaTension = dr.GetOrdinal(helper.DeltaTension);
                    if (!dr.IsDBNull(iDeltaTension)) entity.DeltaTension = Convert.ToString(dr.GetValue(iDeltaTension));

                    int iDeltaAngulo = dr.GetOrdinal(helper.DeltaAngulo);
                    if (!dr.IsDBNull(iDeltaAngulo)) entity.DeltaAngulo = Convert.ToString(dr.GetValue(iDeltaAngulo));

                    int iDeltaFrecuencia = dr.GetOrdinal(helper.DeltaFrecuencia);
                    if (!dr.IsDBNull(iDeltaFrecuencia)) entity.DeltaFrecuencia = Convert.ToString(dr.GetValue(iDeltaFrecuencia));

                    int iSobreTCheckActivo = dr.GetOrdinal(helper.SobreTCheckActivo);
                    if (!dr.IsDBNull(iSobreTCheckActivo)) entity.SobreTCheckActivo = Convert.ToString(dr.GetValue(iSobreTCheckActivo));

                    int iSobreTU = dr.GetOrdinal(helper.SobreTU);
                    if (!dr.IsDBNull(iSobreTU)) entity.SobreTU = Convert.ToString(dr.GetValue(iSobreTU));

                    int iSobreTT = dr.GetOrdinal(helper.SobreTT);
                    if (!dr.IsDBNull(iSobreTT)) entity.SobreTT = Convert.ToString(dr.GetValue(iSobreTT));

                    int iSobreTUU = dr.GetOrdinal(helper.SobreTUU);
                    if (!dr.IsDBNull(iDeltaTension)) entity.SobreTUU = Convert.ToString(dr.GetValue(iSobreTUU));

                    int iSobreTTT = dr.GetOrdinal(helper.SobreTTT);
                    if (!dr.IsDBNull(iSobreTTT)) entity.SobreTTT = Convert.ToString(dr.GetValue(iSobreTTT));

                    int iSobreCCheckActivo = dr.GetOrdinal(helper.SobreCCheckActivo);
                    if (!dr.IsDBNull(iSobreCCheckActivo)) entity.SobreCCheckActivo = Convert.ToString(dr.GetValue(iSobreCCheckActivo));

                    int iSobreCI = dr.GetOrdinal(helper.SobreCI);
                    if (!dr.IsDBNull(iSobreCI)) entity.SobreCI = Convert.ToString(dr.GetValue(iSobreCI));

                    int iPmuCheckActivo = dr.GetOrdinal(helper.PmuCheckActivo);
                    if (!dr.IsDBNull(iPmuCheckActivo)) entity.PmuCheckActivo = Convert.ToString(dr.GetValue(iPmuCheckActivo));

                    int iPmuAccion = dr.GetOrdinal(helper.PmuAccion);
                    if (!dr.IsDBNull(iPmuAccion)) entity.PmuAccion = Convert.ToString(dr.GetValue(iPmuAccion));

                    int iIdInterruptorMS = dr.GetOrdinal(helper.IdInterruptorMS);
                    if (!dr.IsDBNull(iIdInterruptorMS)) entity.IdInterruptorMS = Convert.ToString(dr.GetValue(iIdInterruptorMS));

                    int iIdMandoSincronizado = dr.GetOrdinal(helper.IdMandoSincronizado);
                    if (!dr.IsDBNull(iIdMandoSincronizado)) entity.IdMandoSincronizado = Convert.ToString(dr.GetValue(iIdMandoSincronizado));

                    int iMedidaMitigacion = dr.GetOrdinal(helper.MedidaMitigacion);
                    if (!dr.IsDBNull(iMedidaMitigacion)) entity.MedidaMitigacion = Convert.ToString(dr.GetValue(iMedidaMitigacion));

                    int iReleTorsImpl = dr.GetOrdinal(helper.ReleTorsImpl);
                    if (!dr.IsDBNull(iReleTorsImpl)) entity.ReleTorsImpl = Convert.ToString(dr.GetValue(iReleTorsImpl));

                    int iRelePmuAccion = dr.GetOrdinal(helper.RelePmuAccion);
                    if (!dr.IsDBNull(iRelePmuAccion)) entity.RelePmuAccion = Convert.ToString(dr.GetValue(iRelePmuAccion));

                    int iRelePmuImpl = dr.GetOrdinal(helper.RelePmuImpl);
                    if (!dr.IsDBNull(iRelePmuImpl)) entity.RelePmuImpl = Convert.ToString(dr.GetValue(iRelePmuImpl));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));

                    int iAccion = dr.GetOrdinal(helper.Accion);
                    if (!dr.IsDBNull(iAccion)) entity.Accion = Convert.ToString(dr.GetValue(iAccion));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO GetByIdEquipoProtec(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdEquipoProtec);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iIdTitular = dr.GetOrdinal(helper.IdTitular);
                    if (!dr.IsDBNull(iIdTitular)) entity.IdTitular = Convert.ToInt32(dr.GetValue(iIdTitular));

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iIdCelda = dr.GetOrdinal(helper.IdCelda);
                    if (!dr.IsDBNull(iIdCelda)) entity.IdCelda = Convert.ToInt32(dr.GetValue(iIdCelda));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iIdMarca = dr.GetOrdinal(helper.IdMarca);
                    if (!dr.IsDBNull(iIdMarca)) entity.IdMarca = Convert.ToString(dr.GetValue(iIdMarca));

                    int iModelo = dr.GetOrdinal(helper.Modelo);
                    if (!dr.IsDBNull(iModelo)) entity.Modelo = Convert.ToString(dr.GetValue(iModelo));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iIdSistermaRele = dr.GetOrdinal(helper.IdSistermaRele);
                    if (!dr.IsDBNull(iIdSistermaRele)) entity.IdSistermaRele = Convert.ToString(dr.GetValue(iIdSistermaRele));

                    int iIdTipoUso = dr.GetOrdinal(helper.IdTipoUso);
                    if (!dr.IsDBNull(iIdTipoUso)) entity.IdTipoUso = Convert.ToString(dr.GetValue(iIdTipoUso));

                    int iRtcPrimario = dr.GetOrdinal(helper.RtcPrimario);
                    if (!dr.IsDBNull(iRtcPrimario)) entity.RtcPrimario = Convert.ToString(dr.GetValue(iRtcPrimario));

                    int iRtcSecundario = dr.GetOrdinal(helper.RtcSecundario);
                    if (!dr.IsDBNull(iRtcSecundario)) entity.RtcSecundario = Convert.ToString(dr.GetValue(iRtcSecundario));

                    int iRttPrimario = dr.GetOrdinal(helper.RttPrimario);
                    if (!dr.IsDBNull(iRttPrimario)) entity.RttPrimario = Convert.ToString(dr.GetValue(iRttPrimario));

                    int iRttSecundario = dr.GetOrdinal(helper.RttSecundario);
                    if (!dr.IsDBNull(iRttSecundario)) entity.RttSecundario = Convert.ToString(dr.GetValue(iRttSecundario));

                    int iProtCondinables = dr.GetOrdinal(helper.ProtCondinables);
                    if (!dr.IsDBNull(iProtCondinables)) entity.ProtCondinables = Convert.ToString(dr.GetValue(iProtCondinables));

                    int iSincroCheckActivo = dr.GetOrdinal(helper.SincroCheckActivo);
                    if (!dr.IsDBNull(iSincroCheckActivo)) entity.SincroCheckActivo = Convert.ToString(dr.GetValue(iSincroCheckActivo));

                    int iIdInterruptor = dr.GetOrdinal(helper.IdInterruptor);
                    if (!dr.IsDBNull(iIdInterruptor)) entity.IdInterruptor = Convert.ToString(dr.GetValue(iIdInterruptor));

                    int iDeltaTension = dr.GetOrdinal(helper.DeltaTension);
                    if (!dr.IsDBNull(iDeltaTension)) entity.DeltaTension = Convert.ToString(dr.GetValue(iDeltaTension));

                    int iDeltaAngulo = dr.GetOrdinal(helper.DeltaAngulo);
                    if (!dr.IsDBNull(iDeltaAngulo)) entity.DeltaAngulo = Convert.ToString(dr.GetValue(iDeltaAngulo));
                    
                    int iDeltaFrecuencia = dr.GetOrdinal(helper.DeltaFrecuencia);
                    if (!dr.IsDBNull(iDeltaFrecuencia)) entity.DeltaFrecuencia = Convert.ToString(dr.GetValue(iDeltaFrecuencia));

                    int iSobreTCheckActivo = dr.GetOrdinal(helper.SobreTCheckActivo);
                    if (!dr.IsDBNull(iSobreTCheckActivo)) entity.SobreTCheckActivo = Convert.ToString(dr.GetValue(iSobreTCheckActivo));

                    int iSobreTU = dr.GetOrdinal(helper.SobreTU);
                    if (!dr.IsDBNull(iSobreTU)) entity.SobreTU = Convert.ToString(dr.GetValue(iSobreTU));

                    int iSobreTT = dr.GetOrdinal(helper.SobreTT);
                    if (!dr.IsDBNull(iSobreTT)) entity.SobreTT = Convert.ToString(dr.GetValue(iSobreTT));

                    int iSobreTUU = dr.GetOrdinal(helper.SobreTUU);
                    if (!dr.IsDBNull(iSobreTUU)) entity.SobreTUU = Convert.ToString(dr.GetValue(iSobreTUU));

                    int iSobreTTT = dr.GetOrdinal(helper.SobreTTT);
                    if (!dr.IsDBNull(iSobreTTT)) entity.SobreTTT = Convert.ToString(dr.GetValue(iSobreTTT));

                    int iSobreCCheckActivo = dr.GetOrdinal(helper.SobreCCheckActivo);
                    if (!dr.IsDBNull(iSobreCCheckActivo)) entity.SobreCCheckActivo = Convert.ToString(dr.GetValue(iSobreCCheckActivo));

                    int iSobreCI = dr.GetOrdinal(helper.SobreCI);
                    if (!dr.IsDBNull(iSobreCI)) entity.SobreCI = Convert.ToString(dr.GetValue(iSobreCI));

                    int iPmuCheckActivo = dr.GetOrdinal(helper.PmuCheckActivo);
                    if (!dr.IsDBNull(iPmuCheckActivo)) entity.PmuCheckActivo = Convert.ToString(dr.GetValue(iPmuCheckActivo));

                    int iPmuAccion = dr.GetOrdinal(helper.PmuAccion);
                    if (!dr.IsDBNull(iPmuAccion)) entity.PmuAccion = Convert.ToString(dr.GetValue(iPmuAccion));

                    int iIdInterruptorMS = dr.GetOrdinal(helper.IdInterruptorMS);
                    if (!dr.IsDBNull(iIdInterruptorMS)) entity.IdInterruptorMS = Convert.ToString(dr.GetValue(iIdInterruptorMS));

                    int iIdMandoSincronizado = dr.GetOrdinal(helper.IdMandoSincronizado);
                    if (!dr.IsDBNull(iIdMandoSincronizado)) entity.IdMandoSincronizado = Convert.ToString(dr.GetValue(iIdMandoSincronizado));

                    int iReleTorsImpl = dr.GetOrdinal(helper.ReleTorsImpl);
                    if (!dr.IsDBNull(iReleTorsImpl)) entity.ReleTorsImpl = Convert.ToString(dr.GetValue(iReleTorsImpl));

                    int iRelePmuImpl = dr.GetOrdinal(helper.RelePmuImpl);
                    if (!dr.IsDBNull(iRelePmuImpl)) entity.RelePmuImpl = Convert.ToString(dr.GetValue(iRelePmuImpl));

                    int iIdProyecto = dr.GetOrdinal(helper.IdProyecto);
                    if (!dr.IsDBNull(iIdProyecto)) entity.IdProyecto = Convert.ToInt32(dr.GetValue(iIdProyecto));

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = Convert.ToString(dr.GetValue(iFecha));

                    int iMedidaMitigacion = dr.GetOrdinal(helper.MedidaMitigacion);
                    if (!dr.IsDBNull(iMedidaMitigacion)) entity.MedidaMitigacion = Convert.ToString(dr.GetValue(iMedidaMitigacion));

                    int iRelePmuAccion = dr.GetOrdinal(helper.RelePmuAccion);
                    if (!dr.IsDBNull(iRelePmuAccion)) entity.RelePmuAccion = Convert.ToString(dr.GetValue(iRelePmuAccion));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));
                    
                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListLineaTiempo(int equicodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListLineaTiempo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = Convert.ToString(dr.GetValue(iFecha));

                    int iProyectoNomb = dr.GetOrdinal(helper.ProyectoNomb);
                    if (!dr.IsDBNull(iProyectoNomb)) entity.ProyectoNomb = Convert.ToString(dr.GetValue(iProyectoNomb));

                    int iProyectoDesc = dr.GetOrdinal(helper.ProyectoDesc);
                    if (!dr.IsDBNull(iProyectoDesc)) entity.ProyectoDesc = Convert.ToString(dr.GetValue(iProyectoDesc));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListEquipamientoModificado(int epproycodi)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListEquipamientoModificado);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, epproycodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iSubestacionNomb = dr.GetOrdinal(helper.SubestacionNomb);
                    if (!dr.IsDBNull(iSubestacionNomb)) entity.SubestacionNomb = dr.GetValue(iSubestacionNomb).ToString();

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iRele = dr.GetOrdinal(helper.Rele);
                    if (!dr.IsDBNull(iRele)) entity.Rele = dr.GetValue(iRele).ToString();

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iSistemaRele = dr.GetOrdinal(helper.SistemaRele);
                    if (!dr.IsDBNull(iSistemaRele)) entity.SistemaRele = Convert.ToString(dr.GetValue(iSistemaRele));

                    int iMarca = dr.GetOrdinal(helper.Marca);
                    if (!dr.IsDBNull(iMarca)) entity.Marca = Convert.ToString(dr.GetValue(iMarca));

                    int iModelo = dr.GetOrdinal(helper.Modelo);
                    if (!dr.IsDBNull(iSistemaRele)) entity.Modelo = Convert.ToString(dr.GetValue(iModelo));

                    int iTipoUso = dr.GetOrdinal(helper.TipoUso);
                    if (!dr.IsDBNull(iTipoUso)) entity.TipoUso = Convert.ToString(dr.GetValue(iTipoUso));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));

                    int iAccion = dr.GetOrdinal(helper.Accion);
                    if (!dr.IsDBNull(iAccion)) entity.Accion = Convert.ToString(dr.GetValue(iAccion));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO GetCantidadEquipoSGOCOESEliminar(int epequicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCantidadEquipoSGOCOESEliminar);

            dbProvider.AddInParameter(command, helper.Epequicodi, DbType.Int32, epequicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iNroEquipos = dr.GetOrdinal(helper.NroEquipos);
                    if (!dr.IsDBNull(iNroEquipos)) entity.NroEquipos = Convert.ToInt32(dr.GetValue(iNroEquipos));
                }
            }

            return entity;
        }

        public EprEquipoDTO GetDetalleArbolEquipoProteccion(int equicodi, int nivel)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetDetalleArbolEquipoProteccion);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Nivel, DbType.Int32, nivel);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iMemoriaCalculo = dr.GetOrdinal(helper.MemoriaCalculo);
                    if (!dr.IsDBNull(iMemoriaCalculo)) entity.MemoriaCalculo = Convert.ToString(dr.GetValue(iMemoriaCalculo));
                }
            }

            return entity;
        }
        
        public List<EprEquipoDTO> ListaReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListReleSincronismo);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iCodigoInterruptor = dr.GetOrdinal("codigo_interruptor");
                    if (!dr.IsDBNull(iCodigoInterruptor)) entity.CodigoInterruptor = Convert.ToString(dr.GetValue(iCodigoInterruptor));

                    int iDeltaTension = dr.GetOrdinal("delta_tension");
                    if (!dr.IsDBNull(iDeltaTension)) entity.DeltaTension = Convert.ToString(dr.GetValue(iDeltaTension));

                    int iDeltaAngulo = dr.GetOrdinal("delta_angulo");
                    if (!dr.IsDBNull(iDeltaAngulo)) entity.DeltaAngulo = Convert.ToString(dr.GetValue(iDeltaAngulo));

                    int iDeltaFrecuencia = dr.GetOrdinal("delta_frecuencia");
                    if (!dr.IsDBNull(iDeltaFrecuencia)) entity.DeltaFrecuencia = Convert.ToString(dr.GetValue(iDeltaFrecuencia));


                    int iUsuarioModificacion = dr.GetOrdinal("usuario_modificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecha_modificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaBuscarCeldaAcoplamiento(string codigoId, string codigo, int ubicacion,
            int empresa, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListBuscarCeldaAcoplamiento);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreaDesc = dr.GetOrdinal("areadesc");
                    if (!dr.IsDBNull(iAreaDesc)) entity.AreaDescripcion = Convert.ToString(dr.GetValue(iAreaDesc));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iEquipoTension = dr.GetOrdinal("equitension");
                    if (!dr.IsDBNull(iEquipoTension)) entity.EquipoTension = Convert.ToString(dr.GetValue(iEquipoTension));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    int iEquipoEstadoDescripcion = dr.GetOrdinal("equiestadodesc");
                    if (!dr.IsDBNull(iEquipoEstadoDescripcion)) entity.EquipoEstadoDescripcion = Convert.ToString(dr.GetValue(iEquipoEstadoDescripcion));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public string RegistrarLinea(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRegistrarLinea);

            dbProvider.AddInParameter(command, helper.IdLinea, DbType.Int32, equipo.IdLinea);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.String, equipo.IdArea);
            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, "CAPACIDAD_A_COMENT", DbType.String, equipo.CapacidadAComent);
            dbProvider.AddInParameter(command, helper.CapacidadMva, DbType.String, equipo.CapacidadMva);
            dbProvider.AddInParameter(command, "CAPACIDAD_MVA_COMENT", DbType.String, equipo.CapacidadMvaComent);
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.String, equipo.IdCelda);
            dbProvider.AddInParameter(command, helper.IdCelda2, DbType.String, equipo.IdCelda2);
            dbProvider.AddInParameter(command, helper.IdBancoCondensador, DbType.String, equipo.IdBancoCondensador);
            dbProvider.AddInParameter(command, "CAPACIDAD_A_BANCO_CONDENSADOR", DbType.String, equipo.CapacidadABancoCondensador);
            dbProvider.AddInParameter(command, "CAPACIDAD_A_BANCO_CONDENSADOR_COMENT", DbType.String, equipo.CapacidadABancoCondensadorComent);
            dbProvider.AddInParameter(command, "CAPACIDAD_MVAR_BANCO_CONDENSADOR", DbType.String, equipo.CapacidadMvarBancoCondensador);
            dbProvider.AddInParameter(command, "CAPACIDAD_MVAR_BANCO_CONDENSADOR_COMENT", DbType.String, equipo.CapacidadMvarBancoCondensadorComent);
            dbProvider.AddInParameter(command, helper.CapacTransCond1Porcen, DbType.String, equipo.CapacTransCond1Porcen);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_COND_1_PORCEN_COMENT", DbType.String, equipo.CapacTransCond1PorcenComent);
            dbProvider.AddInParameter(command, helper.CapacTransCond1Min, DbType.String, equipo.CapacTransCond1Min);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_COND_1_MIN_COMENT", DbType.String, equipo.CapacTransCond1MinComent);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_CORR_1_A", DbType.String, equipo.CapacTransCond1A);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_CORR_1_A_COMENT", DbType.String, equipo.CapacTransCond1AComent);
            dbProvider.AddInParameter(command, helper.CapacTransCond2Porcen, DbType.String, equipo.CapacTransCond2Porcen);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_COND_2_PORCEN_COMENT", DbType.String, equipo.CapacTransCond2PorcenComent);
            dbProvider.AddInParameter(command, helper.CapacTransCond2Min, DbType.String, equipo.CapacTransCond2Min);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_COND_2_MIN_COMENT", DbType.String, equipo.CapacTransCond2MinComent);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_CORR_2_A", DbType.String, equipo.CapacTransCond2A);
            dbProvider.AddInParameter(command, "CAPAC_TRANS_CORR_2_A_COMENT", DbType.String, equipo.CapacTransCond2AComent);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, "CAPACIDAD_TRANSMISION_A_COMENT", DbType.String, equipo.CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMVA, DbType.String, equipo.CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, "CAPACIDAD_TRANSMISION_MVA_COMENT", DbType.String, equipo.CapacidadTransmisionMvaComent);
            dbProvider.AddInParameter(command, helper.LimiteSegCoes, DbType.String, equipo.LimiteSegCoes);
            dbProvider.AddInParameter(command, "LIMITE_SEG_COES_COMENT", DbType.String, equipo.LimiteSegCoesComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, "FACTOR_LIMITANTE_CALC_COMENT", DbType.String, equipo.FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);
            dbProvider.AddInParameter(command, "FACTOR_LIMITANTE_FINAL_COMENT", DbType.String, equipo.FactorLimitanteFinalComent);
            dbProvider.AddInParameter(command, helper.Observaciones, DbType.String, equipo.Observaciones);
            dbProvider.AddInParameter(command, helper.UsuarioAuditoria, DbType.String, equipo.UsuarioAuditoria);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }


        #region GESTPROTEC Evaluación
        public List<EprEquipoDTO> ListCeldaEvaluacion(string idUbicacion)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCeldaEvaluacion);
            dbProvider.AddInParameter(command, helper.IdUbicacion, DbType.String, idUbicacion);
            dbProvider.AddInParameter(command, helper.IdUbicacion, DbType.String, idUbicacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iPosicionNucleoTc = dr.GetOrdinal(helper.PosicionNucleoTc);
                    if (!dr.IsDBNull(iPosicionNucleoTc)) entity.PosicionNucleoTc = Convert.ToString(dr.GetValue(iPosicionNucleoTc));

                    int iPickUp = dr.GetOrdinal(helper.PickUp);
                    if (!dr.IsDBNull(iPickUp)) entity.PickUp = Convert.ToString(dr.GetValue(iPickUp));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaBuscarReactor(string codigoId, string codigo, int ubicacion,
            int empresa, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListBuscarReactor);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iAreaDescripcion = dr.GetOrdinal("areadesc");
                    if (!dr.IsDBNull(iAreaDescripcion)) entity.AreaDescripcion = Convert.ToString(dr.GetValue(iAreaDescripcion));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iEquipoEstadoDescripcion = dr.GetOrdinal("equiestadodesc");
                    if (!dr.IsDBNull(iEquipoEstadoDescripcion)) entity.EquipoEstadoDescripcion = Convert.ToString(dr.GetValue(iEquipoEstadoDescripcion));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<EprEquipoDTO> ListBancoEvaluacion()
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListBancoEvaluacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iCapacidadA = dr.GetOrdinal(helper.CapacidadA);
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMvar = dr.GetOrdinal(helper.CapacidadMvar);
                    if (!dr.IsDBNull(iCapacidadMvar)) entity.CapacidadMvar = Convert.ToString(dr.GetValue(iCapacidadMvar));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO GetIdLineaIncluir(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetIdLineaIncluir);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iIdProyecto = dr.GetOrdinal(helper.IdProyecto);
                    if (!dr.IsDBNull(iIdProyecto)) entity.IdProyecto = Convert.ToInt32(dr.GetValue(iIdProyecto));

                    int iProyectoNomb = dr.GetOrdinal(helper.ProyectoNomb);
                    if (!dr.IsDBNull(iProyectoNomb)) entity.ProyectoNomb = Convert.ToString(dr.GetValue(iProyectoNomb));

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = Convert.ToString(dr.GetValue(iFecha));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iIdUbicacion = dr.GetOrdinal(helper.IdUbicacion);
                    if (!dr.IsDBNull(iIdUbicacion)) entity.IdUbicacion = Convert.ToInt32(dr.GetValue(iIdUbicacion));

                    int iUbicacion = dr.GetOrdinal(helper.Ubicacion);
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iIdTitular = dr.GetOrdinal(helper.IdTitular);
                    if (!dr.IsDBNull(iIdTitular)) entity.IdTitular = Convert.ToInt32(dr.GetValue(iIdTitular));

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iIdArea = dr.GetOrdinal(helper.IdArea);
                    if (!dr.IsDBNull(iIdArea)) entity.IdArea = Convert.ToInt32(dr.GetValue(iIdArea));

                    int iLongitud = dr.GetOrdinal(helper.Longitud);
                    if (!dr.IsDBNull(iLongitud)) entity.Longitud = Convert.ToString(dr.GetValue(iLongitud));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iCapacidadA = dr.GetOrdinal(helper.CapacidadA);
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMva = dr.GetOrdinal(helper.CapacidadMva);
                    if (!dr.IsDBNull(iCapacidadMva)) entity.CapacidadMva = Convert.ToString(dr.GetValue(iCapacidadMva));

                    int iIdCelda = dr.GetOrdinal(helper.IdCelda);
                    if (!dr.IsDBNull(iIdCelda)) entity.IdCelda = Convert.ToInt32(dr.GetValue(iIdCelda));

                    int iCeldaPosicionNucleoTc = dr.GetOrdinal(helper.CeldaPosicionNucleoTc);
                    if (!dr.IsDBNull(iCeldaPosicionNucleoTc)) entity.CeldaPosicionNucleoTc = Convert.ToString(dr.GetValue(iCeldaPosicionNucleoTc));

                    int iCeldaPickUp = dr.GetOrdinal(helper.CeldaPickUp);
                    if (!dr.IsDBNull(iCeldaPickUp)) entity.CeldaPickUp = Convert.ToString(dr.GetValue(iCeldaPickUp));

                    int iIdCelda2 = dr.GetOrdinal(helper.IdCelda2);
                    if (!dr.IsDBNull(iIdCelda2)) entity.IdCelda2 = Convert.ToInt32(dr.GetValue(iIdCelda2));

                    int iCelda2PosicionNucleoTc = dr.GetOrdinal(helper.Celda2PosicionNucleoTc);
                    if (!dr.IsDBNull(iCelda2PosicionNucleoTc)) entity.Celda2PosicionNucleoTc = Convert.ToString(dr.GetValue(iCelda2PosicionNucleoTc));

                    int iCelda2PickUp = dr.GetOrdinal(helper.Celda2PickUp);
                    if (!dr.IsDBNull(iCelda2PickUp)) entity.Celda2PickUp = Convert.ToString(dr.GetValue(iCelda2PickUp));

                    int iIdBancoCondensador = dr.GetOrdinal(helper.IdBancoCondensador);
                    if (!dr.IsDBNull(iIdBancoCondensador)) entity.IdBancoCondensador = Convert.ToInt32(dr.GetValue(iIdBancoCondensador));

                    int iBancoCondensadorSerieCapacidadA = dr.GetOrdinal(helper.BancoCondensadorSerieCapacidadA);
                    if (!dr.IsDBNull(iBancoCondensadorSerieCapacidadA)) entity.BancoCondensadorSerieCapacidadA = Convert.ToString(dr.GetValue(iBancoCondensadorSerieCapacidadA));

                    int iBancoCondensadorSerieCapacidadMVAR = dr.GetOrdinal(helper.BancoCondensadorSerieCapacidadMVAR);
                    if (!dr.IsDBNull(iBancoCondensadorSerieCapacidadMVAR)) entity.BancoCondensadorSerieCapacidadMVAR = Convert.ToString(dr.GetValue(iBancoCondensadorSerieCapacidadMVAR));

                    int iCapacTransCond1Porcen = dr.GetOrdinal(helper.CapacTransCond1Porcen);
                    if (!dr.IsDBNull(iCapacTransCond1Porcen)) entity.CapacTransCond1Porcen = Convert.ToString(dr.GetValue(iCapacTransCond1Porcen));

                    int iCapacTransCond1Min = dr.GetOrdinal(helper.CapacTransCond1Min);
                    if (!dr.IsDBNull(iCapacTransCond1Min)) entity.CapacTransCond1Min = Convert.ToString(dr.GetValue(iCapacTransCond1Min));


                    int iCapacTransCond1A = dr.GetOrdinal(helper.CapacTransCond1A);
                    if (!dr.IsDBNull(iCapacTransCond1A)) entity.CapacTransCond1A = Convert.ToString(dr.GetValue(iCapacTransCond1A));

                    int iCapacTransCond2Porcen = dr.GetOrdinal(helper.CapacTransCond2Porcen);
                    if (!dr.IsDBNull(iCapacTransCond2Porcen)) entity.CapacTransCond2Porcen = Convert.ToString(dr.GetValue(iCapacTransCond2Porcen));

                    int iCapacTransCond2Min = dr.GetOrdinal(helper.CapacTransCond2Min);
                    if (!dr.IsDBNull(iCapacTransCond2Min)) entity.CapacTransCond2Min = Convert.ToString(dr.GetValue(iCapacTransCond2Min));

                    int iCapacTransCond2A = dr.GetOrdinal(helper.CapacTransCond2A);
                    if (!dr.IsDBNull(iCapacTransCond2A)) entity.CapacTransCond2A = Convert.ToString(dr.GetValue(iCapacTransCond2A));

                    int iCapacidadTransmisionA = dr.GetOrdinal(helper.CapacidadTransmisionA);
                    if (!dr.IsDBNull(iCapacidadTransmisionA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadTransmisionA));

                    int iCapacidadTransmisionMVA = dr.GetOrdinal(helper.CapacidadTransmisionMVA);
                    if (!dr.IsDBNull(iCapacidadTransmisionMVA)) entity.CapacidadTransmisionMVA = Convert.ToString(dr.GetValue(iCapacidadTransmisionMVA));

                    int iLimiteSegCoes = dr.GetOrdinal(helper.LimiteSegCoes);
                    if (!dr.IsDBNull(iLimiteSegCoes)) entity.LimiteSegCoes = Convert.ToString(dr.GetValue(iLimiteSegCoes));

                    int iFactorLimitanteCalc = dr.GetOrdinal(helper.FactorLimitanteCalc);
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.FactorLimitanteCalc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteFinal = dr.GetOrdinal(helper.FactorLimitanteFinal);
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iObservaciones = dr.GetOrdinal(helper.Observaciones);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iUsuarioModificacion = dr.GetOrdinal("usumodificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecmodificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iCapacidadABancoCondensador = dr.GetOrdinal("capacidad_a_banco_condensador");
                    if (!dr.IsDBNull(iCapacidadABancoCondensador)) entity.CapacidadABancoCondensador = Convert.ToString(dr.GetValue(iCapacidadABancoCondensador));

                    int iCapacidadABancoCondensadorComent = dr.GetOrdinal("capacidad_a_banco_condensador_coment");
                    if (!dr.IsDBNull(iCapacidadABancoCondensadorComent)) entity.CapacidadABancoCondensadorComent = Convert.ToString(dr.GetValue(iCapacidadABancoCondensadorComent));

                    int iCapacidadMvarBancoCondensador = dr.GetOrdinal("capacidad_mvar_banco_condensador");
                    if (!dr.IsDBNull(iCapacidadMvarBancoCondensador)) entity.CapacidadMvarBancoCondensador = Convert.ToString(dr.GetValue(iCapacidadMvarBancoCondensador));

                    int iCapacidadMvarBancoCondensadorComent = dr.GetOrdinal("capacidad_mvar_banco_condensador_coment");
                    if (!dr.IsDBNull(iCapacidadMvarBancoCondensadorComent)) entity.CapacidadMvarBancoCondensadorComent = Convert.ToString(dr.GetValue(iCapacidadMvarBancoCondensadorComent));

                    int iCapacidadAComent = dr.GetOrdinal("capacidad_a_coment");
                    if (!dr.IsDBNull(iCapacidadAComent)) entity.CapacidadAComent = Convert.ToString(dr.GetValue(iCapacidadAComent));

                    int iCapacidadMvaComent = dr.GetOrdinal("capacidad_mva_coment");
                    if (!dr.IsDBNull(iCapacidadMvaComent)) entity.CapacidadMvaComent = Convert.ToString(dr.GetValue(iCapacidadMvaComent));

                    int iCapacTransCond1PorcenComent = dr.GetOrdinal("capac_trans_cond_1_porcen_coment");
                    if (!dr.IsDBNull(iCapacTransCond1PorcenComent)) entity.CapacTransCond1PorcenComent = Convert.ToString(dr.GetValue(iCapacTransCond1PorcenComent));

                    int iCapacTransCond1MinComent = dr.GetOrdinal("capac_trans_cond_1_min_coment");
                    if (!dr.IsDBNull(iCapacTransCond1MinComent)) entity.CapacTransCond1MinComent = Convert.ToString(dr.GetValue(iCapacTransCond1MinComent));

                    int iCapacTransCond1AComent = dr.GetOrdinal("capac_trans_cond_1_a_coment");
                    if (!dr.IsDBNull(iCapacTransCond1AComent)) entity.CapacTransCond1AComent = Convert.ToString(dr.GetValue(iCapacTransCond1AComent));

                    int iCapacTransCond2PorcenComent = dr.GetOrdinal("capac_trans_cond_2_porcen_coment");
                    if (!dr.IsDBNull(iCapacTransCond2PorcenComent)) entity.CapacTransCond2PorcenComent = Convert.ToString(dr.GetValue(iCapacTransCond2PorcenComent));

                    int iCapacTransCond2MinComent = dr.GetOrdinal("capac_trans_cond_2_min_coment");
                    if (!dr.IsDBNull(iCapacTransCond2MinComent)) entity.CapacTransCond2MinComent = Convert.ToString(dr.GetValue(iCapacTransCond2MinComent));

                    int iCapacTransCond2AComent = dr.GetOrdinal("capac_trans_cond_2_a_coment");
                    if (!dr.IsDBNull(iCapacTransCond2AComent)) entity.CapacTransCond2AComent = Convert.ToString(dr.GetValue(iCapacTransCond2AComent));

                    int iCapacidadTransmisionAComent = dr.GetOrdinal("capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionAComent)) entity.CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionAComent));

                    int iCapacidadTransmisionMvaComent = dr.GetOrdinal("capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvaComent)) entity.CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvaComent));

                    int iLimiteSegCoesComent = dr.GetOrdinal("limite_seg_coes_coment");
                    if (!dr.IsDBNull(iLimiteSegCoesComent)) entity.LimiteSegCoesComent = Convert.ToString(dr.GetValue(iLimiteSegCoesComent));

                    int iFactorLimitanteCalcComent = dr.GetOrdinal("factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iFactorLimitanteCalcComent)) entity.FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iFactorLimitanteCalcComent));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListLineaEvaluacionPrincipal(string equicodi, string codigo, string emprcodi, string equiestado, string idsuestacion1, string idsuestacion2, string idarea, string tension)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListLineaEvaluacionPrincipal);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, codigo);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, equiestado);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, equiestado);
            dbProvider.AddInParameter(command, helper.Subestacion1, DbType.String, idsuestacion1);
            dbProvider.AddInParameter(command, helper.Subestacion1, DbType.String, idsuestacion1);
            dbProvider.AddInParameter(command, helper.Subestacion2, DbType.String, idsuestacion2);
            dbProvider.AddInParameter(command, helper.Subestacion2, DbType.String, idsuestacion2);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.String, idarea);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.String, idarea);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iEquiAbrev = dr.GetOrdinal(helper.EquiAbrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EquiAbrev = Convert.ToString(dr.GetValue(iEquiAbrev));

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iSubestacion1 = dr.GetOrdinal(helper.Subestacion1);
                    if (!dr.IsDBNull(iSubestacion1)) entity.Subestacion1 = Convert.ToString(dr.GetValue(iSubestacion1));

                    int iSubestacion2 = dr.GetOrdinal(helper.Subestacion2);
                    if (!dr.IsDBNull(iSubestacion2)) entity.Subestacion2 = Convert.ToString(dr.GetValue(iSubestacion2));

                    int iCapacidadA = dr.GetOrdinal(helper.CapacidadA);
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMva = dr.GetOrdinal(helper.CapacidadMva);
                    if (!dr.IsDBNull(iCapacidadMva)) entity.CapacidadMva = Convert.ToString(dr.GetValue(iCapacidadMva));

                    int iCapacTransCond1Porcen = dr.GetOrdinal(helper.CapacTransCond1Porcen);
                    if (!dr.IsDBNull(iCapacTransCond1Porcen)) entity.CapacTransCond1Porcen = Convert.ToString(dr.GetValue(iCapacTransCond1Porcen));

                    int iCapacTransCond1Min = dr.GetOrdinal(helper.CapacTransCond1Min);
                    if (!dr.IsDBNull(iCapacTransCond1Min)) entity.CapacTransCond1Min = Convert.ToString(dr.GetValue(iCapacTransCond1Min));

                    int iCapacTransCond2Porcen = dr.GetOrdinal(helper.CapacTransCond2Porcen);
                    if (!dr.IsDBNull(iCapacTransCond2Porcen)) entity.CapacTransCond2Porcen = Convert.ToString(dr.GetValue(iCapacTransCond2Porcen));

                    int iCapacTransCond2Min = dr.GetOrdinal(helper.CapacTransCond2Min);
                    if (!dr.IsDBNull(iCapacTransCond2Min)) entity.CapacTransCond2Min = Convert.ToString(dr.GetValue(iCapacTransCond2Min));

                    int iFactorLimitanteFinal = dr.GetOrdinal(helper.FactorLimitanteFinal);
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fechamodificacionstr);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fechamodificacionstr = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iUsuarioAuditoria = dr.GetOrdinal(helper.UsuarioAuditoria);
                    if (!dr.IsDBNull(iUsuarioAuditoria)) entity.UsuarioAuditoria = Convert.ToString(dr.GetValue(iUsuarioAuditoria));

                    int iCapacidadTransmisionAComent = dr.GetOrdinal("capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionAComent)) entity.CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionAComent));

                    int iCapacidadTransmisionMvaComent = dr.GetOrdinal("capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvaComent)) entity.CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvaComent));

                    int iCapacTransCond1PorcenComent = dr.GetOrdinal("capac_trans_cond_1_porcen_coment");
                    if (!dr.IsDBNull(iCapacTransCond1PorcenComent)) entity.CapacTransCond1PorcenComent = Convert.ToString(dr.GetValue(iCapacTransCond1PorcenComent));

                    int iCapacTransCond1MinComent = dr.GetOrdinal("capac_trans_cond_1_min_coment");
                    if (!dr.IsDBNull(iCapacTransCond1MinComent)) entity.CapacTransCond1MinComent = Convert.ToString(dr.GetValue(iCapacTransCond1MinComent));

                    int iCapacTransCond2PorcenComent = dr.GetOrdinal("capac_trans_cond_2_porcen_coment");
                    if (!dr.IsDBNull(iCapacTransCond2PorcenComent)) entity.CapacTransCond2PorcenComent = Convert.ToString(dr.GetValue(iCapacTransCond2PorcenComent));

                    int iCapacTransCond2MinComent = dr.GetOrdinal("capac_trans_cond_2_min_coment");
                    if (!dr.IsDBNull(iCapacTransCond2MinComent)) entity.CapacTransCond2MinComent = Convert.ToString(dr.GetValue(iCapacTransCond2MinComent));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }




        #endregion

        public List<EprEquipoDTO> ListaBuscarTransformador(int tipo, string codigoId, string codigo, int ubicacion,
            int empresa, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListBuscarTransformador);
            dbProvider.AddInParameter(command, "tipo", DbType.Int32, tipo);
            dbProvider.AddInParameter(command, "tipo", DbType.Int32, tipo);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "ubicacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iTipo = dr.GetOrdinal("tipo");
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToString(dr.GetValue(iTipo));

                    int iAreaDesc = dr.GetOrdinal("areadesc");
                    if (!dr.IsDBNull(iAreaDesc)) entity.AreaDescripcion = Convert.ToString(dr.GetValue(iAreaDesc));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iEquipoEstadoDescripcion = dr.GetOrdinal("equiestadodesc");
                    if (!dr.IsDBNull(iEquipoEstadoDescripcion)) entity.EquipoEstadoDescripcion = Convert.ToString(dr.GetValue(iEquipoEstadoDescripcion));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqFamiliaDTO> ListaTransformadores()
        {

            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListTransformadores);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal("famcodi");
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamabrev = dr.GetOrdinal("famabrev");
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = Convert.ToString(dr.GetValue(iFamabrev));

                    int iTipoecodi = dr.GetOrdinal("tipoecodi");
                    if (!dr.IsDBNull(iTipoecodi)) entity.Tipoecodi = Convert.ToInt32(dr.GetValue(iTipoecodi));

                    int iTareacodi = dr.GetOrdinal("tareacodi");
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

                    int iFamnomb = dr.GetOrdinal("famnomb");
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = Convert.ToString(dr.GetValue(iFamnomb));

                    int iFamnumconec = dr.GetOrdinal("famnumconec");
                    if (!dr.IsDBNull(iFamnumconec)) entity.Famnumconec = Convert.ToInt32(dr.GetValue(iFamnumconec));

                    int iFamnombgraf = dr.GetOrdinal("famnombgraf");
                    if (!dr.IsDBNull(iFamnombgraf)) entity.Famnombgraf = Convert.ToString(dr.GetValue(iFamnombgraf));

                    int iFamestado = dr.GetOrdinal("famestado");
                    if (!dr.IsDBNull(iFamestado)) entity.Famestado = Convert.ToString(dr.GetValue(iFamestado));

                    int iUsuarioCreacion = dr.GetOrdinal("usuariocreacion");
                    if (!dr.IsDBNull(iUsuarioCreacion)) entity.UsuarioCreacion = Convert.ToString(dr.GetValue(iUsuarioCreacion));

                    int iFechaCreacion = dr.GetOrdinal("fechacreacion");
                    if (!dr.IsDBNull(iFechaCreacion)) entity.FechaCreacion = Convert.ToDateTime(dr.GetValue(iFechaCreacion));

                    int iUsuarioUpdate = dr.GetOrdinal("usuarioupdate");
                    if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = Convert.ToString(dr.GetValue(iUsuarioUpdate));

                    int iFechaUpdate = dr.GetOrdinal("fechaupdate");
                    if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = Convert.ToDateTime(dr.GetValue(iFechaUpdate));
                    
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaTransversalConsultarEquipo(string codigoId)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListTransversalConsultarEquipo);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iPropiedad = dr.GetOrdinal("propiedad");
                    if (!dr.IsDBNull(iPropiedad)) entity.Propiedad = Convert.ToString(dr.GetValue(iPropiedad));

                    int iValor = dr.GetOrdinal("valor");
                    if (!dr.IsDBNull(iValor)) entity.Valor = Convert.ToString(dr.GetValue(iValor));

                    int iComentario = dr.GetOrdinal("comentario");
                    if (!dr.IsDBNull(iComentario)) entity.Comentario = Convert.ToString(dr.GetValue(iComentario));

                    int iMotivoActualizacion = dr.GetOrdinal("motivo_actualizacion");
                    if (!dr.IsDBNull(iMotivoActualizacion)) entity.MotivoActualizacion = Convert.ToString(dr.GetValue(iMotivoActualizacion));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerTransversalHistorialActualizacion(string codigoId)
        {
            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTransversalHistorialActualizacion);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListaTransversalActualizaciones(string codigoId)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransversalActualizaciones);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEpproycodi = dr.GetOrdinal("epproycodi");
                    if (!dr.IsDBNull(iEpproycodi)) entity.Epproycodi = Convert.ToString(dr.GetValue(iEpproycodi));

                    int iMotivo = dr.GetOrdinal("motivo");
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = Convert.ToString(dr.GetValue(iMotivo));

                    int iUsuario = dr.GetOrdinal("usuario");
                    if (!dr.IsDBNull(iUsuario)) entity.Usuario = Convert.ToString(dr.GetValue(iUsuario));

                    int iFechaActualizacion = dr.GetOrdinal("fecha_actualizacion");
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.FechaActualizacion = Convert.ToString(dr.GetValue(iFechaActualizacion));

                    int iFechaProyecto = dr.GetOrdinal("fecha_proyecto");
                    if (!dr.IsDBNull(iFechaProyecto)) entity.FechaProyecto = Convert.ToString(dr.GetValue(iFechaProyecto));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaTransversalPropiedadesActualizadas(string codigoId, string proyId)
        {
            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransversalPropiedadesActualizadas);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "epproycodi", DbType.String, proyId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPropnomb = dr.GetOrdinal("propnomb");
                    if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = Convert.ToString(dr.GetValue(iPropnomb));

                    int iValor = dr.GetOrdinal("valor");
                    if (!dr.IsDBNull(iValor)) entity.Valor = Convert.ToString(dr.GetValue(iValor));

                    int iPropequicomentario = dr.GetOrdinal("propequicomentario");
                    if (!dr.IsDBNull(iPropequicomentario)) entity.Propequicomentario = Convert.ToString(dr.GetValue(iPropequicomentario));
   
                    int iPropcodi = dr.GetOrdinal("propcodi");
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToString(dr.GetValue(iPropcodi));

                    int iFechapropequi = dr.GetOrdinal("fechapropequi");
                    if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = Convert.ToString(dr.GetValue(iFechapropequi));

                    int iOrden = dr.GetOrdinal("orden");
                    if (!dr.IsDBNull(iOrden)) entity.Orden = Convert.ToString(dr.GetValue(iOrden));

                    int iFechaVigencia = dr.GetOrdinal("fecha_vigencia");
                    if (!dr.IsDBNull(iFechaVigencia)) entity.FechaVigencia = Convert.ToString(dr.GetValue(iFechaVigencia));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public string RegistrarReactor(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRegistrarReactor);

            dbProvider.AddInParameter(command, helper.IdReactor, DbType.Int32, equipo.IdReactor);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.IdCelda1, DbType.String, equipo.IdCelda1);
            dbProvider.AddInParameter(command, helper.IdCelda2, DbType.String, equipo.IdCelda22);
            dbProvider.AddInParameter(command, helper.CapacidadMvar, DbType.String, equipo.CapacidadMvar);
            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionAComent, DbType.String, equipo.CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvar, DbType.String, equipo.CapacidadTransmisionMvar);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvarComent, DbType.String, equipo.CapacidadTransmisionMvarComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalcComent, DbType.String, equipo.FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinalComent, DbType.String, equipo.FactorLimitanteFinalComent);
            dbProvider.AddInParameter(command, helper.Observaciones, DbType.String, equipo.Observaciones);
            dbProvider.AddInParameter(command, helper.UsuarioAuditoria, DbType.String, equipo.UsuarioAuditoria);

            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }

        public string RegistrarTransformador(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRegistrarTransformador);

            dbProvider.AddInParameter(command, helper.IdTransformador, DbType.Int32, equipo.IdTransformador);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.D1IdCelda, DbType.String, equipo.D1IdCelda);
            dbProvider.AddInParameter(command, helper.D1CapacidadOnanMva, DbType.String, equipo.D1CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D1CapacidadOnanMvaComent, DbType.String, equipo.D1CapacidadOnanMvaComent);
            dbProvider.AddInParameter(command, helper.D1CapacidadOnafMva, DbType.String, equipo.D1CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D1CapacidadOnafMvaComent, DbType.String, equipo.D1CapacidadOnafMvaComent);
            dbProvider.AddInParameter(command, helper.D1CapacidadMva, DbType.String, equipo.D1CapacidadMva);
            dbProvider.AddInParameter(command, helper.D1CapacidadMvaComent, DbType.String, equipo.D1CapacidadMvaComent);
            dbProvider.AddInParameter(command, helper.D1CapacidadA, DbType.String, equipo.D1CapacidadA);
            dbProvider.AddInParameter(command, helper.D1CapacidadAComent, DbType.String, equipo.D1CapacidadAComent);
            dbProvider.AddInParameter(command, helper.D1PosicionTcA, DbType.String, equipo.D1PosicionTcA);
            dbProvider.AddInParameter(command, helper.D1PosicionPickUpA, DbType.String, equipo.D1PosicionPickUpA);
            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionA, DbType.String, equipo.D1CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionAComent, DbType.String, equipo.D1CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionMva, DbType.String, equipo.D1CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionMvaComent, DbType.String, equipo.D1CapacidadTransmisionMvaComent);
            dbProvider.AddInParameter(command, helper.D1FactorLimitanteCalc, DbType.String, equipo.D1FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D1FactorLimitanteCalcComent, DbType.String, equipo.D1FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.D1FactorLimitanteFinal, DbType.String, equipo.D1FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D1FactorLimitanteFinalComent, DbType.String, equipo.D1FactorLimitanteFinalComent);
            dbProvider.AddInParameter(command, helper.D2IdCelda, DbType.String, equipo.D2IdCelda);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnanMva, DbType.String, equipo.D2CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnanMvaComent, DbType.String, equipo.D2CapacidadOnanMvaComent);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnafMva, DbType.String, equipo.D2CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnafMvaComent, DbType.String, equipo.D2CapacidadOnafMvaComent);
            dbProvider.AddInParameter(command, helper.D2CapacidadMva, DbType.String, equipo.D2CapacidadMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadMvaComent, DbType.String, equipo.D2CapacidadMvaComent);
            dbProvider.AddInParameter(command, helper.D2CapacidadA, DbType.String, equipo.D2CapacidadA);
            dbProvider.AddInParameter(command, helper.D2CapacidadAComent, DbType.String, equipo.D2CapacidadAComent);
            dbProvider.AddInParameter(command, helper.D2PosicionTcA, DbType.String, equipo.D2PosicionTcA);
            dbProvider.AddInParameter(command, helper.D2PosicionPickUpA, DbType.String, equipo.D2PosicionPickUpA);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionA, DbType.String, equipo.D2CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionAComent, DbType.String, equipo.D2CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionMva, DbType.String, equipo.D2CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionMvaComent, DbType.String, equipo.D2CapacidadTransmisionMvaComent);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteCalc, DbType.String, equipo.D2FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteCalcComent, DbType.String, equipo.D2FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteFinal, DbType.String, equipo.D2FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteFinalComent, DbType.String, equipo.D2FactorLimitanteFinalComent);
            dbProvider.AddInParameter(command, helper.D3IdCelda, DbType.String, equipo.D3IdCelda);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnanMva, DbType.String, equipo.D3CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnanMvaComent, DbType.String, equipo.D3CapacidadOnanMvaComent);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnafMva, DbType.String, equipo.D3CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnafMvaComent, DbType.String, equipo.D3CapacidadOnafMvaComent);
            dbProvider.AddInParameter(command, helper.D3CapacidadMva, DbType.String, equipo.D3CapacidadMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadMvaComent, DbType.String, equipo.D3CapacidadMvaComent);
            dbProvider.AddInParameter(command, helper.D3CapacidadA, DbType.String, equipo.D3CapacidadA);
            dbProvider.AddInParameter(command, helper.D3CapacidadAComent, DbType.String, equipo.D3CapacidadAComent);
            dbProvider.AddInParameter(command, helper.D3PosicionTcA, DbType.String, equipo.D3PosicionTcA);
            dbProvider.AddInParameter(command, helper.D3PosicionPickUpA, DbType.String, equipo.D3PosicionPickUpA);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionA, DbType.String, equipo.D3CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionAComent, DbType.String, equipo.D3CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionMva, DbType.String, equipo.D3CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionMvaComent, DbType.String, equipo.D3CapacidadTransmisionMvaComent);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteCalc, DbType.String, equipo.D3FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteCalcComent, DbType.String, equipo.D3FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteFinal, DbType.String, equipo.D3FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteFinalComent, DbType.String, equipo.D3FactorLimitanteFinalComent);
            
            dbProvider.AddInParameter(command, helper.D4IdCelda, DbType.String, equipo.D4IdCelda);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnanMva, DbType.String, equipo.D4CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnanMvaComent, DbType.String, equipo.D4CapacidadOnanMvaComent);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnafMva, DbType.String, equipo.D4CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnafMvaComent, DbType.String, equipo.D4CapacidadOnafMvaComent);
            dbProvider.AddInParameter(command, helper.D4CapacidadMva, DbType.String, equipo.D4CapacidadMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadMvaComent, DbType.String, equipo.D4CapacidadMvaComent);
            dbProvider.AddInParameter(command, helper.D4CapacidadA, DbType.String, equipo.D4CapacidadA);
            dbProvider.AddInParameter(command, helper.D4CapacidadAComent, DbType.String, equipo.D4CapacidadAComent);
            dbProvider.AddInParameter(command, helper.D4PosicionTcA, DbType.String, equipo.D4PosicionTcA);
            dbProvider.AddInParameter(command, helper.D4PosicionPickUpA, DbType.String, equipo.D4PosicionPickUpA);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionA, DbType.String, equipo.D4CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionAComent, DbType.String, equipo.D4CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionMva, DbType.String, equipo.D4CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionMvaComent, DbType.String, equipo.D4CapacidadTransmisionMvaComent);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteCalc, DbType.String, equipo.D4FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteCalcComent, DbType.String, equipo.D4FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteFinal, DbType.String, equipo.D4FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteFinalComent, DbType.String, equipo.D4FactorLimitanteFinalComent);

            dbProvider.AddInParameter(command, helper.Observaciones, DbType.String, equipo.Observaciones);
            dbProvider.AddInParameter(command, helper.UsuarioAuditoria, DbType.String, equipo.UsuarioAuditoria);

            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }

        public List<EprEquipoDTO> ListaReactor(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReactor);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("codigo_id");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iTension = dr.GetOrdinal("tension");
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iCapacidadA = dr.GetOrdinal("capacidad_a");
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMvar = dr.GetOrdinal("capacidad_mvar");
                    if (!dr.IsDBNull(iCapacidadMvar)) entity.CapacidadMvar = Convert.ToString(dr.GetValue(iCapacidadMvar));

                    int iFactorLimitanteFinal = dr.GetOrdinal("factor_limitante_final");
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    int iActualizadoPor = dr.GetOrdinal("actualizado_por");
                    if (!dr.IsDBNull(iActualizadoPor)) entity.ActualizadoPor = Convert.ToString(dr.GetValue(iActualizadoPor));

                    int iActualizadoEl = dr.GetOrdinal("actualizado_el");
                    if (!dr.IsDBNull(iActualizadoEl)) entity.ActualizadoEl = Convert.ToString(dr.GetValue(iActualizadoEl));

                    int iCapacidadAComent = dr.GetOrdinal("capacidad_a_coment");
                    if (!dr.IsDBNull(iCapacidadAComent)) entity.CapacidadAComent = Convert.ToString(dr.GetValue(iCapacidadAComent));

                    int iCapacidadMvaComent = dr.GetOrdinal("capacidad_mvar_coment");
                    if (!dr.IsDBNull(iCapacidadMvaComent)) entity.CapacidadMvaComent = Convert.ToString(dr.GetValue(iCapacidadMvaComent));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerReactorPorId(int codigoId)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerReactorPorId);
            dbProvider.AddInParameter(command, "v_equicodi", DbType.Int32, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iIdProyecto = dr.GetOrdinal("id_proyecto");
                    if (!dr.IsDBNull(iIdProyecto)) entity.IdProyecto = Convert.ToInt32(dr.GetValue(iIdProyecto));
                    
                    int iProyecto = dr.GetOrdinal("proyecto");
                    if (!dr.IsDBNull(iProyecto)) entity.Proyecto = Convert.ToString(dr.GetValue(iProyecto));

                    int iFechaActualizacion = dr.GetOrdinal("fecha_actualizacion");
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.FechaActualizacion = Convert.ToString(dr.GetValue(iFechaActualizacion));

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iIdUbicacion = dr.GetOrdinal("id_ubicacion");
                    if (!dr.IsDBNull(iIdUbicacion)) entity.IdUbicacion = Convert.ToInt32(dr.GetValue(iIdUbicacion));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iIdEmpresa = dr.GetOrdinal("id_empresa");
                    if (!dr.IsDBNull(iIdEmpresa)) entity.IdEmpresa = Convert.ToString(dr.GetValue(iIdEmpresa));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iIdCelda1 = dr.GetOrdinal("id_celda_1");
                    if (!dr.IsDBNull(iIdCelda1)) entity.IdCelda1 = Convert.ToString(dr.GetValue(iIdCelda1));

                    int iCelda1PosicionNucleoTc = dr.GetOrdinal("celda_1_posicion_nucleo_tc");
                    if (!dr.IsDBNull(iCelda1PosicionNucleoTc)) entity.Celda1PosicionNucleoTc = Convert.ToString(dr.GetValue(iCelda1PosicionNucleoTc));

                    int iCelda1PickUp = dr.GetOrdinal("celda_1_pick_up");
                    if (!dr.IsDBNull(iCelda1PickUp)) entity.Celda1PickUp = Convert.ToString(dr.GetValue(iCelda1PickUp));

                    int iIdCelda2 = dr.GetOrdinal("id_celda_2");
                    if (!dr.IsDBNull(iIdCelda2)) entity.IdCelda22 = Convert.ToString(dr.GetValue(iIdCelda2));

                    int iCelda2PosicionNucleoTc = dr.GetOrdinal("celda_2_posicion_nucleo_tc");
                    if (!dr.IsDBNull(iCelda2PosicionNucleoTc)) entity.Celda2PosicionNucleoTc = Convert.ToString(dr.GetValue(iCelda2PosicionNucleoTc));

                    int iCelda2PickUp = dr.GetOrdinal("celda_2_pick_up");
                    if (!dr.IsDBNull(iCelda2PickUp)) entity.Celda2PickUp = Convert.ToString(dr.GetValue(iCelda2PickUp));

                    int iNivelTension = dr.GetOrdinal("nivel_tension");
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = Convert.ToString(dr.GetValue(iNivelTension));

                    int iCapacidadMvar = dr.GetOrdinal("capacidad_mvar");
                    if (!dr.IsDBNull(iCapacidadMvar)) entity.CapacidadMvar = Convert.ToString(dr.GetValue(iCapacidadMvar));

                    int iCapacidadA = dr.GetOrdinal("capacidad_a");
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadTransmisionA = dr.GetOrdinal("capacidad_transmision_a");
                    if (!dr.IsDBNull(iCapacidadTransmisionA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadTransmisionA));

                    int iCapacidadTransmisionAComent = dr.GetOrdinal("capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionAComent)) entity.CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionAComent));

                    int iCapacidadTransmisionMvar = dr.GetOrdinal("capacidad_transmision_mvar");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvar)) entity.CapacidadTransmisionMvar = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvar));

                    int iCapacidadTransmisionMvarComent = dr.GetOrdinal("capacidad_transmision_mvar_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvarComent)) entity.CapacidadTransmisionMvarComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvarComent));

                    int iFactorLimitanteCalc = dr.GetOrdinal("factor_limitante_calc");
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.FactorLimitanteCalc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteCalcComent = dr.GetOrdinal("factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iFactorLimitanteCalcComent)) entity.FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iFactorLimitanteCalcComent));

                    int iFactorLimitanteFinal = dr.GetOrdinal("factor_limitante_final");
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                    int iObservaciones = dr.GetOrdinal("observaciones");
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iUsuarioModificacion = dr.GetOrdinal("usumodificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecmodificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListaCeldaAcoplamiento(string codigoId, string codigo, int ubicacion,int empresa, int area
            , string estado, string tension)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCeldaAcoplamiento);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, ubicacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("codigo_id");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iTension = dr.GetOrdinal("tension");
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iCapacidadA = dr.GetOrdinal("capacidad_transmision_a");
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMvar = dr.GetOrdinal("capacidad_transmision_mva");
                    if (!dr.IsDBNull(iCapacidadMvar)) entity.CapacidadTransmisionMVA = Convert.ToString(dr.GetValue(iCapacidadMvar));

                    int iFactorLimitanteFinal = dr.GetOrdinal("factor_limitante_final");
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    int iActualizadoPor = dr.GetOrdinal("actualizado_por");
                    if (!dr.IsDBNull(iActualizadoPor)) entity.ActualizadoPor = Convert.ToString(dr.GetValue(iActualizadoPor));

                    int iActualizadoEl = dr.GetOrdinal("actualizado_el");
                    if (!dr.IsDBNull(iActualizadoEl)) entity.ActualizadoEl = Convert.ToString(dr.GetValue(iActualizadoEl));

                    int iCapacidadTransmisionAComent = dr.GetOrdinal("capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionAComent)) entity.CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionAComent));

                    int iCapacidadTransmisionMvaComent = dr.GetOrdinal("capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvaComent)) entity.CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvaComent));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerCeldaAcoplamientoPorId(int codigoId)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCeldaAcoplamientoPorId);
            dbProvider.AddInParameter(command, "v_equicodi", DbType.Int32, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iIdProyecto = dr.GetOrdinal("id_proyecto");
                    if (!dr.IsDBNull(iIdProyecto)) entity.IdProyecto = Convert.ToInt32(dr.GetValue(iIdProyecto));

                    int iProyecto = dr.GetOrdinal("proyecto");
                    if (!dr.IsDBNull(iProyecto)) entity.Proyecto = Convert.ToString(dr.GetValue(iProyecto));

                    int iFechaActualizacion = dr.GetOrdinal("fecha_actualizacion");
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.FechaActualizacion = Convert.ToString(dr.GetValue(iFechaActualizacion));

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iIdUbicacion = dr.GetOrdinal("id_ubicacion");
                    if (!dr.IsDBNull(iIdUbicacion)) entity.IdUbicacion = Convert.ToInt32(dr.GetValue(iIdUbicacion));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iIdEmpresa = dr.GetOrdinal("id_empresa");
                    if (!dr.IsDBNull(iIdEmpresa)) entity.IdEmpresa = Convert.ToString(dr.GetValue(iIdEmpresa));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iPosicionNucleoTc = dr.GetOrdinal("posicion_nucleo_tc");
                    if (!dr.IsDBNull(iPosicionNucleoTc)) entity.PosicionNucleoTc = Convert.ToString(dr.GetValue(iPosicionNucleoTc));

                    int iPickUp = dr.GetOrdinal("pick_up");
                    if (!dr.IsDBNull(iPickUp)) entity.PickUp = Convert.ToString(dr.GetValue(iPickUp));

                    int iIdInterruptor = dr.GetOrdinal("id_interruptor");
                    if (!dr.IsDBNull(iIdInterruptor)) entity.IdInterruptor = Convert.ToString(dr.GetValue(iIdInterruptor));

                    int iInterruptorEmpresa = dr.GetOrdinal("interruptor_empresa");
                    if (!dr.IsDBNull(iInterruptorEmpresa)) entity.InterruptorEmpresa = Convert.ToString(dr.GetValue(iInterruptorEmpresa));

                    int iInterruptorTension = dr.GetOrdinal("interruptor_tension");
                    if (!dr.IsDBNull(iInterruptorTension)) entity.InterruptorTension = Convert.ToString(dr.GetValue(iInterruptorTension));

                    int iInterruptorCapacidadA = dr.GetOrdinal("interruptor_capacidad_a");
                    if (!dr.IsDBNull(iInterruptorCapacidadA)) entity.InterruptorCapacidadA = Convert.ToString(dr.GetValue(iInterruptorCapacidadA));

                    int iInterruptorCapacidadAComent = dr.GetOrdinal("interruptor_capacidad_a_coment");
                    if (!dr.IsDBNull(iInterruptorCapacidadAComent)) entity.InterruptorCapacidadAComent = Convert.ToString(dr.GetValue(iInterruptorCapacidadAComent));

                    int iInterruptorCapacidadMva = dr.GetOrdinal("interruptor_capacidad_mva");
                    if (!dr.IsDBNull(iInterruptorCapacidadMva)) entity.InterruptorCapacidadMva = Convert.ToString(dr.GetValue(iInterruptorCapacidadMva));

                    int iInterruptorCapacidadMvaComent = dr.GetOrdinal("interruptor_capacidad_mva_coment");
                    if (!dr.IsDBNull(iInterruptorCapacidadMvaComent)) entity.InterruptorCapacidadMvaComent = Convert.ToString(dr.GetValue(iInterruptorCapacidadMvaComent));

                    int iCapacidadTransmisionA = dr.GetOrdinal("capacidad_transmision_a");
                    if (!dr.IsDBNull(iCapacidadTransmisionA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadTransmisionA));

                    int iCapacidadTransmisionAComent = dr.GetOrdinal("capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionAComent)) entity.CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionAComent));

                    int iCapacidadTransmisionMva = dr.GetOrdinal("capacidad_transmision_mva");
                    if (!dr.IsDBNull(iCapacidadTransmisionMva)) entity.CapacidadTransmisionMva = Convert.ToString(dr.GetValue(iCapacidadTransmisionMva));

                    int iCapacidadTransmisionMvaComent = dr.GetOrdinal("capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iCapacidadTransmisionMvaComent)) entity.CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iCapacidadTransmisionMvaComent));

                    int iFactorLimitanteCalc = dr.GetOrdinal("factor_limitante_calc");
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.FactorLimitanteCalc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteCalcComent = dr.GetOrdinal("factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iFactorLimitanteCalcComent)) entity.FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iFactorLimitanteCalcComent));

                    int iFactorLimitanteFinal = dr.GetOrdinal("factor_limitante_final");
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iFactorLimitanteFinalComent = dr.GetOrdinal("factor_limitante_final_coment");
                    if (!dr.IsDBNull(iFactorLimitanteFinalComent)) entity.FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iFactorLimitanteFinalComent));

                    int iObservaciones = dr.GetOrdinal("observaciones");
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iUsuarioModificacion = dr.GetOrdinal("usumodificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecmodificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListaReleSobretension(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReleSobretension);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iNivelTension = dr.GetOrdinal("nivel_tension");
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = Convert.ToString(dr.GetValue(iNivelTension));

                    int iSobreTU = dr.GetOrdinal("sobre_t_u");
                    if (!dr.IsDBNull(iSobreTU)) entity.SobreTU = Convert.ToString(dr.GetValue(iSobreTU));

                    int iSobreTT = dr.GetOrdinal("sobre_t_t");
                    if (!dr.IsDBNull(iSobreTT)) entity.SobreTT = Convert.ToString(dr.GetValue(iSobreTT));

                    int iSobreTUU = dr.GetOrdinal("sobre_t_uu");
                    if (!dr.IsDBNull(iSobreTUU)) entity.SobreTUU = Convert.ToString(dr.GetValue(iSobreTUU));

                    int iSobreTTT = dr.GetOrdinal("sobre_t_tt");
                    if (!dr.IsDBNull(iSobreTTT)) entity.SobreTTT = Convert.ToString(dr.GetValue(iSobreTTT));

                    int iUsuarioModificacion = dr.GetOrdinal("usuario_modificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecha_modificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReleMandoSincronizado);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iCodigoInterruptor = dr.GetOrdinal("codigo_interruptor");
                    if (!dr.IsDBNull(iCodigoInterruptor)) entity.CodigoInterruptor = Convert.ToString(dr.GetValue(iCodigoInterruptor));

                    int iMandoSincronizado = dr.GetOrdinal("mando_sincronizado");
                    if (!dr.IsDBNull(iMandoSincronizado)) entity.MandoSincronizado = Convert.ToString(dr.GetValue(iMandoSincronizado));

                    int iUsuarioModificacion = dr.GetOrdinal("usuario_modificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecha_modificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaReleTorcional(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReleTorcional);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iMedidaMitigacion = dr.GetOrdinal("medida_mitigacion");
                    if (!dr.IsDBNull(iMedidaMitigacion)) entity.MedidaMitigacion = Convert.ToString(dr.GetValue(iMedidaMitigacion));

                    int iReleTorsionalImplementadoDsc = dr.GetOrdinal("rele_torsional_implementado_dsc");
                    if (!dr.IsDBNull(iReleTorsionalImplementadoDsc)) entity.ReleTorsionalImplementadoDsc = Convert.ToString(dr.GetValue(iReleTorsionalImplementadoDsc));

                    int iUsuarioModificacion = dr.GetOrdinal("usuario_modificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecha_modificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaRelePMU(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListRelePMU);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iAccion = dr.GetOrdinal("accion");
                    if (!dr.IsDBNull(iAccion)) entity.Accion = Convert.ToString(dr.GetValue(iAccion));

                    int iUsuarioModificacion = dr.GetOrdinal("usuario_modificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecha_modificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EprEquipoDTO> ListaTransformador(string codigoId, string codigo, int tipo, int subestacion,
            int empresa, int area, string estado, string tension)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransformador);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "tipo", DbType.Int32, tipo);
            dbProvider.AddInParameter(command, "tipo", DbType.Int32, tipo);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "id_subestacion", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "id_area", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);
            dbProvider.AddInParameter(command, "tension", DbType.String, tension);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("codigo_id");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    int iEquipoEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquipoEstado)) entity.EquipoEstado = Convert.ToString(dr.GetValue(iEquipoEstado));

                    int iActualizadoPor = dr.GetOrdinal("actualizado_por");
                    if (!dr.IsDBNull(iActualizadoPor)) entity.ActualizadoPor = Convert.ToString(dr.GetValue(iActualizadoPor));

                    int iActualizadoEl = dr.GetOrdinal("actualizado_el");
                    if (!dr.IsDBNull(iActualizadoEl)) entity.ActualizadoEl = Convert.ToString(dr.GetValue(iActualizadoEl));

                    int iTipo= dr.GetOrdinal("tipo");
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToString(dr.GetValue(iTipo));

                    int iFamcodi = dr.GetOrdinal("famcodi");
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iD1CapacidadMva = dr.GetOrdinal("d1_capacidad_mva");
                    if (!dr.IsDBNull(iD1CapacidadMva)) entity.D1CapacidadMva = Convert.ToString(dr.GetValue(iD1CapacidadMva));

                    int iD2CapacidadMva = dr.GetOrdinal("d2_capacidad_mva");
                    if (!dr.IsDBNull(iD2CapacidadMva)) entity.D2CapacidadMva = Convert.ToString(dr.GetValue(iD2CapacidadMva));

                    int iD3CapacidadMva = dr.GetOrdinal("d3_capacidad_mva");
                    if (!dr.IsDBNull(iD3CapacidadMva)) entity.D3CapacidadMva = Convert.ToString(dr.GetValue(iD3CapacidadMva));

                    int iD4CapacidadMva = dr.GetOrdinal("d4_capacidad_mva");
                    if (!dr.IsDBNull(iD4CapacidadMva)) entity.D4CapacidadMva = Convert.ToString(dr.GetValue(iD4CapacidadMva));

                    int iD1CapacidadMvaComent = dr.GetOrdinal("d1_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD1CapacidadMvaComent)) entity.D1CapacidadMvaComent = Convert.ToString(dr.GetValue(iD1CapacidadMvaComent));

                    int iD2CapacidadMvaComent = dr.GetOrdinal("d2_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD2CapacidadMvaComent)) entity.D2CapacidadMvaComent = Convert.ToString(dr.GetValue(iD2CapacidadMvaComent));

                    int iD3CapacidadMvaComent = dr.GetOrdinal("d3_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD3CapacidadMvaComent)) entity.D3CapacidadMvaComent = Convert.ToString(dr.GetValue(iD3CapacidadMvaComent));

                    int iD4CapacidadMvaComent = dr.GetOrdinal("d4_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD4CapacidadMvaComent)) entity.D4CapacidadMvaComent = Convert.ToString(dr.GetValue(iD4CapacidadMvaComent));

                    int iD1CapacidadA = dr.GetOrdinal("d1_capacidad_a");
                    if (!dr.IsDBNull(iD1CapacidadA)) entity.D1CapacidadA = Convert.ToString(dr.GetValue(iD1CapacidadA));

                    int iD2CapacidadA = dr.GetOrdinal("d2_capacidad_a");
                    if (!dr.IsDBNull(iD2CapacidadA)) entity.D2CapacidadA = Convert.ToString(dr.GetValue(iD2CapacidadA));

                    int iD3CapacidadA = dr.GetOrdinal("d3_capacidad_a");
                    if (!dr.IsDBNull(iD3CapacidadA)) entity.D3CapacidadA = Convert.ToString(dr.GetValue(iD3CapacidadA));

                    int iD4CapacidadA = dr.GetOrdinal("d4_capacidad_a");
                    if (!dr.IsDBNull(iD4CapacidadA)) entity.D4CapacidadA = Convert.ToString(dr.GetValue(iD4CapacidadA));

                    int iD1CapacidadAComent = dr.GetOrdinal("d1_capacidad_a_coment");
                    if (!dr.IsDBNull(iD1CapacidadAComent)) entity.D1CapacidadAComent = Convert.ToString(dr.GetValue(iD1CapacidadAComent));

                    int iD2CapacidadAComent = dr.GetOrdinal("d2_capacidad_a_coment");
                    if (!dr.IsDBNull(iD2CapacidadAComent)) entity.D2CapacidadAComent = Convert.ToString(dr.GetValue(iD2CapacidadAComent));

                    int iD3CapacidadAComent = dr.GetOrdinal("d3_capacidad_a_coment");
                    if (!dr.IsDBNull(iD3CapacidadAComent)) entity.D3CapacidadAComent = Convert.ToString(dr.GetValue(iD3CapacidadAComent));

                    int iD4CapacidadAComent = dr.GetOrdinal("d4_capacidad_a_coment");
                    if (!dr.IsDBNull(iD4CapacidadAComent)) entity.D4CapacidadAComent = Convert.ToString(dr.GetValue(iD4CapacidadAComent));

                    int iD1FactorLimitanteFinal = dr.GetOrdinal("d1_factor_limitante_final");
                    if (!dr.IsDBNull(iD1FactorLimitanteFinal)) entity.D1FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD1FactorLimitanteFinal));

                    int iD2FactorLimitanteFinal = dr.GetOrdinal("d2_factor_limitante_final");
                    if (!dr.IsDBNull(iD2FactorLimitanteFinal)) entity.D2FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD2FactorLimitanteFinal));

                    int iD3FactorLimitanteFinal = dr.GetOrdinal("d3_factor_limitante_final");
                    if (!dr.IsDBNull(iD3FactorLimitanteFinal)) entity.D3FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD3FactorLimitanteFinal));

                    int iD4FactorLimitanteFinal = dr.GetOrdinal("d4_factor_limitante_final");
                    if (!dr.IsDBNull(iD4FactorLimitanteFinal)) entity.D4FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD4FactorLimitanteFinal));

                    int iD1FactorLimitanteFinalComent = dr.GetOrdinal("d1_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD1FactorLimitanteFinalComent)) entity.D1FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD1FactorLimitanteFinalComent));

                    int iD2FactorLimitanteFinalComent = dr.GetOrdinal("d2_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD2FactorLimitanteFinalComent)) entity.D2FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD2FactorLimitanteFinalComent));

                    int iD3FactorLimitanteFinalComent = dr.GetOrdinal("d3_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD3FactorLimitanteFinalComent)) entity.D3FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD3FactorLimitanteFinalComent));

                    int iD4FactorLimitanteFinalComent = dr.GetOrdinal("d4_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD4FactorLimitanteFinalComent)) entity.D4FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD4FactorLimitanteFinalComent));

                    int iD1Tension = dr.GetOrdinal("d1_tension");
                    if (!dr.IsDBNull(iD1Tension)) entity.D1Tension = Convert.ToString(dr.GetValue(iD1Tension));

                    int iD2Tension = dr.GetOrdinal("d2_tension");
                    if (!dr.IsDBNull(iD2Tension)) entity.D2Tension = Convert.ToString(dr.GetValue(iD2Tension));

                    int iD3Tension = dr.GetOrdinal("d3_tension");
                    if (!dr.IsDBNull(iD3Tension)) entity.D3Tension = Convert.ToString(dr.GetValue(iD3Tension));

                    int iD4Tension = dr.GetOrdinal("d4_tension");
                    if (!dr.IsDBNull(iD4Tension)) entity.D4Tension = Convert.ToString(dr.GetValue(iD4Tension));

                    int iD1TensionComent = dr.GetOrdinal("d1_tension_coment");
                    if (!dr.IsDBNull(iD1TensionComent)) entity.D1TensionComent = Convert.ToString(dr.GetValue(iD1TensionComent));

                    int iD2TensionComent = dr.GetOrdinal("d2_tension_coment");
                    if (!dr.IsDBNull(iD2TensionComent)) entity.D2TensionComent = Convert.ToString(dr.GetValue(iD2TensionComent));

                    int iD3TensionComent = dr.GetOrdinal("d3_tension_coment");
                    if (!dr.IsDBNull(iD3TensionComent)) entity.D3TensionComent = Convert.ToString(dr.GetValue(iD3TensionComent));

                    int iD4TensionComent = dr.GetOrdinal("d4_tension_coment");
                    if (!dr.IsDBNull(iD4TensionComent)) entity.D4TensionComent = Convert.ToString(dr.GetValue(iD4TensionComent));

                    int iCantidadFilas = dr.GetOrdinal("cantidad_filas");
                    if (!dr.IsDBNull(iCantidadFilas)) entity.CantidadFilas = Convert.ToInt32(dr.GetValue(iCantidadFilas));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerTransformadorPorId(int codigoId)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTransformadorPorId);
            dbProvider.AddInParameter(command, "v_equicodi", DbType.Int32, codigoId);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iIdProyecto = dr.GetOrdinal("id_proyecto");
                    if (!dr.IsDBNull(iIdProyecto)) entity.IdProyecto = Convert.ToInt32(dr.GetValue(iIdProyecto));

                    int iProyecto = dr.GetOrdinal("proyecto");
                    if (!dr.IsDBNull(iProyecto)) entity.Proyecto = Convert.ToString(dr.GetValue(iProyecto));

                    int iFechaActualizacion = dr.GetOrdinal("fecha_actualizacion");
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.FechaActualizacion = Convert.ToString(dr.GetValue(iFechaActualizacion));

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iIdUbicacion = dr.GetOrdinal("id_ubicacion");
                    if (!dr.IsDBNull(iIdUbicacion)) entity.IdUbicacion = Convert.ToInt32(dr.GetValue(iIdUbicacion));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iIdEmpresa = dr.GetOrdinal("id_empresa");
                    if (!dr.IsDBNull(iIdEmpresa)) entity.IdEmpresa = Convert.ToString(dr.GetValue(iIdEmpresa));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));
                    

                    int iD1IdCelda = dr.GetOrdinal("d1_id_celda");
                    if (!dr.IsDBNull(iD1IdCelda)) entity.D1IdCelda = Convert.ToString(dr.GetValue(iD1IdCelda));

                    int iD2IdCelda = dr.GetOrdinal("d2_id_celda");
                    if (!dr.IsDBNull(iD2IdCelda)) entity.D2IdCelda = Convert.ToString(dr.GetValue(iD2IdCelda));

                    int iD3IdCelda = dr.GetOrdinal("d3_id_celda");
                    if (!dr.IsDBNull(iD3IdCelda)) entity.D3IdCelda = Convert.ToString(dr.GetValue(iD3IdCelda));

                    int iD4IdCelda = dr.GetOrdinal("d4_id_celda");
                    if (!dr.IsDBNull(iD4IdCelda)) entity.D4IdCelda = Convert.ToString(dr.GetValue(iD4IdCelda));

                    int iD1Tension = dr.GetOrdinal("d1_tension");
                    if (!dr.IsDBNull(iD1Tension)) entity.D1Tension = Convert.ToString(dr.GetValue(iD1Tension));

                    int iD2Tension = dr.GetOrdinal("d2_tension");
                    if (!dr.IsDBNull(iD2Tension)) entity.D2Tension = Convert.ToString(dr.GetValue(iD2Tension));

                    int iD3Tension = dr.GetOrdinal("d3_tension");
                    if (!dr.IsDBNull(iD3Tension)) entity.D3Tension = Convert.ToString(dr.GetValue(iD3Tension));

                    int iD4Tension = dr.GetOrdinal("d4_tension");
                    if (!dr.IsDBNull(iD4Tension)) entity.D4Tension = Convert.ToString(dr.GetValue(iD4Tension));

                    int iD1CapacidadOnanMva = dr.GetOrdinal("d1_capacidad_onan_mva");
                    if (!dr.IsDBNull(iD1CapacidadOnanMva)) entity.D1CapacidadOnanMva = Convert.ToString(dr.GetValue(iD1CapacidadOnanMva));

                    int iD2CapacidadOnanMva = dr.GetOrdinal("d2_capacidad_onan_mva");
                    if (!dr.IsDBNull(iD2CapacidadOnanMva)) entity.D2CapacidadOnanMva = Convert.ToString(dr.GetValue(iD2CapacidadOnanMva));

                    int iD3CapacidadOnanMva = dr.GetOrdinal("d3_capacidad_onan_mva");
                    if (!dr.IsDBNull(iD3CapacidadOnanMva)) entity.D3CapacidadOnanMva = Convert.ToString(dr.GetValue(iD3CapacidadOnanMva));

                    int iD4CapacidadOnanMva = dr.GetOrdinal("d4_capacidad_onan_mva");
                    if (!dr.IsDBNull(iD4CapacidadOnanMva)) entity.D4CapacidadOnanMva = Convert.ToString(dr.GetValue(iD4CapacidadOnanMva));

                    int iD1CapacidadOnanMvaComent = dr.GetOrdinal("d1_capacidad_onan_mva_coment");
                    if (!dr.IsDBNull(iD1CapacidadOnanMvaComent)) entity.D1CapacidadOnanMvaComent = Convert.ToString(dr.GetValue(iD1CapacidadOnanMvaComent));

                    int iD2CapacidadOnanMvaComent = dr.GetOrdinal("d2_capacidad_onan_mva_coment");
                    if (!dr.IsDBNull(iD2CapacidadOnanMvaComent)) entity.D2CapacidadOnanMvaComent = Convert.ToString(dr.GetValue(iD2CapacidadOnanMvaComent));

                    int iD3CapacidadOnanMvaComent = dr.GetOrdinal("d3_capacidad_onan_mva_coment");
                    if (!dr.IsDBNull(iD3CapacidadOnanMvaComent)) entity.D3CapacidadOnanMvaComent = Convert.ToString(dr.GetValue(iD3CapacidadOnanMvaComent));

                    int iD4CapacidadOnanMvaComent = dr.GetOrdinal("d4_capacidad_onan_mva_coment");
                    if (!dr.IsDBNull(iD4CapacidadOnanMvaComent)) entity.D4CapacidadOnanMvaComent = Convert.ToString(dr.GetValue(iD4CapacidadOnanMvaComent));

                    int iD1CapacidadOnafMva = dr.GetOrdinal("d1_capacidad_onaf_mva");
                    if (!dr.IsDBNull(iD1CapacidadOnafMva)) entity.D1CapacidadOnafMva = Convert.ToString(dr.GetValue(iD1CapacidadOnafMva));

                    int iD2CapacidadOnafMva = dr.GetOrdinal("d2_capacidad_onaf_mva");
                    if (!dr.IsDBNull(iD2CapacidadOnafMva)) entity.D2CapacidadOnafMva = Convert.ToString(dr.GetValue(iD2CapacidadOnafMva));

                    int iD3CapacidadOnafMva = dr.GetOrdinal("d3_capacidad_onaf_mva");
                    if (!dr.IsDBNull(iD3CapacidadOnafMva)) entity.D3CapacidadOnafMva = Convert.ToString(dr.GetValue(iD3CapacidadOnafMva));

                    int iD4CapacidadOnafMva = dr.GetOrdinal("d4_capacidad_onaf_mva");
                    if (!dr.IsDBNull(iD4CapacidadOnafMva)) entity.D4CapacidadOnafMva = Convert.ToString(dr.GetValue(iD4CapacidadOnafMva));

                    int iD1CapacidadOnafMvaComent = dr.GetOrdinal("d1_capacidad_onaf_mva_coment");
                    if (!dr.IsDBNull(iD1CapacidadOnafMvaComent)) entity.D1CapacidadOnafMvaComent = Convert.ToString(dr.GetValue(iD1CapacidadOnafMvaComent));

                    int iD2CapacidadOnafMvaComent = dr.GetOrdinal("d2_capacidad_onaf_mva_coment");
                    if (!dr.IsDBNull(iD2CapacidadOnafMvaComent)) entity.D2CapacidadOnafMvaComent = Convert.ToString(dr.GetValue(iD2CapacidadOnafMvaComent));

                    int iD3CapacidadOnafMvaComent = dr.GetOrdinal("d3_capacidad_onaf_mva_coment");
                    if (!dr.IsDBNull(iD3CapacidadOnafMvaComent)) entity.D3CapacidadOnafMvaComent = Convert.ToString(dr.GetValue(iD3CapacidadOnafMvaComent));

                    int iD4CapacidadOnafMvaComent = dr.GetOrdinal("d4_capacidad_onaf_mva_coment");
                    if (!dr.IsDBNull(iD4CapacidadOnafMvaComent)) entity.D4CapacidadOnafMvaComent = Convert.ToString(dr.GetValue(iD4CapacidadOnafMvaComent));

                    int iD1CapacidadMva = dr.GetOrdinal("d1_capacidad_mva");
                    if (!dr.IsDBNull(iD1CapacidadMva)) entity.D1CapacidadMva = Convert.ToString(dr.GetValue(iD1CapacidadMva));

                    int iD2CapacidadMva = dr.GetOrdinal("d2_capacidad_mva");
                    if (!dr.IsDBNull(iD2CapacidadMva)) entity.D2CapacidadMva = Convert.ToString(dr.GetValue(iD2CapacidadMva));

                    int iD3CapacidadMva = dr.GetOrdinal("d3_capacidad_mva");
                    if (!dr.IsDBNull(iD3CapacidadMva)) entity.D3CapacidadMva = Convert.ToString(dr.GetValue(iD3CapacidadMva));

                    int iD4CapacidadMva = dr.GetOrdinal("d4_capacidad_mva");
                    if (!dr.IsDBNull(iD4CapacidadMva)) entity.D4CapacidadMva = Convert.ToString(dr.GetValue(iD4CapacidadMva));

                    int iD1CapacidadMvaComent = dr.GetOrdinal("d1_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD1CapacidadMvaComent)) entity.D1CapacidadMvaComent = Convert.ToString(dr.GetValue(iD1CapacidadMvaComent));

                    int iD2CapacidadMvaComent = dr.GetOrdinal("d2_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD2CapacidadMvaComent)) entity.D2CapacidadMvaComent = Convert.ToString(dr.GetValue(iD2CapacidadMvaComent));

                    int iD3CapacidadMvaComent = dr.GetOrdinal("d3_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD3CapacidadMvaComent)) entity.D3CapacidadMvaComent = Convert.ToString(dr.GetValue(iD3CapacidadMvaComent));

                    int iD4CapacidadMvaComent = dr.GetOrdinal("d4_capacidad_mva_coment");
                    if (!dr.IsDBNull(iD4CapacidadMvaComent)) entity.D4CapacidadMvaComent = Convert.ToString(dr.GetValue(iD4CapacidadMvaComent));

                    int iD1CapacidadA = dr.GetOrdinal("d1_capacidad_a");
                    if (!dr.IsDBNull(iD1CapacidadA)) entity.D1CapacidadA = Convert.ToString(dr.GetValue(iD1CapacidadA));

                    int iD2CapacidadA = dr.GetOrdinal("d2_capacidad_a");
                    if (!dr.IsDBNull(iD2CapacidadA)) entity.D2CapacidadA = Convert.ToString(dr.GetValue(iD2CapacidadA));

                    int iD3CapacidadA = dr.GetOrdinal("d3_capacidad_a");
                    if (!dr.IsDBNull(iD3CapacidadA)) entity.D3CapacidadA = Convert.ToString(dr.GetValue(iD3CapacidadA));

                    int iD4CapacidadA = dr.GetOrdinal("d4_capacidad_a");
                    if (!dr.IsDBNull(iD4CapacidadA)) entity.D4CapacidadA = Convert.ToString(dr.GetValue(iD4CapacidadA));

                    int iD1CapacidadAComent = dr.GetOrdinal("d1_capacidad_a_coment");
                    if (!dr.IsDBNull(iD1CapacidadAComent)) entity.D1CapacidadAComent = Convert.ToString(dr.GetValue(iD1CapacidadAComent));

                    int iD2CapacidadAComent = dr.GetOrdinal("d2_capacidad_a_coment");
                    if (!dr.IsDBNull(iD2CapacidadAComent)) entity.D2CapacidadAComent = Convert.ToString(dr.GetValue(iD2CapacidadAComent));

                    int iD3CapacidadAComent = dr.GetOrdinal("d3_capacidad_a_coment");
                    if (!dr.IsDBNull(iD3CapacidadAComent)) entity.D3CapacidadAComent = Convert.ToString(dr.GetValue(iD3CapacidadAComent));

                    int iD4CapacidadAComent = dr.GetOrdinal("d4_capacidad_a_coment");
                    if (!dr.IsDBNull(iD4CapacidadAComent)) entity.D4CapacidadAComent = Convert.ToString(dr.GetValue(iD4CapacidadAComent));

                    int iD1CapacidadTransmisionMva = dr.GetOrdinal("d1_capacidad_transmision_mva");
                    if (!dr.IsDBNull(iD1CapacidadTransmisionMva)) entity.D1CapacidadTransmisionMva = Convert.ToString(dr.GetValue(iD1CapacidadTransmisionMva));

                    int iD2CapacidadTransmisionMva = dr.GetOrdinal("d2_capacidad_transmision_mva");
                    if (!dr.IsDBNull(iD2CapacidadTransmisionMva)) entity.D2CapacidadTransmisionMva = Convert.ToString(dr.GetValue(iD2CapacidadTransmisionMva));

                    int iD3CapacidadTransmisionMva = dr.GetOrdinal("d3_capacidad_transmision_mva");
                    if (!dr.IsDBNull(iD3CapacidadTransmisionMva)) entity.D3CapacidadTransmisionMva = Convert.ToString(dr.GetValue(iD3CapacidadTransmisionMva));

                    int iD4CapacidadTransmisionMva = dr.GetOrdinal("d4_capacidad_transmision_mva");
                    if (!dr.IsDBNull(iD4CapacidadTransmisionMva)) entity.D4CapacidadTransmisionMva = Convert.ToString(dr.GetValue(iD4CapacidadTransmisionMva));

                    int iD1CapacidadTransmisionMvaComent = dr.GetOrdinal("d1_capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iD1CapacidadTransmisionMvaComent)) entity.D1CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iD1CapacidadTransmisionMvaComent));

                    int iD2CapacidadTransmisionMvaComent = dr.GetOrdinal("d2_capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iD2CapacidadTransmisionMvaComent)) entity.D2CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iD2CapacidadTransmisionMvaComent));

                    int iD3CapacidadTransmisionMvaComent = dr.GetOrdinal("d3_capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iD3CapacidadTransmisionMvaComent)) entity.D3CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iD3CapacidadTransmisionMvaComent));

                    int iD4CapacidadTransmisionMvaComent = dr.GetOrdinal("d4_capacidad_transmision_mva_coment");
                    if (!dr.IsDBNull(iD4CapacidadTransmisionMvaComent)) entity.D4CapacidadTransmisionMvaComent = Convert.ToString(dr.GetValue(iD4CapacidadTransmisionMvaComent));

                    int iD1CapacidadTransmisionA = dr.GetOrdinal("d1_capacidad_transmision_a");
                    if (!dr.IsDBNull(iD1CapacidadTransmisionA)) entity.D1CapacidadTransmisionA = Convert.ToString(dr.GetValue(iD1CapacidadTransmisionA));

                    int iD2CapacidadTransmisionA = dr.GetOrdinal("d2_capacidad_transmision_a");
                    if (!dr.IsDBNull(iD2CapacidadTransmisionA)) entity.D2CapacidadTransmisionA = Convert.ToString(dr.GetValue(iD2CapacidadTransmisionA));

                    int iD3CapacidadTransmisionA = dr.GetOrdinal("d3_capacidad_transmision_a");
                    if (!dr.IsDBNull(iD3CapacidadTransmisionA)) entity.D3CapacidadTransmisionA = Convert.ToString(dr.GetValue(iD3CapacidadTransmisionA));

                    int iD4CapacidadTransmisionA = dr.GetOrdinal("d4_capacidad_transmision_a");
                    if (!dr.IsDBNull(iD4CapacidadTransmisionA)) entity.D4CapacidadTransmisionA = Convert.ToString(dr.GetValue(iD4CapacidadTransmisionA));

                    int iD1CapacidadTransmisionAComent = dr.GetOrdinal("d1_capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iD1CapacidadTransmisionAComent)) entity.D1CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iD1CapacidadTransmisionAComent));

                    int iD2CapacidadTransmisionAComent = dr.GetOrdinal("d2_capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iD2CapacidadTransmisionAComent)) entity.D2CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iD2CapacidadTransmisionAComent));

                    int iD3CapacidadTransmisionAComent = dr.GetOrdinal("d3_capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iD3CapacidadTransmisionAComent)) entity.D3CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iD3CapacidadTransmisionAComent));

                    int iD4CapacidadTransmisionAComent = dr.GetOrdinal("d4_capacidad_transmision_a_coment");
                    if (!dr.IsDBNull(iD4CapacidadTransmisionAComent)) entity.D4CapacidadTransmisionAComent = Convert.ToString(dr.GetValue(iD4CapacidadTransmisionAComent));

                    int iD1FactorLimitanteCalc = dr.GetOrdinal("d1_factor_limitante_calc");
                    if (!dr.IsDBNull(iD1FactorLimitanteCalc)) entity.D1FactorLimitanteCalc = Convert.ToString(dr.GetValue(iD1FactorLimitanteCalc));

                    int iD2FactorLimitanteCalc = dr.GetOrdinal("d2_factor_limitante_calc");
                    if (!dr.IsDBNull(iD2FactorLimitanteCalc)) entity.D2FactorLimitanteCalc = Convert.ToString(dr.GetValue(iD2FactorLimitanteCalc));

                    int iD3FactorLimitanteCalc = dr.GetOrdinal("d3_factor_limitante_calc");
                    if (!dr.IsDBNull(iD3FactorLimitanteCalc)) entity.D3FactorLimitanteCalc = Convert.ToString(dr.GetValue(iD3FactorLimitanteCalc));

                    int iD4FactorLimitanteCalc = dr.GetOrdinal("d4_factor_limitante_calc");
                    if (!dr.IsDBNull(iD4FactorLimitanteCalc)) entity.D4FactorLimitanteCalc = Convert.ToString(dr.GetValue(iD4FactorLimitanteCalc));

                    int iD1FactorLimitanteCalcComent = dr.GetOrdinal("d1_factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iD1FactorLimitanteCalcComent)) entity.D1FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iD1FactorLimitanteCalcComent));

                    int iD2FactorLimitanteCalcComent = dr.GetOrdinal("d2_factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iD2FactorLimitanteCalcComent)) entity.D2FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iD2FactorLimitanteCalcComent));

                    int iD3FactorLimitanteCalcComent = dr.GetOrdinal("d3_factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iD3FactorLimitanteCalcComent)) entity.D3FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iD3FactorLimitanteCalcComent));

                    int iD4FactorLimitanteCalcComent = dr.GetOrdinal("d4_factor_limitante_calc_coment");
                    if (!dr.IsDBNull(iD4FactorLimitanteCalcComent)) entity.D4FactorLimitanteCalcComent = Convert.ToString(dr.GetValue(iD4FactorLimitanteCalcComent));

                    int iD1FactorLimitanteFinal = dr.GetOrdinal("d1_factor_limitante_final");
                    if (!dr.IsDBNull(iD1FactorLimitanteFinal)) entity.D1FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD1FactorLimitanteFinal));

                    int iD2FactorLimitanteFinal = dr.GetOrdinal("d2_factor_limitante_final");
                    if (!dr.IsDBNull(iD2FactorLimitanteFinal)) entity.D2FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD2FactorLimitanteFinal));

                    int iD3FactorLimitanteFinal = dr.GetOrdinal("d3_factor_limitante_final");
                    if (!dr.IsDBNull(iD3FactorLimitanteFinal)) entity.D3FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD3FactorLimitanteFinal));

                    int iD4FactorLimitanteFinal = dr.GetOrdinal("d4_factor_limitante_final");
                    if (!dr.IsDBNull(iD4FactorLimitanteFinal)) entity.D4FactorLimitanteFinal = Convert.ToString(dr.GetValue(iD4FactorLimitanteFinal));

                    int iD1FactorLimitanteFinalComent = dr.GetOrdinal("d1_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD1FactorLimitanteFinalComent)) entity.D1FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD1FactorLimitanteFinalComent));

                    int iD2FactorLimitanteFinalComent = dr.GetOrdinal("d2_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD2FactorLimitanteFinalComent)) entity.D2FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD2FactorLimitanteFinalComent));

                    int iD3FactorLimitanteFinalComent = dr.GetOrdinal("d3_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD3FactorLimitanteFinalComent)) entity.D3FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD3FactorLimitanteFinalComent));

                    int iD4FactorLimitanteFinalComent = dr.GetOrdinal("d4_factor_limitante_final_coment");
                    if (!dr.IsDBNull(iD4FactorLimitanteFinalComent)) entity.D4FactorLimitanteFinalComent = Convert.ToString(dr.GetValue(iD4FactorLimitanteFinalComent));

                    int iObservaciones = dr.GetOrdinal("observaciones");
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iUsuarioModificacion = dr.GetOrdinal("usumodificacion");
                    if (!dr.IsDBNull(iUsuarioModificacion)) entity.UsuarioModificacion = Convert.ToString(dr.GetValue(iUsuarioModificacion));

                    int iFechaModificacion = dr.GetOrdinal("fecmodificacion");
                    if (!dr.IsDBNull(iFechaModificacion)) entity.FechaModificacion = Convert.ToString(dr.GetValue(iFechaModificacion));

                    int iD1Pickup = dr.GetOrdinal("d1_pickup");
                    if (!dr.IsDBNull(iD1Pickup)) entity.D1PickUp = Convert.ToString(dr.GetValue(iD1Pickup));

                    int iD1PosicionTCA = dr.GetOrdinal("d1_posicionTca");
                    if (!dr.IsDBNull(iD1PosicionTCA)) entity.D1PosicionTcA = Convert.ToString(dr.GetValue(iD1PosicionTCA));

                    int iD2Pickup = dr.GetOrdinal("d2_pickup");
                    if (!dr.IsDBNull(iD2Pickup)) entity.D2PickUp = Convert.ToString(dr.GetValue(iD2Pickup));

                    int iD2PosicionTCA = dr.GetOrdinal("d2_posicionTca");
                    if (!dr.IsDBNull(iD2PosicionTCA)) entity.D2PosicionTcA = Convert.ToString(dr.GetValue(iD2PosicionTCA));

                    int iD3Pickup = dr.GetOrdinal("d3_pickup");
                    if (!dr.IsDBNull(iD3Pickup)) entity.D3PickUp = Convert.ToString(dr.GetValue(iD3Pickup));

                    int iD3PosicionTcA = dr.GetOrdinal("d3_posicionTca");
                    if (!dr.IsDBNull(iD3PosicionTcA)) entity.D3PosicionTcA = Convert.ToString(dr.GetValue(iD3PosicionTcA));

                    int iD4Pickup = dr.GetOrdinal("d4_pickup");
                    if (!dr.IsDBNull(iD4Pickup)) entity.D4PickUp = Convert.ToString(dr.GetValue(iD4Pickup));

                    int iD4PosicionTcA = dr.GetOrdinal("d4_posicionTca");
                    if (!dr.IsDBNull(iD4PosicionTcA)) entity.D4PosicionTcA = Convert.ToString(dr.GetValue(iD4PosicionTcA));

                }
            }
            return entity;
        }

        public string RegistrarCeldaAcoplamiento(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRegistrarCeldaAcoplamiento);
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.Int32, equipo.IdCelda);
            dbProvider.AddInParameter(command, helper.IdProyecto, DbType.Int32, equipo.IdProyecto);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.String, equipo.Fecha);
            dbProvider.AddInParameter(command, helper.IdInterruptor, DbType.String, equipo.IdInterruptor);
            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, helper.CapacidadAComent, DbType.String, equipo.CapacidadAComent);
            dbProvider.AddInParameter(command, helper.CapacidadMvar, DbType.String, equipo.CapacidadMvar);
            dbProvider.AddInParameter(command, helper.CapacidadMvarComent, DbType.String, equipo.CapacidadMvarComent);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionAComent, DbType.String, equipo.CapacidadTransmisionAComent);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvar, DbType.String, equipo.CapacidadTransmisionMvar);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvarComent, DbType.String, equipo.CapacidadTransmisionMvarComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalcComent, DbType.String, equipo.FactorLimitanteCalcComent);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinalComent, DbType.String, equipo.FactorLimitanteFinalComent);
            dbProvider.AddInParameter(command, helper.Observaciones, DbType.String, equipo.Observaciones);
            dbProvider.AddInParameter(command, helper.UsuarioAuditoria, DbType.String, equipo.UsuarioAuditoria);
            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }

        public EprEquipoDTO ObtenerEquipoPorId(string id)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEquipoPorId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEmprCodi = dr.GetOrdinal("emprcodi");
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToString(dr.GetValue(iEmprCodi));

                    int iGrupoCodi = dr.GetOrdinal("grupocodi");
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = Convert.ToString(dr.GetValue(iGrupoCodi));

                    int iEleCodi = dr.GetOrdinal("elecodi");
                    if (!dr.IsDBNull(iEleCodi)) entity.EleCodi = Convert.ToString(dr.GetValue(iEleCodi));

                    int iAreacodigo = dr.GetOrdinal("areacodi");
                    if (!dr.IsDBNull(iAreacodigo)) entity.Areacodigo = Convert.ToString(dr.GetValue(iAreacodigo));

                    int iFamCodigo = dr.GetOrdinal("famcodi");
                    if (!dr.IsDBNull(iFamCodigo)) entity.FamCodigo = Convert.ToString(dr.GetValue(iFamCodigo));

                    int iIdUbicacion = dr.GetOrdinal("equiabrev");
                    if (!dr.IsDBNull(iIdUbicacion)) entity.EquiAbrev = Convert.ToString(dr.GetValue(iIdUbicacion));

                    int iEquiNomb = dr.GetOrdinal("equinomb");
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = Convert.ToString(dr.GetValue(iEquiNomb));

                    int iEquiAbrev2 = dr.GetOrdinal("equiabrev2");
                    if (!dr.IsDBNull(iEquiAbrev2)) entity.EquiAbrev2 = Convert.ToString(dr.GetValue(iEquiAbrev2));

                    int iEquiTension = dr.GetOrdinal("equitension");
                    if (!dr.IsDBNull(iEquiTension)) entity.EquiTension = Convert.ToString(dr.GetValue(iEquiTension));

                    int iEquiPadre = dr.GetOrdinal("equipadre");
                    if (!dr.IsDBNull(iEquiPadre)) entity.EquiPadre = Convert.ToString(dr.GetValue(iEquiPadre));

                    int iEquiPot = dr.GetOrdinal("equipot");
                    if (!dr.IsDBNull(iEquiPot)) entity.EquiPot = Convert.ToString(dr.GetValue(iEquiPot));

                    int iLastUser = dr.GetOrdinal("lastuser");
                    if (!dr.IsDBNull(iLastUser)) entity.LastUser = Convert.ToString(dr.GetValue(iLastUser));

                    int iLastDate = dr.GetOrdinal("lastdate");
                    if (!dr.IsDBNull(iLastDate)) entity.LastDate = Convert.ToString(dr.GetValue(iLastDate));

                    int iECodigo = dr.GetOrdinal("ecodigo");
                    if (!dr.IsDBNull(iECodigo)) entity.ECodigo = Convert.ToString(dr.GetValue(iECodigo));
                    
                    int iOsinergCodi = dr.GetOrdinal("osinergcodi");
                    if (!dr.IsDBNull(iOsinergCodi)) entity.OsinergCodi = Convert.ToString(dr.GetValue(iOsinergCodi));

                    int iOsinergCodigen = dr.GetOrdinal("osinergcodigen");
                    if (!dr.IsDBNull(iOsinergCodigen)) entity.OsinergCodigen = Convert.ToString(dr.GetValue(iOsinergCodigen));

                    int iOperadorEmprcodi = dr.GetOrdinal("operadoremprcodi");
                    if (!dr.IsDBNull(iOperadorEmprcodi)) entity.OperadorEmprcodi = Convert.ToString(dr.GetValue(iOperadorEmprcodi));

                    int iLastCodi = dr.GetOrdinal("lastcodi");
                    if (!dr.IsDBNull(iLastCodi)) entity.LastCodi = Convert.ToString(dr.GetValue(iLastCodi));

                    int iEquiFechiniopcom = dr.GetOrdinal("equifechiniopcom");
                    if (!dr.IsDBNull(iEquiFechiniopcom)) entity.EquiFechiniopcom = Convert.ToString(dr.GetValue(iEquiFechiniopcom));

                    int iEquiFechfinopcom = dr.GetOrdinal("equifechfinopcom");
                    if (!dr.IsDBNull(iEquiFechfinopcom)) entity.EquiFechfinopcom = Convert.ToString(dr.GetValue(iEquiFechfinopcom));

                    int iEmprNomb = dr.GetOrdinal("emprnomb");
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iFamNomb = dr.GetOrdinal("famnomb");
                    if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = Convert.ToString(dr.GetValue(iFamNomb));

                    int iFamAbrev = dr.GetOrdinal("famabrev");
                    if (!dr.IsDBNull(iFamAbrev)) entity.FamAbrev = Convert.ToString(dr.GetValue(iFamAbrev));

                    int iAreaNomb = dr.GetOrdinal("areanomb");
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iTareaAbrev = dr.GetOrdinal("tareaabrev");
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TareaAbrev = Convert.ToString(dr.GetValue(iTareaAbrev));

                    int iUsuarioUpdate = dr.GetOrdinal("usuarioupdate");
                    if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = Convert.ToString(dr.GetValue(iUsuarioUpdate));

                    int iFechaUpdate = dr.GetOrdinal("fechaupdate");
                    if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = Convert.ToString(dr.GetValue(iFechaUpdate));

                    int iEquiManiobr = dr.GetOrdinal("equimaniobra");
                    if (!dr.IsDBNull(iEquiManiobr)) entity.EquiManiobr = Convert.ToString(dr.GetValue(iEquiManiobr));

                    int iEquiEstado = dr.GetOrdinal("equiestado");
                    if (!dr.IsDBNull(iEquiEstado)) entity.EquiEstado = Convert.ToString(dr.GetValue(iEquiEstado));

                }
            }
            return entity;
        }

        public List<EprEquipoDTO> ListaInterruptorPorAreacodi(string id)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListaInterruptorPorAreacodi);
            dbProvider.AddInParameter(command, "areacodi", DbType.String, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();
                    
                    
                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal("equinomb");
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iAreaNomb = dr.GetOrdinal("areanomb");
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iEmprNomb = dr.GetOrdinal("emprnomb");
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iPosicionNucleoTc = dr.GetOrdinal("posicion_nucleo_tc");
                    if (!dr.IsDBNull(iPosicionNucleoTc)) entity.PosicionNucleoTc = Convert.ToString(dr.GetValue(iPosicionNucleoTc));

                    int iPickUp = dr.GetOrdinal("pick_up");
                    if (!dr.IsDBNull(iPickUp)) entity.PickUp = Convert.ToString(dr.GetValue(iPickUp));

                    int iTension = dr.GetOrdinal("tension");
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iCapacidadA = dr.GetOrdinal("capacidada");
                    if (!dr.IsDBNull(iCapacidadA)) entity.CapacidadA = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadMva = dr.GetOrdinal("capacidadmva");
                    if (!dr.IsDBNull(iCapacidadMva)) entity.CapacidadMva = Convert.ToString(dr.GetValue(iCapacidadMva));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerCabeceraEquipoPorId(int id)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCabeceraEquipoPorId);
            dbProvider.AddInParameter(command, "equicodi", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal("equicodi");
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal("codigo");
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iUbicacion = dr.GetOrdinal("ubicacion");
                    if (!dr.IsDBNull(iUbicacion)) entity.Ubicacion = Convert.ToString(dr.GetValue(iUbicacion));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iTipo = dr.GetOrdinal("tipo");
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToString(dr.GetValue(iTipo));

                    int iFamCodigo = dr.GetOrdinal("famcodigo");
                    if (!dr.IsDBNull(iFamCodigo)) entity.FamCodigo = Convert.ToString(dr.GetValue(iFamCodigo));

                    int iTension = dr.GetOrdinal("tension");
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iAreacodi = dr.GetOrdinal("areacodi");
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodigo = Convert.ToString(dr.GetValue(iAreacodi));

                }
            }
            return entity;
        }
        public List<EprEquipoDTO> ListaReporteLimiteCapacidad(int revision, string descripcion)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReporteLimiteCapacidad);
            dbProvider.AddInParameter(command, "revision", DbType.Int32, revision);
            dbProvider.AddInParameter(command, "revision", DbType.Int32, revision);
            dbProvider.AddInParameter(command, "descripcion", DbType.String, descripcion);
            dbProvider.AddInParameter(command, "descripcion", DbType.String, descripcion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEqrtlcCodi = dr.GetOrdinal("EPRTLCCODI");
                    if (!dr.IsDBNull(iEqrtlcCodi)) entity.EprtlcCodi = Convert.ToString(dr.GetValue(iEqrtlcCodi));

                    int iEqrtlcRevision = dr.GetOrdinal("EPRTLCREVISION");
                    if (!dr.IsDBNull(iEqrtlcRevision)) entity.EprtlcRevision = Convert.ToString(dr.GetValue(iEqrtlcRevision));

                    int iEqrtlcDescripcion = dr.GetOrdinal("EPRTLCDESCRIPCION");
                    if (!dr.IsDBNull(iEqrtlcDescripcion)) entity.EprtlcDescripcion = Convert.ToString(dr.GetValue(iEqrtlcDescripcion));

                    int iEqrtlcFecemision = dr.GetOrdinal("EPRTLCFECEMISION");
                    if (!dr.IsDBNull(iEqrtlcFecemision)) entity.EprtlcFecemision = Convert.ToString(dr.GetValue(iEqrtlcFecemision));

                    int iEqrtlcUsuelabora = dr.GetOrdinal("EPRTLCUSUELABORA");
                    if (!dr.IsDBNull(iEqrtlcUsuelabora)) entity.EprtlcUsuelabora = Convert.ToString(dr.GetValue(iEqrtlcUsuelabora));

                    int iEqrtlcUsurevisa = dr.GetOrdinal("EPRTLCUSUREVISA");
                    if (!dr.IsDBNull(iEqrtlcUsurevisa)) entity.EprtlcUsurevisa = Convert.ToString(dr.GetValue(iEqrtlcUsurevisa));

                    int iEqrtlcUsuaprueba = dr.GetOrdinal("EPRTLCUSUAPRUEBA");
                    if (!dr.IsDBNull(iEqrtlcUsuaprueba)) entity.EprtlcUsuaprueba = Convert.ToString(dr.GetValue(iEqrtlcUsuaprueba));

                    int iEqrtlcEstregistro = dr.GetOrdinal("EPRTLCESTREGISTRO");
                    if (!dr.IsDBNull(iEqrtlcEstregistro)) entity.EprtlcEstregistro = Convert.ToString(dr.GetValue(iEqrtlcEstregistro));

                    int iEqrtlcUsucreacion = dr.GetOrdinal("EPRTLCUSUCREACION");
                    if (!dr.IsDBNull(iEqrtlcUsucreacion)) entity.EprtlcUsucreacion = Convert.ToString(dr.GetValue(iEqrtlcUsucreacion));

                    int iEqrtlcFeccreacion = dr.GetOrdinal("EPRTLCFECCREACION");
                    if (!dr.IsDBNull(iEqrtlcFeccreacion)) entity.EprtlcFeccreacion = Convert.ToString(dr.GetValue(iEqrtlcFeccreacion));

                    int iEqrtlcUsumodificacion = dr.GetOrdinal("EPRTLCUSUMODIFICACION");
                    if (!dr.IsDBNull(iEqrtlcUsumodificacion)) entity.EprtlcUsumodificacion = Convert.ToString(dr.GetValue(iEqrtlcUsumodificacion));

                    int iEqrtlcFecmodificacion = dr.GetOrdinal("EPRTLCFECMODIFICACION");
                    if (!dr.IsDBNull(iEqrtlcFecmodificacion)) entity.EprtlcFecmodificacion = Convert.ToString(dr.GetValue(iEqrtlcFecmodificacion));

                    int iEprtlcNoarchivo = dr.GetOrdinal("EPRTLCNOARCHIVO");
                    if (!dr.IsDBNull(iEprtlcNoarchivo)) entity.EprtlcNoarchivo = Convert.ToString(dr.GetValue(iEprtlcNoarchivo));
                    
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprEquipoDTO ObtenerReporteLimiteCapacidadPorId(int id)
        {

            EprEquipoDTO entity = new EprEquipoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerReporteLimiteCapacidadPorId);
            dbProvider.AddInParameter(command, "codigo", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprEquipoDTO();

                    int iEqrtlcCodi = dr.GetOrdinal("EPRTLCCODI");
                    if (!dr.IsDBNull(iEqrtlcCodi)) entity.EprtlcCodi = Convert.ToString(dr.GetValue(iEqrtlcCodi));

                    int iEqrtlcRevision = dr.GetOrdinal("EPRTLCREVISION");
                    if (!dr.IsDBNull(iEqrtlcRevision)) entity.EprtlcRevision = Convert.ToString(dr.GetValue(iEqrtlcRevision));

                    int iEqrtlcDescripcion = dr.GetOrdinal("EPRTLCDESCRIPCION");
                    if (!dr.IsDBNull(iEqrtlcDescripcion)) entity.EprtlcDescripcion = Convert.ToString(dr.GetValue(iEqrtlcDescripcion));

                    int iEqrtlcFecemision = dr.GetOrdinal("EPRTLCFECEMISION");
                    if (!dr.IsDBNull(iEqrtlcFecemision)) entity.EprtlcFecemision = Convert.ToString(dr.GetValue(iEqrtlcFecemision));

                    int iEqrtlcUsuelabora = dr.GetOrdinal("EPRTLCUSUELABORA");
                    if (!dr.IsDBNull(iEqrtlcUsuelabora)) entity.EprtlcUsuelabora = Convert.ToString(dr.GetValue(iEqrtlcUsuelabora));

                    int iEqrtlcUsurevisa = dr.GetOrdinal("EPRTLCUSUREVISA");
                    if (!dr.IsDBNull(iEqrtlcUsurevisa)) entity.EprtlcUsurevisa = Convert.ToString(dr.GetValue(iEqrtlcUsurevisa));

                    int iEqrtlcUsuaprueba = dr.GetOrdinal("EPRTLCUSUAPRUEBA");
                    if (!dr.IsDBNull(iEqrtlcUsuaprueba)) entity.EprtlcUsuaprueba = Convert.ToString(dr.GetValue(iEqrtlcUsuaprueba));

                    int iEqrtlcEstregistro = dr.GetOrdinal("EPRTLCESTREGISTRO");
                    if (!dr.IsDBNull(iEqrtlcEstregistro)) entity.EprtlcEstregistro = Convert.ToString(dr.GetValue(iEqrtlcEstregistro));

                    int iEqrtlcUsucreacion = dr.GetOrdinal("EPRTLCUSUCREACION");
                    if (!dr.IsDBNull(iEqrtlcUsucreacion)) entity.EprtlcUsucreacion = Convert.ToString(dr.GetValue(iEqrtlcUsucreacion));

                    int iEqrtlcFeccreacion = dr.GetOrdinal("EPRTLCFECCREACION");
                    if (!dr.IsDBNull(iEqrtlcFeccreacion)) entity.EprtlcFeccreacion = Convert.ToString(dr.GetValue(iEqrtlcFeccreacion));

                    int iEqrtlcUsumodificacion = dr.GetOrdinal("EPRTLCUSUMODIFICACION");
                    if (!dr.IsDBNull(iEqrtlcUsumodificacion)) entity.EprtlcUsumodificacion = Convert.ToString(dr.GetValue(iEqrtlcUsumodificacion));

                    int iEqrtlcFecmodificacion = dr.GetOrdinal("EPRTLCFECMODIFICACION");
                    if (!dr.IsDBNull(iEqrtlcFecmodificacion)) entity.EprtlcFecmodificacion = Convert.ToString(dr.GetValue(iEqrtlcFecmodificacion));

                    int iEprtlcNoarchivo = dr.GetOrdinal("EPRTLCNOARCHIVO");
                    if (!dr.IsDBNull(iEprtlcNoarchivo)) entity.EprtlcNoarchivo = Convert.ToString(dr.GetValue(iEprtlcNoarchivo));
                }
            }
            return entity;
        }

        public int GuardarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerMaxIdReporteLimiteCapacidad);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlObtenerMaxRevision);
            object resultRev = dbProvider.ExecuteScalar(command);
            int rev = 1;
            if (resultRev != null) rev = Convert.ToInt32(resultRev);

            command = dbProvider.GetSqlStringCommand(helper.SqlGuardarReporteLimiteCapacidad);
            dbProvider.AddInParameter(command, "EprtlcCodi", DbType.Int32, id);
            dbProvider.AddInParameter(command, "EprtlcRevision", DbType.Int32, rev);
            dbProvider.AddInParameter(command, "EprtlcDescripcion", DbType.String, entity.EprtlcDescripcion);
            dbProvider.AddInParameter(command, "EprtlcFecemision", DbType.String, entity.EprtlcFecemision);
            dbProvider.AddInParameter(command, "EprtlcUsuelabora", DbType.String, entity.EprtlcUsuelabora);
            dbProvider.AddInParameter(command, "EprtlcUsurevisa", DbType.String, entity.EprtlcUsurevisa);
            dbProvider.AddInParameter(command, "EprtlcUsuaprueba", DbType.String, entity.EprtlcUsuaprueba);
            dbProvider.AddInParameter(command, "EprtlcEstregistro", DbType.String, entity.EprtlcEstregistro);
            dbProvider.AddInParameter(command, "EprtlcUsucreacion", DbType.String, entity.EprtlcUsucreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void ActualizarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarReporteLimiteCapacidad);
            dbProvider.AddInParameter(command, "EprtlcDescripcion", DbType.String, entity.EprtlcDescripcion);
            dbProvider.AddInParameter(command, "EprtlcFecemision", DbType.String, entity.EprtlcFecemision);
            dbProvider.AddInParameter(command, "EprtlcUsuelabora", DbType.String, entity.EprtlcUsuelabora);
            dbProvider.AddInParameter(command, "EprtlcUsurevisa", DbType.String, entity.EprtlcUsurevisa);
            dbProvider.AddInParameter(command, "EprtlcUsuaprueba", DbType.String, entity.EprtlcUsuaprueba);
            dbProvider.AddInParameter(command, "EprtlcUsumodificacion", DbType.String, entity.EprtlcUsumodificacion);
            dbProvider.AddInParameter(command, "EprtlcCodi", DbType.Int32, Convert.ToInt32(entity.EprtlcCodi));
            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarReporteLimiteCapacidad);
            dbProvider.AddInParameter(command, "EprtlcEstregistro", DbType.String, entity.EprtlcEstregistro);
            dbProvider.AddInParameter(command, "EprtlcUsumodificacion", DbType.String, entity.EprtlcUsumodificacion);
            dbProvider.AddInParameter(command, "EprtlcCodi", DbType.Int32, Convert.ToInt32(entity.EprtlcCodi));
            dbProvider.ExecuteNonQuery(command);
        }


        #region GESPROTECT - Exportacion Datos Reles

        public List<EprEquipoDTO> ListaExportarReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
          int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListReleSincronismoReporte);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iCodigoInterruptor = dr.GetOrdinal("codigo_interruptor");
                    if (!dr.IsDBNull(iCodigoInterruptor)) entity.CodigoInterruptor = Convert.ToString(dr.GetValue(iCodigoInterruptor));

                    int iDeltaTension = dr.GetOrdinal("delta_tension");
                    if (!dr.IsDBNull(iDeltaTension)) entity.DeltaTension = Convert.ToString(dr.GetValue(iDeltaTension));

                    int iDeltaAngulo = dr.GetOrdinal("delta_angulo");
                    if (!dr.IsDBNull(iDeltaAngulo)) entity.DeltaAngulo = Convert.ToString(dr.GetValue(iDeltaAngulo));

                    int iDeltaFrecuencia = dr.GetOrdinal("delta_frecuencia");
                    if (!dr.IsDBNull(iDeltaFrecuencia)) entity.DeltaFrecuencia = Convert.ToString(dr.GetValue(iDeltaFrecuencia));                    


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaExportarReleSobreTension(string codigoId, string codigo, int subestacion, int celda,
         int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListReleSobreTensionReporte);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal(helper.Celda);
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iNivelTension = dr.GetOrdinal("nivel_tension");
                    if (!dr.IsDBNull(iNivelTension)) entity.NivelTension = Convert.ToString(dr.GetValue(iNivelTension));

                    int iSobreTU = dr.GetOrdinal("sobre_t_u");
                    if (!dr.IsDBNull(iSobreTU)) entity.SobreTU = Convert.ToString(dr.GetValue(iSobreTU));

                    int iSobreTT = dr.GetOrdinal("sobre_t_t");
                    if (!dr.IsDBNull(iSobreTT)) entity.SobreTT = Convert.ToString(dr.GetValue(iSobreTT));

                    int iSobreTUU = dr.GetOrdinal("sobre_t_uu");
                    if (!dr.IsDBNull(iSobreTUU)) entity.SobreTUU = Convert.ToString(dr.GetValue(iSobreTUU));

                    int iSobreTTT = dr.GetOrdinal("sobre_t_tt");
                    if (!dr.IsDBNull(iSobreTTT)) entity.SobreTTT = Convert.ToString(dr.GetValue(iSobreTTT));                   

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaExportarReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
           int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListReleMandoSincronizadoReporte);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iMandoSincronizado = dr.GetOrdinal("mando_sincronizado");
                    if (!dr.IsDBNull(iMandoSincronizado)) entity.MandoSincronizado = Convert.ToString(dr.GetValue(iMandoSincronizado));
                  


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaExportarReleTorsional(string codigoId, string codigo, int subestacion, int celda,
           int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListReleTorsionalReporte);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iMedidaMitigacion = dr.GetOrdinal("medida_mitigacion");
                    if (!dr.IsDBNull(iMedidaMitigacion)) entity.MedidaMitigacion = Convert.ToString(dr.GetValue(iMedidaMitigacion));

                    int iReleTorsionalImplementadoDsc = dr.GetOrdinal("torsional_implementado");
                    if (!dr.IsDBNull(iReleTorsionalImplementadoDsc)) entity.ReleTorsionalImplementadoDsc = Convert.ToString(dr.GetValue(iReleTorsionalImplementadoDsc));

                    


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoDTO> ListaExportarRelePmu(string codigoId, string codigo, int subestacion, int celda,
           int empresa, int area, string estado)
        {

            List<EprEquipoDTO> entitys = new List<EprEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListRelePmuReporte);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "equicodi", DbType.String, codigoId);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "codigo", DbType.String, codigo);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "subestacioncodi", DbType.Int32, subestacion);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "equicodicelda", DbType.Int32, celda);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, empresa);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "areacodi", DbType.Int32, area);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);
            dbProvider.AddInParameter(command, "equiestado", DbType.String, estado);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoDTO entity = new EprEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    int iArea = dr.GetOrdinal("area");
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iEmpresa = dr.GetOrdinal("empresa");
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));

                    int iSubestacion = dr.GetOrdinal("subestacion");
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

                    int iCelda = dr.GetOrdinal("celda");
                    if (!dr.IsDBNull(iCelda)) entity.Celda = Convert.ToString(dr.GetValue(iCelda));

                    int iAccion = dr.GetOrdinal("accion");
                    if (!dr.IsDBNull(iAccion)) entity.Accion = Convert.ToString(dr.GetValue(iAccion));
                   
                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = Convert.ToString(dr.GetValue(iEstado));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion


        public string ExcluirEquipoProtecciones(EprEquipoDTO equipo)
        {
            string resultado = "";

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlExcluirEquipoProtecciones);

            dbProvider.AddInParameter(command, "ID_EQUIPO", DbType.Int32, equipo.Equicodi);
            dbProvider.AddInParameter(command, helper.UsuarioAuditoria, DbType.String, equipo.UsuarioAuditoria);

            dbProvider.AddOutParameter(command, helper.Resultado, DbType.String, 4000);

            dbProvider.ExecuteNonQuery(command);

            resultado = dbProvider.GetParameterValue(command, helper.Resultado) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Resultado);

            return resultado;

        }

        public void AgregarEliminarArchivoReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAgregarEliminarArchivoReporteLimiteCapacidad);
            dbProvider.AddInParameter(command, "EprtlcNoarchivo", DbType.String, entity.EprtlcNoarchivo);
            dbProvider.AddInParameter(command, "EprtlcUsumodificacion", DbType.String, entity.EprtlcUsumodificacion);
            dbProvider.AddInParameter(command, "EprtlcCodi", DbType.Int32, Convert.ToInt32(entity.EprtlcCodi));
            dbProvider.ExecuteNonQuery(command);
        }

        public EprEquipoDTO ObtenerDatoCelda(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDatoCelda);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            EprEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprEquipoDTO();
                    int iPosicionNucleoTc = dr.GetOrdinal(helper.PosicionNucleoTc);
                    if (!dr.IsDBNull(iPosicionNucleoTc)) entity.PosicionNucleoTc = Convert.ToString(dr.GetValue(iPosicionNucleoTc));

                    int iPickUp = dr.GetOrdinal(helper.PickUp);
                    if (!dr.IsDBNull(iPickUp)) entity.PickUp = Convert.ToString(dr.GetValue(iPickUp));

                }
            }

            return entity;
        }

        public string ObtenerFechaReportePorId(int id)
        {

            string fecha = "";
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerFechaReportePorId);
            dbProvider.AddInParameter(command, "codigo", DbType.Int32, id);
            dbProvider.AddInParameter(command, "codigo", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iEqrtlcFecemision = dr.GetOrdinal("EPRTLCFECEMISION");
                    if (!dr.IsDBNull(iEqrtlcFecemision)) fecha = Convert.ToString(dr.GetValue(iEqrtlcFecemision));
                }
            }
            return fecha;
        }

    }


}
