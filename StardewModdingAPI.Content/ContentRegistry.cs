﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using StardewModdingAPI.Content.Utilities;

namespace StardewModdingAPI.Content
{
    class ContentRegistry
    {
        private IMod Mod;
        public delegate T AssetLoader<T>(string assetName, Func<string, T> loadBase);
        public delegate void AssetInjector<T>(string assetName, ref T asset);
        public ContentRegistry(IMod mod)
        {
            Mod = mod;
        }
        /// <summary>
        /// Enables you to add a custom <see cref="IContentHandler"/> that the content manager will process for content
        /// </summary>
        /// <param name="handler">Your content handler implementation</param>
        public void AddContentHandler(IContentHandler handler)
        {
            ExtendibleContentManager.AddContentHandler(handler);
        }
        /// <summary>
        /// Lets you replace a region of pixels in one texture with the contents of another texture
        /// </summary>
        /// <param name="asset">The texture that you wish to modify</param>
        /// <param name="patch">The texture used for the modification</param>
        /// <param name="region">The area you wish to replace</param>
        public void RegisterTexturePatch(string asset, string patch, Rectangle region)
        {
                if (!Plugins.TextureLoader.AssetMap.ContainsKey(asset))
                    Plugins.TextureLoader.AssetMap.Add(asset, new List<TextureData>());
                Plugins.TextureLoader.AssetMap[asset].Add(new TextureData(patch, region));
            if (Plugins.TextureLoader.AssetCache.ContainsKey(asset))
                Plugins.TextureLoader.AssetCache.Remove(asset);
        }
        /// <summary>
        /// Lets you define a xnb file to completely replace with another
        /// This will only work if none of the more specific loaders deal with the file first
        /// </summary>
        /// <param name="asset">The asset (Relative to Content and without extension) to replace</param>
        /// <param name="replacement">The asset (Relative to Mods and without extension) to use instead</param>
        public void RegisterXnbLoader(string asset, string replacement)
        {
            Plugins.XnbLoader.AssetMap.Add(asset, replacement);
        }
        /// <summary>
        /// If none of the build in content handlers are sufficient, and making a custom one is overkill, this method lets you handle the loading for one specific asset
        /// </summary>
        /// <typeparam name="T">The Type the asset is loaded as</typeparam>
        /// <param name="asset">The asset (Relative to Content and without extension) to handle</param>
        /// <param name="loader">The delegate assigned to handle loading for this asset</param>
        public void RegisterAssetLoader<T>(string asset, AssetLoader<T> loader)
        {
            Plugins.DelegatedContentHandler.AssetLoadMap.Add(asset, loader);
        }
        /// <summary>
        /// If none of the build in content handlers are sufficient, and making a custom one is overkill, this method lets you handle the loading for a specific type of asset
        /// </summary>
        /// <typeparam name="T">The Type the asset is loaded as</typeparam>
        /// <param name="loader">The delegate assigned to handle loading for this type</param>
        public void RegisterTypeLoader<T>(AssetLoader<T> loader)
        {
            Plugins.DelegatedContentHandler.TypeLoadMap.Add(typeof(T), loader);
        }
        /// <summary>
        /// If none of the build in content handlers are sufficient, and making a custom one is overkill, this method lets you handle the injection for one specific asset
        /// </summary>
        /// <typeparam name="T">The Type the asset is loaded as</typeparam>
        /// <param name="asset">The asset (Relative to Content and without extension) to handle</param>
        /// <param name="injector">The delegate assigned to handle injection for this asset</param>
        public void RegisterAssetInjector<T>(string asset, AssetInjector<T> injector)
        {
            if (!Plugins.DelegatedContentHandler.AssetInjectMap.ContainsKey(asset))
                Plugins.DelegatedContentHandler.AssetInjectMap.Add(asset, new List<Delegate>());
            Plugins.DelegatedContentHandler.AssetInjectMap[asset].Add(injector);
        }
        /// <summary>
        /// If none of the build in content handlers are sufficient, and making a custom one is overkill, this method lets you handle the injection for a specific type of asset
        /// </summary>
        /// <typeparam name="T">The Type the asset is loaded as</typeparam>
        /// <param name="injector">The delegate assigned to handle loading for this type</param>
        public void RegisterAssetInjector<T>(AssetInjector<T> injector)
        {
            if (!Plugins.DelegatedContentHandler.TypeInjectMap.ContainsKey(typeof(T)))
                Plugins.DelegatedContentHandler.TypeInjectMap.Add(typeof(T), new List<Delegate>());
            Plugins.DelegatedContentHandler.TypeInjectMap[typeof(T)].Add(injector);
        }
    }
}
