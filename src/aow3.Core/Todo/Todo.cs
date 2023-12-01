using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace aow3.Todo
{
    [Table("Todos", Schema = "todo")]
    public class TodoItem : FullAuditedEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
