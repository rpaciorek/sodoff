﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

[ApiController]
public class AuthenticationController : Controller {

    private readonly DBContext ctx;

    public AuthenticationController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/GetRules")]
    [EncryptResponse]
    public IActionResult GetRules() {
        GetProductRulesResponse response = new GetProductRulesResponse {
            GlobalSecretKey = "11A0CC5A-C4DF-4A0E-931C-09A44C9966AE"
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/LoginParent")]
    [DecryptRequest("parentLoginData")]
    [EncryptResponse]
    public IActionResult LoginParent([FromForm] string apiKey) {
        ParentLoginData data = XmlUtil.DeserializeXml<ParentLoginData>(Request.Form["parentLoginData"]);

        // Authenticate the user
        User? user = null;
        if (apiKey == "1552008f-4a95-46f5-80e2-58574da65875" || apiKey == "6738196d-2a2c-4ef8-9b6e-1252c6ec7325") { // World Of JumpStart, Math Blaster
            user = ctx.Users.FirstOrDefault(e => e.Email == data.UserName);
        } else {
            user = ctx.Users.FirstOrDefault(e => e.Username == data.UserName);
        }

        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, data.Password) != PasswordVerificationResult.Success) {
            return Ok(new ParentLoginInfo { Status = MembershipUserStatus.InvalidPassword });
        }

        // check if session already exists for user
        Session currentSession = ctx.Sessions.FirstOrDefault(e => e.UserId == user.Id);

        if (currentSession != null)
        {

            if (DateTime.UtcNow >= currentSession.CreatedAt.Value.AddDays(7))
            {
                // session has expired, delete it from the database and refuse login

                ctx.Sessions.Remove(currentSession);
                ctx.SaveChanges();

                return Ok(new ParentLoginInfo { Status = MembershipUserStatus.InvalidApiToken });
            }

            var cL = new List<sodoff.Schema.UserLoginInfo>();
            foreach (var viking in user.Vikings)
            {
                cL.Add(new sodoff.Schema.UserLoginInfo { UserName = viking.Name, UserID = viking.Id });
            }

            var res = new ParentLoginInfo
            {
                UserName = user.Username,
                Email = user.Email,
                ApiToken = currentSession.ApiToken,
                UserID = user.Id,
                Status = MembershipUserStatus.Success,
                SendActivationReminder = false,
                UnAuthorized = false,
                ChildList = cL.ToArray()
            };

            return Ok(res);
        }

        // Create session
        Session session = new Session {
            User = user,
            ApiToken = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        };

        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        var childList = new List<sodoff.Schema.UserLoginInfo>();
        foreach (var viking in user.Vikings) {
            childList.Add(new sodoff.Schema.UserLoginInfo{UserName = viking.Name, UserID = viking.Id});
        }

        var response = new ParentLoginInfo {
            UserName = user.Username,
            Email = user.Email,
            ApiToken = session.ApiToken,
            UserID = user.Id,
            Status = MembershipUserStatus.Success,
            SendActivationReminder = false,
            UnAuthorized = false,
            ChildList = childList.ToArray()
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/AuthenticateUser")]
    [DecryptRequest("username")]
    [DecryptRequest("password")]
    public bool AuthenticateUser() {
        String username = Request.Form["username"];
        String password = Request.Form["password"];

        // Authenticate the user
        User? user = ctx.Users.FirstOrDefault(e => e.Username == username);
        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, password) != PasswordVerificationResult.Success) {
            return false;
        }

        return true;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/GetUserInfoByApiToken")]
    public IActionResult GetUserInfoByApiToken([FromForm] string apiToken, [FromForm] string apiKey) {
        // First check if this is a user session
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is not null) {
            return Ok(new UserInfo {
                UserID = user.Id,
                Username = user.Username,
                MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                FacebookUserID = 0,
                MultiplayerEnabled = (apiKey != "a1a13a0a-7c6e-4e9b-b0f7-22034d799013" && apiKey != "a2a09a0a-7c6e-4e9b-b0f7-22034d799013" && apiKey != "a3a12a0a-7c6e-4e9b-b0f7-22034d799013"),
                IsApproved = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Then check if this is a viking session
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is not null)
        {
            return Ok(new UserInfo {
                UserID = viking.Id,
                Username = viking.Name,
                FacebookUserID = 0,
                MultiplayerEnabled = (apiKey != "a1a13a0a-7c6e-4e9b-b0f7-22034d799013" && apiKey != "a2a09a0a-7c6e-4e9b-b0f7-22034d799013" && apiKey != "a3a12a0a-7c6e-4e9b-b0f7-22034d799013"),
                IsApproved = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Otherwise, this is a bad session, return empty UserInfo
        return Ok(new UserInfo {});
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/IsValidApiToken_V2")]
    public IActionResult IsValidApiToken([FromForm] string? apiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null)
            return Ok(ApiTokenStatus.TokenNotFound);
        return Ok(ApiTokenStatus.TokenValid);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/IsValidApiToken")] // used by World Of Jumpstart (FutureLand)
    public IActionResult IsValidApiToken_V1([FromForm] string? apiToken)
    {
        if (apiToken is null)
            return Ok(false);
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null)
            return Ok(false);
        return Ok(true);
    }

    // This is more of a "create session for viking", rather than "login child"
    [Route("AuthenticationWebService.asmx/LoginChild")]
    [DecryptRequest("childUserID")]
    [EncryptResponse]
    public IActionResult LoginChild([FromForm] string parentApiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null) {
            return Unauthorized();
        }

        // Find the viking
        string? childUserID = Request.Form["childUserID"];
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == childUserID);
        if (viking is null) {
            return Unauthorized();
        }

        // Check if user is viking parent
        if (user != viking.User) {
            return Unauthorized();
        }

        // check if a session for child already exists
        Session existingSession = ctx.Sessions.FirstOrDefault(e => e.VikingId == viking.Id);

        if (existingSession != null)
        {
            if(DateTime.UtcNow >= existingSession.CreatedAt.Value.AddDays(7))
            {
                // delete session to keep database clean
                ctx.Sessions.Remove(existingSession);
            }
        }

        // Create session
        Session session = new Session {
            Viking = viking,
            ApiToken = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        };
        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        // Return back the api token
        return Ok(session.ApiToken);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ScoreWebService.asmx/SetScore")]
    public IActionResult SetScore()
    {
        // TODO - placeholder
        return Ok(true);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/DeleteAccountNotification")]
    public IActionResult DeleteAccountNotification([FromForm] string apiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null)
            return Ok(MembershipUserStatus.ValidationError);

        ctx.Users.Remove(user);
        ctx.SaveChanges();

        return Ok(MembershipUserStatus.Success);
    }
}
