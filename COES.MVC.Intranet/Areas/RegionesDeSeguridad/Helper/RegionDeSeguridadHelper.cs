using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RegionesDeSeguridad.Models;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RegionesDeSeguridad.Helper
{
    public class RegionDeSeguridadHelper
    {
        public static DataCargaMasiva LeerExcelFile2(string ruta, string file, List<CpRecursoDTO> recursos, List<PrGrupoDTO> listaUnidades, string usuario)
        {
            Boolean retorno = false;
            Boolean salir = false;
            FileInfo fileInfo = new FileInfo(file);
            DataCargaMasiva Data = new DataCargaMasiva();
            SegRegionDTO region = new SegRegionDTO();
            List<SegRegionDTO> listaRegion = new List<SegRegionDTO>();
            List<SegCoordenadaDTO> listaCoordenada = new List<SegCoordenadaDTO>();
            List<SegRegionequipoDTO> listaEquipo = new List<SegRegionequipoDTO>();
            List<SegRegiongrupoDTO> listaGrupo = new List<SegRegiongrupoDTO>();
            List<CmRegionseguridadDTO> listaCmRegion = new List<CmRegionseguridadDTO>();
            List<CmRegionseguridadDetalleDTO> listaCmRegionDetalle = new List<CmRegionseguridadDetalleDTO>();
            CmRegionseguridadDTO regCmRegion;
            CmRegionseguridadDetalleDTO regCmRegioDet;
            SegRegiongrupoDTO grupo;
            SegRegionequipoDTO equipo;
            SegCoordenadaDTO coordenada;
            int fila = 3;
            int columna = 2;
            int idRegion = 0;
            int idOrden = 0;
            int idTipo = 0;
            int idZona = 0;
            int idSicoes = 0;
            int idRecurso = 0;
            string valor;
            decimal flujo1, flujo2, gener1, gener2;
            decimal? deltaGen;
            decimal? deltaFlu;
            string strTipo;
            int idRegSegcodi = 0;
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Lista Regiones
                while (!salir)
                {
                    valor = (ws.Cells[fila, columna].Value != null) ? ws.Cells[fila, columna].Value.ToString() : string.Empty;
                    if (int.TryParse(valor, out idRegion))
                    {
                        region = new SegRegionDTO();
                        region.RegcodiExcel = idRegion;
                        region.Regnombre = (ws.Cells[fila, columna + 1].Value != null) ? ws.Cells[fila, columna + 1].Value.ToString() : string.Empty;
                        region.Regusucreacion = usuario;
                        region.Regusumodificacion = usuario;
                        region.Regfeccreacion = DateTime.Now;
                        region.Regfecmodificacion = DateTime.Now;
                        region.Regestado = ConstantesRegionesDeSeguridad.Activo;
                        fila++;
                        listaRegion.Add(region);
                    }
                    else
                        salir = true;
                }
                Data.ListaRegion = listaRegion;
                /// Lista coordenada
                ws = xlPackage.Workbook.Worksheets[2];
                fila = 3;
                columna = 1;
                salir = false;
                while (!salir)
                {
                    valor = (ws.Cells[fila, columna].Value != null) ? ws.Cells[fila, columna].Value.ToString() : string.Empty;
                    if (int.TryParse(valor, out idRegion))
                    {
                        coordenada = new SegCoordenadaDTO();
                        coordenada.RegcodiExcel = idRegion;
                        valor = (ws.Cells[fila, columna + 2].Value != null) ? ws.Cells[fila, columna + 2].Value.ToString() : string.Empty;
                        coordenada.Segconro = (int.TryParse(valor, out idOrden)) ? idOrden : 0;
                        valor = (ws.Cells[fila, columna + 1].Value != null) ? ws.Cells[fila, columna + 1].Value.ToString() : string.Empty;
                        coordenada.Segcotipo = (int.TryParse(valor, out idTipo)) ? idTipo - 1 : 0;
                        // Zona
                        valor = (ws.Cells[fila, columna + 10].Value != null) ? ws.Cells[fila, columna + 10].Value.ToString() : string.Empty;
                        switch (valor)
                        {
                            case ConstantesRegionesDeSeguridad.NoRestringe:
                                idZona = 0;
                                break;
                            case ConstantesRegionesDeSeguridad.HaciaAbajo:
                                idZona = 1;
                                break;
                            case ConstantesRegionesDeSeguridad.HaciaArriba:
                                idZona = 2;
                                break;
                        }
                        coordenada.Zoncodi = idZona;
                        valor = (ws.Cells[fila, columna + 6].Value != null) ? ws.Cells[fila, columna + 6].Value.ToString() : string.Empty;
                        coordenada.Segcoflujo1 = (decimal.TryParse(valor, out flujo1)) ? flujo1 : 0;
                        valor = (ws.Cells[fila, columna + 7].Value != null) ? ws.Cells[fila, columna + 7].Value.ToString() : string.Empty;
                        coordenada.Segcogener1 = (decimal.TryParse(valor, out gener1)) ? gener1 : 0;
                        valor = (ws.Cells[fila, columna + 8].Value != null) ? ws.Cells[fila, columna + 8].Value.ToString() : string.Empty;
                        coordenada.Segcoflujo2 = (decimal.TryParse(valor, out flujo2)) ? flujo2 : 0;
                        valor = (ws.Cells[fila, columna + 9].Value != null) ? ws.Cells[fila, columna + 9].Value.ToString() : string.Empty;
                        coordenada.Segcogener2 = (decimal.TryParse(valor, out gener2)) ? gener2 : 0;
                        coordenada.Segcousucreacion = usuario;
                        coordenada.Segcousumodificacion = usuario;
                        coordenada.Segcofecmodificacion = DateTime.Now;
                        coordenada.Segcofeccreacion = DateTime.Now;
                        coordenada.Segcoestado = ConstantesRegionesDeSeguridad.Activo;
                        if (idZona != 0) //Se crea una region para Costo Marginal
                        {
                            idRegSegcodi++;
                            regCmRegion = new CmRegionseguridadDTO();
                            regCmRegion.RegsegcodiExcel = idRegSegcodi;
                            regCmRegion.Regsegestado = ConstantesAppServicio.Activo;
                            regCmRegion.Regsegdirec = ((idZona == 1) ? 2 : 1).ToString();
                            deltaGen = coordenada.Segcogener2 - coordenada.Segcogener1;
                            deltaFlu = coordenada.Segcoflujo2 - coordenada.Segcoflujo1;
                            regCmRegion.Regsegvalorm = (deltaFlu == 0) ? 1000 : deltaGen / deltaFlu;
                            regCmRegion.Regsegusucreacion = usuario;
                            regCmRegion.Regsegusumodificacion = usuario;
                            regCmRegion.Regsegfeccreacion = DateTime.Now;
                            regCmRegion.Regsegfecmodificacion = DateTime.Now;
                            var findReg = listaRegion.Find(x => x.RegcodiExcel == idRegion);
                            if (findReg != null)
                            {
                                strTipo = (coordenada.Segcotipo == 0) ? "MIN" : (coordenada.Segcotipo == 1) ? "MED" : "MAX";
                                regCmRegion.Regsegnombre = findReg.Regnombre.Trim() + "-" + strTipo + "-" + coordenada.Segconro;
                            }
                            coordenada.RegsegcodiExcel = idRegSegcodi;
                            listaCmRegion.Add(regCmRegion);
                        }
                        fila++;
                        listaCoordenada.Add(coordenada);
                    }
                    else
                        salir = true;
                }
                Data.ListaCoordenada = listaCoordenada;
                /// Lista Equipo
                ws = xlPackage.Workbook.Worksheets[3];
                fila = 3;
                columna = 1;
                salir = false;
                while (!salir)
                {
                    valor = (ws.Cells[fila, columna].Value != null) ? ws.Cells[fila, columna].Value.ToString() : string.Empty;
                    if (int.TryParse(valor, out idRegion))
                    {
                        valor = (ws.Cells[fila, columna + 5].Value != null) ? ws.Cells[fila, columna + 5].Value.ToString() : string.Empty;
                        if (int.TryParse(valor, out idRecurso))
                        {
                            var findIdSicoes = recursos.Find(x => x.Recurcodi == idRecurso);
                            if (findIdSicoes != null)
                            {
                                idSicoes = (int)findIdSicoes.Recurcodisicoes;
                            }
                        }
                        valor = (ws.Cells[fila, columna + 3].Value != null) ? ws.Cells[fila, columna + 3].Value.ToString() : string.Empty;
                        switch (valor)
                        {
                            case ConstantesRegionesDeSeguridad.Minima:
                                idTipo = 0;
                                break;
                            case ConstantesRegionesDeSeguridad.Media:
                                idTipo = 1;
                                break;
                            case ConstantesRegionesDeSeguridad.Maxima:
                                idTipo = 2;
                                break;

                        }
                        // Tipo equipo
                        valor = (ws.Cells[fila, columna + 4].Value != null) ? ws.Cells[fila, columna + 4].Value.ToString() : string.Empty;
                        switch (valor)
                        {
                            case ConstantesRegionesDeSeguridad.ModoTermica:
                                grupo = new SegRegiongrupoDTO();
                                grupo.Grupocodi = idSicoes;
                                grupo.Segcotipo = idTipo;
                                grupo.RegcodiExcel = idRegion;
                                grupo.Reggfeccreacion = DateTime.Now;
                                grupo.Reggusucreacion = usuario;
                                listaGrupo.Add(grupo);
                                var listarectas = listaCoordenada.Where(x => x.RegcodiExcel == idRegion && x.RegsegcodiExcel > 0);
                                var lunidad = listaUnidades.Where(x => x.Grupocodi == idSicoes);
                                foreach (var reg in listarectas)
                                {
                                    foreach (var p in lunidad)
                                    {
                                        regCmRegioDet = new CmRegionseguridadDetalleDTO();
                                        regCmRegioDet.Equicodi = p.Equicodi;
                                        regCmRegioDet.RegsegcodiExcel = reg.RegsegcodiExcel;
                                        regCmRegioDet.Regsegfeccreacion = DateTime.Now;
                                        regCmRegioDet.Regsegusucreacion = usuario;
                                        listaCmRegionDetalle.Add(regCmRegioDet);
                                    }
                                }
                                break;
                            default:
                                equipo = new SegRegionequipoDTO();
                                equipo.Equicodi = idSicoes;
                                equipo.Segcotipo = idTipo;
                                equipo.RegcodiExcel = idRegion;
                                equipo.Regefeccreacion = DateTime.Now;
                                equipo.Regeusucreacion = usuario;
                                listaEquipo.Add(equipo);
                                var listaEq = listaCoordenada.Where(x => x.RegcodiExcel == idRegion && x.RegsegcodiExcel > 0);
                                foreach (var reg in listaEq)
                                {
                                    regCmRegioDet = new CmRegionseguridadDetalleDTO();
                                    regCmRegioDet.Equicodi = idSicoes;
                                    regCmRegioDet.RegsegcodiExcel = reg.RegsegcodiExcel;
                                    regCmRegioDet.Regsegfeccreacion = DateTime.Now;
                                    regCmRegioDet.Regsegusucreacion = usuario;
                                    listaCmRegionDetalle.Add(regCmRegioDet);
                                }
                                break;
                        }
                        fila++;
                    }
                    else
                        salir = true;
                }
                if (listaEquipo.Count > 0)
                    Data.ListaEquipo = listaEquipo;
                if (listaGrupo.Count > 0)
                    Data.ListaGrupo = listaGrupo;
                Data.ListaCmRegion = listaCmRegion;
                Data.ListaCmRegionDetalle = listaCmRegionDetalle;

            }
            //GenerarFileExcel(Data, ruta);
            return Data;
        }

        public static void GenerarFileExcel(DataCargaMasiva data, string ruta)
        {
            int fila, columna, i;
            FileInfo newFile = new FileInfo(ruta + "carga.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + "carga.xlsx");
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Region de Seguridad");
                ws = xlPackage.Workbook.Worksheets[1];
                fila = 2;
                columna = 2;
                ws.Cells[fila, columna].Value = "Código";
                ws.Cells[fila, columna + 1].Value = "Región de Seguridad";
                i = 1;
                foreach (var reg in data.ListaRegion)
                {
                    ws.Cells[fila + i, columna].Value = reg.Regcodi;
                    ws.Cells[fila + i, columna + 1].Value = reg.Regnombre;
                    i++;
                }
                xlPackage.Save();
            }
        }
    }
}