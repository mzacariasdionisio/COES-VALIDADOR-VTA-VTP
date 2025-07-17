using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Areas.Yupana.Models;

namespace COES.MVC.Intranet.Areas.Yupana.Helper
{
    public class FuncionHelper
    {
        public static List<VariableModel> GetListaVariablesOut()
        {
            List<VariableModel> lista = new List<VariableModel>()
            {
                new VariableModel() { IdVariable = "56", NombreVariable = "Potencia de Racionamiento en el Nodo N" , TipoParametro=0 }, //0
                new VariableModel() { IdVariable = "57", NombreVariable = "Potencia de Exceso en el Nodo N" , TipoParametro = 0},       //1
                new VariableModel() { IdVariable = "58", NombreVariable = "Ángulo de la Tensión en el Nodo N" , TipoParametro = 0 },       //2
                new VariableModel() { IdVariable = "59", NombreVariable = "Pérdidas en las Equipos de Transmisión" , TipoParametro = 0},             //3
                new VariableModel() { IdVariable = "60", NombreVariable = "Flujo en Equipos de Transmisión" , TipoParametro = 0},     //4
                new VariableModel() { IdVariable = "61", NombreVariable = "Flujo por las Líneas Sentido 2", TipoParametro = 0 },     //5
                new VariableModel() { IdVariable = "62", NombreVariable = "Potencias Térmicas" , TipoParametro = 0},                     //6
                new VariableModel() { IdVariable = "63", NombreVariable = "Potencias Hidráulicas", TipoParametro = 0 },                  //7
                new VariableModel() { IdVariable = "64", NombreVariable = "Volúmenes de Embalses", TipoParametro = 0 },                   //8
                new VariableModel() { IdVariable = "65", NombreVariable = "Caudal Descargado en los Embalses" , TipoParametro = 0},      //9
                new VariableModel() { IdVariable = "66", NombreVariable = "Vertimiento Plantas Hidráulicas", TipoParametro = 0 },                   //10
                new VariableModel() { IdVariable = "67", NombreVariable = "Vertimiento Embalses", TipoParametro = 0 },       //11
                
                new VariableModel() { IdVariable = "68", NombreVariable = "Pérdidas Ficticias", TipoParametro = 2 },       //12
                new VariableModel() { IdVariable = "69", NombreVariable = "Reserva por URS", TipoParametro = 2 },       //13
                new VariableModel() { IdVariable = "70", NombreVariable = "Reserva Secundaria Hidro", TipoParametro = 2 },       //14
                new VariableModel() { IdVariable = "71", NombreVariable = "Reserva Secundaria Térmica", TipoParametro = 2 },       //15

                new VariableModel() { IdVariable = "106", NombreVariable = "RSF hacia arriba por URS", TipoParametro = 2 },       //23
                new VariableModel() { IdVariable = "107", NombreVariable = "RSF hacia abajo por URS", TipoParametro = 2 },       //24

                new VariableModel() { IdVariable = "102", NombreVariable = "RSF hacia arriba por Unidad hidro", TipoParametro = 0 },       //25
                new VariableModel() { IdVariable = "103", NombreVariable = "RSF hacia abajo por Unidad hidro", TipoParametro = 0 },       //26
                new VariableModel() { IdVariable = "100", NombreVariable = "RSF hacia arriba por Unidad térmica", TipoParametro = 0 },       //27
                new VariableModel() { IdVariable = "101", NombreVariable = "RSF hacia abajo por Unidad térmica", TipoParametro = 0 },       //28 
            };
            return lista;
        }

        public static List<VariableModel> GetListaEcuacionesOutGams()
        {
            List<VariableModel> lista = new List<VariableModel>()
            {
            //Ecuaciones
            new VariableModel() { IdVariable = "73", NombreVariable = "CMg por Barra", TipoParametro = 1 },      //16
                new VariableModel() { IdVariable = "74", NombreVariable = "CMg de Planta", TipoParametro = 1 },                   //17
                new VariableModel() { IdVariable = "75", NombreVariable = "CMg de Embalse", TipoParametro = 1 },       //18
                new VariableModel() { IdVariable = "72", NombreVariable = "CMg Líneas", TipoParametro = 2 },       //19
                new VariableModel() { IdVariable = "98", NombreVariable = "CMg de suma de flujos superior", TipoParametro = 1 },       //20
                new VariableModel() { IdVariable = "99", NombreVariable = "CMg de suma de flujos inferior", TipoParametro = 1 },       //21
                            };
            return lista;
        }

        public static List<VariableModel> GetListaCostosOutGams()
        {
            List<VariableModel> lista = new List<VariableModel>()
            {
                new VariableModel() { IdVariable = "77", NombreVariable = "Costo Arranques Térmicos", TipoParametro = 3 },       //24
                new VariableModel() { IdVariable = "76", NombreVariable = "Costo de Operación Térmico", TipoParametro = 3 },       //23
                new VariableModel() { IdVariable = "78", NombreVariable = "Costo de Operación Hidros", TipoParametro = 3 },       //22
                new VariableModel() { IdVariable = "85", NombreVariable = "Costo Función Costo Futuro", TipoParametro = 2 },
                new VariableModel() { IdVariable = "84", NombreVariable = "Costo Vertimiento de Centrales", TipoParametro = 3 } ,
                new VariableModel() { IdVariable = "83", NombreVariable = "Costo Vertimiento de Embalses", TipoParametro = 3 } ,
                new VariableModel() { IdVariable = "81", NombreVariable = "Costo Racionamientos (Déficit Demanda)", TipoParametro = 3 },
                new VariableModel() { IdVariable = "82", NombreVariable = "Costo de Excesos de Potencia", TipoParametro = 3 } ,
                new VariableModel() { IdVariable = "108", NombreVariable = "Costo de Reserva Secundaria(Up)", TipoParametro = 3 } ,
                new VariableModel() { IdVariable = "109", NombreVariable = "Costo de Reserva Secundaria(Down)", TipoParametro = 3 } ,
                new VariableModel() { IdVariable = "110", NombreVariable = "Costo de Déficit de Reserva Secundaria (Up)", TipoParametro = 3 },
                new VariableModel() { IdVariable = "111", NombreVariable = "Costo de Déficit de Reserva Secundaria (Down)", TipoParametro = 3 },
                new VariableModel() { IdVariable = "112", NombreVariable = "Costo de Déficit por Región de Seguridad", TipoParametro = 3 },       //22
                new VariableModel() { IdVariable = "113", NombreVariable = "Costo de Exceso por Región de Seguridad", TipoParametro = 3 },       //25
                new VariableModel() { IdVariable = "94", NombreVariable = "TOTAL", TipoParametro = 1 }
            };
            return lista;
        }
    }
}