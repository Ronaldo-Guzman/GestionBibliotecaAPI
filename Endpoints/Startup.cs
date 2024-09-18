using GestionBibliotecaAPI.Edpoints;

namespace GestionBibliotecaAPI.Endpoints
{
    public static class Startup
    {
        public static void useEndpoints (this WebApplication app)
        {
            LibroEndpoints.Add (app);
            UsuarioEndpoints.Add (app);
        }
    }
}
