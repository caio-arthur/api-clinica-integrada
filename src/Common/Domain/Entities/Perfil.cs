using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class Perfil : IdentityRole<Guid>
    {
        public Perfil(string role) : base(role) {

        }
        public Perfil() {

        }
    }
}
