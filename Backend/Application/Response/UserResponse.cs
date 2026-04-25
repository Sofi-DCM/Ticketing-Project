using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class UserResponse
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
