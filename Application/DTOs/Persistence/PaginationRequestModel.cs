namespace Application.DTOs.Persistence
{
    public class PaginationRequestModel
    {
        public int Pagina { get; set; }
        public float Limite { get; set; }
        public required FiltroModel[] Filtros { get; set; }
        public string? CampoOrdenar { get; set; }
        public string? Orden { get; set; }
    }
}
