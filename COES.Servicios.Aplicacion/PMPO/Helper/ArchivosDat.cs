using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PMPO.Helper
{
    public static class ArchivosDat
    {


        public async static Task<bool[]> GenerarDatSemanal(List<PmoDatPmhiTrDTO> pmhisepe,
            List<PmoDatPmhiTrDTO> pmtrsepe,
            List<PmoDatDbusDTO> dbus,
            List<PmoDatDbfDTO> dbf,
            List<PmoDatCgndDTO> cgnd,
            List<PmoDatMgndDTO> mgnd,
            List<PmoDatGndseDTO> gndse,
            List<PrGrupoDTO> gndseCabeceras,
            string directorioBase)
        {
            if (pmhisepe != null)
                FileServer.DeleteBlob("pmhisepe.dat", directorioBase);

            if (pmtrsepe != null)
                FileServer.DeleteBlob("pmtrsepe.dat", directorioBase);

            if (dbus != null)
                FileServer.DeleteBlob("dbus.dat", directorioBase);

            if (dbf != null)
                FileServer.DeleteBlob("dbf005pe.dat", directorioBase);

            if (cgnd != null)
                FileServer.DeleteBlob("cgndpe.dat", directorioBase);

            if (mgnd != null)
                FileServer.DeleteBlob("mgndpe.dat", directorioBase);

            if (gndse != null)
                FileServer.DeleteBlob("gndse05pe.dat", directorioBase);

            try
            {
                var datHidraulico = GenerarDatDisponibilidadSemanal(directorioBase, ConstantesPMPO.TsddpPlantaHidraulica, "pmhisepe.dat", pmhisepe);
                var datTermico = GenerarDatDisponibilidadSemanal(directorioBase, ConstantesPMPO.TsddpPlantaTermica, "pmtrsepe.dat", pmtrsepe);
                var datDbus = GenerarDatDbusNew(directorioBase, "dbus.dat", dbus);
                var datDbf = GenerarDatDbfNew(directorioBase, "dbf005pe.dat", dbf);
                var datCgndpe = GenerarDatCgndpeNew(directorioBase, "cgndpe.dat", cgnd);
                var datMgnd = GenerarDatMgndpeNew(directorioBase, "mgndpe.dat", mgnd);
                var datGndse = GenerarDatGndseNew(directorioBase, "gndse05pe.dat", gndse, gndseCabeceras);

                var resultado = await Task.WhenAll(datHidraulico, datTermico, datDbus, datDbf, datCgndpe, datMgnd, datGndse);

                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async static Task<bool[]> GenerarDatMensual(List<PmoDatPmhiTrDTO> pmhisepe, List<PmoDatPmhiTrDTO> pmtrsepe, string directorioBase)
        {
            if (pmhisepe != null)
                FileServer.DeleteBlob("pmhimepe.dat", directorioBase);

            if (pmtrsepe != null)
                FileServer.DeleteBlob("pmtrmepe.dat", directorioBase);

            try
            {
                var datHidraulico = GenerarDatDisponibilidadMensual(directorioBase, ConstantesPMPO.TsddpPlantaHidraulica, "pmhimepe.dat", pmhisepe);
                var datTermico = GenerarDatDisponibilidadMensual(directorioBase, ConstantesPMPO.TsddpPlantaTermica, "pmtrmepe.dat", pmtrsepe);

                var resultado = await Task.WhenAll(datHidraulico, datTermico);

                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async static Task<bool> GenerarDatDisponibilidadSemanal(string directorio, int tsddpcodi, string fileName, List<PmoDatPmhiTrDTO> pmhisepe)
        {
            if (pmhisepe.Any())
            {
                string sLinea2 = tsddpcodi == ConstantesPMPO.TsddpPlantaTermica ? "Unidades   :    2  (1=No.Unid;2=%;3=MW)"
                                                                            : "Unidades   :    2  (1=No.Unid;2=%;3=MW;4=m^3/s)";

                using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
                {
                    await dat.WriteLineAsync("Tipo Info. :    2  (1=Reduc;2=Absoluto)");
                    await dat.WriteLineAsync(sLinea2);

                    string cabecera = "A~NO";
                    for (var x = 1; x < 53; x++) cabecera += " " + x.ToString().PadLeft(7, '.');

                    List<PmoSddpCodigoDTO> listaPlanta = pmhisepe.GroupBy(x => x.Sddpcodi).Select(x => new PmoSddpCodigoDTO()
                    {
                        Sddpcodi = x.Key,
                        Sddpnomb = x.First().Sddpnomb,
                        Planta = x.First().Planta
                    }).OrderBy(x => x.Planta).ToList();

                    foreach (var regPlanta in listaPlanta)
                    {
                        List<PmoDatPmhiTrDTO> listxPlanta = pmhisepe.Where(x => x.Sddpcodi == regPlanta.Sddpcodi).ToList();

                        await dat.WriteLineAsync("**** Planta:" + regPlanta.Planta.PadRight(18, ' ').Substring(0, 18));
                        await dat.WriteLineAsync(cabecera);

                        foreach (var registro in listxPlanta)
                        {
                            string lDatos = registro.PmPmhtAnhio.ToString();
                            for (int i = 1; i <= 52; i++)
                            {
                                decimal? valor = (decimal?)registro.GetType().GetProperty("PmPmhtDisp" + i.ToString("D2")).GetValue(registro, null);

                                if (i == 44)
                                { }

                                string valorTxt = MathHelper.Round(valor.GetValueOrDefault(0), 1).ToString("0.0").Replace(".0", ".");

                                lDatos += valorTxt.PadLeft(8, ' ');
                            }

                            await dat.WriteLineAsync(lDatos);
                        }
                    }
                }
            }

            return pmhisepe.Any();
        }

        private async static Task<bool> GenerarDatDisponibilidadMensual(string directorio, int tsddpcodi, string fileName, List<PmoDatPmhiTrDTO> pmhisepe)
        {
            if (pmhisepe.Any())
            {
                string sLinea2 = tsddpcodi == ConstantesPMPO.TsddpPlantaTermica ? "Unidades   :    2  (1=No.Unid;2=%;3=MW)"
                                                                            : "Unidades   :    2  (1=No.Unid;2=%;3=MW;4=m^3/s)";

                using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
                {
                    await dat.WriteLineAsync("Tipo Info. :    2  (1=Reduc;2=Absoluto)");
                    await dat.WriteLineAsync(sLinea2);

                    string cabecera = "A~NO";
                    for (var x = 1; x <= 12; x++) cabecera += " " + x.ToString().PadLeft(7, '.');

                    List<PmoSddpCodigoDTO> listaPlanta = pmhisepe.GroupBy(x => x.Sddpcodi).Select(x => new PmoSddpCodigoDTO()
                    {
                        Sddpcodi = x.Key,
                        Sddpnomb = x.First().Sddpnomb,
                        Planta = x.First().Planta
                    }).OrderBy(x => x.Planta).ToList();

                    foreach (var regPlanta in listaPlanta)
                    {
                        List<PmoDatPmhiTrDTO> listxPlanta = pmhisepe.Where(x => x.Sddpcodi == regPlanta.Sddpcodi).ToList();

                        await dat.WriteLineAsync("**** Planta:" + regPlanta.Planta.PadRight(18, ' ').Substring(0, 18));
                        await dat.WriteLineAsync(cabecera);

                        foreach (var registro in listxPlanta)
                        {
                            string lDatos = registro.PmPmhtAnhio.ToString();
                            for (int i = 1; i <= 12; i++)
                            {
                                decimal? valor = (decimal?)registro.GetType().GetProperty("PmPmhtDisp" + i.ToString("D2")).GetValue(registro, null);

                                string valorTxt = MathHelper.Round(valor.GetValueOrDefault(0), 1).ToString("0.0").Replace(".0", ".");

                                lDatos += valorTxt.PadLeft(8, ' ');
                            }

                            await dat.WriteLineAsync(lDatos);
                        }
                    }
                }
            }

            return pmhisepe.Any();
        }

        private async static Task<bool> GenerarDatDbusNew(string directorio, string fileName, List<PmoDatDbusDTO> dbus)
        {
            bool terminado;

            if (dbus == null)
                return terminado = true;

            using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
            {
                //cabecera
                string cabecera = string.Empty;
                cabecera += " NUM. ";
                cabecera += "Tp ";
                cabecera += "...NombreB.. ";
                cabecera += "Id  ";
                cabecera += "# ";
                cabecera += "tg ";
                cabecera += "Plnt ";
                cabecera += "Nombre Gener.";
                cabecera += "Area ";
                cabecera += "%per1 Ploa1 Pind1 PerF1 ";
                cabecera += "%per2 Ploa2 Pind2 PerF2 ";
                cabecera += "%per3 Ploa3 Pind3 PerF3 ";
                cabecera += "%per4 Ploa4 Pind4 PerF4 ";
                cabecera += "%per5 Ploa5 Pind5 PerF5 ";
                cabecera += "Icca";

                await dat.WriteLineAsync(cabecera);

                foreach (var registro in dbus)
                {
                    var num = registro.Num == null ? string.Empty.PadLeft(5, ' ') : registro.Num.ToString().PadLeft(5, ' ');
                    var tp = registro.Tp == null ? string.Empty.ToString().PadLeft(2, ' ') : registro.Tp.ToString().PadLeft(2, ' ');
                    var nombreB = registro.NombreB == null ? string.Empty.ToString().PadLeft(12, ' ') : registro.NombreB.ToString().PadLeft(12, ' ');
                    var id = registro.Id;
                    var numeral = registro.Numeral != null ? registro.Numeral.ToString().Trim().PadLeft(2, ' ') : string.Empty.PadLeft(2, ' ');//20190308 - NET: Adecuaciones a los archivos .DAT
                                                                                                                                               //var tg = registro.Tg;//20190308 - NET: Adecuaciones a los archivos .DAT
                    var tg = registro.Tg != null ? registro.Tg.ToString().Trim().PadLeft(2, ' ') : string.Empty.PadLeft(2, ' ');//20190308 - NET: Adecuaciones a los archivos .DAT
                    var plnt = registro.Plnt == null ? string.Empty.PadLeft(4, ' ') : registro.Plnt.ToString().PadLeft(4, ' ');
                    var nombreGener = registro.NombreGener == null ? string.Empty.ToString().PadLeft(12, ' ') : registro.NombreGener.ToString().PadLeft(12, ' ');
                    var area = registro.Area == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.Area.ToString().PadLeft(4, ' ');
                    var per1 = registro.Per1;
                    var ploa1 = registro.Ploa1;
                    var pind1 = registro.Pind1;
                    var perf1 = registro.PerF1;
                    var per2 = registro.Per2;
                    var ploa2 = registro.Ploa2;
                    var pind2 = registro.Pind2;
                    var perf2 = registro.PerF2;
                    var per3 = registro.Per3;
                    var ploa3 = registro.Ploa3;
                    var pind3 = registro.Pind3;
                    var perf3 = registro.PerF3;
                    var per4 = registro.Per4;
                    var ploa4 = registro.Ploa4;
                    var pind4 = registro.Pind4;
                    var perf4 = registro.PerF4;
                    var per5 = registro.Per5;
                    var ploa5 = registro.Ploa5;
                    var pind5 = registro.Pind5;
                    var perf5 = registro.PerF5;
                    var icca = registro.Icca == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.Icca.ToString().PadLeft(4, ' ');

                    await dat.WriteLineAsync(
                        num + " " +
                        tp + " " +
                        nombreB + " " +
                        id + " " +
                        numeral + " " +
                        tg + " " +
                        plnt + " " +
                        nombreGener + " " +
                        area + " " +
                         per1 + " " + ploa1 + " " + pind1 + " " + perf1 + " " +
                         per2 + " " + ploa2 + " " + pind2 + " " + perf2 + " " +
                         per3 + " " + ploa3 + " " + pind3 + " " + perf3 + " " +
                         per4 + " " + ploa4 + " " + pind4 + " " + perf4 + " " +
                         per5 + " " + ploa5 + " " + pind5 + " " + perf5 + " " +//20190308 - NET: Adecuaciones a los archivos .DAT
                          icca
                        );
                }

            }



            return terminado = true;
        }

        private async static Task<bool> GenerarDatDbfNew(string directorio, string fileName, List<PmoDatDbfDTO> dbf)
        {
            bool terminado;

            if (dbf == null)
                return terminado = true;

            using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
            {
                //cabecera
                string cabecera = string.Empty;
                cabecera += "!BCod ";
                cabecera += "..Bus.Name.. ";
                cabecera += "LCod ";
                cabecera += "dd/mm/yyyy ";
                cabecera += "Llev ";
                cabecera += "..Load.. ";
                cabecera += "Icca";


                await dat.WriteLineAsync(cabecera);

                foreach (var registro in dbf)
                {
                    var BCod = registro.BCod == null ? string.Empty.PadLeft(5, ' ') : registro.BCod.Trim().PadLeft(5, ' ');//20190308 - NET: Adecuaciones a los archivos .DAT

                    await dat.WriteLineAsync(
                        BCod + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.BusName + " " +
                        registro.LCod + " " +
                        registro.Fecha + " " +
                        registro.Llev + " " +
                        registro.strPmDbf5Carga.PadLeft(8, ' ') + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.Icca
                        );
                }

            }



            return terminado = true;
        }

        private async static Task<bool> GenerarDatCgndpeNew(string directorio, string fileName, List<PmoDatCgndDTO> cgnd)
        {
            bool terminado;

            if (cgnd == null)
                return terminado = true;

            using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
            {
                //cabecera
                string cabecera = string.Empty;
                cabecera += "!Num ";
                cabecera += "Name........ ";
                cabecera += ".Bus. ";
                cabecera += "Tipo ";
                cabecera += "#Uni ";
                cabecera += ".PotIns ";
                cabecera += "..FatOpe ";
                cabecera += "ProbFal ";
                cabecera += "SFal ";//20190713 - NET: Adecuaciones a los archivos .DAT
                cabecera += "Stat. ";//20190713 - NET: Adecuaciones a los archivos .DAT
                cabecera += "....O&M ";//20190713 - NET: Adecuaciones a los archivos .DAT
                cabecera += "CurtCos ";//20190713 - NET: Adecuaciones a los archivos .DAT
                cabecera += "TechTyp";//20190713 - NET: Adecuaciones a los archivos .DAT

                await dat.WriteLineAsync("$version=3");//20190713 - NET: Adecuaciones a los archivos .DAT
                await dat.WriteLineAsync(cabecera);

                foreach (var registro in cgnd)
                {
                    var Num = registro.Num == null ? string.Empty.PadLeft(4, ' ') : registro.Num.Trim().PadLeft(4, ' '); //20190308 - NET: Adecuaciones a los archivos .DAT
                    var Bus = registro.Bus == null ? string.Empty.PadLeft(5, ' ') : registro.Bus.Trim().PadLeft(5, ' '); //20190308 - NET: Adecuaciones a los archivos .DAT

                    await dat.WriteLineAsync(
                        Num + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.Name + " " +
                        Bus + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.Tipo + " " +
                        registro.Uni + " " +
                        registro.strPmCgndPotInstalada.PadLeft(7, ' ') + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.strPmCgndFactorOpe.PadLeft(8, ' ') + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.strPmCgndProbFalla.PadLeft(7, ' ') + " " + //20190308 - NET: Adecuaciones a los archivos .DAT
                        registro.SFal + " " + //20190713 - NET: Adecuaciones a los archivos .DAT
                        " " + Num + " " +//20190713 - NET: Adecuaciones a los archivos .DAT
                        "     0. " + "     0. " + "       "//20190713 - NET: Adecuaciones a los archivos .DAT
                        );
                }

            }



            return terminado = true;
        }

        private async static Task<bool> GenerarDatMgndpeNew(string directorio, string fileName, List<PmoDatMgndDTO> mgnd)
        {
            bool terminado;

            if (mgnd == null)
                return terminado = true;

            using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
            {
                //cabecera
                string cabecera = string.Empty;
                cabecera += "!d/mm/yyyy ";
                cabecera += ".Num ";//20190713 - NET: Adecuaciones a los archivos .DAT    
                cabecera += "Name........ ";
                cabecera += ".Bus. ";
                cabecera += "Tipo ";
                cabecera += "#Uni ";
                cabecera += ".PotIns ";
                cabecera += "..FatOpe ";
                cabecera += "ProbFal ";
                cabecera += "SFal ";//20190713 - NET: Adecuaciones a los archivos .DAT    
                cabecera += "Stat. ";//20190713 - NET: Adecuaciones a los archivos .DAT    
                cabecera += "....O&M ";//20190713 - NET: Adecuaciones a los archivos .DAT    
                cabecera += "CurtCos";//20190713 - NET: Adecuaciones a los archivos .DAT    

                await dat.WriteLineAsync("$version=3");//20190713 - NET: Adecuaciones a los archivos .DAT    
                await dat.WriteLineAsync(cabecera);

                foreach (var registro in mgnd)
                {

                    var fecha = registro.Fecha == null ? string.Empty.ToString().PadLeft(10, ' ') : registro.Fecha;
                    var num = registro.Num == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.Num.ToString().PadLeft(4, ' ');
                    var name = registro.Name == null ? string.Empty.ToString().PadLeft(12, ' ') : registro.Name.ToString().PadLeft(12, ' ');
                    var bus = registro.Bus == null ? string.Empty.ToString().PadLeft(5, ' ') : registro.Bus.ToString().PadLeft(5, ' ');
                    var tipo = registro.Tipo == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.Tipo.ToString().PadLeft(4, ' ');
                    var uni = registro.Uni == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.Uni.ToString().PadLeft(4, ' ');

                    #region 20190308 - NET: Adecuaciones a los archivos .DAT
                    //var potIns = registro.PotIns == null ? string.Empty.ToString().PadLeft(7, ' ') : registro.PotIns.ToString().PadLeft(7, ' ');
                    //var fatOpe = registro.FatOpe == null ? string.Empty.ToString().PadLeft(8, ' ') : registro.FatOpe.ToString().PadLeft(8, ' ');
                    //var probFal = registro.ProbFal == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.ProbFal.ToString().PadLeft(4, ' ');
                    var potIns = registro.strPmMgndPotInstalada == null ? string.Empty.ToString().PadLeft(7, ' ') : registro.strPmMgndPotInstalada.PadLeft(7, ' ');
                    var fatOpe = registro.strPmMgndFactorOpe == null ? string.Empty.ToString().PadLeft(8, ' ') : registro.strPmMgndFactorOpe.PadLeft(8, ' ');
                    var probFal = registro.strPmMgndProbFalla == null ? string.Empty.ToString().PadLeft(7, ' ') : registro.strPmMgndProbFalla.PadLeft(7, ' ');
                    var sFal = registro.SFal == null ? string.Empty.ToString().PadLeft(4, ' ') : registro.SFal.PadLeft(4, ' ');//20190713 - NET: Adecuaciones a los archivos .DAT    

                    await dat.WriteLineAsync(
                       fecha + " " +
                        num + " " +
                        name + " " +
                        bus + " " +
                        tipo + " " +
                        uni + " " +
                        potIns + " " +
                        fatOpe + " " +
                        probFal + " " +//20190713 - NET: Adecuaciones a los archivos .DAT    
                        sFal + " " +//20190713 - NET: Adecuaciones a los archivos .DAT    
                        "      " + "     0. " + "     0."//20190713 - NET: Adecuaciones a los archivos .DAT    
                        );

                    #endregion
                }

            }



            return terminado = true;
        }

        private async static Task<bool> GenerarDatGndseNew(string directorio, string fileName, List<PmoDatGndseDTO> gndse, List<PrGrupoDTO> gndseCabeceras)
        {
            bool terminado;

            if (gndse == null)
                return terminado = true;

            using (StreamWriter dat = FileServer.OpenWriterFile(fileName, directorio))
            {
                //cabecera
                string cabecera = string.Empty;
                cabecera += "!Stg ";
                cabecera += "NScn ";
                cabecera += "LBlk";

                await dat.WriteLineAsync("!Unit: pu");

                foreach (var gndseCabecera in gndseCabeceras)
                {
                    // 20190314 - NET: Levantamiento de observaciones de usuario
                    //cabecera += gndseCabecera.Grupoabrev.Trim().PadLeft(12, ' ') + " ";
                    if (gndseCabecera.Grupoabrev != null)
                        cabecera += " " + gndseCabecera.Grupoabrev.Trim().PadRight(12, ' ').Substring(0, 12);
                    else
                        cabecera += " " + "".PadRight(12, ' ').Substring(0, 12);

                }

                await dat.WriteLineAsync(cabecera);

                //int saltoRegistro = gndseCabeceras.Count();
                int saltoRegistro = gndse.Select(p => p.GrupoCodi).Distinct().Count();


                for (var registro = 0; registro < gndse.Count; registro++)
                {
                    var registroText = gndse[registro].Stg + " " +
                        gndse[registro].Scn + " " +
                        gndse[registro].Lblk + " ";

                    for (var y = 0; y < saltoRegistro; y++)
                    {
                        registroText += gndse[registro + y].strPmGnd5PU.PadLeft(12, ' ') + " ";
                        //registroText += gndse[registro + y].strPmGnd5PU;
                    }

                    await dat.WriteLineAsync(registroText);
                    //registro += saltoRegistro + 3;
                    registro = registro + saltoRegistro - 1;//20190318 - NET: Corrección
                }

            }


            return terminado = true;
        }

    }
}
