using Abp.Application.Services;
using aow3.Todo.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aow3.Todo
{
    public interface ITodoAppService : IApplicationService
    {
        Task<List<TodoItemDto>> GetListAsync();
        Task<TodoItemDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);
    }
}
