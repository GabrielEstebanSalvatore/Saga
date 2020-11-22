using Aplicacion.IoC;
using StructureMap;
using System;
using System.Windows.Forms;

namespace CommerceApp
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuracion del Inyector (StructureMap)
            new StructureMapContainer().Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ObjectFactory.GetInstance<Principal>());
            //Application.Run(ObjectFactory.GetInstance<InicioSesion>());
        }
    }
}
