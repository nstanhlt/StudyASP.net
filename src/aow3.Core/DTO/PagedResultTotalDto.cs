using Abp.Application.Services.Dto;
using Aow.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAWASCO.ERP.DTO
{
    public class PagedResultTotalDto<T> : ListResultDto<T>, IListResult<T>
    {

        public PagedResultTotalDto() : base()
        {

        }

        public PagedResultTotalDto(int totalCount, IReadOnlyList<T> items)
        {
            base.Items = items;
            this.Count = totalCount;
        }

        public int Count { get; set; }
    }
}
