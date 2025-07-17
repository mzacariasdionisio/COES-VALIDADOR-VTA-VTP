using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_CAMBIO_TURNO_SUBSECCION
    /// </summary>
    public class SiCambioTurnoSubseccionRepository : RepositoryBase, ISiCambioTurnoSubseccionRepository
    {
        public SiCambioTurnoSubseccionRepository(string strConn)
            : base(strConn)
        {
        }

        SiCambioTurnoSubseccionHelper helper = new SiCambioTurnoSubseccionHelper();

        public int Save(SiCambioTurnoSubseccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Subseccioncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, entity.Seccioncodi);
            dbProvider.AddInParameter(command, helper.Subseccionnumber, DbType.Int32, entity.Subseccionnumber);
            dbProvider.AddInParameter(command, helper.Despcentromarginal, DbType.String, entity.Despcentromarginal);
            dbProvider.AddInParameter(command, helper.Despursautomatica, DbType.String, entity.Despursautomatica);
            dbProvider.AddInParameter(command, helper.Despmagautomatica, DbType.Decimal, entity.Despmagautomatica);
            dbProvider.AddInParameter(command, helper.Despursmanual, DbType.String, entity.Despursmanual);
            dbProvider.AddInParameter(command, helper.Despmagmanual, DbType.Decimal, entity.Despmagmanual);
            dbProvider.AddInParameter(command, helper.Despcentralaislado, DbType.String, entity.Despcentralaislado);
            dbProvider.AddInParameter(command, helper.Despmagaislado, DbType.Decimal, entity.Despmagaislado);
            dbProvider.AddInParameter(command, helper.Despreprogramas, DbType.String, entity.Despreprogramas);
            dbProvider.AddInParameter(command, helper.Desphorareprog, DbType.String, entity.Desphorareprog);
            dbProvider.AddInParameter(command, helper.Despmotivorepro, DbType.String, entity.Despmotivorepro);
            dbProvider.AddInParameter(command, helper.Desppremisasreprog, DbType.String, entity.Desppremisasreprog);
            dbProvider.AddInParameter(command, helper.Manequipo, DbType.String, entity.Manequipo);
            dbProvider.AddInParameter(command, helper.Mantipo, DbType.String, entity.Mantipo);
            dbProvider.AddInParameter(command, helper.Manhoraconex, DbType.String, entity.Manhoraconex);
            dbProvider.AddInParameter(command, helper.Manconsideraciones, DbType.String, entity.Manconsideraciones);
            dbProvider.AddInParameter(command, helper.Sumsubestacion, DbType.String, entity.Sumsubestacion);
            dbProvider.AddInParameter(command, helper.Summotivocorte, DbType.String, entity.Summotivocorte);
            dbProvider.AddInParameter(command, helper.Sumhorainicio, DbType.String, entity.Sumhorainicio);
            dbProvider.AddInParameter(command, helper.Sumreposicion, DbType.String, entity.Sumreposicion);
            dbProvider.AddInParameter(command, helper.Sumconsideraciones, DbType.String, entity.Sumconsideraciones);
            dbProvider.AddInParameter(command, helper.Regopecentral, DbType.String, entity.Regopecentral);
            dbProvider.AddInParameter(command, helper.Regcentralsubestacion, DbType.String, entity.Regcentralsubestacion);
            dbProvider.AddInParameter(command, helper.Regcentralhorafin, DbType.String, entity.Regcentralhorafin);
            dbProvider.AddInParameter(command, helper.Reglineas, DbType.String, entity.Reglineas);
            dbProvider.AddInParameter(command, helper.Reglineasubestacion, DbType.String, entity.Reglineasubestacion);
            dbProvider.AddInParameter(command, helper.Reglineahorafin, DbType.String, entity.Reglineahorafin);
            dbProvider.AddInParameter(command, helper.Gesequipo, DbType.String, entity.Gesequipo);
            dbProvider.AddInParameter(command, helper.Gesaceptado, DbType.String, entity.Gesaceptado);
            dbProvider.AddInParameter(command, helper.Gesdetalle, DbType.String, entity.Gesdetalle);
            dbProvider.AddInParameter(command, helper.Eveequipo, DbType.String, entity.Eveequipo);
            dbProvider.AddInParameter(command, helper.Everesumen, DbType.String, entity.Everesumen);
            dbProvider.AddInParameter(command, helper.Evehorainicio, DbType.String, entity.Evehorainicio);
            dbProvider.AddInParameter(command, helper.Evereposicion, DbType.String, entity.Evereposicion);
            dbProvider.AddInParameter(command, helper.Infequipo, DbType.String, entity.Infequipo);
            dbProvider.AddInParameter(command, helper.Infplazo, DbType.String, entity.Infplazo);
            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, entity.Infestado);
            dbProvider.AddInParameter(command, helper.Pafecha, DbType.String, entity.Pafecha);
            dbProvider.AddInParameter(command, helper.Pasorteo, DbType.String, entity.Pasorteo);
            dbProvider.AddInParameter(command, helper.Paresultado, DbType.String, entity.Paresultado);
            dbProvider.AddInParameter(command, helper.Pagenerador, DbType.String, entity.Pagenerador);
            dbProvider.AddInParameter(command, helper.Paprueba, DbType.String, entity.Paprueba);
            dbProvider.AddInParameter(command, helper.Desptiporeprog, DbType.String, entity.Desptiporeprog);
            dbProvider.AddInParameter(command, helper.Desparchivoatr, DbType.String, entity.Desparchivoatr);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(SiCambioTurnoSubseccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, entity.Seccioncodi);
            dbProvider.AddInParameter(command, helper.Subseccionnumber, DbType.Int32, entity.Subseccionnumber);
            dbProvider.AddInParameter(command, helper.Despcentromarginal, DbType.String, entity.Despcentromarginal);
            dbProvider.AddInParameter(command, helper.Despursautomatica, DbType.String, entity.Despursautomatica);
            dbProvider.AddInParameter(command, helper.Despmagautomatica, DbType.Decimal, entity.Despmagautomatica);
            dbProvider.AddInParameter(command, helper.Despursmanual, DbType.String, entity.Despursmanual);
            dbProvider.AddInParameter(command, helper.Despmagmanual, DbType.Decimal, entity.Despmagmanual);
            dbProvider.AddInParameter(command, helper.Despcentralaislado, DbType.String, entity.Despcentralaislado);
            dbProvider.AddInParameter(command, helper.Despmagaislado, DbType.Decimal, entity.Despmagaislado);
            dbProvider.AddInParameter(command, helper.Despreprogramas, DbType.String, entity.Despreprogramas);
            dbProvider.AddInParameter(command, helper.Desphorareprog, DbType.String, entity.Desphorareprog);
            dbProvider.AddInParameter(command, helper.Despmotivorepro, DbType.String, entity.Despmotivorepro);
            dbProvider.AddInParameter(command, helper.Desppremisasreprog, DbType.String, entity.Desppremisasreprog);
            dbProvider.AddInParameter(command, helper.Manequipo, DbType.String, entity.Manequipo);
            dbProvider.AddInParameter(command, helper.Mantipo, DbType.String, entity.Mantipo);
            dbProvider.AddInParameter(command, helper.Manhoraconex, DbType.String, entity.Manhoraconex);
            dbProvider.AddInParameter(command, helper.Manconsideraciones, DbType.String, entity.Manconsideraciones);
            dbProvider.AddInParameter(command, helper.Sumsubestacion, DbType.String, entity.Sumsubestacion);
            dbProvider.AddInParameter(command, helper.Summotivocorte, DbType.String, entity.Summotivocorte);
            dbProvider.AddInParameter(command, helper.Sumhorainicio, DbType.String, entity.Sumhorainicio);
            dbProvider.AddInParameter(command, helper.Sumreposicion, DbType.String, entity.Sumreposicion);
            dbProvider.AddInParameter(command, helper.Sumconsideraciones, DbType.String, entity.Sumconsideraciones);
            dbProvider.AddInParameter(command, helper.Regopecentral, DbType.String, entity.Regopecentral);
            dbProvider.AddInParameter(command, helper.Regcentralsubestacion, DbType.String, entity.Regcentralsubestacion);
            dbProvider.AddInParameter(command, helper.Regcentralhorafin, DbType.String, entity.Regcentralhorafin);
            dbProvider.AddInParameter(command, helper.Reglineas, DbType.String, entity.Reglineas);
            dbProvider.AddInParameter(command, helper.Reglineasubestacion, DbType.String, entity.Reglineasubestacion);
            dbProvider.AddInParameter(command, helper.Reglineahorafin, DbType.String, entity.Reglineahorafin);
            dbProvider.AddInParameter(command, helper.Gesequipo, DbType.String, entity.Gesequipo);
            dbProvider.AddInParameter(command, helper.Gesaceptado, DbType.String, entity.Gesaceptado);
            dbProvider.AddInParameter(command, helper.Gesdetalle, DbType.String, entity.Gesdetalle);
            dbProvider.AddInParameter(command, helper.Eveequipo, DbType.String, entity.Eveequipo);
            dbProvider.AddInParameter(command, helper.Everesumen, DbType.String, entity.Everesumen);
            dbProvider.AddInParameter(command, helper.Evehorainicio, DbType.String, entity.Evehorainicio);
            dbProvider.AddInParameter(command, helper.Evereposicion, DbType.String, entity.Evereposicion);
            dbProvider.AddInParameter(command, helper.Infequipo, DbType.String, entity.Infequipo);
            dbProvider.AddInParameter(command, helper.Infplazo, DbType.String, entity.Infplazo);
            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, entity.Infestado);
            dbProvider.AddInParameter(command, helper.Pafecha, DbType.String, entity.Pafecha);
            dbProvider.AddInParameter(command, helper.Pasorteo, DbType.String, entity.Pasorteo);
            dbProvider.AddInParameter(command, helper.Paresultado, DbType.String, entity.Paresultado);
            dbProvider.AddInParameter(command, helper.Pagenerador, DbType.String, entity.Pagenerador);
            dbProvider.AddInParameter(command, helper.Paprueba, DbType.String, entity.Paprueba);
            dbProvider.AddInParameter(command, helper.Desptiporeprog, DbType.String, entity.Desptiporeprog);
            dbProvider.AddInParameter(command, helper.Desparchivoatr, DbType.String, entity.Desparchivoatr);
            dbProvider.AddInParameter(command, helper.Subseccioncodi, DbType.Int32, entity.Subseccioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int subseccioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Subseccioncodi, DbType.Int32, subseccioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCambioTurnoSubseccionDTO GetById(int subseccioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Subseccioncodi, DbType.Int32, subseccioncodi);
            SiCambioTurnoSubseccionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCambioTurnoSubseccionDTO> List()
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();
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

        public List<SiCambioTurnoSubseccionDTO> GetByCriteria(int id)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerRSF(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerRSF, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();
                                        
                    int iUrsNomb = dr.GetOrdinal(helper.UrsNomb);
                    if (!dr.IsDBNull(iUrsNomb)) entity.Ursnomb = dr.GetString(iUrsNomb);

                    int iUrsValor = dr.GetOrdinal(helper.UrsValor);
                    if (!dr.IsDBNull(iUrsValor)) entity.Ursvalor = dr.GetDecimal(iUrsValor);

                    //int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    //int iRus = dr.GetOrdinal(helper.Rus);
                    //int iIcvalor1 = dr.GetOrdinal(helper.Icvalor1);
                    //int iCentral = dr.GetOrdinal(helper.Central);
                    //int iEquiAbrev = dr.GetOrdinal(helper.Equiabrev);

                    //int subcausaCodi = 0;
                    //string rus = string.Empty;
                    //decimal valor = 0;
                    //string central = string.Empty;
                    //string equipo = string.Empty;

                    //if (!dr.IsDBNull(iSubcausacodi)) subcausaCodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));
                    //if (!dr.IsDBNull(iRus)) rus = dr.GetString(iRus);
                    //if (!dr.IsDBNull(iIcvalor1)) valor = dr.GetDecimal(iIcvalor1);
                    //if (!dr.IsDBNull(iCentral)) central = dr.GetString(iCentral);
                    //if (!dr.IsDBNull(iEquiAbrev)) equipo = dr.GetString(iEquiAbrev);

                    //if (subcausaCodi == 319)
                    //{
                    //    entity.Despursautomatica = rus + "-" + central + "-" + equipo;
                    //    entity.Despmagautomatica = valor;
                    //}
                    //else if (subcausaCodi == 318)
                    //{
                    //    entity.Despursmanual = rus + "-" + central + "-" + equipo;
                    //    entity.Despmagmanual = valor;
                    //}

                    entity.Despursautomatica = entity.Ursnomb;
                    entity.Despmagautomatica = entity.Ursvalor;
                    entity.Subcausacodi = 319;
                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            List<SiCambioTurnoSubseccionDTO> listAutomatico = entitys.Where(x => x.Subcausacodi == 319).ToList();
            List<SiCambioTurnoSubseccionDTO> listManual = entitys.Where(x => x.Subcausacodi == 318).ToList();
            List<SiCambioTurnoSubseccionDTO> result = new List<SiCambioTurnoSubseccionDTO>();
            int maxAutomatico = listAutomatico.Count;
            int maxManual = listManual.Count;
            int max = (maxAutomatico > maxManual) ? maxAutomatico : maxManual;

            for (int i = 0; i < max; i++)
            {
                SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                if (i < maxAutomatico)
                {
                    entity.Despursautomatica = listAutomatico[i].Despursautomatica;
                    entity.Despmagautomatica = listAutomatico[i].Despmagautomatica;

                }
                if (i < maxManual)
                {
                    entity.Despursmanual = listManual[i].Despursmanual;
                    entity.Despmagmanual = listManual[i].Despmagmanual;
                }

                entity.Subseccionnumber = seccion;
                result.Add(entity);
            }

            if (result.Count == 0) result = this.CompletarLista(seccion);

            return result;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerReprogramas(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerReprogramas, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iReprograma = dr.GetOrdinal(helper.Reprograma);
                    int iHora = dr.GetOrdinal(helper.Hora);
                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);

                    string reprograma = string.Empty;
                    string descripcion = string.Empty;
                    int hora = 0;
                    string bloque = string.Empty;

                    if (!dr.IsDBNull(iHora)) hora = Convert.ToInt32(dr.GetValue(iHora));
                    if (!dr.IsDBNull(iDescripcion)) descripcion = dr.GetString(iDescripcion);
                    if (!dr.IsDBNull(iReprograma)) reprograma = dr.GetString(iReprograma);

                    if (hora % 2 == 0)
                        bloque = (hora / 2).ToString().PadLeft(2, '0') + ":00";
                    else
                        bloque = (hora / 2).ToString().PadLeft(2, '0') + ":30";

                    entity.Despreprogramas = reprograma;
                    entity.Desphorareprog = bloque;
                    entity.Despmotivorepro = descripcion;
                    entity.Subseccionnumber = seccion;

                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerMantenimientos(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerMantenimientos, fechaFin.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iManequipo = dr.GetOrdinal(helper.Manequipo);
                    if (!dr.IsDBNull(iManequipo)) entity.Manequipo = dr.GetString(iManequipo);

                    int iMantipo = dr.GetOrdinal(helper.Mantipo);
                    if (!dr.IsDBNull(iMantipo)) entity.Mantipo = dr.GetString(iMantipo);

                    int iManhoraconex = dr.GetOrdinal(helper.Manhoraconex);
                    if (!dr.IsDBNull(iManhoraconex)) entity.Manhoraconex = dr.GetString(iManhoraconex);

                    int iManconsideraciones = dr.GetOrdinal(helper.Manconsideraciones);
                    if (!dr.IsDBNull(iManconsideraciones)) entity.Manconsideraciones = dr.GetString(iManconsideraciones);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }


        public List<SiCambioTurnoSubseccionDTO> ObtenerMantenimientoComentario(DateTime fechaAnterior, int turnoAnterior, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerMantenimientosComentario, fechaAnterior.ToString(ConstantesBase.FormatoFecha),
                turnoAnterior);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iManequipo = dr.GetOrdinal(helper.Manequipo);
                    if (!dr.IsDBNull(iManequipo)) entity.Manequipo = dr.GetString(iManequipo);

                    int iMantipo = dr.GetOrdinal(helper.Mantipo);
                    if (!dr.IsDBNull(iMantipo)) entity.Mantipo = dr.GetString(iMantipo);

                    int iManhoraconex = dr.GetOrdinal(helper.Manhoraconex);
                    if (!dr.IsDBNull(iManhoraconex)) entity.Manhoraconex = dr.GetString(iManhoraconex);

                    int iManconsideraciones = dr.GetOrdinal(helper.Manconsideraciones);
                    if (!dr.IsDBNull(iManconsideraciones)) entity.Manconsideraciones = dr.GetString(iManconsideraciones);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;                       
        }


        public List<SiCambioTurnoSubseccionDTO> ObtenerSuministros(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerSuministros, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iSumsubestacion = dr.GetOrdinal(helper.Sumsubestacion);
                    if (!dr.IsDBNull(iSumsubestacion)) entity.Sumsubestacion = dr.GetString(iSumsubestacion);

                    int iSummotivocorte = dr.GetOrdinal(helper.Summotivocorte);
                    if (!dr.IsDBNull(iSummotivocorte)) entity.Summotivocorte = dr.GetString(iSummotivocorte);

                    int iSumreposicion = dr.GetOrdinal(helper.Sumreposicion);
                    if (!dr.IsDBNull(iSumreposicion)) entity.Sumreposicion = dr.GetString(iSumreposicion);

                    int iSumhorainicio = dr.GetOrdinal(helper.Sumhorainicio);
                    if (!dr.IsDBNull(iSumhorainicio)) entity.Sumhorainicio = dr.GetString(iSumhorainicio);

                    int iSumconsideraciones = dr.GetOrdinal(helper.Sumconsideraciones);
                    if (!dr.IsDBNull(iSumconsideraciones)) entity.Sumconsideraciones = dr.GetString(iSumconsideraciones);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerOperacionCentrales(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerOperacionCentral, fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();
                    
                    int iRegopecentral = dr.GetOrdinal(helper.Regopecentral);
                    if (!dr.IsDBNull(iRegopecentral)) entity.Regopecentral = dr.GetString(iRegopecentral);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerLineasDesconectadas(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerLineasDesconectadas, fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();                    

                    int iReglineas = dr.GetOrdinal(helper.Reglineas);
                    if (!dr.IsDBNull(iReglineas)) entity.Reglineas = dr.GetString(iReglineas);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerEventosImportantes(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerEventosImportantes, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iEveequipo = dr.GetOrdinal(helper.Eveequipo);
                    if (!dr.IsDBNull(iEveequipo)) entity.Eveequipo = dr.GetString(iEveequipo);

                    int iEveresumen = dr.GetOrdinal(helper.Everesumen);
                    if (!dr.IsDBNull(iEveresumen)) entity.Everesumen = dr.GetString(iEveresumen);

                    int iEvehorainicio = dr.GetOrdinal(helper.Evehorainicio);
                    if (!dr.IsDBNull(iEvehorainicio)) entity.Evehorainicio = dr.GetString(iEvehorainicio);

                    int iEvereposicion = dr.GetOrdinal(helper.Evereposicion);
                    if (!dr.IsDBNull(iEvereposicion)) entity.Evereposicion = dr.GetString(iEvereposicion);

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }

        public List<SiCambioTurnoSubseccionDTO> ObtenerInformeFalla(DateTime fechaInicio, DateTime fechaFin, int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();

            string query = String.Format(helper.SqlObtenerInformeFalla, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

                    int iInfequipo = dr.GetOrdinal(helper.Infequipo);
                    if (!dr.IsDBNull(iInfequipo)) entity.Infequipo = dr.GetString(iInfequipo);

                    int iInfestado = dr.GetOrdinal(helper.Infestado);
                    if (!dr.IsDBNull(iInfestado)) entity.Infestado = dr.GetString(iInfestado);

                    if (entity.Infestado != "S")
                    {
                        decimal horas = 0;

                        int iInfplazo = dr.GetOrdinal(helper.Infplazo);
                        if (!dr.IsDBNull(iInfplazo)) horas = dr.GetDecimal(iInfplazo);

                        if (horas >= 0)
                        {
                            entity.Infplazo = "Falta " + horas.ToString("#,####.0") + "h";
                        }
                        else
                        {
                            entity.Infplazo = "Plazo vencido";
                        }
                    }

                    entity.Subseccionnumber = seccion;
                    entitys.Add(entity);
                }
            }

            if (entitys.Count == 0) entitys = this.CompletarLista(seccion);

            return entitys;
        }


        public List<SiCambioTurnoSubseccionDTO> CompletarLista(int seccion)
        {
            List<SiCambioTurnoSubseccionDTO> entitys = new List<SiCambioTurnoSubseccionDTO>();
            for (int i = 0; i < 2; i++)
            {
                entitys.Add(new SiCambioTurnoSubseccionDTO { Subseccionnumber = seccion });
            }

            return entitys;
        }
    }
}
