namespace Simulator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += CustomExceptionHandler.OnThreadException;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new HostForm());
        }

        private static class CustomExceptionHandler
        {
            // обработчик исключения
            public static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs t)
            {
                var mess = System.String.Format("{0}", t.Exception.Message.Replace("\r\n", " "));
                //MessageBox.Show(mess, "Фатальная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}