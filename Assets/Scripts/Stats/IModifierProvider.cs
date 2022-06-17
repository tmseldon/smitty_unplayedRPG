using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveMod(Status stat);
        IEnumerable<float> GetMultiplyMod(Status stat);
    }
}
