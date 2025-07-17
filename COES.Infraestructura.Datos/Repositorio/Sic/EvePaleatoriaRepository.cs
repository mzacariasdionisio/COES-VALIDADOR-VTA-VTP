using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EVE_PALEATORIA
    /// </summary>
    public class EvePaleatoriaRepository: RepositoryBase, IEvePaleatoriaRepository
    {
        public EvePaleatoriaRepository(string strConn): base(strConn)
        {
        }

        EvePaleatoriaHelper helper = new EvePaleatoriaHelper();
        SiPersonaHelper helperPersona = new SiPersonaHelper();
        
        public void Save(EvePaleatoriaDTO entity)
        {
            DbCommand command;

            
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pafecha, DbType.DateTime, entity.Pafecha);
            dbProvider.AddInParameter(command, helper.Sic2hop, DbType.String, entity.Sic2hop);
            dbProvider.AddInParameter(command, helper.Hop2ut30d, DbType.String, entity.Hop2ut30d);
            dbProvider.AddInParameter(command, helper.Ut30d2sort, DbType.String, entity.Ut30d2sort);
            dbProvider.AddInParameter(command, helper.Sort2prue, DbType.String, entity.Sort2prue);
            dbProvider.AddInParameter(command, helper.Prueno2pa, DbType.String, entity.Prueno2pa);
            dbProvider.AddInParameter(command, helper.Pa2fin, DbType.String, entity.Pa2fin);
            dbProvider.AddInParameter(command, helper.Pruesi2gprue, DbType.String, entity.Pruesi2gprue);
            dbProvider.AddInParameter(command, helper.Gprueno2nprue, DbType.String, entity.Gprueno2nprue);
            dbProvider.AddInParameter(command, helper.Nprue2fin, DbType.String, entity.Nprue2fin);
            dbProvider.AddInParameter(command, helper.Gpruesi2uprue, DbType.String, entity.Gpruesi2uprue);
            dbProvider.AddInParameter(command, helper.Uprue2rprue, DbType.String, entity.Uprue2rprue);
            dbProvider.AddInParameter(command, helper.Rprue2oa, DbType.String, entity.Rprue2oa);
            dbProvider.AddInParameter(command, helper.Oa2priarr, DbType.String, entity.Oa2priarr);
            dbProvider.AddInParameter(command, helper.Priarrsi2exit, DbType.String, entity.Priarrsi2exit);
            dbProvider.AddInParameter(command, helper.Priarrno2rearr, DbType.String, entity.Priarrno2rearr);
            dbProvider.AddInParameter(command, helper.Rearrno2noexit, DbType.String, entity.Rearrno2noexit);
            dbProvider.AddInParameter(command, helper.Rearrsi2segarr, DbType.String, entity.Rearrsi2segarr);
            dbProvider.AddInParameter(command, helper.Segarrno2noexit, DbType.String, entity.Segarrno2noexit);
            dbProvider.AddInParameter(command, helper.Segarrsi2exit, DbType.String, entity.Segarrsi2exit);
            dbProvider.AddInParameter(command, helper.Exitno2fallunid, DbType.String, entity.Exitno2fallunid);
            dbProvider.AddInParameter(command, helper.Fallunidsi2noexit, DbType.String, entity.Fallunidsi2noexit);
            dbProvider.AddInParameter(command, helper.Exitsi2resprue, DbType.String, entity.Exitsi2resprue);
            dbProvider.AddInParameter(command, helper.Fallunidno2pabort, DbType.String, entity.Fallunidno2pabort);
            dbProvider.AddInParameter(command, helper.Pabort2resprue, DbType.String, entity.Pabort2resprue);
            dbProvider.AddInParameter(command, helper.Resprue2fin, DbType.String, entity.Resprue2fin);
            dbProvider.AddInParameter(command, helper.Noexit2resreslt, DbType.String, entity.Noexit2resreslt);
            dbProvider.AddInParameter(command, helper.Resreslt2fin, DbType.String, entity.Resreslt2fin);
            dbProvider.AddInParameter(command, helper.Final, DbType.String, entity.Final);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Programador, DbType.String, entity.Programador);
            dbProvider.AddInParameter(command, helper.Paobservacion, DbType.String, entity.Paobservacion);
            dbProvider.AddInParameter(command, helper.Paverifdatosing, DbType.String, entity.Paverifdatosing);



            dbProvider.ExecuteNonQuery(command);
            return;
        }


        

        public void Update(EvePaleatoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Sic2hop, DbType.String, entity.Sic2hop);
            dbProvider.AddInParameter(command, helper.Hop2ut30d, DbType.String, entity.Hop2ut30d);
            dbProvider.AddInParameter(command, helper.Ut30d2sort, DbType.String, entity.Ut30d2sort);
            dbProvider.AddInParameter(command, helper.Sort2prue, DbType.String, entity.Sort2prue);
            dbProvider.AddInParameter(command, helper.Prueno2pa, DbType.String, entity.Prueno2pa);
            dbProvider.AddInParameter(command, helper.Pa2fin, DbType.String, entity.Pa2fin);
            dbProvider.AddInParameter(command, helper.Pruesi2gprue, DbType.String, entity.Pruesi2gprue);
            dbProvider.AddInParameter(command, helper.Gprueno2nprue, DbType.String, entity.Gprueno2nprue);
            dbProvider.AddInParameter(command, helper.Nprue2fin, DbType.String, entity.Nprue2fin);
            dbProvider.AddInParameter(command, helper.Gpruesi2uprue, DbType.String, entity.Gpruesi2uprue);
            dbProvider.AddInParameter(command, helper.Uprue2rprue, DbType.String, entity.Uprue2rprue);
            dbProvider.AddInParameter(command, helper.Rprue2oa, DbType.String, entity.Rprue2oa);
            dbProvider.AddInParameter(command, helper.Oa2priarr, DbType.String, entity.Oa2priarr);
            dbProvider.AddInParameter(command, helper.Priarrsi2exit, DbType.String, entity.Priarrsi2exit);
            dbProvider.AddInParameter(command, helper.Priarrno2rearr, DbType.String, entity.Priarrno2rearr);
            dbProvider.AddInParameter(command, helper.Rearrno2noexit, DbType.String, entity.Rearrno2noexit);
            dbProvider.AddInParameter(command, helper.Rearrsi2segarr, DbType.String, entity.Rearrsi2segarr);
            dbProvider.AddInParameter(command, helper.Segarrno2noexit, DbType.String, entity.Segarrno2noexit);
            dbProvider.AddInParameter(command, helper.Segarrsi2exit, DbType.String, entity.Segarrsi2exit);
            dbProvider.AddInParameter(command, helper.Exitno2fallunid, DbType.String, entity.Exitno2fallunid);
            dbProvider.AddInParameter(command, helper.Fallunidsi2noexit, DbType.String, entity.Fallunidsi2noexit);
            dbProvider.AddInParameter(command, helper.Exitsi2resprue, DbType.String, entity.Exitsi2resprue);
            dbProvider.AddInParameter(command, helper.Fallunidno2pabort, DbType.String, entity.Fallunidno2pabort);
            dbProvider.AddInParameter(command, helper.Pabort2resprue, DbType.String, entity.Pabort2resprue);
            dbProvider.AddInParameter(command, helper.Resprue2fin, DbType.String, entity.Resprue2fin);
            dbProvider.AddInParameter(command, helper.Noexit2resreslt, DbType.String, entity.Noexit2resreslt);
            dbProvider.AddInParameter(command, helper.Resreslt2fin, DbType.String, entity.Resreslt2fin);
            dbProvider.AddInParameter(command, helper.Final, DbType.String, entity.Final);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Programador, DbType.String, entity.Programador);
            dbProvider.AddInParameter(command, helper.Paobservacion, DbType.String, entity.Paobservacion);
            dbProvider.AddInParameter(command, helper.Paverifdatosing, DbType.String, entity.Paverifdatosing);

            dbProvider.AddInParameter(command, helper.Pafecha, DbType.DateTime, entity.Pafecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime pafecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pafecha, DbType.DateTime, pafecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public EvePaleatoriaDTO GetById(DateTime pafecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pafecha, DbType.DateTime, pafecha);
            EvePaleatoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EvePaleatoriaDTO> List()
        {
            List<EvePaleatoriaDTO> entitys = new List<EvePaleatoriaDTO>();
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

        public List<EvePaleatoriaDTO> GetByCriteria()
        {
            List<EvePaleatoriaDTO> entitys = new List<EvePaleatoriaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiPersonaDTO> ListProgramador()
        {
            List<SiPersonaDTO> entitys = new List<SiPersonaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helperPersona.SqlListaProgramador);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helperPersona.CreateLista(dr));
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EvePaleatoriaDTO> entitys = new List<EvePaleatoriaDTO>();
            String sql = String.Format(this.helper.TotalRegistros, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;


        }

        public List<EvePaleatoriaDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<EvePaleatoriaDTO> entitys = new List<EvePaleatoriaDTO>();
            String sql = String.Format(this.helper.ObtenerListado, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EvePaleatoriaDTO entity = new EvePaleatoriaDTO();


                    int iPafecha = dr.GetOrdinal(this.helper.Pafecha);
                    if (!dr.IsDBNull(iPafecha)) entity.Pafecha = dr.GetDateTime(iPafecha);


                    int iResultado = dr.GetOrdinal(this.helper.Resultado);
                    if (!dr.IsDBNull(iResultado)) entity.Resultado = dr.GetString(iResultado);

                    int iPruebaExitosa = dr.GetOrdinal(this.helper.PruebaExitosa);
                    if (!dr.IsDBNull(iPruebaExitosa)) entity.PruebaExitosa = dr.GetString(iPruebaExitosa);

                    int iPrimerIntentoExitoso = dr.GetOrdinal(this.helper.PrimerIntentoExitoso);
                    if (!dr.IsDBNull(iPrimerIntentoExitoso)) entity.PrimerIntentoExitoso = dr.GetString(iPrimerIntentoExitoso);

                    int iSegundoIntentoExitoso = dr.GetOrdinal(this.helper.SegundoIntentoExitoso);
                    if (!dr.IsDBNull(iSegundoIntentoExitoso)) entity.SegundoIntentoExitoso = dr.GetString(iSegundoIntentoExitoso);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    entitys.Add(entity);


                }
            }

            return entitys;

        }


        public string ProgramadorPrueba(DateTime Fecha)
        {

            String sql = String.Format(helper.ProgramadorPrueba,
                Fecha.ToString(ConstantesBase.FormatoFecha));


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object nombre = dbProvider.ExecuteScalar(command);

            if (nombre != null) return Convert.ToString(nombre);

            return "";
        }

        public List<string> ListaCoordinadores()
        {
            List<string> entitys = new List<string>();
            DbCommand command = dbProvider.GetSqlStringCommand(helperPersona.SqlListaCoordinadores);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helperPersona.CreateListaCoordinadores(dr));
                }
            }

            return entitys;
        }



    }
}
