using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using sodoff.Configuration;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services
{
    public class BuddyService
    {
        private readonly DBContext ctx;
        private readonly IOptions<ApiServerConfig> config;
        private readonly MessageService messageService;
        private char[] BuddyCodeCharList = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
        };
        public BuddyService(DBContext ctx, IOptions<ApiServerConfig> config, MessageService messageService)
        {
            this.ctx = ctx;
            this.config = config;
            this.messageService = messageService;
        }

        public BuddyList GetBuddyList(Viking viking)
        {
            List<Model.Buddy> buddies = ctx.Buddies.Where(e => e.OwnerID == viking.Uid).ToList();
            List<Schema.Buddy> buddiesRes = new List<Schema.Buddy>();

            if (buddies is null) return new BuddyList();

            foreach (var buddy in buddies)
            {
                AvatarData? avatar = XmlUtil.DeserializeXml<AvatarData>(ctx.Vikings.FirstOrDefault(e => e.Uid == buddy.BuddyID)?.AvatarSerialized);
                buddiesRes.Add(new Schema.Buddy
                {
                    BestBuddy = buddy.BestBuddy,
                    CreateDate = DateTime.Now,
                    UserID = buddy.BuddyID.ToString(),
                    Status = buddy.Status,
                    DisplayName = avatar.DisplayName,
                    OnMobile = false,
                    Online = ctx.Vikings.FirstOrDefault(e => e.Uid == buddy.BuddyID)?.Online ?? false,
                });
            }

            return new BuddyList { Buddy = buddiesRes.ToArray() };
        }

        public BuddyActionResult AddBuddy(Viking owner, Viking receiver, uint gameVersion)
        {
            // create two relations
            Model.Buddy relation = new Model.Buddy { Id = Guid.NewGuid().ToString(), OwnerID = owner.Uid, BuddyID = receiver.Uid, Status = BuddyStatus.PendingApprovalFromOther };
            Model.Buddy relation2 = new Model.Buddy { Id = Guid.NewGuid().ToString(), OwnerID = receiver.Uid, BuddyID = owner.Uid, Status = BuddyStatus.PendingApprovalFromSelf };

            // prevent receiver from receiving request if versions don't match
            if (receiver.GameVersion != owner.GameVersion) return new BuddyActionResult { Result = BuddyActionResultType.InvalidFriendCode };


            if (receiver == owner) return new BuddyActionResult { Result = BuddyActionResultType.CannotAddSelf };
            if (ctx.Buddies.Contains(relation) || ctx.Buddies.Contains(relation2)) return new BuddyActionResult { Result = BuddyActionResultType.AlreadyInList }; // DO NOT ADD IF ALREADY ADDED

            ctx.Buddies.Add(relation);
            ctx.Buddies.Add(relation2);
            ctx.SaveChanges();

            // post message for adding buddy

            messageService.PostDataMessage(owner, receiver, null, MessageType.Post, MessageLevel.WhiteList, MessageTypeID.BuddyList,
                "[[Line1]]=[[{{BuddyUserName}} wants to be added to your buddy list. Is that okay?]]",
                "[[Line1]]=[[{{BuddyUserName}} wants to be added to your buddy list. Is that okay?]]");

            return new BuddyActionResult
            {
                BuddyUserID = receiver.Uid.ToString(),
                Result = BuddyActionResultType.Success,
                Status = relation.Status
            };
        }

        public bool RemoveBuddy(Viking owner, Viking receiver)
        {
            // get buddy relation 1
            Model.Buddy relation = ctx.Buddies.Where(e => e.OwnerID == owner.Uid)
                .FirstOrDefault(e => e.BuddyID == receiver.Uid);

            // remove it
            ctx.Buddies.Remove(relation);

            // get buddy relation 2

            Model.Buddy relation2 = ctx.Buddies.Where(e => e.OwnerID == receiver.Uid)
                .FirstOrDefault(e => e.BuddyID == owner.Uid);

            // remove it
            ctx.Buddies.Remove(relation2);

            ctx.SaveChanges();

            return true;
        }

        public bool AcceptBuddyRequest(Viking owner, Viking receiver)
        {
            // get buddy relation 1
            Model.Buddy relation = ctx.Buddies.Where(e => e.OwnerID == owner.Uid)
                .FirstOrDefault(e => e.BuddyID == receiver.Uid);

            // change status to approved
            relation.Status = BuddyStatus.Approved;

            // get buddy relation 2

            Model.Buddy relation2 = ctx.Buddies.Where(e => e.OwnerID == receiver.Uid)
                .FirstOrDefault(e => e.BuddyID == owner.Uid);

            // change status to approved
            relation2.Status = BuddyStatus.Approved;

            ctx.SaveChanges();

            return true;
        }

        public BuddyActionResult SetBuddyAsBest(Viking viking, Viking vikingToSet, bool best)
        {
            Model.Buddy buddyToSet = ctx.Buddies.Where(e => e.OwnerID == viking.Uid)
                .FirstOrDefault(e => e.BuddyID == vikingToSet.Uid);

            if (buddyToSet == null) return new BuddyActionResult { Result = BuddyActionResultType.Unknown };

            buddyToSet.BestBuddy = best;
            ctx.SaveChanges();

            return new BuddyActionResult { Result = BuddyActionResultType.Success };
        }

        public BuddyLocation GetBuddyLocation(Viking viking)
        {
            Model.Buddy buddy = ctx.Buddies.FirstOrDefault(e => e.BuddyID == viking.Uid);

            if (viking.CurrentRoom != null) return new BuddyLocation
            {
                Server = config.Value.MMOAdress,
                ServerPort = config.Value.MMOPort,
                ServerVersion = "S2X", // always use SmartFox 2X Protocol
                Zone = "JumpStart", // TODO - put this in config
                Room = viking.CurrentRoom,
                UserID = buddy.BuddyID.ToString(),
                MultiplayerID = 1, // TODO - what the fuck is this
                AppName = "JSMain" // TODO - figure out how to detect this
            };
            else return new BuddyLocation();
        }

        public string GetOrSetBuddyCode(Viking viking, string codeOverride = "")
        {
            Random rnd = new Random();

            if (!string.IsNullOrEmpty(codeOverride) && ctx.Vikings.FirstOrDefault(e => e.BuddyCode == codeOverride) == null) viking.BuddyCode = codeOverride;

            if (viking.BuddyCode == null)
            {
                string generatedCode = "";
                for(var i = 0; i < 5; i++)
                {
                    generatedCode = generatedCode + BuddyCodeCharList[rnd.Next(0, BuddyCodeCharList.Length)];
                    if (ctx.Vikings.FirstOrDefault(e => e.BuddyCode == generatedCode) != null) continue;
                }
                viking.BuddyCode = generatedCode;
            }

            ctx.SaveChanges();

            return viking.BuddyCode;
        }
    }
}
