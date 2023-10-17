// namespace GistBlog.BLL.Services.Implementation.PaginationSortingAndFiltering;
//
// public class SortingService<TEntity>
// {
//     public IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortOrder)
//     {
//         switch (sortOrder)
//         {
//             case "title_asc":
//                 return query.OrderBy((TEntity x) => x.Title);
//             case "title_desc":
//                 return query.OrderByDescending((TEntity x) => x.Title);
//             case "date_asc":
//                 return query.OrderBy((TEntity x) => x.DateCreated);
//             case "date_desc":
//                 return query.OrderByDescending((TEntity x) => x.DateCreated);
//             default:
//                 return query.OrderByDescending((TEntity x) => x.DateCreated); // Default sorting
//         }
//     }
// }

