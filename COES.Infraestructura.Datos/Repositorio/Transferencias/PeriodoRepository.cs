using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class PeriodoRepository : RepositoryBase, IPeriodoRepository
    {
        public PeriodoRepository(string strConn) : base(strConn)
        {
        }

        PeriodoHelper helper = new PeriodoHelper();

        public int Save(PeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iPeriCodi = GetCodigoGenerado();

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, entity.PeriNombre);
            dbProvider.AddInParameter(command, helper.Aniocodi, DbType.Int32, entity.AnioCodi);
            dbProvider.AddInParameter(command, helper.Mescodi, DbType.Int32, entity.MesCodi);
            dbProvider.AddInParameter(command, helper.Perianiomes, DbType.Int32, entity.PeriAnioMes);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.RecaNombre);
            dbProvider.AddInParameter(command, helper.Perifechavalorizacion, DbType.DateTime, entity.PeriFechaValorizacion);
            dbProvider.AddInParameter(command, helper.Perifechalimite, DbType.DateTime, entity.PeriFechaLimite);
            dbProvider.AddInParameter(command, helper.Perihoralimite, DbType.String, entity.PeriHoraLimite);
            dbProvider.AddInParameter(command, helper.Perifechaobservacion, DbType.DateTime, entity.PeriFechaObservacion);
            dbProvider.AddInParameter(command, helper.Periestado, DbType.String, entity.PeriEstado);
            dbProvider.AddInParameter(command, helper.Periusername, DbType.String, entity.PeriUserName);
            dbProvider.AddInParameter(command, helper.Perifecins, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PeriFormNuevo, DbType.Int32, entity.PeriFormNuevo);
            dbProvider.ExecuteNonQuery(command);
            return iPeriCodi;
        }

        public void Update(PeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, entity.PeriNombre);
            dbProvider.AddInParameter(command, helper.Aniocodi, DbType.Int32, entity.AnioCodi);
            dbProvider.AddInParameter(command, helper.Mescodi, DbType.Int32, entity.MesCodi);
            dbProvider.AddInParameter(command, helper.Perianiomes, DbType.Int32, entity.PeriAnioMes);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.RecaNombre);
            dbProvider.AddInParameter(command, helper.Perifechavalorizacion, DbType.DateTime, entity.PeriFechaValorizacion);
            dbProvider.AddInParameter(command, helper.Perifechalimite, DbType.DateTime, entity.PeriFechaLimite);
            dbProvider.AddInParameter(command, helper.Perihoralimite, DbType.String, entity.PeriHoraLimite);
            dbProvider.AddInParameter(command, helper.Perifechaobservacion, DbType.DateTime, entity.PeriFechaObservacion);
            dbProvider.AddInParameter(command, helper.Periestado, DbType.String, entity.PeriEstado);
            dbProvider.AddInParameter(command, helper.Perifecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PeriFormNuevo, DbType.Int32, entity.PeriFormNuevo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.PeriCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public PeriodoDTO GetById(System.Int32? id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, id);

            PeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PeriodoDTO> List()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
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

        public List<PeriodoDTO> ListPeriodoPotencia()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeriodoPotencia);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetFirstPeriodoFormatNew()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetFirstPeriodoFormatNew);
            int periCodi = Convert.ToInt32(dbProvider.ExecuteScalar(command).ToString());
            return periCodi;
        }

        public List<PeriodoDTO> GetByCriteria(string nombre)
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public PeriodoDTO GetByAnioMes(int iPeriAnioMes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAnioMes);
            dbProvider.AddInParameter(command, helper.Perianiomes, DbType.Int32, iPeriAnioMes);
            PeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int GetNumRegistros(int iPeriCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNumRegistros);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);

            object result = dbProvider.ExecuteScalar(command);
            int iNumRegistros = -1;
            if (result != null) iNumRegistros = Convert.ToInt32(result);
            return iNumRegistros;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<PeriodoDTO> ListarByEstado(string sPeriEstado)
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarByEstado);
            dbProvider.AddInParameter(command, helper.Periestado, DbType.String, sPeriEstado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PeriodoDTO> ListarByEstadoPublicarCerrado()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarByEstadoPublicarCerrado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public PeriodoDTO BuscarPeriodoAnterior(int iPeriCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeriodoAnteriorById);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);

            PeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PeriodoDTO> ListarPeriodosFuturos(int iPeriCodi)
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarPeriodosFuturos);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public PeriodoDTO ObtenerPeriodoDTR(int anio, int mes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPeriodoDTR);
            dbProvider.AddInParameter(command, helper.Mescodi, DbType.Int32, mes);
            dbProvider.AddInParameter(command, helper.Aniocodi, DbType.Int32, anio);

            PeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new PeriodoDTO();

                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = Convert.ToInt32(dr.GetValue(iPericodi));

                    int iMescodi = dr.GetOrdinal(helper.Mescodi);
                    if (!dr.IsDBNull(iMescodi)) entity.MesCodi = Convert.ToInt32(dr.GetValue(iMescodi));

                    int iAniocodi = dr.GetOrdinal(helper.Aniocodi);
                    if (!dr.IsDBNull(iAniocodi)) entity.AnioCodi = Convert.ToInt32(dr.GetValue(iAniocodi));
                }
            }
            return entity;
        }

        // Inicio de Agregados - Sistema de Compensaciones
        public List<PeriodoDTO> ListarPeriodosTC()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarPeriodosTC);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PeriodoDTO entity = new PeriodoDTO();

                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = dr.GetInt32(iPericodi);

                    int iPerinombre = dr.GetOrdinal(helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.PeriNombre = dr.GetString(iPerinombre);

                    int iAniocodi = dr.GetOrdinal(helper.Aniocodi);
                    if (!dr.IsDBNull(iAniocodi)) entity.AnioCodi = dr.GetInt32(iAniocodi);

                    int iMescodi = dr.GetOrdinal(helper.Mescodi);
                    if (!dr.IsDBNull(iMescodi)) entity.MesCodi = dr.GetInt32(iMescodi);

                    int iRecanombre = dr.GetOrdinal(helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);

                    int iPerifechavalorizacion = dr.GetOrdinal(helper.Perifechavalorizacion);
                    if (!dr.IsDBNull(iPerifechavalorizacion)) entity.PeriFechaValorizacion = dr.GetDateTime(iPerifechavalorizacion);

                    int iPerifechalimite = dr.GetOrdinal(helper.Perifechalimite);
                    if (!dr.IsDBNull(iPerifechalimite)) entity.PeriFechaLimite = dr.GetDateTime(iPerifechalimite);

                    int iPerifechaobservacion = dr.GetOrdinal(helper.Perifechaobservacion);
                    if (!dr.IsDBNull(iPerifechaobservacion)) entity.PeriFechaObservacion = dr.GetDateTime(iPerifechaobservacion);

                    int iPeriestado = dr.GetOrdinal(helper.Periestado);
                    if (!dr.IsDBNull(iPeriestado)) entity.PeriEstado = dr.GetString(iPeriestado);

                    int iPeriusername = dr.GetOrdinal(helper.Periusername);
                    if (!dr.IsDBNull(iPeriusername)) entity.PeriUserName = dr.GetString(iPeriusername);

                    int iPerifecins = dr.GetOrdinal(helper.Perifecins);
                    if (!dr.IsDBNull(iPerifecins)) entity.PeriFecIns = dr.GetDateTime(iPerifecins);

                    int iPerifecact = dr.GetOrdinal(helper.Perifecact);
                    if (!dr.IsDBNull(iPerifecact)) entity.PerifecAct = dr.GetDateTime(iPerifecact);

                    int iPerianiomes = dr.GetOrdinal(helper.Perianiomes);
                    if (!dr.IsDBNull(iPerianiomes)) entity.PeriAnioMes = dr.GetInt32(iPerianiomes);

                    int iPerihoralimite = dr.GetOrdinal(helper.Perihoralimite);
                    if (!dr.IsDBNull(iPerihoralimite)) entity.PeriHoraLimite = dr.GetString(iPerihoralimite);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PeriodoDTO> ListarPeriodosCompensacion()
        {
            List<PeriodoDTO> entitys = new List<PeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarPeriodosCompensacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PeriodoDTO entity = new PeriodoDTO();

                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = dr.GetInt32(iPericodi);

                    int iPerinombre = dr.GetOrdinal(helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.PeriNombre = dr.GetString(iPerinombre);

                    int iAniocodi = dr.GetOrdinal(helper.Aniocodi);
                    if (!dr.IsDBNull(iAniocodi)) entity.AnioCodi = dr.GetInt32(iAniocodi);

                    int iMescodi = dr.GetOrdinal(helper.Mescodi);
                    if (!dr.IsDBNull(iMescodi)) entity.MesCodi = dr.GetInt32(iMescodi);

                    int iRecanombre = dr.GetOrdinal(helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);

                    int iPeriestado = dr.GetOrdinal(helper.Periestado);
                    if (!dr.IsDBNull(iPeriestado)) entity.PeriEstado = dr.GetString(iPeriestado);

                    int iPerianiomes = dr.GetOrdinal(helper.Perianiomes);
                    if (!dr.IsDBNull(iPerianiomes)) entity.PeriAnioMes = dr.GetInt32(iPerianiomes);

                    int iPecanombre = dr.GetOrdinal(helper.Pecanombre);
                    if (!dr.IsDBNull(iPecanombre)) entity.PecaNombre = dr.GetString(iPecanombre);

                    int iPecaversioncomp = dr.GetOrdinal(helper.Pecaversioncomp);
                    if (!dr.IsDBNull(iPecaversioncomp)) entity.PecaVersionComp = dr.GetInt32(iPecaversioncomp);

                    int iPecaversionvtea = dr.GetOrdinal(helper.Pecaversionvtea);
                    if (!dr.IsDBNull(iPecaversionvtea)) entity.PecaVersionVTEA = dr.GetInt32(iPecaversionvtea);

                    int iRecaNombreComp = dr.GetOrdinal(helper.RecaNombreComp);
                    if (!dr.IsDBNull(iRecaNombreComp)) entity.RecaNombreComp = dr.GetString(iRecaNombreComp);

                    int iPecaestadoregistro = dr.GetOrdinal(helper.Pecaestadoregistro);
                    if (!dr.IsDBNull(iPecaestadoregistro)) entity.PecaEstadoRegistro = dr.GetString(iPecaestadoregistro);

                    int iPecadscestado = dr.GetOrdinal(helper.Pecadscestado);
                    if (!dr.IsDBNull(iPecadscestado)) entity.PecaEstadoRegistro = dr.GetString(iPecadscestado);

                    int iPeriinforme = dr.GetOrdinal(helper.Periinforme);
                    if (!dr.IsDBNull(iPeriinforme)) entity.PeriInforme = dr.GetString(iPeriinforme);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public PeriodoDTO GetPeriodoByIdProcesa(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeriodoByIdProcesa);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, id);

            PeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFechaini = dr.GetOrdinal(helper.Fechaini);
                    if (!dr.IsDBNull(iFechaini)) entity.Fechaini = dr.GetDateTime(iFechaini);

                    int iFechafin = dr.GetOrdinal(helper.Fechafin);
                    if (!dr.IsDBNull(iFechafin)) entity.Fechafin = dr.GetDateTime(iFechafin);

                }
            }

            return entity;
        }
        // Fin de Agregados - Sitstema de Compensaciones

        //2018.Setiembre - Agregados por ASSETEC
        public int GetPKTrnContador(string trncnttabla, string trncntcolumna)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPKTrnContador);

            dbProvider.AddInParameter(command, helper.Trncnttabla, DbType.String, trncnttabla);
            dbProvider.AddInParameter(command, helper.Trncntcolumna, DbType.String, trncntcolumna);

            object result = dbProvider.ExecuteScalar(command);
            int iTrncntcontador = -1; //Si devuelce este valor es un error...!
            if (result != null) iTrncntcontador = Convert.ToInt32(result);
            return iTrncntcontador;
        }

        public void UpdatePKTrnContador(string trncnttabla, string trncntcolumna, Int32 trncntcontador)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePKTrnContador);

            dbProvider.AddInParameter(command, helper.Trncntcontador, DbType.Int32, trncntcontador);
            dbProvider.AddInParameter(command, helper.Trncnttabla, DbType.String, trncnttabla);
            dbProvider.AddInParameter(command, helper.Trncntcolumna, DbType.String, trncntcolumna);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
