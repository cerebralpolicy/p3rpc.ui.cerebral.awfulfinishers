using p3rpc.ui.cerebral.awfulfinishers.Configuration;
using p3rpc.ui.cerebral.awfulfinishers.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using p3rpc.ui.cerebral.awfulfinishers.Types;

namespace p3rpc.ui.cerebral.awfulfinishers
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private readonly IUnrealEssentials UnrealEssentials;
        private readonly IUnreal Unreal;

        private bool UseKotone;
        const string NAME = "Awful AOAs";
        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            Log.LogLevel = _configuration.LogLevel;

            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic

            this._modLoader.GetController<IUnrealEssentials>().TryGetTarget(out var unrealEssentials);
            if (unrealEssentials == null)
            {
                throw new ArgumentNullException(nameof(unrealEssentials));
            }
            UnrealEssentials = unrealEssentials;
            this._modLoader.GetController<IUnreal>().TryGetTarget(out var unreal);
            if (unreal == null)
            {
                throw new ArgumentNullException(nameof(unreal));
            }
            Unreal = unreal;

            // SET MOD DIRECTORY
            var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            
            // LOAD BASE
            var partyMemberPath = Path.Combine(modDir, "Common");

            UnrealEssentials.AddFromFolder(partyMemberPath);

            // PLEASE LET THIS WORK

            _modLoader.ModLoaded += OnModLoaded;

            _modLoader.OnModLoaderInitialized += OnAllModsLoaded;
        }

        private void OnAllModsLoaded()
        {
            for (int i = 0; i < 13;  i++)
            {
                var chara = (Character)i;
                var shouldRedirect = _configuration.ReadConfigState(chara);

                if (shouldRedirect)
                {
                    AssignRedirect(i);
                }
            }
        }

       

        private void AssignRedirect(int character)
        {

            if (character == 0)
                return;
            else if (character != 1 && character != 11) 
            { 
                var vanillaAsset = GetBaseAOA(character);
                var awfulAsset = GetAwfulAOA(character);
                Unreal.AssignFName(NAME, vanillaAsset, awfulAsset);
            }
            else if (character == 11)
            {
                var vanillaAsset = GetBaseAOA(character);
                var awfulAsset = GetAwfulAOA(character,_configuration.FrenchMetis);
                Unreal.AssignFName(NAME, vanillaAsset, awfulAsset);
            }
            else
            {
                var vanillaAsset = GetBaseAOA(character);
                var awfulAsset = GetAwfulAOA(character,UseKotone);
                Unreal.AssignFName(NAME, vanillaAsset, awfulAsset);
            }
        } 

        private void OnModLoaded(IModV1 mod, IModConfigV1 config)
        {
            if (config.ModId == "p3rpc.femc")
            {
                UseKotone = true;
            }
        }


        public static string GetBaseAOA(int character)
        {
            bool IsAnswerCharacter = character > 10;
            if (IsAnswerCharacter)
                return GetAssetPath($"/Game/Astrea/Battle/Allout/Materials/Finish2D/T_Btl_AlloutFinishText_Pc{character:00}");
            return GetAssetPath($"/Game/Xrd777/Battle/Allout/Materials/Finish2D/T_Btl_AlloutFinishText_Pc{character:00}");
        }
        public static string GetAwfulAOA(int character, bool useAltFolder = false)
        {
            var fileName = "T_Btl_AlloutFinishText_Pc" + character.ToString("00");
            var aoaFolder = useAltFolder ? "Allout/Alt" : "Allout";
            return GetAssetPath($"/Game/Cerebral/UI/{aoaFolder}/{fileName}");
        }

        public static string GetAssetPath(string assetFile)
        {
            var adjustedPath = assetFile.Replace('\\', '/').Replace(".uasset", string.Empty);

            if (adjustedPath.IndexOf("Content") is int contentIndex && contentIndex > -1)
            {
                adjustedPath = adjustedPath.Substring(contentIndex + 8);
            }

            if (!adjustedPath.StartsWith("/Game/"))
            {
                adjustedPath = $"/Game/{adjustedPath}";
            }
            return adjustedPath;
        }
        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}