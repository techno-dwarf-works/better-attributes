using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Preview;
using Better.Attributes.Runtime.Preview;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Comparers;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Utility
{
    public class __PreviewUtility : HandlerMap<__PreviewUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new WrappersTypeCollection()
            {
                {
                    typeof(PreviewAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(Sprite), typeof(SpriteHandler) },
                        { typeof(Texture2D), typeof(TextureHandler) },
                        { typeof(Component), typeof(AssetHandler) }
                    }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AssignableFromComparer.Instance)
            {
                typeof(Sprite),
                typeof(Texture),
                typeof(Component)
            };
        }
    }
}