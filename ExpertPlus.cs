using Terraria.ModLoader;

namespace ExpertPlus
{
    public class ExpertPlus : Mod
    {
	public static ExpertPlus Instance;
        public override void Load()
        {
            base.Load();
        }

        public override void Unload()
        {
		Logger.Info("Unloading Expert Plus");
        	base.Unload();
        }
    }
}
