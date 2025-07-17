using COES.Service.Tawa.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace COES.Service.Tawa
{
    public partial class ServicioTawa : ServiceBase
    {
        EventLogger logger = new EventLogger();
        public ServicioTawa()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicio del servicio
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            ValidateDate();
        }

        private void ValidateDate()
        {
            DateTime today = DateTime.Now;
            logger.Info("Validación de tercer día hábil " + today + " !");
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            int businessDayCount = 0;
            DateTime currentDate = firstDayOfMonth;

            // Recorremos todos los días del mes hasta llegar al tercer día hábil
            while (currentDate.Month == today.Month)
            {
                if (IsBusinessDay(currentDate)) // Verifica si es un día hábil
                {
                    businessDayCount++;
                }

                // Si hemos llegado al tercer día hábil, ejecutamos la tarea
                if (businessDayCount == 3)
                {
                    if (currentDate.Date == today.Date)
                    {
                        EjecutarTarea();
                    }
                    break;
                }

                // Avanzamos al siguiente día
                currentDate = currentDate.AddDays(1);
            }
        }

        /// <summary>
        /// Se ejecuta cada vez que cumple los 30 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EjecutarTarea()
        {
            //EventLogger logger = new EventLogger();
            try
            {
                ClienteHttp cliente = new ClienteHttp();
                int resultado = await cliente.EjecutarServicioTawa();

                if (resultado == 1)
                {
                    logger.Info("Proceso ejecutado correctamente!");
                }
                else if(resultado == 2){
                    logger.Info("Proceso no fue ejecutado correctamente, no existen archivos en SFTP! " + resultado);
                }
                else
                {
                    logger.Info("Proceso no fue ejecutado correctamente! " + resultado);
                }           

            }
            catch (Exception ex)
            {
                logger.Error("Error", ex);
            }
        }

        /// <summary>
        /// Acción a realizar al finalizar
        /// </summary>
        protected override void OnStop()
        {
        }

        private bool IsBusinessDay(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
