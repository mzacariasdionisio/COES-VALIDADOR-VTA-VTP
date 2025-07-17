using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_FORMATO
    /// </summary>
    public class MeFormatoRepository : RepositoryBase, IMeFormatoRepository
    {
        private string strConexion;
        public MeFormatoRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        MeFormatoHelper helper = new MeFormatoHelper();

        public int Save(MeFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Formatresolucion, DbType.Int32, entity.Formatresolucion);
            dbProvider.AddInParameter(command, helper.Formatperiodo, DbType.Int32, entity.Formatperiodo);
            dbProvider.AddInParameter(command, helper.Formatnombre, DbType.String, entity.Formatnombre);
            dbProvider.AddInParameter(command, helper.Formathorizonte, DbType.Int32, entity.Formathorizonte);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Formatdiaplazo, DbType.Int32, entity.Formatdiaplazo);
            dbProvider.AddInParameter(command, helper.Formatminplazo, DbType.Int32, entity.Formatminplazo);
            dbProvider.AddInParameter(command, helper.Formatcheckblanco, DbType.Int32, entity.Formatcheckblanco);
            dbProvider.AddInParameter(command, helper.Formatcheckplazo, DbType.Int32, entity.Formatcheckplazo);
            dbProvider.AddInParameter(command, helper.Formatallempresa, DbType.Int32, entity.Formatallempresa);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Formatdescrip, DbType.String, entity.Formatdescrip);
            dbProvider.AddInParameter(command, helper.Formatsecundario, DbType.String, entity.Formatsecundario);
            dbProvider.AddInParameter(command, helper.Formatdiafinplazo, DbType.Int32, entity.Formatdiafinplazo);
            dbProvider.AddInParameter(command, helper.Formatminfinplazo, DbType.Int32, entity.Formatminfinplazo);
            dbProvider.AddInParameter(command, helper.Formatnumbloques, DbType.Int32, entity.Formatnumbloques);
            dbProvider.AddInParameter(command, helper.Formatdiafinfueraplazo, DbType.Int32, entity.Formatdiafinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatminfinfueraplazo, DbType.Int32, entity.Formatminfinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatmesplazo, DbType.Int32, entity.Formatmesplazo);
            dbProvider.AddInParameter(command, helper.Formatmesfinplazo, DbType.Int32, entity.Formatmesfinplazo);
            dbProvider.AddInParameter(command, helper.Formatmesfinfueraplazo, DbType.Int32, entity.Formatmesfinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatdependeconfigptos, DbType.Int32, entity.Formatdependeconfigptos);
            dbProvider.AddInParameter(command, helper.Formatenviocheckadicional, DbType.String, entity.Formatenviocheckadicional);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Formatresolucion, DbType.Int32, entity.Formatresolucion);
            dbProvider.AddInParameter(command, helper.Formatperiodo, DbType.Int32, entity.Formatperiodo);
            dbProvider.AddInParameter(command, helper.Formatnombre, DbType.String, entity.Formatnombre);
            dbProvider.AddInParameter(command, helper.Formathorizonte, DbType.Int32, entity.Formathorizonte);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Formatdiaplazo, DbType.Int32, entity.Formatdiaplazo);
            dbProvider.AddInParameter(command, helper.Formatminplazo, DbType.Int32, entity.Formatminplazo);
            dbProvider.AddInParameter(command, helper.Formatcheckblanco, DbType.Int32, entity.Formatcheckblanco);
            dbProvider.AddInParameter(command, helper.Formatcheckplazo, DbType.Int32, entity.Formatcheckplazo);
            dbProvider.AddInParameter(command, helper.Formatallempresa, DbType.Int32, entity.Formatallempresa);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Formatdescrip, DbType.String, entity.Formatdescrip);
            dbProvider.AddInParameter(command, helper.Formatsecundario, DbType.String, entity.Formatsecundario);
            dbProvider.AddInParameter(command, helper.Formatdiafinplazo, DbType.Int32, entity.Formatdiafinplazo);
            dbProvider.AddInParameter(command, helper.Formatminfinplazo, DbType.Int32, entity.Formatminfinplazo);
            dbProvider.AddInParameter(command, helper.Formatnumbloques, DbType.Int32, entity.Formatnumbloques);
            dbProvider.AddInParameter(command, helper.Formatdiafinfueraplazo, DbType.Int32, entity.Formatdiafinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatminfinfueraplazo, DbType.Int32, entity.Formatminfinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatmesplazo, DbType.Int32, entity.Formatmesplazo);
            dbProvider.AddInParameter(command, helper.Formatmesfinplazo, DbType.Int32, entity.Formatmesfinplazo);
            dbProvider.AddInParameter(command, helper.Formatmesfinfueraplazo, DbType.Int32, entity.Formatmesfinfueraplazo);
            dbProvider.AddInParameter(command, helper.Formatdependeconfigptos, DbType.Int32, entity.Formatdependeconfigptos);
            dbProvider.AddInParameter(command, helper.Formatenviocheckadicional, DbType.String, entity.Formatenviocheckadicional);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeFormatoDTO GetById(int formatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            MeFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {

                    entity = helper.Create(dr);

                    int iAreaname = dr.GetOrdinal(helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);

                    int iLecttipo = dr.GetOrdinal(helper.Lecttipo);
                    if (!dr.IsDBNull(iLecttipo)) entity.Lecttipo = Convert.ToInt32(dr.GetValue(iLecttipo));

                    int iFormatnombreOrigen = dr.GetOrdinal(helper.FormatnombreOrigen);
                    if (!dr.IsDBNull(iFormatnombreOrigen)) entity.FormatnombreOrigen = dr.GetString(iFormatnombreOrigen).Trim();

                }
            }

            return entity;
        }

        public List<MeFormatoDTO> List()
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            MeFormatoDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                int iAreaname = dr.GetOrdinal(helper.Areaname);
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);
                    int iLecttipo = dr.GetOrdinal(helper.Lecttipo);
                    if (!dr.IsDBNull(iLecttipo)) entity.Lecttipo = Convert.ToInt32(dr.GetValue(iLecttipo));

                    int iLectnomb = dr.GetOrdinal(helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeFormatoDTO> GetByCriteria(int areaUsuario, int formatcodiOrigen)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, areaUsuario, formatcodiOrigen);
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeFormatoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                int iAreaname = dr.GetOrdinal(helper.Areaname);
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname).Trim();

                    int iFormatnombreOrigen = dr.GetOrdinal(helper.FormatnombreOrigen);
                    if (!dr.IsDBNull(iFormatnombreOrigen)) entity.FormatnombreOrigen = dr.GetString(iFormatnombreOrigen).Trim();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeFormatoDTO> GetByModuloLectura(int idModulo, int idLectura, int idEmpresa)
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            string query = string.Format(helper.SqlGetByModuloLectura, idModulo, idLectura, idEmpresa);
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

        public List<MeFormatoDTO> GetByModuloLecturaMultiple(int idModulo, string lectura, string empresa)
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            string query = string.Format(helper.SqlGetByModuloLecturaMultiple, idModulo, lectura, empresa);
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

        public List<MeFormatoDTO> GetByModulo(int idModulo)
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            string query = string.Format(helper.SqlGetByModulo, idModulo);
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

        public MeFormatoDTO GetByClave(int formatcodi, int emprcodi, DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorClave);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            MeFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAreaname = dr.GetOrdinal(helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);

                    int iLecttipo = dr.GetOrdinal(helper.Lecttipo);
                    if (!dr.IsDBNull(iLecttipo)) entity.Lecttipo = Convert.ToInt32(dr.GetValue(iLecttipo));

                    entity.ListaHoja = new MeHojaRepository(strConexion).GetByCriteria(formatcodi);

                    IEnumerable<MeCabeceraDTO> cabeceras = new MeCabeceraRepository(strConexion).List();

                    if (entity.ListaHoja != null && entity.ListaHoja.Count > 0)
                    {
                        foreach (MeHojaDTO hoja in entity.ListaHoja)
                        {
                            MeCabeceraDTO cabecera = cabeceras.Single(x => x.Cabcodi == hoja.Cabcodi);

                            if (cabecera != null)
                            {
                                hoja.Cabecera = cabecera;
                                hoja.ListaPtos = new MeHojaptomedRepository(strConexion).GetByCriteria2(emprcodi, formatcodi, hoja.Hojacodi, cabecera.Cabquery, fechaPeriodo);
                            }
                        }
                    }
                }
            }

            return entity;
        }

        public List<MeFormatoDTO> GetPendientes(int idModulo, int emprcodi, string fecha)
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            string query = string.Format(helper.SqlGetPendientes, idModulo, emprcodi, fecha);
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

        public List<MeFormatoDTO> ListarFormatoOrigen()
        {
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarFormatoOrigen);
            MeFormatoDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                int iAreaname = dr.GetOrdinal(helper.Areaname);
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
