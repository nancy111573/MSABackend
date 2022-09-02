using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myMSABackend.Model;
using FluentValidation;

namespace myMSABackendTest
{
    public class PokemonValidator : AbstractValidator<Pokemon>
    {
        public PokemonValidator()
        {
            RuleFor(pokemon => pokemon.Name).NotNull().WithMessage("Pokemon must have a name");

            RuleFor(pokemon => pokemon.Power).NotNull().WithMessage("Pokemon must have a value for power");
            RuleFor(pokemon => pokemon.Power).GreaterThan(36).WithMessage("Pokemon power should not be lower than 3");
            RuleFor(pokemon => pokemon.Power).LessThan(635).WithMessage("Pokemon power should not be higher than 635");
        }
    }
}
