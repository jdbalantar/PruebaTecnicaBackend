namespace Application.DTOs.Persistence
{
    public class QueryPaginationResult<T>(int currentPage, int pages, int count, IQueryable<T> data)
    {
        public int PaginaActual { get; internal set; } = currentPage;
        public int Paginas { get; internal set; } = pages;
        public int CantidadRegistros { get; internal set; } = count;
        public IQueryable<T> Data { get; internal set; } = data;
    }
}
