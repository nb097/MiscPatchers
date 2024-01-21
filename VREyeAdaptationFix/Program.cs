using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace VREyeAdaptationFix
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline
                .Instance.AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "VREyeAdaptationFix.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (
                var imagespace in state.LoadOrder.PriorityOrder.ImageSpace().WinningOverrides()
            )
            {
                if (imagespace.Cinematic == null || imagespace.Cinematic.Contrast == 1f)
                    continue;

                var moddedImagespace = imagespace.DeepCopy();

                if (moddedImagespace.Cinematic == null || moddedImagespace.Cinematic.Contrast == 1f)
                    continue;

                moddedImagespace.Cinematic.Contrast = 1f;
                state.PatchMod.ImageSpaces.Set(moddedImagespace);
                Console.WriteLine($"Patched {moddedImagespace.EditorID}");
            }
        }
    }
}
