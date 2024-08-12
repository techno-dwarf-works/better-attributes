using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Preview;
using Better.Attributes.Runtime.Preview;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(PreviewHandler))]
    public class PreviewTypeHandlerBinder : TypeHandlerBinder<PreviewHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new HandlersTypeCollection()
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