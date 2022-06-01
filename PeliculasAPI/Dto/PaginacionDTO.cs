namespace PeliculasAPI.Dto
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;

        private int cantidadRegistrosPorPagina = 10;

        private readonly int cantidadMaximaRegistroPorPagina = 50;

        public int CantidadRegistrosPorPagina
        {
            get => cantidadMaximaRegistroPorPagina;
            set
            {
                // operador ternario

                cantidadRegistrosPorPagina = (value < cantidadMaximaRegistroPorPagina) ? cantidadMaximaRegistroPorPagina : value;

            }
        }
    }
}
