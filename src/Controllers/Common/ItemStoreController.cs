using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ItemStoreController : Controller {

    private readonly DBContext ctx;
    private StoreService storeService;
    private ItemService itemService;

    public ItemStoreController(DBContext ctx, StoreService storeService, ItemService itemService) {
        this.ctx = ctx;
        this.storeService = storeService;
        this.itemService = itemService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetStore")]
    public IActionResult GetStore([FromForm] string getStoreRequest) {
        GetStoreRequest request = XmlUtil.DeserializeXml<GetStoreRequest>(getStoreRequest);

        ItemsInStoreData[] stores = new ItemsInStoreData[request.StoreIDs.Length];
        for (int i = 0; i < request.StoreIDs.Length; i++) {
            stores[i] = storeService.GetStore(request.StoreIDs[i]);
        }

        GetStoreResponse response = new GetStoreResponse {
            Stores = stores
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetItem")]
    public IActionResult GetItem([FromForm] int itemId) {
        if (itemId == 0) // For a null item, return an empty item
            return Ok(new ItemData());
        return Ok(itemService.GetItem(itemId));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetItemsInStore")] // used by World Of Jumpstart
    public IActionResult GetItemsInStore([FromForm] int storeId) {
        return Ok(storeService.GetStore(storeId));
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetRankAttributeData")]
    public IActionResult GetRankAttributeData() {
        // TODO
        return Ok(XmlUtil.ReadResourceXmlString("rankattrib"));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetAnnouncementsByUser")]
    //[VikingSession(UseLock=false)]
    public IActionResult GetAnnouncements([FromForm] string apiKey, [FromForm] int worldObjectID) {
        
        if(apiKey == "873E8F68-FCE1-44EB-96A2-2EFD62DF3AF2" && worldObjectID == 6) // send static announcement to SuperSecret when burger guy is clicked
        {
            return Ok(new AnnouncementList
            {
                Announcements = new[]
                {
                    new Announcement
                    {
                        AnnouncementID = 1,
                        AnnouncementText = "[[Message]]=[[SanctuaryBabySS]]" +
                        "[[SceneName]]=[[SanctuaryBabySS]]",
                        Description = "Go To Enchanted Sanctuary?",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(9999),
                        Type = AnnouncementType.GeneralText
                    }
                }
            });
        }

        if(!System.IO.File.Exists("announcements.xml")) return Ok(new AnnouncementList());

        return Ok(XmlUtil.DeserializeXml<AnnouncementList>(System.IO.File.ReadAllText("announcements.xml")));
    }
}
