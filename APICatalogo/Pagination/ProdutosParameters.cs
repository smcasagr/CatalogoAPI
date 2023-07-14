namespace APICatalogo.Pagination
{
 
    // Relaciona o número da página e o tamanho dela
    public class ProdutosParameters
    {
        const int maxPageSize = 50; // Máximo de registros retornados
        public int PageNumber { get; set; } = 1;
        public int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
