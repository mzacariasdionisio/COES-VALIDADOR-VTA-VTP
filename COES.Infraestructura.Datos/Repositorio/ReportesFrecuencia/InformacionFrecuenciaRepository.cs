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
    public class InformacionFrecuenciaRepository: RepositoryBase, IInformacionFrecuenciaRepository
    {
        public InformacionFrecuenciaRepository(string strConn) : base(strConn)
        {
        }

        InformacionFrecuenciaHelper helper = new InformacionFrecuenciaHelper();

        

        public List<InformacionFrecuenciaDTO> GetReporteFrecuenciaDesviacion()
        {
            List<LecturaDTO> entitys = new List<LecturaDTO>();
            var query = string.Format(helper.SqlReporteFrecuenciaDesviacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<string> listaFechas = new List<string>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateLectura(dr));
                }
            }

            List<InformacionFrecuenciaDTO> listDiferencias = new List<InformacionFrecuenciaDTO>();

            if (entitys.Count>0)
            {
                foreach (var item in entitys)
                {
                    string strFechaHora = item.FechaHoraString;
                    if (!listaFechas.Contains(strFechaHora))
                    {
                        listaFechas.Add(strFechaHora);
                    }
                }

                foreach (string FechaHora in listaFechas)
                {
                    List<LecturaDTO> listLecturas = new List<LecturaDTO>();
                    listLecturas = entitys.Where(x => x.FechaHoraString == FechaHora).ToList();
                    if (listLecturas!=null)
                    {
                        for (int numLecturaFechaHora = 0; numLecturaFechaHora < listLecturas.Count-1; numLecturaFechaHora++)
                        {
                            double decH0 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H0);
                            double decH0Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora+1].H0);
                            double decDiferenciaH0 = decH0 - decH0Comparar;
                            if (decH0 > 0 && decH0Comparar > 0)
                            {
                                if (decDiferenciaH0 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H0.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H0.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH0;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH0 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H0.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H0.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH0;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }                               

                            double decH1 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H1);
                            double decH1Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H1);
                            double decDiferenciaH1 = decH1 - decH1Comparar;
                            if (decH1 > 0 && decH1Comparar > 0)
                            {
                                if (decDiferenciaH1 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(1).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H1.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H1.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH1;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH1 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(1).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H1.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H1.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH1;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH2 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H2);
                            double decH2Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H2);
                            double decDiferenciaH2 = decH2 - decH2Comparar;
                            if (decH2 > 0 && decH2Comparar > 0)
                            {
                                if (decDiferenciaH2 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(2).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H2.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H2.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH2;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH2 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(2).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H2.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H2.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH2;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH3 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H3);
                            double decH3Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H3);
                            double decDiferenciaH3 = decH3 - decH3Comparar;
                            if (decH3 > 0 && decH3Comparar > 0)
                            {
                                if (decDiferenciaH3 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(3).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H3.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H3.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH3;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH3 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(3).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H3.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H3.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH3;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH4 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H4);
                            double decH4Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H4);
                            double decDiferenciaH4 = decH4 - decH4Comparar;
                            if (decH4 > 0 && decH4Comparar > 0)
                            {
                                if (decDiferenciaH4 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(4).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H4.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H4.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH4;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH4 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(4).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H4.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H4.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH4;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH5 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H5);
                            double decH5Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H5);
                            double decDiferenciaH5 = decH5 - decH5Comparar;
                            if (decH5 > 0 && decH5Comparar > 0)
                            {
                                if (decDiferenciaH5 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(5).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H5.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H5.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH5;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH5 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(5).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H5.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H5.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH5;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH6 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H6);
                            double decH6Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H6);
                            double decDiferenciaH6 = decH6 - decH6Comparar;
                            if (decH6 > 0 && decH6Comparar > 0)
                            {
                                if (decDiferenciaH6 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(6).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H6.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H6.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH6;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH6 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(6).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H6.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H6.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH6;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH7 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H7);
                            double decH7Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H7);
                            double decDiferenciaH7 = decH7 - decH7Comparar;
                            if (decH7 > 0 && decH7Comparar > 0)
                            {
                                if (decDiferenciaH7 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(7).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H7.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H7.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH7;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH7 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(7).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H7.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H7.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH7;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH8 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H8);
                            double decH8Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H8);
                            double decDiferenciaH8 = decH8 - decH8Comparar;
                            if (decH8 > 0 && decH8Comparar > 0)
                            {
                                if (decDiferenciaH8 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(8).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H8.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H8.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH8;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH8 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(8).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H8.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H8.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH8;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH9 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H9);
                            double decH9Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H9);
                            double decDiferenciaH9 = decH9 - decH9Comparar;
                            if (decH9 > 0 && decH9Comparar > 0)
                            {
                                if (decDiferenciaH9 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(9).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H9.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H9.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH9;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH9 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(9).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H9.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H9.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH9;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH10 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H10);
                            double decH10Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H10);
                            double decDiferenciaH10 = decH10 - decH10Comparar;
                            if (decH10 > 0 && decH10Comparar > 0)
                            {
                                if (decDiferenciaH10 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(10).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H10.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H10.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH10;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH10 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(10).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H10.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H10.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH10;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH11 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H11);
                            double decH11Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H11);
                            double decDiferenciaH11 = decH11 - decH11Comparar;
                            if (decH11 > 0 && decH11Comparar > 0)
                            {
                                if (decDiferenciaH11 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(11).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H11.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H11.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH11;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH11 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(11).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H11.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H11.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH11;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH12 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H12);
                            double decH12Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H12);
                            double decDiferenciaH12 = decH12 - decH12Comparar;
                            if (decH12 > 0 && decH12Comparar > 0)
                            {
                                if (decDiferenciaH12 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(12).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H12.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H12.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH12;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH12 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(12).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H12.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H12.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH12;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH13 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H13);
                            double decH13Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H13);
                            double decDiferenciaH13 = decH13 - decH13Comparar;
                            if (decH13 > 0 && decH13Comparar > 0)
                            {
                                if (decDiferenciaH13 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(13).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H13.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H13.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH13;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH13 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(13).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H13.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H13.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH13;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH14 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H14);
                            double decH14Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H14);
                            double decDiferenciaH14 = decH14 - decH14Comparar;
                            if (decH14 > 0 && decH14Comparar > 0)
                            {
                                if (decDiferenciaH14 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(14).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H14.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H14.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH14;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH14 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(14).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H14.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H14.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH14;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH15 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H15);
                            double decH15Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H15);
                            double decDiferenciaH15 = decH15 - decH15Comparar;
                            if (decH15 > 0 && decH15Comparar > 0)
                            {
                                if (decDiferenciaH15 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(15).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H15.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H15.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH15;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH15 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(15).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H15.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H15.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH15;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH16 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H16);
                            double decH16Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H16);
                            double decDiferenciaH16 = decH16 - decH16Comparar;
                            if (decH16 > 0 && decH16Comparar > 0)
                            {
                                if (decDiferenciaH16 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(16).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H16.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H16.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH16;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH16 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(16).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H16.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H16.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH16;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH17 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H17);
                            double decH17Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H17);
                            double decDiferenciaH17 = decH17 - decH17Comparar;
                            if (decH17 > 0 && decH17Comparar > 0)
                            {
                                if (decDiferenciaH17 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(17).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H17.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H17.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH17;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH17 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(17).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H17.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H17.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH17;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH18 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H18);
                            double decH18Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H18);
                            double decDiferenciaH18 = decH18 - decH18Comparar;
                            if (decH18 > 0 && decH18Comparar > 0)
                            {
                                if (decDiferenciaH18 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(18).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H18.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H18.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH18;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH18 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(18).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H18.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H18.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH18;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH19 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H19);
                            double decH19Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H19);
                            double decDiferenciaH19 = decH19 - decH19Comparar; 
                            if (decH19 > 0 && decH19Comparar > 0)
                            {
                                if (decDiferenciaH19 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(19).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H19.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H19.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH19;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH19 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(19).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H19.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H19.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH19;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH20 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H20);
                            double decH20Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H20);
                            double decDiferenciaH20 = decH20 - decH20Comparar;
                            if (decH20 > 0 && decH20Comparar > 0)
                            {
                                if (decDiferenciaH20 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(20).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H20.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H20.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH20;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH20 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(20).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H20.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H20.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH20;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH21 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H21);
                            double decH21Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H21);
                            double decDiferenciaH21 = decH21 - decH21Comparar;
                            if (decH21 > 0 && decH21Comparar > 0)
                            {
                                if (decDiferenciaH21 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(21).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H21.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H21.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH21;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH21 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(21).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H21.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H21.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH21;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH22 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H22);
                            double decH22Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H22);
                            double decDiferenciaH22 = decH22 - decH22Comparar;
                            if (decH22 > 0 && decH22Comparar > 0)
                            {
                                if (decDiferenciaH22 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(22).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H22.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H22.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH22;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH22 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(22).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H22.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H22.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH22;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH23 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H23);
                            double decH23Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H23);
                            double decDiferenciaH23 = decH23 - decH23Comparar;
                            if (decH23 > 0 && decH23Comparar > 0)
                            {
                                if (decDiferenciaH23 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(23).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H23.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H23.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH23;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH23 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(23).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H23.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H23.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH23;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH24 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H24);
                            double decH24Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H24);
                            double decDiferenciaH24 = decH24 - decH24Comparar;
                            if (decH24 > 0 && decH24Comparar > 0)
                            {
                                if (decDiferenciaH24 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(24).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H24.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H24.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH24;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH24 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(24).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H24.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H24.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH24;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH25 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H25);
                            double decH25Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H25);
                            double decDiferenciaH25 = decH25 - decH25Comparar;
                            if (decH25 > 0 && decH25Comparar > 0)
                            {
                                if (decDiferenciaH25 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(25).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H25.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H25.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH25;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH25 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(25).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H25.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H25.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH25;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH26 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H26);
                            double decH26Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H26);
                            double decDiferenciaH26 = decH26 - decH26Comparar;
                            if (decH26 > 0 && decH26Comparar > 0)
                            {
                                if (decDiferenciaH26 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(26).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H26.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H26.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH26;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH26 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(26).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H26.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H26.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH26;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH27 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H27);
                            double decH27Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H27);
                            double decDiferenciaH27 = decH27 - decH27Comparar;
                            if (decH27 > 0 && decH27Comparar > 0)
                            {
                                if (decDiferenciaH27 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(27).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H27.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H27.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH27;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH27 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(27).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H27.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H27.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH27;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH28 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H28);
                            double decH28Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H28);
                            double decDiferenciaH28 = decH28 - decH28Comparar;
                            if (decH28 > 0 && decH28Comparar > 0)
                            {
                                if (decDiferenciaH28 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(28).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H28.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H28.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH28;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH28 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(28).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H28.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H28.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH28;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH29 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H29);
                            double decH29Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H29);
                            double decDiferenciaH29 = decH29 - decH29Comparar;
                            if (decH29 > 0 && decH29Comparar > 0)
                            {
                                if (decDiferenciaH29 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(29).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H29.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H29.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH29;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH29 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(29).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H29.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H29.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH29;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH30 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H30);
                            double decH30Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H30);
                            double decDiferenciaH30 = decH30 - decH30Comparar;
                            if (decH30 > 0 && decH30Comparar > 0)
                            {
                                if (decDiferenciaH30 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(30).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H30.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H30.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH30;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH30 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(30).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H30.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H30.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH30;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH31 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H31);
                            double decH31Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H31);
                            double decDiferenciaH31 = decH31 - decH31Comparar;
                            if (decH31 > 0 && decH31Comparar > 0)
                            {
                                if (decDiferenciaH31 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(31).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H31.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H31.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH31;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH31 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(31).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H31.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H31.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH31;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH32 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H32);
                            double decH32Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H32);
                            double decDiferenciaH32 = decH32 - decH32Comparar;
                            if (decH32 > 0 && decH32Comparar > 0)
                            {
                                if (decDiferenciaH32 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(32).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H32.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H32.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH32;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH32 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(32).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H32.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H32.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH32;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH33 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H33);
                            double decH33Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H33);
                            double decDiferenciaH33 = decH33 - decH33Comparar;
                            if (decH33>0 && decH33Comparar>0)
                            {
                                if (decDiferenciaH33 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(33).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H33.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H33.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH33;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH33 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(33).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H33.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H33.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH33;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                            

                            double decH34 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H34);
                            double decH34Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H34);
                            double decDiferenciaH34 = decH34 - decH34Comparar;
                            if (decH34 > 0 && decH34Comparar > 0)
                            {
                                if (decDiferenciaH34 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(34).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H34.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H34.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH34;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH34 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(34).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H34.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H34.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH34;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                            

                            double decH35 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H35);
                            double decH35Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H35);
                            double decDiferenciaH35 = decH35 - decH35Comparar;
                            if (decH35 > 0 && decH35Comparar > 0)
                            {
                                if (decDiferenciaH35 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(35).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H35.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H35.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH35;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH35 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(35).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H35.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H35.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH35;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH36 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H36);
                            double decH36Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H36);
                            double decDiferenciaH36 = decH36 - decH36Comparar;
                            if (decH36 > 0 && decH36Comparar > 0)
                            {
                                if (decDiferenciaH36 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(36).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H36.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H36.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH36;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH36 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(36).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H36.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H36.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH36;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH37 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H37);
                            double decH37Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H37);
                            double decDiferenciaH37 = decH37 - decH37Comparar;
                            if (decH37 > 0 && decH37Comparar > 0)
                            {
                                if (decDiferenciaH37 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(37).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H37.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H37.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH37;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH37 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(37).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H37.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H37.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH37;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH38 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H38);
                            double decH38Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H38);
                            double decDiferenciaH38 = decH38 - decH38Comparar;
                            if (decH38 > 0 && decH38Comparar > 0)
                            {
                                if (decDiferenciaH38 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(38).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H38.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H38.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH38;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH38 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(38).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H38.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H38.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH38;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH39 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H39);
                            double decH39Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H39);
                            double decDiferenciaH39 = decH39 - decH39Comparar;
                            if (decH39 > 0 && decH39Comparar > 0)
                            {
                                if (decDiferenciaH39 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(39).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H39.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H39.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH39;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH39 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(39).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H39.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H39.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH39;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH40 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H40);
                            double decH40Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H40);
                            double decDiferenciaH40 = decH40 - decH40Comparar;
                            if (decH40 > 0 && decH40Comparar > 0)
                            {
                                if (decDiferenciaH40 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(40).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H40.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H40.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH40;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH40 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(40).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H40.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H40.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH40;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                


                            double decH41 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H41);
                            double decH41Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H41);
                            double decDiferenciaH41 = decH41 - decH41Comparar;
                            if (decH41 > 0 && decH41Comparar > 0)
                            {
                                if (decDiferenciaH41 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(41).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H41.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H41.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH41;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH41 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(41).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H41.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H41.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH41;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH42 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H42);
                            double decH42Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H42);
                            double decDiferenciaH42 = decH42 - decH42Comparar;
                            if (decH42 > 0 && decH42Comparar > 0)
                            {
                                if (decDiferenciaH42 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(42).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H42.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H42.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH42;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH42 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(42).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H42.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H42.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH42;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH43 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H43);
                            double decH43Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H43);
                            double decDiferenciaH43 = decH43 - decH43Comparar;
                            if (decH43 > 0 && decH43Comparar > 0)
                            {
                                if (decDiferenciaH43 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(43).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H43.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H43.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH43;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH43 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(43).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H43.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H43.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH43;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH44 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H44);
                            double decH44Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H44);
                            double decDiferenciaH44 = decH44 - decH44Comparar;
                            if (decH44 > 0 && decH44Comparar > 0)
                            {
                                if (decDiferenciaH44 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(44).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H44.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H44.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH44;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH44 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(44).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H44.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H44.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH44;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH45 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H45);
                            double decH45Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H45);
                            double decDiferenciaH45 = decH45 - decH45Comparar;
                            if (decH45 > 0 && decH45Comparar > 0)
                            {
                                if (decDiferenciaH45 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(45).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H45.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H45.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH45;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH45 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(45).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H45.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H45.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH45;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH46 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H46);
                            double decH46Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H46);
                            double decDiferenciaH46 = decH46 - decH46Comparar;
                            if (decH46 > 0 && decH46Comparar > 0)
                            {
                                if (decDiferenciaH46 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(46).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H46.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H46.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH46;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH46 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(46).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H46.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H46.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH46;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH47 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H47);
                            double decH47Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H47);
                            double decDiferenciaH47 = decH47 - decH47Comparar;
                            if (decH47 > 0 && decH47Comparar > 0)
                            {
                                if (decDiferenciaH47 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(47).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H47.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H47.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH47;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH47 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(47).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H47.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H47.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH47;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH48 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H48);
                            double decH48Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H48);
                            double decDiferenciaH48 = decH48 - decH48Comparar;
                            if (decH48 > 0 && decH48Comparar > 0)
                            {
                                if (decDiferenciaH48 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(48).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H48.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H48.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH48;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH48 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(48).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H48.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H48.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH48;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH49 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H49);
                            double decH49Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H49);
                            double decDiferenciaH49 = decH49 - decH49Comparar;
                            if (decH49 > 0 && decH49Comparar > 0)
                            {
                                if (decDiferenciaH49 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(49).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H49.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H49.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH49;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH49 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(49).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H49.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H49.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH49;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH50 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H50);
                            double decH50Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H50);
                            double decDiferenciaH50 = decH50 - decH50Comparar;
                            if (decH50 > 0 && decH50Comparar > 0)
                            {
                                if (decDiferenciaH50 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(50).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H50.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H50.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH50;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH50 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(50).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H50.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H50.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH50;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH51 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H51);
                            double decH51Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H51);
                            double decDiferenciaH51 = decH51 - decH51Comparar;
                            if (decH51 > 0 && decH51Comparar > 0)
                            {
                                if (decDiferenciaH51 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(51).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H51.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H51.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH51;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH51 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(51).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H51.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H51.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH51;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH52 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H52);
                            double decH52Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H52);
                            double decDiferenciaH52 = decH52 - decH52Comparar;
                            if (decH52 > 0 && decH52Comparar > 0)
                            {
                                if (decDiferenciaH52 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(52).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H52.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H52.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH52;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH52 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(52).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H52.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H52.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH52;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH53 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H53);
                            double decH53Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H53);
                            double decDiferenciaH53 = decH53 - decH53Comparar;
                            if (decH53 > 0 && decH53Comparar > 0)
                            {
                                if (decDiferenciaH53 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(53).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H53.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H53.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH53;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH53 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(53).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H53.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H53.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH53;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH54 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H54);
                            double decH54Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H54);
                            double decDiferenciaH54 = decH54 - decH54Comparar;
                            if (decH54 > 0 && decH54Comparar > 0)
                            {
                                if (decDiferenciaH54 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(54).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H54.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H54.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH54;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH54 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(54).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H54.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H54.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH54;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH55 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H55);
                            double decH55Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H55);
                            double decDiferenciaH55 = decH55 - decH55Comparar;
                            if (decH55 > 0 && decH55Comparar > 0)
                            {
                                if (decDiferenciaH55 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(55).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H55.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H55.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH55;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH55 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(55).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H55.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H55.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH55;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH56 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H56);
                            double decH56Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H56);
                            double decDiferenciaH56 = decH56 - decH56Comparar;
                            if (decH56 > 0 && decH56Comparar > 0)
                            {
                                if (decDiferenciaH56 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(56).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H56.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H56.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH56;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH56 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(56).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H56.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H56.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH56;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH57 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H57);
                            double decH57Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H57);
                            double decDiferenciaH57 = decH57 - decH57Comparar;
                            if (decH57 > 0 && decH57Comparar > 0)
                            {
                                if (decDiferenciaH57 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(57).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H57.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H57.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH57;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH57 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(57).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H57.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H57.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH57;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            double decH58 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H58);
                            double decH58Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H58);
                            double decDiferenciaH58 = decH58 - decH58Comparar;
                            if (decH58 > 0 && decH58Comparar > 0)
                            {
                                if (decDiferenciaH58 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(58).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H58.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H58.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH58;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH58 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(58).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H58.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H58.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH58;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                
                            
                            double decH59 = Convert.ToDouble(listLecturas[numLecturaFechaHora].H59);
                            double decH59Comparar = Convert.ToDouble(listLecturas[numLecturaFechaHora + 1].H59);
                            double decDiferenciaH59 = decH59 - decH59Comparar;
                            if (decH59 > 0 && decH59Comparar > 0)
                            {
                                if (decDiferenciaH59 > 0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(59).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H59.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H59.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH59;
                                    listDiferencias.Add(infFrecDTO);
                                }
                                else if (decDiferenciaH59 < -0.005)
                                {
                                    InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                                    infFrecDTO.FechaHora = listLecturas[numLecturaFechaHora].FechaHora.AddSeconds(59).ToString();
                                    infFrecDTO.GPSCodi = listLecturas[numLecturaFechaHora].GPSCodi;
                                    infFrecDTO.GPSNombre = listLecturas[numLecturaFechaHora].GPSNombre;
                                    infFrecDTO.Frecuencia = listLecturas[numLecturaFechaHora].H59.ToString();
                                    infFrecDTO.GPSNombreComparar = listLecturas[numLecturaFechaHora + 1].GPSNombre;
                                    infFrecDTO.FrecuenciaComparar = listLecturas[numLecturaFechaHora + 1].H59.ToString();
                                    infFrecDTO.FrecuenciaDiferencia = decDiferenciaH59;
                                    listDiferencias.Add(infFrecDTO);
                                }
                            }
                                

                            numLecturaFechaHora++;

                        }

                        
                    }
                }

            }
            return listDiferencias;
        }

        public List<InformacionFrecuenciaDTO> GetReporteEventosFrecuencia()
        {
            List<LecturaDTO> entitys = new List<LecturaDTO>();
            List<InformacionFrecuenciaDTO> entitysInfFrec = new List<InformacionFrecuenciaDTO>();
            var query = string.Format(helper.SqlReporteEventosFrecuencia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateLectura(dr));
                }
            }

            if (entitys.Count>0)
            {
                foreach(var item in entitys)
                {
                    if ((Convert.ToDouble(item.H0) > 60.7)  || (Convert.ToDouble(item.H0) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H0.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    } else if ((Convert.ToDouble(item.H1) > 60.7) || (Convert.ToDouble(item.H1) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H1.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H2) > 60.7) || (Convert.ToDouble(item.H2) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H2.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H3) > 60.7) || (Convert.ToDouble(item.H3) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H3.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H4) > 60.7) || (Convert.ToDouble(item.H4) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H4.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H5) > 60.7) || (Convert.ToDouble(item.H5) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H5.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H6) > 60.7) || (Convert.ToDouble(item.H6) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H6.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H7) > 60.7) || (Convert.ToDouble(item.H7) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H7.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H8) > 60.7) || (Convert.ToDouble(item.H8) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H8.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H9) > 60.7) || (Convert.ToDouble(item.H9) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H9.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H10) > 60.7) || (Convert.ToDouble(item.H10) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H10.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H11) > 60.7) || (Convert.ToDouble(item.H11) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H11.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H12) > 60.7) || (Convert.ToDouble(item.H12) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H12.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H13) > 60.7) || (Convert.ToDouble(item.H13) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H13.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H14) > 60.7) || (Convert.ToDouble(item.H14) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H14.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H15) > 60.7) || (Convert.ToDouble(item.H15) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H15.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H16) > 60.7) || (Convert.ToDouble(item.H16) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H16.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H17) > 60.7) || (Convert.ToDouble(item.H17) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H17.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H18) > 60.7) || (Convert.ToDouble(item.H18) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H18.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H19) > 60.7) || (Convert.ToDouble(item.H19) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H19.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H20) > 60.7) || (Convert.ToDouble(item.H20) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H20.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H21) > 60.7) || (Convert.ToDouble(item.H21) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H21.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H22) > 60.7) || (Convert.ToDouble(item.H22) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H22.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H23) > 60.7) || (Convert.ToDouble(item.H23) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H23.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H24) > 60.7) || (Convert.ToDouble(item.H24) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H24.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H25) > 60.7) || (Convert.ToDouble(item.H25) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H25.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H26) > 60.7) || (Convert.ToDouble(item.H26) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H26.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H27) > 60.7) || (Convert.ToDouble(item.H27) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H27.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H28) > 60.7) || (Convert.ToDouble(item.H28) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H28.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H29) > 60.7) || (Convert.ToDouble(item.H29) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H29.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H30) > 60.7) || (Convert.ToDouble(item.H30) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H30.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H31) > 60.7) || (Convert.ToDouble(item.H31) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H31.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H32) > 60.7) || (Convert.ToDouble(item.H32) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H32.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H33) > 60.7) || (Convert.ToDouble(item.H33) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H33.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H34) > 60.7) || (Convert.ToDouble(item.H34) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H34.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H35) > 60.7) || (Convert.ToDouble(item.H35) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H35.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H36) > 60.7) || (Convert.ToDouble(item.H36) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H36.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H37) > 60.7) || (Convert.ToDouble(item.H37) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H37.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H38) > 60.7) || (Convert.ToDouble(item.H38) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H38.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H39) > 60.7) || (Convert.ToDouble(item.H39) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H39.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H40) > 60.7) || (Convert.ToDouble(item.H40) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H40.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H41) > 60.7) || (Convert.ToDouble(item.H41) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H41.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H42) > 60.7) || (Convert.ToDouble(item.H42) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H42.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H43) > 60.7) || (Convert.ToDouble(item.H43) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H43.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H44) > 60.7) || (Convert.ToDouble(item.H44) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H44.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H45) > 60.7) || (Convert.ToDouble(item.H45) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H45.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H46) > 60.7) || (Convert.ToDouble(item.H46) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H46.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H47) > 60.7) || (Convert.ToDouble(item.H47) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H47.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H48) > 60.7) || (Convert.ToDouble(item.H48) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H48.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H49) > 60.7) || (Convert.ToDouble(item.H49) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H49.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H50) > 60.7) || (Convert.ToDouble(item.H50) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H50.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H51) > 60.7) || (Convert.ToDouble(item.H51) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H51.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H52) > 60.7) || (Convert.ToDouble(item.H52) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H52.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H53) > 60.7) || (Convert.ToDouble(item.H53) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H53.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H54) > 60.7) || (Convert.ToDouble(item.H54) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H54.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H55) > 60.7) || (Convert.ToDouble(item.H55) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H55.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H56) > 60.7) || (Convert.ToDouble(item.H56) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H56.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H57) > 60.7) || (Convert.ToDouble(item.H57) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H57.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H58) > 60.7) || (Convert.ToDouble(item.H58) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H58.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                    else if ((Convert.ToDouble(item.H59) > 60.7) || (Convert.ToDouble(item.H59) < 59.3))
                    {
                        InformacionFrecuenciaDTO infFrecDTO = new InformacionFrecuenciaDTO();
                        infFrecDTO.FechaHora = item.FechaHora.ToString();
                        infFrecDTO.GPSCodi = item.GPSCodi;
                        infFrecDTO.GPSNombre = item.GPSNombre;
                        infFrecDTO.Frecuencia = item.H59.ToString();
                        entitysInfFrec.Add(infFrecDTO);
                    }
                }
            }

            return entitysInfFrec;
        }
    }
}
