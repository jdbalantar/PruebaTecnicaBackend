namespace Application.DTOs.Persistence
{
    public class PaginationResult<T>(int currentPage, int pages, int count, ICollection<T> data)
    {
        public int PaginaActual { get; internal set; } = currentPage;
        public int Paginas { get; internal set; } = pages;
        public int CantidadRegistros { get; internal set; } = count;
        public ICollection<T> Data { get; internal set; } = data;
    }
}
