using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnPronosticoDemandaRepository : RepositoryBase, IPrnPronosticoDemandaRepository
    {
        public PrnPronosticoDemandaRepository(string strConn) : base(strConn)
        {

        }

        PrnPronosticoDemandaHelper helper = new PrnPronosticoDemandaHelper();        

        #region Configuración General
        public List<EqAreaNivelDTO> ListPrnNivel()
        {
            List<EqAreaNivelDTO> entitys = new List<EqAreaNivelDTO>();
            EqAreaNivelDTO entity = new EqAreaNivelDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPrnNivel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaNivelDTO();

                    int iAnivecodi = dr.GetOrdinal(helper.Anivelcodi);
                    if (!dr.IsDBNull(iAnivecodi)) entity.ANivelCodi = Convert.ToInt32(dr.GetValue(iAnivecodi));

                    int iAnivelnomb = dr.GetOrdinal(helper.Anivelnomb);
                    if (!dr.IsDBNull(iAnivelnomb)) entity.ANivelNomb = dr.GetString(iAnivelnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListPrnArea(int anivelcodi)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            EqAreaDTO entity = new EqAreaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPrnArea);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListPrnAreaGrupo(int anivelcodi, int areacodi)
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            EqAreaDTO entity = new EqAreaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPrnAreaGrupo);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPrnPtoMedicion(int origlectcodi, int anivelcodi, int areapadre, int areacodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            MePtomedicionDTO entity = new MePtomedicionDTO();
            string strQuery = string.Format(helper.SqlListPrnPtoMedicion, origlectcodi, anivelcodi, areapadre, areacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPrnPtoMedicionDistr(int origlectcodi, int tipoemprcodi, int anivelcodi, int areapadre, int areacodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            MePtomedicionDTO entity = new MePtomedicionDTO();
            string strQuery = string.Format(helper.SqlListPrnPtoMedicionDistr, origlectcodi, tipoemprcodi, anivelcodi, areapadre, areacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public bool ValidarPlantillaAnio(DateTime fecha)
        {
            bool Valid = true;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarPlantillaAnio);
            dbProvider.AddInParameter(command, helper.Pmedatfecha, DbType.DateTime, fecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iPmedatcodi = dr.GetOrdinal(helper.Pmedatcodi);
                    if (!dr.IsDBNull(iPmedatcodi))
                    {
                        Valid = false;
                    }
                }
            }

            return Valid;
        }

        public List<MePtomedicionDTO> ListPtomedicionActivos()
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPtomedicionActivos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Depuración Manual
        public List<PrnMedicion48DTO> ListPuntosClasificadosByFecha(int ejec48, int ejec96, int prntipo, DateTime medifecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            List<PrnMedicion48DTO> entitys = new List<PrnMedicion48DTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntosClasificadosByFecha);
            dbProvider.AddInParameter(command, helper.Prnm48tipo, DbType.Int32, prntipo);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, ejec48);
            dbProvider.AddInParameter(command, helper.Prnm96tipo, DbType.Int32, prntipo);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, ejec96);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));
                    
                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnmintervalo = dr.GetOrdinal(helper.Prnmintervalo);
                    if (!dr.IsDBNull(iPrnmintervalo)) entity.Prnmintervalo = Convert.ToInt32(dr.GetValue(iPrnmintervalo));

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListEmpresasByTipo(int tipoemprcodi, int origlectcodi)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresasByTipo);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, origlectcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Emprrubro = Convert.ToInt32(dr.GetValue(iAreapadre));//Auxiliar

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListSubestacionesByEmpresa(int nivelao, int nivelsubest, int emprcodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSubestacionesByEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, nivelsubest);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, nivelao);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Información Usuarios Libres
        public int CountDespEjecByTipoEmpresa(int tipoemprcodi, int ptomedicodi, int lectcodi, int tipoinfocodi, DateTime fecini, DateTime fecfin)
        {
            int total = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCountDespEjecByTipoEmpresa);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fecini);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fecfin);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    total = dr.GetInt32(0);
                }

            }

            return total;
        }

        public List<EqEquipoDTO> ListSubestacionEmpresa()
        {
            EqEquipoDTO entity = new EqEquipoDTO();
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSubestacionEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Equiabrev = dr.GetString(iEmprabrev);//Emprabrev

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Equiabrev2 = dr.GetString(iAreaabrev);//Areaabrev

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreapadre));//Areapadre

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntosBySubestacionEmpresa(int emprcodi, int areacodi, DateTime medifecha)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntosBySubestacionEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnitv = dr.GetOrdinal(helper.Prnitv);
                    if (!dr.IsDBNull(iPrnitv)) entity.Prnitv = Convert.ToInt32(dr.GetValue(iPrnitv));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Adicionales

        public List<MePtomedicionDTO> ListMePtomedicionAO()
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMePtomedicionAO);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPtoMedicionByEmpresaArea(int tipoemprcodi, int areapadre)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPtoMedicionByEmpresaArea);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areapadre);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public EqAreaDTO GetAreaOperativaByEquipo(int equicodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAreaOperativaByEquipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);//Areaabrev

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));//Areapadre
                }
            }

            return entity;
        }

        //GetAreaOperativaBySubestacion
        public EqAreaDTO GetAreaOperativaBySubestacion(int areacodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAreaOperativaBySubestacion);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                }
            }

            return entity;
        }

        public List<EqAreaDTO> GetAreaOperativaByNivel(int anivelcodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAreaOperativaByNivel);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));//Areapadre

                    int iAnivelcodi= dr.GetOrdinal(helper.Anivelcodi);
                    if (!dr.IsDBNull(iAnivelcodi)) entity.ANivelCodi = Convert.ToInt32(dr.GetValue(iAnivelcodi));//Areapadre

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //0705 Agregado para separar las areas de las sunestaciones + centrales
        public List<EqAreaDTO> GetSubestacionCentralByNivel(int anivelcodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSubestacionCentralByNivel);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));//Areapadre

                    int iAnivelcodi = dr.GetOrdinal(helper.Anivelcodi);
                    if (!dr.IsDBNull(iAnivelcodi)) entity.ANivelCodi = Convert.ToInt32(dr.GetValue(iAnivelcodi));//Areapadre

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> GetSubEstacionSeleccionadas(int areapadre, int anivelcodi)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSubEstacionSeleccionadas);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areapadre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> GetSubEstacionDisponibles(int areanivel, int areapadre, int relpadre, int relnivel, int arearelnivel)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            string query = string.Format(helper.SqlGetSubEstacionDisponibles, areanivel, areapadre, relpadre, relnivel, arearelnivel);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteRelacion(int areacodi, int areapadre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteRelacion);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areapadre);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByPadre(int areapadre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByPadre);

            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areapadre);
            dbProvider.ExecuteNonQuery(command);
        }


        public EqAreaRelDTO GetSubestacionRel(int areacodi)
        {
            EqAreaRelDTO entity = new EqAreaRelDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSubestacionRel);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAreaRlCodi = dr.GetOrdinal(helper.AreaRlCodi);
                    if (!dr.IsDBNull(iAreaRlCodi)) entity.AreaRlCodi = Convert.ToInt32(dr.GetValue(iAreaRlCodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.AreaPadre = Convert.ToInt32(dr.GetValue(iAreapadre));

                }
            }

            return entity;
        }

        //GetBarrasSeleccionadas
        public List<PrGrupoDTO> GetBarrasSeleccionadas(int areacodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBarrasSeleccionadas);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> GetBarrasDisponibles(int areacodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBarrasDisponibles);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdatePrGrupo(int grupocodi, int areacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateGrupo);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            
            dbProvider.ExecuteNonQuery(command);
        }
        #endregion

        //2020-01
        public List<PrGrupoDTO> GetListBarras()
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarra);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodii = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodii)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodii));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListBarrasPM(int catecodi, string barrascp, int version)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlBarraPM, catecodi, barrascp, version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Editar     
        public List<PrGrupoDTO> ListBarrasPMEdit(int catecodi, int version)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlBarraPMEdit, catecodi, version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListBarraCPDisponibles(int catecodi, int prnvercodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBarraCPDisponible);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, prnvercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //17032020
        public List<PrGrupoDTO> ListBarraPMDisponibles(int catecodi, int prnvercodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBarraPMDisponible);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, prnvercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Métodos para el modulo de Relación de Barras
        public List<PrGrupoDTO> ListRelacionBarrasPM(int anivelcodi, int anivelcodi2, int catecodi, string areapadre, string grupocodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListRelacionBarrasPM, anivelcodi, anivelcodi2, catecodi, areapadre, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

                    int iAreapadreabrev = dr.GetOrdinal(helper.Areapadreabrev);
                    if (!dr.IsDBNull(iAreapadreabrev)) entity.Areapadreabrev = dr.GetString(iAreapadreabrev);

                    int iAreapadrenomb = dr.GetOrdinal(helper.Areapadrenomb);
                    if (!dr.IsDBNull(iAreapadrenomb)) entity.Areapadrenomb = dr.GetString(iAreapadrenomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb); ;

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb); ;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListRelacionPtosPorBarraPM(string grupocodibarra)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListRelacionPtosPorBarraPM, grupocodibarra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
        public List<PrnAgrupacionDTO> AgrupacionesPuntosList(int origen, int barra)
        {
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            string query = string.Format(helper.SqlListRelacionAgrupacionPunto, origen, barra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAgrupacionDTO entity = new PrnAgrupacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtogrphijocodi = dr.GetOrdinal(helper.Ptogrphijocodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptogrphijocodi = Convert.ToInt32(dr.GetValue(iPtogrphijocodi));

                    int iPtogrphijodesc = dr.GetOrdinal(helper.Ptogrphijodesc);
                    if (!dr.IsDBNull(iPtogrphijodesc)) entity.Ptogrphijodesc = dr.GetString(iPtogrphijodesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionDTO> AgrupacionesList(int origen, int barra)
        {
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            string query = string.Format(helper.SqlAgrupacionesList, origen, barra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAgrupacionDTO entity = new PrnAgrupacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
        public List<MePtomedicionDTO> ListPuntosNoAgrupaciones(int orgagrupacion, int barra, int orgpunto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlPuntosNoAgrupacionesList, orgagrupacion, barra, orgpunto);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);
                    
                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntosBarra(int grupobarra)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlPuntosBarraList, grupobarra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateMeMedicionBarra(int punto, int barra, string user)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMeMedicionBarra);

            dbProvider.AddInParameter(command, helper.Grupocodibarra, DbType.Int32, barra);            
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, punto);

            dbProvider.ExecuteNonQuery(command);
        }
        //Fin
        public List<PrGrupoDTO> ListBarrasPMNombre(int grupocodi, int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListBarrasPMNombre, grupocodi, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb); ;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrGrupoDTO ListBarrasCPNombre(int grupocodi, int catecodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            string query = string.Format(helper.SqlListBarrasCPNombre, grupocodi, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb); ;

                }
            }

            return entity;
        }

        public List<PrGrupoDTO> GetLisBarrasSoloPM(int catecodi, string barracp)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListBarrasSoloPM, catecodi, barracp);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb); ;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> GetLisBarrasSoloCP(int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListBarrasSoloCP, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //14042020
        //Lista empresas para filtro Relacion Barras ListEmpresaBarrasRel
        public List<SiEmpresaDTO> ListEmpresaBarrasRel()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaBarrasRel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Lista Puntos y Agrupaciones por Empresas
        public List<PrGrupoDTO> ListPuntosByEmpresa(string empresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListPuntosByEmpresa, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //
        //Lista empresas para filtro Relacion Barras ListEmpresaBarrasRel
        public List<SiEmpresaDTO> ListEmpresaByBarra(string barra)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlListEmpresaByBarra, barra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Lista de Brras en la tabla PtoMedicion
        public List<PrGrupoDTO> ListBarrasInPtoMedicion(int anivelcodi, int anivelcodi2, int catecodi, string areapadre, string grupocodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListBarrasInPtoMedicion, anivelcodi, anivelcodi2, catecodi, areapadre, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

                    int iAreapadreabrev = dr.GetOrdinal(helper.Areapadreabrev);
                    if (!dr.IsDBNull(iAreapadreabrev)) entity.Areapadreabrev = dr.GetString(iAreapadreabrev);

                    int iAreapadrenomb = dr.GetOrdinal(helper.Areapadrenomb);
                    if (!dr.IsDBNull(iAreapadrenomb)) entity.Areapadrenomb = dr.GetString(iAreapadrenomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb); ;

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Lista de perdidas por barra
        public List<PrnPrdTransversalDTO> ListaPerdidasTransversalesByBarra(string barras)
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            string query = string.Format(helper.SqlListaPerdidasTransversalesByBarra, barras);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();

                    int iPrdtrncodi = dr.GetOrdinal(helper.Prdtrncodi);
                    if (!dr.IsDBNull(iPrdtrncodi)) entity.Prdtrncodi = Convert.ToInt32(dr.GetValue(iPrdtrncodi));

                    int iPrdtrnetqnomb = dr.GetOrdinal(helper.Prdtrnetqnomb);
                    if (!dr.IsDBNull(iPrdtrnetqnomb)) entity.Prdtrnetqnomb = dr.GetString(iPrdtrnetqnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrdtrnperdida = dr.GetOrdinal(helper.Prdtrnperdida);
                    if (!dr.IsDBNull(iPrdtrnperdida)) entity.Prdtrnperdida = dr.GetDecimal(iPrdtrnperdida);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //
        public List<PrGrupoDTO> PerdidasTransversalesCPDisponibles(string barra)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlPerdidasTransversalesCPDisponibles, barra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnPrdTransversalDTO> PerdidasTransversalesCPSeleccionadas(string codigo)
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            string query = string.Format(helper.SqlPerdidasTransversalesCPSeleccionadas, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();

                    //int iPrdtrncodi = dr.GetOrdinal(helper.Prdtrncodi);
                    //if (!dr.IsDBNull(iPrdtrncodi)) entity.Prdtrncodi = Convert.ToInt32(dr.GetValue(iPrdtrncodi));

                    int iPrdtrnetqnomb = dr.GetOrdinal(helper.Prdtrnetqnomb);
                    if (!dr.IsDBNull(iPrdtrnetqnomb)) entity.Prdtrnetqnomb = dr.GetString(iPrdtrnetqnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrdtrnperdida = dr.GetOrdinal(helper.Prdtrnperdida);
                    if (!dr.IsDBNull(iPrdtrnperdida)) entity.Prdtrnperdida = dr.GetDecimal(iPrdtrnperdida);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeletePerdidaTransversal(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePerdidaTransversal);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int VersionActiva()
        {
            int version = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVersionActiva);
           
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    version = dr.GetInt32(0);

                }
            }

            return version;
        }

        //Barras Defecto
        public List<PrGrupoDTO> ListBarraDefecto(int catecodi, int prnvercodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBarraDefecto);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, prnvercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Bitacora
        public List<MeJustificacionDTO> ListaBitacora(string fechaIni, 
            string fechaFin, string lectcodi, 
            string tipoempresa, int regIni, 
            int regFin)
        {
            List<MeJustificacionDTO> entitys = new List<MeJustificacionDTO>();

            string query = string.Format(helper.SqlListaBitacora, 
                fechaIni, fechaFin, lectcodi,
                tipoempresa, regIni, regFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeJustificacionDTO entity = new MeJustificacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = dr.GetString(iArea);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iHorainicio = dr.GetOrdinal(helper.Horainicio);
                    if (!dr.IsDBNull(iHorainicio)) entity.Horainicio = dr.GetString(iHorainicio);

                    int iHorafin = dr.GetOrdinal(helper.Horafin);
                    if (!dr.IsDBNull(iHorafin)) entity.Horafin = dr.GetString(iHorafin);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iConsumoprevisto = dr.GetOrdinal(helper.ConsumoPrevisto);
                    if (!dr.IsDBNull(iConsumoprevisto)) entity.Consumoprevisto = dr.GetDecimal(iConsumoprevisto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListUnidadByTipo(int tipo) {

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListUnidadByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntosFormulas(int tipo)
        {

            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListPuntosFormulas, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ListPerfilRuleByEstimador(string prefijo)
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            string query = string.Format(helper.SqlListPerfilRuleByEstimador, prefijo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region PRONDEMA.E3
        public List<MePtomedicionDTO> ListPtomedicionByOriglectcodi(int origlectcodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListPtomedicionByOriglectcodi, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnMediciongrpDTO MedicionBarraByFechaVersionBarra(string fecha, int version, int barra)
        {
            PrnMediciongrpDTO entity = new PrnMediciongrpDTO();
            string query = string.Format(helper.SqlMedicionBarraByFechaVersionBarra, fecha, version, barra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iVergrpcodi = dr.GetOrdinal(helper.Vergrpcodi);
                    if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                }
            }

            return entity;
        }

        public List<PrnMediciongrpDTO> ListVersionMedicionGrp()
        {
            List<PrnMediciongrpDTO> entitys = new List<PrnMediciongrpDTO>();
            string query = string.Format(helper.SqlListVersionMedicionGrp);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMediciongrpDTO entity = new PrnMediciongrpDTO();

                    int iVergrpcodi = dr.GetOrdinal(helper.Vergrpcodi);
                    if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

                    int iVergrpnomb = dr.GetOrdinal(helper.Vergrpnomb);
                    if (!dr.IsDBNull(iVergrpnomb)) entity.Vergrpnomb = dr.GetString(iVergrpnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateMedicionTrasladoCarga(PrnMediciongrpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMedicionTrasladoCarga);

            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.Prnmgrtraslado, DbType.String, entity.PrnmgrTraslado);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prnmgrtipo, DbType.Int32, entity.Prnmgrtipo);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnMediciongrpDTO> GetLisBarrasSoloCPTraslado(int catecodi, int vergrpcodi)
        {
            List<PrnMediciongrpDTO> entitys = new List<PrnMediciongrpDTO>();
            string query = string.Format(helper.SqlListBarrasSoloCPTraslado, catecodi, vergrpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMediciongrpDTO entity = new PrnMediciongrpDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrnmgrtrasladocount = dr.GetOrdinal(helper.Prnmgrtrasladocount);
                    if (!dr.IsDBNull(iPrnmgrtrasladocount)) entity.PrnmgrTrasladoCount = Convert.ToInt32(dr.GetValue(iPrnmgrtrasladocount));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnMediciongrpDTO GetBarrasCPGroupByFechaTipo(int prnmgrtipo, DateTime medifecha)
        {
            PrnMediciongrpDTO entity = new PrnMediciongrpDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBarrasCPGroupByFechaTipo);

            dbProvider.AddInParameter(command, helper.Prnmgrtipo, DbType.Int32, prnmgrtipo);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iPrnmgrtipo = dr.GetOrdinal(helper.Prnmgrtipo);
                    if (!dr.IsDBNull(iPrnmgrtipo)) entity.Prnmgrtipo = Convert.ToInt32(dr.GetValue(iPrnmgrtipo));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);
                }
            }

            return entity;
        }

        //Assetec 20220201
        /// <summary>
        /// Data para formato de Demanda CP
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetDataFormatoPronosticoDemandaByVersion(int formatcodi, DateTime fechaini, DateTime fechafin, int version)
        {
            List<PrnMediciongrpDTO> entitys = new List<PrnMediciongrpDTO>();

            string query = string.Format(helper.SqlGetListFormatoDemandaCPByVersion, fechaini.ToString(ConstantesBase.FormatoFechaBase), fechafin.ToString(ConstantesBase.FormatoFechaBase), version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMediciongrpDTO entity = new PrnMediciongrpDTO();
                    //entity = helper.Create(dr);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        //Assetec 20220321
        public List<MePtomedicionDTO> GetAgrupacionByBarraPM(int pm)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            MePtomedicionDTO entity = new MePtomedicionDTO();
            string strQuery = string.Format(helper.SqlGetAgrupacionByBarraPM, pm);
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion


        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 07-03-2022 metodos tabla BITACORA
        // -----------------------------------------------------------------------------------------------------------------
        #region BITACORA.E3
        public void SaveBitacora3(PrnBitacoraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdBitacora);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveBitacora);

            dbProvider.AddInParameter(command, helper.Prnbitcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Prnbithorainicio, DbType.String, entity.Prnbithorainicio);
            dbProvider.AddInParameter(command, helper.Prnbithorafin, DbType.String, entity.Prnbithorafin);
            dbProvider.AddInParameter(command, helper.Prnbittipregistro, DbType.String, entity.Prnbittipregistro);
            dbProvider.AddInParameter(command, helper.Prnbitocurrencia, DbType.String, entity.Prnbitocurrencia);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prnbitconstipico, DbType.Decimal, entity.Prnbitconstipico);
            dbProvider.AddInParameter(command, helper.Prnbitconsprevisto, DbType.Decimal, entity.Prnbitconsprevisto);
            dbProvider.AddInParameter(command, helper.Prnbitptodiferencial, DbType.Decimal, entity.Prnbitptodiferencial);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Prnbitvalor, DbType.String, entity.Prnbitvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnBitacoraDTO> ListBitacora(string fechaIni,
            string fechaFin, string tipregistro, 
            string tipoemprcod, string lectcodi)
        {
            List<PrnBitacoraDTO> entitys = new List<PrnBitacoraDTO>();

            string query = string.Format(helper.SqlListBitacora,
                fechaIni, fechaFin, tipregistro,
                tipoemprcod, lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnBitacoraDTO entity = new PrnBitacoraDTO();

                    int iPrnbitcodi = dr.GetOrdinal(helper.Prnbitcodi);
                    if (!dr.IsDBNull(iPrnbitcodi)) entity.Prnbitcodi = Convert.ToInt32(dr.GetValue(iPrnbitcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnbithorainicio = dr.GetOrdinal(helper.Prnbithorainicio);
                    if (!dr.IsDBNull(iPrnbithorainicio)) entity.Prnbithorainicio = dr.GetString(iPrnbithorainicio);

                    int iPrnbithorafin = dr.GetOrdinal(helper.Prnbithorafin);
                    if (!dr.IsDBNull(iPrnbithorafin)) entity.Prnbithorafin = dr.GetString(iPrnbithorafin);

                    int iPrnbittipregistro = dr.GetOrdinal(helper.Prnbittipregistro);
                    if (!dr.IsDBNull(iPrnbittipregistro)) entity.Prnbittipregistro = dr.GetString(iPrnbittipregistro);

                    int iPrnbitocurrencia = dr.GetOrdinal(helper.Prnbitocurrencia);
                    if (!dr.IsDBNull(iPrnbitocurrencia)) entity.Prnbitocurrencia = dr.GetString(iPrnbitocurrencia);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrnbitconstipico = dr.GetOrdinal(helper.Prnbitconstipico);
                    if (!dr.IsDBNull(iPrnbitconstipico)) entity.Prnbitconstipico = dr.GetDecimal(iPrnbitconstipico);

                    int iPrnbitconsprevisto = dr.GetOrdinal(helper.Prnbitconsprevisto);
                    if (!dr.IsDBNull(iPrnbitconsprevisto)) entity.Prnbitconsprevisto = dr.GetDecimal(iPrnbitconsprevisto);

                    int iPrnbitptodiferencial = dr.GetOrdinal(helper.Prnbitptodiferencial);
                    if (!dr.IsDBNull(iPrnbitptodiferencial)) entity.Prnbitptodiferencial = dr.GetDecimal(iPrnbitptodiferencial);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iLectcodi = dr.GetOrdinal(helper.Lectodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

                    int iTipoemprcod = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcod)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcod));


                    int iPrnbitvalor = dr.GetOrdinal(helper.Prnbitvalor);
                    if (!dr.IsDBNull(iPrnbitvalor)) entity.Prnbitvalor = dr.GetString(iPrnbitvalor); // valor

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        // ------------------------------------ FIN ASSETEC 14-03-2022 -----------------------------------------------------

        #region Mejoras PRODEM.E3 40 horas
        public int TotalRegConsultaBitacora(string idLectura,
            string idTipoEmpresa,
            string fechaIni,
            string fechaFin)
        {
            int total = 0;
            string query = string.Format(helper.SqlTotalRegConsultaBitacora,
                idLectura, idTipoEmpresa,
                fechaIni, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }
        #endregion
    }
}
