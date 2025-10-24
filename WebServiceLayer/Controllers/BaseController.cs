using DataServiceLayer;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers
{
    public class BaseController<TDataService> : ControllerBase where TDataService : BaseDataService
    {
        protected readonly TDataService _dataService;
        protected readonly LinkGenerator _generator;
        protected readonly IMapper _mapper;

        public BaseController(
            TDataService dataService,
            LinkGenerator generator,
            IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        protected object CreatePaging<T>(string endpointName, IEnumerable<T> items, int numOfItems, QueryParams queryParams)
        {
            var numOfPages = (int)Math.Ceiling((double)numOfItems / queryParams.PageSize);
            var prev = queryParams.Page > 0
            ? GetUrl(endpointName, new { page = queryParams.Page - 1, queryParams.PageSize })
            : null;

            var next = queryParams.Page < numOfPages - 1
            ? GetUrl(endpointName, new { page = queryParams.Page + 1, queryParams.PageSize })
            : null;

            var first = GetUrl(endpointName, new { page = 0, queryParams.PageSize });
            var cur = GetUrl(endpointName, new { queryParams.Page, queryParams.PageSize });
            var last = GetUrl(endpointName, new { page = numOfPages - 1, queryParams.PageSize });

            return new
            {
                First = first,
                Prev = prev,
                Next = next,
                Last = last,
                Current = cur,
                NumberOfPages = numOfPages,
                NumberOfIems = numOfItems,
                Items = items
            };
        }

        protected string? GetUrl(string endpointName, object values)
        {
            return _generator.GetUriByName(HttpContext, endpointName, values);
        }
    }
    
}
