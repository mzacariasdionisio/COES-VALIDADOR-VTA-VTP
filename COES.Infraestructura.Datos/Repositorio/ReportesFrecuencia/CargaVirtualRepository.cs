using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class CargaVirtualRepository : RepositoryBase, ICargaVirtualRepository
    {

        public CargaVirtualRepository(string strConn) : base(strConn)
        {
        }

        CargaVirtualHelper helper = new CargaVirtualHelper();

        public CargaVirtualDTO GetById(int IdCarga)
        {
            CargaVirtualDTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.IdCarga, DbType.Int32, IdCarga);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }

        public List<CargaVirtualDTO> GetListaCargaVirtual(string FechaInicial, string FechaFinal, string CodEquipo)
        {
            int intCodEquipo = 0;
            if (!string.IsNullOrEmpty(CodEquipo))
            {
                intCodEquipo = Convert.ToInt32(CodEquipo);
            }
            List<CargaVirtualDTO> entitys = new List<CargaVirtualDTO>();
            var query = string.Format(helper.SqlListaCargaVirtual,
                                           FechaInicial,
                                           FechaFinal,
                                           intCodEquipo
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

        public List<LecturaVirtualDTO> GetListaLecturaVirtual(int IdCarga)
        {
            List<LecturaVirtualDTO> entitys = new List<LecturaVirtualDTO>();
            var query = string.Format(helper.SqlListaLecturaVirtual,
                                           IdCarga
                                           );
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateLecturaVirtual(dr));
                }
            }
            return entitys;
        }

        public List<SiEmpresaDTO> GetListaEmpresasCargaVirtual()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaEmpresasCargaVirtual);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEmpresa(dr));
                }
            }
            return entitys;
        }

        public List<CentralDTO> GetListaCentralPorEmpresa(int CodEmpresa)
        {
            List<CentralDTO> entitys = new List<CentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCentralPorEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, CodEmpresa);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateCentral(dr));
                }
            }
            return entitys;
        }
        public List<UnidadDTO> GetListaUnidadPorCentralEmpresa(int CodEmpresa, string Central)
        {
            List<UnidadDTO> entitys = new List<UnidadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaUnidadPorCentralEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, CodEmpresa);
            dbProvider.AddInParameter(command, helper.CentralNomb, DbType.String, Central);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateUnidad(dr));
                }
            }
            return entitys;
        }

        public CargaVirtualDTO SaveUpdate(CargaVirtualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            

            string query = string.Format(helper.SqlSave, id, entity.GPSCodi, entity.TipoCarga, entity.CodEmpresa, entity.CodCentral, entity.CodUnidad, entity.FechaCargaInicio, entity.FechaCargaFin, entity.ArchivoCarga, entity.UsuCarga);
            command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            entity.IdCarga = id;

            

            return entity;
        }


        public CargaVirtualDTO SaveUpdateExterno(CargaVirtualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            string strDataCarga = string.Empty;

            string query = string.Format(helper.SqlSaveExterno, id, entity.GPSCodi, entity.TipoCarga, entity.ArchivoCarga, entity.UsuCarga, strDataCarga);
            command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            LecturaDTO entLectura = new LecturaDTO();
            List<decimal?> listaFrec = new List<decimal?>();

            string queryLectura = string.Empty;

            if (id > 0)
            {
                string[] sresult = entity.DataCarga.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < sresult.Length; i++)
                {
                    if (i >= 0)
                    {
                        string[] sLecturaVirtual = sresult[i].Split(new string[] { ";" }, StringSplitOptions.None);
                        if (!string.IsNullOrEmpty(sLecturaVirtual[0]))
                        {
                            LecturaVirtualDTO entLecturaVirtual = new LecturaVirtualDTO();
                            entLecturaVirtual.IdCarga = id;
                            entLecturaVirtual.Frecuencia = Convert.ToDecimal(sLecturaVirtual[2]);
                            entLecturaVirtual.Tension = 0;
                            entLecturaVirtual.FecHora = sLecturaVirtual[0];
                            //this.SaveLecturaVirtual(entLecturaVirtual);
                            queryLectura = queryLectura + this.SaveLecturaVirtualString(entLecturaVirtual) + ";";

                            string[] sLectura = sLecturaVirtual[0].Split(new string[] { ":" }, StringSplitOptions.None);
                            if (sLectura[2] == "59")
                            {
                                entLectura.H59 = Convert.ToDecimal(sLecturaVirtual[2]);
                                entLectura.GPSCodi = entity.GPSCodi;
                                entLectura.FecHora = sLectura[0] +  ":" + sLectura[1] + ":00";
                                //entLectura.Voltaje = Convert.ToDecimal(sLecturaVirtual[1]);
                                listaFrec.Add(entLectura.H59);

                                entLectura.Maximo = listaFrec.Max();
                                entLectura.Minimo = listaFrec.Min();
                                entLectura.Num = listaFrec.Count();

                                decimal? decMedia = listaFrec.Average();
                                double dblSumDifCuadrado = 0;
                                double dblSumFrecCuadrado = 0;
                                double dblSumValores = 0;
                                foreach (decimal decFrecuencia in listaFrec)
                                {
                                    decimal? decDif = decFrecuencia - decMedia;
                                    double decDifCuadrado = Math.Pow(Convert.ToDouble(decDif), 2);
                                    dblSumDifCuadrado = dblSumDifCuadrado + decDifCuadrado;

                                    double dblFrecCuadrado = Math.Pow(Convert.ToDouble(decFrecuencia), 2);
                                    dblSumFrecCuadrado = dblSumFrecCuadrado + dblFrecCuadrado;
                                    dblSumValores = dblSumValores + Convert.ToDouble(decFrecuencia);
                                }
                                dblSumValores = (dblSumValores - (entLectura.Num * 60)) / 60;


                                double dblDesvStandar = Math.Sqrt(dblSumDifCuadrado / entLectura.Num);
                                double dblVSF = (Math.Sqrt(dblSumFrecCuadrado / entLectura.Num) - 60);

                                entLectura.Desv = Convert.ToDecimal(dblSumValores);
                                entLectura.VSF = Convert.ToDecimal(dblVSF);

                                //this.SaveLectura(entLectura);
                                queryLectura = queryLectura + this.SaveLecturaString(entLectura) + ";";
                            }
                            else if (sLectura[2] == "58")
                            {
                                entLectura.H58 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H58);
                            }
                            else if (sLectura[2] == "57")
                            {
                                entLectura.H57 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H57);
                            }
                            else if (sLectura[2] == "56")
                            {
                                entLectura.H56 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H56);
                            }
                            else if (sLectura[2] == "55")
                            {
                                entLectura.H55 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H55);
                            }
                            else if (sLectura[2] == "54")
                            {
                                entLectura.H54 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H54);
                            }
                            else if (sLectura[2] == "53")
                            {
                                entLectura.H53 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H53);
                            }
                            else if (sLectura[2] == "52")
                            {
                                entLectura.H52 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H52);
                            }
                            else if (sLectura[2] == "51")
                            {
                                entLectura.H51 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H51);
                            }
                            else if (sLectura[2] == "50")
                            {
                                entLectura.H50 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H50);
                            }
                            else if (sLectura[2] == "49")
                            {
                                entLectura.H49 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H49);
                            }
                            else if (sLectura[2] == "48")
                            {
                                entLectura.H48 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H48);
                            }
                            else if (sLectura[2] == "47")
                            {
                                entLectura.H47 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H47);
                            }
                            else if (sLectura[2] == "46")
                            {
                                entLectura.H46 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H46);
                            }
                            else if (sLectura[2] == "45")
                            {
                                entLectura.H45 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H45);
                            }
                            else if (sLectura[2] == "44")
                            {
                                entLectura.H44 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H44);
                            }
                            else if (sLectura[2] == "43")
                            {
                                entLectura.H43 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H43);
                            }
                            else if (sLectura[2] == "42")
                            {
                                entLectura.H42 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H42);
                            }
                            else if (sLectura[2] == "41")
                            {
                                entLectura.H41 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H41);
                            }
                            else if (sLectura[2] == "40")
                            {
                                entLectura.H40 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H40);
                            }
                            else if (sLectura[2] == "39")
                            {
                                entLectura.H39 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H39);
                            }
                            else if (sLectura[2] == "38")
                            {
                                entLectura.H38 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H38);
                            }
                            else if (sLectura[2] == "37")
                            {
                                entLectura.H37 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H37);
                            }
                            else if (sLectura[2] == "36")
                            {
                                entLectura.H36 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H36);
                            }
                            else if (sLectura[2] == "35")
                            {
                                entLectura.H35 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H35);
                            }
                            else if (sLectura[2] == "34")
                            {
                                entLectura.H34 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H34);
                            }
                            else if (sLectura[2] == "33")
                            {
                                entLectura.H33 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H33);
                            }
                            else if (sLectura[2] == "32")
                            {
                                entLectura.H32 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H32);
                            }
                            else if (sLectura[2] == "31")
                            {
                                entLectura.H31 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H31);
                            }
                            else if (sLectura[2] == "30")
                            {
                                entLectura.H30 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H30);
                            }
                            else if (sLectura[2] == "29")
                            {
                                entLectura.H29 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H29);
                            }
                            else if (sLectura[2] == "28")
                            {
                                entLectura.H28 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H28);
                            }
                            else if (sLectura[2] == "27")
                            {
                                entLectura.H27 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H27);
                            }
                            else if (sLectura[2] == "26")
                            {
                                entLectura.H26 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H26);
                            }
                            else if (sLectura[2] == "25")
                            {
                                entLectura.H25 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H25);
                            }
                            else if (sLectura[2] == "24")
                            {
                                entLectura.H24 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H24);
                            }
                            else if (sLectura[2] == "23")
                            {
                                entLectura.H23 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H23);
                            }
                            else if (sLectura[2] == "22")
                            {
                                entLectura.H22 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H22);
                            }
                            else if (sLectura[2] == "21")
                            {
                                entLectura.H21 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H21);
                            }
                            else if (sLectura[2] == "20")
                            {
                                entLectura.H20 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H20);
                            }
                            else if (sLectura[2] == "19")
                            {
                                entLectura.H19 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H19);
                            }
                            else if (sLectura[2] == "18")
                            {
                                entLectura.H18 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H18);
                            }
                            else if (sLectura[2] == "17")
                            {
                                entLectura.H17 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H17);
                            }
                            else if (sLectura[2] == "16")
                            {
                                entLectura.H16 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H16);
                            }
                            else if (sLectura[2] == "15")
                            {
                                entLectura.H15 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H15);
                            }
                            else if (sLectura[2] == "14")
                            {
                                entLectura.H14 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H14);
                            }
                            else if (sLectura[2] == "13")
                            {
                                entLectura.H13 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H13);
                            }
                            else if (sLectura[2] == "12")
                            {
                                entLectura.H12 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H12);
                            }
                            else if (sLectura[2] == "11")
                            {
                                entLectura.H11 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H11);
                            }
                            else if (sLectura[2] == "10")
                            {
                                entLectura.H10 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H10);
                            }
                            else if (sLectura[2] == "09")
                            {
                                entLectura.H9 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H9);
                            }
                            else if (sLectura[2] == "08")
                            {
                                entLectura.H8 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H8);
                            }
                            else if (sLectura[2] == "07")
                            {
                                entLectura.H7 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H7);
                            }
                            else if (sLectura[2] == "06")
                            {
                                entLectura.H6 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H6);
                            }
                            else if (sLectura[2] == "05")
                            {
                                entLectura.H5 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H5);
                            }
                            else if (sLectura[2] == "04")
                            {
                                entLectura.H4 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H4);
                            }
                            else if (sLectura[2] == "03")
                            {
                                entLectura.H3 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H3);
                            }
                            else if (sLectura[2] == "02")
                            {
                                entLectura.H2 = Convert.ToDecimal(sLecturaVirtual[2]);
                                listaFrec.Add(entLectura.H2);
                            }
                            else if (sLectura[2] == "01")
                            {
                                entLectura.H1 = Convert.ToDecimal(sLecturaVirtual[2]);
                                entLectura.GPSCodi = entity.GPSCodi;
                                //entLectura.FecHora = sLecturaVirtual[0];
                                entLectura.Voltaje = 0;

                                listaFrec.Add(entLectura.H1);
                            }
                            else if (sLectura[2] == "00")
                            {
                                entLectura = new LecturaDTO();
                                entLectura.H0 = Convert.ToDecimal(sLecturaVirtual[2]);
                                entLectura.GPSCodi = entity.GPSCodi;
                                entLectura.FecHora = sLecturaVirtual[0];
                                entLectura.Voltaje = 0;

                                listaFrec = new List<decimal?>();
                                listaFrec.Add(entLectura.H0);
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

            return entity;
        }

        public LecturaVirtualDTO SaveLecturaVirtual(LecturaVirtualDTO entity)
        {
            string query = string.Format(helper.SqlSaveLecturaVirtual, entity.IdCarga, entity.FecHora, entity.Frecuencia, entity.Tension);
            DbCommand  command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            return entity;
        }

        public string SaveLecturaVirtualString(LecturaVirtualDTO entity)
        {
            string query = string.Format(helper.SqlSaveLecturaVirtual, entity.IdCarga, entity.FecHora, entity.Frecuencia, entity.Tension);
            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //object idMsg = dbProvider.ExecuteReader(command);
            return query;
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

        public LecturaDTO SaveLectura(LecturaDTO entity)
        {
            string query = string.Format(helper.SqlSaveLectura, entity.FecHora, entity.GPSCodi, entity.VSF, entity.Maximo, entity.Minimo, entity.Voltaje, entity.Num, entity.Desv, entity.H0, entity.H1, entity.H2, entity.H3, entity.H4, entity.H5, entity.H6, entity.H7, entity.H8, entity.H9, entity.H10, entity.H11, entity.H12, entity.H13, entity.H14, entity.H15, entity.H16, entity.H17, entity.H18, entity.H19, entity.H20, entity.H21, entity.H22, entity.H23, entity.H24, entity.H25, entity.H26, entity.H27, entity.H28, entity.H29, entity.H30, entity.H31, entity.H32, entity.H33, entity.H34, entity.H35, entity.H36, entity.H37, entity.H38, entity.H39, entity.H40, entity.H41, entity.H42, entity.H43, entity.H44, entity.H45, entity.H46, entity.H47, entity.H48, entity.H49, entity.H50, entity.H51, entity.H52, entity.H53, entity.H54, entity.H55, entity.H56, entity.H57, entity.H58, entity.H59, entity.DevSec);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object idMsg = dbProvider.ExecuteReader(command);
            return entity;
        }

        public string SaveLecturaString(LecturaDTO entity)
        {
            string query = string.Format(helper.SqlSaveLectura, entity.FecHora, entity.GPSCodi, entity.VSF, entity.Maximo, entity.Minimo, entity.Voltaje, entity.Num, entity.Desv, entity.H0, entity.H1, entity.H2, entity.H3, entity.H4, entity.H5, entity.H6, entity.H7, entity.H8, entity.H9, entity.H10, entity.H11, entity.H12, entity.H13, entity.H14, entity.H15, entity.H16, entity.H17, entity.H18, entity.H19, entity.H20, entity.H21, entity.H22, entity.H23, entity.H24, entity.H25, entity.H26, entity.H27, entity.H28, entity.H29, entity.H30, entity.H31, entity.H32, entity.H33, entity.H34, entity.H35, entity.H36, entity.H37, entity.H38, entity.H39, entity.H40, entity.H41, entity.H42, entity.H43, entity.H44, entity.H45, entity.H46, entity.H47, entity.H48, entity.H49, entity.H50, entity.H51, entity.H52, entity.H53, entity.H54, entity.H55, entity.H56, entity.H57, entity.H58, entity.H59, entity.DevSec);
            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //object idMsg = dbProvider.ExecuteReader(command);
            return query;
        }

        
    }
}
