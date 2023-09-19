// using GistBlog.DAL.Enums;
// using Microsoft.Extensions.Logging;
//
// namespace GistBlog.BLL.Services.Implementation.PaginationSortingAndFiltering;
//
// public class SearchService<TEntity>
// {
//     private readonly ILogger<SearchService<TEntity>> _logger;
//
//     public SearchService(ILogger<SearchService<TEntity>> logger)
//     {
//         _logger = logger;
//     }
//
//     public IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string searchCategory)
//     {
//         if (!string.IsNullOrEmpty(searchCategory))
//         {
//             _logger.LogInformation($"SearchCategory input: {searchCategory}");
//
//             searchCategory = searchCategory.ToLower();
//             if (Enum.TryParse<Category>(searchCategory, true, out var parsedCategory))
//             {
//                 _logger.LogInformation($"Parsed Category: {parsedCategory}");
//
//                 query = query.Where(x => x.Category == parsedCategory);
//             }
//             else
//             {
//                 _logger.LogError($"Invalid searchCategory: {searchCategory}");
//             }
//         }
//
//         return query;
//     }
// }

