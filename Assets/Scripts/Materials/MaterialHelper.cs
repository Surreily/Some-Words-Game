using System.Linq;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public static class MaterialHelper {
        public static StaticMaterial SetUpStaticMaterial(Sprite sprite) {
            Texture2D texture = SpriteHelper.CreateTextureFromSprite(sprite);

            return new StaticMaterial(texture);
        }

        public static AnimatedMaterial SetUpAnimatedMaterial(Sprite[] sprites, GlobalTimer timer) {
            Texture2D[] textures = sprites
                .Select(s => SpriteHelper.CreateTextureFromSprite(s))
                .ToArray();

            AnimatedMaterial animatedMaterial = new AnimatedMaterial(textures);
            animatedMaterial.RegisterTimer(timer);

            return animatedMaterial;
        }
    }
}
