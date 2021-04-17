using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mapper.ViewModels
{
    public class MarkerDialogViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Markers { get; } = new ObservableCollection<string>()
        {
            "_001_helmetDrawingImage",
            "_003_coralDrawingImage",
            "_005_oxygen_tankDrawingImage",
            "_006_diverDrawingImage",
            "_004_flippersDrawingImage",
            "_007_cameraDrawingImage",
            "_008_submarineDrawingImage",
            "_009_flashlightDrawingImage",
            "_010_turtleDrawingImage",
            "_011_sharkDrawingImage",
            "_012_boatDrawingImage",
            "_013_reefDrawingImage",
            "_014_fishDrawingImage",
            "_016_puffer_fishDrawingImage",
            "_018_gaugeDrawingImage",
            "_021_harpoonDrawingImage",
            "_022_coralDrawingImage",
            "_023_coralDrawingImage",
            "_024_scuba_divingDrawingImage",
            "_026_jellyfishDrawingImage",
            "_027_clownfishDrawingImage",
            "_028_bagDrawingImage",
            "_029_caveDrawingImage",
            "_030_reefDrawingImage",
            "_031_manta_rayDrawingImage",
            "_032_snorkel_gearDrawingImage",
            "_033_shipwreckDrawingImage",
            "_034_fishDrawingImage",
            "_037_snorkelDrawingImage",
            "_038_face_maskDrawingImage",
            "_040_compassDrawingImage"
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
