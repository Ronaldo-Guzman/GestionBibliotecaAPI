using GestionBibliotecaAPI.Edpoints;

namespace GestionBibliotecaAPI.Endpoints
{
    public static class Startup
    {
        public static void useEndpoints (this WebApplication app)
        {
            AutorEndpoints.Add (app);
            EstadoPrestamoEndpoints.Add (app);
            LibroEndpoints.Add (app);
            PrestamoEdpoints.Add (app);
            UsuarioEndpoints.Add (app);
        }
    }
}
