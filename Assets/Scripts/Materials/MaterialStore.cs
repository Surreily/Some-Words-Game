namespace Surreily.SomeWords.Scripts.Materials {
    public class MaterialStore {
        public MaterialStore(GlobalTimer timer) {
            Ui = new UiMaterialStore(timer);
            Font = new FontMaterialStore(timer);
            Map = new MapMaterialStore(timer);
            Level = new LevelMaterialStore(timer);
        }

        public UiMaterialStore Ui { get; }
        public FontMaterialStore Font { get; }
        public MapMaterialStore Map { get; }
        public LevelMaterialStore Level { get; }
    }
}