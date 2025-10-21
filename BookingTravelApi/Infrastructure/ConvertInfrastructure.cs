using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BookingTravelApi.Infrastructure
{
    public class ConvertInfrastructure
    {
        static public List<int> ParseToListInt(String? str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return [];
            }

            return str.Split(",").Select(i => int.Parse(i)).ToList();
        }
    }
}