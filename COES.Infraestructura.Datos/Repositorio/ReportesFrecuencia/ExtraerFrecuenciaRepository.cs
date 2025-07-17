using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System.IO;
using System.Configuration;
using COES.Framework.Base.Tools;


namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class ExtraerFrecuenciaRepository : RepositoryBase, IExtraerFrecuenciaRepository
    {
        public ExtraerFrecuenciaRepository(string strConn) : base(strConn)
        {
        }

        ExtraerFrecuenciaHelper helper = new ExtraerFrecuenciaHelper();
        public static string UserLogin = ConfigurationManager.AppSettings["UserFrecuencia"];
        public static string Domain = ConfigurationManager.AppSettings["DomainFrecuencia"];
        public static string Password = ConfigurationManager.AppSettings["PasswordFrecuencia"];

        public ExtraerFrecuenciaDTO GetById(int IdCarga)
        {
            ExtraerFrecuenciaDTO entitys = null;
            var query = string.Format(helper.SqlGetById,
                                          IdCarga
                                          );
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }

        public List<ExtraerFrecuenciaDTO> GetListaExtraerFrecuencia(string FechaInicial, string FechaFinal)
        {
            List<ExtraerFrecuenciaDTO> entitys = new List<ExtraerFrecuenciaDTO>();
            var query = string.Format(helper.SqlListaExtraerFrecuencia,
                                          FechaInicial,
                                          FechaFinal
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

        public List<LecturaVirtualDTO> GetListaMilisegundos(int IdCarga)
        {
            List<LecturaVirtualDTO> entitys = new List<LecturaVirtualDTO>();
            var query = string.Format(helper.SqlListaMilisegundos, IdCarga);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateMiliseg(dr));
                }
            }
            return entitys;
        }

        public ExtraerFrecuenciaDTO SaveUpdate(ExtraerFrecuenciaDTO entity)
        {
            //try
            //{
                int numRegistros = 0;
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                int id = 1;
                if (result != null) id = Convert.ToInt32(result);

                var query = string.Format(helper.SqlSave, id,
                                                entity.GPSCodi,
                                                entity.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                                                entity.FechaHoraFin.ToString("dd/MM/yyyy HH:mm:ss"),
                                                entity.ArchivoCarga,
                                                entity.UsuCreacion
                                                );

                DbCommand command2 = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command2);

                string strRutaFile = string.Empty;
                EquipoGPSDTO equipoGPS = new EquipoGPSDTO();
                equipoGPS = this.GetEquipoById(entity.GPSCodi);
                if (equipoGPS != null)
                {
                    strRutaFile = equipoGPS.RutaFile;
                    if (!string.IsNullOrEmpty(strRutaFile))
                    {
                        string strCodEquipoGPS = entity.GPSCodi.ToString("D3");
                        string strAnio = entity.FechaHoraInicio.ToString("yyyy");
                        string strMes = entity.FechaHoraInicio.ToString("MM");
                        string strDia = entity.FechaHoraInicio.ToString("dd");
                        string path = strRutaFile + "a" + strAnio + "m" + strMes + "/Fv02_" + strCodEquipoGPS + "_" + strAnio + strMes + strDia + ".dat";
                        //string text = FileServer.ReadFileFromDirectory(strRutaFile, path);
                        //entity.DataCarga = text;
                        using (new Impersonator(UserLogin, Domain, Password, strRutaFile))
                        {
                            //string path = LocalDirectory + url;
                            if (Directory.Exists(strRutaFile))
                            {
                                //string file = path + targetBlobName;
                                if (File.Exists(path))
                                {
                                    string text = System.IO.File.ReadAllText(path);
                                    entity.DataCarga = text;
                                }

                            }

                        }


                    }
                }


                if (id > 0)
                {
                string[] sresult = entity.DataCarga.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int intAnio = Convert.ToInt32(entity.FechaHoraInicio.ToString("yyyy"));
                DateTime dt = new DateTime(day: 1, month: 1, year: intAnio, hour: 0, minute: 0, second: 0);
                TimeSpan tsFechaHoraInicio = entity.FechaHoraInicio - dt;
                var difSecondsInicio = tsFechaHoraInicio.TotalSeconds;
                difSecondsInicio = difSecondsInicio + 18000;//Se resta 5 horas -5GMT: 5*60*60

                TimeSpan tsFechaHoraFin = entity.FechaHoraFin - dt;
                var difSecondsFin = tsFechaHoraFin.TotalSeconds;
                difSecondsFin = difSecondsFin + 18000;
                string queryLectura = string.Empty;
                int contadorMilisegundos = 0;


                for (int i = 0; i < sresult.Length; i++)
                {
                    string[] sLecturaSegundos = sresult[i].Split(new string[] { "|" }, StringSplitOptions.None);
                    if (sLecturaSegundos.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(sLecturaSegundos[0]))
                        {
                            int intValorSegundosInicial = Convert.ToInt32(sLecturaSegundos[0]);
                            int intValorSegundos = 0;

                            decimal decValorTension = Convert.ToDecimal(sLecturaSegundos[5]);
                            if ((intValorSegundosInicial >= difSecondsInicio) && (intValorSegundosInicial <= difSecondsFin))
                            {
                                for (int j = 0; j < sLecturaSegundos.Length; j++)
                                {
                                    if (j > 5)
                                    {
                                    numRegistros++;
                                        contadorMilisegundos++;
                                        try
                                        {
                                            
                                            string[] sLecturaMiliseg = sLecturaSegundos[j].Split(new string[] { ":" }, StringSplitOptions.None);
                                            LecturaVirtualDTO entLecturaVirtual = new LecturaVirtualDTO();
                                            entLecturaVirtual.IdCarga = id;
                                            entLecturaVirtual.Frecuencia = Convert.ToDecimal(sLecturaMiliseg[1]) + 60;
                                            intValorSegundos = intValorSegundosInicial - 18000;
                                            DateTime dtFecha = dt.AddSeconds(intValorSegundos);
                                            DateTime dtFechaFinal = dtFecha.AddMilliseconds(Convert.ToInt32(sLecturaMiliseg[0]));
                                            entLecturaVirtual.FecHora = dtFechaFinal.ToString("dd-MM-yyyy HH:mm:ss");
                                            entLecturaVirtual.Miliseg = Convert.ToInt32(sLecturaMiliseg[0]);
                                            entLecturaVirtual.Tension = Convert.ToDecimal(decValorTension);
                                            //this.SaveLecturaMiliseg(entLecturaVirtual);
                                            queryLectura = queryLectura + this.SaveLecturaMilisegString(entLecturaVirtual) + ";";
                                        }
                                        catch (Exception ex)
                                        {
                                            entity.Mensaje = ex.Message;
                                        }

                                        
                                    }
                                }
                                if (contadorMilisegundos > 3600) {
                                    //Grabar Query String
                                    if (!string.IsNullOrEmpty(queryLectura))
                                    {
                                        this.SaveLecturaQuery(queryLectura);
                                        queryLectura = string.Empty;
                                    }
                                    contadorMilisegundos = 0;
                                }

                            }

                            
                        }

                    }


                }

                //Grabar Query String
                if (!string.IsNullOrEmpty(queryLectura))
                {
                    this.SaveLecturaQuery(queryLectura);
                }

            }
                if (numRegistros==0)
                {
                    entity.Mensaje = "El archivo no contiene informacion de registros de milisegundos.";
                }
                entity.DataCarga = string.Empty;
                return entity;
            //}
            //catch (Exception ex)
            //{
            //    entity.Mensaje = ex.Message;
            //    return entity;
            //}
            

            
        }

        public int SaveLecturaQuery(string query)
        {
            try
            {
                string queryFinal = "begin\n\n";
                queryFinal = queryFinal + query + "\n\n";
                queryFinal = queryFinal + "end;";

                DbCommand command = dbProvider.GetSqlStringCommand(queryFinal);
                dbProvider.ExecuteReader(command);

                return 1;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message, ex);
                return 0;
            }

        }

        public EquipoGPSDTO GetEquipoById(int GPSCodi)
        {
            EquipoGPSDTO entitys = null;
            var query = string.Format(helper.SqlGetEquipoGPS, GPSCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.CreateEquipoGPS(dr);
                }
            }
            return entitys;
        }

        public LecturaVirtualDTO SaveLecturaMiliseg(LecturaVirtualDTO entity)
        {
            string query = string.Format(helper.SqlSaveLecturaMiliseg, entity.IdCarga, entity.FecHora, entity.Frecuencia, entity.Miliseg, entity.Tension);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            return entity;
        }

        public string SaveLecturaMilisegString(LecturaVirtualDTO entity)
        {
            string query = string.Format(helper.SqlSaveLecturaMiliseg, entity.IdCarga, entity.FecHora, entity.Frecuencia, entity.Miliseg, entity.Tension);
            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //object idMsg = dbProvider.ExecuteReader(command);
            return query;
        }



    }
}
