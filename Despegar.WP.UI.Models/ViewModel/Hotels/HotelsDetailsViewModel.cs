using Despegar.Core.Business.Hotels.HotelDetails;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsDetailsViewModel : ViewModelBase
    {
        public INavigator navigator { get; set; }
        public IHotelService hotelService { get; set; }
        public HotelsCrossParameters CrossParameters { get; set; }
        public HotelDatails HotelDetail { get; set; }
        public ICollection<string> ImagesTestList { get; set; }

        public HotelsDetailsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t): base(t)
        {
            this.navigator = navigator;
            this.hotelService = hotelService;

            ImagesTestList = GetPictureList();
        }

        private List<string> GetPictureList()
        {
            List<string> picList = new List<string>()
                {
                    "73f8723c-bbe5-4889-9089-004578c77ade",
                    "b4f5c320-0f80-40df-b725-7de4c4892dea",
                    "645e77b2-0eda-455d-9598-6ad281bd547e",
                    "a06bcb6d-3991-4a69-b9f2-7ababceb659a",
                    "b260ce79-8e5a-42e0-9c9c-4d2a7901ddb8",
                    "3574ee67-738c-4808-bdd7-f26502de9c4e",
                    "d189c6b3-935b-4c7e-983c-9988be4751e0",
                    "ebd8c8ef-fa45-42f8-9c0e-58e34a198395",
                    "1689b8d9-bebd-44be-9125-177ae217d94d",
                    "f660c51a-865e-4901-a91b-51b9531490e4",
                    "ea0310f3-8e94-4958-9a99-25be2d06c329",
                    "99065d62-532f-4888-ac42-4d90507865bf",
                    "d2895f85-0fff-4cac-862b-1dc226e80eda",
                    "0c28bab6-3f50-46b0-a116-7199be27f856",
                    "7f8c7753-7c8f-4f5b-8c21-8f77d9256662",
                    "8c0318d0-af39-4dab-af5c-269a34981f36",
                    "6b6389c8-fd7f-474c-847b-6176f5fc894c",
                    "bd3bcfac-1266-44ce-bdbb-4d71831429c2",
                    "0a429676-34ad-4585-ae38-3c5a3d2f301a",
                    "16382366-1142-4ffd-9b3f-55b3fc71f363",
                    "fcfe873b-ea95-4fa8-adb4-6973a3d8d31b",
                    "f26a5706-e8a6-4579-b4bd-f64a5d76c4b5",
                    "5591824d-e2cd-45a6-a4b4-3373bcdc4d8a",
                    "659e2613-97d4-41b4-b246-3fa054981c51",
                    "4ba1f7c1-173c-4465-bc26-2df912cd44fb",
                    "8df6ef63-c628-49e5-975f-34a00842f733",
                    "49c0fec9-68f3-4211-9f71-23969f484375",
                    "719fca4a-7957-4bc3-a87c-0c094c6c4a99"
                };

            return picList;
        }

    }
}
